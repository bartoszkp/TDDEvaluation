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
                int remaining;
                if (signal.Granularity == Granularity.Day)
                    remaining = (int)(timediff.Ticks / (new TimeSpan(1, 0, 0, 0).Ticks));
                else if (signal.Granularity == Granularity.Second)
                    remaining = (int)(timediff.Ticks / (new TimeSpan(0, 0, 1).Ticks));
                else if (signal.Granularity == Granularity.Minute)
                    remaining = (int)(timediff.Ticks / (new TimeSpan(0, 1, 0).Ticks));
                else if (signal.Granularity == Granularity.Hour)
                    remaining = (int)(timediff.Ticks / (new TimeSpan(1, 0, 0).Ticks));
                else
                    remaining = (int)(timediff.Ticks / (new TimeSpan(30, 0, 0, 0).Ticks));
                remaining -= result.Count();
                return result.Concat(Enumerable.Repeat(Datum<T>.CreateNone(signal, new DateTime()), remaining));
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
