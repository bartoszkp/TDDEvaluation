using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override void FillDatums(ref List<Datum<T>> datumsList, DateTime time, DateTime toExcludedUtc, Signal signal,
            MissingValuePolicy<T> mvp = null, object additionalDatums = null)
        {
            List<Datum<T>> convertedAdditionalDatums = ((IEnumerable<Datum<T>>)additionalDatums).ToList();
            int index = convertedAdditionalDatums.FindIndex(x => x.Timestamp == time);
            if(index >= 0)
            {
                datumsList.Add(new Datum<T>() { Quality = convertedAdditionalDatums.ElementAt(index).Quality,
                    Timestamp = time,
                    Value = convertedAdditionalDatums.ElementAt(index).Value });
            }
        }
    }
}
