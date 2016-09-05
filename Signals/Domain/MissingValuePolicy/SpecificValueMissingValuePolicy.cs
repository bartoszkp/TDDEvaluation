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

        public override Datum<T> GetMissingValue(Signal signal, DateTime timestamp, Datum<T> previous = null, Datum<T> next = null, 
            Datum<T> shadowDatum = null)
        {
            return new Datum<T>
            {
                Quality = Quality,
                Value = Value,
                Signal = signal,
                Timestamp = timestamp
            };
        }
    }
}
