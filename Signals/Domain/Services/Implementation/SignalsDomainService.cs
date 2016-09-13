using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Mapster;
using System.Reflection;

namespace Domain.Services.Implementation
{
    [UnityRegister]
    public class SignalsDomainService : ISignalsDomainService
    {
        private readonly ISignalsRepository signalsRepository;
        private readonly ISignalsDataRepository signalsDataRepository;
        private readonly IMissingValuePolicyRepository missingValuePolicyRepository;

        public SignalsDomainService(
            ISignalsRepository signalsRepository, 
            ISignalsDataRepository signalsDataRepository, 
            IMissingValuePolicyRepository missingValuePolicyRepository)
        {
            this.signalsRepository = signalsRepository;
            this.signalsDataRepository = signalsDataRepository;
            this.missingValuePolicyRepository = missingValuePolicyRepository;
        }

        public Signal Add(Signal newSignal)
        {
            if (newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            var signal = this.signalsRepository.Add(newSignal);
            var policyInstance = typeof(NoneQualityMissingValuePolicy<>)
                .MakeGenericType(new Type[] { DataTypeUtils.GetNativeType(signal.DataType) });

            SetMissingValuePolicy(signal, Activator.CreateInstance(policyInstance) as MissingValuePolicyBase);

            return signal;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path path)
        {
            return signalsRepository.Get(path);
        }

        public void CheckShadowMVP<T>(Signal signal, MissingValuePolicyBase domainMissingValuePolicy)
        {
            if (!(domainMissingValuePolicy is ShadowMissingValuePolicy<T>)) return;

            var mvp = domainMissingValuePolicy as ShadowMissingValuePolicy<T>;

            if (signal.DataType != mvp.ShadowSignal.DataType ||
                signal.Granularity != mvp.ShadowSignal.Granularity)
                throw new IncorrectShadowSignalException();

            if (mvp != null && mvp.ShadowSignal != null)
            {
                var shadowMvp = missingValuePolicyRepository.Get(mvp.ShadowSignal);
                if(shadowMvp is ShadowMissingValuePolicy<T>)
                    throw new IncorrectShadowSignalException();
            }
        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase domainMissingValuePolicy)
        {
            if (signal == null)
            {
                throw new CouldntGetASignalException();
            }
            
            try
            {
                typeof(SignalsDomainService)
                .GetMethod("CheckShadowMVP")
                .MakeGenericMethod(DataTypeUtils.GetNativeType(signal.DataType))
                .Invoke(this, new object[] { signal, domainMissingValuePolicy });
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }
            missingValuePolicyRepository.Set(signal, domainMissingValuePolicy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            if (signal == null)
            {
                throw new CouldntGetASignalException();
            }
            var mvp = missingValuePolicyRepository.Get(signal);

            if(mvp == null)
            {
                return null;
            }

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> domainData)
        {
            foreach (var x in domainData)
            {
                x.Signal = signal;
            }

            CheckDatasDateTimeCorrection(domainData);

            signalsDataRepository.SetData<T>(domainData);
        }
   

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            SignalUtils.CheckCorrectionOfTimestamp(fromIncludedUtc, signal.Granularity);

            var getData = signalsDataRepository
                .GetData<T>(signal, fromIncludedUtc, toExcludedUtc)
                ?.OrderBy(s => s.Timestamp).ToArray();

            var getMissingValuePolicy = GetMissingValuePolicy(signal);

            if (getMissingValuePolicy == null)
                return getData;
            
     
            var result = new List<Datum<T>>();
            var dt = fromIncludedUtc;

            if (fromIncludedUtc == toExcludedUtc)
            {
                var datum = getData.FirstOrDefault(d => dt <= d.Timestamp);
                if (datum == null)
                    datum = GenerateDatumFromPolicy(getMissingValuePolicy as MissingValuePolicy<T>, signal, dt);
                result.Add(datum);
            }

            while(dt < toExcludedUtc )
            {
                var next = SignalUtils.GetNextDate(dt, signal.Granularity);

                var datum = getData.FirstOrDefault(d => dt <= d.Timestamp && next > d.Timestamp);
                if (datum == null)
                    datum = GenerateDatumFromPolicy(getMissingValuePolicy as MissingValuePolicy<T>, signal, dt);
                result.Add(datum);

                dt = next;
            }
            return result;
        }

        private Datum<T> GenerateDatumFromPolicy<T>(MissingValuePolicy<T> mvp, Signal signal, DateTime timestamp)
        {
            Datum<T> result = null;

            if (mvp is SpecificValueMissingValuePolicy<T> || mvp is NoneQualityMissingValuePolicy<T>)
            {
                result = mvp.FillMissingValue(new Datum<T>() { Signal = signal, Timestamp = timestamp });
            }
            else if (mvp is ShadowMissingValuePolicy<T>)
            {
                result = signalsDataRepository.GetData<T>((mvp as ShadowMissingValuePolicy<T>).ShadowSignal, timestamp, timestamp).SingleOrDefault();

                if (result == null)
                    result = new Datum<T>()
                    {
                        Quality = Quality.None,
                        Signal = signal,
                        Timestamp = timestamp,
                        Value = default(T)
                    };
            }
            else if(mvp is ZeroOrderMissingValuePolicy<T>)
            {
                var datum = signalsDataRepository.GetDataOlderThan<T>(signal, timestamp, 1).FirstOrDefault();
                if (datum == null)
                    result = new Datum<T>() { Quality = Quality.None, Value = default(T), Timestamp = timestamp };
                else
                    result = new Datum<T>() { Quality = datum.Quality, Value = datum.Value, Timestamp = timestamp };
            }
            else  if (mvp is FirstOrderMissingValuePolicy<T>)
            {
                if (typeof(T) == typeof(bool) || typeof(T) == typeof(string))
                {
                    throw new StringAndBooleanException();
                }
                var olderDatum = signalsDataRepository.GetDataOlderThan<T>(signal, timestamp, 1).FirstOrDefault();
                var newerDatum = signalsDataRepository.GetDataNewerThan<T>(signal, timestamp, 1).FirstOrDefault();
                if (olderDatum != null && newerDatum != null)
                {
                    var timestampDifferenceOlderNewerDatum = Datum<T>.GetTimeStampsDifference(signal, olderDatum.Timestamp, newerDatum.Timestamp);
                    var timestampDifferenceOlderMissingDatum = Datum<T>.GetTimeStampsDifference(signal, olderDatum.Timestamp, timestamp);
                    var missingValue = newerDatum.GetFirstOrderValueToAdd(olderDatum.Value, timestampDifferenceOlderNewerDatum, timestampDifferenceOlderMissingDatum);
                    if (newerDatum.Quality > olderDatum.Quality)
                        result = new Datum<T>() { Quality = newerDatum.Quality, Signal = signal, Timestamp = timestamp, Value = missingValue };
                    else
                        result = new Datum<T>() { Quality = olderDatum.Quality, Signal = signal, Timestamp = timestamp, Value = missingValue };
                }
                else
                    result = new Datum<T>() { Quality = Quality.None, Value = default(T), Signal = signal, Timestamp = timestamp }; 
            }

            return result;
        }

        public PathEntry GetPathEntry(Path pathDomain)
        {
            var signals = signalsRepository.GetAllWithPathPrefix(pathDomain);

            var filteredSignals = signals.Where(s => s.Path.Length == pathDomain.Length + 1);
            var subFolders = signals
                .Where(s => s.Path.Length > pathDomain.Length + 1)
                .Select(s => s.Path.GetPrefix(pathDomain.Length + 1))
                .Distinct();

            var result = new PathEntry(filteredSignals, subFolders);

            return result;
        }

        private void CheckDatasDateTimeCorrection<T>(IEnumerable<Datum<T>> data)
        {
            foreach (var d in data)
            {
                SignalUtils.CheckCorrectionOfTimestamp(d.Timestamp, d.Signal.Granularity);
            }
        }

        public void Delete(int signalId)
        {
            var signal = GetById(signalId);
            if (signal == null)
            { 
                throw new CouldntGetASignalException();
            }

            this.missingValuePolicyRepository.Set(signal, null);
            try
            {
                typeof(ISignalsDataRepository)
                .GetMethod("DeleteData")
                .MakeGenericMethod(DataTypeUtils.GetNativeType(signal.DataType))
                .Invoke(signalsDataRepository, new object[] { signal });
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException;
            }

            this.signalsRepository.Delete(signal);
        }

        private void CheckCorrectionOfGetCoarseDataArguments(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            SignalUtils.CheckCorrectionOfTimestamp(fromIncludedUtc, granularity);
            SignalUtils.CheckCorrectionOfTimestamp(toExcludedUtc, granularity);

            if (signal.Granularity > granularity)
            {
                throw new IncorrectGranularityException();
            }

            if (signal.DataType == DataType.Boolean ||
                signal.DataType == DataType.String)
            {
                throw new StringAndBooleanException();
            }
        }

        private Quality GetLowerQualityWithSpecialBehaviourOfNone(Quality main, Quality sub)
        {
            if (main != Quality.None)
            {
                if (sub == Quality.None)
                    main = sub;
                else if (sub > main)
                    main = sub;
            }

            return main;
        }

        public IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            CheckCorrectionOfGetCoarseDataArguments(signal,granularity,fromIncludedUtc,toExcludedUtc);

            IEnumerable<Datum<T>> data = GetData<T>(signal,fromIncludedUtc,toExcludedUtc);
            List<Datum<T>> coarsedData = new List<Datum<T>>();
            DateTime date = fromIncludedUtc;
            DateTime destenationDate = SignalUtils.GetNextDate(fromIncludedUtc,granularity);
            dynamic value = 0, numberOfValues = 0;
            Quality lowestQuality = Quality.Good;
            DateTime timestamp = fromIncludedUtc;

            if(date == toExcludedUtc)
            {
                toExcludedUtc = destenationDate;
            }

            while (date <= toExcludedUtc)
            {
                dynamic datum = data.FirstOrDefault(d => date == d.Timestamp);
                if (date == destenationDate)
                {
                    var datumToAdd = new Datum<T>()
                    {
                        Quality = lowestQuality,
                        Signal = signal,
                        Timestamp = timestamp,
                        Value = value / numberOfValues
                    };
                    value = 0;
                    numberOfValues = 0;
                    lowestQuality = Quality.Good;
                    timestamp = destenationDate;
                    destenationDate = SignalUtils.GetNextDate(destenationDate,granularity);
                    coarsedData.Add(datumToAdd);

                    if (date == toExcludedUtc) break;
                }
                if (datum != null) {
                    value += datum.Value;
                    numberOfValues++;
                    lowestQuality = GetLowerQualityWithSpecialBehaviourOfNone(lowestQuality,datum.Quality);
                }
                date = SignalUtils.GetNextDate(date, signal.Granularity);
            }
            return coarsedData;
        }
    }
}
