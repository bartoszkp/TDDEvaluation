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

        public void SetData(IEnumerable<Datum<object>> newDomainDatum)
        {
            try
            {
                switch (newDomainDatum.First().Value.GetType().ToString())
                {
                    case "System.Double":
                        var newList = new List<Datum<double>>();
                        foreach (Datum<object> d in newDomainDatum)
                        {
                            newList.Add(new Datum<double>() { Quality = d.Quality, Signal = d.Signal, Timestamp = d.Timestamp, Value = Convert.ToDouble(d.Value) });
                        }
                        this.signalsDataRepository.SetData(newList);
                        break;
                    case "System.Decimal":
                        var newList1 = new List<Datum<decimal>>();
                        foreach (Datum<object> d in newDomainDatum)
                        {
                            newList1.Add(new Datum<decimal>() { Quality = d.Quality, Signal = d.Signal, Timestamp = d.Timestamp, Value = Convert.ToDecimal(d.Value) });
                        }
                        this.signalsDataRepository.SetData(newList1);
                        break;
                    case "System.Int32":
                        var newList2 = new List<Datum<int>>();
                        foreach (Datum<object> d in newDomainDatum)
                        {
                            newList2.Add(new Datum<int>() { Quality = d.Quality, Signal = d.Signal, Timestamp = d.Timestamp, Value = Convert.ToInt32(d.Value) });
                        }
                        this.signalsDataRepository.SetData(newList2);
                        break;
                    case "System.String":
                        var newList3 = new List<Datum<string>>();
                        foreach (Datum<object> d in newDomainDatum)
                        {
                            newList3.Add(new Datum<string>() { Quality = d.Quality, Signal = d.Signal, Timestamp = d.Timestamp, Value = Convert.ToString(d.Value) });
                        }
                        this.signalsDataRepository.SetData(newList3);
                        break;
                    case "System.Boolean":
                        var newList4 = new List<Datum<bool>>();
                        foreach (Datum<object> d in newDomainDatum)
                        {
                            newList4.Add(new Datum<bool>() { Quality = d.Quality, Signal = d.Signal, Timestamp = d.Timestamp, Value = Convert.ToBoolean(d.Value) });
                        }
                        this.signalsDataRepository.SetData(newList4);
                        break;
                }
            }
            catch (Exception)
            {
                throw new InvalidValueType();
            }
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
    }
}
