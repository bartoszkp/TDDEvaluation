using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetMissingDatum(IEnumerable<Datum<T>> data, DateTime dt)
        {
            var datum = data.Where(d => d.Timestamp < dt).LastOrDefault();

            if (datum != null)
            {
                datum.Timestamp = dt;
                return datum;
            }

            return new Datum<T>()
            {
                Quality = Quality.None,
                Timestamp = dt,
                Value = default(T)
            };
        }
    }
}
