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
            if(newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            var signal = this.signalsRepository.Add(newSignal);
            if(missingValuePolicyRepository == null)
            {
                return signal;
            }

            string typeName = signal.DataType.GetNativeType().Name;

            switch (typeName)
            {
                case "Int32":
                    GenericSetCall<int>(signal);
                    break;
                case "Double":
                    GenericSetCall<double>(signal);
                    break;
                case "Decimal":
                    GenericSetCall<decimal>(signal);
                    break;
                case "Boolean":
                    GenericSetCall<bool>(signal);
                    break;
                case "String":
                    GenericSetCall<string>(signal);
                    break;
            }
            return signal;
        }


        private void GenericSetCall<T>(Signal signal)
        {
            this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<T>());
        }
        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> data)
        {
            var signal = GetById(signalId);
            
            foreach(var item in data)
            {
                item.Signal = signal;
            }

            signalsDataRepository.SetData(data);
        }
        
        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId);

            var data = this.signalsDataRepository
                .GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
            if(missingValuePolicyRepository == null || fromIncludedUtc == toExcludedUtc)
            {
                return data;
            }
            var mvp = GetMissingValuePolicy(signal) as MissingValuePolicy.MissingValuePolicy<T>;

            return mvp.FillData(signal, data, fromIncludedUtc, toExcludedUtc).ToArray();
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

        public PathEntry GetPathEntry(Path pathDomain)
        {
            var signals = signalsRepository.GetAllWithPathPrefix(pathDomain);

            if (signals == null)
                return null;

            List<Signal> directPathSignals = new List<Signal>();
            foreach(var signal in signals)
            {
                if (signal.Path.Length == pathDomain.Length + 1)
                    directPathSignals.Add(signal);
            }

            List<Path> subPaths = new List<Path>();
            foreach(var signal in signals)
            {
                if (signal.Path.Length > pathDomain.Length + 1)
                    subPaths.Add(signal.Path.GetPrefix(pathDomain.Length + 1));
            }
            
            return new PathEntry(directPathSignals.AsEnumerable<Signal>().ToArray(), subPaths.AsEnumerable<Path>().ToArray());
        }
    }
}
