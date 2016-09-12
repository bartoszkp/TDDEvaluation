using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override Datum<T> FillMissingValue(Signal signal, DateTime tmp, IEnumerable<Datum<T>> data, Datum<T> previousDatum = null, Datum<T> nextDatum = null, IEnumerable<Datum<T>> shadowData = null)
        {
            var datum = data.FirstOrDefault(d => d.Timestamp == tmp);
            if (datum != null)
                return datum;
            else
            {
                if (previousDatum == null || nextDatum == null)
                    return Datum<T>.CreateNone(signal, tmp);
                else
                {
                    Granularity granularity = signal.Granularity;

                    T newValue;
                    int numberOfTimePeriodsBetweenPreviousAndNext = FindNumberOfPeriodsBetweenTwoDates(previousDatum.Timestamp, nextDatum.Timestamp, granularity);
                    int numberOfTimePeriodsBetweenPreviousAndTmp = FindNumberOfPeriodsBetweenTwoDates(previousDatum.Timestamp, tmp, granularity);
                    double valuesDifferenceBetweenPreviousAndNext = Convert.ToDouble(nextDatum.Value) - Convert.ToDouble(previousDatum.Value);
                    double growthForOnePeriod = valuesDifferenceBetweenPreviousAndNext / numberOfTimePeriodsBetweenPreviousAndNext;
                    newValue = (T)Convert.ChangeType(Convert.ToDouble(previousDatum.Value) + growthForOnePeriod * numberOfTimePeriodsBetweenPreviousAndTmp, typeof(T));

                    Quality newQuality;
                    int comparitionOfQualities = CompareQualities(previousDatum.Quality, nextDatum.Quality);
                    if (comparitionOfQualities == -1) newQuality = previousDatum.Quality;
                    else newQuality = nextDatum.Quality;

                    return (new Datum<T>()
                    {
                        Value = newValue,
                        Quality = newQuality,
                        Timestamp = tmp
                    });
                }
            }
        }

        private int FindNumberOfPeriodsBetweenTwoDates(DateTime dateTime1, DateTime dateTime2, Granularity granularity)
        {
            int numberOfPeriods = 0;

            switch (granularity)
            {
                case Granularity.Second: numberOfPeriods = (int)Math.Round((dateTime2 - dateTime1).TotalSeconds); break;
                case Granularity.Minute: numberOfPeriods = (int)Math.Round((dateTime2 - dateTime1).TotalMinutes); break;
                case Granularity.Hour: numberOfPeriods = (int)Math.Round((dateTime2 - dateTime1).TotalHours); break;
                case Granularity.Day: numberOfPeriods = (int)Math.Round((dateTime2 - dateTime1).TotalDays); break;
                case Granularity.Week: numberOfPeriods = (int)Math.Round((dateTime2 - dateTime1).TotalDays / 7); break;
                case Granularity.Month: numberOfPeriods = (int)Math.Round((dateTime2 - dateTime1).TotalDays / 30); break;
                case Granularity.Year: numberOfPeriods = (int)Math.Round((dateTime2 - dateTime1).TotalDays / 365); break;
                default: break;
            }

            return numberOfPeriods;
        }

        private int CompareQualities(Quality q1, Quality q2)
        {
            if (q1 == Quality.None | q2 == Quality.None)
            {
                if (q2 == Quality.None & q1 != Quality.None) return -1;
                if (q1 == Quality.None & q2 != Quality.None) return 1;
                return 0;
            }
            else
            {
                if (q1 > q2) return -1;
                if (q1 < q2) return 1;
                return 0;
            }
        }
    }
}
