using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {

        private Quality Quality = Quality.None;
        private T Value = default(T);

        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc, Datum<T> olderData)
        {
            if (olderData != null)
            {
                Quality = olderData.Quality;
                Value = olderData.Value;
            }

            returnListDatum = new List<Datum<T>>();
            while (fromIncludedUtc < toExcludedUtc)
            {
                var elementOfList = data.FirstOrDefault(x => x.Timestamp == fromIncludedUtc);
                if (elementOfList != null)
                {
                    this.Quality = elementOfList.Quality;
                    this.Value = elementOfList.Value;
                }
                returnListDatum.Add(new Datum<T>() { Signal = signal, Quality = this.Quality, Timestamp = fromIncludedUtc, Value = this.Value });
                fromIncludedUtc = AddToDateTime(fromIncludedUtc, signal.Granularity);
            }
            return returnListDatum;
        }

        private List<Datum<T>> returnListDatum;

    }
}
