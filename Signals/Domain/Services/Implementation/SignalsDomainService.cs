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
            
            var result= this.signalsRepository.Add(newSignal);

            var dataType = result.DataType;       

            switch (dataType)
            {
                case Domain.DataType.Boolean:
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<Boolean>());
                    break;
                case Domain.DataType.Integer:
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<int>());
                    break;
                case Domain.DataType.Double:
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<double>());
                    break;
                case Domain.DataType.Decimal:
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<decimal>());
                    break;
                case Domain.DataType.String:
                    missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<string>());
                    break;
                default:
                    break;
            }

            return result;
        }

        public Signal GetByPath(Path signalPath)
        {
            var result = this.signalsRepository.Get(signalPath);


            return result;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase policy)
        {
            var signal = MissingValuePolicySignal(signalId);

            missingValuePolicyRepository.Set(signal, policy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = MissingValuePolicySignal(signalId);
            var mvp = this.missingValuePolicyRepository.Get(signal);

            if (mvp != null)
                mvp = TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                    as MissingValuePolicyBase;

            return mvp;
        }

        private Signal MissingValuePolicySignal(int signalId)
        {
            var signal = this.signalsRepository.Get(signalId);

            if (signal == null)
                throw new SignalDoesntExistException();

            return signal;
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> data)
        {
            var signal = this.signalsRepository.Get(signalId);
            var dataList = data.ToList();

            foreach (var d in dataList)
                d.Signal = signal;

            this.signalsDataRepository.SetData(dataList);
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = this.signalsRepository.Get(signalId);           
            var result = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            var sortedList = result.Where(x=>x.Timestamp>=fromIncludedUtc&&x.Timestamp<toExcludedUtc).OrderBy(x => x.Timestamp).ToList();

          
            var r = GetMissingValuePolicy(signalId);
            var time = fromIncludedUtc;


            if (r.GetType() == typeof(NoneQualityMissingValuePolicy<T>))
            {
                while (time < toExcludedUtc)
                {
                    if (sortedList.FindIndex(x=>x.Timestamp==time)<0)
                    {
                     
                        sortedList.Add(new Datum<T>() { Quality = Quality.None, Timestamp = time, Value = default(T) });
                    }
                    time = AddTime(signal.Granularity, time);
                }
            }
            return sortedList.OrderBy(s=>s.Timestamp);
        }

        private  DateTime AddTime(Granularity granularity,DateTime time)
        {
            switch (granularity)
            {
                case Granularity.Second:
                   return time.AddSeconds(1);
                    break;
                case Granularity.Minute:
                    return time.AddMinutes(1);
                    break;
                case Granularity.Hour:
                    return time.AddHours(1);
                    break;
                case Granularity.Day:
                    return time.AddDays(1);
                    break;
                case Granularity.Week:
                    return time.AddDays(7);
                    break;
                case Granularity.Month:
                    return time.AddMonths(1);
                    break;
                case Granularity.Year:
                    return time.AddYears(1);
                    break;
                default:
                    return new DateTime();
                    break;
            }
        }

        public Type GetDataTypeById(int signalId)
        {
            var dataType = this.signalsRepository.Get(signalId)
                ?.DataType;

            if (dataType.HasValue == false)
                throw new SignalDoesntExistException();

            return DataTypeUtils.GetNativeType(dataType.Value);
        }
    }
}
