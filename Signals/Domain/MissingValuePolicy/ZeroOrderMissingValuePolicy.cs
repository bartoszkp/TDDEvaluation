using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> GetDatum(DateTime timeStamp, Granularity granularity, IEnumerable<Datum<T>> otherData = null,
            IEnumerable<Datum<T>> previousSamples = null, IEnumerable<Datum<T>> nextSamples = null)
        {
            var previousData = otherData?.Where(d => d.Timestamp < timeStamp);

            List<Datum<T>> date;
            if (previousSamples != null && previousSamples.Count() > 0)
            {
                date = previousSamples.ToList();
                date.InsertRange(date.Count(), previousData);
            }
            else
                date = otherData.ToList(); 

            if (date == null || date.Count() == 0)
                return new Datum<T>[] {
                    new Datum<T>()
                    {
                        Value = default(T),
                        Quality = Quality.None,
                        Timestamp = timeStamp
                    }
                };


            var previousDatum = date.Aggregate((a,b) => a.Timestamp > b.Timestamp ? a : b);

            return new Datum<T>[] {
                new Datum<T>()
                {
                    Quality = previousDatum.Quality,
                    Value = previousDatum.Value,
                    Timestamp = timeStamp
                }
            };
        }
    }
}
