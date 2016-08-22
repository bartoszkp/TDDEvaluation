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

        public override Datum<T> GetDatum(DateTime timeStamp, IEnumerable<Datum<T>> otherData = null)
        {
            return new Datum<T>()
            {
                Quality = Quality,
                Value = Value
            };
        }
    }
}
