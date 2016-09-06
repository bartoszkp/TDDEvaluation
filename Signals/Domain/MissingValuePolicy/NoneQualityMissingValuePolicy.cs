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
            CreateList(data, signal, fromIncludedUtc, toExcludedUtc);
            return dateTimeList
                    .Select(d => dataDictionary.ContainsKey(d) ? dataDictionary[d] : Datum<T>.CreateNone(Signal, d));
        }

        private void CreateList(IEnumerable<Datum<T>> data, Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            dateTimeList = new List<DateTime>();
            granularityToDateTime = Dictionary();
            var timestamp = fromIncludedUtc;
            var key = signal.Granularity;
            var switchGranularity = new Dictionary<Granularity, Action>()
            {
                {Granularity.Second, ()=> CreateDateTimeList(data, timestamp, key, (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalSeconds) },
                {Granularity.Minute, ()=> CreateDateTimeList(data, timestamp, key, (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalMinutes)},
                {Granularity.Hour, ()=> CreateDateTimeList(data, timestamp, key, (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalHours)},
                {Granularity.Day, ()=> CreateDateTimeList(data, timestamp, key, (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays)},
                {Granularity.Week, ()=> CreateDateTimeList(data, timestamp, key, (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays / 7)},
                {Granularity.Month, ()=> CreateDateTimeList(data, timestamp, key, (toExcludedUtc.Month - fromIncludedUtc.Month) + 12 * (toExcludedUtc.Year - fromIncludedUtc.Year))},
                {Granularity.Year, ()=> CreateDateTimeList(data, timestamp, key, (int)(toExcludedUtc.Year - fromIncludedUtc.Year))}
            };
            switchGranularity[key].Invoke();
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
                    dateTimeList.Add(timestamp);
                timestamp = granularityToDateTime[key](timestamp);
            }
        }

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



        private Dictionary<DateTime, Datum<T>> dataDictionary;
        private Dictionary<Granularity, Func<DateTime, DateTime>> granularityToDateTime;
        private List<DateTime> dateTimeList;


    }
}
