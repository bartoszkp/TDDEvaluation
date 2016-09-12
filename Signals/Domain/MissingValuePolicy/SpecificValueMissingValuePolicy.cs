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

        public override Datum<T> FillMissingValue(Signal signal, DateTime tmp, IEnumerable<Datum<T>> data, Datum<T> previousDatum = null, Datum<T> nextDatum = null, IEnumerable<Datum<T>> shadowData = null)
        {
            var datum = data.FirstOrDefault(d => d.Timestamp == tmp);
            return (datum ?? Datum<T>.CreateSpecific(Signal, tmp, this.Quality, this.Value));
        }
    }
}
