using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> FillMissingValue(Signal signal, DateTime tmp, IEnumerable<Datum<T>> data, Datum<T> previousDatum = null, Datum<T> nextDatum = null, IEnumerable<Datum<T>> shadowData = null)
        {
            var datum = data.FirstOrDefault(d => d.Timestamp == tmp);
            if (datum != null)
                return datum;
            else
            {
                if (previousDatum == null)
                    return Datum<T>.CreateNone(signal, tmp);
                else
                    return (new Datum<T>()
                    {
                        Quality = previousDatum.Quality,
                        Value = previousDatum.Value,
                        Timestamp = tmp
                    });
            }
        }
    }
}
