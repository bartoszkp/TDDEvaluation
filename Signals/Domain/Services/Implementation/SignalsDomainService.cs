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
            if (newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            var result = this.signalsRepository.Add(newSignal);
            var dataTypeSwitch = new Dictionary<DataType, Action>
            {
                { DataType.Boolean,()=>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<bool>()) },
                { DataType.Decimal, () =>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<decimal>())},
                { DataType.Double,() =>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<double>())},
                { DataType.Integer,()=>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<int>())},
                { DataType.String, ()=>missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<string>())}
            };
            dataTypeSwitch[newSignal.DataType].Invoke();
            return result;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path newPath)
        { 
            if(newPath.Components.Equals(""))
            {
                throw new PathIsEmptyException();
            }

            return this.signalsRepository.Get(newPath);
        }

        public void SetMissingValuePolicy(Domain.Signal signal, MissingValuePolicyBase mvpDomain)
        {
            this.missingValuePolicyRepository.Set(signal, mvpDomain);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var missingValuePolicy = this.missingValuePolicyRepository?.Get(signal);
            if (missingValuePolicy == null)
                return null;
            return TypeAdapter.Adapt(missingValuePolicy, missingValuePolicy.GetType(), missingValuePolicy.GetType().BaseType)
                    as MissingValuePolicy.MissingValuePolicyBase;
            
        }
        
        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
           
            if (GetMissingValuePolicy(signal) == null)
            {
                var gettingList = this.signalsDataRepository?.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();
                var returnList = new List<Datum<T>>();
                DateTime checkedDateTime = fromIncludedUtc;
                switch (signal.Granularity)
                {
                    case Granularity.Minute:
                        {
                            int countElementOfList = toExcludedUtc.Minute - fromIncludedUtc.Minute;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
                            returnList=createListDatum<T>(countElementOfList, checkedDateTime, gettingList, returnList,Granularity.Minute);
                            break;
                        }
                    case Granularity.Month:
                        {
                            int countElementOfList = toExcludedUtc.Month - fromIncludedUtc.Month;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
                            returnList = createListDatum<T>(countElementOfList, checkedDateTime, gettingList, returnList,Granularity.Month);
                            break;
                        }
                    case Granularity.Hour:
                        {
                            int countElementOfList = toExcludedUtc.Hour - fromIncludedUtc.Hour;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
                            returnList = createListDatum<T>(countElementOfList, checkedDateTime, gettingList, returnList,Granularity.Hour);
                            break;
                        }
                    case Granularity.Day:
                        {
                            int countElementOfList = toExcludedUtc.Day - fromIncludedUtc.Day;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
                            returnList = createListDatum<T>(countElementOfList, checkedDateTime, gettingList, returnList, Granularity.Day);
                            break;
                        }
                    case Granularity.Second:
                        {
                            int countElementOfList = toExcludedUtc.Second - fromIncludedUtc.Second;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
                            returnList = createListDatum<T>(countElementOfList, checkedDateTime, gettingList, returnList, Granularity.Second);
                            break;
                        }
                    case Granularity.Week:
                        {
                            int countElementOfList = toExcludedUtc.DayOfYear / 7 - fromIncludedUtc.Second / 7;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
                            createListDatum<T>(countElementOfList, checkedDateTime, gettingList, returnList,Granularity.Week);
                            break;
                        }
                    case Granularity.Year:
                        {
                            int countElementOfList = toExcludedUtc.Year - fromIncludedUtc.Year;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
                            createListDatum<T>(countElementOfList, checkedDateTime, gettingList, returnList,Granularity.Year);
                            break;
                        }
                }

                return returnList;

            }
            return this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> datum)
        {
            foreach (var d in datum)
            {
                d.Signal = signal;
            }
            this.signalsDataRepository.SetData<T>(datum);
        }










        public List<Datum<T>> createListDatum<T>(int countElementOfList, DateTime checkedDateTime, Datum<T>[] gettingList, List<Datum<T>> returnList, Granularity granularity)
        {
            for (int i = 0; i < countElementOfList; i++)
            {
                Datum<T> xx = gettingList.FirstOrDefault(x => x.Timestamp == checkedDateTime);
                if (xx == null)
                {
                    var addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                    returnList.Add(addingItem);
                }
                else
                    returnList.Add(xx);
                var addTimeSpan = new Dictionary<Granularity, Action>
                {
                    {Granularity.Day,() => checkedDateTime = checkedDateTime.AddDays(1)},
                    {Granularity.Hour,() => checkedDateTime = checkedDateTime.AddHours(1)},
                    {Granularity.Minute,() => checkedDateTime = checkedDateTime.AddMinutes(1)},
                    {Granularity.Month,() => checkedDateTime = checkedDateTime.AddMonths(1)},
                    {Granularity.Second,() => checkedDateTime = checkedDateTime.AddSeconds(1)},
                    {Granularity.Week,() => checkedDateTime = checkedDateTime.AddDays(7)},
                    {Granularity.Year,() => checkedDateTime = checkedDateTime.AddYears(1)}
                };
                addTimeSpan[granularity].Invoke();
            }
            return returnList;
        }
    }
}
