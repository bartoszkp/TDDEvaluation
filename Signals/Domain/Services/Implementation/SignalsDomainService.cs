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
            switch (newSignal.DataType)
            {
                case DataType.Boolean:
                    {
                        missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<bool>());
                        break;
                    }
                case DataType.Decimal:
                    {
                        missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<decimal>());
                        break;
                    }
                case DataType.Double:
                    {
                        missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<double>());
                        break;
                    }
                case DataType.Integer:
                    {
                        missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<int>());
                        break;
                    }
                case DataType.String:
                    {
                        missingValuePolicyRepository.Set(result, new NoneQualityMissingValuePolicy<string>());
                        break;
                    }
            }
            return result;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository?.Get(signalId);
        }

        public Signal Get(Path pathDomain)
        {
            var result = signalsRepository.Get(pathDomain);
            if (result == null)
            {
                return null;
            }
            else
            {
                return result; 
            }
        }

        public void SetMissingValuePolicy(Domain.Signal signal, MissingValuePolicyBase missingValuePolicy)
        {
            this.missingValuePolicyRepository.Set(signal, missingValuePolicy);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(Signal signal)
        {
            var mvp = this.missingValuePolicyRepository?.Get(signal);
            if (mvp == null)
            {
                return null;
            }
            else
            {
                return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType) as MissingValuePolicy.MissingValuePolicyBase;
            }
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> datum)
        {
            if(datum == null)
            {
                this.signalsDataRepository.SetData<T>(datum);
                return;
            }
            var ListOfDatum = datum.ToList();
            foreach(var d in ListOfDatum)
            {
                d.Signal = signal;
            }

            this.signalsDataRepository.SetData<T>(ListOfDatum);
        }

        

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            
            if(GetMissingValuePolicy(signal)!=null)
            {
                var gettingList = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();
                var returnList = new List<Datum<T>>();
                var granulitary = signal.Granularity;
                DateTime checkedDateTime = fromIncludedUtc;
                switch (granulitary)
                {
                    case Granularity.Minute:
                        {
                            int countElementOfList = toExcludedUtc.Minute - fromIncludedUtc.Minute;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
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
                                checkedDateTime = checkedDateTime.AddMinutes(1);
                            }
                            break;
                        }
                    case Granularity.Month:
                        {
                            int countElementOfList = toExcludedUtc.Month - fromIncludedUtc.Month;
                            if (countElementOfList+1 == gettingList.Length)
                                return gettingList;
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
                                checkedDateTime = checkedDateTime.AddMonths(1);
                            }
                            break;
                        }
                    case Granularity.Hour:
                        {
                            int countElementOfList = toExcludedUtc.Hour - fromIncludedUtc.Hour;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
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
                                checkedDateTime = checkedDateTime.AddHours(1);
                            }
                            break;
                        }
                    case Granularity.Day:
                        {
                            int countElementOfList = toExcludedUtc.Day-fromIncludedUtc.Day;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
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
                                checkedDateTime = checkedDateTime.AddDays(1);
                            }
                                break;
                        }
                    case Granularity.Second:
                        {
                            int countElementOfList = toExcludedUtc.Second - fromIncludedUtc.Second;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
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
                                checkedDateTime = checkedDateTime.AddSeconds(1);
                            }
                            break;
                        }
                    case Granularity.Week:
                        {
                            int countElementOfList = toExcludedUtc.DayOfYear/7 - fromIncludedUtc.Second/7;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
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
                                checkedDateTime = checkedDateTime.AddDays(7);
                            }
                            break;
                        }
                    case Granularity.Year:
                        {
                            int countElementOfList = toExcludedUtc.Year - fromIncludedUtc.Year;
                            if (countElementOfList + 1 == gettingList.Length)
                                return gettingList;
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
                                checkedDateTime = checkedDateTime.AddYears(1);
                            }
                            break;
                        }
                }
                
               return returnList;
                
            }
            return this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();
        }
    }
}
