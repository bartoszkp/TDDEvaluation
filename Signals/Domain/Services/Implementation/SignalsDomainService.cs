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
            var signal = this.signalsRepository.Add(newSignal);

            var typeOfSignal = signal.DataType;

            switch (typeOfSignal)
            {
                case DataType.Boolean:
                    missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<bool>());
                    break;
                case DataType.Integer:
                    missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<int>());
                    break;
                case DataType.Double:
                    missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<double>());
                    break;
                case DataType.Decimal:
                    missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<decimal>());
                    break;
                case DataType.String:
                    missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<string>());
                    break;
                default:
                    break;
            }

            return signal;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path path)
        {
            return this.signalsRepository.Get(path);
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId);

            var result = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            var resultArray = result.OrderBy(datum => datum.Timestamp).ToArray();

            var mvp = GetMissingValuePolicy(signalId);

            if (mvp != null && mvp.GetType().GetGenericTypeDefinition() == typeof(NoneQualityMissingValuePolicy<>))
                resultArray = FillArray(resultArray, fromIncludedUtc, toExcludedUtc);

            return resultArray;
        }

        private Datum<T>[] FillArray<T>(Datum<T>[] array, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (array.Length < 1)
                return array;

            var signal = array.First().Signal;
            DateTime tmp;
            var dateModifier = DateModifier(signal.Granularity);
            var filledArray = new Datum<T>[NumberOfPeriods(fromIncludedUtc, toExcludedUtc, signal.Granularity)];

            tmp = fromIncludedUtc;
            int j = 0;
            for (int i = 0; i < filledArray.Length; i++)
            {
                if (array[j].Timestamp == tmp)
                {
                    filledArray[i] = array[j];
                    j = (j + 1) % array.Length;
                }
                else
                    filledArray[i] = Datum<T>.CreateNone(signal, tmp);
                tmp = dateModifier(tmp);
            }

            return filledArray;
        }

        private Func<DateTime, DateTime> DateModifier(Granularity granularity)
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
                    return (date) => date;
            }
        }

        private int NumberOfPeriods(DateTime fromIncludedUtc, DateTime toExcludedUtc, Granularity granularity)
        {
            var timeSpan = toExcludedUtc - fromIncludedUtc;
            switch (granularity)
            {
                case Granularity.Second:
                    return (int)timeSpan.TotalSeconds;
                case Granularity.Minute:
                    return (int)timeSpan.TotalMinutes;
                case Granularity.Hour:
                    return (int)timeSpan.TotalHours;
                case Granularity.Day:
                    return (int)timeSpan.TotalDays;
                case Granularity.Week:
                    return (int)(timeSpan.TotalDays / 7);
                case Granularity.Month:
                    return (toExcludedUtc.Month - fromIncludedUtc.Month) + 12 * (toExcludedUtc.Year - fromIncludedUtc.Year);
                case Granularity.Year:
                    return toExcludedUtc.Year - fromIncludedUtc.Year;
                default:
                    return 0;
            }
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> enumerable)
        {
            var signal = GetById(signalId);

            foreach (var item in enumerable)
                item.Signal = signal;

            signalsDataRepository.SetData(enumerable);
        }
        
        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = signalsRepository.Get(signalId);

            var mvp = missingValuePolicyRepository.Get(signal);

            if (mvp == null) return null;

            else return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase missingValuePolicyBase)
        {
            var signal = signalsRepository.Get(signalId);

            missingValuePolicyRepository.Set(signal, missingValuePolicyBase);
        }
    }
}