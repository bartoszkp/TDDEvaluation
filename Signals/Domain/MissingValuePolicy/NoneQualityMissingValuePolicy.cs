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
            dataDictionary = data.ToDictionary(d => d.Timestamp, d => d);
            
            var datum = DateTimeList(data, signal);
            
            return datum
                .Select(d => dataDictionary.ContainsKey(d) ? dataDictionary[d] : Datum<T>.CreateNone(Signal, d));
        }

        private List<DateTime> DateTimeList(IEnumerable<Datum<T>> data, Signal signal)
        {
            var key = signal.Granularity;

            dictionary = Dictionary();

            DateTime firstTimestamp = data.First().Timestamp;

            datetimeList = new List<DateTime>();

            int count = 0;

            switch (signal.Granularity)
            {
                case Granularity.Second:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalSeconds + 1;
                    CreateDateTimeList(data, firstTimestamp, key, count);
                    break;
                case Granularity.Minute:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalMinutes + 1;
                    CreateDateTimeList(data, firstTimestamp, key, count);
                    break;
                case Granularity.Hour:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalHours + 1;
                    CreateDateTimeList(data, firstTimestamp, key, count);
                    break;
                case Granularity.Day:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays + 1;
                    CreateDateTimeList(data, firstTimestamp, key, count);
                    break;
                case Granularity.Week:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays / 7 + 1;
                    CreateDateTimeList(data, firstTimestamp, key, count);
                    break;
                case Granularity.Month:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays / 30 + 1;
                    CreateDateTimeList(data, firstTimestamp, key, count);
                    break;
                case Granularity.Year:
                    count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays / 365 + 1;
                    CreateDateTimeList(data, firstTimestamp, key, count);
                    break;
            }
            
            return datetimeList;
        }

        private void CreateDateTimeList(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key, int count)
        {
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                if (dataDictionary.ContainsKey(firstTimestamp))
                {
                    datetimeList.Add(data.ElementAt(index).Timestamp);
                    index++;
                }
                else
                {
                    datetimeList.Add(firstTimestamp);
                }
                firstTimestamp = dictionary[key](firstTimestamp);
            }
        }

        //private void CreateDateTimeListForMinuteGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        //{
            
        //    int index = 0;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (dataDictionary.ContainsKey(firstTimestamp))
        //        {
        //            datetimeList.Add(data.ElementAt(index).Timestamp);
        //            index++;
        //        }
        //        else
        //        {
        //            datetimeList.Add(firstTimestamp);
        //        }
        //        firstTimestamp = dictionary[key](firstTimestamp);
        //    }
        //}


        //private void CreateDataTimeListForHourGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        //{
        //    int count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalHours + 1;
        //    int index = 0;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (dataDictionary.ContainsKey(firstTimestamp))
        //        {
        //            datetimeList.Add(data.ElementAt(index).Timestamp);
        //            index++;
        //        }
        //        else
        //        {
        //            datetimeList.Add(firstTimestamp);
        //        }
        //        firstTimestamp = dictionary[key](firstTimestamp);
        //    }
        //}

        //private void CreateDateTimeListForDayGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        //{
        //    int count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays + 1;
        //    int index = 0;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (dataDictionary.ContainsKey(firstTimestamp))
        //        {
        //            datetimeList.Add(data.ElementAt(index).Timestamp);
        //            index++;
        //        }
        //        else
        //        {
        //            datetimeList.Add(firstTimestamp);
        //        }
        //        firstTimestamp = dictionary[key](firstTimestamp);
        //    }
        //}

        //private void CreateDataTimeListForWeekGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        //{
        //    int count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays / 7 + 1;
        //    int index = 0;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (dataDictionary.ContainsKey(firstTimestamp))
        //        {
        //            datetimeList.Add(data.ElementAt(index).Timestamp);
        //            index++;
        //        }
        //        else
        //        {
        //            datetimeList.Add(firstTimestamp);
        //        }
        //        firstTimestamp = dictionary[key](firstTimestamp);
        //    }
        //}

        //private void CreateDateTimeListForMonthGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        //{
        //    int count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays / 30 + 1;
        //    int index = 0;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (dataDictionary.ContainsKey(firstTimestamp))
        //        {
        //            datetimeList.Add(data.ElementAt(index).Timestamp);
        //            index++;
        //        }
        //        else
        //        {
        //            datetimeList.Add(dictionary[key](firstTimestamp));
        //        }
        //        firstTimestamp = dictionary[key](firstTimestamp);
        //    }
        //}

        //private void CreateDateTimeListForYearGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        //{
        //    int count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays / 365 + 1;
        //    int index = 0;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (dataDictionary.ContainsKey(firstTimestamp))
        //        {
        //            datetimeList.Add(data.ElementAt(index).Timestamp);
        //            index++;
        //        }
        //        else
        //        {
        //            datetimeList.Add(firstTimestamp);
        //        }
        //        firstTimestamp = dictionary[key](firstTimestamp);
        //    }
        //}


        private List<DateTime> datetimeList;
        private Dictionary<Granularity, Func<DateTime, DateTime>> dictionary;
        private Dictionary<DateTime, Datum<T>> dataDictionary;

        private Dictionary<Granularity, Func<DateTime, DateTime>> Dictionary()
        {
            return new Dictionary<Granularity, Func<DateTime, DateTime>>
            {
                {Granularity.Second, time => time.AddSeconds(1)},
                {Granularity.Minute, time => time.AddMinutes(1)},
                {Granularity.Hour, time => time.AddHours(1)},
                {Granularity.Day, time => time.AddDays(1)},
                {Granularity.Week, time => time.AddDays(7)},
                {Granularity.Month, time => time.AddMonths(1)},
                {Granularity.Year, time => time.AddYears(1)}
            };
        }
    }
}
