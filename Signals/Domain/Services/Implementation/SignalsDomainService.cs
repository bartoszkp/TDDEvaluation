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
                throw new ArgumentException("no signal with this id");

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
                throw new ArgumentException("no signal with this id");
            }
            var mvp = missingValuePolicyRepository.Get(signal);

            if(mvp == null)
            {
                return null;
            }

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetData<T>(IEnumerable<Datum<T>> domainData)
        {
            CheckDatasDateTimeCorrection(domainData);

            signalsDataRepository.SetData<T>(domainData);
        }
   

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            CheckTimestampsCorrection(signal, fromIncludedUtc);

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
            else  if (mvp is FirstOrderMissingValuePolicy<int> || mvp is FirstOrderMissingValuePolicy<double> || mvp is FirstOrderMissingValuePolicy<decimal>)
            {
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
                CheckTimestampsCorrection(d.Signal, d.Timestamp);
            }
        }

        private void CheckTimestampsCorrection(Signal signal, DateTime dateTime)
        {
            if ((signal.Granularity == Granularity.Second && dateTime.Millisecond != 0)
                || (signal.Granularity == Granularity.Minute && (dateTime.Second != 0 || dateTime.Millisecond != 0) )
                || (signal.Granularity == Granularity.Hour && (dateTime.Minute != 0 || dateTime.Second != 0 || dateTime.Millisecond != 0) )
                || (signal.Granularity == Granularity.Day && dateTime.TimeOfDay.Ticks != 0)
                || (signal.Granularity == Granularity.Week && (dateTime.TimeOfDay.Ticks != 0 || dateTime.DayOfWeek != DayOfWeek.Monday) )
                || (signal.Granularity == Granularity.Month
                    && (dateTime.TimeOfDay.Ticks != 0 || DateTime.Compare(dateTime, new DateTime(dateTime.Year, dateTime.Month, 1)) != 0))
                || (signal.Granularity == Granularity.Year 
                    && (dateTime.TimeOfDay.Ticks != 0 || DateTime.Compare(dateTime, new DateTime(dateTime.Year, 1, 1)) != 0)) )
                throw new ArgumentException("DateTime is incorrect");
        }

        public void Delete(int signalId)
        {
            var signal = GetById(signalId);
            if (signal == null)
                throw new ArgumentException("Signal with given Id does not exist.");

            this.missingValuePolicyRepository.Set(signal, null);
            switch (signal.DataType)
            {
                case DataType.Boolean:
                    this.signalsDataRepository.DeleteData<bool>(signal);
                    break;
                case DataType.Integer:
                    this.signalsDataRepository.DeleteData<int>(signal);
                    break;
                case DataType.Double:
                    this.signalsDataRepository.DeleteData<double>(signal);
                    break;
                case DataType.Decimal:
                    this.signalsDataRepository.DeleteData<decimal>(signal);
                    break;
                case DataType.String:
                    this.signalsDataRepository.DeleteData<string>(signal);
                    break;
                default:
                    this.signalsDataRepository.DeleteData<bool>(signal);
                    break;
            }

            this.signalsRepository.Delete(signal);
        }
    }
}
