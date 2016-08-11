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
            Signal addedSignal = this.signalsRepository.Add(newSignal);

            SetMissingValuePolicyDependsOnSignalDatatype(newSignal);

            return newSignal;
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

        public void SetData<T>(IEnumerable<Datum<T>> dataDomain)
        {
            List<Datum<T>> dataDomainOrderedList = dataDomain.OrderBy(d => d.Timestamp).ToList();          

            if(dataDomain.Count() > 1)
                dataDomainOrderedList = AddMissingData(dataDomainOrderedList);

            this.signalsDataRepository.SetData<T>(dataDomainOrderedList);
        }


        public IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            return this.signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
        }
        
        private void SetMissingValuePolicyDependsOnSignalDatatype(Signal signal)
        {
            switch (signal.DataType)
            {
                case DataType.Boolean:
                    this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<bool>());
                    break;
                case DataType.Decimal:
                    this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<decimal>());
                    break;
                case DataType.Double:
                    this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<double>());
                    break;
                case DataType.Integer:
                    this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<int>());
                    break;
                case DataType.String:
                    this.missingValuePolicyRepository.Set(signal, new NoneQualityMissingValuePolicy<string>());
                    break;
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
                        missingDatas.Add(new Datum<T>()
                        {
                            Id = 0,
                            Quality = Quality.None,
                            Timestamp = dataDomainList[i].Timestamp.AddDays(1),
                            Signal = dataDomainList[i].Signal,
                            Value = default(T)
                        });
                    }
                }
            }

            if (dataDomainList.First().Signal.Granularity == Granularity.Week)
            {
                for (int i = 0; i < dataDomainList.Count - 1; i++)
                {
                    if (dataDomainList[i].Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddDays(-7)) != 0)
                    {
                        missingDatas.Add(new Datum<T>()
                        {
                            Id = 0,
                            Quality = Quality.None,
                            Timestamp = dataDomainList[i].Timestamp.AddDays(7),
                            Signal = dataDomainList[i].Signal,
                            Value = default(T)
                        });
                    }
                }
            }

            if (dataDomainList.First().Signal.Granularity == Granularity.Month)
            {
                for (int i = 0; i < dataDomainList.Count - 1; i++)
                {
                    if (dataDomainList[i].Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddMonths(-1)) != 0)
                    {
                        missingDatas.Add(new Datum<T>()
                        {
                            Id = 0,
                            Quality = Quality.None,
                            Timestamp = dataDomainList[i].Timestamp.AddMonths(1),
                            Signal = dataDomainList[i].Signal,
                            Value = default(T)
                        });
                    }
                }
            }

            if (dataDomainList.First().Signal.Granularity == Granularity.Year)
            {
                for (int i = 0; i < dataDomainList.Count - 1; i++)
                {
                    if (dataDomainList[i].Timestamp.CompareTo(dataDomainList[i + 1].Timestamp.AddYears(-1)) != 0)
                    {
                        missingDatas.Add(new Datum<T>()
                        {
                            Id = 0,
                            Quality = Quality.None,
                            Timestamp = dataDomainList[i].Timestamp.AddYears(1),
                            Signal = dataDomainList[i].Signal,
                            Value = default(T)
                        });
                    }
                }
            }

            dataDomainList.AddRange(missingDatas);
            return dataDomainList;
        }
    }
}
