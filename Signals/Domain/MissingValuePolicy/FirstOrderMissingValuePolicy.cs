using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetMissingDatum(IEnumerable<Datum<T>> data, DateTime dt)
        {
            var datum = data.Where(d => d.Timestamp == dt).LastOrDefault();
            var quality = Quality.None;
            var value = default(T);

            if (datum != null)
            {
                quality = datum.Quality;
                value = datum.Value;
            }

            return new Datum<T>()
            {
                Quality = quality,
                Timestamp = dt,
                Value = value
            };
        }

    }
}
