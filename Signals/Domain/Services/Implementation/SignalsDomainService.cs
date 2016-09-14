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

        public void Delete<T>(int signalId)
        {
            var signal = signalsRepository.Get(signalId);
            missingValuePolicyRepository.Set(signal, null);
            signalsDataRepository.DeleteData<T>(signal);
            signalsRepository.Delete(signal);
        }

        public Signal Add(Signal newSignal)
        {
            var result = this.signalsRepository.Add(newSignal);

            SetDefaultMissingValuePolicy(result);

            return result;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path path)
        {
            return this.signalsRepository.Get(path);
        }

        public PathEntry GetPathEntry(Path path)
        {
            var prefixedSignals = signalsRepository.GetAllWithPathPrefix(path);
                        
            var subpaths_query = prefixedSignals
                .Where(s => s.Path.Components.Count() > path.Components.Count() + 1)
                .Select(s => s.Path.Components.Take(path.Components.Count() + 1))
                .GroupBy(s => s.Last())
                .Select(s => s.First());

            var subpaths = new List<Path>();
            foreach (var component_str in subpaths_query)            
                subpaths.Add(Path.FromString(Path.JoinComponents(component_str)));

            var signals = prefixedSignals.Where(s => s.Path.Components.Count() == path.Components.Count() + 1);

            var pathEntry = new PathEntry(signals, subpaths);
            return pathEntry;
        }

        public void SetData<T>(IEnumerable<Datum<T>> domain_data)
        {
            if(domain_data != null && domain_data.Count() > 0)
            {
                var granularity = domain_data.First().Signal.Granularity;

                foreach(var datum in domain_data)                
                    if (!TimestampIsValid(datum.Timestamp, granularity))
                        throw new DatumTimestampException();
            }
            this.signalsDataRepository.SetData(domain_data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded)
        {
            if(!TimestampIsValid(fromIncluded, signal.Granularity) || !TimestampIsValid(toExcluded, signal.Granularity))
                throw new DatumTimestampException();

            var result = this.signalsDataRepository.GetData<T>(signal, fromIncluded, toExcluded);
            if (result != null) result.OrderBy(s=>s.Timestamp);
            var policy = GetMissingValuePolicy(signal);

            if (policy == null)
                return result;
            if (fromIncluded > toExcluded)
                return new List<Datum<T>>();
            
            DateTime tmp = fromIncluded;
            var timeNextMethod = GetTimeNextMethod(signal.Granularity);
            var filledData = new List<Datum<T>>();

            var mvp = policy as MissingValuePolicy<T>;
            List<Datum<T>> shadowData = null;
            Datum<T> previousDatum = null;
            Datum<T> nextDatum = null;

            if (mvp is ZeroOrderMissingValuePolicy<T> || mvp is FirstOrderMissingValuePolicy<T>)
                previousDatum = signalsDataRepository.GetDataOlderThan<T>(signal, toExcluded, 1).FirstOrDefault();
            if (mvp is FirstOrderMissingValuePolicy<T>)
                nextDatum = signalsDataRepository.GetDataNewerThan<T>(signal, fromIncluded, 1).FirstOrDefault();
            if(mvp is ShadowMissingValuePolicy<T>)
                shadowData = signalsDataRepository.GetData<T>((policy as ShadowMissingValuePolicy<T>).ShadowSignal, fromIncluded, toExcluded).ToList();

            if (fromIncluded == toExcluded)                            
                filledData.Add(mvp.FillMissingValue(signal, tmp, result, previousDatum, nextDatum, shadowData));            
            else
            {
                while (tmp < toExcluded)
                {
                    if(mvp is FirstOrderMissingValuePolicy<T> || mvp is ZeroOrderMissingValuePolicy<T>)
                    {
                        var prev = signalsDataRepository.GetDataOlderThan<T>(signal, tmp, 1);
                        var next = signalsDataRepository.GetDataNewerThan<T>(signal, tmp, 1);
                        if (prev == null || next == null)
                        {
                            filledData.Add(Datum<T>.CreateNone(signal, tmp));
                            tmp = timeNextMethod(tmp);
                            continue;
                        }
                        else
                        {
                            previousDatum = prev.FirstOrDefault();
                            nextDatum = next.FirstOrDefault();
                        }                            
                    }
                    filledData.Add(mvp.FillMissingValue(signal, tmp, result, previousDatum, nextDatum, shadowData));
                    tmp = timeNextMethod(tmp);
                }
            }

            return filledData;          
        }
        
        public IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc, int periodSize)
        {
            var filledList = new List<Datum<T>>();
            var data = GetData<T>(signal, fromIncludedUtc, toExcludedUtc).ToList();            
            int coarseLength = data.Count / periodSize;

            for (int i = 0; i < coarseLength; ++i)
            {
                var fragmentData = data.GetRange(i * periodSize, periodSize);
                
                var coarseDatum = new Datum<T>()
                {
                    Quality = fragmentData.Select(d => d.Quality).Min(),
                    Timestamp = data[i * periodSize].Timestamp,
                    Value = GetMeanValue(fragmentData, periodSize)
                };
                filledList.Add(coarseDatum);
            }

            return filledList;
        }        

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase missingValuePolicy)
        {
            ShadowMVPHandling(signal, missingValuePolicy);
            this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var mvp = this.missingValuePolicyRepository.Get(signal);
            if (mvp == null)
                return null;

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicyBase;
        }

        private void ShadowMVPHandling(Signal signal, MissingValuePolicyBase missingValuePolicy)
        {
            if (missingValuePolicy is ShadowMissingValuePolicy<bool> |
                missingValuePolicy is ShadowMissingValuePolicy<int> |
                missingValuePolicy is ShadowMissingValuePolicy<double> |
                missingValuePolicy is ShadowMissingValuePolicy<decimal> |
                missingValuePolicy is ShadowMissingValuePolicy<string>)
            {
                switch (signal.DataType)
                {
                    case DataType.Boolean:
                        if (missingValuePolicy.NativeDataType == typeof(bool) &&
                             (missingValuePolicy as ShadowMissingValuePolicy<bool>).ShadowSignal.DataType == signal.DataType &&
                             (missingValuePolicy as ShadowMissingValuePolicy<bool>).ShadowSignal.Granularity == signal.Granularity &&
                            ShadowDependencyCycleCheck(signal, missingValuePolicy as ShadowMissingValuePolicy<bool>))
                            return;
                        else throw new IncompatibleShadowSignalException();
                    case DataType.Integer:
                        if (missingValuePolicy.NativeDataType == typeof(int) &&
                             (missingValuePolicy as ShadowMissingValuePolicy<int>).ShadowSignal.DataType == signal.DataType &
                             (missingValuePolicy as ShadowMissingValuePolicy<int>).ShadowSignal.Granularity == signal.Granularity &&
                            ShadowDependencyCycleCheck(signal, missingValuePolicy as ShadowMissingValuePolicy<int>))
                            return;
                        else throw new IncompatibleShadowSignalException();
                    case DataType.Double:
                        if (missingValuePolicy.NativeDataType == typeof(double) &&
                             (missingValuePolicy as ShadowMissingValuePolicy<double>).ShadowSignal.DataType == signal.DataType &
                             (missingValuePolicy as ShadowMissingValuePolicy<double>).ShadowSignal.Granularity == signal.Granularity &&
                            ShadowDependencyCycleCheck(signal, missingValuePolicy as ShadowMissingValuePolicy<double>))
                            return;
                        else throw new IncompatibleShadowSignalException();                        
                    case DataType.Decimal:
                        if (missingValuePolicy.NativeDataType == typeof(decimal) &&
                             (missingValuePolicy as ShadowMissingValuePolicy<decimal>).ShadowSignal.DataType == signal.DataType &
                             (missingValuePolicy as ShadowMissingValuePolicy<decimal>).ShadowSignal.Granularity == signal.Granularity &&
                            ShadowDependencyCycleCheck(signal, missingValuePolicy as ShadowMissingValuePolicy<decimal>))
                            return;
                        else throw new IncompatibleShadowSignalException();                        
                    case DataType.String:
                        if (missingValuePolicy.NativeDataType == typeof(string) &&
                             (missingValuePolicy as ShadowMissingValuePolicy<string>).ShadowSignal.DataType == signal.DataType &
                             (missingValuePolicy as ShadowMissingValuePolicy<string>).ShadowSignal.Granularity == signal.Granularity &&
                            ShadowDependencyCycleCheck(signal, missingValuePolicy as ShadowMissingValuePolicy<string>))
                            return;
                        else throw new IncompatibleShadowSignalException();                        
                    default:
                        break;
                }
            }
        }
        private bool ShadowDependencyCycleCheck<T>(Signal signal, ShadowMissingValuePolicy<T> mvp)
        {
            List<int> ids = new List<int>();
            int newId = signal.Id.Value;

            ShadowMissingValuePolicy<T> temp_mvp = mvp;
            while (temp_mvp != null)
            {
                if (temp_mvp.ShadowSignal != null)
                    ids.Add(temp_mvp.ShadowSignal.Id.Value);
                var shadowSignal = signalsRepository.Get(temp_mvp.ShadowSignal.Id.Value);

                var next_mvp = missingValuePolicyRepository.Get(shadowSignal);
                if (next_mvp == null || !(next_mvp is ShadowMissingValuePolicy<T>))
                    break;
                temp_mvp = next_mvp as ShadowMissingValuePolicy<T>;
            }

            if (ids.Contains(newId))
                throw new ShadowDependencyCycleException();
                        
            return true;
        }

        private void SetDefaultMissingValuePolicy(Signal signal)
        {
            var policy = MissingPolicyValueFromType(signal.DataType, typeof(MissingValuePolicy.NoneQualityMissingValuePolicy<>));

            SetMissingValuePolicy(signal, policy);
        }

        private MissingValuePolicy.MissingValuePolicyBase MissingPolicyValueFromType(DataType dataType, Type type)
        {
            var genericType = type.MakeGenericType(dataType.GetNativeType());

            return (MissingValuePolicy.MissingValuePolicyBase)Activator.CreateInstance(genericType);
        }

        private Func<DateTime, DateTime> GetTimeNextMethod(Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return (date) => date.AddSeconds(1);
                case Granularity.Minute:
                    return (date) => date.AddMinutes(1);
                case Granularity.Hour:
                    return (date) => date.AddHours(1);
                case Granularity.Day:
                    return (date) => date.AddDays(1);
                case Granularity.Week:
                    return (date) => date.AddDays(7);
                case Granularity.Month:
                    return (date) => date.AddMonths(1);
                case Granularity.Year:
                    return (date) => date.AddYears(1);
                default:
                    throw new NotImplementedException();
            }
        }
        private bool TimestampIsValid(DateTime tmp, Granularity g)
        {
            bool seconds = tmp.Millisecond == 0;
            if (g == Granularity.Second)
                return seconds;
            bool minutes = seconds && tmp.Second == 0;
            if (g == Granularity.Minute)
                return minutes;
            bool hours = minutes && tmp.Minute == 0;
            if (g == Granularity.Hour)
                return hours;
            bool days = hours && tmp.Hour == 0;
            if (g == Granularity.Day)
                return days;
            bool weeks = days && tmp.Day % 7 == 1;
            if (g == Granularity.Week)
                return weeks;
            bool months = days && tmp.Day == 1;
            if (g == Granularity.Month)
                return months;
            bool years = months && tmp.Month == 1;
            if (g == Granularity.Year)
                return years;

            return false;
        }

        private dynamic GetStep<T>(Signal signal, Datum<T> currentDatum, Datum<T> nextDatum)
        {
            int timeDifference = 0;
            switch (signal.Granularity)
            {
                case Granularity.Second: timeDifference = nextDatum.Timestamp.Second - currentDatum.Timestamp.Second; break;
                case Granularity.Minute: timeDifference = nextDatum.Timestamp.Minute - currentDatum.Timestamp.Minute; break;
                case Granularity.Hour: timeDifference = nextDatum.Timestamp.Hour - currentDatum.Timestamp.Hour; break;
                case Granularity.Day: timeDifference = nextDatum.Timestamp.Day - currentDatum.Timestamp.Day; break;
                case Granularity.Week: timeDifference = (int)(nextDatum.Timestamp - currentDatum.Timestamp).TotalDays / 7; break;
                case Granularity.Month: timeDifference = nextDatum.Timestamp.Month - currentDatum.Timestamp.Month; break;
                case Granularity.Year: timeDifference = nextDatum.Timestamp.Year - currentDatum.Timestamp.Year; break;
            }

            var valuesDifference = (dynamic)nextDatum.Value - (dynamic)currentDatum.Value;
            if (timeDifference == 0) timeDifference = 1;
            return valuesDifference / timeDifference;
        }

        private T GetMeanValue<T>(List<Datum<T>> data, int div)
        {
            dynamic sum = 0;
            foreach (var datum in data)
                sum += datum.Value;

            var mean = sum / div;
            return mean;
        }
    }
}
