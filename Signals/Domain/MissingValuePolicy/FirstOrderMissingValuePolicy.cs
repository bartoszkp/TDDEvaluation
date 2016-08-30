using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetDatum(DateTime timeStamp, IEnumerable<Datum<T>> otherData = null,
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
                return new Datum<T>()
                {
                    Value = default(T),
                    Quality = Quality.None
                };

            dynamic previousValue= previousDatum.Value, nextValue=nextDatum.Value;

            return new Datum<T>()
            {
                Quality = previousDatum.Quality > nextDatum.Quality ? previousDatum.Quality : nextDatum.Quality,
                Value = (previousValue + nextValue)/ 2
            };
        }
    }
}
