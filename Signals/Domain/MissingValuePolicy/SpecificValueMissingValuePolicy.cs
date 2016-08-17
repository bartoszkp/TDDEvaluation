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
            if (fromIncludedUtc > toExcludedUtc) return new List<Datum<T>>();

            else if (fromIncludedUtc == toExcludedUtc)
            {
                List<Datum<T>> datumWithTimestampEqualFromAndToUtc = new List<Datum<T>>();

                foreach (var datum in datums)
                {
                    if (datum.Timestamp == fromIncludedUtc) datumWithTimestampEqualFromAndToUtc.Add(datum);
                }

                return datumWithTimestampEqualFromAndToUtc;
            }

            else
            {
                List<Datum<T>> filledList = new List<Datum<T>>();

                Granularity granularity = signal.Granularity;

                DateTime tmp = fromIncludedUtc;

                switch (granularity)
                {
                    case Granularity.Second:

                        while (tmp < toExcludedUtc)
                        {
                            AddToTheListSuitableDatum(filledList, tmp, datums, this.Quality, this.Value);

                            tmp = tmp.AddSeconds(1);
                        }

                        break;

                    case Granularity.Minute:

                        while (tmp < toExcludedUtc)
                        {
                            AddToTheListSuitableDatum(filledList, tmp, datums, this.Quality, this.Value);

                            tmp = tmp.AddMinutes(1);
                        }

                        break;

                    case Granularity.Hour:

                        while (tmp < toExcludedUtc)
                        {
                            AddToTheListSuitableDatum(filledList, tmp, datums, this.Quality, this.Value);

                            tmp = tmp.AddHours(1);
                        }

                        break;

                    case Granularity.Day:

                        while (tmp < toExcludedUtc)
                        {
                            AddToTheListSuitableDatum(filledList, tmp, datums, this.Quality, this.Value);

                            tmp = tmp.AddDays(1);
                        }

                        break;

                    case Granularity.Week:

                        while (tmp < toExcludedUtc)
                        {
                            AddToTheListSuitableDatum(filledList, tmp, datums, this.Quality, this.Value);

                            tmp = tmp.AddDays(7);
                        }

                        break;

                    case Granularity.Month:

                        while (tmp < toExcludedUtc)
                        {
                            AddToTheListSuitableDatum(filledList, tmp, datums, this.Quality, this.Value);

                            tmp = tmp.AddMonths(1);
                        }

                        break;

                    case Granularity.Year:

                        while (tmp < toExcludedUtc)
                        {
                            AddToTheListSuitableDatum(filledList, tmp, datums, this.Quality, this.Value);

                            tmp = tmp.AddYears(1);
                        }

                        break;

                    default: break;

                }

                return filledList;
            }
        }

        private void AddToTheListSuitableDatum(List<Datum<T>> filledList, DateTime tmp, IEnumerable<Domain.Datum<T>> datums, Quality quality, T value)
        {
            Datum<T> newDatum = new Datum<T>()
            {
                Quality = quality,
                Timestamp = tmp,
                Value = value
            };

            foreach (var datum in datums)
            {
                if (newDatum.Timestamp == datum.Timestamp) newDatum = datum;
            }

            filledList.Add(newDatum);
        }
    }
}
