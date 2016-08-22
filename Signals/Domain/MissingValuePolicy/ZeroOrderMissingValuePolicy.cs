using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetDatum(DateTime timeStamp, IEnumerable<Datum<T>> otherData = null)
        {
            return new Datum<T>();
        }
    }
}
