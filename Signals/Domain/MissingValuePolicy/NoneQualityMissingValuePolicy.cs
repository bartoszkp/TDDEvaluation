using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetDatum(DateTime timeStamp, IEnumerable<Datum<T>> otherData = null, IEnumerable<Datum<T>> dataOutOfRange = null)
        {
            return new Datum<T>()
            {
                Quality = Quality.None,
                Value = default(T)
            };
        }
    }
}
