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

        public override void FillDatums(ref List<Datum<T>> datumsList, DateTime time, DateTime toExcludedUtc, Signal signal,
            MissingValuePolicy<T> mvp = null, object additionalDatums = null)
        {
            var specifiedMvp = mvp as SpecificValueMissingValuePolicy<T>;
            datumsList.Add(new Datum<T>() { Quality = specifiedMvp.Quality, Timestamp = time, Value = specifiedMvp.Value });
        }
    }
}
