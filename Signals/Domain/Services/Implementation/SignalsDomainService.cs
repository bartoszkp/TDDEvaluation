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
            var signal =  this.signalsRepository.Add(newSignal);

            return signal;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal GetByPath(Path path)
        {
            return this.signalsRepository.Get(path);
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = signalsRepository.Get(signalId);

            var result = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            return result.ToArray();
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> enumerable)
        {
            var signal = signalsRepository.Get(signalId);

            foreach (var item in enumerable)
            {
                item.Signal = signal;
            }

            Type type = null;

            foreach (var item in enumerable)
            {
                type = item.Value.GetType();
            }

            if (type == typeof(bool)) signalsDataRepository.SetData<bool>(enumerable as IEnumerable<Datum<bool>>);
            else if (type == typeof(int)) signalsDataRepository.SetData<int>(enumerable as IEnumerable<Datum<int>>);
            else if (type == typeof(double)) signalsDataRepository.SetData<double>(enumerable as IEnumerable<Datum<double>>);
            else if (type == typeof(decimal)) signalsDataRepository.SetData<decimal>(enumerable as IEnumerable<Datum<decimal>>);
            else if (type == typeof(string)) signalsDataRepository.SetData<string>(enumerable as IEnumerable<Datum<string>>);
            else throw new ArgumentException("Type of the 'data' parameter's signals must be bool, int, double, decimal or string.");
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = signalsRepository.Get(signalId);

            var mvp = missingValuePolicyRepository.Get(signal);

            if (mvp == null) return null;

            else return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType())
                as MissingValuePolicy.MissingValuePolicyBase;
        }
    }
}