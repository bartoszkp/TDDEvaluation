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
            else if (MVP is ShadowMissingValuePolicy<T>)
            {
                return (MVP as ShadowMissingValuePolicy<T>).SetMissingValues(signalsDataRepository, signal, fromIncludedUtc, toExcludedUtc);
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
                    fromIncludedUtc = AddTimestamp(signal, fromIncludedUtc);
                }
                while (fromIncludedUtc < toExcludedUtc);

                return datumList;
            }
        }  

        public void SetMVP(Signal domainSetMVPSignal, MissingValuePolicyBase domainPolicyBase)
        {
            if(domainPolicyBase.GetType().GetGenericTypeDefinition() == typeof(ShadowMissingValuePolicy<>))
            {
                dynamic shadowPolicy = domainPolicyBase;
                if (shadowPolicy.ShadowSignal.Granularity != domainSetMVPSignal.Granularity || shadowPolicy.ShadowSignal.DataType != domainSetMVPSignal.DataType)
                    throw new InvalidSignalForShadowing();

                // ----------------------------------------------------------------------

                if (domainPolicyBase.NativeDataType == typeof(bool))
                {
                    Signal shadowOfSignalWhichHasToBeShadow =
                    (missingValuePolicyRepository.Get
                    ((domainPolicyBase as ShadowMissingValuePolicy<bool>).ShadowSignal) as ShadowMissingValuePolicy<bool>).ShadowSignal;

                    if (shadowOfSignalWhichHasToBeShadow == null) return;
                    if (shadowOfSignalWhichHasToBeShadow.Id == domainSetMVPSignal.Id) throw new Exception();
                }
                /*if (domainPolicyBase.NativeDataType == typeof(int))
                {
                    var shadowOfSignalWhichHasToBeShadow = (domainPolicyBase as ShadowMissingValuePolicy<int>).ShadowSignal;
                    if (shadowOfSignalWhichHasToBeShadow == domainSetMVPSignal) throw new Exception();
                }
                if (domainPolicyBase.NativeDataType == typeof(double))
                {
                    var shadowOfSignalWhichHasToBeShadow = (domainPolicyBase as ShadowMissingValuePolicy<double>).ShadowSignal;
                    if (shadowOfSignalWhichHasToBeShadow == domainSetMVPSignal) throw new Exception();
                }
                if (domainPolicyBase.NativeDataType == typeof(decimal))
                {
                    var shadowOfSignalWhichHasToBeShadow = (domainPolicyBase as ShadowMissingValuePolicy<decimal>).ShadowSignal;
                    if (shadowOfSignalWhichHasToBeShadow == domainSetMVPSignal) throw new Exception();
                }
                if (domainPolicyBase.NativeDataType == typeof(string))
                {
                    var shadowOfSignalWhichHasToBeShadow = (domainPolicyBase as ShadowMissingValuePolicy<string>).ShadowSignal;
                    if (shadowOfSignalWhichHasToBeShadow == domainSetMVPSignal) throw new Exception();
                }*/

            }
            this.missingValuePolicyRepository.Set(domainSetMVPSignal, domainPolicyBase);
        }

        public MissingValuePolicy.MissingValuePolicyBase GetMVP(Signal domainSignal)
        {
            var result = this.missingValuePolicyRepository.Get(domainSignal);
            if (result == null) return null;
            else return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
        }

       

        public void SetData<T>(Signal setDataSignal, IEnumerable<Datum<T>> datum)
        {
            Granularity granularity = setDataSignal.Granularity;

            datum.Any(s=>ChekCorrectTimeStamp<T>(granularity, s.Timestamp));

            this.signalsDataRepository.SetData(datum.Select(d => { d.Signal = setDataSignal; return d; }).ToList());
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

        public IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            ChekCorrectTimeStamp<T>(signal.Granularity, fromIncludedUtc);
            ChekCorrectTimeStamp<T>(signal.Granularity, toExcludedUtc);
            ChekCorrectTimeStamp<T>(granularity, fromIncludedUtc);
            ChekCorrectTimeStamp<T>(granularity, toExcludedUtc);

            if (fromIncludedUtc > toExcludedUtc) return new Datum<T>[] { };
            {
                var MVP = GetMVP(signal);
                List<Datum<T>> filledData = new List<Datum<T>>();
                List<Datum<T>> reducedData = new List<Datum<T>>();
                DateTime tmp = fromIncludedUtc;

                if (fromIncludedUtc == toExcludedUtc) toExcludedUtc = AddTimestamp(granularity, toExcludedUtc);

                if (MVP is ZeroOrderMissingValuePolicy<T>) filledData = 
                        (MVP as ZeroOrderMissingValuePolicy<T>).SetMissingValues(signalsDataRepository, signal, fromIncludedUtc, toExcludedUtc).ToList();
                else if (MVP is FirstOrderMissingValuePolicy<T>)
                    filledData = (MVP as FirstOrderMissingValuePolicy<T>).SetMissingValues(signalsDataRepository, signal, fromIncludedUtc, toExcludedUtc).ToList();
                else if (MVP is ShadowMissingValuePolicy<T>)
                    filledData = (MVP as ShadowMissingValuePolicy<T>).SetMissingValues(signalsDataRepository, signal, fromIncludedUtc, toExcludedUtc).ToList();
                else filledData = SetMissingValuesWhenGivenIsNoneQualityMVPOrSpecificValueMVP<T>(signal, fromIncludedUtc, toExcludedUtc, MVP);                

                do
                {
                    IEnumerable<Datum<T>> dataFromOnePeriod = FillDataFromOnePeriod(granularity, filledData, tmp);
                    double sum = 0;
                    int count = dataFromOnePeriod.Count();
                    foreach (var datum in dataFromOnePeriod) sum += (double)(Convert.ChangeType(datum.Value, typeof(double)));
                    reducedData.Add(new Datum<T>() { Quality = FindLeastQuality(dataFromOnePeriod), Timestamp = tmp, Value = (T)(Convert.ChangeType((sum / count), typeof(T))) });
                    tmp = AddTimestamp(granularity, tmp);
                } while (tmp < toExcludedUtc);

                return reducedData;
            }
        }




        private void SetDefaultMVPForSignal(Signal signal)
        {
            switch (signal.DataType)
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

        private DateTime AddTimestamp(Signal signal, DateTime datetime)
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

        private DateTime AddTimestamp(Granularity granularity, DateTime datetime)
        {
            switch (granularity)
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

        private SpecificValueMissingValuePolicy<T> GetMVPData<T>(MissingValuePolicyBase MVP, List<Datum<T>> datums)
        {
            if (MVP is Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<T>)
                return MVP as SpecificValueMissingValuePolicy<T>;

            return new SpecificValueMissingValuePolicy<T>() { Quality = Quality.None, Value = default(T) };
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


        private List<Datum<T>> SetMissingValuesWhenGivenIsNoneQualityMVPOrSpecificValueMVP<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc, MissingValuePolicyBase MVP)
        {
            List<Datum<T>> datumList = new List<Datum<T>>();
            if (fromIncludedUtc > toExcludedUtc) return datumList.ToList();
            var tempDatum = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
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
                fromIncludedUtc = AddTimestamp(signal, fromIncludedUtc);
            }
            while (fromIncludedUtc < toExcludedUtc);

            return datumList.ToList();
        }

        private IEnumerable<Datum<T>> FillDataFromOnePeriod <T>(Granularity granularity, IEnumerable<Datum<T>> filledData, DateTime tmp)
        {
            switch (granularity)
            {
                case Granularity.Second: return filledData.Where(d => (d.Timestamp >= tmp) && (d.Timestamp < tmp.AddSeconds(1)));
                case Granularity.Minute: return filledData.Where(d => (d.Timestamp >= tmp) && (d.Timestamp < tmp.AddMinutes(1)));
                case Granularity.Hour: return filledData.Where(d => (d.Timestamp >= tmp) && (d.Timestamp < tmp.AddHours(1)));
                case Granularity.Day: return filledData.Where(d => (d.Timestamp >= tmp) && (d.Timestamp < tmp.AddDays(1)));
                case Granularity.Week: return filledData.Where(d => (d.Timestamp >= tmp) && (d.Timestamp < tmp.AddDays(7)));
                case Granularity.Month: return filledData.Where(d => (d.Timestamp >= tmp) && (d.Timestamp < tmp.AddMinutes(1)));
                case Granularity.Year: return filledData.Where(d => (d.Timestamp >= tmp) && (d.Timestamp < tmp.AddYears(1)));
                default: throw new Exception();
            }
        }

        private Quality FindLeastQuality<T>(IEnumerable<Datum<T>> dataFromOnePeriod)
        {
            Quality leastQuality = Quality.Good;
            foreach (var datum in dataFromOnePeriod)
            {
                if (datum.Quality == Quality.None) return Quality.None;
                if (datum.Quality > leastQuality) leastQuality = datum.Quality;
            }
            return leastQuality;
        }
    }
}