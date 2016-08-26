using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services.DataFillHelpers
{
    public static class ZeroOrderFillHelper
    {
        public static void FillMissingData<T>(Granularity granularity, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            T actuallValue = default(T);
            var time = toExcluded - fromIncluded;
            var quality = Domain.Quality.None;
            
            switch(granularity)
            {
                case Granularity.Day:
                    {
                        for(int i=0; i < time.TotalDays; i++)
                        {
                            if (data[i].Timestamp != fromIncluded.AddDays(i))
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = quality,
                                    Value = actuallValue,
                                    Timestamp = fromIncluded.AddDays(i),
                                };
                                data.Insert(i, newItem);

                            }
                            else
                            {
                                actuallValue = data[i].Value;
                                quality = data[i].Quality;
                             }
                        }

                        break;
                    }
                case Granularity.Hour:
                    {
                        for (int i = 0; i < time.TotalHours; i++)
                        {
                            if (data[i].Timestamp != fromIncluded.AddHours(i))
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = quality,
                                    Value = actuallValue,
                                    Timestamp = fromIncluded.AddHours(i),
                                };
                                data.Insert(i, newItem);

                            }
                            else
                            {
                                actuallValue = data[i].Value;
                                quality = data[i].Quality;
                             }
                        }
                        break;
                    }
                case Granularity.Minute:
                    {
                        for (int i = 0; i < time.TotalMinutes; i++)
                        {
                            if (data[i].Timestamp != fromIncluded.AddMinutes(i))
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = quality,
                                    Value = actuallValue,
                                    Timestamp = fromIncluded.AddMinutes(i),
                                };
                                data.Insert(i, newItem);

                            }
                            else
                            {
                                actuallValue = data[i].Value;
                                quality = data[i].Quality;
                            }
                        }
                        break;
                    }
                case Granularity.Month:
                    {
                        int months = toExcluded.Month - fromIncluded.Month + (12 * (toExcluded.Year - fromIncluded.Year));
                        for (int i = 0; i < months ; i++)
                        {
                            if (data[i].Timestamp != fromIncluded.AddMonths(i))
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = quality,
                                    Value = actuallValue,
                                    Timestamp = fromIncluded.AddMonths(i),
                                };
                                data.Insert(i, newItem);

                            }
                            else
                            {
                                actuallValue = data[i].Value;
                                quality = data[i].Quality;
                            }
                        }
                        break;
                    }
                case Granularity.Second:
                    {
                        for (int i = 0; i < time.TotalSeconds; i++)
                        {
                            if (data[i].Timestamp != fromIncluded.AddSeconds(i))
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = quality,
                                    Value = actuallValue,
                                    Timestamp = fromIncluded.AddSeconds(i),
                                };
                                data.Insert(i, newItem);

                            }
                            else
                            {
                                actuallValue = data[i].Value;
                                quality = data[i].Quality;
                            }
                        }
                        break;
                    }
                case Granularity.Week:
                    {
                        for (int i = 0; i < (time.TotalDays / 7); i++)
                        {
                            if (data[i].Timestamp != fromIncluded.AddDays(7 * i))
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = quality,
                                    Value = actuallValue,
                                    Timestamp = fromIncluded.AddDays(7 * i),
                                };
                                data.Insert(i, newItem);

                            }
                            else
                            {
                                actuallValue = data[i].Value;
                                quality = data[i].Quality;
                            }
                        }
                        break;
                    }
                case Granularity.Year:
                    {
                        var year = toExcluded.Year - fromIncluded.Year;
                        for (int i = 0; i < year; i++)
                        {
                            if (data[i].Timestamp != fromIncluded.AddYears(i))
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = quality,
                                    Value = actuallValue,
                                    Timestamp = fromIncluded.AddYears(i),
                                };
                                data.Insert(i, newItem);

                            }
                            else
                            {
                                actuallValue = data[i].Value;
                                quality = data[i].Quality;
                            }
                        }
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
    }
}
