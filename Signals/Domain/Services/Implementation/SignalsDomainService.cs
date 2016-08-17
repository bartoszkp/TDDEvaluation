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

        public Signal GetById(int signalId)
        {
            var result = this.signalsRepository.Get(signalId);
            return result;
        }

        public Signal Add(Signal newSignal)
        {
            if (newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            var signal = this.signalsRepository.Add(newSignal);

            SetDefaultMVPForSignal(signal);            

            return signal;
        }

        public Signal Get(Path pathDto)
        {
            var result = this.signalsRepository.Get(pathDto);
            return result;
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal getSignal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            return this.signalsDataRepository.GetData<T>(getSignal, fromIncludedUtc, toExcludedUtc);
        }

        public void SetMVP(Signal domainSetMVPSignal, MissingValuePolicyBase domainPolicyBase)
        {
            this.missingValuePolicyRepository.Set(domainSetMVPSignal, domainPolicyBase);
        }

        public MissingValuePolicy.MissingValuePolicyBase GetMVP(Signal domainSignal)
        {
            var result = this.missingValuePolicyRepository.Get(domainSignal);
            if (result == null) return null;
            else return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
        }

        private void SetDefaultMVPForSignal(Signal signal)
        {
            switch(signal.DataType)
            {
                case DataType.Boolean:
                    SetMVP(signal, new NoneQualityMissingValuePolicy<bool>());
                    break;

                case DataType.Decimal:
                    SetMVP(signal, new NoneQualityMissingValuePolicy<decimal>());
                    break;

                case DataType.Double:
                    SetMVP(signal, new NoneQualityMissingValuePolicy<double>());
                    break;

                case DataType.Integer:
                    SetMVP(signal, new NoneQualityMissingValuePolicy<int>());
                    break;

                case DataType.String:
                    SetMVP(signal, new NoneQualityMissingValuePolicy<string>());
                    break;

                default:
                    throw new UnsupportedTypeForMVP();                    
            }            
        }

        public void SetData<T>(Signal setDataSignal, IEnumerable<Datum<T>> datum)
        {
            this.signalsDataRepository.SetData(datum.Select(d => { d.Signal = setDataSignal; return d; }).ToList());
        }
    }
}
