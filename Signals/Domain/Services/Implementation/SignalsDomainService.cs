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

        private void SetDefaultMissingPolicy(Signal signal)
        {
            MissingValuePolicyBase policy;

            switch (signal.DataType)
            {
                case Domain.DataType.Boolean:
                        policy = new NoneQualityMissingValuePolicy<Boolean>();
                    break;
                case Domain.DataType.Decimal:
                        policy = new NoneQualityMissingValuePolicy<Decimal>();
                    break;
                case Domain.DataType.Double:
                         policy = new NoneQualityMissingValuePolicy<Double>();
                    break;
                case Domain.DataType.Integer:
                        policy = new NoneQualityMissingValuePolicy<Int32>();
                    break;
                case Domain.DataType.String:
                        policy = new NoneQualityMissingValuePolicy<String>();
                    break;
                default: return;
            }

            this.missingValuePolicyRepository.Set(signal, policy);
        }

        public Signal Add(Signal newSignal)
        {
            if (newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            var signal = this.signalsRepository.Add(newSignal);

            SetDefaultMissingPolicy(signal);

            return signal;
        }

        public Signal GetByPath(Path domainPath)
        {
            Signal foundSignal = signalsRepository.Get(domainPath);

            if (foundSignal == null)
                throw new SignalNotExistException();

            return foundSignal;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public void SetData<T>(Signal foundSignal, IEnumerable<Datum<T>> data)
        {
            List<Datum<T>> dataList = data.ToList();
            for (int i = 0; i < dataList.Count; ++i)
                dataList[i].Signal = foundSignal;

            this.signalsDataRepository.SetData(dataList);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal foundSignal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            return this.signalsDataRepository.GetData<T>(foundSignal, fromIncludedUtc, toExcludedUtc);
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domainMvp)
        {
            Signal foundSignal = GetById(signalId);
            if (foundSignal == null)
                throw new SignalNotExistException();
                   
            missingValuePolicyRepository.Set(foundSignal, domainMvp);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            Signal foundSignal = GetById(signalId);
            if (foundSignal == null)
                throw new SignalNotExistException();

            var mvp = missingValuePolicyRepository.Get(foundSignal);

            if (mvp == null)
                return null;

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }
    }
}
