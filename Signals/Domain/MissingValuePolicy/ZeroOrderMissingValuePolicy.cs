using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetDatum(DateTime timeStamp, IEnumerable<Datum<T>> otherData = null, IEnumerable<Datum<T>> dataOutOfRange = null)
        {
            var previousData = otherData?.Where(d => d.Timestamp < timeStamp);

            List<Datum<T>> date;
            if (dataOutOfRange != null && dataOutOfRange.Count() > 0)
            {
                date = dataOutOfRange.ToList();
                date.InsertRange(dataOutOfRange.Count() / 2, previousData.ToList());
            }
            else
                date = otherData.ToList(); 

            if (date == null || date.Count() == 0)
                return new Datum<T>()
                {
                    Value = default(T),
                    Quality = Quality.None
                };


            var previousDatum = date.Aggregate((a,b) => a.Timestamp > b.Timestamp ? a : b);

            return new Datum<T>()
            {
                Quality = previousDatum.Quality,
                Value = previousDatum.Value
            };
        }
    }
}
