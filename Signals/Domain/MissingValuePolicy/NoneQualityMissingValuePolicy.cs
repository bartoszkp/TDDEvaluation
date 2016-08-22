﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            dataDictionary = data.ToDictionary(d => d.Timestamp, d => d);

            List(data, signal, fromIncludedUtc, toExcludedUtc);

                return dateTimeList
                        .Select(d => dataDictionary.ContainsKey(d) ? dataDictionary[d] : Datum<T>.CreateNone(Signal, d));
        }

        private void List(IEnumerable<Datum<T>> data, Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var key = signal.Granularity;
            
            dateTimeList = new List<DateTime>();

            granularityToDateTime = Dictionary();

            int count = 0;

            var timestamp = fromIncludedUtc;

            switch (key)
            {
                case Granularity.Second:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalSeconds;
                    CreateDateTimeList(data, timestamp, key, count);
                    break;
                case Granularity.Minute:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalMinutes;
                    CreateDateTimeList(data, timestamp, key, count);
                    break;
                case Granularity.Hour:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalHours;
                    CreateDateTimeList(data, timestamp, key, count);
                    break;
                case Granularity.Day:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays;
                    CreateDateTimeList(data, timestamp, key, count);
                    break;
                case Granularity.Week:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays / 7;
                    CreateDateTimeList(data, timestamp, key, count);
                    break;
                case Granularity.Month:
                    count = (toExcludedUtc.Month - fromIncludedUtc.Month) + 12 * (toExcludedUtc.Year - fromIncludedUtc.Year);
                    CreateDateTimeList(data, timestamp, key, count);
                    break;
                case Granularity.Year:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays / 365;
                    CreateDateTimeList(data, timestamp, key, count);
                    break;
            }
        }

        private void CreateDateTimeList(IEnumerable<Datum<T>> data, DateTime timestamp, Granularity key, int count)
        {
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                if (dataDictionary.ContainsKey(timestamp))
                {
                    dateTimeList.Add(data.ElementAt(index).Timestamp);
                    index++;
                }
                else
                {
                    dateTimeList.Add(timestamp);
                }
                timestamp = granularityToDateTime[key](timestamp);
            }
        }

        private Dictionary<DateTime, Datum<T>> dataDictionary;
        private Dictionary<Granularity, Func<DateTime, DateTime>> granularityToDateTime;
        private List<DateTime> dateTimeList;

        private Dictionary<Granularity, Func<DateTime, DateTime>> Dictionary()
        {
            return new Dictionary<Granularity, Func<DateTime, DateTime>>
            {
                {Granularity.Second, time => time.AddSeconds(1) },
                {Granularity.Minute, time => time.AddMinutes(1) },
                {Granularity.Hour, time => time.AddHours(1) },
                {Granularity.Day, time => time.AddDays(1) },
                {Granularity.Week, time => time.AddDays(7) },
                {Granularity.Month, time => time.AddMonths(1) },
                {Granularity.Year, time => time.AddYears(1) }
            };
        }
    }
}
