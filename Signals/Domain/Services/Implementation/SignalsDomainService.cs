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
            if(newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            var signal = this.signalsRepository.Add(newSignal);
            if(missingValuePolicyRepository == null)
            {
                return signal;
            }

            string typeName = signal.DataType.GetNativeType().Name;

            switch (typeName)
            {
                case "Int32":
                    SetDefaultMissingValuePolicyToNoneQualityMissingValuePolicy<int>(signal);
                    break;
                case "Double":
                    SetDefaultMissingValuePolicyToNoneQualityMissingValuePolicy<double>(signal);
                    break;
                case "Decimal":
                    SetDefaultMissingValuePolicyToNoneQualityMissingValuePolicy<decimal>(signal);
                    break;
                case "Boolean":
                    SetDefaultMissingValuePolicyToNoneQualityMissingValuePolicy<bool>(signal);
                    break;
                case "String":
                    SetDefaultMissingValuePolicyToNoneQualityMissingValuePolicy<string>(signal);
                    break;
            }
            return signal;
        }


        private void SetDefaultMissingValuePolicyToNoneQualityMissingValuePolicy<T>(Signal signal)
        {
            this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<T>());
        }
        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> data)
        {
            var signal = GetById(signalId);
            
            foreach(var item in data)
            {
                item.Signal = signal;
                
            }
            if ((data != null) && (signal !=null))
            {
                foreach(var item in data)
                {
                    VerifyTimeStamp<T>(signal.Granularity, item);
                }
            }
            signalsDataRepository.SetData(data);
        }
        
        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            
            var signal = GetById(signalId);
            Datum<T> secondaryItem = new Datum<T>() { Signal = signal, Timestamp = fromIncludedUtc };
            VerifyTimeStamp<T>(signal.Granularity, secondaryItem);
            secondaryItem = new Datum<T>() { Signal = signal, Timestamp = toExcludedUtc };
            VerifyTimeStamp<T>(signal.Granularity, secondaryItem);

            var data = this.signalsDataRepository
                .GetData<T>(signal, fromIncludedUtc, toExcludedUtc);            

            foreach (var item in data)
            {
                VerifyTimeStamp<T>(signal.Granularity, item);
            }

            if (fromIncludedUtc == toExcludedUtc)
            {
                List<Datum<T>> returnElement = new List<Datum<T>>();
                int index = 0;
                foreach(var x in data)
                {
                    if (fromIncludedUtc == x.Timestamp)
                    {
                        returnElement.Add(x);
                        return returnElement;
                    }
                    index++;    
                }
            }
            if(missingValuePolicyRepository == null)
            {
                return data;
            }
            var mvp = GetMissingValuePolicy(signal) as MissingValuePolicy.MissingValuePolicy<T>;

            if (mvp is ShadowMissingValuePolicy<T>)
            {
                var shadowMvp = mvp as ShadowMissingValuePolicy<T>;
                var shadowSignalData = GetShadowSignalData<T>(shadowMvp.ShadowSignal.Id.Value, fromIncludedUtc, toExcludedUtc).ToList();
                return shadowMvp.FillData(signal, data.ToList(), shadowSignalData, fromIncludedUtc, toExcludedUtc)
                    .OrderBy(d => d.Timestamp).ToArray();

            }


            var olderDatum = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1).FirstOrDefault();
            var newestDatum = signalsDataRepository.GetDataNewerThan<T>(signal, toExcludedUtc, 1).FirstOrDefault();
            return mvp.FillData(signal, data, fromIncludedUtc, toExcludedUtc, olderDatum, newestDatum).ToArray();
        }

        public Signal GetByPath(Path path)
        {
            var result = signalsRepository.Get(path);

            if (result == null)
                return null;

            return result;
        }

        public void SetMissingValuePolicy(Signal exampleSignal, MissingValuePolicyBase policy)
        {
            this.missingValuePolicyRepository.Set(exampleSignal, policy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var result = this.missingValuePolicyRepository.Get(signal);
            return TypeAdapter.Adapt(result, result.GetType(), result.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
        }

        public PathEntry GetPathEntry(Path pathDomain)
        {
            var signals = signalsRepository.GetAllWithPathPrefix(pathDomain);

            if (signals == null)
                return null;

            var directPathSignals = signals
                .Where(s => s.Path.Length == pathDomain.Length + 1)
                .Select(s => s)
                .ToArray();
            
            var subPaths = signals
                .Where(s => s.Path.Length > pathDomain.Length + 1)
                .Select(s => s.Path.GetPrefix(pathDomain.Length + 1))
                .Distinct()
                .ToArray();
            
            return new PathEntry(directPathSignals, subPaths.AsEnumerable<Path>().ToArray());
        }

        public void Delete(int signalId)
        {
            Signal signal = GetById(signalId);
            missingValuePolicyRepository.Set(signal, null);
            switch (signal.DataType)
            {
                case DataType.Boolean:
                    signalsDataRepository.DeleteData<bool>(signal);
                    break;

                case DataType.Decimal:
                    signalsDataRepository.DeleteData<decimal>(signal);
                    break;

                case DataType.Double:
                    signalsDataRepository.DeleteData<double>(signal);
                    break;

                case DataType.Integer:
                    signalsDataRepository.DeleteData<int>(signal);
                    break;

                case DataType.String:
                    signalsDataRepository.DeleteData<string>(signal);
                    break;
            }
            signalsRepository.Delete(signal);
        }


        public void VerifyTimeStamp<T>(Granularity granularity, Datum<T> checkingElement)
        {
            var checkGranularity = new Dictionary<Granularity, Action>
            {
                {Granularity.Second, () => GranularitySecond<T>(checkingElement) },
                {Granularity.Hour, () => GranularityHour<T>(checkingElement) },
                {Granularity.Minute, () => GranularityMinute<T>(checkingElement) },
                {Granularity.Day, () => GranularityDay<T>(checkingElement) },
                {Granularity.Week, () => GranularityWeek<T>(checkingElement) },
                {Granularity.Month, () => GranularityMonth<T>(checkingElement) },
                {Granularity.Year, () => GranularityYear<T>(checkingElement) }
            };
            checkGranularity[granularity].Invoke();
        }

        private IEnumerable<Datum<T>> GetShadowSignalData<T>(int shadowSignalId,DateTime fromIncluded,DateTime toExcluded)
        {
            var shadowSignal = GetById(shadowSignalId);
            return this.signalsDataRepository.GetData<T>(shadowSignal,fromIncluded,toExcluded);
        }


        private void GranularitySecond<T>(Datum<T> checkingElement)
        {
            if (checkingElement.Timestamp.Millisecond != 0)
                throw new TimestampHaveWrongFormatException();
        }

        private void GranularityMinute<T>(Datum<T> checkingElement)
        {
            if ((checkingElement.Timestamp.Millisecond != 0) || (checkingElement.Timestamp.Second != 0))
            {
                throw new TimestampHaveWrongFormatException();
            }
        }
        private void GranularityHour<T>(Datum<T> checkingElement)
        {
            if ((checkingElement.Timestamp.Millisecond != 0) || (checkingElement.Timestamp.Second != 0) || (checkingElement.Timestamp.Minute != 0))
            {
                throw new TimestampHaveWrongFormatException();
            }
        }
        private void GranularityDay<T>(Datum<T> checkingElement)
        {
            if ((checkingElement.Timestamp.Millisecond != 0) || (checkingElement.Timestamp.Second != 0) || (checkingElement.Timestamp.Minute != 0) || (checkingElement.Timestamp.Hour != 0))
            {
                throw new TimestampHaveWrongFormatException();
            }
        }
        private void GranularityWeek<T>(Datum<T> checkingElement)
        {
            DateTime dd = new DateTime(2000, 1, 1, 0, 0, 0);
            if ((checkingElement.Timestamp.Millisecond != 0) || (checkingElement.Timestamp.Second != 0) || (checkingElement.Timestamp.Minute != 0) || (checkingElement.Timestamp.Hour != 0) || (checkingElement.Timestamp.DayOfWeek != DayOfWeek.Monday))
            {
                throw new TimestampHaveWrongFormatException();
            }
        }
        private void GranularityMonth<T>(Datum<T> checkingElement)
        {
            if ((checkingElement.Timestamp.Millisecond != 0) || (checkingElement.Timestamp.Second != 0) || (checkingElement.Timestamp.Minute != 0) || (checkingElement.Timestamp.Hour != 0) || (checkingElement.Timestamp.Day != 1))
            {
                throw new TimestampHaveWrongFormatException();
            }
        }

        private void GranularityYear<T>(Datum<T> checkingElement)
        {
            if ((checkingElement.Timestamp.Millisecond != 0) || (checkingElement.Timestamp.Second != 0) || (checkingElement.Timestamp.Minute != 0) || (checkingElement.Timestamp.Hour != 0) || (checkingElement.Timestamp.Day != 1) || (checkingElement.Timestamp.Month != 1))
            {
                throw new TimestampHaveWrongFormatException();
            }
        }

        public IEnumerable<Datum<T>> GetCoarseData<T>(int signalId, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = GetById(signalId);

            CheckArguments<T>(signal.Granularity, granularity, fromIncludedUtc, toExcludedUtc);

            decimal averageValue = 0.0m;
            var quality = Domain.Quality.Good;
            var dateTime = fromIncludedUtc;
            var timeStamp = AddOrSubtractDateTime(dateTime, granularity, 1);

            var data = GetData<T>(signalId, fromIncludedUtc, toExcludedUtc);
            var dataToReturn = new List<Datum<T>>();

            int howManyValues = 0;
            foreach (var item in data)
            {
                averageValue += Convert.ToDecimal(item.Value);
                howManyValues++;

                if (item.Quality > quality || item.Quality == Quality.None)
                    quality = item.Quality;

                dateTime = AddOrSubtractDateTime(dateTime, signal.Granularity, 1);

                if (dateTime == timeStamp)
                {
                    dataToReturn.Add(new Datum<T>()
                    {
                        Quality = quality,
                        Signal = signal,
                        Timestamp = AddOrSubtractDateTime(dateTime, granularity, -1),
                        Value = CountAverageValue<T>(averageValue, howManyValues),
                    });

                    quality = Domain.Quality.Good;
                    howManyValues = 0;
                    averageValue = 0.0m;
                    timeStamp = AddOrSubtractDateTime(timeStamp, granularity, 1);
                }
            }

            return dataToReturn;
        }

        private DateTime AddOrSubtractDateTime(DateTime dateTime, Granularity granularity, int addOrSubtract)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return dateTime.AddSeconds(1 * addOrSubtract);

                case Granularity.Minute:
                    return dateTime.AddMinutes(1 * addOrSubtract);

                case Granularity.Hour:
                    return dateTime.AddHours(1 * addOrSubtract);

                case Granularity.Day:
                    return dateTime.AddDays(1 * addOrSubtract);

                case Granularity.Week:
                    return dateTime.AddDays(7 * addOrSubtract);

                case Granularity.Month:
                    return dateTime.AddMonths(1 * addOrSubtract);

                case Granularity.Year:
                    return dateTime.AddYears(1 * addOrSubtract);
            }

            return new DateTime();
        }

        private T CountAverageValue<T>(decimal averageValue, int howManyValues)
        {
            var value = averageValue / howManyValues;
            return (T)Convert.ChangeType(value, typeof(T));
        }

        private void CheckArguments<T>(Granularity signalGranularity, Granularity argumentGranularity, 
            DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            VerifyTimeStamp(argumentGranularity, new Datum<T>()
            {
                Timestamp = fromIncludedUtc,
            });
            VerifyTimeStamp(argumentGranularity, new Datum<T>()
            {
                Timestamp = toExcludedUtc,
            });

            if (signalGranularity >= argumentGranularity)
                throw new Domain.Exceptions.GetCoarseDataBadGranularityException();
        }
    }
}
