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

            var dbItem = this.signalsRepository.Add(newSignal);

            switch (dbItem.DataType)
            {
                case Domain.DataType.Boolean:
                    this.SetMissingValuePolicy(dbItem, new NoneQualityMissingValuePolicy<bool>());
                    break;
                case Domain.DataType.Decimal:
                    this.SetMissingValuePolicy(dbItem, new NoneQualityMissingValuePolicy<decimal>());
                    break;
                case Domain.DataType.Double:
                    this.SetMissingValuePolicy(dbItem, new NoneQualityMissingValuePolicy<double>());
                    break;
                case Domain.DataType.Integer:
                    this.SetMissingValuePolicy(dbItem, new NoneQualityMissingValuePolicy<int>());
                    break;
                case Domain.DataType.String:
                    SetMissingValuePolicy(dbItem, new NoneQualityMissingValuePolicy<string>());
                    break;
            }

            return dbItem;
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

        public void SetMissingValuePolicy(int signalId, MissingValuePolicyBase mvpDomain)
        {
            var signal = signalsRepository.Get(signalId);

            if (signal == null)
            {
                throw new SignalIsNullException();
            }

            this.missingValuePolicyRepository.Set(signal, mvpDomain);
        }

        public void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase mvpDomain)
        {
            if (signal == null)
            {
                throw new SignalIsNullException();
            }

            this.missingValuePolicyRepository.Set(signal, mvpDomain);
        }

        public MissingValuePolicyBase GetMissingValuePolicy(int signalId)
        {
            var signal = signalsRepository.Get(signalId);

            if (signal == null)
            {
                throw new SignalIsNullException();
            }
            else
            {
                var mvp = this.missingValuePolicyRepository.Get(signal);

                return TypeAdapter.Adapt(mvp, mvp.GetType(), mvp.GetType().BaseType)
                    as MissingValuePolicy.MissingValuePolicyBase;
            }
        }

        public IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var signal = this.GetById(signalId);
            var data = signalsDataRepository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);
            var ListOfData = data.OrderBy(x => x.Timestamp).ToList();

            var MVP = GetMissingValuePolicy(signalId);

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

        public void SetData<T>(IEnumerable<Datum<T>> domianModel)
        {
            signalsDataRepository.SetData(domianModel);
        }
    }
}
