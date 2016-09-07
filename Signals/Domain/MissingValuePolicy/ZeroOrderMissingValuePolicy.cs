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
            Datum<T> olderDatum = null, Datum<T> neverDatum = null)
        {
            var currentDate = new DateTime(fromIncludedUtc.Ticks);
            List<Datum<T>> result = new List<Datum<T>>(data);

            while (currentDate < toExcludedUtc)
            {
                if (result.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    Datum<T> missingDatum = null;

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
                            Value = default(T),
                            Timestamp = currentDate,
                            Quality = Quality.None
                        };
                    }

                    result.Add(missingDatum);
                }

                currentDate = AddTime(currentDate, signal.Granularity);
            }

            return result;
        }

    }
}
