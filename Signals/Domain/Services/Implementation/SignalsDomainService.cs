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

        public Signal Add<T>(Signal newSignal, NoneQualityMissingValuePolicy<T> nonePolicy)
        {
            if (newSignal.Id.HasValue)
            {
                throw new IdNotNullException();
            }

            var toReturn = this.signalsRepository.Add(newSignal);

            missingValuePolicyRepository.Set(newSignal, nonePolicy);

            return toReturn;
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

        public MissingValuePolicyBase GetMissingValuePolicyBase(int signalId)
        {
            var signal = signalsRepository.Get(signalId);
            if (signal == null)
            {
                throw new SignalWithThisIdNonExistException();
            }
            var mvp = missingValuePolicyRepository.Get(signal);
            if (mvp == null)
                return null;
            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetMissingValuePolicyBase(int signalId, MissingValuePolicyBase policy)
        {
            var signal = signalsRepository.Get(signalId);
            if (signal == null)
            {
                throw new SignalWithThisIdNonExistException();
            }
            missingValuePolicyRepository.Set(signal, policy);
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new SignalWithThisIdNonExistException();

            var datums = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
            IEnumerable<Datum<T>> sortedDatums = datums.OrderBy(datum => datum.Timestamp);

            sortedDatums = FillMissingData(signal.Granularity, sortedDatums, fromIncludedUtc, toExcludedUtc);

            return sortedDatums;
        }

        public void SetData<T>(int signalId, IEnumerable<Datum<T>> dataDomain)
        {
            Signal signal = GetById(signalId);
            if (signal == null)
                throw new SignalWithThisIdNonExistException();

            var datums = new Datum<T> [dataDomain.Count()];
            int i = 0;
            foreach (Datum<T> d in dataDomain)
            {
                datums[i++] = new Datum<T>
                {
                    Quality = d.Quality,
                    Timestamp = d.Timestamp,
                    Value = d.Value,
                    Signal = signal
                };
            }

            IEnumerable<Datum<T>> sortedDatums = datums.OrderBy(datum => datum.Timestamp);
            signalsDataRepository.SetData<T>(sortedDatums);
        }

        private IEnumerable<Datum<T>> FillMissingData<T>(Domain.Granularity granularity, IEnumerable<Datum<T>> sortedDatums, 
            DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            List<Datum<T>> sortedDatumsList = new List<Datum<T>>();
            sortedDatumsList = sortedDatums.ToList();

            List<Datum<T>> newList = new List<Datum<T>>();

            if (granularity == Granularity.Year)
            {
                int yearsDifference = (toExcludedUtc.Year - fromIncludedUtc.Year);

                if (yearsDifference == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }
                else
                {
                    for(int index = 0, currentYear = fromIncludedUtc.Year; currentYear < toExcludedUtc.Year;
                        index++, currentYear++)
                    {
                        if (sortedDatumsList[index].Timestamp.Year != currentYear)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = fromIncludedUtc,
                            };

                            datumToAdd.Timestamp = new DateTime(currentYear, fromIncludedUtc.Month, fromIncludedUtc.Day, fromIncludedUtc.Hour, fromIncludedUtc.Minute, fromIncludedUtc.Second);

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }
            else if (granularity == Granularity.Month)
            {
                int monthsDifference = (toExcludedUtc.Month - fromIncludedUtc.Month);

                if (monthsDifference == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }
                else
                {
                    for (int index = 0, currentMonth = fromIncludedUtc.Month; currentMonth < toExcludedUtc.Month;
                        index++, currentMonth++)
                    {
                        if (sortedDatumsList[index].Timestamp.Month != currentMonth)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = fromIncludedUtc,
                            };

                            datumToAdd.Timestamp = new DateTime(fromIncludedUtc.Year, currentMonth, fromIncludedUtc.Day, fromIncludedUtc.Hour, fromIncludedUtc.Minute, fromIncludedUtc.Second);

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }
            else if (granularity == Granularity.Week)
            {
                
            }
            else if (granularity == Granularity.Day)
            {
                int daysDifference = (toExcludedUtc.Day - fromIncludedUtc.Day);

                if (daysDifference == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }
                else
                {
                    for (int index = 0, currentDay = fromIncludedUtc.Day; currentDay < toExcludedUtc.Day;
                        index++, currentDay++)
                    {
                        if (sortedDatumsList[index].Timestamp.Day != currentDay)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = fromIncludedUtc,
                            };

                            datumToAdd.Timestamp = new DateTime(fromIncludedUtc.Year, fromIncludedUtc.Month, currentDay, fromIncludedUtc.Hour, fromIncludedUtc.Minute, fromIncludedUtc.Second);

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }
            else if (granularity == Granularity.Hour)
            {
                int hoursDifference = (toExcludedUtc.Hour - fromIncludedUtc.Hour);

                if (hoursDifference == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }
                else
                {
                    for (int index = 0, currentHour = fromIncludedUtc.Hour; currentHour < toExcludedUtc.Hour;
                        index++, currentHour++)
                    {
                        if (sortedDatumsList[index].Timestamp.Hour != currentHour)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = fromIncludedUtc,
                            };

                            datumToAdd.Timestamp = new DateTime(fromIncludedUtc.Year, fromIncludedUtc.Month, fromIncludedUtc.Day, currentHour, fromIncludedUtc.Minute, fromIncludedUtc.Second);

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }
            else if (granularity == Granularity.Minute)
            {
                int minutesDifference = (toExcludedUtc.Minute - fromIncludedUtc.Minute);

                if (minutesDifference == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }
                else
                {
                    for (int index = 0, currentMinute = fromIncludedUtc.Minute; currentMinute < toExcludedUtc.Minute;
                        index++, currentMinute++)
                    {
                        if (sortedDatumsList[index].Timestamp.Minute != currentMinute)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = fromIncludedUtc,
                            };

                            datumToAdd.Timestamp = new DateTime(fromIncludedUtc.Year, fromIncludedUtc.Month, fromIncludedUtc.Day, fromIncludedUtc.Hour, currentMinute, fromIncludedUtc.Second);

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }
            else if (granularity == Granularity.Second)
            {
                int secondsDifference = (toExcludedUtc.Second - fromIncludedUtc.Second);

                if (secondsDifference == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }
                else
                {
                    for (int index = 0, currentSecond = fromIncludedUtc.Second; currentSecond < toExcludedUtc.Second;
                        index++, currentSecond++)
                    {
                        if (sortedDatumsList[index].Timestamp.Second != currentSecond)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = fromIncludedUtc,
                            };

                            datumToAdd.Timestamp = new DateTime(fromIncludedUtc.Year, fromIncludedUtc.Month, fromIncludedUtc.Day, fromIncludedUtc.Hour, fromIncludedUtc.Minute, currentSecond);

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }

            sortedDatumsList.AddRange(newList);
            return sortedDatumsList.OrderBy(datum => datum.Timestamp);
        }
    }
}
