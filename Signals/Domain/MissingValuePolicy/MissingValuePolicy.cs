using Domain.Infrastructure; // TODO ugly dependency, change iface to DateTime
using System.Collections.Generic;

namespace Domain.MissingValuePolicy
{
    public abstract class MissingValuePolicy
    {
        public virtual int? Id { get; set; }

        public virtual Signal Signal { get; set; }

        public abstract IEnumerable<Datum<T>> FillMissingData<T>(TimeEnumerator timeEnumerator, IEnumerable<Datum<T>> readData);
        /*
        Interpolation*/
    }
}