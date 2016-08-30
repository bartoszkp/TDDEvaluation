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

            var result = this.signalsRepository.Add(newSignal);

            switch (result.DataType)
            {
                case DataType.Boolean:
                    this.SetMissingValuePolicy(result, new NoneQualityMissingValuePolicy<bool>());
                    break;
                case DataType.Integer:
                    this.SetMissingValuePolicy(result, new NoneQualityMissingValuePolicy<int>());
                    break;
                case DataType.Double:
                    this.SetMissingValuePolicy(result, new NoneQualityMissingValuePolicy<double>());
                    break;
                case DataType.Decimal:
                    this.SetMissingValuePolicy(result, new NoneQualityMissingValuePolicy<decimal>());
                    break;
                case DataType.String:
                    this.SetMissingValuePolicy(result, new NoneQualityMissingValuePolicy<string>());
                    break;
                default:
                    break;
            }

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

        public void SetData<T>(IEnumerable<Datum<T>> data)
        {
            CheckTimestampCorrectness(data);
            this.signalsDataRepository.SetData(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            CheckTimestamp(fromIncludedUtc, signal.Granularity);


            Datum<T> lastDatum = null;
            List<Datum<T>> result;
            var missingValuePolicy = this.missingValuePolicyRepository.Get(signal);
            result = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).OrderBy(d => d.Timestamp).ToList();

            var olderData = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1);

            if (olderData.Count() == 1)
            {
                lastDatum = olderData.ToArray()[0];
            }


            if (result.Count == 0)
            {
                result = new List<Datum<T>>() { createDatumBaseOnMissingValuePolicy<T>(signal,fromIncludedUtc,missingValuePolicy,lastDatum )};

            }

           
            var date = fromIncludedUtc;

            while (date < toExcludedUtc)
            {
                var datum = (from x in result
                             where x.Timestamp == date
                             select x).FirstOrDefault();
                
                if (datum == null)
                {
                    Datum<T> tempDatum = createDatumBaseOnMissingValuePolicy<T>(signal, date, missingValuePolicy,lastDatum);
                    result.Add(tempDatum);
                    datum = tempDatum;
                }
                lastDatum = datum;
                date = AddTime(signal.Granularity, date);
            }

            CheckTimestampCorrectness(result.OrderBy(x => x.Timestamp).ToArray());
            return result.OrderBy(x => x.Timestamp);
        }

        private void CheckTimestamp(DateTime date, Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Year:
                    if (date.Month != 1)
                    {
                        throw new IncorrectDatumTimestampException();
                    }
                    goto case Granularity.Month;
                case Granularity.Month:
                    if (date.Day != 1)
                    {
                        throw new IncorrectDatumTimestampException();
                    }
                    goto case Granularity.Day;
                case Granularity.Week:
                    if (date.DayOfWeek!=DayOfWeek.Monday)
                    {
                        throw new IncorrectDatumTimestampException();
                    }
                    goto case Granularity.Day;
                case Granularity.Day:
                    if (date.Hour != 0)
                    {
                        throw new IncorrectDatumTimestampException();
                    }
                    goto case Granularity.Hour;
                case Granularity.Hour:
                    if (date.Minute != 0)
                    {
                        throw new IncorrectDatumTimestampException();
                    }
                    goto case Granularity.Minute;
                case Granularity.Minute:
                    if (date.Second != 0)
                    {
                        throw new IncorrectDatumTimestampException();
                    }
                    goto case Granularity.Second;
                case Granularity.Second:
                    if (date.Millisecond != 0)
                    {
                        throw new IncorrectDatumTimestampException();
                    }
                    break;
            }
        }

        private void CheckTimestampCorrectness<T>(IEnumerable<Datum<T>> data)
        {
            foreach(var match in data)
            {
                if(match.Signal == null)
                {
                    continue;
                }
                CheckTimestamp(match.Timestamp,match.Signal.Granularity);
            }
        }

        private Datum<T> createDatumBaseOnMissingValuePolicy<T>(Signal signal, DateTime date, MissingValuePolicyBase missingValuePolicy,Datum<T> lastDatum)
        {
            if (missingValuePolicy is NoneQualityMissingValuePolicy<T>)
            {
                return Domain.Datum<T>.CreateNone(signal,date);
            }
            else if (missingValuePolicy is SpecificValueMissingValuePolicy<T>)
            {
                return new Datum<T>
                {
                    Quality = (missingValuePolicy as SpecificValueMissingValuePolicy<T>).Quality,
                    Signal = signal,
                    Timestamp = date,
                    Value = (missingValuePolicy as SpecificValueMissingValuePolicy<T>).Value
                };
            }
            else if(missingValuePolicy is ZeroOrderMissingValuePolicy<T>)
            {
                if (lastDatum == null)
                {
                    return Domain.Datum<T>.CreateNone(signal, date);
                }
                else
                {
                    return new Datum<T>
                    {
                        Quality = lastDatum.Quality,
                        Signal = signal,
                        Timestamp = date,
                        Value = lastDatum.Value
                    };
                }
            }
            return new Datum<T>()
            {
                Signal = signal,
                Timestamp = date
            };
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var result = this.missingValuePolicyRepository.Get(signal);

            if (result == null)
                return null;

            return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase missingValuePolicy)
        {
            missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }

        private DateTime AddTime(Granularity granularity, DateTime date)
        {
            switch (granularity) {
                case Granularity.Second:
                    return date.AddSeconds(1);
                case Granularity.Minute:
                    return date.AddMinutes(1);
                case Granularity.Hour:
                    return date.AddHours(1);
                case Granularity.Day:
                    return date.AddDays(1);
                case Granularity.Week:
                    return date.AddDays(7);
                case Granularity.Month:
                    return date.AddMonths(1);
                case Granularity.Year:
                    return date.AddYears(1);
                default:
                    return date;
            }
        }

        public PathEntry GetPathEntry(Path domainPath)
        {
            var FindSignals = signalsRepository.GetAllWithPathPrefix(domainPath).ToList();

            List<Path> subPaths = new List<Path>();
            List<Signal> pathEntrySignals = new List<Signal>();

            for (int i = 0; i < FindSignals.Count(); i++)
            {
                var signalPath = string.Join("/", FindSignals[i].Path.Components);
                var domainPathString = string.Join("/", domainPath.Components);

                if (signalPath.Length<domainPathString.Length+1) continue;
                var pathWithoutDomainPath = signalPath.Remove(signalPath.IndexOf(domainPathString), domainPathString.Length + 1);

                string[] Components = pathWithoutDomainPath.Split('/');

                if (Components.Count() > 1)
                {
                    if (!subPaths.Contains(Path.FromString(domainPathString + "//" + Components[0])))
                    {
                        subPaths.Add(Path.FromString(domainPathString + "//" + Components[0]));
                    }
                }
                else
                {
                    pathEntrySignals.Add(signalsRepository.Get(FindSignals[i].Path));
                }
            }

            return new PathEntry(pathEntrySignals,subPaths);
        }

        public void Delete(Signal signal)
        {
            missingValuePolicyRepository.Set(signal, null);
            signalsRepository.Delete(signal);
        }

        public void DeleteData<T>(Signal signal)
        {
            signalsDataRepository.DeleteData<T>(signal);
        }
    }
}
