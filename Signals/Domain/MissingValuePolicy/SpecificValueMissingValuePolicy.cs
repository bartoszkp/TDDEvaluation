using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }

        public override Quality GetQuality()
        {
            return this.Quality;
        }

        public override void SetQuality(Quality quality)
        {
            this.Quality = quality;
        }
    }
}
