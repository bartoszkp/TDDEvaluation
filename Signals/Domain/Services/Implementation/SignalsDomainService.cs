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
            return this.signalsRepository.Add(newSignal);
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository?.Get(signalId);
        }

        public Signal Get(Path pathDomain)
        {
            var result = signalsRepository.Get(pathDomain);
            if (result == null)
            {
                throw new ArgumentException("Invalid argument - signal with path" + pathDomain + "does not exist.");
            }
            else
            {
                return result; 
            }
        }

        public void SetMissingValuePolicy(Domain.Signal signal, MissingValuePolicyBase missingValuePolicy)
        {
            this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var mvp = this.missingValuePolicyRepository.Get(signal);
            if (mvp == null)
            {
                return null;
            }
            else
            {
                return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
            }
        }

        public void SetData(Signal signal, IEnumerable<Datum<double>> datum)
        {
            if(datum == null)
            {
                this.signalsDataRepository.SetData<double>(datum);
                return;
            }

            foreach(var d in datum)
            {
                d.Signal = signal;
            }

            this.signalsDataRepository.SetData<double>(datum);
        }

        public IEnumerable<Datum<double>> GetData(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if(signal == null)
            {
                return this.signalsDataRepository.GetData<double>(signal, fromIncludedUtc, toExcludedUtc);
            }
            return this.signalsDataRepository.GetData<double>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();
        }
    }
}
