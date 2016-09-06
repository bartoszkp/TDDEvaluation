using Domain.MissingValuePolicy;
using Domain.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    static class SpecificDataFillHelper
    {

        public static List<Datum<T>> FillMissingData<T>(SpecificValueMissingValuePolicy<T> mvp, List<Datum<T>> data, DateTime from, DateTime to)
        {
            switch (mvp.Signal.Granularity)
            {
                case Granularity.Second:
                    FillSecondGranularityData(mvp, data, from, to);
                    break;

                case Granularity.Minute:
                    FillMinuteGranularityData(mvp, data, from, to);
                    break;

                case Granularity.Hour:
                    FillHourGranularityData(mvp, data, from, to);
                    break;

                case Granularity.Day:
                    FillDayGranularityData(mvp, data, from, to);
                    break;

                case Granularity.Week:
                    FillWeekGranularityData(mvp, data, from, to);
                    break;

                case Granularity.Month:
                    FillMonthGranularityData(mvp, data, from, to);
                    break;

                case Granularity.Year:
                    FillYearGranularityData(mvp, data, from, to);
                    break;

                default:
                    break;
            }

            return data;
        }




        private static void FillSecondGranularityData<T>(SpecificValueMissingValuePolicy<T> mvp, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var missingDatum = new Datum<T>()
                    {
                        Value = mvp.Value,
                        Timestamp = currentDate,
                        Quality = mvp.Quality
                    };

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddSeconds(1);
            }

        }

        private static void FillMonthGranularityData<T>(SpecificValueMissingValuePolicy<T> mvp, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var missingDatum = new Datum<T>()
                    {
                        Value = mvp.Value,
                        Timestamp = currentDate,
                        Quality = mvp.Quality
                    };

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddMonths(1);
            }
        }

        private static void FillDayGranularityData<T>(SpecificValueMissingValuePolicy<T> mvp, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var missingDatum = new Datum<T>()
                    {
                        Value = mvp.Value,
                        Timestamp = currentDate,
                        Quality = mvp.Quality
                    };

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddDays(1);
            }

        }


        private static void FillMinuteGranularityData<T>(SpecificValueMissingValuePolicy<T> mvp, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var missingDatum = new Datum<T>()
                    {
                        Value = mvp.Value,
                        Timestamp = currentDate,
                        Quality = mvp.Quality
                    };

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddMinutes(1);
            }
        }


        private static void FillYearGranularityData<T>(SpecificValueMissingValuePolicy<T> mvp, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var missingDatum = new Datum<T>()
                    {
                        Value = mvp.Value,
                        Timestamp = currentDate,
                        Quality = mvp.Quality
                    };

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddYears(1);
            }
        }

        private static void FillWeekGranularityData<T>(SpecificValueMissingValuePolicy<T> mvp, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var missingDatum = new Datum<T>()
                    {
                        Value = mvp.Value,
                        Timestamp = currentDate,
                        Quality = mvp.Quality
                    };

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddDays(7);
            }
        }

        private static void FillHourGranularityData<T>(SpecificValueMissingValuePolicy<T> mvp, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var missingDatum = new Datum<T>()
                    {
                        Value = mvp.Value,
                        Timestamp = currentDate,
                        Quality = mvp.Quality
                    };

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddHours(1);
            }
        }



    }
}
