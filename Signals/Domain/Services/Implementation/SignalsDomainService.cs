using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Mapster;

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

            return this.signalsRepository.Add(newSignal);
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path pathDomain)
        {
            var result = signalsRepository.Get(pathDomain);
            return result;
        }

        public void Delete(int signalId)
        {
            var signal = GetById(signalId);
            if (signal == null)
                throw new SignalNotFoundException();

            missingValuePolicyRepository.Set(signal, null);
            DeleteSignalsData(signal);
            signalsRepository.Delete(signal);
        }

        public void SetMissingValuePolicy(MissingValuePolicyBase policy)
        {
            if (policy.Signal == null) throw new ArgumentException();
            missingValuePolicyRepository.Set(policy.Signal, policy);
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase policy)
        {
            Signal signal = this.signalsRepository.Get(signalId);
            if (signal == null) throw new ArgumentException();

            CheckShadow(signal, policy);

            missingValuePolicyRepository.Set(signal, policy);
        }

        public MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(int signalID)
        {
            Signal signal = signalsRepository.Get(signalID);
            if (signal == null) throw new ArgumentException();

            MissingValuePolicyBase result = missingValuePolicyRepository.Get(signal);

            if (result != null)
            {
                return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType)
                 as MissingValuePolicy.MissingValuePolicyBase;
            }
            else return null;
        }

        public void SetData<T>(IEnumerable<Datum<T>> data)
        {
            signalsDataRepository.SetData<T>(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            IEnumerable<Datum<T>> result = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).ToArray();
            var earlierDatum = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1).FirstOrDefault();
            var laterDatum = signalsDataRepository.GetDataNewerThan<T>(signal, toExcludedUtc, 1).FirstOrDefault();

            var policy = GetMissingValuePolicy(signal.Id.Value) as MissingValuePolicy<T>;

            if (policy != null)
            {
                if(policy is ShadowMissingValuePolicy<T>)
                {
                    var shadowPolicy = (ShadowMissingValuePolicy<T>)policy;
                    var shadowSignal = shadowPolicy.ShadowSignal;

                    if (fromIncludedUtc > toExcludedUtc) return new List<Datum<T>>();
                    else if (fromIncludedUtc == toExcludedUtc)
                    {
                        var datum = result.FirstOrDefault(d => d.Timestamp == fromIncludedUtc);
                        if (datum == null) return ShadowDatum<T>(shadowSignal, fromIncludedUtc);
                        else return new[] { datum };
                    }

                    else
                    {
                        List<Datum<T>> newDatumList = new List<Datum<T>>();
                        DateTime tmpTime = fromIncludedUtc;
                        
                        while(tmpTime < toExcludedUtc)
                        {
                            var datum = result.FirstOrDefault(d => d.Timestamp == tmpTime);
                            if (datum == null) newDatumList.Add(ShadowDatum<T>(shadowSignal, tmpTime).First());
                            else newDatumList.Add(datum);

                            tmpTime = IncreaseDateTime(signal.Granularity, tmpTime);
                        }

                        return newDatumList;
                    }
                }
                else result = policy.SetMissingValue(signal, result, fromIncludedUtc, toExcludedUtc, earlierDatum, laterDatum);
            }
            return result;
        }

        private DateTime IncreaseDateTime(Granularity granularity, DateTime datetime)
        {
            DateTime newDate = new DateTime();

            switch(granularity)
            {
                case Granularity.Day: return newDate = datetime.AddDays(1);
                case Granularity.Hour: return newDate = datetime.AddHours(1);
                case Granularity.Minute: return newDate = datetime.AddMinutes(1);
                case Granularity.Month: return newDate = datetime.AddMonths(1);
                case Granularity.Second: return newDate = datetime.AddSeconds(1);
                case Granularity.Week: return newDate = datetime.AddDays(7);
                case Granularity.Year: return newDate = datetime.AddYears(1);
                default: return newDate;
            }
        }

        private IEnumerable<Datum<T>> ShadowDatum<T>(Signal signal, DateTime date)
        {
            List<Datum<T>> shadowDatum = signalsDataRepository.GetData<T>(signal, date, date).ToList();
            if (shadowDatum != null && shadowDatum.Count != 0) return shadowDatum;
            else return new[] { Datum<T>.CreateNone(signal, date) };
        }

        public PathEntry GetPathEntry(Path path)
        {
            var allSignalsWithPathPrefix = signalsRepository.GetAllWithPathPrefix(path);

            var signals = new List<Signal>();

            var subPaths = new List<Path>();

            foreach (var signal in allSignalsWithPathPrefix)
            {
                if (signal.Path.Components.Count() == path.Components.Count()) continue;

                else if (signal.Path.Components.Count() == path.Components.Count() + 1) signals.Add(signal);

                else
                {
                    string pathToString = null;

                    foreach (var component in path.Components)
                    {
                        pathToString += component + "/";
                    }

                    var newSubPath = Path.FromString(pathToString + signal.Path.Components.ToArray()[path.Length]);

                    if (!subPaths.Contains(newSubPath)) subPaths.Add(newSubPath);
                }
            }

            return new PathEntry(signals, subPaths);
        }

        public IEnumerable<Datum<T>> GetDataOlderThan<T>(Signal signal, DateTime excludedUtc, int maxSampleCount)
        {
            return signalsDataRepository.GetDataOlderThan<T>(signal, excludedUtc, maxSampleCount);
        }

        public IEnumerable<Datum<T>> GetDataNewerThan<T>(Signal signal, DateTime includedUtc, int maxSampleCount)
        {
            return signalsDataRepository.GetDataNewerThan<T>(signal, includedUtc, maxSampleCount);
        }

        private void DeleteSignalsData(Signal signal)
        {
            switch (signal.DataType)
            {
                case DataType.Boolean:
                    signalsDataRepository.DeleteData<bool>(signal);
                    return;
                case DataType.Decimal:
                    signalsDataRepository.DeleteData<decimal>(signal);
                    return;
                case DataType.Double:
                    signalsDataRepository.DeleteData<double>(signal);
                    return;
                case DataType.Integer:
                    signalsDataRepository.DeleteData<int>(signal);
                    return;
                case DataType.String:
                    signalsDataRepository.DeleteData<string>(signal);
                    return;
                default:
                    return;
            }
        }

        private void CheckShadow(Signal signal, MissingValuePolicyBase policy)
        {
            switch(signal.DataType)
            {
                case DataType.Boolean: if (policy is ShadowMissingValuePolicy<bool>)    ShadowSignalIsCorrectly<bool>   (signal, policy as ShadowMissingValuePolicy<bool>); break;
                case DataType.Decimal: if (policy is ShadowMissingValuePolicy<decimal>) ShadowSignalIsCorrectly<decimal>(signal, policy as ShadowMissingValuePolicy<decimal>); break;
                case DataType.Double:  if (policy is ShadowMissingValuePolicy<double>)  ShadowSignalIsCorrectly<double> (signal, policy as ShadowMissingValuePolicy<double>); break;
                case DataType.Integer: if (policy is ShadowMissingValuePolicy<int>)     ShadowSignalIsCorrectly<int>    (signal, policy as ShadowMissingValuePolicy<int>); break;
                case DataType.String:  if (policy is ShadowMissingValuePolicy<string>)  ShadowSignalIsCorrectly<string> (signal, policy as ShadowMissingValuePolicy<string>); break;
            }
        }

        private void ShadowSignalIsCorrectly<T>(Signal signal, ShadowMissingValuePolicy<T> policy)
        {            
            Signal shadowSignal = policy.ShadowSignal;

            if (signal.DataType != shadowSignal.DataType || signal.Granularity != shadowSignal.Granularity) throw new ShadowMissingValuePolicyException();
            
            while(shadowSignal != null)
            {
                if (shadowSignal.Id == signal.Id) throw new ShadowMissingCyclePolicyException();

                var ShadowSignalPolicy = missingValuePolicyRepository.Get(shadowSignal) as ShadowMissingValuePolicy<T>;
                if (ShadowSignalPolicy == null) break;
                else shadowSignal = ShadowSignalPolicy.ShadowSignal;
            }
        }

        public IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (fromIncludedUtc == toExcludedUtc) toExcludedUtc = IncreaseDateTime(granularity, toExcludedUtc);
            var sourseData = this.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).ToArray();

            if (signal.Granularity > granularity) throw new GetCoarseDataGranularityExceptions();
            if (typeof(T) == typeof(bool) || typeof(T) == typeof(string)) throw new GetCoarseDataTypeExceptions();

            List<Datum<T>> newDatumList = new List<Datum<T>>();

            DateTime tmpTime = fromIncludedUtc;

            while (tmpTime < toExcludedUtc)
            {
                newDatumList.Add(calculateAVGDatum<T>(signal, sourseData, tmpTime, IncreaseDateTime(granularity, tmpTime)));
                tmpTime = IncreaseDateTime(granularity, tmpTime);
            }
            return newDatumList;
        }
        private Datum<T> calculateAVGDatum<T>(Signal signal, Datum<T>[] datums, DateTime currentDate, DateTime endsDate)
        {
            var targetDatums = datums.Where(q => q.Timestamp >= currentDate && q.Timestamp < endsDate);

            Quality quality;
            if (targetDatums.Select(s => s.Quality).Contains(Quality.None)) quality = Quality.None;
            else quality = targetDatums.Max(m => m.Quality);

            
            dynamic summ = default(T);
            foreach (var i in targetDatums)
            { summ = summ + i.Value; }

            T AVG = summ / targetDatums.Count();

            return new Datum<T>()
            {
                Quality = quality,
                Signal  =signal,
                Timestamp = currentDate,
                Value = AVG,
            };
        }
    }
}
