using System;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override Datum<T> FillMissingValue(Datum<T> datum)
        {
            throw new NotImplementedException();
        }
    }
}
