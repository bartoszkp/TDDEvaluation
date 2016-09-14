using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using Domain.Services.Implementation;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, 
            DateTime fromIncludedUtc, DateTime toExcludedUtc,
            Datum<T> olderDatum = null, Datum<T> neverDatum = null, SignalsDomainService service = null)
        {
            T value = default(T);
            Quality quality = Quality.None;

            Datum<T> missingDatum;

            var currentDate = new DateTime(fromIncludedUtc.Ticks);
            List<Datum<T>> result = new List<Datum<T>>(data);
            while (currentDate < toExcludedUtc)
            {
                var currentItem = result.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0);
                if (currentItem == null)
                {
                    if (olderDatum != null)
                    {
                        missingDatum = new Datum<T>()
                        {
                            Value = olderDatum.Value,
                            Timestamp = currentDate,
                            Quality = olderDatum.Quality
                        };
                    }
                    else
                    {
                        missingDatum = new Datum<T>()
                        {
                            Value = value,
                            Timestamp = currentDate,
                            Quality = quality
                        };
                    }

                    result.Add(missingDatum);
                }
                else
                {
                    value = currentItem.Value;
                    quality = currentItem.Quality;
                }


                currentDate = AddTime(currentDate, signal.Granularity);
            }

            return result;
        }

    }
}
