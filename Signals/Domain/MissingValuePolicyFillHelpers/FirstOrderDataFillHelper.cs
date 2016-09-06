using Domain.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Infrastructure.NHibernateIgnore]
    class FirstOrderDataFillHelper : MissingValuePolicyFillHelper
    {

        public static List<Datum<T>> FillMissingData<T>(Signal signal, SignalsDomainService service,
            List<Datum<T>> data, DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<T>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<T>(signal, currentDate, 1);
                    Datum<T> previousDatum = null;
                    Datum<T> nextDatum = null;
                    bool previousDatumExisted = true;

                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<T>()
                        {
                            Timestamp = addTime(currentDate, signal.Granularity, -1),
                            Value = default(T),
                            Quality = Quality.None
                        };
                        previousDatumExisted = false;
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0 || !previousDatumExisted)
                    {
                        var timestamp = addTime(currentDate, signal.Granularity);

                        nextDatum = new Datum<T>()
                        {
                            Timestamp = timestamp,
                            Value = default(T),
                            Quality = Quality.None
                        };
                        if (nextDatum.Timestamp < toExcluded)
                            data.Add(nextDatum);
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var quality = DetermineQuality(previousDatum.Quality, nextDatum.Quality);
                    var difference = IntervalDifference(nextDatum.Timestamp, previousDatum.Timestamp, signal.Granularity);

                    if (difference > 2)
                    {
                        var tempDate = new DateTime(currentDate.Ticks);
                        var valueToAdd = CalculateValueToAdd(nextDatum.Value, previousDatum.Value, difference);
                        var value = AggregateValue(valueToAdd, previousDatum.Value);


                        while (tempDate < nextDatum.Timestamp && tempDate < toExcluded)
                        {
                            var missingDatum = new Datum<T>()
                            {
                                Quality = quality,
                                Timestamp = new DateTime(tempDate.Ticks),
                                Value = value
                            };

                            data.Add(missingDatum);

                            value = AggregateValue(value, valueToAdd);
                            tempDate = addTime(tempDate, signal.Granularity);
                        }
                    }
                    else
                    {
                        var missingDatum = new Datum<T>()
                        {
                            Timestamp = new DateTime(currentDate.Ticks),
                            Quality = quality,
                            Value = CalculateValueToAdd(nextDatum.Value, previousDatum.Value, 2)
                        };

                        if (nextDatum.Quality == Quality.None)
                        {
                            missingDatum.Value = default(T);
                            missingDatum.Quality = Quality.None;
                        }


                        data.Add(missingDatum);
                    }
                }

                currentDate = addTime(currentDate, signal.Granularity);
            }

            return data;
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
