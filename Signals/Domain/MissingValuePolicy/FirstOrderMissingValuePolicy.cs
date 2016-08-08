using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> GetMissingValue(Signal signal, DateTime timeStamp)
        {
            throw new NotImplementedException();
        }
    }
}
