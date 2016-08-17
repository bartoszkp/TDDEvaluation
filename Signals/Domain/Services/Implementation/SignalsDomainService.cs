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
            List<Datum<T>> datumList = new List<Datum<T>>();

            var MVP = GetMVP(getSignal);

            if (MVP is Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<T>)
            {
                GetDataOverMVP(datumList, getSignal, fromIncludedUtc, toExcludedUtc, Quality.None, default(T));
                return datumList;
            }

            if(MVP is Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<T>)
            {
                var SvMVP = (SpecificValueMissingValuePolicy<T>)MVP;
                GetDataOverMVP(datumList, getSignal, fromIncludedUtc, toExcludedUtc, SvMVP.Quality, SvMVP.Value);
                return datumList;
            }

            return this.signalsDataRepository.GetData<T>(getSignal, fromIncludedUtc, toExcludedUtc);
        }

        private List<Datum<T>> GetDataOverMVP<T>(List<Datum<T>> datumList, Signal getSignal, DateTime fromIncludedUtc, DateTime toExcludedUtc, Quality quality, T value )
        {
            if (fromIncludedUtc > toExcludedUtc) return datumList;

            var tempDatum = this.signalsDataRepository.GetData<T>(getSignal, fromIncludedUtc, toExcludedUtc);

            do
            {
                Datum<T> datum;
                try { datum = tempDatum.Any(d => d.Timestamp == fromIncludedUtc) ? tempDatum.First(d => d.Timestamp == fromIncludedUtc) : null; }
                catch { datum = null; }

                if (datum == null) datumList.Add(new Datum<T>() { Signal = getSignal, Quality = quality, Timestamp = fromIncludedUtc, Value = value });
                else datumList.Add(datum);

                fromIncludedUtc = AddTimpestamp(getSignal, fromIncludedUtc);
            }
            while (fromIncludedUtc < toExcludedUtc);

            return datumList;
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

        private DateTime AddTimpestamp(Signal signal, DateTime datetime)
        {
            switch(signal.Granularity)
            {
                case Granularity.Day: return datetime = datetime.AddDays(1);
                case Granularity.Hour: return datetime = datetime.AddHours(1);
                case Granularity.Minute: return datetime = datetime.AddMinutes(1);
                case Granularity.Month: return datetime = datetime.AddMonths(1);
                case Granularity.Second: return datetime = datetime.AddSeconds(1);
                case Granularity.Week: return datetime = datetime.AddDays(7);
                case Granularity.Year: return datetime = datetime.AddYears(1);
                default: return datetime;
            }
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
