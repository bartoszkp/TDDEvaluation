using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
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
            var result =  this.signalsRepository.Add(newSignal);

            Type mvpType = typeof(MissingValuePolicy.NoneQualityMissingValuePolicy<>);
            mvpType = mvpType.MakeGenericType(result.DataType.GetNativeType());
            MissingValuePolicy.MissingValuePolicyBase mvp  = Activator.CreateInstance(mvpType) as MissingValuePolicy.MissingValuePolicyBase;
            SetMissingValuePolicy(result, mvp);

            return result;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path signalPath)
        {
            Signal signal = this.signalsRepository.Get(signalPath);

            return signal;
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            List<Datum<T>> dataList = data.ToList();
            for (int i =0;i < dataList.Count; ++i)
                dataList[i].Signal = signal;

            this.signalsDataRepository.SetData<T>(dataList);
        }

        private DateTime GetNextDateFromGranularity(DateTime current, Granularity granularity)
        {
            switch(granularity)
            {
                case Granularity.Second:
                    return current.AddSeconds(1);
                case Granularity.Minute:
                    return current.AddMinutes(1);
                case Granularity.Hour:
                    return current.AddHours(1);
                case Granularity.Day:
                    return current.AddDays(1);
                case Granularity.Week:
                    return current.AddDays(7);
                case Granularity.Month:
                    return current.AddMonths(1);
                case Granularity.Year:
                    return current.AddYears(1);
            }

            return current;
        }

        public IEnumerable<Domain.Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Signal signal = GetById(signalId);

            var data = this.signalsDataRepository
                .GetData<T>(signal, fromIncludedUtc, toExcludedUtc)
                .OrderBy(d => d.Timestamp).ToList();

            MissingValuePolicy.MissingValuePolicy<T> mvp = GetMissingValuePolicy(signal) as MissingValuePolicy.MissingValuePolicy<T>;
            DateTime current = fromIncludedUtc;

            int i = 0;
            while(current < toExcludedUtc)
            {
                if (i >= data.Count || data[i].Timestamp != current)
                    data.Add(mvp.GetMissingValue(signal, current));
                else
                    i++;

                current = GetNextDateFromGranularity(current, signal.Granularity);
            }

            return data.OrderBy(d => d.Timestamp);

        }

        public void SetMissingValuePolicy(Signal signal, Domain.MissingValuePolicy.MissingValuePolicyBase policy)
        {
            this.missingValuePolicyRepository.Set(signal,policy);
        }

        public MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var mvp = this.missingValuePolicyRepository.Get(signal);

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
               as MissingValuePolicy.MissingValuePolicyBase;
        }
    }
}
