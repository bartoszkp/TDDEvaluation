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
            bool isFromIncludedUtcCorrect = CheckCorrectnessOfDate(signal, fromIncludedUtc);

            if (!isFromIncludedUtcCorrect) throw new ArgumentException();

            var data = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).OrderBy(datum => datum.Timestamp);

            var mvp = GetMissingValuePolicy(signal);

            IEnumerable<Datum<T>> filledData = data;

            if (mvp != null)
                filledData = CheckMissingValues(signal, data, fromIncludedUtc, toExcludedUtc);
            
            return filledData;
        }

        private IEnumerable<Datum<object>> FillDataWhenGivenIsZeroOrderMvp()
        {
            throw new NotImplementedException();
        }

        private bool CheckCorrectnessOfDate(Signal signal, DateTime fromIncludedUtc)
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
            foreach (var datum in data)
                if (!CheckCorrectnessOfDate(signal, datum.Timestamp))
                    return false;

            return true;
        }


        private IEnumerable<Datum<T>> CheckMissingValues<T>(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            switch (signal.Granularity)
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
            var mvp = GetMissingValuePolicy(signal) as MissingValuePolicy<T>;

            if (fromIncludedUtc == toExcludedUtc)
                return new[] { data.FirstOrDefault(datum => datum.Timestamp == fromIncludedUtc)
                                ?? FillMissingRecord(data, signal, fromIncludedUtc, mvp) };

            List<Datum<T>> list = new List<Datum<T>>();

            for (DateTime d = fromIncludedUtc; d < toExcludedUtc; d = dateTimeStep(d))
                list.Add(
                    data.FirstOrDefault(datum => datum.Timestamp == d)
                    ?? FillMissingRecord(data, signal, d, mvp));

            return list;
        }
        private Datum<T> FillMissingRecord<T>(IEnumerable<Datum<T>> data, Signal signal, DateTime dateTime, MissingValuePolicy<T> mvp)
        {
            if (mvp is NoneQualityMissingValuePolicy<T>)
            {
                return Datum<T>.CreateNone(signal, dateTime);
            }
            else if (mvp is SpecificValueMissingValuePolicy<T>)
            {
                var specificMvp = mvp as SpecificValueMissingValuePolicy<T>;
                return new Datum<T>
                {
                    Quality = specificMvp.Quality,
                    Value = specificMvp.Value,
                    Signal = signal,
                    Timestamp = dateTime
                };
            }
            else if (mvp is ZeroOrderMissingValuePolicy<T>)
            {
                var previous = data.LastOrDefault(datum => datum.Timestamp < dateTime)
                    ?? signalsDataRepository.GetDataOlderThan<T>(signal, dateTime, 1).SingleOrDefault()
                    ?? Datum<T>.CreateNone(signal, dateTime);

                return new Datum<T>
                {
                    Quality = previous.Quality,
                    Value = previous.Value,
                    Signal = signal,
                    Timestamp = dateTime
                };
            }
            else return Datum<T>.CreateNone(signal, dateTime);
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
