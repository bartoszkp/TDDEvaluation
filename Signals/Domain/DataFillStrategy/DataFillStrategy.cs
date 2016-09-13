using Domain.Exceptions;
using Domain.Infrastructure;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.DataFillStrategy
{
    [NHibernateIgnore]
    public abstract class DataFillStrategy
    {
        
        
        protected MissingValuePolicyBase missingValuePolicy;
        protected abstract void incrementData(ref DateTime date);

        public void FillMissingData<T>(Signal signal, List<Domain.Datum<T>> datums, DateTime after, DateTime before, ISignalsDataRepository signalsDataRepository)
        {
            var dict = new Dictionary<DateTime, Datum<T>>();
            var datum = new Datum<T>();
            List<Domain.Datum<T>> datumsFirst = new List<Datum<T>>();
            if (this.missingValuePolicy is MissingValuePolicy.NoneQualityMissingValuePolicy<T>)
                SetupNoneQualityMissingValuePolicy(datum, datums, dict, before, after);
            else if (this.missingValuePolicy is MissingValuePolicy.SpecificValueMissingValuePolicy<T>)
                SetupSpecificValueMissingValuePolicy(datum, datums, dict, before, after);
            else if (this.missingValuePolicy is MissingValuePolicy.ZeroOrderMissingValuePolicy<T>)
                SetupZeroOrderMissingValuePolicy(datums, dict, signal, signalsDataRepository, after, before, datum);
            else if (this.missingValuePolicy is MissingValuePolicy.ShadowMissingValuePolicy<T>)
                SetupShadowMissingValuePolicy(datums, dict, signalsDataRepository, after, before);
            else if (this.missingValuePolicy is MissingValuePolicy.FirstOrderMissingValuePolicy<T>)
                SetupFirstOrderMissingValuePolicy(datums, signal, signalsDataRepository, after, before, datumsFirst);
        }

        private void SetupShadowMissingValuePolicy<T>(List<Datum<T>> datums, Dictionary<DateTime, Datum<T>> dict, ISignalsDataRepository signalsDataRepository, DateTime after, DateTime before)
        {
            var shadowData = signalsDataRepository.GetData<T>(((ShadowMissingValuePolicy<T>)missingValuePolicy).ShadowSignal, after, before);
            foreach (var item in shadowData)
            {
                dict.Add(item.Timestamp, item);
            }
            CreateReturnsListDatum<T>(after, before, datums, null, dict);
        }



        public void SetupNoneQualityMissingValuePolicy<T>(Datum<T> datum, List<Datum<T>> datums, Dictionary<DateTime, Datum<T>> dict, DateTime before, DateTime after)
        {
            datum = new Datum<T>()
            {
                Quality = Quality.None,
                Value = default(T),
            };
            CreateReturnsListDatum(after, before, datums, datum, dict);
        }

        public void SetupSpecificValueMissingValuePolicy<T>(Datum<T> datum, List<Datum<T>> datums, Dictionary<DateTime, Datum<T>> dict, DateTime before, DateTime after)
        {
            var mvp = missingValuePolicy as MissingValuePolicy.SpecificValueMissingValuePolicy<T>;
            datum = new Datum<T>()
            {
                Quality = mvp.Quality,
                Value = mvp.Value,
            };
            CreateReturnsListDatum(after, before, datums, datum, dict);
        }

        public void SetupZeroOrderMissingValuePolicy<T>(List<Datum<T>> datums, Dictionary<DateTime, Datum<T>> dict, Signal signal, ISignalsDataRepository signalsDataRepository,DateTime after, DateTime before, Datum<T> datum )
        {
            var xx = signalsDataRepository.GetDataOlderThan<T>(signal, after, 1).ToList();
            if (datums.Find(d => d.Timestamp < after) == null)
            {
                
                Domain.Datum<T> aa = xx.SingleOrDefault();
                if (aa != null)
                    dict.Add(after, new Datum<T>() { Quality = aa.Quality, Value = aa.Value });
            }
            foreach (var item in datums)
            {
                dict.Add(item.Timestamp, item);
            }
            dict.Remove(before);
            CreateReturnsListDatum(after, before, datums, datum, dict);
        }

        public void SetupFirstOrderMissingValuePolicy<T>(List<Datum<T>> datums, Signal signal, ISignalsDataRepository signalsDataRepository, DateTime after,DateTime before, List<Domain.Datum<T>> datumsFirst)
        {
                if (after == before)
                {
                    Datum<T> datumFirst = new Datum<T>()
                    {
                        Quality = datums.ElementAt(0).Quality,
                        Value = datums.ElementAt(0).Value,
                        Timestamp = datums.ElementAt(0).Timestamp,
                    };

                    datums.Clear();
                    datums.Add(datumFirst);

                return;
                }


            while (after < before)
            {
                var dataOlder = signalsDataRepository.GetDataOlderThan<T>(signal, after, 1);
                var dataNever = signalsDataRepository.GetDataNewerThan<T>(signal, after, 1);
                if ((dataOlder == null) || (dataOlder.Count<Datum<T>>() == 0))
                {
                    if (datums.Find(x => x.Timestamp == after) == null)
                    {
                        datumsFirst.Add(new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = after
                        });
                    }
                    else
                    {
                        datumsFirst.Add(datums.Find(x => x.Timestamp == after));
                    }
                    after = AddingTimespanToDataTime(after, signal.Granularity);
                }
                else
                if ((dataNever == null) || (dataNever.Count<Datum<T>>() == 0))
                {
                    if (datums.Find(x => x.Timestamp == after) == null)
                    {
                        datumsFirst.Add(new Datum<T>()
                        {
                            Quality = Quality.None,
                            Value = default(T),
                            Timestamp = after
                        });
                    }
                    after = AddingTimespanToDataTime(after, signal.Granularity);
                }
                else
                { 
                    switch (signal.DataType)
                    {
                        case DataType.Boolean:
                            {
                                CallToExceptionWrongDataType();
                                break;
                            }
                        case DataType.Decimal:
                            {
                                var value = Convert.ToDecimal(dataOlder.ElementAt(0).Value);
                                Quality quality = ChoiseQuality(dataNever.ElementAt(0).Quality, dataOlder.ElementAt(0).Quality);
                                var substractTime = CalculateSubstractTime(dataNever.ElementAt(0).Timestamp, dataOlder.ElementAt(0).Timestamp, signal.Granularity);
                                var substractValue = Convert.ToDecimal(dataNever.ElementAt(0).Value) - Convert.ToDecimal(dataOlder.ElementAt(0).Value);
                                if (Convert.ToInt16(substractTime) > CalculateSubstractTime(before, after, signal.Granularity))
                                {
                                    int valueIteration = Convert.ToInt16(CalculateSubstractTime(before, after, signal.Granularity));
                                    for (int i = 0; i < valueIteration; i++)
                                    {
                                        value = value + substractValue / Convert.ToDecimal(substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(decimal)),
                                            Timestamp = after
                                        });
                                        after = AddingTimespanToDataTime(after, signal.Granularity);
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
                                            Timestamp = after
                                        });
                                        after = AddingTimespanToDataTime(after, signal.Granularity);
                                    }
                                }
                                break;
                            }
                        case DataType.Double:
                            {
                                var value = Convert.ToDouble(dataOlder.ElementAt(0).Value);
                                Quality quality = ChoiseQuality(dataNever.ElementAt(0).Quality, dataOlder.ElementAt(0).Quality);
                                var substractTime = CalculateSubstractTime(dataNever.ElementAt(0).Timestamp, dataOlder.ElementAt(0).Timestamp, signal.Granularity);
                                var substractValue = Convert.ToDouble(dataNever.ElementAt(0).Value) - Convert.ToDouble(dataOlder.ElementAt(0).Value);
                                if (Convert.ToInt16(substractTime) > CalculateSubstractTime(before, after, signal.Granularity))
                                {
                                    int valueIteration = Convert.ToInt16(CalculateSubstractTime(before, after, signal.Granularity));
                                    for (int i = 0; i < valueIteration; i++)
                                    {
                                        value = value + substractValue / (substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(double)),
                                            Timestamp = after
                                        });
                                        after = AddingTimespanToDataTime(after, signal.Granularity);
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
                                            Timestamp = after
                                        });
                                        after = AddingTimespanToDataTime(after, signal.Granularity);
                                    }
                                }
                                break;
                            }
                        case DataType.Integer:
                            {
                                var value = Convert.ToInt32(dataOlder.ElementAt(0).Value);
                                Quality quality = ChoiseQuality(dataNever.ElementAt(0).Quality,dataOlder.ElementAt(0).Quality);
                                var substractTime = CalculateSubstractTime(dataNever.ElementAt(0).Timestamp, dataOlder.ElementAt(0).Timestamp, signal.Granularity);
                                var substractValue = Convert.ToInt16(dataNever.ElementAt(0).Value) - Convert.ToInt16(dataOlder.ElementAt(0).Value);
                                if (Convert.ToInt16(substractTime) > CalculateSubstractTime(before, after, signal.Granularity))
                                {
                                    int valueIteration = Convert.ToInt16(CalculateSubstractTime(before, after, signal.Granularity));
                                    for (int i = 0; i < valueIteration; i++)
                                    {
                                        value = value + Convert.ToInt32(substractValue) / Convert.ToInt32(substractTime);
                                        datumsFirst.Add(new Datum<T>()
                                        {
                                            Quality = quality,
                                            Value = (T)(value).Adapt(value.GetType(), typeof(int)),
                                            Timestamp = after
                                        });
                                        after = AddingTimespanToDataTime(after, signal.Granularity);
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
                                        Timestamp = after
                                    });
                                    after = AddingTimespanToDataTime(after, signal.Granularity);
                                }
                                break;
                            }
                        case DataType.String:
                            {
                                CallToExceptionWrongDataType();
                                break;
                            }
                    }
                }
            }

            RemoveElementsFromDatumAndAddingNewElements(datums, datumsFirst);
        }

        public double CalculateSubstractTime(DateTime dataNever, DateTime dataOlder, Granularity granularitySignal)
        {
            double returnValue=0;
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

        public void RemoveElementsFromDatumAndAddingNewElements<T>(List<Domain.Datum<T>> datums, List<Domain.Datum<T>> datumsFirst)
        {
            datums.Clear();
            for (int i = 0; i < datumsFirst.Count; i++)
                datums.Add(datumsFirst.ElementAt(i));
        }
        
        public void CreateReturnsListDatum<T>(DateTime after, DateTime before, List<Datum<T>> datums, Datum<T> datum, Dictionary<DateTime, Datum<T>> dict)
        {
            DateTime currentDate = new DateTime(after.Ticks);
            DateTime lastDate = currentDate;
            while (currentDate <= before)
            {
                if (datums.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null && this.missingValuePolicy is MissingValuePolicy.ZeroOrderMissingValuePolicy<T>)
                {
                    if (dict.ContainsKey(lastDate))
                        datum = dict[lastDate];
                    datums.Add(new Datum<T>()
                    {
                        Quality = datum.Quality,
                        Value = datum.Value,
                        Timestamp = currentDate
                    });
                }
                else if (this.missingValuePolicy is MissingValuePolicy.ShadowMissingValuePolicy<T>)
                {
                    var date = datums.Find(x => x.Timestamp == currentDate);

                    if (date == null)
                    {
                        if (dict.ContainsKey(currentDate))
                            datum = dict[currentDate];
                        else
                        {
                            datum = new Datum<T>()
                            {
                                Quality = Quality.None,
                                Value = default(T),
                                Timestamp = currentDate
                            };
                        }

                        datums.Add(datum);
                    }
                }
                else if (datums.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                    datums.Add(new Datum<T>()
                    {
                        Quality = datum.Quality,
                        Value = datum.Value,
                        Timestamp = currentDate
                    });

                lastDate = currentDate;
                incrementData(ref currentDate);
            }
        }

        public DateTime AddingTimespanToDataTime(DateTime addedData, Granularity granularitySignal)
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

        public Quality ChoiseQuality(Quality dataNeverQuality, Quality dataOlderQuality)
        {
            int dataNeverQualityValue = SetValueToQuality(dataNeverQuality);
            int dataOlderQualityValue = SetValueToQuality(dataOlderQuality);
            
            if (dataOlderQualityValue < dataNeverQualityValue)
                return dataOlderQuality;
            else
                return dataNeverQuality;
        }

        public int SetValueToQuality(Quality dataQuality)
        {
            int value = 0 ;
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

        public void CallToExceptionWrongDataType()
        {
            throw new DataTypeWrongFormatException();
        }
    }

}
