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

        public void SetData(int signalId, IEnumerable<Datum<bool>> data)
        {
            var signal = GetById(signalId);

            foreach (var item in data)
            {
                item.Signal = signal;
            }

            signalsDataRepository.SetData(data);
        }

        public void SetData(int signalId, IEnumerable<Datum<decimal>> data)
        {
            var signal = GetById(signalId);

            foreach (var item in data)
            {
                item.Signal = signal;
            }

            signalsDataRepository.SetData(data);
        }

        public void SetData(int signalId, IEnumerable<Datum<double>> data)
        {
            var signal = GetById(signalId);

            foreach (var item in data)
            {
                item.Signal = signal;
            }

            signalsDataRepository.SetData(data);
        }

        public void SetData(int signalId, IEnumerable<Datum<int>> data)
        {
            var signal = GetById(signalId);

            foreach (var item in data)
            {
                item.Signal = signal;
            }

            signalsDataRepository.SetData(data);
        }

        public void SetData(int signalId, IEnumerable<Datum<string>> data)
        {
            var signal = GetById(signalId);

            foreach (var item in data)
            {
                item.Signal = signal;
            }

            signalsDataRepository.SetData(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId);

            return signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
        }
    }
}
