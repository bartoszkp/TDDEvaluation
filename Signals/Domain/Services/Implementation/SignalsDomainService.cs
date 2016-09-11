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

            if (signal.Path == null)
                throw new ArgumentNullException(nameof(signal.Path));

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
            signal.Granularity.ValidateTimestamp(fromIncludedUtc);

            var missingValuePolicy = GetMissingValuePolicy(signal)
                as MissingValuePolicy.MissingValuePolicy<T>;

            return missingValuePolicy.GetDataAndFillMissingSamples(
                new TimeEnumerator(fromIncludedUtc, toExcludedUtc, signal.Granularity),
                this.signalsDataRepository);
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
            CheckSignalsCompatibilityWithPolicy(signal, missingValuePolicy);

            this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }

        private void CheckSignalsCompatibilityWithPolicy(Signal signal, MissingValuePolicy.MissingValuePolicyBase missingValuePolicy)
        {
            if (!missingValuePolicy.CompatibleNativeTypes.Contains(signal.DataType.GetNativeType()))
            {
                throw new IncompatibleSignalDataType();
            }

            if (!missingValuePolicy.CompatibleGranularities.Contains(signal.Granularity))
            {
                throw new IncompatibleSignalGranularity();
            }

            if (missingValuePolicy.DependsOn(signal, missingValuePolicyRepository))
            {
                throw new MissingValuePolicyDependencyCycleException();
            }
        }

        public IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            // TODO validate granularity > Signal.granularity
            // TODO throw on String? and on Bool
            granularity.ValidateTimestamp(fromIncludedUtc); // TODO tests?
            granularity.ValidateTimestamp(toExcludedUtc); // TODO ? / tests?

            var fromTimestampCoarse = fromIncludedUtc;
            var coarseToTimestampEnumerator
                = new TimeEnumerator(
                    GetNextDate(fromIncludedUtc, granularity),
                    GetNextDate(toExcludedUtc, granularity),
                    granularity);
            foreach (var toTimestampCoarse in coarseToTimestampEnumerator)
            {
                var samples = GetData<T>(signal, fromTimestampCoarse, toTimestampCoarse);

                yield return new Datum<T>
                {
                    Signal = signal,
                    Timestamp = fromTimestampCoarse,
                    Value = (T)Convert.ChangeType(samples.Average(d => Convert.ToDecimal(d.Value)), typeof(T)),
                    Quality = samples.Select(d => d.Quality).Aggregate(GranularityUtils.GetMinQuality),
                };

                fromTimestampCoarse = toTimestampCoarse;
            }
        }

        private DateTime GetNextDate(DateTime time, Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return time.AddSeconds(1);
                case Granularity.Minute:
                    return time.AddMinutes(1);
                case Granularity.Hour:
                    return time.AddHours(1);
                case Granularity.Day:
                    return time.AddDays(1);
                case Granularity.Week:
                    return time.AddDays(7);
                case Granularity.Month:
                    return time.AddMonths(1);
                case Granularity.Year:
                    return time.AddYears(1);
            }

            throw new InvalidOperationException();
        }
    }
}
