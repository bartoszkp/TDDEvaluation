using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        private T value = default(T);
        private Quality quality = Quality.None;

        public virtual Quality Quality { get { return this.quality; } }
        public virtual T Value { get { return this.value; } }
    }
}
