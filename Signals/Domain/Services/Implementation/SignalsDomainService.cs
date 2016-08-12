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

            var res = this.signalsRepository.Add(newSignal);

            if (newSignal.DataType == DataType.Boolean) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<bool>());
            if (newSignal.DataType == DataType.Decimal) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<decimal>());
            if (newSignal.DataType == DataType.Double) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<double>());
            if (newSignal.DataType == DataType.Integer) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<int>());
            if (newSignal.DataType == DataType.String) missingValuePolicyRepository.Set(res, new NoneQualityMissingValuePolicy<string>());

            return res;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository?.Get(signalId);
        }

        public Signal Get(Path pathDomain)
        {
            var result = signalsRepository.Get(pathDomain);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result; 
            }
        }

        public void SetMissingValuePolicy(Domain.Signal signal, MissingValuePolicyBase missingValuePolicy)
        {
            this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var mvp = this.missingValuePolicyRepository?.Get(signal);
            if (mvp == null)
            {
                return null;
            }
            else
            {
                return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
            }
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> datum)
        {
            if(datum == null)
            {
                this.signalsDataRepository.SetData<T>(datum);
                return;
            }

            foreach(var d in datum)
            {
                d.Signal = signal;
            }

            this.signalsDataRepository.SetData<T>(datum);
        }
        //Naprawienie buga, aby mo¿na bylo ustawiaæ i pobierac dowolny typ danych, Testy by³y wczeœniej napisane
        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {           
            
                return this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();
        }
    }
}
