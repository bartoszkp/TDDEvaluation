using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }

        public virtual IEnumerable<Domain.Datum<T>> SetMissingValue(Signal signal, IEnumerable<Datum<T>> datums, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (fromIncludedUtc > toExcludedUtc)
                return new List<Datum<T>>();
            else if (fromIncludedUtc == toExcludedUtc)
            {
                var datum = datums.FirstOrDefault(d => d.Timestamp == fromIncludedUtc);
                return new[] { datum ?? Datum(signal, fromIncludedUtc) };
            }
            else
            {
                List<Datum<T>> filledList = new List<Datum<T>>();

                Granularity granularity = signal.Granularity;

                DateTime tmp = fromIncludedUtc;

                while (tmp < toExcludedUtc)
                {
                    AddToTheListSuitableDatum(filledList, tmp, datums, signal);

                    tmp = NextDate(tmp, granularity);
                }

                return filledList;
            }
        }

        private void AddToTheListSuitableDatum(List<Datum<T>> filledList, DateTime tmp, IEnumerable<Domain.Datum<T>> datums, Signal signal)
        {
            Datum<T> newDatum = datums.FirstOrDefault(datum => datum.Timestamp == tmp);

            filledList.Add(newDatum ?? Datum(signal, tmp));
        }

        private Datum<T> Datum(Signal signal, DateTime timestamp)
        {
            return new Datum<T>()
            {
                Quality = Quality,
                Timestamp = timestamp,
                Value = Value,
                Signal = signal
            };
        }

        private DateTime NextDate(DateTime date, Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return date.AddSeconds(1);
                case Granularity.Minute:
                    return date.AddMinutes(1);
                case Granularity.Hour:
                    return date.AddHours(1);
                case Granularity.Day:
                    return date.AddDays(1);
                case Granularity.Week:
                    return date.AddDays(7);
                case Granularity.Month:
                    return date.AddMonths(1);
                case Granularity.Year:
                    return date.AddYears(1);
                default: return date;
            }
        }
    }
}
