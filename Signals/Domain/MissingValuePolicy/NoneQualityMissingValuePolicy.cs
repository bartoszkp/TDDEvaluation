using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillData<T>(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var dataDictionary = data.ToDictionary(d => d.Timestamp, d => d);

            var granularityToDateTime = new Dictionary<Granularity, Func<DateTime, DateTime>>()
            {
                {Granularity.Month, time => time.AddMonths(1) }
            };

            List<DateTime> list = new List<DateTime>();

            var timestamp = data.First().Timestamp;

            if (signal.Granularity == Granularity.Month)
            {
                int count = (int)toExcludedUtc.Subtract(fromIncludedUtc).TotalDays / 30 + 1;
                int index = 0;
                for (int i = 0; i < count; i++)
                {
                    if (dataDictionary.ContainsKey(timestamp))
                    {
                        list.Add(data.ElementAt(index).Timestamp);
                        index++;
                    }
                    else
                    {
                        list.Add(timestamp);
                    }
                    timestamp = granularityToDateTime[Granularity.Month](timestamp);
                }
            }
                return list
                        .Select(d => dataDictionary.ContainsKey(d) ? dataDictionary[d] : Datum<T>.CreateNone(Signal, d));
        }
    }
}
