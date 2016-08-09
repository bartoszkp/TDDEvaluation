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
            var result = signalsRepository.Add(newSignal);
            switch(result.DataType)
            {
                case (DataType.Boolean):
                    SetMissingValuePolicy(result.Id.Value, new NoneQualityMissingValuePolicy<bool>());
                    break;

                case (DataType.Decimal):
                    SetMissingValuePolicy(result.Id.Value, new NoneQualityMissingValuePolicy<decimal>());
                    break;

                case (DataType.Double):
                    SetMissingValuePolicy(result.Id.Value, new NoneQualityMissingValuePolicy<double>());
                    break;

                case (DataType.Integer):
                    SetMissingValuePolicy(result.Id.Value, new NoneQualityMissingValuePolicy<int>());
                    break;

                case (DataType.String):
                    SetMissingValuePolicy(result.Id.Value, new NoneQualityMissingValuePolicy<string>());
                    break;
            }
            return result;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path signalPath)
        {
            if (signalPath == null)
                throw new ArgumentNullException("Attempted to get signal with null path");

            var result = this.signalsRepository.Get(signalPath);
            return result;
        }
        
        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = this.GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException("Cannot get missing value policy for not exisitng signal");

            var mvp = this.missingValuePolicyRepository.Get(signal);

            if (mvp == null)
                return null;

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase policy)
        {
            var signal = GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException("Attempted to set missing value policy to a non exsisting signal");
            this.missingValuePolicyRepository.Set(signal, policy);

        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var result = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();
            if (result == null)
                return null;

            for (int j = result.Length-1; j>0; --j)
                for (int i=0; i<j; ++i)
                    if (result[i].Timestamp > result[i+1].Timestamp)
                    {
                        var r = result[i];
                        result[i] = result[i + 1];
                        result[i + 1] = r;
                    }

            var mvp = GetMissingValuePolicy(signal.Id.Value);
            if (mvp != null && typeof(NoneQualityMissingValuePolicy<T>) == mvp.GetType())
            {
                List<Datum<T>> datums = new List<Datum<T>>();
                var date = DateTime.MinValue;
                while (date < fromIncludedUtc)
                    increaseDate(ref date, signal.Granularity);

                for (int i = 0; date<toExcludedUtc && i<result.Length; increaseDate(ref date, signal.Granularity))
                {
                    if (result[i].Timestamp == date)
                        datums.Add(result[i++]);
                    else
                        datums.Add(new Datum<T>() { Quality = Quality.None, Timestamp = date, Value = default(T) });
                }
                for (; date < toExcludedUtc; increaseDate(ref date, signal.Granularity))
                    datums.Add(new Datum<T>() { Quality = Quality.None, Timestamp = date, Value = default(T) });

                return datums?.ToArray();
            }

            return result;
        }

        public void SetData<T>(IEnumerable<Datum<T>> data, Signal signal)
        {
            if (data == null)
                throw new ArgumentNullException("Attempted to set null data for a signal");

            SetSignalForDatumCollection(data, signal);

            signalsDataRepository.SetData(data);
        }

        private void SetSignalForDatumCollection<T>(IEnumerable<Domain.Datum<T>> data, Signal signal)
        {
            if (!data.Any() || signal == null)
                return;

            foreach (var datum in data)
            {
                datum.Signal = signal;
            }
        }

        private void increaseDate(ref DateTime date, Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    date = date.AddSeconds(1);
                    return;

                case Granularity.Minute:
                    date = date.AddMinutes(1);
                    return;

                case Granularity.Hour:
                    date = date.AddHours(1);
                    return;

                case Granularity.Day:
                    date = date.AddDays(1);
                    return;

                case Granularity.Week:
                    date = date.AddDays(7);
                    return;

                case Granularity.Month:
                    date = date.AddMonths(1);
                    return;

                case Granularity.Year:
                    date = date.AddYears(1);
                    return;
            }
            throw new NotSupportedException("Granularity: " + granularity.ToString() + " is not supported");
        }

    }
}
