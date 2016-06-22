using Domain.Infrastructure; // TODO ugly dependency, change iface to DateTime
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public abstract class MissingValuePolicy
    {
        public virtual int? Id { get; set; }

        public virtual Signal Signal { get; set; }

        public virtual IEnumerable<Datum<T>> FillMissingData<T>(TimeEnumerator timeEnumerator, IEnumerable<Datum<T>> readData)
        {
            var abstractResult = FillMissingData(timeEnumerator, readData as IEnumerable<DatumBase>);

            return abstractResult.Cast<Datum<T>>();
        }

        protected abstract IEnumerable<DatumBase> FillMissingData(TimeEnumerator timeEnumerator, IEnumerable<DatumBase> readData);
        /*
        Interpolation*/
    }
}