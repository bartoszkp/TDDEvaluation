using Domain.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class ZeroOrderDataFillHelper
    {

        public static List<Datum<T>> FillMissingData<T>(SignalsDomainService service, Signal signal, List<Datum<T>> data, DateTime from, DateTime to)
        {


            switch (signal.Granularity)
            {
                case Granularity.Second:
                    FillSecondGranularityData(signal, service, data, from, to);
                    break;

                case Granularity.Minute:
                    FillMinuteGranularityData(signal, service, data, from, to);
                    break;

                case Granularity.Hour:
                    FillHourGranularityData(signal, service, data, from, to);
                    break;

                case Granularity.Day:
                    FillDayGranularityData(signal, service, data, from, to);
                    break;

                case Granularity.Week:
                    FillWeekGranularityData(signal, service, data, from, to);
                    break;

                case Granularity.Month:
                    FillMonthGranularityData(signal, service, data, from, to);
                    break;

                case Granularity.Year:
                    FillYearGranularityData(signal, service, data, from, to);
                    break;

                default:
                    break;
            }

            return data;
        }




        private static void FillSecondGranularityData<T>(Signal signal,SignalsDomainService service,List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var olderData = service.GetDataOlderThan<T>(signal, currentDate, 1);

                    Datum<T> missingDatum = null;

                    if (olderData.Count() != 0)
                    {
                        var previousDatum = olderData.ElementAt(0);

                        missingDatum = new Datum<T>()
                        {
                            Value = previousDatum.Value,
                            Timestamp = currentDate,
                            Quality = previousDatum.Quality
                        };
                    }
                    else
                    {
                        missingDatum = new Datum<T>()
                        {
                            Value = default(T),
                            Timestamp = currentDate,
                            Quality = Quality.None
                        };
                    }

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddSeconds(1);
            }

        }

        private static void FillMonthGranularityData<T>(Signal signal, SignalsDomainService service, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var olderData = service.GetDataOlderThan<T>(signal, currentDate, 1);

                    Datum<T> missingDatum = null;

                    if (olderData.Count() != 0)
                    {
                        var previousDatum = olderData.ElementAt(0);

                        missingDatum = new Datum<T>()
                        {
                            Value = previousDatum.Value,
                            Timestamp = currentDate,
                            Quality = previousDatum.Quality
                        };
                    }
                    else
                    {
                        missingDatum = new Datum<T>()
                        {
                            Value = default(T),
                            Timestamp = currentDate,
                            Quality = Quality.None
                        };
                    }

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddMonths(1);
            }
        }

        private static void FillDayGranularityData<T>(Signal signal, SignalsDomainService service, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var olderData = service.GetDataOlderThan<T>(signal, currentDate, 1);

                    Datum<T> missingDatum = null;

                    if (olderData.Count() != 0)
                    {
                        var previousDatum = olderData.ElementAt(0);

                        missingDatum = new Datum<T>()
                        {
                            Value = previousDatum.Value,
                            Timestamp = currentDate,
                            Quality = previousDatum.Quality
                        };
                    }
                    else
                    {
                        missingDatum = new Datum<T>()
                        {
                            Value = default(T),
                            Timestamp = currentDate,
                            Quality = Quality.None
                        };
                    }

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddDays(1);
            }

        }


        private static void FillMinuteGranularityData<T>(Signal signal, SignalsDomainService service, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var olderData = service.GetDataOlderThan<T>(signal, currentDate, 1);

                    Datum<T> missingDatum = null;

                    if (olderData.Count() != 0)
                    {
                        var previousDatum = olderData.ElementAt(0);

                        missingDatum = new Datum<T>()
                        {
                            Value = previousDatum.Value,
                            Timestamp = currentDate,
                            Quality = previousDatum.Quality
                        };
                    }
                    else
                    {
                        missingDatum = new Datum<T>()
                        {
                            Value = default(T),
                            Timestamp = currentDate,
                            Quality = Quality.None
                        };
                    }

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddMinutes(1);
            }
        }


        private static void FillYearGranularityData<T>(Signal signal, SignalsDomainService service, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var olderData = service.GetDataOlderThan<T>(signal, currentDate, 1);

                    Datum<T> missingDatum = null;

                    if (olderData.Count() != 0)
                    {
                        var previousDatum = olderData.ElementAt(0);

                        missingDatum = new Datum<T>()
                        {
                            Value = previousDatum.Value,
                            Timestamp = currentDate,
                            Quality = previousDatum.Quality
                        };
                    }
                    else
                    {
                        missingDatum = new Datum<T>()
                        {
                            Value = default(T),
                            Timestamp = currentDate,
                            Quality = Quality.None
                        };
                    }

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddYears(1);
            }
        }

        private static void FillWeekGranularityData<T>(Signal signal, SignalsDomainService service, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var olderData = service.GetDataOlderThan<T>(signal, currentDate, 1);

                    Datum<T> missingDatum = null;

                    if (olderData.Count() != 0)
                    {
                        var previousDatum = olderData.ElementAt(0);

                        missingDatum = new Datum<T>()
                        {
                            Value = previousDatum.Value,
                            Timestamp = currentDate,
                            Quality = previousDatum.Quality
                        };
                    }
                    else
                    {
                        missingDatum = new Datum<T>()
                        {
                            Value = default(T),
                            Timestamp = currentDate,
                            Quality = Quality.None
                        };
                    }

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddDays(7);
            }
        }

        private static void FillHourGranularityData<T>(Signal signal, SignalsDomainService service, List<Datum<T>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var olderData = service.GetDataOlderThan<T>(signal, currentDate, 1);

                    Datum<T> missingDatum = null;

                    if (olderData.Count() != 0)
                    {
                        var previousDatum = olderData.ElementAt(0);

                        missingDatum = new Datum<T>()
                        {
                            Value = previousDatum.Value,
                            Timestamp = currentDate,
                            Quality = previousDatum.Quality
                        };
                    }
                    else
                    {
                        missingDatum = new Datum<T>()
                        {
                            Value = default(T),
                            Timestamp = currentDate,
                            Quality = Quality.None
                        };
                    }

                    data.Add(missingDatum);
                }

                currentDate = currentDate.AddHours(1);
            }
        }





    }
}
