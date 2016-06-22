using System;
using System.Collections.Generic;
using Domain.Repositories;
using Domain.Infrastructure;
using System.Linq;
using Domain.Exceptions;
using Mapster;

namespace Domain.Services.Implementation
{
    public class SignalsDomainService : ISignalsDomainService
    {
        private readonly ISignalsRepository signalRepository;
        private readonly IMissingValuePolicyRepository missingValuePolicyRepository;

        public SignalsDomainService(ISignalsRepository signalRepository, IMissingValuePolicyRepository missingValuePolicyRepository)
        {
            this.signalRepository = signalRepository;
            this.missingValuePolicyRepository = missingValuePolicyRepository;
        }

        public Signal Get(Path path)
        {
            var result = this.signalRepository.Get(path);

            if (result == null)
            {
                throw new KeyNotFoundException();
            }

            return result;
        }

        public Signal Get(int signalId)
        {
            var result = this.signalRepository.Get(signalId);

            if (result == null)
            {
                throw new KeyNotFoundException();
            }

            return result;
        }
 
        public Signal Add(Signal signal)
        {
            if (signal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            var defaultPolicy = MissingValuePolicy.MissingValuePolicyBase.CreateForNativeType(
                typeof(MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                DataTypeUtils.GetNativeType(signal.DataType));

            var result = this.signalRepository.Add(signal);

            this.missingValuePolicyRepository.Set(result, defaultPolicy);

            return result;
        }

        public PathEntry GetPathEntry(Path path)
        {
            var allSignals = this.signalRepository.GetAllWithPathPrefix(path);

            var directDescendants = allSignals.Where(s => s.Path.Length == path.Length + 1).ToArray();
            var subPaths = allSignals
                .Where(s => s.Path.Length > path.Length + 1)
                .Select(s => s.Path.GetPrefix(path.Length + 1))
                .Distinct()
                .ToArray();

            return new PathEntry(directDescendants, subPaths);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var readData = this.signalRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            return this.FillMissingData(signal, new TimeEnumerator(fromIncludedUtc, toExcludedUtc, signal.Granularity), readData);
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            foreach (var d in data)
            {
                d.Signal = signal;
                signal.Granularity.ValidateTimestamp(d.Timestamp);
            }

            this.signalRepository.SetData<T>(data);
        }

        private IEnumerable<Datum<T>> FillMissingData<T>(Signal signal, TimeEnumerator timeEnumerator, IEnumerable<Datum<T>> readData)
        {
            var missingValuePolicy = GetMissingValuePolicy(signal)
                as MissingValuePolicy.MissingValuePolicy<T>;

            return missingValuePolicy.FillMissingData(timeEnumerator, readData);
        }

        public MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var concretePolicy = this.missingValuePolicyRepository.Get(signal);

            return TypeAdapter.Adapt(concretePolicy, concretePolicy.GetType(), concretePolicy.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetMissingValuePolicyConfig(Signal signal, MissingValuePolicy.MissingValuePolicyBase missingValuePolicy)
        {
            this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }
    }
}
