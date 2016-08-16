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

            var signal = this.signalsRepository.Add(newSignal);
            var policyInstance = typeof(NoneQualityMissingValuePolicy<>)
                .MakeGenericType(new Type[] { DataTypeUtils.GetNativeType(signal.DataType) });

            SetMissingValuePolicy(signal, Activator.CreateInstance(policyInstance) as MissingValuePolicyBase);

            return signal;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path path)
        {
            return signalsRepository.Get(path);
        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase domainMissingValuePolicy)
        {
            if (signal == null)
            {
                throw new ArgumentException("no signal with this id");
            }
            missingValuePolicyRepository.Set(signal, domainMissingValuePolicy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            if (signal == null)
            {
                throw new ArgumentException("no signal with this id");
            }
            var mvp = missingValuePolicyRepository.Get(signal);

            if(mvp == null)
            {
                return null;
            }

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetData<T>(IEnumerable<Datum<T>> domainData)
        {
            signalsDataRepository.SetData<T>(domainData);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var getData = signalsDataRepository
                .GetData<T>(signal, fromIncludedUtc, toExcludedUtc)
                ?.OrderBy(s => s.Timestamp).ToArray();

            var getMissingValuePolicy = GetMissingValuePolicy(signal);

            if (getMissingValuePolicy == null)
                return getData;
            
            var result = new List<Datum<T>>();
            var dt = fromIncludedUtc;

            while(dt < toExcludedUtc || dt == fromIncludedUtc)
            {
                var next = SignalUtils.GetNextDate(dt, signal.Granularity);

                var datum = getData.FirstOrDefault(d => dt <= d.Timestamp && next > d.Timestamp);
                if (datum == null)
                    datum = GenerateDatumFromPolicy(getMissingValuePolicy as MissingValuePolicy<T>, signal, dt);
                result.Add(datum);

                dt = next;
            }
            return result;
        }

        private Datum<T> GenerateDatumFromPolicy<T>(MissingValuePolicy<T> mvp, Signal signal, DateTime timestamp)
        {
            var result = new Datum<T>()
            {
                Signal = signal,
                Timestamp = timestamp
            };

            return mvp.FillMissingValue(result);
        }

        public PathEntry GetPathEntry(Path pathDomain)
        {
            var signals = signalsRepository.GetAllWithPathPrefix(pathDomain);

            var filteredSignals = signals.Where(s => s.Path.Length == pathDomain.Length + 1);
            var subFolders = signals
                .Where(s => s.Path.Length > pathDomain.Length + 1)
                .Select(s => s.Path.GetPrefix(pathDomain.Length + 1))
                .Distinct();

            var result = new PathEntry(filteredSignals, subFolders);

            return result;
        }
    }
}
