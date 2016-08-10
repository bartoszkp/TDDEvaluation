using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Mapster;
using Domain;
using System.Collections;

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
                throw new IdNotNullException();

            var signal = this.signalsRepository.Add(newSignal);

            if (missingValuePolicyRepository == null)
            {
                return signal;
            }
            
            string typeName = signal.DataType.GetNativeType().Name;

            switch (typeName)
            {
                case "Int32":
                    GenericSetDataCall<int>(signal);
                    break;
                case "Double":
                    GenericSetDataCall<double>(signal);
                    break;
                case "Decimal":
                    GenericSetDataCall<decimal>(signal);
                    break;
                case "Boolean":
                    GenericSetDataCall<bool>(signal);
                    break;
                case "String":
                    GenericSetDataCall<string>(signal);
                    break;
            }

            return signal;
        }

        private void GenericSetDataCall<T>(Signal signal)
        {
            this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<T>());
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path path)
        {
            var result = signalsRepository.Get(path);
            if (result == null)
                return null;
            return result;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domainPolicyBase)
        {
            var gettingSignalFromRepository = signalsRepository.Get(signalId);
            if (gettingSignalFromRepository == null)
                throw new SignalIsNotException();
            missingValuePolicyRepository.Set(gettingSignalFromRepository, domainPolicyBase);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signalDomain)
        {
            var result = this.missingValuePolicyRepository.Get(signalDomain);
            if (result == null)
                throw new IdNotNullException();
            else
                return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> data)
        {
            if(data == null)
            {
                this.signalsDataRepository.SetData<T>(data);
                return;
            }

            foreach(var d in data)
            {
                d.Signal = signal;
            }

            this.signalsDataRepository.SetData<T>(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUTC, DateTime toExcludedUTC)
        {
            var data = this.signalsDataRepository
                .GetData<T>(signal, fromIncludedUTC, toExcludedUTC);

            var missingValuePolicy = GetMissingValuePolicy(signal)
                as MissingValuePolicy.MissingValuePolicy<T>;

            return missingValuePolicy.FillData(signal, data).ToArray();
        }
    }
}
