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

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            ChekCorrectTimeStamp<T>(signal.Granularity, fromIncludedUtc);

            var MVP = GetMVP(signal);

            if (MVP is ZeroOrderMissingValuePolicy<T>)
            {
                return (MVP as ZeroOrderMissingValuePolicy<T>).SetMissingValues(signalsDataRepository, signal, fromIncludedUtc, toExcludedUtc);
            }
            else if (MVP is FirstOrderMissingValuePolicy<T>)
            {
                return (MVP as FirstOrderMissingValuePolicy<T>).SetMissingValues(signalsDataRepository, signal, fromIncludedUtc, toExcludedUtc);
            }
            else    //NoneQuality || SpecificValue
            {

                List<Datum<T>> datumList = new List<Datum<T>>();

                if (fromIncludedUtc > toExcludedUtc) return datumList;

                var tempDatum = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

                do
                {
                    Datum<T> datum = tempDatum.FirstOrDefault(d => d.Timestamp == fromIncludedUtc);

                    if (datum == null)
                    {
                        var MVPasSpecificValueMVP = GetMVPData<T>(MVP, datumList);
                        datumList.Add(new Datum<T>()
                        {
                            Signal = signal,
                            Quality = MVPasSpecificValueMVP.Quality,
                            Timestamp = fromIncludedUtc,
                            Value = MVPasSpecificValueMVP.Value
                        });
                    }
                    else datumList.Add(datum);
                    fromIncludedUtc = AddTimpestamp(signal, fromIncludedUtc);
                }
                while (fromIncludedUtc < toExcludedUtc);

                return datumList;
            }
        }  

        private SpecificValueMissingValuePolicy<T> GetMVPData<T>(MissingValuePolicyBase MVP, List<Datum<T>> datums)
        {
            if (MVP is Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<T>)
                return MVP as SpecificValueMissingValuePolicy<T>;

            return new SpecificValueMissingValuePolicy<T>() { Quality = Quality.None, Value = default(T) };
        }

        private DateTime AddTimpestamp(Signal signal, DateTime datetime)
        {
            switch (signal.Granularity)
            {
                case Granularity.Day: return datetime.AddDays(1);
                case Granularity.Hour: return datetime.AddHours(1);
                case Granularity.Minute: return datetime.AddMinutes(1);
                case Granularity.Month: return datetime.AddMonths(1);
                case Granularity.Second: return datetime.AddSeconds(1);
                case Granularity.Week: return datetime.AddDays(7);
                case Granularity.Year: return datetime.AddYears(1);
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

            datum.Any(s=>ChekCorrectTimeStamp<T>(granularity, s.Timestamp));

            this.signalsDataRepository.SetData(datum.Select(d => { d.Signal = setDataSignal; return d; }).ToList());
        }
        private bool ChekCorrectTimeStamp<T>(Granularity granularity, DateTime timestamp)
        {
            switch (granularity)
            {
                case Granularity.Second: { if (timestamp.Millisecond != 0)                                                                     { throw new InvalidTimeStampException(); } break; }
                case Granularity.Minute: { if (timestamp.Second!= 0                    || ChekCorrectTimeStamp<T>(granularity - 1, timestamp)) { throw new InvalidTimeStampException(); } break; }
                case Granularity.Hour:   { if (timestamp.Minute != 0                   || ChekCorrectTimeStamp<T>(granularity - 1, timestamp)) { throw new InvalidTimeStampException(); } break; }
                case Granularity.Day:    { if (timestamp.Hour != 0                     || ChekCorrectTimeStamp<T>(granularity - 1, timestamp)) { throw new InvalidTimeStampException(); } break; }
                case Granularity.Week:   { if (timestamp.DayOfWeek != DayOfWeek.Monday || ChekCorrectTimeStamp<T>(granularity - 1, timestamp)) { throw new InvalidTimeStampException(); } break; }
                case Granularity.Month:  { if (timestamp.Day != 1                      || ChekCorrectTimeStamp<T>(granularity - 2, timestamp)) { throw new InvalidTimeStampException(); } break; }
                case Granularity.Year:   { if (timestamp.Month != 1                    || ChekCorrectTimeStamp<T>(granularity - 1, timestamp)) { throw new InvalidTimeStampException(); } break; }
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

        public void Delete(int signalId)
        {
            var signal = signalsRepository.Get(signalId);

            if (signal == null) throw new InvalidSignalId();

            missingValuePolicyRepository.Set(signal, null);

            switch (signal.DataType)
            {
                case DataType.Boolean: signalsDataRepository.DeleteData<bool>(signal); break;
                case DataType.Integer: signalsDataRepository.DeleteData<int>(signal); break;
                case DataType.Double: signalsDataRepository.DeleteData<double>(signal); break;
                case DataType.Decimal: signalsDataRepository.DeleteData<decimal>(signal); break;
                case DataType.String: signalsDataRepository.DeleteData<string>(signal); break;
                default: break;
            }

            signalsRepository.Delete(signal);
        }
    }
}
