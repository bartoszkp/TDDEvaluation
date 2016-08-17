﻿using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataFillHelpers
{
    static class SpecificDataFillHelper
    {
        public static void FillMissingData<T>(Granularity granularity, List<Datum<T>> data, T value,
            DateTime fromIncluded, DateTime toExcluded)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    FillSecondGranularityData(data, value, fromIncluded, toExcluded);
                    break;

                case Granularity.Minute:
                    FillMinuteGranularityData(data, value, fromIncluded, toExcluded);
                    break;

                case Granularity.Hour:
                    FillHourGranularityData(data, value, fromIncluded, toExcluded);
                    break;

                case Granularity.Day:
                    FillDayGranularityData(data, value, fromIncluded, toExcluded);
                    break;

                case Granularity.Week:
                    FillWeekGranularityData(data, value, fromIncluded, toExcluded);
                    break;

                case Granularity.Month:
                    FillMonthGranularityData(data, value, fromIncluded, toExcluded);
                    break;
                case Granularity.Year:
                    FillYearGranularityData(data, value, fromIncluded, toExcluded);
                    break;

                default:
                    break;
            }
        }

        private static void FillSecondGranularityData<T>(List<Datum<T>> data, T value,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            if (data.Count == 0)
            {
                while (currentDate < toExcluded)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.None,
                        Value = default(T),
                        Timestamp = currentDate
                    });


                    currentDate = currentDate.AddSeconds(1);

                }

                return;
            }



            while (currentDate < toExcluded)
            {
                if (data.Single(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.Fair,
                        Value = value,
                        Timestamp = currentDate
                    });
                }

                currentDate = currentDate.AddSeconds(1);

            }






        }

        private static void FillMonthGranularityData<T>(List<Datum<T>> data, T value,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            if (data.Count == 0)
            {
                while (currentDate < toExcluded)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.None,
                        Value = default(T),
                        Timestamp = currentDate
                    });


                    currentDate = currentDate.AddMonths(1);

                }

                return;
            }

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.Fair,
                        Value = value,
                        Timestamp = currentDate
                    });
                }

                currentDate = currentDate.AddMonths(1);

            }
        }

        private static void FillWeekGranularityData<T>(List<Datum<T>> data, T value,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            if (data.Count == 0)
            {
                while (currentDate < toExcluded)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.None,
                        Value = default(T),
                        Timestamp = currentDate
                    });


                    currentDate = currentDate.AddDays(7);

                }

                return;
            }

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.Fair,
                        Value = value,
                        Timestamp = currentDate
                    });
                }

                currentDate = currentDate.AddDays(7);

            }
        }

        private static void FillYearGranularityData<T>(List<Datum<T>> data, T value,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            if (data.Count == 0)
            {
                while (currentDate < toExcluded)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.None,
                        Value = default(T),
                        Timestamp = currentDate
                    });


                    currentDate = currentDate.AddYears(7);

                }

                return;
            }

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.Fair,
                        Value = value,
                        Timestamp = currentDate
                    });
                }

                currentDate = currentDate.AddYears(7);

            }
        }

        private static void FillDayGranularityData<T>(List<Datum<T>> data, T value,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            if (data.Count == 0)
            {
                while (currentDate < toExcluded)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.None,
                        Value = default(T),
                        Timestamp = currentDate
                    });


                    currentDate = currentDate.AddDays(1);

                }

                return;
            }

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.Fair,
                        Value = value,
                        Timestamp = currentDate
                    });
                }

                currentDate = currentDate.AddDays(1);

            }
        }

        private static void FillHourGranularityData<T>(List<Datum<T>> data, T value,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            if (data.Count == 0)
            {
                while (currentDate < toExcluded)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.None,
                        Value = default(T),
                        Timestamp = currentDate
                    });


                    currentDate = currentDate.AddHours(7);

                }

                return;
            }

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.Fair,
                        Value = value,
                        Timestamp = currentDate
                    });
                }

                currentDate = currentDate.AddHours(7);

            }
        }


        private static void FillMinuteGranularityData<T>(List<Datum<T>> data, T value,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            if (data.Count == 0)
            {
                while (currentDate < toExcluded)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.None,
                        Value = default(T),
                        Timestamp = currentDate
                    });


                    currentDate = currentDate.AddMinutes(7);

                }

                return;
            }

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    data.Add(new Datum<T>()
                    {
                        Quality = Quality.Fair,
                        Value = value,
                        Timestamp = currentDate
                    });
                }

                currentDate = currentDate.AddMinutes(7);

            }
        }
    }


}
