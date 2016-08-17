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
            var missingValuePolicy = this.missingValuePolicyRepository.Get(signal);
            if (missingValuePolicy == null)
                return null;
            return TypeAdapter.Adapt(missingValuePolicy, missingValuePolicy.GetType(), missingValuePolicy.GetType().BaseType)
                    as MissingValuePolicy.MissingValuePolicyBase;
            
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = this.GetById(signalId);
            var data = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
            var ListOfData = data.OrderBy(x => x.Timestamp).ToList();

            var MVP = GetMissingValuePolicy(signal);

            if(MVP is MissingValuePolicy.NoneQualityMissingValuePolicy<T>)
            {
                for(int i = 0; i < ListOfData.Count - 1; i++)
                {
                    switch(signal.Granularity)
                    {
                        case Granularity.Day:
                            {
                                var time = ListOfData[i + 1].Timestamp - ListOfData[i].Timestamp;
                                for(int y = 0; y <= time.TotalDays; y++)
                                {
                                    var newItem = new Datum<T>()
                                    {
                                        Quality = Quality.None,
                                        Timestamp = ListOfData[i].Timestamp.AddDays(1.0),
                                        Value = default(T)
                                    };
                                    ListOfData.Insert(i + 1, newItem);
                                    i++;
                                } 
                            }
                            break;
                        case Granularity.Hour:
                            {
                                var time = ListOfData[i + 1].Timestamp - ListOfData[i].Timestamp;
                                for (int y = 0; y <= time.TotalHours; y++)
                                {
                                    var newItem = new Datum<T>()
                                    {
                                        Quality = Quality.None,
                                        Timestamp = ListOfData[i].Timestamp.AddHours(1.0),
                                        Value = default(T)
                                    };
                                    ListOfData.Insert(i + 1, newItem);
                                    i++;
                                }
                            }
                            break;
                        case Granularity.Minute:
                            {
                                var time = ListOfData[i + 1].Timestamp - ListOfData[i].Timestamp;
                                for (int y = 0; y <= time.TotalMinutes; y++)
                                {
                                    var newItem = new Datum<T>()
                                    {
                                        Quality = Quality.None,
                                        Timestamp = ListOfData[i].Timestamp.AddMinutes(1.0),
                                        Value = default(T)
                                    };
                                    ListOfData.Insert(i + 1, newItem);
                                    i++;
                                }
                            }
                            break;
                        case Granularity.Month:
                            {
                                var time = ListOfData[i + 1].Timestamp.Month - ListOfData[i].Timestamp.Month + (12 * (ListOfData[i + 1].Timestamp.Year - ListOfData[i].Timestamp.Year)) - 1;
                                for (int y = 0; y <= time - 1; y++)
                                {
                                    var newItem = new Datum<T>()
                                    {
                                        Quality = Quality.None,
                                        Timestamp = ListOfData[i].Timestamp.AddMonths(1),
                                        Value = default(T)
                                    };
                                    ListOfData.Insert(i + 1, newItem);
                                    i++;
                                }
                            }
                            break;
                        case Granularity.Second:
                            {
                                var time = ListOfData[i + 1].Timestamp - ListOfData[i].Timestamp;
                                for (int y = 0; y <= time.TotalSeconds; y++)
                                {
                                    var newItem = new Datum<T>()
                                    {
                                        Quality = Quality.None,
                                        Timestamp = ListOfData[i].Timestamp.AddSeconds(1.0),
                                        Value = default(T)
                                    };
                                    ListOfData.Insert(i + 1, newItem);
                                    i++;
                                }
                            }
                            break;
                        case Granularity.Week:
                            {
                                var time = ListOfData[i + 1].Timestamp - ListOfData[i].Timestamp;
                                for (int y = 0; y <= time.TotalDays / 7; y++)
                                {
                                    var newItem = new Datum<T>()
                                    {
                                        Quality = Quality.None,
                                        Timestamp = ListOfData[i].Timestamp.AddDays(7.0),
                                        Value = default(T)
                                    };
                                    ListOfData.Insert(i + 1, newItem);
                                    i++;
                                }
                            }
                            break;
                        case Granularity.Year:
                            {
                                var time = ListOfData[i + 1].Timestamp.Year - ListOfData[i].Timestamp.Year;
                                for (int y = 0; y <= time; y++)
                                {
                                    var newItem = new Datum<T>()
                                    {
                                        Quality = Quality.None,
                                        Timestamp = ListOfData[i].Timestamp.AddYears(1),
                                        Value = default(T)
                                    };
                                    ListOfData.Insert(i + 1, newItem);
                                    i++;
                                }
                            }
                            break;
                    }
                }
            }
            return ListOfData;
        }

        public void SetData<T>(Signal signal, IEnumerable<Datum<T>> datum)
        {
            foreach (var d in datum)
            {
                d.Signal = signal;
            }
            this.signalsDataRepository.SetData<T>(datum);
        }
    }
}
