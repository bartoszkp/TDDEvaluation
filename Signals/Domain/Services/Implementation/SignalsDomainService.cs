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
                if (ValidateTimestamp(d.Timestamp, signal.Granularity))
                    d.Signal = signal;
                else 
                    throw new InvalidTimestampException();
            }

            this.signalsDataRepository.SetData<T>(ListOfDatum);
        }

        

        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (!ValidateTimestamp(fromIncludedUtc, signal.Granularity) || !ValidateTimestamp(toExcludedUtc, signal.Granularity))
                throw new InvalidTimestampException();

            var policy = GetMissingValuePolicy(signal);
            if(policy != null)
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

                                    Datum<T> addingItem;

                                    if (policy.GetType() == typeof(SpecificValueMissingValuePolicy<T>))
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.Fair, Timestamp = checkedDateTime, Value = ((SpecificValueMissingValuePolicy<T>)policy).Value };
                                    }

                                    if (policy.GetType() == typeof(ZeroOrderMissingValuePolicy<T>))
                                    {
                                        if (i == 0)
                                        {
                                            addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                        } 
                                        else
                                        {
                                            var previousItem = returnList.ElementAt(i - 1);
                                            addingItem = new Datum<T>() { Quality = previousItem.Quality, Timestamp = checkedDateTime, Value = previousItem.Value };
                                        }

                                        
                                    }

                                    else
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                    }
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

                                    Datum<T> addingItem;

                                    if (policy.GetType() == typeof(SpecificValueMissingValuePolicy<T>))
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.Fair, Timestamp = checkedDateTime, Value = ((SpecificValueMissingValuePolicy<T>)policy).Value };
                                    }

                                    if (policy.GetType() == typeof(ZeroOrderMissingValuePolicy<T>))
                                    {
                                        if (i == 0)
                                        {
                                            addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                        }
                                        else
                                        {
                                            var previousItem = returnList.ElementAt(i - 1);
                                            addingItem = new Datum<T>() { Quality = previousItem.Quality, Timestamp = checkedDateTime, Value = previousItem.Value };
                                        }
                                    }

                                    else
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                    }
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

                                    Datum<T> addingItem;

                                    if (policy.GetType() == typeof(SpecificValueMissingValuePolicy<T>))
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.Fair, Timestamp = checkedDateTime, Value = ((SpecificValueMissingValuePolicy<T>)policy).Value };
                                    }

                                    if (policy.GetType() == typeof(ZeroOrderMissingValuePolicy<T>))
                                    {
                                        if (i == 0)
                                        {
                                            addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                        }
                                        else
                                        {
                                            var previousItem = returnList.ElementAt(i - 1);
                                            addingItem = new Datum<T>() { Quality = previousItem.Quality, Timestamp = checkedDateTime, Value = previousItem.Value };
                                        }
                                    }

                                    else
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                    }
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

                                    Datum<T> addingItem;

                                    if (policy.GetType() == typeof(SpecificValueMissingValuePolicy<T>))
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.Fair, Timestamp = checkedDateTime, Value = ((SpecificValueMissingValuePolicy<T>)policy).Value };
                                    }

                                    if (policy.GetType() == typeof(ZeroOrderMissingValuePolicy<T>))
                                    {
                                        if (i == 0)
                                        {
                                            addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                        }
                                        else
                                        {
                                            var previousItem = returnList.ElementAt(i - 1);
                                            addingItem = new Datum<T>() { Quality = previousItem.Quality, Timestamp = checkedDateTime, Value = previousItem.Value };
                                        }
                                    }

                                    else
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                    }
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
                            var time = toExcludedUtc - fromIncludedUtc;

                            int countElementOfList = (int)time.TotalSeconds;
                            if (countElementOfList + 1 == gettingList?.Length)
                                return gettingList;
                            for (int i = 0; i < countElementOfList; i++)
                            {

                                Datum<T> xx = gettingList.FirstOrDefault(x => x.Timestamp == checkedDateTime);
                                if (xx == null)
                                {

                                    Datum<T> addingItem;

                                    if (policy.GetType() == typeof(SpecificValueMissingValuePolicy<T>))
                                    {
                                        addingItem = new Datum<T>() { Quality = ((SpecificValueMissingValuePolicy<T>)policy).Quality, Timestamp = checkedDateTime, Value = ((SpecificValueMissingValuePolicy<T>)policy).Value };
                                    }

                                    else if (policy.GetType() == typeof(ZeroOrderMissingValuePolicy<T>))
                                    {
                                        if (i == 0)
                                        {
                                            addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                        }
                                        else
                                        {
                                            var previousItem = returnList.ElementAt(i - 1);
                                            addingItem = new Datum<T>() { Quality = previousItem.Quality, Timestamp = checkedDateTime, Value = previousItem.Value };
                                        }
                                    }

                                    else
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                    }
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

                                    Datum<T> addingItem;

                                    if (policy.GetType() == typeof(SpecificValueMissingValuePolicy<T>))
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.Fair, Timestamp = checkedDateTime, Value = ((SpecificValueMissingValuePolicy<T>)policy).Value };
                                    }

                                    if (policy.GetType() == typeof(ZeroOrderMissingValuePolicy<T>))
                                    {
                                        if (i == 0)
                                        {
                                            addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                        }
                                        else
                                        {
                                            var previousItem = returnList.ElementAt(i - 1);
                                            addingItem = new Datum<T>() { Quality = previousItem.Quality, Timestamp = checkedDateTime, Value = previousItem.Value };
                                        }
                                    }

                                    else
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                    }
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

                                    Datum<T> addingItem;

                                    if (policy.GetType() == typeof(SpecificValueMissingValuePolicy<T>))
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.Fair, Timestamp = checkedDateTime, Value = ((SpecificValueMissingValuePolicy<T>)policy).Value };
                                    }

                                    if (policy.GetType() == typeof(ZeroOrderMissingValuePolicy<T>))
                                    {
                                        if (i == 0)
                                        {
                                            addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                        }
                                        else
                                        {
                                            var previousItem = returnList.ElementAt(i - 1);
                                            addingItem = new Datum<T>() { Quality = previousItem.Quality, Timestamp = checkedDateTime, Value = previousItem.Value };
                                        }
                                    }

                                    else
                                    {
                                        addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                    }
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

        public PathEntry GetPathEntry(Path path)
        {
            var signals = signalsRepository.GetAllWithPathPrefix(path);
            List<Signal> ListOfSignals = new List<Signal>();
            List<Path> SubPaths = new List<Path>();

            if (path == null)
                return null;
            else
            {
                foreach (var item in signals)
                {
                    if (item.Path.Length == path.Length + 1)
                    {
                        ListOfSignals.Add(item);
                        continue;
                    }
                    if (item.Path.Length > path.Length)
                    {
                        var subpath = item.Path.GetPrefix(path.Length + 1);
                        if (!SubPaths.Contains(subpath))
                            SubPaths.Add(subpath);
                    }
                }
                return new PathEntry(ListOfSignals, SubPaths);
            }

        }


        private bool ValidateTimestamp(DateTime timestamp,Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return timestamp.Millisecond == 0;
                    
                case Granularity.Minute:
                    return timestamp.Second == 0 && timestamp.Millisecond == 0;

                case Granularity.Hour:
                    return timestamp.Minute == 0 && timestamp.Second == 0 && timestamp.Millisecond == 0;

                case Granularity.Day:
                    return timestamp.Hour == 0 && timestamp.Minute == 0 && timestamp.Second == 0 && timestamp.Millisecond == 0;

                case Granularity.Week:
                    return timestamp.DayOfWeek == DayOfWeek.Monday && timestamp.Hour == 0 && timestamp.Minute == 0 && timestamp.Second == 0 && timestamp.Millisecond == 0;

                case Granularity.Month:
                    return timestamp.Day == 1 && timestamp.Hour == 0 && timestamp.Minute == 0 && timestamp.Second == 0 && timestamp.Millisecond == 0;

                case Granularity.Year:
                    return timestamp.Month == 1 &&  timestamp.Day == 1 && timestamp.Hour == 0 && timestamp.Minute == 0 && timestamp.Second == 0 && timestamp.Millisecond == 0;

                default:
                    break;
            }

            return false;
        }


    }
}
