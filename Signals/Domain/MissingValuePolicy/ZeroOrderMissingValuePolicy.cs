using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using Domain.Services.Implementation;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var timestampBegin = fromIncludedUtc;
            var timestampEnd = toExcludedUtc;
            var dateTimeComaparator = DateTime.Compare(timestampBegin, timestampEnd);

            if (dateTimeComaparator > 0)
            {
                for (int i = 0; i < data.Count(); i++)
                {
                    data.ToList().RemoveAt(i);
                }

                data.ToArray();

                return data;
            }

            dataDictionary = data.ToDictionary(d => d.Timestamp, d => d);

            var datum = DateTimeList(data, signal, fromIncludedUtc, toExcludedUtc);

            var resultEnumerable = datum
                .Select(d => dataDictionary.ContainsKey(d) ? dataDictionary[d] : Datum<T>.CreateNone(Signal, d));

            var resultList = resultEnumerable.ToList();
            

            int j = 0;
            foreach (var item in resultList)
            {
                if (item.Quality == Quality.None && item.Value.Equals(default(T)) && j != 0)
                {
                    resultList.ElementAt(j).Quality = resultList.ElementAt(j - 1).Quality;
                    resultList.ElementAt(j).Value = resultList.ElementAt(j - 1).Value;
                }

                j++;
            }

            return resultList;
        }

        private List<DateTime> DateTimeList(IEnumerable<Datum<T>> data, Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var key = signal.Granularity;

            dictionary = Dictionary();

            datetimeList = new List<DateTime>();

            int count = 0;

            TimeSpan timeSpan = toExcludedUtc.Subtract(fromIncludedUtc);

            switch (signal.Granularity)
            {
                case Granularity.Second:
                    count = (int)timeSpan.TotalSeconds;
                    CreateDateTimeList(data, fromIncludedUtc, key, count);
                    break;
                case Granularity.Minute:
                    count = (int)timeSpan.TotalMinutes;
                    CreateDateTimeList(data, fromIncludedUtc, key, count);
                    break;
                case Granularity.Hour:
                    count = (int)timeSpan.TotalHours;
                    CreateDateTimeList(data, fromIncludedUtc, key, count);
                    break;
                case Granularity.Day:
                    count = (int)timeSpan.TotalDays;
                    CreateDateTimeList(data, fromIncludedUtc, key, count);
                    break;
                case Granularity.Week:
                    count = (int)timeSpan.TotalDays / 7;
                    CreateDateTimeList(data, fromIncludedUtc, key, count);
                    break;
                case Granularity.Month:
                    count = (int)timeSpan.TotalDays / 30;
                    CreateDateTimeList(data, fromIncludedUtc, key, count);
                    break;
                case Granularity.Year:
                    count = (int)timeSpan.TotalDays / 365;
                    CreateDateTimeList(data, fromIncludedUtc, key, count);
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
