using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetMissingDatum(IEnumerable<Datum<T>> data, DateTime dt)
        {
            return new Datum<T>()
            {
                Quality = Quality.None,
                Timestamp = dt,
                Value = default(T)
            };
        }
    }
}
