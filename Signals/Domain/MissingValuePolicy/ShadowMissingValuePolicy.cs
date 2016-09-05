using System;
using System.Collections.Generic;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override IEnumerable<Datum<T>> GetDatum(DateTime timeStamp, Granularity granularity, IEnumerable<Datum<T>> otherData = null, IEnumerable<Datum<T>> previousSamples = null, IEnumerable<Datum<T>> nextSamples = null)
        {
            return new Domain.Datum<T>[] { };
        }

        public virtual void CheckSignalDataTypeAndGranularity(Signal signal)
        {
            if (signal.DataType != this.ShadowSignal.DataType || signal.Granularity != this.ShadowSignal.Granularity)
                throw new ArgumentException("Failed to assign ShadowMissingValuePolicy to the signal.");
        }
    }
}
