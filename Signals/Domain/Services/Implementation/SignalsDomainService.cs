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

        public Signal Get(Path path)
        {
            return this.signalsRepository.Get(path);
        }

        public Signal Get(int signalId)
        {
            return this.signalsRepository.Get(signalId);
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

            var result = this.signalsRepository.Add(signal);

            this.missingValuePolicyRepository.Set(result, defaultPolicy);

            return result;
        }

        public void Delete(int signalId)
        {
            var signal = this.signalsRepository.Get(signalId);

            if (signal == null)
            {
                throw new KeyNotFoundException();
            }

            this.missingValuePolicyRepository.Set(signal, null);

            var deleteDataMethod = ReflectionUtils.GetMethodInfo<ISignalsDataRepository>(sdr => sdr.DeleteData<object>(null));

            var concreteDataMethod = deleteDataMethod
                .GetGenericMethodDefinition()
                .MakeGenericMethod(signal.DataType.GetNativeType());

            concreteDataMethod.Invoke(this.signalsDataRepository, new object[] { signal });

            this.signalsRepository.Delete(signal);
        }

        public PathEntry GetPathEntry(Path path)
        {
            var allSignals = this.signalsRepository.GetAllWithPathPrefix(path);

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
            var readData = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            return this.FillMissingData(signal,
                                        new TimeEnumerator(fromIncludedUtc, toExcludedUtc, signal.Granularity),
                                        readData);
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            foreach (var d in data)
            {
                d.Signal = signal;
                signal.Granularity.ValidateTimestamp(d.Timestamp);
            }

            this.signalsDataRepository.SetData<T>(data);
        }

        private IEnumerable<Datum<T>> FillMissingData<T>(Signal signal, TimeEnumerator timeEnumerator, IEnumerable<Datum<T>> readData)
        {
            var missingValuePolicy = GetMissingValuePolicy(signal)
                as MissingValuePolicy.MissingValuePolicy<T>;

            var olderData = this.signalsDataRepository.GetDataOlderThan<T>(signal,
                                                                           timeEnumerator.FromIncludedUtc,
                                                                           missingValuePolicy.OlderDataSampleCountNeeded);
            var newerData = this.signalsDataRepository.GetDataNewerThan<T>(signal,
                                                                           timeEnumerator.ToExcludedUtcUtc,
                                                                           missingValuePolicy.NewerDataSampleCountNeeded);

            return missingValuePolicy.FillMissingData(timeEnumerator,
                                                      readData,
                                                      olderData,
                                                      newerData);
        }

        public MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var concretePolicy = this.missingValuePolicyRepository.Get(signal);

            if (concretePolicy == null)
            {
                return null;
            }

            return TypeAdapter.Adapt(concretePolicy, concretePolicy.GetType(), concretePolicy.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicy.MissingValuePolicyBase missingValuePolicy)
        {
            if (!missingValuePolicy.CompatibleNativeTypes.Contains(signal.DataType.GetNativeType()))
            {
                throw new IncompatibleSignalDataType();
            }

            this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }
    }
}
