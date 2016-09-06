using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> GetDatum(DateTime timeStamp, Granularity granularity, IEnumerable<Datum<T>> otherData = null,
            IEnumerable<Datum<T>> previousSamples = null, IEnumerable<Datum<T>> nextSamples = null)
        {
            if (previousSamples.Count() == 1)
            {
                return new Datum<T>[]
                {
                    new Datum<T>()
                    {
                        Quality = previousSamples.First().Quality,
                        Timestamp = timeStamp,
                        Value = previousSamples.First().Value
                    }
                };
            }
            return new Datum<T>[]
            {
                new Datum<T>()
                {
                    Quality = Quality.None,
                    Timestamp = timeStamp,
                    Value = default(T)
                }
            };
        }
    }
}
