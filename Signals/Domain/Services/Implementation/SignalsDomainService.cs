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

            return this.signalsRepository.Add(newSignal);
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path path)
        {
            return this.signalsRepository.Get(path);
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
            return this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase policy)
        {
            if (policy != null)
            {
                if (policy.Id.HasValue)
                    throw new IdNotNullException();

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
