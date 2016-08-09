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
            var result = this.signalsRepository.Add(newSignal);

            SetDefaultMissingValuePolicy(result);

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

        public void SetData<T>(IEnumerable<Datum<T>> domain_data)
        {
            this.signalsDataRepository.SetData(domain_data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded)
        {
            var result = this.signalsDataRepository.GetData<T>(signal, fromIncluded, toExcluded);

            var policy = GetMissingValuePolicy(signal);

            if (policy != null)
            {
                var timediff = toExcluded - fromIncluded;
                TimeSpan span;
                int remaining;
                if (signal.Granularity == Granularity.Day)
                    span = new TimeSpan(1, 0, 0, 0);
                else if (signal.Granularity == Granularity.Second)
                    span = new TimeSpan(0, 0, 1);
                else if (signal.Granularity == Granularity.Minute)
                    span = new TimeSpan(0, 1, 0);
                else if (signal.Granularity == Granularity.Hour)
                    span = new TimeSpan(1, 0, 0);
                else if (signal.Granularity == Granularity.Week)
                    span = new TimeSpan(7, 0, 0, 0);
                else if (signal.Granularity == Granularity.Month)
                    span = new TimeSpan(30, 0, 0, 0);
                else
                    span = new TimeSpan(365, 0, 0, 0);
                remaining = (int)(timediff.Ticks / span.Ticks);
                var dates = Enumerable.Range(0, remaining).Select(i => fromIncluded + new TimeSpan(span.Ticks * i));
                dates = dates.Except(dates.Intersect(result.Select(d => d.Timestamp)));
                return result.Union(dates.Select(date => Datum<T>.CreateNone(signal, date)));
            }
            return result;
        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicy.MissingValuePolicyBase missingValuePolicy)
        {
            this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }

        public MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var mvp = this.missingValuePolicyRepository.Get(signal);
            if (mvp == null)
                return null;

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        private void SetDefaultMissingValuePolicy(Signal signal)
        {
            var policy = MissingPolicyValueFromType(signal.DataType, typeof(MissingValuePolicy.NoneQualityMissingValuePolicy<>));

            SetMissingValuePolicy(signal, policy);
        }

        private MissingValuePolicy.MissingValuePolicyBase MissingPolicyValueFromType(DataType dataType, Type type)
        {
            var genericType = type.MakeGenericType(dataType.GetNativeType());

            return (MissingValuePolicy.MissingValuePolicyBase)Activator.CreateInstance(genericType);
        }
    }
}
