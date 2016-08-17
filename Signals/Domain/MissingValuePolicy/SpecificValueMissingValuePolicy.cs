using System;
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
            var dataDictionary = data.ToDictionary(d => d.Timestamp, d => d);

            var key = signal.Granularity;

            var granularitytoDateTime = Dictionary();

            var dateTimeList = new List<DateTime>();

            var timestamp = fromIncludedUtc;

            if(key == Granularity.Month)
            {
                int count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays / 30;
                int index = 0;
                for(int i = 0; i < count; i++)
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
                    timestamp = granularitytoDateTime[key](timestamp);
                }
            }
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

        private Dictionary<Granularity, Func<DateTime, DateTime>> Dictionary()
        {
            return new Dictionary<Granularity, Func<DateTime, DateTime>>
            {
                {Granularity.Month, time => time.AddMonths(1) }
            };
        }
    }
}
