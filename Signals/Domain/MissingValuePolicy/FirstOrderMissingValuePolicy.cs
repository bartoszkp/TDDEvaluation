using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetDatum(DateTime timeStamp, IEnumerable<Datum<T>> otherData = null, IEnumerable<Datum<T>> dataOutOfRange = null)
        {
            throw new NotImplementedException();
        }
    }
}
