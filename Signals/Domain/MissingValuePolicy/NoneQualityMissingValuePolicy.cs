using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> GetDatum(DateTime timeStamp, Granularity granularity, IEnumerable<Datum<T>> otherData = null,
            IEnumerable<Datum<T>> previousSamples = null, IEnumerable<Datum<T>> nextSamples = null)
        {
            return new Datum<T>[] {
                new Datum<T>()
                {
                    Quality = Quality.None,
                    Value = default(T),
                    Timestamp = timeStamp
                }
            };
        }
    }
}
