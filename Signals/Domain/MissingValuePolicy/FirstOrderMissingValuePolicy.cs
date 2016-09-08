using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, 
            DateTime fromIncludedUtc, DateTime toExcludedUtc,
            Datum<T> olderDatum = null, Datum<T> newerDatum = null)
        {
            var currentDate = new DateTime(fromIncludedUtc.Ticks);
            var result = new List<Datum<T>>(data);

            while (currentDate < toExcludedUtc)
            {
                if (result.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    bool previousDatumExisted = true;

                    if (olderDatum == null)
                    {
                        olderDatum = new Datum<T>()
                        {
                            Timestamp = AddTime(currentDate, signal.Granularity, -1),
                            Value = default(T),
                            Quality = Quality.None
                        };
                        previousDatumExisted = false;
                    }


                    if (newerDatum == null || !previousDatumExisted)
                    {
                        var timestamp = AddTime(currentDate, signal.Granularity);

                        newerDatum = new Datum<T>()
                        {
                            Timestamp = timestamp,
                            Value = default(T),
                            Quality = Quality.None
                        };
                    }

                    var quality = DetermineQuality(olderDatum.Quality, newerDatum.Quality);
                    var difference = IntervalDifference(newerDatum.Timestamp, olderDatum.Timestamp, signal.Granularity);

                    if (difference > 2)
                    {
                        var tempDate = new DateTime(currentDate.Ticks);
                        var valueToAdd = CalculateValueToAdd(newerDatum.Value, olderDatum.Value, difference);
                        var value = AggregateValue(valueToAdd, olderDatum.Value);


                        while (tempDate < newerDatum.Timestamp && tempDate < toExcludedUtc)
                        {
                            var missingDatum = new Datum<T>()
                            {
                                Quality = quality,
                                Timestamp = new DateTime(tempDate.Ticks),
                                Value = value
                            };

                            result.Add(missingDatum);

                            value = AggregateValue(value, valueToAdd);
                            tempDate = AddTime(tempDate, signal.Granularity);
                        }
                    }
                    else
                    {
                        var missingDatum = new Datum<T>()
                        {
                            Timestamp = new DateTime(currentDate.Ticks),
                            Quality = quality,
                            Value = CalculateValueToAdd(newerDatum.Value, olderDatum.Value, 2)
                        };

                        if (newerDatum.Quality == Quality.None)
                        {
                            missingDatum.Value = default(T);
                            missingDatum.Quality = Quality.None;
                        }


                        result.Add(missingDatum);
                    }
                }

                currentDate = AddTime(currentDate, signal.Granularity);
            }

            return result;
        }

        protected static Quality DetermineQuality(Quality q1, Quality q2)
        {

            switch (q1)
            {
                case Quality.None:
                    return q1;

                case Quality.Good:
                    if (q2 != q1)
                        return q2;
                    return q1;

                case Quality.Fair:
                    if (q2 != q1 && q2 != Quality.Good)
                        return q2;
                    return q1;

                case Quality.Poor:
                    if (q2 != q1 && q2 != Quality.Good && q2 != Quality.Fair)
                        return q2;
                    return q1;

                case Quality.Bad:
                    if (q2 != q1 && q2 != Quality.Good && q2 != Quality.Fair && q2 != Quality.Poor)
                        return q2;
                    return q1;

                default:
                    return Quality.None;
            }

        }

        protected static int IntervalDifference(DateTime next, DateTime prev, Granularity granularity)
        {
            var difference = next - prev;
            switch (granularity)
            {
                case Granularity.Second:
                    return (int)difference.TotalSeconds;

                case Granularity.Minute:
                    return (int)difference.TotalMinutes;

                case Granularity.Hour:
                    return (int)difference.TotalHours;

                case Granularity.Day:
                    return (int)difference.TotalDays;

                case Granularity.Week:
                    return (int)difference.TotalDays / 7;

                case Granularity.Month:
                    return (next.Year - prev.Year) * 12 + next.Month - prev.Month;

                case Granularity.Year:
                    return next.Year - prev.Year;
            }
            throw new NotSupportedException("Granularity: " + granularity.ToString() + " is not supported");
        }

        protected static T CalculateValueToAdd<T>(T nextT, T prevT, int difference)
        {
            if (typeof(T) == typeof(double))
            {
                var next = (double)Convert.ChangeType(nextT, typeof(double));
                var prev = (double)Convert.ChangeType(prevT, typeof(double));
                var value = (next - prev) / difference;
                return (T)Convert.ChangeType(value, typeof(T));
            }

            if (typeof(T) == typeof(decimal))
            {
                var next = (decimal)Convert.ChangeType(nextT, typeof(decimal));
                var prev = (decimal)Convert.ChangeType(prevT, typeof(decimal));
                var value = (next - prev) / difference;
                return (T)Convert.ChangeType(value, typeof(T));
            }

            if (typeof(T) == typeof(int))
            {
                var next = (int)Convert.ChangeType(nextT, typeof(int));
                var prev = (int)Convert.ChangeType(prevT, typeof(int));
                var value = (next - prev) / difference;
                return (T)Convert.ChangeType(value, typeof(T));
            }

            throw new NotSupportedException("Type: " + typeof(T).ToString() + " is not supported");
        }

        protected static T AggregateValue<T>(T nextT, T prevT)
        {
            if (typeof(T) == typeof(double))
            {
                var next = (double)Convert.ChangeType(nextT, typeof(double));
                var prev = (double)Convert.ChangeType(prevT, typeof(double));
                var value = next + prev;
                return (T)Convert.ChangeType(value, typeof(T));
            }

            if (typeof(T) == typeof(decimal))
            {
                var next = (decimal)Convert.ChangeType(nextT, typeof(decimal));
                var prev = (decimal)Convert.ChangeType(prevT, typeof(decimal));
                var value = next + prev;
                return (T)Convert.ChangeType(value, typeof(T));
            }

            if (typeof(T) == typeof(int))
            {
                var next = (int)Convert.ChangeType(nextT, typeof(int));
                var prev = (int)Convert.ChangeType(prevT, typeof(int));
                var value = next + prev;
                return (T)Convert.ChangeType(value, typeof(T));
            }

            throw new NotSupportedException("Type: " + typeof(T).ToString() + " is not supported");
        }
    }
}
