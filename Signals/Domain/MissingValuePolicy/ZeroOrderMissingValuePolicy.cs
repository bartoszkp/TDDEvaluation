using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override void FillDatums(ref List<Datum<T>> datumsList, DateTime time, DateTime toExcludedUtc, Signal signal,
            MissingValuePolicy<T> mvp = null, object additionalDatums = null)
        {
            Datum<T> dataToAdd = (Datum<T>)additionalDatums;
            if (dataToAdd != null)
                datumsList.Add(new Datum<T>() { Quality = dataToAdd.Quality, Timestamp = time, Value = dataToAdd.Value });
            else
                datumsList.Add(new Datum<T>() { Quality = Quality.None, Timestamp = time, Value = default(T) });
        }
    }
}
