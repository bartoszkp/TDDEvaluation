using Domain.Infrastructure;
using System;
using System.Collections.Generic;

namespace Domain.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }

        public override IEnumerable<Datum<T>> FillMissingData(TimeEnumerator timeEnumerator, IEnumerable<Datum<T>> readData)
        {
            throw new NotImplementedException();
        }
    }
}
