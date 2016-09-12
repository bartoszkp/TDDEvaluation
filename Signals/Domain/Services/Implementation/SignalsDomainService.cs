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
            if (datum == null)
            {
                this.signalsDataRepository.SetData<T>(datum);
                return;
            }
            var ListOfDatum = datum.ToList();
            foreach (var d in ListOfDatum)
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
            var datumReturnList = new List<Datum<T>>();

            if (fromIncludedUtc == toExcludedUtc)
            {
                toExcludedUtc = addTime(signal.Granularity, toExcludedUtc);
            }

            if (policy != null)
            {
                var gettingList = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc)?.ToArray();
                var returnList = new List<Datum<T>>();
                var granulitary = signal.Granularity;
                DateTime checkedDateTime = fromIncludedUtc;

                int countElementOfList = TimeDifference(granulitary, toExcludedUtc, fromIncludedUtc);
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
                                returnList = signalsDataRepository.GetDataOlderThan<T>(signal, checkedDateTime, 1).ToList();
                                if (returnList.Count == 0)
                                    addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                else
                                {
                                    returnList[0].Timestamp = checkedDateTime;
                                    addingItem = null;
                                }
                            }
                            else
                            {
                                if (returnList.Count == 0)
                                {
                                    addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                                }
                                else
                                {
                                    var previousItem = returnList.ElementAt(i - 1);
                                    addingItem = new Datum<T>() { Quality = previousItem.Quality, Timestamp = checkedDateTime, Value = previousItem.Value };
                                }
                            }
                        }

                        else if (policy.GetType() == typeof(FirstOrderMissingValuePolicy<T>))
                        {
                            var x0 = signalsDataRepository.GetDataOlderThan<T>(signal, checkedDateTime, 1);
                            var x1 = signalsDataRepository.GetDataNewerThan<T>(signal, checkedDateTime, 1);

                            if (x0.Count() == 0 || x1.Count() == 0)
                            {
                                returnList.Add(new Datum<T>()
                                {
                                    Quality = Quality.None,
                                    Signal = signal,
                                    Timestamp = checkedDateTime,
                                    Value = default(T),
                                });
                            }

                            else
                            {
                                Domain.Quality qualityToAdd;
                                var timeDifference = TimeDifference(granulitary, x1.ElementAt(0).Timestamp, x0.ElementAt(0).Timestamp);
                                var countElementOfListMinusTwo = countElementOfList - 2;

                                var qualityForNewerElement = x1.ElementAt(0).Quality;
                                var qualityForOlderElement = x0.ElementAt(0).Quality;

                                if (qualityForNewerElement < qualityForOlderElement)
                                {
                                    qualityToAdd = qualityForOlderElement;
                                }
                                else if (qualityForOlderElement < qualityForNewerElement)
                                {
                                    qualityToAdd = qualityForNewerElement;
                                }
                                else qualityToAdd = x0.ElementAt(0).Quality;

                                decimal avarage = (Convert.ToDecimal((Convert.ChangeType(x1.ElementAt(0).Value, typeof(T)))) - Convert.ToDecimal(Convert.ChangeType(x0.ElementAt(0).Value, typeof(T)))) / timeDifference;
                                decimal valueToAdd = Convert.ToDecimal(Convert.ChangeType(x0.ElementAt(0).Value, typeof(T)));

                                for (int j = 0; j < timeDifference && j < countElementOfList; j++, i++)
                                {
                                    if (checkedDateTime != toExcludedUtc)
                                    {

                                        if (j == countElementOfListMinusTwo)
                                        {
                                            qualityToAdd = x1.ElementAt(0).Quality;
                                            valueToAdd += avarage;
                                            var itemToAdd = new Datum<T>()
                                            {
                                                Quality = qualityToAdd,
                                                Signal = signal,
                                                Timestamp = checkedDateTime,
                                                Value = (T)Convert.ChangeType(valueToAdd, typeof(T)),
                                            };

                                            returnList.Add(itemToAdd);
                                        }
                                        else
                                        {
                                            valueToAdd += avarage;
                                            var itemToAdd = new Datum<T>()
                                            {
                                                Quality = qualityToAdd,
                                                Signal = signal,
                                                Timestamp = checkedDateTime,
                                                Value = (T)Convert.ChangeType(valueToAdd, typeof(T)),
                                            };

                                            returnList.Add(itemToAdd);
                                            checkedDateTime = addTime(signal.Granularity, checkedDateTime);
                                        }
                                    }
                                }
                                i--;

                                checkedDateTime = addTime(signal.Granularity, checkedDateTime, -1);
                            }
                            addingItem = null;
                        }

                        else
                        {
                            addingItem = new Datum<T>() { Quality = Quality.None, Timestamp = checkedDateTime, Value = default(T) };
                        }
                        if (addingItem != null)
                            returnList.Add(addingItem);
                    }
                    else
                        returnList.Add(xx);
                    checkedDateTime = addTime(signal.Granularity, checkedDateTime);
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


        private bool ValidateTimestamp(DateTime timestamp, Granularity granularity)
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
                    return timestamp.Month == 1 && timestamp.Day == 1 && timestamp.Hour == 0 && timestamp.Minute == 0 && timestamp.Second == 0 && timestamp.Millisecond == 0;

                default:
                    break;
            }

            return false;
        }

        public void Delete(int signalId)
        {
            var signalToDelete = GetById(signalId);
            if (signalToDelete == null)
                throw new Exceptions.SignalDoesNotExists();

            missingValuePolicyRepository.Set(signalToDelete, null);

            switch (signalToDelete.DataType)
            {
                case Domain.DataType.Boolean:
                    signalsDataRepository.DeleteData<bool>(signalToDelete);
                    break;

                case Domain.DataType.Decimal:
                    signalsDataRepository.DeleteData<decimal>(signalToDelete);
                    break;

                case Domain.DataType.Double:
                    signalsDataRepository.DeleteData<double>(signalToDelete);
                    break;

                case Domain.DataType.Integer:
                    signalsDataRepository.DeleteData<int>(signalToDelete);
                    break;

                case Domain.DataType.String:
                    signalsDataRepository.DeleteData<string>(signalToDelete);
                    break;
            }

            signalsRepository.Delete(signalToDelete);
        }

        public DateTime addTime(Granularity granularity, DateTime time, int timeToAdd = 1)
        {
            switch (granularity)
            {
                case Granularity.Day:
                    return time.AddDays(timeToAdd);

                case Granularity.Hour:
                    return time.AddHours(timeToAdd);

                case Granularity.Minute:
                    return time.AddMinutes(timeToAdd);

                case Granularity.Month:
                    return time.AddMonths(timeToAdd);

                case Granularity.Second:
                    return time.AddSeconds(timeToAdd);

                case Granularity.Week:
                    return time.AddDays(7 * timeToAdd);

                case Granularity.Year:
                    return time.AddYears(timeToAdd);
            }

            throw new NotSupportedException("Granularity " + granularity.ToString() + " is not supported");
        }

        public int TimeDifference(Granularity granularity, DateTime newer, DateTime older)
        {
            switch(granularity)
            {
                case Granularity.Day:
                    return (int)(newer - older).TotalDays;

                case Granularity.Hour:
                    return (int)(newer - older).TotalHours;

                case Granularity.Minute:
                    return (int)(newer - older).TotalMinutes;

                case Granularity.Month:
                    return (newer.Year - older.Year) * 12 + newer.Month - older.Month;

                case Granularity.Second:
                    return (int)(newer - older).TotalSeconds;

                case Granularity.Week:
                    return (int)(newer - older).TotalDays / 7;

                case Granularity.Year:
                    return newer.Year - older.Year;
            }

            throw new NotSupportedException("Granularity " + granularity.ToString() + " is not supported");
        }
    }
}
