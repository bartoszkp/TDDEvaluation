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

            var datums = new Datum<T>[dataDomain.Count()];
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

            List<Datum<T>> sortedDatumsListToReturn = new List<Datum<T>>();
            sortedDatumsListToReturn = sortedDatums.ToList();

            List<Datum<T>> newList = new List<Datum<T>>();

            if (granularity == Granularity.Year)
            {
                int yearsDifference = (toExcludedUtc.Year - fromIncludedUtc.Year);

                if (sortedDatumsList.Count == 0)
                {
                    for (DateTime currentDate = fromIncludedUtc; currentDate < toExcludedUtc;
                        currentDate = currentDate.AddYears(1))
                    {
                        Datum<T> datumToAdd = new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = currentDate,
                        };

                        sortedDatumsList.Add(datumToAdd);
                    }
                }

                if (yearsDifference == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }
                else
                {
                    DateTime currentDate = fromIncludedUtc;
                    for (int index = 0; currentDate < toExcludedUtc;
                        index++, currentDate = currentDate.AddYears(1))
                    {
                        if (index == sortedDatumsList.Count)
                            sortedDatumsList.Add(new Datum<T>());

                        if (sortedDatumsList[index].Timestamp.Year != currentDate.Year)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = currentDate,
                            };

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }
            else if (granularity == Granularity.Month)
            {
                int monthsDifference = (toExcludedUtc.Year - fromIncludedUtc.Year) * 12 + (toExcludedUtc.Month - fromIncludedUtc.Month);

                if (sortedDatumsList.Count == 0)
                {
                    for (DateTime currentDate = fromIncludedUtc; currentDate < toExcludedUtc;
                        currentDate = currentDate.AddMonths(1))
                    {
                        Datum<T> datumToAdd = new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = currentDate,
                        };

                        sortedDatumsList.Add(datumToAdd);
                    }
                }

                if (monthsDifference == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }
                else
                {
                    DateTime currentDate = fromIncludedUtc;
                    for (int index = 0; currentDate < toExcludedUtc;
                        index++, currentDate = currentDate.AddMonths(1))
                    {
                        if (index == sortedDatumsList.Count)
                            sortedDatumsList.Add(new Datum<T>());

                        if (sortedDatumsList[index].Timestamp != currentDate)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = currentDate,
                            };

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }
            else if (granularity == Granularity.Week)
            {
                double weeksDifference = ((toExcludedUtc - fromIncludedUtc).TotalDays) / 7.0;

                if (sortedDatumsList.Count == 0)
                {
                    for (DateTime currentDate = fromIncludedUtc; currentDate < toExcludedUtc;
                        currentDate = currentDate = currentDate.AddDays(7))
                    {
                        Datum<T> datumToAdd = new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = currentDate,
                        };

                        sortedDatumsList.Add(datumToAdd);
                    }
                }

                if (Math.Ceiling(weeksDifference) == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }
                else
                {
                    DateTime currentDate = fromIncludedUtc;
                    for (int index = 0; currentDate < toExcludedUtc;
                        index++, currentDate = currentDate.AddDays(7))
                    {
                        if (index == sortedDatumsList.Count)
                            sortedDatumsList.Add(new Datum<T>());

                        if (sortedDatumsList[index].Timestamp != currentDate)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = currentDate,
                            };

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }
            else if (granularity == Granularity.Day)
            {
                double daysDifference = (toExcludedUtc - fromIncludedUtc).TotalDays;

                if (sortedDatumsList.Count == 0)
                {
                    for (DateTime currentDate = fromIncludedUtc; currentDate < toExcludedUtc;
                        currentDate = currentDate.AddDays(1))
                    {
                        Datum<T> datumToAdd = new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = currentDate,
                        };

                        sortedDatumsList.Add(datumToAdd);
                    }
                }

                if ((int)daysDifference == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }
                else
                {
                    DateTime currentDate = fromIncludedUtc;
                    for (int index = 0; currentDate < toExcludedUtc;
                        index++, currentDate = currentDate.AddDays(1))
                    {
                        if (index == sortedDatumsList.Count)
                            sortedDatumsList.Add(new Datum<T>());

                        if (sortedDatumsList[index].Timestamp != currentDate)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = currentDate,
                            };

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }
            else if (granularity == Granularity.Hour)
            {
                double hoursDifference = (toExcludedUtc - fromIncludedUtc).TotalHours;

                if ((int)hoursDifference == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }

                if (sortedDatumsList.Count == 0)
                {
                    for (DateTime currentDate = fromIncludedUtc; currentDate < toExcludedUtc;
                        currentDate = currentDate.AddHours(1))
                    {
                        Datum<T> datumToAdd = new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = currentDate,
                        };

                        sortedDatumsList.Add(datumToAdd);
                    }
                }

                else
                {
                    DateTime currentDate = fromIncludedUtc;
                    for (int index = 0; currentDate < toExcludedUtc;
                        index++, currentDate = currentDate.AddHours(1.0))
                    {
                        if (index == sortedDatumsList.Count)
                            sortedDatumsList.Add(new Datum<T>());

                        if (sortedDatumsList[index].Timestamp != currentDate)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = currentDate,
                            };

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }
            else if (granularity == Granularity.Minute)
            {
                double minutesDifference = (toExcludedUtc - fromIncludedUtc).TotalMinutes;

                if (sortedDatumsList.Count == 0)
                {
                    for (DateTime currentDate = fromIncludedUtc; currentDate < toExcludedUtc;
                        currentDate = currentDate.AddMinutes(1))
                    {
                        Datum<T> datumToAdd = new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = currentDate,
                        };

                        sortedDatumsList.Add(datumToAdd);
                    }
                }

                if ((int)minutesDifference == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }
                else
                {
                    DateTime currentDate = fromIncludedUtc;
                    for (int index = 0; currentDate < toExcludedUtc;
                        index++, currentDate = currentDate.AddMinutes(1.0))
                    {
                        if (index == sortedDatumsList.Count)
                            sortedDatumsList.Add(new Datum<T>());

                        if (sortedDatumsList[index].Timestamp != currentDate)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = currentDate,
                            };

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }
            else if (granularity == Granularity.Second)
            {
                double secondsDifference = (toExcludedUtc - fromIncludedUtc).TotalSeconds;

                if (sortedDatumsList.Count == 0)
                {
                    for (DateTime currentDate = fromIncludedUtc; currentDate < toExcludedUtc;
                        currentDate = currentDate.AddSeconds(1))
                    {
                        Datum<T> datumToAdd = new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = currentDate,
                        };

                        sortedDatumsList.Add(datumToAdd);
                    }
                }

                if ((int)secondsDifference == sortedDatumsList.Count() && sortedDatumsList[0].Timestamp == fromIncludedUtc)
                {
                    return sortedDatums;
                }
                else
                {
                    DateTime currentDate = fromIncludedUtc;
                    for (int index = 0; currentDate < toExcludedUtc;
                        index++, currentDate = currentDate.AddSeconds(1.0))
                    {
                        if (index == sortedDatumsList.Count)
                            sortedDatumsList.Add(new Datum<T>());

                        if (sortedDatumsList[index].Timestamp != currentDate)
                        {
                            Datum<T> datumToAdd = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = currentDate,
                            };

                            newList.Add(datumToAdd);
                            index--;
                        }
                    }
                }
            }




            sortedDatumsListToReturn.AddRange(newList);
            return sortedDatumsListToReturn.OrderBy(datum => datum.Timestamp);
        }

        public PathEntry GetPathEntry(Path prefixPath)
        {
            var matchingSignals = this.signalsRepository.GetAllWithPathPrefix(prefixPath);

            if (matchingSignals == null)
                return null;

            var filteredMatchingSignals = matchingSignals.Where(s => s.Path.Length == prefixPath.Length + 1);

            var signalsInSubpaths = matchingSignals.Where(s => s.Path.Length > prefixPath.Length + 1);

            var subpaths = new List<Path>();

            foreach (var signal in signalsInSubpaths)
            {
                var filteredComponents = new string[prefixPath.Length + 1];

                for (int i = 0; i < filteredComponents.Length; i++)
                {
                    filteredComponents[i] = signal.Path.Components.ElementAt(i);
                }

                Path subpath = Path.FromString(Path.JoinComponents(filteredComponents));
                if (subpaths.Find(s => s.Equals(subpath)) == null)
                    subpaths.Add(subpath);


            }


            return new PathEntry(filteredMatchingSignals, subpaths);

        }
    }
}
