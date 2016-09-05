using System;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override Datum<T> GetMissingValue(Signal signal, DateTime timestamp, Datum<T> previous = null, Datum<T> next = null, 
            Datum<T> shadowDatum = null)
        {
            if (shadowDatum != null)
                return shadowDatum;
            else
                return Datum<T>.CreateNone(signal, timestamp);            
        }
    }
}
