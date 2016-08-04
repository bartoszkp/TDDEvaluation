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
            return this.signalsRepository.Add(newSignal);
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path path)
        {
            
                var result = signalsRepository.Get(path);
                if (result == null)
                    throw new PathNotExistException();
                return result;
            
            
        }
        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domainPolicyBase)
        {
            var gettingSignalFromRepository = signalsRepository.Get(signalId);
            if (gettingSignalFromRepository == null)
                throw new SignalIsNotException();
            missingValuePolicyRepository.Set(gettingSignalFromRepository, domainPolicyBase);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signalDomain)
        {
            var result = this.missingValuePolicyRepository.Get(signalDomain);
            if (result == null) return null;
            else
                return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
        }
    }
}
