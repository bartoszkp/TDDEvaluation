using System;
using System.Collections.Generic;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override Datum<T> GetMissingDatum(IEnumerable<Datum<T>> data, DateTime dt)
        {
            throw new NotImplementedException();
        }
    }
}
