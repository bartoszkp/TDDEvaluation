using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Mapster;
using DataAccess.DataFillHelpers;
using Domain.Services.DataFillHelpers;

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
                throw new IdNotNullException();
            var signal = this.signalsRepository.Add(newSignal);
            if (missingValuePolicyRepository == null)
                return signal;
            var switchDataType = new Dictionary<DataType, Action>()
            {
                {DataType.Boolean, ()=>this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<bool>()) },
                {DataType.Decimal, ()=>this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<decimal>()) },
                {DataType.Double, ()=>this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<double>()) },
                {DataType.Integer, ()=>this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<int>()) },
                {DataType.String, ()=>this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<string>()) }
            };
            switchDataType[signal.DataType].Invoke();

            return signal;
        }

        public Signal GetById(int signalId)
        {
            var result = signalsRepository.Get(signalId);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public Signal Get(Path pathDomain)
        {
            var result = signalsRepository.Get(pathDomain);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            if (signal == null)
            {
                throw new NoSuchSignalException();
            }
            var result = this.missingValuePolicyRepository.Get(signal);
            if (result == null)
                throw new NonExistMissingValuePolicy();
            else
                return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetMissingValuePolicyBase(int signalId, MissingValuePolicyBase policy)
        {
            var signal = signalsRepository.Get(signalId);
            if (signal == null)
                throw new NoSuchSignalException();
            policy.CheckGranularitiesAndDataTypes(signal);
            policy.IsDependencyCycle(signal, missingValuePolicyRepository);
            missingValuePolicyRepository.Set(signal, policy);
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId);
            Datum<T> secondaryItem = new Datum<T>() { Signal = signal, Timestamp = fromIncludedUtc };
            VerifyTimeStamp<T>(signal.Granularity, secondaryItem.Timestamp);
            secondaryItem = new Datum<T>() { Signal = signal, Timestamp = toExcludedUtc };
            VerifyTimeStamp<T>(signal.Granularity, secondaryItem.Timestamp);
            DateTime dataFrom= new DateTime();
            var dataOlder = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1);
            if(dataOlder.Count()!=0)
            {
                if (dataOlder.ElementAt(0).Timestamp < toExcludedUtc)
                {
                    dataFrom = dataOlder.ElementAt(0).Timestamp;
                }
            }
            else
            {
                dataFrom = fromIncludedUtc;
            }
            var data = this.signalsDataRepository
                .GetData<T>(signal, dataFrom, toExcludedUtc);

            foreach (var item in data)
            {
                VerifyTimeStamp<T>(signal.Granularity, item.Timestamp);
            }
            if (fromIncludedUtc == toExcludedUtc)
            {
                List<Datum<T>> returnElement = new List<Datum<T>>();
                int indeks = 0;
                if (data.Count() == 0)
                {
                    returnElement.Add(secondaryItem);
                    return returnElement;
                }
                    
                foreach (var x in data)
                {
                    if (fromIncludedUtc == x.Timestamp)
                    {
                        returnElement.Add(x);
                        return returnElement;
                    }
                    indeks++;
                }
            }
            if (missingValuePolicyRepository == null)
                return data;
            var missingValuePolicy = GetMissingValuePolicy(signal) as MissingValuePolicy.MissingValuePolicy<T>;
            return missingValuePolicy.FillData(signal, data, fromIncludedUtc, toExcludedUtc,signalsDataRepository).ToArray();
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> dataDomain)
        {
            var signal = GetById(signalId);

            foreach (var item in dataDomain)
            {
                item.Signal = signal;
            }
            if ((dataDomain != null) && (signal != null))
                foreach (var item in dataDomain)
                {
                    VerifyTimeStamp<T>(signal.Granularity, item.Timestamp);
                }
            signalsDataRepository.SetData(dataDomain);
        }

        public PathEntry GetPathEntry(Path prefixPath)
        {
            var matchingSignals = this.signalsRepository.GetAllWithPathPrefix(prefixPath);

            if (matchingSignals == null)
                return null;

            var filteredMatchingSignals = matchingSignals.Where(s => s.Path.Length == prefixPath.Length + 1);

            var signalsInSubPaths = matchingSignals.Where(s => s.Path.Length > prefixPath.Length + 1);

            var subPaths = CreateSubPathsList(signalsInSubPaths, prefixPath);


            return new PathEntry(filteredMatchingSignals, subPaths);

        }

        public void Delete(int signalId)
        {
            var signal = GetById(signalId);
            if (signal == null)
                throw new IdNotNullException();
            SetMissingValuePolicyBase(signalId, null);
            var switchDataType = new Dictionary<DataType, Action>()
            {
                {DataType.Boolean,()=>signalsDataRepository.DeleteData<bool>(signal) },
                {DataType.Decimal,()=>signalsDataRepository.DeleteData<decimal>(signal) },
                {DataType.Double,()=>signalsDataRepository.DeleteData<double>(signal) },
                {DataType.Integer,()=>signalsDataRepository.DeleteData<int>(signal) },
                {DataType.String,()=>signalsDataRepository.DeleteData<string>(signal) }
            };
            switchDataType[signal.DataType].Invoke();
            signalsRepository.Delete(signal);
        }



        private List<Path> CreateSubPathsList(IEnumerable<Signal> signalsInSubPaths, Path prefixPath)
        {
            var subPaths = new List<Path>();

            foreach (var signal in signalsInSubPaths)
            {
                var filteredComponents = new string[prefixPath.Length + 1];

                for (int i = 0; i < filteredComponents.Length; i++)
                {
                    filteredComponents[i] = signal.Path.Components.ElementAt(i);
                }

                Path subpath = Path.FromString(Path.JoinComponents(filteredComponents));
                if (subPaths.Find(s => s.Equals(subpath)) == null)
                    subPaths.Add(subpath);
            }

            return subPaths;
        }


        #region SetupGranularity
        public void VerifyTimeStamp<T>(Granularity granularity, DateTime timestamp)
        {
            var checkGranularity = new Dictionary<Granularity, Action>
            {
                {Granularity.Second, () => GranularitySecond<T>(timestamp) },
                {Granularity.Hour, () => GranularityHour<T>(timestamp) },
                {Granularity.Minute, () => GranularityMinute<T>(timestamp) },
                {Granularity.Day, () => GranularityDay<T>(timestamp) },
                {Granularity.Week, () => GranularityWeek<T>(timestamp) },
                {Granularity.Month, () => GranularityMonth<T>(timestamp) },
                {Granularity.Year, () => GranularityYear<T>(timestamp) }
            };
            checkGranularity[granularity].Invoke();
        }
        private void GranularitySecond<T>(DateTime timestamp)
        {
            if (timestamp.Millisecond != 0)
                throw new TimestampHaveWrongFormatException();
        }
        private void GranularityMinute<T>(DateTime timestamp)
        {
            if ((timestamp.Millisecond != 0) || (timestamp.Second != 0))
            {
                throw new TimestampHaveWrongFormatException();
            }
        }
        private void GranularityHour<T>(DateTime timestamp)
        {
            if ((timestamp.Millisecond != 0) || (timestamp.Second != 0) || (timestamp.Minute != 0))
            {
                throw new TimestampHaveWrongFormatException();
            }
        }
        private void GranularityDay<T>(DateTime timestamp)
        {
            if ((timestamp.Millisecond != 0) || (timestamp.Second != 0) || (timestamp.Minute != 0) || (timestamp.Hour != 0))
            {
                throw new TimestampHaveWrongFormatException();
            }
        }
        private void GranularityWeek<T>(DateTime timestamp)
        {
            if ((timestamp.Millisecond != 0) || (timestamp.Second != 0) || (timestamp.Minute != 0) || (timestamp.Hour != 0) || (timestamp.DayOfWeek != DayOfWeek.Monday))
            {
                throw new TimestampHaveWrongFormatException();
            }
        }
        private void GranularityMonth<T>(DateTime timestamp)
        {
            if ((timestamp.Millisecond != 0) || (timestamp.Second != 0) || (timestamp.Minute != 0) || (timestamp.Hour != 0) || (timestamp.Day != 1))
            {
                throw new TimestampHaveWrongFormatException();
            }
        }
        private void GranularityYear<T>(DateTime timestamp)
        {
            if ((timestamp.Millisecond != 0) || (timestamp.Second != 0) || (timestamp.Minute != 0) || (timestamp.Hour != 0) || (timestamp.Day != 1) || (timestamp.Month != 1))
            {
                throw new TimestampHaveWrongFormatException();
            }
        }

        public IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if(fromIncludedUtc > toExcludedUtc)
            {
                return new List<Datum<T>>() { new Datum<T>() { Quality = Quality.None, Timestamp = fromIncludedUtc, Value = default(T) } }.ToArray();
            }


            VerifyTimeStamp<T>(granularity, fromIncludedUtc);
            VerifyTimeStamp<T>(granularity, toExcludedUtc);

            var policy = GetMissingValuePolicy(signal) as MissingValuePolicy.MissingValuePolicy<T>;

            int count;
            dateTimeList = new List<DateTime>();

            if(fromIncludedUtc == toExcludedUtc)
            {
                CreateDateTimeList(fromIncludedUtc, granularity, 1);
                return CreateCoarseDataEnumerable(signal, policy);
            }

            switch (granularity)
            {
                case Granularity.Second:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalSeconds;
                    CreateDateTimeList(fromIncludedUtc, granularity, count);
                    break;
                case Granularity.Minute:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalMinutes;
                    CreateDateTimeList(fromIncludedUtc, granularity, count);
                    break;
                case Granularity.Hour:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).Hours;
                    CreateDateTimeList(fromIncludedUtc, granularity, count);
                    break;
                case Granularity.Day:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays;
                    CreateDateTimeList(fromIncludedUtc, granularity, count);
                    break;
                case Granularity.Week:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays / 7;
                    CreateDateTimeList(fromIncludedUtc, granularity, count);
                    break;
                case Granularity.Month:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays / 30;
                    CreateDateTimeList(fromIncludedUtc, granularity, count);
                    break;
                case Granularity.Year:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays / 365;
                    CreateDateTimeList(fromIncludedUtc, granularity, count);
                    break;
            }
            
            return CreateCoarseDataEnumerable(signal, policy);
        }

        private IEnumerable<Datum<T>> CreateCoarseDataEnumerable<T>(Signal signal, MissingValuePolicy<T> policy)
        {
            List<Datum<T>> coarseDataList = new List<Datum<T>>();
            IEnumerable<Datum<T>> data;
            IEnumerable<Datum<T>> filledData;
            Quality quality;
            T value;

            int index = 1;

            foreach(var timestamp in dateTimeList)
            {
                data = signalsDataRepository.GetData<T>(signal, timestamp, dateTimeList.ElementAt(index));
                filledData = policy.FillData(signal, data, timestamp, dateTimeList.ElementAt(index), signalsDataRepository);

                quality = SetOutQuality(filledData);
                value = SetOutValue(filledData);

                coarseDataList.Add(new Datum<T>() { Quality = quality, Timestamp = timestamp, Value = value });
                if (dateTimeList.ElementAt(index) == dateTimeList.Last())
                    break;
                index++;
            }

            return coarseDataList.ToArray();
        }

        private T SetOutValue<T>(IEnumerable<Datum<T>> filledData)
        {
            return (T)Convert.ChangeType(filledData.Average(d => Convert.ToDecimal(d.Value)), typeof(T));
        }

        private Quality SetOutQuality<T>(IEnumerable<Datum<T>> filledData)
        {
            var tempQuality = Quality.Good;
            foreach (var item in filledData)
            {
                if (item.Quality == Quality.None)
                    return Quality.None;
                else if (item.Quality > tempQuality)
                    tempQuality = item.Quality;
            }
            return tempQuality;
        }

        private void CreateDateTimeList(DateTime fromIncludedUtc, Granularity granularity, int count)
        {
            var timestamp = fromIncludedUtc;
            for (int i = 0; i < count + 1; i++)
            {
                dateTimeList.Add(timestamp);
                timestamp = granularityToTimestampDictionary[granularity](timestamp);
            }
        }

        private Dictionary<Granularity, Func<DateTime, DateTime>> granularityToTimestampDictionary =
            new Dictionary<Granularity, Func<DateTime, DateTime>>()
        {
                {Granularity.Second, time => time.AddSeconds(1) },
                {Granularity.Minute, time => time.AddMinutes(1) },
                {Granularity.Hour, time => time.AddHours(1) },
                {Granularity.Day, time => time.AddDays(1) },
                {Granularity.Week, time => time.AddDays(7) },
                {Granularity.Month, time => time.AddMonths(1) },
                {Granularity.Year, time => time.AddYears(1) }
        };

        private List<DateTime> dateTimeList;

        #endregion
    }
}
