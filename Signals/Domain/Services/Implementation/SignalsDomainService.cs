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
            return this.signalsRepository.Add(newSignal);
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path signalPath)
        {
            Signal signal = this.signalsRepository.Get(signalPath);
            if (signal == null)
                throw new CouldntGetASignalException();
            return signal;
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            List<Datum<T>> dataList = data.ToList();
            for (int i =0;i < dataList.Count; ++i)
                dataList[i].Signal = signal;

            this.signalsDataRepository.SetData<T>(dataList);
        }

        public IEnumerable<Domain.Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Signal signal = GetById(signalId);
            return this.signalsDataRepository.GetData<T>(signal,fromIncludedUtc,toExcludedUtc);
        }

        public void SetMissingValuePolicy<T>(Signal signal, Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<T> policy)
        {
            this.missingValuePolicyRepository.Set(signal,policy);
        }

        public MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy<T>(Signal signal)
        {
            return new MissingValuePolicy.SpecificValueMissingValuePolicy<Double>() as MissingValuePolicy.MissingValuePolicyBase;
        }
    }
}
