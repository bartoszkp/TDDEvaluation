using Domain;
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

                    break;
                case Granularity.Hour:
                    break;
                case Granularity.Day:
                    break;
                case Granularity.Week:
                    break;
                case Granularity.Month:
                    FillMonthGranularityData(data, value, fromIncluded, toExcluded);
                    break;
                case Granularity.Year:
                    break;
                default:
                    break;
            }
        }

        private static void FillSecondGranularityData<T>(List<Datum<T>> data, T value,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

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


    }


}
