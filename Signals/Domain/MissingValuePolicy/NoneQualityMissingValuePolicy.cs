using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> FillMissingValue(Signal signal, DateTime tmp, IEnumerable<Datum<T>> data, Datum<T> previousDatum = null, Datum<T> nextDatum = null, IEnumerable<Datum<T>> shadowData = null)
        {
            return data.FirstOrDefault(d => d.Timestamp == tmp) ?? Datum<T>.CreateNone(signal, tmp);
        }
    }
}
