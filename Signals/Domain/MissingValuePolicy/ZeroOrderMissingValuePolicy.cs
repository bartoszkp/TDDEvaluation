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

        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc, Datum<T> olderDatum, Datum<T> newestDatum)
        {
            returnListDatum = new List<Datum<T>>();

            if (olderDatum != null)
            {
                Quality = olderDatum.Quality;
                Value = olderDatum.Value;
            }

            if (fromIncludedUtc == toExcludedUtc && data.Count() == 0)
            {
                if (olderDatum != null)
                {
                    returnListDatum.Add(new Datum<T>()
                    {
                        Quality = olderDatum.Quality,
                        Value = olderDatum.Value,
                        Timestamp = new DateTime(fromIncludedUtc.Ticks)
                    });
                }
                else
                {
                    returnListDatum.Add(new Datum<T>()
                    {
                        Quality = Quality.None,
                        Value = default(T),
                        Timestamp = new DateTime(fromIncludedUtc.Ticks)
                    });
                }
                return returnListDatum;

            }


            
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
