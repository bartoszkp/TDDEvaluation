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

        public Signal GetById(int signalId)
        {
            var item = this.signalsRepository.Get(signalId);
            if (item == null)
                throw new NoSuchSignalException();
            return item;
        }

        public Signal Add(Signal newSignal)
        {
            if (newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }
            return this.signalsRepository.Add(newSignal);
        }

        public Signal Get(Path pathDto)
        {
            var item = this.signalsRepository.Get(pathDto);
            if (item == null)
                throw new NoSuchSignalException();
            return item;
        }

        public void SetData<T>(IEnumerable<Datum<T>> data)
        {
            signalsDataRepository.SetData<T>(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var items = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
            return items;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domianPolicy)
        {
            var signal = this.GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException();

            this.missingValuePolicyRepository.Set(signal, domianPolicy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = this.GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException();

            var mvp = this.missingValuePolicyRepository.Get(signal);
            if(mvp != null)
                return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
            else
                return null;
        }
    }
}
