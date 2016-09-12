using System;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override Datum<T> GetDatumToFill(DateTime timestamp)
        {
            throw new NotImplementedException();
        }
    }
}
