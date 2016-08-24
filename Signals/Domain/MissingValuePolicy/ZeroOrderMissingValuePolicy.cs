using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual IEnumerable<Datum<T>> SetMissingValue(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            if (fromIncludedUtc > toExcludedUtc) return new List<Datum<T>>();

            else if (fromIncludedUtc == toExcludedUtc)
            {
                List<Datum<T>> filledList = new List<Datum<T>>();

                AddToListSuitableDatum(filledList, data, fromIncludedUtc);

                return filledList;
            }

            else
            {
                List<Datum<T>> filledList = new List<Datum<T>>();

                DateTime tmp = fromIncludedUtc;

                Granularity granularity = signal.Granularity;

                switch (granularity)
                {
                    case Granularity.Second:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(filledList, data, tmp);

                            tmp = tmp.AddSeconds(1);
                        }

                        break;

                    case Granularity.Minute:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(filledList, data, tmp);

                            tmp = tmp.AddMinutes(1);
                        }

                        break;

                    case Granularity.Hour:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(filledList, data, tmp);

                            tmp = tmp.AddHours(1);
                        }

                        break;

                    case Granularity.Day:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(filledList, data, tmp);

                            tmp = tmp.AddDays(1);
                        }

                        break;

                    case Granularity.Week:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(filledList, data, tmp);

                            tmp = tmp.AddDays(7);
                        }

                        break;

                    case Granularity.Month:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(filledList, data, tmp);

                            tmp = tmp.AddMonths(1);
                        }

                        break;

                    case Granularity.Year:

                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(filledList, data, tmp);

                            tmp = tmp.AddYears(1);
                        }

                        break;

                    default: break;

                }

                return filledList;
            }
        }


        private void AddToListSuitableDatum(List<Datum<T>> filledList,  IEnumerable<Datum<T>> data, DateTime tmp )
        {
            Datum<T> previousDatum = null;

            Datum<T> newDatum = null;

            if (filledList.Count != 0) previousDatum = filledList.Last();

            if (previousDatum == null)
            {
                newDatum = new Datum<T>()
                {
                    Timestamp = tmp,
                    Quality = Quality.None,
                    Value = default(T)
                };
            }

            else
            {
                newDatum = new Datum<T>()
                {
                    Timestamp = tmp,
                    Quality = previousDatum.Quality,
                    Value = previousDatum.Value,
                };
            }

            foreach (var datum in data)
            {
                if (newDatum.Timestamp == datum.Timestamp)
                {
                    newDatum.Value = datum.Value;
                    newDatum.Signal = datum.Signal;
                    newDatum.Quality = datum.Quality;
                    break;
                }
            }

            filledList.Add(newDatum);
        }
    }
}