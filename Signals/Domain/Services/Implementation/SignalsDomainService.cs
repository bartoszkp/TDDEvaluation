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
                TimestampsCheck<T>(domain_data);
            }
            this.signalsDataRepository.SetData(domain_data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded)
        {
            SingleTimestampCheck(signal, fromIncluded);

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

        private void TimestampsCheck<T>(IEnumerable<Datum<T>> data)
        {
            var signalGranlarity = data.First().Signal.Granularity;

            switch (signalGranlarity)
            {
                case Granularity.Day:
                    if (data.Any(d => d.Timestamp != new DateTime(d.Timestamp.Year, d.Timestamp.Month, d.Timestamp.Day, 0, 0, 0))) throw new DatumTimestampException();
                    break;

                case Granularity.Hour:
                    if (data.Any(d => d.Timestamp != new DateTime(d.Timestamp.Year, d.Timestamp.Month, d.Timestamp.Day, d.Timestamp.Hour, 0, 0))) throw new DatumTimestampException();
                    break; ;

                case Granularity.Minute:
                    if (data.Any(d => d.Timestamp != new DateTime(d.Timestamp.Year, d.Timestamp.Month, d.Timestamp.Day, d.Timestamp.Hour, d.Timestamp.Minute, 0))) throw new DatumTimestampException();
                    break;

                case Granularity.Month:
                    if (data.Any(d => d.Timestamp != new DateTime(d.Timestamp.Year, d.Timestamp.Month, 1, 0, 0, 0))) throw new DatumTimestampException();
                    break;

                case Granularity.Second:
                    if (data.Any(d => d.Timestamp != new DateTime(d.Timestamp.Year, d.Timestamp.Month, d.Timestamp.Day, d.Timestamp.Hour, d.Timestamp.Minute, d.Timestamp.Second, 0))) throw new DatumTimestampException();
                    break;

                case Granularity.Week:
                    if (data.Any(d => d.Timestamp != new DateTime(d.Timestamp.Year, d.Timestamp.Month, d.Timestamp.Day, 0, 0, 0) | d.Timestamp.DayOfWeek == DayOfWeek.Monday)) throw new DatumTimestampException();
                    break;

                case Granularity.Year:
                    if (data.Any(d => d.Timestamp != new DateTime(d.Timestamp.Year, 1, 1, 0, 0, 0))) throw new DatumTimestampException();
                    break;
            }
        }

        private void SingleTimestampCheck(Signal signal, DateTime timestamp)
        {
            switch (signal.Granularity)
            {
                case Granularity.Day:
                    if (timestamp != new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, 0, 0, 0)) throw new DatumTimestampException();
                    break;

                case Granularity.Hour:
                    if (timestamp != new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, 0, 0)) throw new DatumTimestampException();
                    break;

                case Granularity.Minute:
                    if (timestamp != new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, 0)) throw new DatumTimestampException();
                    break;

                case Granularity.Month:
                    if (timestamp != new DateTime(timestamp.Year, timestamp.Month, 1, 0, 0, 0)) throw new DatumTimestampException();
                    break;

                case Granularity.Second:
                    if (timestamp != new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, timestamp.Second, 0)) throw new DatumTimestampException();
                    break;

                case Granularity.Week:
                    if (timestamp != new DateTime(timestamp.Year, timestamp.Month, timestamp.Day, 0, 0, 0) || timestamp.DayOfWeek != DayOfWeek.Monday) throw new DatumTimestampException();
                    break;

                case Granularity.Year:
                    if (timestamp != new DateTime(timestamp.Year, 1, 1, 0, 0, 0)) throw new DatumTimestampException();
                    break;
            }
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
                case Granularity.Week: timeDifference = (int)(nextDatum.Timestamp -currentDatum.Timestamp).TotalDays / 7; break;
                case Granularity.Month: timeDifference = nextDatum.Timestamp.Month - currentDatum.Timestamp.Month; break;
                case Granularity.Year: timeDifference = nextDatum.Timestamp.Year - currentDatum.Timestamp.Year; break;
            }

            var valuesDifference = (dynamic)nextDatum.Value - (dynamic)currentDatum.Value;
            if (timeDifference == 0) timeDifference = 1;
            return valuesDifference / timeDifference;
        }

        private Datum<T> getNewerDatum<T>(Signal signal, DateTime to)
        {
            var datum = signalsDataRepository.GetData<T>(signal, to, to);
            if (datum.Count() == 0) { datum = signalsDataRepository.GetDataNewerThan<T>(signal, to, 1); }
            return datum.FirstOrDefault();
        }
        private Datum<T> getOlderDatum<T>(Signal signal, DateTime from)
        {
            var datum = signalsDataRepository.GetData<T>(signal, from, from);
            if (datum.Count() == 0) { datum = signalsDataRepository.GetDataOlderThan<T>(signal, from, 1); }
            return datum.FirstOrDefault();
        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicy.MissingValuePolicyBase missingValuePolicy)
        {
            if (missingValuePolicy is ShadowMissingValuePolicy<bool> |
                missingValuePolicy is ShadowMissingValuePolicy<int> |
                missingValuePolicy is ShadowMissingValuePolicy<double> |
                missingValuePolicy is ShadowMissingValuePolicy<decimal> |
                missingValuePolicy is ShadowMissingValuePolicy<string> )
            {
                switch (signal.DataType)
                {
                    case DataType.Boolean:
                        if (missingValuePolicy.NativeDataType == typeof(bool) &&
                             (missingValuePolicy as ShadowMissingValuePolicy<bool>).ShadowSignal.DataType == signal.DataType &
                             (missingValuePolicy as ShadowMissingValuePolicy<bool>).ShadowSignal.Granularity == signal.Granularity)
                            missingValuePolicyRepository.Set(signal, missingValuePolicy);
                        else throw new ArgumentException();
                       break;
                    case DataType.Integer:
                        if (missingValuePolicy.NativeDataType == typeof(int) &&
                             (missingValuePolicy as ShadowMissingValuePolicy<int>).ShadowSignal.DataType == signal.DataType &
                             (missingValuePolicy as ShadowMissingValuePolicy<int>).ShadowSignal.Granularity == signal.Granularity)
                            missingValuePolicyRepository.Set(signal, missingValuePolicy);
                        else throw new ArgumentException();
                        break;
                    case DataType.Double:
                        if (missingValuePolicy.NativeDataType == typeof(double) &&
                             (missingValuePolicy as ShadowMissingValuePolicy<double>).ShadowSignal.DataType == signal.DataType &
                             (missingValuePolicy as ShadowMissingValuePolicy<double>).ShadowSignal.Granularity == signal.Granularity)
                            missingValuePolicyRepository.Set(signal, missingValuePolicy);
                        else throw new ArgumentException();
                        break;
                    case DataType.Decimal:
                        if (missingValuePolicy.NativeDataType == typeof(decimal) &&
                             (missingValuePolicy as ShadowMissingValuePolicy<decimal>).ShadowSignal.DataType == signal.DataType &
                             (missingValuePolicy as ShadowMissingValuePolicy<decimal>).ShadowSignal.Granularity == signal.Granularity)
                            missingValuePolicyRepository.Set(signal, missingValuePolicy);
                        else throw new ArgumentException();
                        break;
                    case DataType.String:
                        if (missingValuePolicy.NativeDataType == typeof(string) &&
                             (missingValuePolicy as ShadowMissingValuePolicy<string>).ShadowSignal.DataType == signal.DataType &
                             (missingValuePolicy as ShadowMissingValuePolicy<string>).ShadowSignal.Granularity == signal.Granularity)
                            missingValuePolicyRepository.Set(signal, missingValuePolicy);
                        else throw new ArgumentException();
                        break;
                    default:
                        break;
                }
            }
            else this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }

        public MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var mvp = this.missingValuePolicyRepository.Get(signal);
            if (mvp == null)
                return null;

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
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
    }
}
