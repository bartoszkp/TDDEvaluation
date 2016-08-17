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

            var res = this.signalsRepository.Add(newSignal);

            if (newSignal.DataType == DataType.Boolean) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<bool>());
            if (newSignal.DataType == DataType.Decimal) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<decimal>());
            if (newSignal.DataType == DataType.Double) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<double>());
            if (newSignal.DataType == DataType.Integer) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<int>());
            if (newSignal.DataType == DataType.String) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<string>());

            return res;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path path)
        {
            return this.signalsRepository.Get(path);
        }

        public IEnumerable<Signal> GetPathEntry(Path prefix)
        {
           IEnumerable<Signal> result = signalsRepository.GetAllWithPathPrefix(prefix);
           return result;
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

            return CheckMissingValues(signal, data, fromIncludedUtc, toExcludedUtc);
        }

        private IEnumerable<Datum<T>> CheckMissingValues<T>(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var newList = new List<Datum<T>>();

            switch(signal.Granularity)
            {
                case Granularity.Second:
                    return FillMissingRecords(dt => dt.AddSeconds(1), signal, data, fromIncludedUtc, toExcludedUtc);

                case Granularity.Minute:
                    return FillMissingRecords(dt => dt.AddMinutes(1), signal, data, fromIncludedUtc, toExcludedUtc);

                case Granularity.Hour:
                    return FillMissingRecords(dt => dt.AddHours(1), signal, data, fromIncludedUtc, toExcludedUtc);

                case Granularity.Day:
                    return FillMissingRecords(dt => dt.AddDays(1), signal, data, fromIncludedUtc, toExcludedUtc);

                case Granularity.Week:
                    return FillMissingRecords(dt => dt.AddDays(7), signal, data, fromIncludedUtc, toExcludedUtc);

                case Granularity.Month:
                    return FillMissingRecords(dt => dt.AddMonths(1), signal, data, fromIncludedUtc, toExcludedUtc);

                case Granularity.Year:
                    return FillMissingRecords(dt => dt.AddYears(1), signal, data, fromIncludedUtc, toExcludedUtc);

                default: return null;
            }
        }

        public IEnumerable<Datum<T>> FillMissingRecords<T>(Func<DateTime, DateTime> dateTimeStep, Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            List<Datum<T>> list = new List<Datum<T>>();
            for (DateTime d = fromIncludedUtc; d < toExcludedUtc; d = dateTimeStep(d))
            {
                FillMissingRecord(data, signal, d, ref list);
            }
            if (list.Count == 0 && fromIncludedUtc == toExcludedUtc)
            {
                FillMissingRecord(data, signal, fromIncludedUtc, ref list);
            }
            return list;
        }
        private void FillMissingRecord<T>(IEnumerable<Datum<T>> data, Signal signal, DateTime dateTime, ref List<Datum<T>> list)
        {
            var mvp = GetMissingValuePolicy(signal);

            Quality quality = Quality.None;
            T value = default(T);

            if (mvp is NoneQualityMissingValuePolicy<T>)
                        { quality = Quality.None; value = default(T); }
            if (mvp is SpecificValueMissingValuePolicy<T>)
                        { var SpecificMvp = mvp as SpecificValueMissingValuePolicy<T>; quality = SpecificMvp.Quality; value = SpecificMvp.Value; }
            
            if (data.Any(time => time.Timestamp == dateTime))
            {
                list.Add(data.First(t => t.Timestamp == dateTime));
            }
            else list.Add(new Datum<T>() { Quality = quality, Timestamp = dateTime, Value = value});
        }
        public void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase policy)
        {
            if (policy != null)
            {
                if (policy.Id.HasValue)
                    throw new IdNotNullException();
                if (policy.NativeDataType != signal.DataType.GetNativeType())
                    throw new TypeMismatchException();
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
