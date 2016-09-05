using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetMissingValue(Signal signal, DateTime timestamp, Datum<T> previous = null, Datum<T> next = null, 
            Datum<T> shadowDatum = null)
        {
            if (previous == null)
                return Datum<T>.CreateNone(signal, timestamp);

            return new Datum<T>
            {
                Quality = previous.Quality,
                Value = previous.Value,
                Signal = signal,
                Timestamp = timestamp
            };
        }
    }
}