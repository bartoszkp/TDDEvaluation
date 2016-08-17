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
            if (newSignal.Id != null)
                throw new IdNotNullException();

            Signal addedSignal = this.signalsRepository.Add(newSignal);

            var policy = SetNoneQualityMissingValuePolicy(newSignal);
            this.missingValuePolicyRepository.Set(addedSignal, policy);

            return addedSignal;
        }

        public Signal GetById(int signalId)
        {
            return this.signalsRepository.Get(signalId);
        }

        public Signal Get(Path pathDomain)
        {
            var signal = this.signalsRepository.Get(pathDomain);

            return signal;
        }

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase policy)
        {
            var signal = this.GetById(signalId);

            if (signal == null)
                throw new ArgumentException("Signal with given Id not found.");

            this.missingValuePolicyRepository.Set(signal, policy);
        }

        public Domain.MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = this.GetById(signalId);

            if (signal == null)
                throw new ArgumentException("Signal with given Id not found.");

            var mvp = this.missingValuePolicyRepository.Get(signal);

            if (mvp == null)
                throw new ArgumentException("Argument does not exist");

            return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                as MissingValuePolicy.MissingValuePolicyBase;
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> dataDomain)
        {
            List<Datum<T>> dataDomainOrderedList = dataDomain.OrderBy(d => d.Timestamp).ToList();          

            for(int i = 0; i < dataDomain.Count(); ++i)
            {
                dataDomainOrderedList.ElementAt(i).Signal = signal;
            }

            if (dataDomain.Count() > 1)
                dataDomainOrderedList = AddMissingData(dataDomainOrderedList);

            this.signalsDataRepository.SetData<T>(dataDomainOrderedList);
        }


        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var data = this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            var policy = SetNoneQualityMissingValuePolicy(signal);

            return DataFilledWithMissingValues<T>(data, signal, policy ,fromIncludedUtc,toExcludedUtc);
        }

        private IEnumerable<Domain.Datum<T>> DataFilledWithMissingValues<T>(IEnumerable<Domain.Datum<T>> data, 
            Signal signal, MissingValuePolicyBase policy, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            List<Domain.Datum<T>> filledList = new List<Datum<T>>();
            var array = data.OrderBy(x => x.Timestamp).ToArray();
            int indexOfArray = 0;
            DateTime lastIterativeTime;

            if (fromIncludedUtc == toExcludedUtc)
            {
                for (; indexOfArray < array.Length; ++indexOfArray)
                {
                    if (array[indexOfArray].Timestamp == fromIncludedUtc)
                    {
                        filledList.Add(array[indexOfArray]);
                        return filledList;
                    }
                }
            }
            for (DateTime iterativeTime = fromIncludedUtc; iterativeTime<toExcludedUtc;)
            {
                if (indexOfArray < array.Length && array[indexOfArray].Timestamp >= iterativeTime)
                {
                    lastIterativeTime = iterativeTime;
                    AddTimeBasedOnGranulatity(signal.Granularity, ref iterativeTime);

                    if (array[indexOfArray].Timestamp < iterativeTime)
                    {
                        filledList.Add(array.ElementAt(indexOfArray));
                        ++indexOfArray;
                    }
                    else
                    {
                        filledList.Add(GetDatumFilledWithMissingValuePolicy<T>(policy, signal, lastIterativeTime));
                    }
                }
                else
                {
                    lastIterativeTime = iterativeTime;
                    AddTimeBasedOnGranulatity(signal.Granularity, ref iterativeTime);
                    filledList.Add(GetDatumFilledWithMissingValuePolicy<T>(policy, signal, lastIterativeTime));
                }
            }
            return filledList;
        }

        private Domain.Datum<T> GetDatumFilledWithMissingValuePolicy<T>(MissingValuePolicyBase policy,Signal signal, 
            DateTime timestamp)
        {
            if(policy is NoneQualityMissingValuePolicy<T>)
            {
                return new Datum<T>()
                {
                    Id = 0,
                    Signal = signal,
                    Timestamp = timestamp,
                    Value = default(T),
                    Quality = Quality.None
                };
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void AddTimeBasedOnGranulatity(Granularity granularity,ref DateTime dateTime)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    dateTime = dateTime.AddSeconds(1);
                    return;
                case Granularity.Minute:
                    dateTime = dateTime.AddMinutes(1);
                    return;
                case Granularity.Hour:
                    dateTime = dateTime.AddHours(1);
                    return;
                case Granularity.Day:
                    dateTime = dateTime.AddDays(1);
                    return;
                case Granularity.Week:
                    dateTime = dateTime.AddDays(7);
                    return;
                case Granularity.Month:
                    dateTime = dateTime.AddMonths(1);
                    return;
                case Granularity.Year:
                    dateTime = dateTime.AddYears(1);
                    return;

                default: throw new NotImplementedException();
            }
        }

        private MissingValuePolicyBase SetNoneQualityMissingValuePolicy(Signal signal)
        {
            switch (signal.DataType)
            {
                case DataType.Boolean:
                    return new NoneQualityMissingValuePolicy<bool>();
                case DataType.Decimal:
                    return new NoneQualityMissingValuePolicy<decimal>();
                case DataType.Double:
                    return new NoneQualityMissingValuePolicy<double>();
                case DataType.Integer:
                    return new NoneQualityMissingValuePolicy<int>();
                case DataType.String:
                    return new NoneQualityMissingValuePolicy<string>();
                default:
                    throw new TypeUnsupportedException();
            }
        }
        
        private List<Datum<T>> AddMissingData<T>(List<Datum<T>> dataDomainList)
        {       
            dataDomainList = AddDataDependOnGranurality(dataDomainList);

            return dataDomainList.OrderBy( l => l.Timestamp).ToList();
        }


        private List<Datum<T>> AddDataDependOnGranurality<T>(List<Datum<T>> dataDomainList)
        {
            List<Datum<T>> missingDatas = new List<Datum<T>>();

            if (dataDomainList.First().Signal.Granularity == Granularity.Second)
            {
                for (int i = 0; i < dataDomainList.Count - 1; i++)
                {
                    if (dataDomainList[i].Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddSeconds(-1)) != 0)
                    {
                        int addingAmountOfTime = 0;
                        do
                        {
                            addingAmountOfTime++;

                            missingDatas.Add(new Datum<T>()
                            {
                                Id = 0,
                                Quality = Quality.None,
                                Timestamp = dataDomainList[i].Timestamp.AddSeconds(addingAmountOfTime),
                                Signal = dataDomainList[i].Signal,
                                Value = default(T)
                            });

                        } while (missingDatas.Last().Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddSeconds(-1)) < 0);

                    }
                }
            }

            if (dataDomainList.First().Signal.Granularity == Granularity.Minute)
            {
                for (int i = 0; i < dataDomainList.Count - 1; i++)
                {
                    if (dataDomainList[i].Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddMinutes(-1)) != 0)
                    {
                        int addingAmountOfTime = 0;
                        do
                        {
                            addingAmountOfTime++;

                            missingDatas.Add(new Datum<T>()
                            {
                                Id = 0,
                                Quality = Quality.None,
                                Timestamp = dataDomainList[i].Timestamp.AddMinutes(addingAmountOfTime),
                                Signal = dataDomainList[i].Signal,
                                Value = default(T)
                            });

                        } while (missingDatas.Last().Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddMinutes(-1)) < 0);
                    }
                }
            }

            if (dataDomainList.First().Signal.Granularity == Granularity.Hour)
            {
                for (int i = 0; i < dataDomainList.Count - 1; i++)
                {
                    if (dataDomainList[i].Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddHours(-1)) != 0)
                    {
                        int addingAmountOfTime = 0;
                        do
                        {
                            addingAmountOfTime++;

                            missingDatas.Add(new Datum<T>()
                            {
                                Id = 0,
                                Quality = Quality.None,
                                Timestamp = dataDomainList[i].Timestamp.AddHours(addingAmountOfTime),
                                Signal = dataDomainList[i].Signal,
                                Value = default(T)
                            });

                        } while (missingDatas.Last().Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddHours(-1)) < 0);
                    }
                }
            }

            if (dataDomainList.First().Signal.Granularity == Granularity.Day)
            {
                for (int i = 0; i < dataDomainList.Count - 1; i++)
                {
                    if (dataDomainList[i].Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddDays(-1)) != 0)
                    {
                        int addingAmountOfTime = 0;
                        do
                        {
                            addingAmountOfTime++;

                            missingDatas.Add(new Datum<T>()
                            {
                                Id = 0,
                                Quality = Quality.None,
                                Timestamp = dataDomainList[i].Timestamp.AddDays(addingAmountOfTime),
                                Signal = dataDomainList[i].Signal,
                                Value = default(T)
                            });

                        } while (missingDatas.Last().Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddDays(-1)) < 0);
                    }
                }
            }

            if (dataDomainList.First().Signal.Granularity == Granularity.Week)
            {
                for (int i = 0; i < dataDomainList.Count - 1; i++)
                {
                    if (dataDomainList[i].Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddDays(-7)) != 0)
                    {
                        int addingAmountOfTime = 0;
                        do
                        {
                            addingAmountOfTime += 7;

                            missingDatas.Add(new Datum<T>()
                            {
                                Id = 0,
                                Quality = Quality.None,
                                Timestamp = dataDomainList[i].Timestamp.AddDays(addingAmountOfTime),
                                Signal = dataDomainList[i].Signal,
                                Value = default(T)
                            });

                        } while (missingDatas.Last().Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddDays(-7)) < 0);
                    }
                }
            }

            if (dataDomainList.First().Signal.Granularity == Granularity.Month)
            {
                for (int i = 0; i < dataDomainList.Count - 1; i++)
                {
                    if (dataDomainList[i].Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddMonths(-1)) != 0)
                    {
                        int addingAmountOfTime = 0;
                        do
                        {
                            addingAmountOfTime++;

                            missingDatas.Add(new Datum<T>()
                            {
                                Id = 0,
                                Quality = Quality.None,
                                Timestamp = dataDomainList[i].Timestamp.AddMonths(addingAmountOfTime),
                                Signal = dataDomainList[i].Signal,
                                Value = default(T)
                            });

                        } while (missingDatas.Last().Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddMonths(-1)) < 0);
                    }
                }
            }

            if (dataDomainList.First().Signal.Granularity == Granularity.Year)
            {
                for (int i = 0; i < dataDomainList.Count - 1; i++)
                {
                    if (dataDomainList[i].Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddYears(-1)) != 0)
                    {
                        int addingAmountOfTime = 0;
                        do
                        {
                            addingAmountOfTime++;

                            missingDatas.Add(new Datum<T>()
                            {
                                Id = 0,
                                Quality = Quality.None,
                                Timestamp = dataDomainList[i].Timestamp.AddYears(addingAmountOfTime),
                                Signal = dataDomainList[i].Signal,
                                Value = default(T)
                            });

                        } while (missingDatas.Last().Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddYears(-1)) < 0);
                    }
                }
            }

            dataDomainList.AddRange(missingDatas);
            return dataDomainList;
        }

        public PathEntry GetByPrefixPath(Path path)
        {
            var pathEntry = new PathEntry();

            var allSignals = this.signalsRepository.GetAllWithPathPrefix(path);

            return FillPathEntryLists(allSignals, path);
        }

        private PathEntry FillPathEntryLists(IEnumerable<Domain.Signal> signals, Path prefix)
        {
            List<Signal> listOfSignals = new List<Signal>();
            List<Path> listOfPaths = new List<Path>();

            int prefixCount = prefix.Components.ToArray().Length;

            var array = signals.ToArray();

            foreach(var signal in array)
            {
                if (signal.Path.Components.ToArray().Length - 1 == prefixCount)
                {
                    if (PathEquals(signal.Path, prefix))
                    {
                        listOfSignals.Add(signal);
                    }
                }
                else
                {
                    Path path = signal.Path;
                    List<string> components = new List<string>();
                    for(int i = 0; i < prefixCount + 1; ++i)
                    {
                        components.Add(path.Components.ElementAt(i));
                    }
                    string stringComponents = Domain.Path.JoinComponents(components);
                    listOfPaths.Add(Domain.Path.FromString(stringComponents));
                }
            }

            //var distincedList = listOfPaths.GroupBy(x => x).Select(w => w.First()).ToArray();

            return new Domain.PathEntry(listOfSignals, listOfPaths.Distinct());
        }

        private class CompareSameLengthPaths : IEqualityComparer<Path>
        {
            public bool Equals(Path path1, Path path2)
            {
                int size = path1.Components.ToArray().Length;
                for (int i = 0; i < size; ++i)
                {
                    if (path1.Components.ElementAt(i) != path2.Components.ElementAt(i))
                        return true;
                }
                return false;
            }

            public int GetHashCode(Path path)
            {
                return path.Components.ToArray().Length;
            }
        }

        private bool PathEquals(Path expected, Path actual)
        {
            int actualCount = actual.Components.ToArray().Length;
            for (int i = 0; i < actualCount; ++i)
            {
                if (!expected.Components.ElementAt(i).Equals(actual.Components.ElementAt(i)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
