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
            if (fromIncluded == toExcluded)
            {
                if (result.Count() == 0)
                    return result;

                return new Datum<T>[1] { result.First() };
            }                

            var policy = GetMissingValuePolicy(signal);

            if (policy != null)
            {
                var timeNextMethod = GetTimeNextMethod(signal.Granularity);
                var timePreviousMethod = GetTimePreviousMethod(signal.Granularity);
                var date = fromIncluded;
                var newData = new List<Datum<T>>();

                if (policy is NoneQualityMissingValuePolicy<T>)
                    while (date < toExcluded)
                    {
                        var datum = result.FirstOrDefault(d => d.Timestamp == date);                    
                        newData.Add(datum ?? Datum<T>.CreateNone(policy.Signal, date));
                        date = timeNextMethod(date);
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

                        if(date == fromIncluded)
                        {
                            datum = result.FirstOrDefault(d => d.Timestamp == date);
                            if (datum == null) datum = new Datum<T>() { Quality = Quality.None, Value = default(T), Timestamp = date};
                            newData.Add(datum);
                        }

                        else
                        {
                            datum = result.FirstOrDefault(d => d.Timestamp == date);
                            previousDatum = result.FirstOrDefault(d => d.Timestamp == timePreviousMethod(date));
                            if (datum != null) newData.Add(datum);

                            else
                            {
                                if(previousDatum != null) newData.Add(Datum<T>.CreateSpecific(policy.Signal, date, previousDatum.Quality, previousDatum.Value));
                                else throw new ZeroOrderMVPException();
                            }
                        }
                        date = timeNextMethod(date);
                    }
                }

                else if (policy is FirstOrderMissingValuePolicy<T>)
                {
                    var newerData = getNewerDatum<T>(signal, fromIncluded);
                    var olderData = getOlderDatum<T>(signal, toExcluded);

                    dynamic step = 0;
                    while (date < toExcluded)
                    {
                        Datum<T> datum = null;

                        if (date < newerData.Timestamp || date > olderData.Timestamp)
                        {
                            datum = new Datum<T>() { Quality = Quality.None, Timestamp = new DateTime(), Value = default(T) };
                            newData.Add(datum);
                        }
                        
                        step = GetStep<T>(signal, newerData, olderData);
                        
                        if (date > newerData.Timestamp)
                        {
                            var previousDatum = newData.Last();
                            Quality quality;
                            if (previousDatum.Quality > olderData.Quality) quality = previousDatum.Quality; else quality = olderData.Quality;

                            datum = new Datum<T>() { Timestamp = previousDatum.Timestamp, Value = previousDatum.Value + step, Quality = quality };

                            newData.Add(datum);
                        }
                        date = timeNextMethod(date);
                    }
                }

                else
                    throw new NotImplementedException();

                return newData;
            }
            return result;
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
                //case Granularity.Week: timeDifference = nextDatum.Timestamp.Week - currentDatum.Timestamp.Week; break;  --------------------------------------!!!!!!!!!!!!!!!!!!!!!!!!!!!
                case Granularity.Month: timeDifference = nextDatum.Timestamp.Month - currentDatum.Timestamp.Month; break;
                case Granularity.Year: timeDifference = nextDatum.Timestamp.Year - currentDatum.Timestamp.Year; break;
            }

            var valuesDifference = (dynamic)nextDatum.Value - (dynamic)currentDatum.Value;
            if (timeDifference == 0) timeDifference = 1;
            return valuesDifference / timeDifference;
        }

        private Datum<T> getNewerDatum<T>(Signal signal, DateTime from)
        {
            var datum = signalsDataRepository.GetData<T>(signal, from, from);
            if (datum.Count() == 0) { datum = signalsDataRepository.GetDataNewerThan<T>(signal, from, 1); }
            return datum.FirstOrDefault();
        }
        private Datum<T> getOlderDatum<T>(Signal signal, DateTime to)
        {
            var datum = signalsDataRepository.GetData<T>(signal, to, to);
            if (datum.Count() == 0) { datum = signalsDataRepository.GetDataOlderThan<T>(signal, to, 1); }
            return datum.FirstOrDefault();
        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicy.MissingValuePolicyBase missingValuePolicy)
        {
            this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
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
