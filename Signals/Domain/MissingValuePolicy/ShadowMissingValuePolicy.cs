using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override IEnumerable<Datum<T>> GetDatum(DateTime timeStamp, Granularity granularity, IEnumerable<Datum<T>> otherData = null, IEnumerable<Datum<T>> previousSamples = null, IEnumerable<Datum<T>> nextSamples = null)
        {
            var shadowSignalData = nextSamples.ToList();
            var datumToInsert = shadowSignalData.Where(d => d.Timestamp == timeStamp).FirstOrDefault();

            if (datumToInsert != null)
            {
                return new Datum<T>[]
                {
                    new Datum<T>()
                    {
                        Quality = datumToInsert.Quality,
                        Timestamp = timeStamp,
                        Value = datumToInsert.Value
                    }
                };
            }

            return new Datum<T>[] { };
        }

        public virtual void CheckSignalDataTypeAndGranularity(Signal signal)
        {
            if (signal.DataType != this.ShadowSignal.DataType || signal.Granularity != this.ShadowSignal.Granularity)
                throw new ArgumentException("Failed to assign ShadowMissingValuePolicy to the signal.");
        }
    }
}
