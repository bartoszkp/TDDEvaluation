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
                    if (data.Any(d => d.Timestamp != new DateTime(d.Timestamp.Year, d.Timestamp.Month, d.Timestamp.Day, 0, 0, 0) && d.Timestamp.DayOfWeek != DayOfWeek.Monday)) throw new DatumTimestampException();
                    break;

                case Granularity.Year:
                    if (data.Any(d => d.Timestamp != new DateTime(d.Timestamp.Year, 1, 1, 0, 0, 0))) throw new DatumTimestampException();
                    break;
            }
        }

        private void SingleTimestampCheck(Signal signal,DateTime timestamp)
        {
            switch(signal.Granularity)
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

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded)
        {
            SingleTimestampCheck(signal, fromIncluded);

            var result = this.signalsDataRepository.GetData<T>(signal, fromIncluded, toExcluded);
            if (result != null) result.OrderBy(s=>s.Timestamp);

            var policy = GetMissingValuePolicy(signal);
            if (policy == null) return result;
            else
            {
                var timeNextMethod = GetTimeNextMethod(signal.Granularity);
                var timePreviousMethod = GetTimePreviousMethod(signal.Granularity);
                var date = fromIncluded;
                var newData = new List<Datum<T>>();

                if (policy is NoneQualityMissingValuePolicy<T>)
                {
                    if (fromIncluded == toExcluded)
                    {
                        var datum = result.FirstOrDefault(d => d.Timestamp == date);
                        newData.Add(datum ?? Datum<T>.CreateNone(policy.Signal, date));
                    }
                    else
                    {
                        while (date < toExcluded)
                        {
                            var datum = result.FirstOrDefault(d => d.Timestamp == date);
                            newData.Add(datum ?? Datum<T>.CreateNone(policy.Signal, date));
                            date = timeNextMethod(date);
                        }
                    }
                }
                else if (policy is SpecificValueMissingValuePolicy<T>)
                {
                    var mvp = policy as SpecificValueMissingValuePolicy<T>;
                    while (date < toExcluded)
                    {
                        var datum = result.FirstOrDefault(d => d.Timestamp == date);
                        newData.Add(datum ?? Datum<T>.CreateSpecific(policy.Signal, date, mvp.Quality, mvp.Value));
                        date = timeNextMethod(date);
                    }
                }
                else if (policy is ZeroOrderMissingValuePolicy<T>)
                {
                    while (date < toExcluded)
                    {
                        Datum<T> datum;
                        Datum<T> previousDatum;

                        if (date == fromIncluded)
                        {
                            datum = result.FirstOrDefault(d => d.Timestamp == date);
                            if (datum == null) datum = new Datum<T>() { Quality = Quality.None, Value = default(T), Timestamp = date };
                            newData.Add(datum);
                        }

                        else
                        {
                            datum = result.FirstOrDefault(d => d.Timestamp == date);
                            previousDatum = result.FirstOrDefault(d => d.Timestamp == timePreviousMethod(date));
                            if (datum != null) newData.Add(datum);

                            else
                            {
                                if (previousDatum != null) newData.Add(Datum<T>.CreateSpecific(policy.Signal, date, previousDatum.Quality, previousDatum.Value));
                                else throw new ZeroOrderMVPException();
                            }
                        }
                        date = timeNextMethod(date);
                    }
                }

                else if (policy is FirstOrderMissingValuePolicy<T>)
                {
                    var newerData = getNewerDatum<T>(signal, toExcluded);
                    if (newerData == null) newerData = result.Last();
                    var olderData = getOlderDatum<T>(signal, fromIncluded);
                    if (olderData == null) olderData = result.First();

                    dynamic step = 0;
                    while (date < toExcluded)
                    {
                        Datum<T> datum = null;

                        if (date > newerData.Timestamp || date < olderData.Timestamp)
                        {
                            datum = new Datum<T>() { Quality = Quality.None, Timestamp = date, Value = default(T) };
                            newData.Add(datum);
                            date = timeNextMethod(date);
                            continue;
                        }


                        var nextExsistingDatum = getNewerDatum<T>(signal, timeNextMethod(date));
                        var currentDatum = result.FirstOrDefault<Datum<T>>(d => d.Timestamp == date);
                        if (currentDatum != null)
                        {
                            newData.Add(currentDatum);
                            olderData = currentDatum;
                            if (nextExsistingDatum != null) step = GetStep<T>(signal, currentDatum, nextExsistingDatum);
                        }

                        if (date > olderData.Timestamp)
                        {
                            var previousDatum = newData.LastOrDefault();
                            if (previousDatum != null)
                            {
                                Quality quality;

                                if (previousDatum.Quality > nextExsistingDatum.Quality) quality = previousDatum.Quality; else quality = nextExsistingDatum.Quality;

                                datum = new Datum<T>() { Timestamp = timeNextMethod(previousDatum.Timestamp), Value = previousDatum.Value + step, Quality = quality };

                            }
                            else
                            {
                                var prevExsistingDatum = getOlderDatum<T>(signal, date);
                                step = GetStep<T>(signal, prevExsistingDatum, nextExsistingDatum);
                                datum = this.GetData<T>(signal, prevExsistingDatum.Timestamp, nextExsistingDatum.Timestamp).First(q => q.Timestamp == date);
                            }
                            newData.Add(datum);
                        }
                        date = timeNextMethod(date);
                    }
                }

                else if (policy is ShadowMissingValuePolicy<T>)
                {
                    if (fromIncluded > toExcluded) return new Datum<T>[] { };

                    else if(fromIncluded == toExcluded)
                    {
                        var filledData = new List<Datum<T>>();
                        var signalData = signalsDataRepository.GetData<T>(signal, fromIncluded, toExcluded);
                        var shadowData = signalsDataRepository.GetData<T>((policy as ShadowMissingValuePolicy<T>).ShadowSignal, fromIncluded, toExcluded);

                        AddToListSuitableDatumWhenGivenIsSMVP(filledData, signalData, fromIncluded);
                        if (filledData.Count() == 0) AddToListSuitableDatumWhenGivenIsSMVP(filledData, shadowData, fromIncluded);
                        if (filledData.Count() == 0) filledData.Add(new Datum<T>() { Signal = signal, Quality = Quality.None, Value = default(T), Timestamp = fromIncluded });
                        return filledData;
                    }

                    else
                    {
                        var filledData = new List<Datum<T>>();
                        var signalData = signalsDataRepository.GetData<T>(signal, fromIncluded, toExcluded);
                        var shadowData = signalsDataRepository.GetData<T>((policy as ShadowMissingValuePolicy<T>).ShadowSignal, fromIncluded, toExcluded);
                        var tmp = fromIncluded;

                        switch (signal.Granularity)
                        {
                            case Granularity.Second:
                                while (tmp < toExcluded)
                                {
                                    filledData.Add(new Datum<T>() { Quality = Quality.None, Value = default(T) });
                                    tmp = tmp.AddSeconds(1);
                                }
                                break;
                            case Granularity.Minute:
                                while (tmp < toExcluded)
                                {
                                    filledData.Add(new Datum<T>() { Quality = Quality.None, Value = default(T) });
                                    tmp = tmp.AddMinutes(1);
                                }
                                break;
                            case Granularity.Hour:
                                while (tmp < toExcluded)
                                {
                                    filledData.Add(new Datum<T>() { Quality = Quality.None, Value = default(T) });
                                    tmp = tmp.AddHours(1);
                                }
                                break;
                            case Granularity.Day:
                                while (tmp < toExcluded)
                                {
                                    filledData.Add(new Datum<T>() { Quality = Quality.None, Value = default(T) });
                                    tmp = tmp.AddDays(1);
                                }
                                break;
                            case Granularity.Week:
                                while (tmp < toExcluded)
                                {
                                    filledData.Add(new Datum<T>() { Quality = Quality.None, Value = default(T) });
                                    tmp = tmp.AddDays(7);
                                }
                                break;
                            case Granularity.Month:
                                while (tmp < toExcluded)
                                {
                                    filledData.Add(new Datum<T>() { Quality = Quality.None, Value = default(T) });
                                    tmp = tmp.AddMonths(1);
                                }
                                break;
                            case Granularity.Year:
                                while (tmp < toExcluded)
                                {
                                    filledData.Add(new Datum<T>() { Quality = Quality.None, Value = default(T) });
                                    tmp = tmp.AddYears(1);
                                }
                                break;
                            default: break;
                        }
                        return filledData;
                    }
                }

                else throw new NotImplementedException();

                return newData;
            }
        }

        private void AddToListSuitableDatumWhenGivenIsSMVP<T>(List<Datum<T>> filledData, IEnumerable<Datum<T>> data, DateTime fromIncluded)
        {
            foreach (var datum in data)
            {
                if (fromIncluded == datum.Timestamp)
                {
                    filledData.Add(new Datum<T> { Id = datum.Id, Quality = datum.Quality, Signal = datum.Signal, Timestamp = datum.Timestamp, Value = datum.Value });
                }
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

        private Func<DateTime, DateTime> GetTimePreviousMethod(Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return (date) => date.AddSeconds(-1);
                case Granularity.Minute:
                    return (date) => date.AddMinutes(-1);
                case Granularity.Hour:
                    return (date) => date.AddHours(-1);
                case Granularity.Day:
                    return (date) => date.AddDays(-1);
                case Granularity.Week:
                    return (date) => date.AddDays(-7);
                case Granularity.Month:
                    return (date) => date.AddMonths(-1);
                case Granularity.Year:
                    return (date) => date.AddYears(-1);
                default:
                    throw new NotImplementedException();
            }
        }

        public void Delete<T>(int signalId)
        {
            var signal = signalsRepository.Get(signalId);
            missingValuePolicyRepository.Set(signal, null);
            signalsDataRepository.DeleteData<T>(signal);
            signalsRepository.Delete(signal);
        }
    }
}
