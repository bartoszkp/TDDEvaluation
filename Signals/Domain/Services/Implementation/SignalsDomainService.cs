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
            var signal = this.signalsRepository.Add(newSignal);
            if(missingValuePolicyRepository == null)
            {
                return signal;
            }
            if (newSignal.DataType.GetNativeType() == typeof(double))
            {
                this.missingValuePolicyRepository.Set(newSignal, new NoneQualityMissingValuePolicy<double>());
            }
            else if(newSignal.DataType.GetNativeType() == typeof(int))
            {
                this.missingValuePolicyRepository.Set(newSignal, new NoneQualityMissingValuePolicy<int>());
            }
            return signal;
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

        public Signal GetByPath(Path path)
        {
            var result = signalsRepository.Get(path);

            if (result == null)
                return null;

            return result;
        }

        public void SetMissingValuePolicy(Signal exampleSignal, MissingValuePolicyBase policy)
        {
            this.missingValuePolicyRepository.Set(exampleSignal, policy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var result = this.missingValuePolicyRepository.Get(signal);
            return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
        }
    }
}
