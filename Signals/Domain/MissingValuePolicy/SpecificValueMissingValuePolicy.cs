using Domain.Infrastructure;
using System;
using System.Collections.Generic;

namespace Domain.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy<T> : MissingValuePolicy<T>
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

        public override IEnumerable<Datum<T>> FillMissingData(TimeEnumerator timeEnumerator, IEnumerable<Datum<T>> readData)
        {
            throw new NotImplementedException();
        }
    }
}
