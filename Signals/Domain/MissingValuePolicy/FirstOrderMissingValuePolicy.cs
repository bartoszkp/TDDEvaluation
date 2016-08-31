using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;
using Domain.Repositories;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T> 
    {
        public virtual IEnumerable<Datum<T>> SetMissingValues(ISignalsDataRepository repository, Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc) 
        {
            if (typeof(T) == typeof(string) | typeof(T) == typeof(bool)) return repository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            if (fromIncludedUtc > toExcludedUtc) return new List<Datum<T>>();

            var filledData = new List<Datum<T>>();
            DateTime tmp = fromIncludedUtc;
            var data = repository.GetData<T>(signal, fromIncludedUtc, toExcludedUtc);

            if (fromIncludedUtc == toExcludedUtc) AddToListSuitableDatum(repository, signal, data, filledData, tmp);
            else
            {
                switch (signal.Granularity)
                {
                    case Granularity.Second:
                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, data, filledData, tmp);
                            tmp = tmp.AddSeconds(1);
                        }
                        break;
                    case Granularity.Minute:
                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, data, filledData, tmp);
                            tmp = tmp.AddMinutes(1);
                        }
                        break;
                    case Granularity.Hour:
                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, data, filledData, tmp);
                            tmp = tmp.AddHours(1);
                        }
                        break;
                    case Granularity.Day:
                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, data, filledData, tmp);
                            tmp = tmp.AddDays(1);
                        }
                        break;
                    case Granularity.Week:
                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, data, filledData, tmp);
                            tmp = tmp.AddDays(7);
                        }
                        break;
                    case Granularity.Month:
                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, data, filledData, tmp);
                            tmp = tmp.AddMonths(1);
                        }
                        break;
                    case Granularity.Year:
                        while (tmp < toExcludedUtc)
                        {
                            AddToListSuitableDatum(repository, signal, data, filledData, tmp);
                            tmp = tmp.AddYears(1);
                        }
                        break;
                    default: break;
                }
               
            }
            return filledData;
        }

        private void AddToListSuitableDatum(ISignalsDataRepository repository, Signal signal, IEnumerable<Datum<T>> data, List<Datum<T>> filledData, DateTime tmp)
        {
            Datum<T> datumWithTimestampEqualToTmp = SearchForDatumWithTimestampEqualToTmp(data, tmp);
            if (datumWithTimestampEqualToTmp != null) filledData.Add(datumWithTimestampEqualToTmp);
            else
            {
                Datum<T> previousDatum = repository.GetDataOlderThan<T>(signal, tmp, 1).FirstOrDefault();
                Datum<T> nextDatum = repository.GetDataNewerThan<T>(signal, tmp, 1).FirstOrDefault();

                if (previousDatum == null | nextDatum == null) filledData.Add(new Datum<T>() { Timestamp = tmp, Quality = Quality.None, Value = default(T) });
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

                    filledData.Add(new Datum<T>()
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

        private Datum<T> SearchForDatumWithTimestampEqualToTmp(IEnumerable<Datum<T>> data, DateTime tmp)
        {
            Datum<T> datumWithTimestampEqualToTmp = null;

            foreach (var datum in data)
            {
                if (datum.Timestamp == tmp)
                {
                    datumWithTimestampEqualToTmp = new Datum<T>()
                    {
                        Quality = datum.Quality,
                        Value = datum.Value,
                        Timestamp = tmp
                    };
                    break;
                }
            }

            return datumWithTimestampEqualToTmp;
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