using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using Domain.Repositories;
using Mapster;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> datumsIEnumerable, DateTime fromIncludedUtc, DateTime toExcludedUtc, ISignalsDataRepository signalsDataRepository)
        {
            List<Domain.Datum<T>> datumsFirst = new List<Datum<T>>();
            List<Domain.Datum<T>> datums = datumsIEnumerable.ToList<Datum<T>>();
            while (fromIncludedUtc < toExcludedUtc)
            {
                
                var dataOlder = signalsDataRepository.GetDataOlderThan<T>(signal, fromIncludedUtc, 1);
                var dataNewer = signalsDataRepository.GetDataNewerThan<T>(signal, fromIncludedUtc, 1);
                if ((dataOlder == null) || (dataOlder.Count() == 0))
                {
                    if (datums.Find(x => x.Timestamp == fromIncludedUtc) == null)
                    {
                        datumsFirst.Add(new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = fromIncludedUtc
                        });
                    }
                    else
                    {
                        datumsFirst.Add(datums.Find(x => x.Timestamp == fromIncludedUtc));
                    }
                    fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                }
                else if ((dataNewer == null) || (dataNewer.Count() == 0))
                {
                    if (datums.Find(x => x.Timestamp == fromIncludedUtc) == null)
                    {
                        datumsFirst.Add(new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = fromIncludedUtc
                        });
                    }
                    fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                }
                else if(dataOlder.First().Value.Equals(default(T)) || dataNewer.First().Value.Equals(default(T)))
                {
                    datumsFirst.Add(new Datum<T>()
                    {
                        Quality = Quality.None,
                        Value = default(T),
                        Timestamp = fromIncludedUtc
                    });
                    fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                }
                else
                {
                    switch (signal.DataType)
                    {
                        case DataType.Boolean:
                            {
                                break;
                            }
                        case DataType.Decimal:
                            {
                                var value = Convert.ToDecimal(dataOlder.ElementAt(0).Value);
                                Quality quality = ChoiseQuality(dataNewer.ElementAt(0).Quality, dataOlder.ElementAt(0).Quality);
                                var substractTime = CalculateSubstractTime(dataNewer.ElementAt(0).Timestamp, dataOlder.ElementAt(0).Timestamp, signal.Granularity);
                                var substractValue = Convert.ToDecimal(dataNewer.ElementAt(0).Value) - Convert.ToDecimal(dataOlder.ElementAt(0).Value);
                                if (Convert.ToInt16(substractTime) > CalculateSubstractTime(toExcludedUtc, fromIncludedUtc, signal.Granularity))
                                {
                                    int valueIteration = Convert.ToInt16(CalculateSubstractTime(toExcludedUtc, fromIncludedUtc, signal.Granularity));
                                    for (int i = 0; i < valueIteration; i++)
                                    {
                                        value = value + substractValue / Convert.ToDecimal(substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(decimal)),
                                            Timestamp = fromIncludedUtc
                                        });
                                        fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    }
                                    break;
                                }
                                else
                                {
                                    for (int i = 0; i < Convert.ToInt16(substractTime); i++)
                                    {
                                        value = value + substractValue / Convert.ToDecimal(substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(decimal)),
                                            Timestamp = fromIncludedUtc
                                        });
                                        fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    }
                                }
                                break;
                            }
                        case DataType.Double:
                            {
                                var value = Convert.ToDouble(dataOlder.ElementAt(0).Value);
                                Quality quality = ChoiseQuality(dataNewer.ElementAt(0).Quality, dataOlder.ElementAt(0).Quality);
                                var substractTime = CalculateSubstractTime(dataNewer.ElementAt(0).Timestamp, dataOlder.ElementAt(0).Timestamp, signal.Granularity);
                                var substractValue = Convert.ToDouble(dataNewer.ElementAt(0).Value) - Convert.ToDouble(dataOlder.ElementAt(0).Value);
                                if (Convert.ToInt16(substractTime) > CalculateSubstractTime(toExcludedUtc, fromIncludedUtc, signal.Granularity))
                                {
                                    int valueIteration = Convert.ToInt16(CalculateSubstractTime(toExcludedUtc, fromIncludedUtc, signal.Granularity));
                                    for (int i = 0; i < valueIteration; i++)
                                    {
                                        value = value + substractValue / (substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(double)),
                                            Timestamp = fromIncludedUtc
                                        });
                                        fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    }
                                    break;
                                }
                                else
                                {
                                    for (int i = 0; i < Convert.ToInt16(substractTime); i++)
                                    {
                                        value = value + substractValue / (substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(double)),
                                            Timestamp = fromIncludedUtc
                                        });
                                        fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    }
                                }
                                break;
                            }
                        case DataType.Integer:
                            {
                                var value = Convert.ToInt32(dataOlder.ElementAt(0).Value);
                                Quality quality = ChoiseQuality(dataNewer.ElementAt(0).Quality, dataOlder.ElementAt(0).Quality);
                                var substractTime = CalculateSubstractTime(dataNewer.ElementAt(0).Timestamp, dataOlder.ElementAt(0).Timestamp, signal.Granularity);
                                var substractValue = Convert.ToInt16(dataNewer.ElementAt(0).Value) - Convert.ToInt16(dataOlder.ElementAt(0).Value);
                                if (Convert.ToInt16(substractTime) > CalculateSubstractTime(toExcludedUtc, fromIncludedUtc, signal.Granularity))
                                {
                                    int valueIteration = Convert.ToInt16(CalculateSubstractTime(toExcludedUtc, fromIncludedUtc, signal.Granularity));
                                    for (int i = 0; i < valueIteration; i++)
                                    {
                                        value = value + Convert.ToInt32(substractValue) / Convert.ToInt32(substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(int)),
                                            Timestamp = fromIncludedUtc
                                        });
                                        fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    }
                                    break;
                                }
                                else
                                    for (int i = 0; i < Convert.ToInt16(substractTime); i++)
                                    {
                                        value = value + Convert.ToInt32(substractValue) / Convert.ToInt32(substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(int)),
                                            Timestamp = fromIncludedUtc
                                        });
                                        fromIncludedUtc = AddingTimespanToDataTime(fromIncludedUtc, signal.Granularity);
                                    }
                                break;
                            }
                        case DataType.String:
                            {
                                break;
                            }
                    }
                }
            }
            return datumsFirst;
        }

        
        

        

        private DateTime AddingTimespanToDataTime(DateTime addedData, Granularity granularitySignal)
        {
            var checkGranularity = new Dictionary<Granularity, Action>()
            {
                {Granularity.Day, ()=> addedData=addedData.AddDays(1) },
                {Granularity.Hour, ()=> addedData=addedData.AddHours(1) },
                {Granularity.Minute, ()=> addedData=addedData.AddMinutes(1) },
                {Granularity.Month, ()=> addedData=addedData.AddMonths(1) },
                {Granularity.Second, ()=> addedData=addedData.AddSeconds(1) },
                {Granularity.Week, ()=> addedData=addedData.AddDays(7) },
                {Granularity.Year, ()=> addedData=addedData.AddYears(1) }
            };
            checkGranularity[granularitySignal].Invoke();
            return addedData;
        }

        private double CalculateSubstractTime(DateTime dataNever, DateTime dataOlder, Granularity granularitySignal)
        {
            double returnValue = 0;
            var calulateSubstract = new Dictionary<Granularity, Action>()
            {
                {Granularity.Day, ()=> returnValue=(dataNever - dataOlder).TotalDays },
                {Granularity.Hour, ()=> returnValue=(dataNever - dataOlder).TotalHours },
                {Granularity.Minute, ()=> returnValue=(dataNever - dataOlder).TotalMinutes },
                {Granularity.Month, ()=> returnValue=((dataNever.Year - dataOlder.Year)*12)+dataNever.Month - dataOlder.Month },
                {Granularity.Second, ()=> returnValue=(dataNever - dataOlder).TotalSeconds },
                {Granularity.Week, ()=> returnValue=(dataNever - dataOlder).TotalDays/7 },
                {Granularity.Year, ()=> returnValue=(dataNever.Year - dataOlder.Year)}
            };
            calulateSubstract[granularitySignal].Invoke();

            return returnValue;
        }

        private void RemoveElementsFromDatumAndAddingNewElements<T>(List<Domain.Datum<T>> datums, List<Domain.Datum<T>> datumsFirst)
        {
            for (int i = 0; i < datums.Count; i++)
                datums.RemoveAt(i);
            for (int i = 0; i < datumsFirst.Count; i++)
                datums.Add(datumsFirst.ElementAt(i));
        }


        private Quality ChoiseQuality(Quality dataNeverQuality, Quality dataOlderQuality)
        {
            int dataNeverQualityValue = SetValueToQuality(dataNeverQuality);
            int dataOlderQualityValue = SetValueToQuality(dataOlderQuality);

            if (dataOlderQualityValue < dataNeverQualityValue)
                return dataOlderQuality;
            else
                return dataNeverQuality;
        }

        private int SetValueToQuality(Quality dataQuality)
        {
            int value = 0;
            switch (dataQuality)
            {
                case Quality.Bad:
                    {
                        value = 1;
                        break;
                    }
                case Quality.Fair:
                    {
                        value = 3;
                        break;
                    }
                case Quality.Good:
                    {
                        value = 4;
                        break;
                    }
                case Quality.None:
                    {
                        value = 0;
                        break;
                    }
                case Quality.Poor:
                    {
                        value = 2;
                        break;
                    }
            }
            return value;
        }



    }
}
