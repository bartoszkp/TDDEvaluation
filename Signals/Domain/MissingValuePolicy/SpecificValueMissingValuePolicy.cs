﻿using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }

        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            dataDictionary = data.ToDictionary(d => d.Timestamp, d => d);

            List(data, signal, fromIncludedUtc, toExcludedUtc);
            
            return dateTimeList
                .Select(d => dataDictionary.ContainsKey(d) ? dataDictionary[d] : new Datum<T>()
                {
                    Id = 0,
                    Signal = this.Signal,
                    Timestamp = d,
                    Value = this.Value,
                    Quality = this.Quality
                });
        }

        private void List(IEnumerable<Datum<T>> data, Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {

            var key = signal.Granularity;

            granularityToDateTime = Dictionary();

            dateTimeList = new List<DateTime>();

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
                case Granularity.Month:
                    count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays / 30;
                    CreateDateTimeList(data, timestamp, key, count);
                    break;

            }
            //if (key == Granularity.Second)
            //{
            //    int count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalSeconds;
            //    int index = 0;
            //    for (int i = 0; i < count; i++)
            //    {
            //        if (dataDictionary.ContainsKey(timestamp))
            //        {
            //            dateTimeList.Add(data.ElementAt(index).Timestamp);
            //            index++;
            //        }
            //        else
            //        {
            //            dateTimeList.Add(timestamp);
            //        }
            //        timestamp = granularityToDateTime[key](timestamp);
            //    }
            //}
            //else if (key == Granularity.Month)
            //{
            //    int count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays / 30;
            //    int index = 0;
            //    for (int i = 0; i < count; i++)
            //    {
            //        if (dataDictionary.ContainsKey(timestamp))
            //        {
            //            dateTimeList.Add(data.ElementAt(index).Timestamp);
            //            index++;
            //        }
            //        else
            //        {
            //            dateTimeList.Add(timestamp);
            //        }
            //        timestamp = granularityToDateTime[key](timestamp);
            //    }
            //}
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
                {Granularity.Month, time => time.AddMonths(1) }
            };
        }
    }
}
