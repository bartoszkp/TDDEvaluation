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

        public Signal Get(Path newPath)
        { 
            if(newPath.Components.Equals(""))
            {
                throw new PathIsEmptyException();
            }

            return this.signalsRepository.Get(newPath);
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase mvpDomain)
        {
            var signal = signalsRepository.Get(signalId);

            if (signal == null)
            {
                throw new SignalIsNullException();
            }

            this.missingValuePolicyRepository.Set(signal, mvpDomain);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = signalsRepository.Get(signalId);

            if (signal == null)
            {
                throw new SignalIsNullException();
            }
            else
            {
                var mvp = this.missingValuePolicyRepository.Get(signal);

                return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                    as MissingValuePolicy.MissingValuePolicyBase;
            }
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = this.GetById(signalId);
            return signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
        }

        public void SetData<T>(IEnumerable<Datum<T>> domianModel)
        {
            signalsDataRepository.SetData(domianModel);
        }
    }
}
