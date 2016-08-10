using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data)
        {
            var dataDict = data.ToDictionary(d => d.Timestamp, d => d);
            
            var datum = DateTimeList(dataDict, data, signal);
            
            return datum
                .Select(d => dataDict.ContainsKey(d) ? dataDict[d] : Datum<T>.CreateNone(Signal, d));
        }

        private List<DateTime> DateTimeList(Dictionary<DateTime, Datum<T>> dataDict, IEnumerable<Datum<T>> data, Signal signal)
        {
            var key = signal.Granularity;

            var dictionary = Dictionary();

            DateTime timestamp = data.First().Timestamp;

            List<DateTime> datetimeList = new List<DateTime>();

            int count = 0;
            int index = 0;

            switch (signal.Granularity)
            {
                case Granularity.Second:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalSeconds + 1;
                    for (int i = 0; i < count; i++)
                    {
                        if (dataDict.ContainsKey(timestamp))
                        {
                            datetimeList.Add(data.ElementAt(index).Timestamp);
                            index++;
                        }
                        else
                        {
                            datetimeList.Add(dictionary[key](timestamp));
                        }
                        timestamp = timestamp.AddSeconds(1);
                    }
                    break;
                case Granularity.Minute:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalMinutes + 1;
                    for (int i = 0; i < count; i++)
                    {
                        if (dataDict.ContainsKey(timestamp))
                        {
                            datetimeList.Add(data.ElementAt(index).Timestamp);
                            index++;
                        }
                        else
                        {
                            datetimeList.Add(dictionary[key](timestamp));
                        }
                        timestamp = timestamp.AddMinutes(1);
                    }
                    break;
                case Granularity.Hour:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalHours + 1;
                    for (int i = 0; i < count; i++)
                    {
                        if (dataDict.ContainsKey(timestamp))
                        {
                            datetimeList.Add(data.ElementAt(index).Timestamp);
                            index++;
                        }
                        else
                        {
                            datetimeList.Add(dictionary[key](timestamp));
                        }
                        timestamp = timestamp.AddHours(1);
                    }
                    break;
                case Granularity.Day:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays + 1;
                    for (int i = 0; i < count; i++)
                    {
                        if (dataDict.ContainsKey(timestamp))
                        {
                            datetimeList.Add(data.ElementAt(index).Timestamp);
                            index++;
                        }
                        else
                        {
                            datetimeList.Add(dictionary[key](timestamp));
                        }
                        timestamp = timestamp.AddDays(1);
                    }
                    break;
                case Granularity.Week:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays / 7 + 1;
                    for (int i = 0; i < count; i++)
                    {
                        if (dataDict.ContainsKey(timestamp))
                        {
                            datetimeList.Add(data.ElementAt(index).Timestamp);
                            index++;
                        }
                        else
                        {
                            datetimeList.Add(dictionary[key](timestamp));
                        }
                        timestamp = timestamp.AddDays(7);
                    }
                    break;
                case Granularity.Month:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays / 30 + 1;
                    for (int i = 0; i < count; i++)
                    {
                        if (dataDict.ContainsKey(timestamp))
                        {
                            datetimeList.Add(data.ElementAt(index).Timestamp);
                            index++;
                        }
                        else
                        {
                            datetimeList.Add(dictionary[key](timestamp));
                        }
                        timestamp = timestamp.AddMonths(1);
                    }
                    break;
                case Granularity.Year:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays / 365 + 1;
                    for (int i = 0; i < count; i++)
                    {
                        if (dataDict.ContainsKey(timestamp))
                        {
                            datetimeList.Add(data.ElementAt(index).Timestamp);
                            index++;
                        }
                        else
                        {
                            datetimeList.Add(dictionary[key](timestamp));
                        }
                        timestamp = timestamp.AddYears(1);
                    }
                    break;
            }
            
            return datetimeList;
        }

        private Dictionary<Granularity, Func<DateTime, DateTime>> Dictionary()
        {
            return new Dictionary<Granularity, Func<DateTime, DateTime>>
            {
                {Granularity.Second, time => time},
                {Granularity.Minute, time => time},
                {Granularity.Hour, time => time},
                {Granularity.Day, time => time },
                {Granularity.Week, time => time},
                {Granularity.Month, time => time },
                {Granularity.Year, time => time}
            };
        }
    }
}
