using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> GetDatum(DateTime timeStamp, Granularity granularity, IEnumerable<Datum<T>> otherData = null,
            IEnumerable<Datum<T>> previousSamples = null, IEnumerable<Datum<T>> nextSamples = null)
        {
            var previousData = otherData?.Where(d => d.Timestamp < timeStamp).ToList();
            var nextData = otherData?.Where(d => d.Timestamp > timeStamp).ToList();

            if (previousSamples != null && previousSamples.Count() > 0)
                previousData.InsertRange(0,previousSamples);

            if(nextSamples != null && nextSamples.Count() > 0)
                nextData.InsertRange(nextData.Count, nextSamples);

            Domain.Datum<T> previousDatum = null; 
            Domain.Datum<T> nextDatum = null;
            if (previousData != null && previousData.Count() > 0) 
                previousDatum = previousData.Aggregate((a, b) => a.Timestamp > b.Timestamp ? a : b);
            if (nextData != null && nextData.Count() > 0)
                nextDatum = nextData.Aggregate((a, b) => a.Timestamp < b.Timestamp ? a : b);

            if (previousDatum == null || nextDatum == null)
                return new Datum<T>[] {
                    new Datum<T>()
                    {
                        Value = default(T),
                        Quality = Quality.None,
                        Timestamp = timeStamp
                    }
                };

            dynamic previousValue= previousDatum.Value, nextValue=nextDatum.Value;

            List<Datum<T>> datums = new List<Datum<T>>();
            for(DateTime currentTimestamp = AddTime(granularity, previousDatum.Timestamp); currentTimestamp < nextDatum.Timestamp; 
                currentTimestamp=AddTime(granularity,currentTimestamp))
            {
                datums.Add(new Datum<T>()
                {
                    Quality = previousDatum.Quality > nextDatum.Quality ? previousDatum.Quality : nextDatum.Quality,
                    Timestamp = currentTimestamp
                });
            }

            T difference =  (nextValue - previousValue) / (datums.Count + 1);
            int times=1;
            foreach (var datum in datums)
            {
                datum.Value = previousValue + (dynamic)difference * times;
                times++;
            }

            return datums;
        }

        private DateTime AddTime(Granularity granuality, DateTime date)
        {
            if (granuality == Granularity.Second) return date.AddSeconds(1);
            if (granuality == Granularity.Minute) return date.AddMinutes(1);
            if (granuality == Granularity.Hour) return date.AddHours(1);
            if (granuality == Granularity.Day) return date.AddDays(1);
            if (granuality == Granularity.Week) return date.AddDays(7);
            if (granuality == Granularity.Month) return date.AddMonths(1);
            if (granuality == Granularity.Year) return date.AddYears(1);
            return date;
        }
    }
}
