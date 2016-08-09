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
            return this.signalsRepository.Get(signalId);
        }

        public Signal Add(Signal newSignal)
        {
            if (newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }
            var result = this.signalsRepository.Add(newSignal);

            switch (result.DataType)
            {
                case DataType.Boolean:
                    this.missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<bool>()); 
                    break;

                case DataType.Integer:
                    this.missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<int>());
                    break;

                case DataType.Double:
                    this.missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<double>());
                    break;

                case DataType.Decimal:
                    this.missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<decimal>());
                    break;

                case DataType.String:
                    this.missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<string>());
                    break;
            }
            
            return result;

        }

        public Signal Get(Path pathDto)
        {
            return this.signalsRepository.Get(pathDto);
        }

        public void SetData<T>(IEnumerable<Datum<T>> data)
        {
            signalsDataRepository.SetData<T>(data);
        }

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var items = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc).ToList();

            var mvp = GetMissingValuePolicy(signal.Id.GetValueOrDefault());

            FillMissingData(mvp, items, fromIncludedUtc, toExcludedUtc);

            var result = from d in items
                         orderby d.Timestamp
                         select d;

            return result;

        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domainPolicy)
        {
            var signal = this.GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException();

            this.missingValuePolicyRepository.Set(signal, domainPolicy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = this.GetById(signalId);
            if (signal == null)
                throw new NoSuchSignalException();

            var mvp = this.missingValuePolicyRepository.Get(signal);
            if (mvp != null)
                return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
            else
                return null;
        }

        private void FillMissingData<T>(MissingValuePolicyBase mvp, List<Datum<T>> datum, DateTime after, DateTime before)
        {

            int currentMonth = after.Month + 1;

            while (currentMonth < before.Month - 1)
            {
                Datum<T> missingDatum = new Datum<T>()
                {
                    Quality = Quality.None,
                    Timestamp = new DateTime(after.Year, currentMonth, after.Day),
                    Value = default(T)
                };
                datum.Add(missingDatum);
                currentMonth++;
            }
        }



    }
}
