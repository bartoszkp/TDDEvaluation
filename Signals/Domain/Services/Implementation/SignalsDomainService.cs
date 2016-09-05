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
                throw new IdNotNullException();            

            var res = this.signalsRepository.Add(newSignal);
            
            if (newSignal.DataType == DataType.Boolean) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<bool>());
            else if (newSignal.DataType == DataType.Decimal) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<decimal>());
            else if (newSignal.DataType == DataType.Double) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<double>());
            else if (newSignal.DataType == DataType.Integer) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<int>());
            else if (newSignal.DataType == DataType.String) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<string>());

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

        public void Delete(int signalId)
        {
            var signal = GetById(signalId);

            SetMissingValuePolicy(signal, null);
            DeleteData(signal);

            signalsRepository.Delete(signal);
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            bool areDataTimestampsCorrect = CheckCorrectnessOfDataTimestamps(signal.Granularity, data);

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
            bool isFromIncludedUtcCorrect = CheckCorrectnessOfDate(signal.Granularity, fromIncludedUtc);

            if (!isFromIncludedUtcCorrect) throw new ArgumentException();

            var data = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).OrderBy(datum => datum.Timestamp);

            var mvp = GetMissingValuePolicy(signal);

            IEnumerable<Datum<T>> filledData = data;

            if (mvp != null)
                filledData = FillMissingRecords(signal, data, fromIncludedUtc, toExcludedUtc);

            return filledData;
        }        

        public IEnumerable<Datum<T>> FillMissingRecords<T>(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var mvp = GetMissingValuePolicy(signal) as MissingValuePolicy<T>;
            var dateTimeStep = GetTimeStepFunction(signal.Granularity);

            if (fromIncludedUtc == toExcludedUtc)
                return new[] { data.FirstOrDefault(datum => datum.Timestamp == fromIncludedUtc)
                                ?? FillMissingRecord(data, signal, fromIncludedUtc, mvp, dateTimeStep) };

            List<Datum<T>> list = new List<Datum<T>>();            

            for (DateTime d = fromIncludedUtc; d < toExcludedUtc; d = dateTimeStep(d))
                list.Add(
                    data.FirstOrDefault(datum => datum.Timestamp == d)
                    ?? FillMissingRecord(data, signal, d, mvp, dateTimeStep));

            return list;
        }
        private Datum<T> FillMissingRecord<T>(IEnumerable<Datum<T>> data, Signal signal, DateTime dateTime, MissingValuePolicy<T> mvp, Func<DateTime, DateTime> dateTimeStep)
        {
            if (mvp is NoneQualityMissingValuePolicy<T> || mvp is SpecificValueMissingValuePolicy<T>)            
                return mvp.GetMissingValue(signal, dateTime);            
            else if (mvp is ZeroOrderMissingValuePolicy<T>)
            {
                var previous = data.LastOrDefault(datum => datum.Timestamp < dateTime)
                    ?? signalsDataRepository.GetDataOlderThan<T>(signal, dateTime, 1).SingleOrDefault();

                return mvp.GetMissingValue(signal, dateTime, previous);
            }
            else if (mvp is FirstOrderMissingValuePolicy<T>)
            {
                if (typeof(T) == typeof(string) || typeof(T) == typeof(bool))
                    throw new InvalidPolicyDataTypeException(typeof(T));

                var previous = data.LastOrDefault(datum => datum.Timestamp < dateTime)
                    ?? signalsDataRepository.GetDataOlderThan<T>(signal, dateTime, 1).SingleOrDefault();
                var next = data.FirstOrDefault(datum => datum.Timestamp > dateTime)
                    ?? signalsDataRepository.GetDataNewerThan<T>(signal, dateTime, 1).SingleOrDefault();

                return mvp.GetMissingValue(signal, dateTime, previous, next);
            }
            else return Datum<T>.CreateNone(signal, dateTime);
        }

        private void DeleteData(Signal signal)
        {
            var type = signal.DataType.GetNativeType();
            var methodInfo = typeof(ISignalsDataRepository).GetMethod("DeleteData").MakeGenericMethod(type);
            methodInfo.Invoke(signalsDataRepository, new[] { signal });
        }

        private bool CheckCorrectnessOfDate(Granularity granularity, DateTime fromIncludedUtc)
        {
            switch (granularity)
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

        private bool CheckCorrectnessOfDataTimestamps<T>(Granularity granularity, IEnumerable<Datum<T>> data)
        {
            foreach (var datum in data)
                if (!CheckCorrectnessOfDate(granularity, datum.Timestamp))
                    return false;

            return true;
        }

        public static Func<DateTime, DateTime> GetTimeStepFunction(Granularity granularity)
        {
            switch(granularity)
            {
                case Granularity.Day:
                    return d => d.AddDays(1);
                case Granularity.Hour:
                    return d => d.AddHours(1);
                case Granularity.Minute:
                    return d => d.AddMinutes(1);
                case Granularity.Month:
                    return d => d.AddMonths(1);
                case Granularity.Second:
                    return d => d.AddSeconds(1);
                case Granularity.Week:
                    return d => d.AddDays(7);
                case Granularity.Year:
                    return d => d.AddYears(1);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
