using Domain.Repositories;
using System;
using System.Collections.Generic;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc, ISignalsDataRepository signalsDataRepository)
        {
            throw new NotImplementedException();
        }
    }
}
