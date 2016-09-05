using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetMissingValue(Signal signal, DateTime timestamp, Datum<T> previous = null, Datum<T> next = null, 
            Datum<T> shadowDatum = null)
        {
            return Datum<T>.CreateNone(signal, timestamp);
        }
    }
}
