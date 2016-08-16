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

        public override IEnumerable<Datum<T>> FillData<T>(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            return null;
        }
    }
}
