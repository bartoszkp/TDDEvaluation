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
            bool areDataTimestampsCorrect = CheckCorrectnessOfDataTimestamps(signal, data);

            if (!areDataTimestampsCorrect) throw new ArgumentException();

            data = data.Select(d =>
            {
                d.Signal = signal;
                return d;
            }).ToList();

            this.signalsDataRepository.SetData(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            bool isFromIncludedUtcCorrect = CheckCorrectnessOfFromIncludedUtc(signal, fromIncludedUtc);

            if (!isFromIncludedUtcCorrect) throw new ArgumentException();

            var data = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            var mvp = missingValuePolicyRepository.Get(signal);

            IEnumerable<Datum<T>> filledData = data;

            if (mvp is NoneQualityMissingValuePolicy<T> | mvp is SpecificValueMissingValuePolicy<T> )
                filledData = CheckMissingValues(signal, data, fromIncludedUtc, toExcludedUtc);

            if (mvp is ZeroOrderMissingValuePolicy<T>) filledData = (mvp as ZeroOrderMissingValuePolicy<T>).SetMissingValue(signal, data, fromIncludedUtc, toExcludedUtc);

            return filledData;
        }

        private IEnumerable<Datum<object>> FillDataWhenGivenIsZeroOrderMvp()
        {
            throw new NotImplementedException();
        }

        private bool CheckCorrectnessOfFromIncludedUtc(Signal signal, DateTime fromIncludedUtc)
        {
            switch (signal.Granularity)
            {
                case Granularity.Second:
                    if (fromIncludedUtc.Millisecond != 0)
                        return false;
                    break;
                case Granularity.Minute:
                    if (fromIncludedUtc.Millisecond != 0 |
                        fromIncludedUtc.Second != 0)
                        return false;
                    break;
                case Granularity.Hour:
                    if (fromIncludedUtc.Millisecond != 0 |
                        fromIncludedUtc.Second != 0 |
                        fromIncludedUtc.Minute != 0)
                        return false;
                    break;
                case Granularity.Day:
                    if (fromIncludedUtc.Millisecond != 0 |
                        fromIncludedUtc.Second != 0 |
                        fromIncludedUtc.Minute != 0 |
                        fromIncludedUtc.Hour != 0)
                        return false;
                    break;
                case Granularity.Week:
                    if (fromIncludedUtc.Millisecond != 0 |
                        fromIncludedUtc.Second != 0 |
                        fromIncludedUtc.Minute != 0 |
                        fromIncludedUtc.Hour != 0 |
                        fromIncludedUtc.DayOfWeek != DayOfWeek.Monday)
                        return false;
                    break;
                case Granularity.Month:
                    if (fromIncludedUtc.Millisecond != 0 |
                        fromIncludedUtc.Second != 0 |
                        fromIncludedUtc.Minute != 0 |
                        fromIncludedUtc.Hour != 0 |
                        fromIncludedUtc.Day != 1)
                        return false;
                    break;
                case Granularity.Year:
                    if (fromIncludedUtc.Millisecond != 0 |
                        fromIncludedUtc.Second != 0 |
                        fromIncludedUtc.Minute != 0 |
                        fromIncludedUtc.Hour != 0 |
                        fromIncludedUtc.Day != 1 |
                        fromIncludedUtc.Month != 1)
                        return false;
                    break;
                default:
                    break;
            }

            return true;
        }

        private bool CheckCorrectnessOfDataTimestamps<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            switch (signal.Granularity)
            {
                case Granularity.Second:
                    foreach (var datum in data)
                    {
                        if (datum.Timestamp.Millisecond != 0)
                            return false;
                    }
                    break;
                case Granularity.Minute:
                    foreach (var datum in data)
                    {
                        if (datum.Timestamp.Millisecond != 0 |
                            datum.Timestamp.Second != 0)
                            return false;
                    }
                    break;
                case Granularity.Hour:
                    foreach (var datum in data)
                    {
                        if (datum.Timestamp.Millisecond != 0 |
                            datum.Timestamp.Second != 0 |
                            datum.Timestamp.Minute != 0)
                            return false;
                    }
                    break;
                case Granularity.Day:
                    foreach (var datum in data)
                    {
                        if (datum.Timestamp.Millisecond != 0 |
                            datum.Timestamp.Second != 0 |
                            datum.Timestamp.Minute != 0 |
                            datum.Timestamp.Hour != 0)
                            return false;
                    }
                    break;
                case Granularity.Week:
                    foreach (var datum in data)
                    {
                        if (datum.Timestamp.Millisecond != 0 |
                            datum.Timestamp.Second != 0 |
                            datum.Timestamp.Minute != 0 |
                            datum.Timestamp.Hour != 0 |
                            datum.Timestamp.DayOfWeek != DayOfWeek.Monday)
                            return false;
                    }
                    break;
                case Granularity.Month:
                    foreach (var datum in data)
                    {
                        if (datum.Timestamp.Millisecond != 0 |
                            datum.Timestamp.Second != 0 |
                            datum.Timestamp.Minute != 0 |
                            datum.Timestamp.Hour != 0 |
                            datum.Timestamp.Day != 1)
                            return false;
                    }
                    break;
                case Granularity.Year:
                    foreach (var datum in data)
                    {
                        if (datum.Timestamp.Millisecond != 0 |
                            datum.Timestamp.Second != 0 |
                            datum.Timestamp.Minute != 0 |
                            datum.Timestamp.Hour != 0 |
                            datum.Timestamp.Day != 1 |
                            datum.Timestamp.Month != 1)
                            return false;
                    }
                    break;
                default:
                    break;
            }

            return true;
        }

       


        private IEnumerable<Datum<T>> CheckMissingValues<T>(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
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
