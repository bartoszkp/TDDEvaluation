using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using Domain.Services.Implementation;

namespace Domain.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }

        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, 
            DateTime fromIncludedUtc, DateTime toExcludedUtc,
            Datum<T> olderDatum = null, Datum<T> neverDatum = null, SignalsDomainService service = null)
        {
            var currentDate = new DateTime(fromIncludedUtc.Ticks);
            List<Datum<T>> result = new List<Datum<T>>(data);

            while (currentDate < toExcludedUtc)
            {
                if (result.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var missingDatum = new Datum<T>()
                    {
                        Value = this.Value,
                        Timestamp = currentDate,
                        Quality = this.Quality
                    };

                    result.Add(missingDatum);
                }

                currentDate = AddTime(currentDate, signal.Granularity);
            }

            return result;
        }

    }
}
