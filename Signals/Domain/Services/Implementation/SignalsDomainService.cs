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

        public Signal GetByPath(Path path)
        {
            return this.signalsRepository.Get(path);
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            data = data.Select(d =>
            {
                d.Signal = signal;
                return d;
            }).ToList();

            this.signalsDataRepository.SetData(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var data =  this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            if (GetMissingValuePolicy(signal) != null && GetMissingValuePolicy(signal).GetType().Name.Contains("NoneQualityMissingValuePolicy"))
            {
                return CheckMissingValues(signal, data, fromIncludedUtc, toExcludedUtc);
            }
            else return this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
        }

        private IEnumerable<Datum<T>> CheckMissingValues<T>(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var newList = new List<Datum<T>>();

            switch(signal.Granularity)
            {
                case Granularity.Second:
                    return FillMissingRecords(dt => dt.AddSeconds(1), data, fromIncludedUtc, toExcludedUtc);

                case Granularity.Minute:
                    return FillMissingRecords(dt => dt.AddMinutes(1), data, fromIncludedUtc, toExcludedUtc);

                case Granularity.Hour:
                    return FillMissingRecords(dt => dt.AddHours(1), data, fromIncludedUtc, toExcludedUtc);

                case Granularity.Day:
                    return FillMissingRecords(dt => dt.AddDays(1), data, fromIncludedUtc, toExcludedUtc);

                case Granularity.Week:
                    return FillMissingRecords(dt => dt.AddDays(7), data, fromIncludedUtc, toExcludedUtc);

                case Granularity.Month:
                    return FillMissingRecords(dt => dt.AddMonths(1), data, fromIncludedUtc, toExcludedUtc);

                case Granularity.Year:
                    return FillMissingRecords(dt => dt.AddYears(1), data, fromIncludedUtc, toExcludedUtc);

                default: return null;
            }
        }

        public IEnumerable<Datum<T>> FillMissingRecords<T>(Func<DateTime, DateTime> dateTimeStep, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            List<Datum<T>> list = new List<Datum<T>>();
            for (DateTime d = fromIncludedUtc; d < toExcludedUtc; d = dateTimeStep(d))
            {
                if (data.Any(time => time.Timestamp == d))
                {
                    list.Add(data.First(t => t.Timestamp == d));
                }
                else list.Add(new Datum<T>() { Quality = Quality.None, Timestamp = d, Value = default(T) });
            }
            return list;
        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase policy)
        {
            if (policy != null)
            {
                if (policy.Id.HasValue)
                    throw new IdNotNullException();

                policy.Signal = signal;
            }
            this.missingValuePolicyRepository.Set(signal, policy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var result = this.missingValuePolicyRepository.Get(signal);
            if (result != null)
                return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType)
                    as MissingValuePolicy.MissingValuePolicyBase;
            else
                return null;
        }
    }
}
