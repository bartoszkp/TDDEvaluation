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
            VerifyTimeStamp<T>(signal.Granularity, secondaryItem);
            secondaryItem = new Datum<T>() { Signal = signal, Timestamp = toExcludedUtc };
            VerifyTimeStamp<T>(signal.Granularity, secondaryItem);
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
                VerifyTimeStamp<T>(signal.Granularity, item);
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
                    VerifyTimeStamp<T>(signal.Granularity, item);
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

        #endregion
    }
}
