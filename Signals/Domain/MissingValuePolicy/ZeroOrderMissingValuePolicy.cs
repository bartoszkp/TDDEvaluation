using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetDatum(DateTime timeStamp, IEnumerable<Datum<T>> otherData = null)
        {
            var previousData = otherData?.Where(d => d.Timestamp < timeStamp);
            if (previousData == null || previousData.Count() == 0)
                return new Datum<T>();

            var previousDatum = previousData.Aggregate((a,b) => a.Timestamp > b.Timestamp ? a : b);

            return new Datum<T>()
            {
                Quality = previousDatum.Quality,
                Value = previousDatum.Value
            };
        }
    }
}
