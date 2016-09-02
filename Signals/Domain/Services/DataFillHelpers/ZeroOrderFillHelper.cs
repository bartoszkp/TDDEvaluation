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
            int dateTimeCompare = 0;
            int dateTimeCompareWithToExclude = 0;
            
            switch(granularity)
            {
                case Granularity.Day:
                    {
                        for(int i=0; i < time.TotalDays; i++)
                        {
                            dateTimeCompare = DateTime.Compare(data[i].Timestamp, fromIncluded);      

                            if (data[i].Timestamp != fromIncluded.AddDays(i) && dateTimeCompare < 0)
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = data[i].Quality,
                                    Value = data[i].Value,
                                    Timestamp = fromIncluded.AddDays(i),
                                };
                                data.Insert(i, newItem);
                            }
                            if (data[i].Timestamp != fromIncluded.AddDays(i) && dateTimeCompare > 0)
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
                            dateTimeCompare = DateTime.Compare(data[i].Timestamp, fromIncluded);

                            if (data[i].Timestamp != fromIncluded.AddHours(i) && dateTimeCompare < 0)
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = data[i].Quality,
                                    Value = data[i].Value,
                                    Timestamp = fromIncluded.AddHours(i),
                                };
                                data.Insert(i, newItem);

                            }
                            if (data[i].Timestamp != fromIncluded.AddHours(i) && dateTimeCompare > 0)
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
                            dateTimeCompare = DateTime.Compare(data[i].Timestamp, fromIncluded);

                            if (data[i].Timestamp != fromIncluded.AddMinutes(i) && dateTimeCompare < 0)
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = data[i].Quality,
                                    Value = data[i].Value,
                                    Timestamp = fromIncluded.AddMinutes(i),
                                };
                                data.Insert(i, newItem);

                            }
                            if (data[i].Timestamp != fromIncluded.AddMinutes(i) && dateTimeCompare > 0)
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
                            dateTimeCompare = DateTime.Compare(data[i].Timestamp, fromIncluded);

                            if (data[i].Timestamp != fromIncluded.AddMonths(i) && dateTimeCompare < 0)
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = data[i].Quality,
                                    Value = data[i].Value,
                                    Timestamp = fromIncluded.AddMonths(i),
                                };
                                data.Insert(i, newItem);

                            }
                            else if (data[i].Timestamp != fromIncluded.AddMonths(i) && dateTimeCompare > 0)
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
                            dateTimeCompare = DateTime.Compare(data[i].Timestamp, fromIncluded);

                            if (data[i].Timestamp != fromIncluded.AddSeconds(i) && dateTimeCompare < 0)
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = data[i].Quality,
                                    Value = data[i].Value,
                                    Timestamp = fromIncluded.AddSeconds(i),
                                };
                                data.Insert(i, newItem);

                            }
                            else if (data[i].Timestamp != fromIncluded.AddSeconds(i) && dateTimeCompare > 0)
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
                            dateTimeCompare = DateTime.Compare(data[i].Timestamp, fromIncluded);

                            if (data[i].Timestamp != fromIncluded.AddDays(7 * i) && dateTimeCompare < 0)
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = data[i].Quality,
                                    Value = data[i].Value,
                                    Timestamp = fromIncluded.AddDays(7 * i),
                                };
                                data.Insert(i, newItem);

                            }
                            else if (data[i].Timestamp != fromIncluded.AddDays(7 * i) && dateTimeCompare > 0)
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
                            dateTimeCompare = DateTime.Compare(data[i].Timestamp, fromIncluded);

                            if (data[i].Timestamp != fromIncluded.AddYears(i) && dateTimeCompare < 0)
                            {
                                Datum<T> newItem = new Datum<T>()
                                {
                                    Quality = data[i].Quality,
                                    Value = data[i].Value,
                                    Timestamp = fromIncluded.AddYears(i),
                                };
                                data.Insert(i, newItem);

                            }
                            else if (data[i].Timestamp != fromIncluded.AddYears(i) && dateTimeCompare > 0)
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
