using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override Datum<T> FillMissingValue(Signal signal, DateTime tmp, IEnumerable<Datum<T>> data, 
            Datum<T> previousDatum = null, Datum<T> nextDatum = null, IEnumerable<Datum<T>> shadowData = null)
        {
            var signalDatum = data.FirstOrDefault(d => d.Timestamp == tmp);
            if (signalDatum != null)
                return signalDatum;
            else
            {
                var shadowDatum = shadowData.FirstOrDefault(d => d.Timestamp == tmp);
                if (shadowDatum != null)
                    return shadowDatum;
                else
                    return Datum<T>.CreateNone(signal, tmp);
            }            
        }
    }
}
