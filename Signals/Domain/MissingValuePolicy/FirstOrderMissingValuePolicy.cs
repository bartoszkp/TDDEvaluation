using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc, Datum<T> olderDatum, Datum<T> newestDatum)
        {
            var filledData = new List<Datum<T>>();
            foreach (var d in data)
            {
                for (; fromIncludedUtc < toExcludedUtc; fromIncludedUtc = AddToDateTime(fromIncludedUtc, signal.Granularity))
                {
                    if (d.Timestamp == fromIncludedUtc)
                    {
                        filledData.Add(d);
                        olderDatum = d;
                        fromIncludedUtc = AddToDateTime(fromIncludedUtc, signal.Granularity);
                        break;
                    }
                    filledData.Add(fillDatum(signal, fromIncludedUtc, olderDatum, d));
                }
            }
            for (; fromIncludedUtc < toExcludedUtc; fromIncludedUtc = AddToDateTime(fromIncludedUtc, signal.Granularity))
                filledData.Add(fillDatum(signal, fromIncludedUtc, olderDatum, newestDatum));
            return filledData;
        }

        private Datum<T> fillDatum (Signal signal, DateTime fromIncludedUtc, Datum<T> olderDatum, Datum<T> newestDatum)
        {
            var diff = difference(olderDatum.Timestamp, newestDatum.Timestamp, signal.Granularity);
            var pos = difference(olderDatum.Timestamp, fromIncludedUtc, signal.Granularity);
            return new Datum<T>()
            {
                Signal = signal,
                Timestamp = fromIncludedUtc,
                Quality = olderDatum.Quality,
                Value = calculateValue(olderDatum.Value, newestDatum.Value, diff, pos)
            };
        }

        private T calculateValue<T>(T olderValue, T newestValue, int diff, int pos)
        {
            if (typeof(T) == typeof(int))
            {
                var olderValueInt = ((int)Convert.ChangeType(olderValue, typeof(int)));
                var newestValueInt = ((int)Convert.ChangeType(newestValue, typeof(int)));
                var result = olderValueInt + (newestValueInt - olderValueInt) * pos / diff;
                return ((T)Convert.ChangeType(result, typeof(T)));
            }
            if (typeof(T) == typeof(double))
            {
                var olderValueInt = ((double)Convert.ChangeType(olderValue, typeof(double)));
                var newestValueInt = ((double)Convert.ChangeType(newestValue, typeof(double)));
                var result = olderValueInt + (newestValueInt - olderValueInt) * pos / diff;
                return ((T)Convert.ChangeType(result, typeof(T)));
            }
            if (typeof(T) == typeof(decimal))
            {
                var olderValueInt = ((decimal)Convert.ChangeType(olderValue, typeof(decimal)));
                var newestValueInt = ((decimal)Convert.ChangeType(newestValue, typeof(decimal)));
                var result = olderValueInt + (newestValueInt - olderValueInt) * pos / diff;
                return ((T)Convert.ChangeType(result, typeof(T)));
            }

            throw new NotSupportedException("Type: " + typeof(T).ToString() + " is not supported");
        }

        private int difference(DateTime d1, DateTime d2, Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Day:
                    return (int)(d2 - d1).TotalDays;

                case Granularity.Hour:
                    return (int)(d2 - d1).TotalHours;

                case Granularity.Minute:
                    return (int)(d2 - d1).TotalMinutes;

                case Granularity.Month:
                    return (d2.Year - d1.Year) * 12 + (d2.Month - d1.Month);

                case Granularity.Second:
                    return (int)(d2 - d1).TotalSeconds;

                case Granularity.Week:
                    return (int)(d2 - d1).TotalDays/7;

                case Granularity.Year:
                    return d2.Year - d1.Year;
            }

            throw new NotSupportedException("Granularity: " + granularity.ToString() + " is not supported");

        }
    }
}

