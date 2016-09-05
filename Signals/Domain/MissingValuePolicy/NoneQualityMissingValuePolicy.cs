using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using System;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override void FillDatums(ref List<Datum<T>> datumsList, DateTime time, DateTime toExcludedUtc, Signal signal,
            MissingValuePolicy<T> mvp = null, object additionalDatums = null)
        {
            datumsList.Add(new Datum<T>() { Quality = Quality.None, Timestamp = time, Value = default(T) });
        }
    }
}
