using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using Domain.Repositories;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc, ISignalsDataRepository signalsDataRepository)
        {
            throw new NotImplementedException();
        }
    }
}
