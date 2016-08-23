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
            var MVP = GetMVP(getSignal);
            
            List<Datum<T>> datumList = new List<Datum<T>>();

            if (fromIncludedUtc > toExcludedUtc) return datumList;

            var tempDatum = this.signalsDataRepository.GetData<T>(getSignal, fromIncludedUtc, toExcludedUtc);

            do
            {
                Datum<T> datum;
                try
                {
                    datum = tempDatum
                        .Any(d => d.Timestamp == fromIncludedUtc)?
                        tempDatum.First(d => d.Timestamp == fromIncludedUtc):
                        null;
                }
                catch { datum = null; }

                if (datum == null) datumList.Add(new Datum<T>()
                {
                    Signal = getSignal,
                    Quality = GetMVPData<T>(MVP, datumList).Quality,
                    Timestamp = fromIncludedUtc,
                    Value = GetMVPData<T>(MVP, datumList).Value
                });
                else datumList.Add(datum);
                fromIncludedUtc = AddTimpestamp(getSignal, fromIncludedUtc);
            }
            while (fromIncludedUtc < toExcludedUtc);

            return datumList;
        }  

    private SpecificValueMissingValuePolicy<T> GetMVPData<T>(MissingValuePolicyBase MVP, List<Datum<T>> datums)
        {
            if (MVP is Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<T>)
            { return new SpecificValueMissingValuePolicy<T>() { Quality = Quality.None, Value = default(T) }; }

            if (MVP is Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<T>)
            {
                
                return MVP as SpecificValueMissingValuePolicy<T>;
            }
            if (MVP is Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<T>)
            {
                int length = datums.Count();
                var result = new SpecificValueMissingValuePolicy<T>();

                if (length > 0) { result.Quality = datums[length - 1].Quality; result.Value = datums[length - 1].Value; }
                else { result.Quality = Quality.None; result.Value = default(T); }
                return result;
            }
            return new SpecificValueMissingValuePolicy<T>() { Quality = Quality.None, Value = default(T) };
        }

        private DateTime AddTimpestamp(Signal signal, DateTime datetime)
        {
            switch (signal.Granularity)
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
            Granularity granularity = setDataSignal.Granularity;

            ChekCorrectTimeStamp<T>(granularity, datum);

            this.signalsDataRepository.SetData(datum.Select(d => { d.Signal = setDataSignal; return d; }).ToList());
        }
        private bool ChekCorrectTimeStamp<T>(Granularity granularity, IEnumerable<Datum<T>> datum)
        {
            switch (granularity)
            {
                case Granularity.Second: { if (datum.Any(s => s.Timestamp.Millisecond != 0))                                                                 { throw new InvalidCastException(); } break; }
                case Granularity.Minute: { if (datum.Any(s => s.Timestamp.Second!= 0                    || ChekCorrectTimeStamp<T>(granularity - 1, datum))) { throw new InvalidCastException(); } break; }
                case Granularity.Hour:   { if (datum.Any(s => s.Timestamp.Minute != 0                   || ChekCorrectTimeStamp<T>(granularity - 1, datum))) { throw new InvalidCastException(); } break; }
                case Granularity.Day:    { if (datum.Any(s => s.Timestamp.Hour != 0                     || ChekCorrectTimeStamp<T>(granularity - 1, datum))) { throw new InvalidCastException(); } break; }
                case Granularity.Week:   { if (datum.Any(s => s.Timestamp.DayOfWeek != DayOfWeek.Monday || ChekCorrectTimeStamp<T>(granularity - 1, datum))) { throw new InvalidCastException(); } break; }
                case Granularity.Month:  { if (datum.Any(s => s.Timestamp.Day != 1                      || ChekCorrectTimeStamp<T>(granularity - 2, datum))) { throw new InvalidCastException(); } break; }
                case Granularity.Year:   { if (datum.Any(s => s.Timestamp.Month != 1                    || ChekCorrectTimeStamp<T>(granularity - 1, datum))) { throw new InvalidCastException(); } break; }
            }
            return false;
        }

        public PathEntry GetPathEntry(Path pathDomain)
        {

            int lengthEntryPath = pathDomain.Length + 1;
            IEnumerable<Domain.Signal> signalsDomain = signalsRepository.GetAllWithPathPrefix(pathDomain).ToArray();

            IEnumerable<Signal> signals = signalsDomain.Where<Signal>(s => s.Path.Components.Count() == lengthEntryPath).Select(signal => signal);
            IEnumerable<Path> subPaths = signalsDomain
                .Where<Signal>(s => s.Path.Components.Count() > lengthEntryPath)
                .Select(signal => signal.Path.GetPrefix(lengthEntryPath))
                .Distinct();
            PathEntry pathEntry = new PathEntry(signals, subPaths);
            return pathEntry;

            
        }
    }
}
