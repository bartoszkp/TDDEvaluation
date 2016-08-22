using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }

        public override Datum<T> GetMissingDatum(IEnumerable<Datum<T>> data, DateTime dt)
        {
            return new Datum<T>()
            {
                Quality = Quality,
                Value = Value,
                Timestamp = dt
            };
        }
    }
}
