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
            this.signalsDataRepository.SetData(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            bool skipFirst = false;
            List<Datum<T>> result;
            result = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).OrderBy(d => d.Timestamp).ToList();
            if (result.Count == 0)
            {
                result = new List<Datum<T>>() { new Datum<T>() };
                skipFirst = true;
            }

            var missingValuePolicy = this.missingValuePolicyRepository.Get(signal);
            var date = fromIncludedUtc;

            while (date < toExcludedUtc)
            {
                var datum = (from x in result
                             where x.Timestamp == date
                             select x).FirstOrDefault();

                if (datum == null)
                {
                    Datum<T> tempDatum = createDatumBaseOnMissingValuePolicy<T>(signal, date, missingValuePolicy);
                    result.Add(tempDatum);
                }
                date = AddTime(signal.Granularity, date);
            }

            if (skipFirst == true) return result.OrderBy(x => x.Timestamp).Skip(1);
            return result.OrderBy(x => x.Timestamp);
        }

        private Datum<T> createDatumBaseOnMissingValuePolicy<T>(Signal signal, DateTime date, MissingValuePolicyBase missingValuePolicy)
        {
            if (missingValuePolicy is NoneQualityMissingValuePolicy<T>)
            {
                return new Datum<T>
                {
                    Quality = Quality.None,
                    Signal = signal,
                    Timestamp = date,
                    Value = default(T)
                };
            }
            if (missingValuePolicy is SpecificValueMissingValuePolicy<T>)
            {
                return new Datum<T>
                {
                    Quality = (missingValuePolicy as SpecificValueMissingValuePolicy<T>).Quality,
                    Signal = signal,
                    Timestamp = date,
                    Value = (missingValuePolicy as SpecificValueMissingValuePolicy<T>).Value
                };
            }
            return new Datum<T>();
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
            this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }

        private DateTime AddTime(Granularity granuality, DateTime date)
        {
            if (granuality == Granularity.Second) return date.AddSeconds(1);
            if (granuality == Granularity.Minute) return date.AddMinutes(1);
            if (granuality == Granularity.Hour) return date.AddHours(1);
            if (granuality == Granularity.Day) return date.AddDays(1);
            if (granuality == Granularity.Week) return date.AddDays(7);
            if (granuality == Granularity.Month) return date.AddMonths(1);
            if (granuality == Granularity.Year) return date.AddYears(1);
            return date;
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
                    if (!subPaths.Contains(Path.FromString(Components[0]))) subPaths.Add(Path.FromString(Components[0])); 
                    
                }
                else
                {
                    pathEntrySignals.Add(signalsRepository.Get(FindSignals[i].Path));
                }
            }

            return new PathEntry(pathEntrySignals,subPaths);
        }
    }
}
