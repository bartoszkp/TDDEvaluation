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
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path path)
        {
            return this.signalsRepository.Get(path);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = this.signalsRepository.Get(signalId);

            var mvp = this.missingValuePolicyRepository.Get(signal);

            if (mvp == null) return null;

            else return mvp;

        }

        public IEnumerable<Datum<double>> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = signalsRepository.Get(signalId);

            return this.signalsDataRepository.GetData<double>(signal, fromIncludedUtc, toExcludedUtc).ToArray();
        }

        public void SetData(int signalId, IEnumerable<Datum<double>> enumerable)
        {
            var signal = signalsRepository.Get(signalId);

            foreach (var item in enumerable)
            {
                item.Signal = signal;
            }

            signalsDataRepository.SetData<double>(enumerable);
        }
    }
}