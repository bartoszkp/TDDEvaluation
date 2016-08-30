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
            var previousData = otherData?.Where(d => d.Timestamp < timeStamp);
            var nextData = otherData?.Where(d => d.Timestamp > timeStamp);

            List<Datum<T>> date;
            if (previousSamples != null && previousSamples.Count() > 0)
            {
                date = previousSamples.ToList();
                date.AddRange(previousData);
            }
            else
            {
                date = otherData.ToList();
            }

            if(nextSamples != null && nextSamples.Count() > 0)
            {
                date.AddRange(nextData);
            }

            if (date == null || date.Count() == 0)
                return new Datum<T>()
                {
                    Value = default(T),
                    Quality = Quality.None
                };


            var previousDatum = date.Aggregate((a, b) => a.Timestamp > b.Timestamp ? a : b);
            var nextDatum = date.Aggregate((a, b) => a.Timestamp < b.Timestamp ? a : b);

            dynamic previousValue= previousDatum.Value, nextValue=nextDatum.Value;

            return new Datum<T>()
            {
                Quality = previousDatum.Quality > nextDatum.Quality ? previousDatum.Quality : nextDatum.Quality,
                Value = (previousValue + nextValue)/ 2
            };
        }
    }
}
