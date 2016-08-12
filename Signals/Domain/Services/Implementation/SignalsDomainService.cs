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
            var signal =  this.signalsRepository.Add(newSignal);

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
            var signal = signalsRepository.Get(signalId);

            var result = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            var resultArray = result.ToArray();

            SortArrayByTimestamp(resultArray);

            var mvp = GetMissingValuePolicy(signalId);

            if (mvp is NoneQualityMissingValuePolicy<bool> ||
                mvp is NoneQualityMissingValuePolicy<int> ||
                mvp is NoneQualityMissingValuePolicy<double> || 
                mvp is NoneQualityMissingValuePolicy<decimal> ||
                mvp is NoneQualityMissingValuePolicy<string>) resultArray = FillArray(resultArray, fromIncludedUtc, toExcludedUtc);
            
            return resultArray;
        }

        private Datum<T>[] FillArray<T>(Datum<T>[] array, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var granularity = array[0].Signal.Granularity;
            Datum<T>[] filledArray = null;
            int timeSpan;
            DateTime tmp;

            switch (granularity)
            {
                case Granularity.Second:
                    timeSpan = toExcludedUtc.Second - fromIncludedUtc.Second;
                    filledArray = new Datum<T>[timeSpan];
                    tmp = fromIncludedUtc;
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>();
                        filledArray[i].Quality = Quality.None;
                        filledArray[i].Timestamp = tmp;
                        filledArray[i].Value = default(T);
                        tmp = new DateTime(tmp.Year, tmp.Month, tmp.Day, tmp.Hour, tmp.Minute, tmp.Second+1);
                    }
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (filledArray[i].Timestamp == array[j].Timestamp) filledArray[i] = array[j];
                        }
                    }
                    break;
                case Granularity.Minute:
                    timeSpan = toExcludedUtc.Minute - fromIncludedUtc.Minute;
                    filledArray = new Datum<T>[timeSpan];
                    tmp = fromIncludedUtc;
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>();
                        filledArray[i].Quality = Quality.None;
                        filledArray[i].Timestamp = tmp;
                        filledArray[i].Value = default(T);
                        tmp = new DateTime(tmp.Year, tmp.Month, tmp.Day, tmp.Hour, tmp.Minute+1, tmp.Second);
                    }
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (filledArray[i].Timestamp == array[j].Timestamp) filledArray[i] = array[j];
                        }
                    }
                    break;
                case Granularity.Hour:
                    timeSpan = toExcludedUtc.Hour - fromIncludedUtc.Hour;
                    filledArray = new Datum<T>[timeSpan];
                    tmp = fromIncludedUtc;
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>();
                        filledArray[i].Quality = Quality.None;
                        filledArray[i].Timestamp = tmp;
                        filledArray[i].Value = default(T);
                        tmp = new DateTime(tmp.Year, tmp.Month, tmp.Day, tmp.Hour+1, tmp.Minute, tmp.Second);
                    }
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (filledArray[i].Timestamp == array[j].Timestamp) filledArray[i] = array[j];
                        }
                    }
                    break;
                case Granularity.Day:
                    timeSpan = toExcludedUtc.Day - fromIncludedUtc.Day;
                    filledArray = new Datum<T>[timeSpan];
                    tmp = fromIncludedUtc;
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>();
                        filledArray[i].Quality = Quality.None;
                        filledArray[i].Timestamp = tmp;
                        filledArray[i].Value = default(T);
                        tmp = new DateTime(tmp.Year, tmp.Month, tmp.Day+1, tmp.Hour, tmp.Minute, tmp.Second);
                    }
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (filledArray[i].Timestamp == array[j].Timestamp) filledArray[i] = array[j];
                        }
                    }
                    break;
                case Granularity.Week:
                    timeSpan = toExcludedUtc.Day - fromIncludedUtc.Day / 7;
                    filledArray = new Datum<T>[timeSpan];
                    tmp = fromIncludedUtc;
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>();
                        filledArray[i].Quality = Quality.None;
                        filledArray[i].Timestamp = tmp;
                        filledArray[i].Value = default(T);
                        tmp = new DateTime(tmp.Year, tmp.Month, tmp.Day+7, tmp.Hour, tmp.Minute, tmp.Second);
                    }
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (filledArray[i].Timestamp == array[j].Timestamp) filledArray[i] = array[j];
                        }
                    }
                    break;
                case Granularity.Month:
                    timeSpan = toExcludedUtc.Month - fromIncludedUtc.Month;
                    filledArray = new Datum<T>[timeSpan];
                    tmp = fromIncludedUtc;
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>();
                        filledArray[i].Quality = Quality.None;
                        filledArray[i].Timestamp = tmp;
                        filledArray[i].Value = default(T);
                        tmp = new DateTime(tmp.Year, tmp.Month + 1, tmp.Day);                        
                    }
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (filledArray[i].Timestamp == array[j].Timestamp) filledArray[i] = array[j];
                        }
                    }
                    break;
                case Granularity.Year:
                    timeSpan = toExcludedUtc.Year - fromIncludedUtc.Year;
                    filledArray = new Datum<T>[timeSpan];
                    tmp = fromIncludedUtc;
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>();
                        filledArray[i].Quality = Quality.None;
                        filledArray[i].Timestamp = tmp;
                        filledArray[i].Value = default(T);
                        tmp = new DateTime(tmp.Year+1, tmp.Month, tmp.Day);
                    }
                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (filledArray[i].Timestamp == array[j].Timestamp) filledArray[i] = array[j];
                        }
                    }
                    break;
                default:
                    break;
            }

            return filledArray;
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> enumerable)
        {
            var signal = signalsRepository.Get(signalId);

            foreach (var item in enumerable)
            {
                item.Signal = signal;
            }

            Type type = null;

            foreach (var item in enumerable)
            {
                type = item.Value.GetType();
            }

            if (type == typeof(bool)) signalsDataRepository.SetData<bool>(enumerable as IEnumerable<Datum<bool>>);
            else if (type == typeof(int)) signalsDataRepository.SetData<int>(enumerable as IEnumerable<Datum<int>>);
            else if (type == typeof(double)) signalsDataRepository.SetData<double>(enumerable as IEnumerable<Datum<double>>);
            else if (type == typeof(decimal)) signalsDataRepository.SetData<decimal>(enumerable as IEnumerable<Datum<decimal>>);
            else if (type == typeof(string)) signalsDataRepository.SetData<string>(enumerable as IEnumerable<Datum<string>>);
            else throw new ArgumentException("Type of the 'data' parameter's signals must be bool, int, double, decimal or string.");
        }

        

        private void SortArrayByTimestamp<T>(Datum<T>[] array)
        {
            while (true)
            {
                bool changed = false;
                for (int i = 0; i < array.Length - 1; i++)
                {
                    var tmp = array[i];
                    if (array[i].Timestamp > array[i + 1].Timestamp)
                    {
                        array[i] = array[i + 1];
                        array[i + 1] = tmp;
                        changed = true;
                    }
                }
                if (!changed) break;
            }
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