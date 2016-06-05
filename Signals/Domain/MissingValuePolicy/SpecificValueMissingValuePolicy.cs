using Domain.Infrastructure;
using System;
using System.Collections.Generic;

namespace Domain.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy<T> : MissingValuePolicy
    {
        public virtual T Value { get; private set; }

        public virtual Quality Quality { get; private set; }

        public SpecificValueMissingValuePolicy()
        {
            this.Value = default(T);
            this.Quality = Quality.None;
        }

        public SpecificValueMissingValuePolicy(T value, Quality quality)
        {
            this.Value = value;
            this.Quality = quality;
        }

        public override IEnumerable<Datum<S>> FillMissingData<S>(TimeEnumerator timeEnumerator, IEnumerable<Datum<S>> readData)
        {
            throw new NotImplementedException();
        }
    }
}
