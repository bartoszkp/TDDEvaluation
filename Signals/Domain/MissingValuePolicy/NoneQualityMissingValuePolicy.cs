using System.Collections.Generic;
using System.Linq;
using System;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> SetMissingValue(Signal signal, IEnumerable<Datum<T>> datums, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (fromIncludedUtc > toExcludedUtc)
                return new List<Datum<T>>();
            else if (fromIncludedUtc == toExcludedUtc)
            {
                var datum = datums.FirstOrDefault(d => d.Timestamp == fromIncludedUtc);
                return new[] { datum ?? Datum<T>.CreateNone(signal, fromIncludedUtc) };
            }
            else
            {
                List<Datum<T>> filledList = new List<Datum<T>>();

                Granularity granularity = signal.Granularity;

                DateTime tmp = fromIncludedUtc;

                while (tmp < toExcludedUtc)
                {
                    AddToTheListSuitableDatum(filledList, tmp, datums, signal);

                    tmp = DateHelper.NextDate(tmp, granularity);
                }

                return filledList;
            }
        }

        private void AddToTheListSuitableDatum(List<Datum<T>> filledList, DateTime tmp, IEnumerable<Domain.Datum<T>> datums, Signal signal)
        {
            Datum<T> newDatum = datums.FirstOrDefault(datum => datum.Timestamp == tmp);

            filledList.Add(newDatum ?? Datum<T>.CreateNone(signal, tmp));
        }
    }
}