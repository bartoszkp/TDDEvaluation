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

            return this.signalsRepository.Add(newSignal);
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path pathDomain)
        {
            var result = signalsRepository.Get(pathDomain);
            return result;
        }

        public void SetMissingValuePolicy(MissingValuePolicyBase policy)
        {
            if (policy.Signal == null) throw new ArgumentException();
            missingValuePolicyRepository.Set(policy.Signal, policy);
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase policy)
        {
            Signal signal = this.signalsRepository.Get(signalId);
            if (signal == null) throw new ArgumentException();
            missingValuePolicyRepository.Set(signal, policy);
        }

        public MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(int signalID)
        {
            Signal signal = signalsRepository.Get(signalID);
            if (signal == null) throw new ArgumentException();

            MissingValuePolicyBase result = missingValuePolicyRepository.Get(signal);

            if (result != null)
            {
                return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType)
                 as MissingValuePolicy.MissingValuePolicyBase;
            }
            else return null;
        }

        public void SetData<T>(IEnumerable<Datum<T>> data)
        {
            signalsDataRepository.SetData<T>(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            return signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
        }

        public PathEntry GetPathEntry(Path path)
        {
            var allSignalsWithPathPrefix = signalsRepository.GetAllWithPathPrefix(path);

            var signals = new List<Signal>();

            var subPaths = new List<Path>();

            foreach (var signal in allSignalsWithPathPrefix)
            {
                if (signal.Path.Components.Count() == path.Components.Count()) continue;

                else if (signal.Path.Components.Count() == path.Components.Count() + 1) signals.Add(signal);

                else
                {
                    string pathToString = null;

                    foreach (var component in path.Components)
                    {
                        pathToString += component + "/";
                    }

                    var newSubPath = Path.FromString(pathToString + signal.Path.Components.ToArray()[path.Length]);

                    if (!subPaths.Contains(newSubPath)) subPaths.Add(newSubPath);
                }
            }

            return new PathEntry(signals, subPaths);
        }

        public IEnumerable<Datum<T>> GetDataOlderThan<T>(Signal signal, DateTime excludedUtc, int maxSampleCount)
        {
            return signalsDataRepository.GetDataOlderThan<T>(signal, excludedUtc, maxSampleCount);
        }
    }
}