using Domain.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    static class FirstOrderDataFillHelperInt
    {

        public static List<Datum<int>> FillMissingData(Signal signal, SignalsDomainService service,
            List<Datum<int>> data, DateTime from, DateTime to)
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
                    FillYearGranularityData(signal, service, data, from, to); ;
                    break;

                default:
                    break;
            }

            return data;
        }

        private static void FillWeekGranularityData(Signal signal, SignalsDomainService service, List<Datum<int>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<int>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<int>(signal, currentDate, 1);
                    Datum<int> previousDatum = null;
                    Datum<int> nextDatum = null;


                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day - 7),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0)
                    {
                        nextDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 7),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var tempDate = new DateTime(currentDate.Ticks);

                    var timeDifference = currentDate - previousDatum.Timestamp;

                    var weeksDifference = (int)timeDifference.TotalDays / 7;
                    var valueToAdd = (nextDatum.Value - previousDatum.Value) / weeksDifference;
                    var value = valueToAdd;
                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);

                    if (nextDatum.Timestamp.Day - currentDate.Day != 7)
                    {
                        while (tempDate < nextDatum.Timestamp)
                        {
                            var missingDatum = new Datum<int>()
                            {
                                Quality = quality,
                                Timestamp = new DateTime(tempDate.Ticks),
                                Value = value
                            };

                            data.Add(missingDatum);

                            value += valueToAdd;
                            tempDate = tempDate.AddDays(7);
                        }
                    }
                    else
                    {
                        var missingDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Ticks),
                            Quality = quality,
                            Value = (nextDatum.Value - previousDatum.Value) / 2
                        };

                        if (nextDatum.Quality == Quality.None)
                        {
                            missingDatum.Value = 0;
                            missingDatum.Quality = Quality.None;
                        }


                        data.Add(missingDatum);
                    }
                }

                currentDate = currentDate.AddDays(7);
            }
        }

        private static void FillDayGranularityData(Signal signal, SignalsDomainService service, List<Datum<int>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<int>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<int>(signal, currentDate, 1);
                    Datum<int> previousDatum = null;
                    Datum<int> nextDatum = null;


                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day - 1),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0)
                    {
                        nextDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day + 1),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var tempDate = new DateTime(currentDate.Ticks);

                    var timeDifference = currentDate - previousDatum.Timestamp;

                    var daysDifference = (int)timeDifference.TotalDays;
                    var valueToAdd = (nextDatum.Value - previousDatum.Value) / daysDifference;
                    var value = valueToAdd;
                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);

                    if (nextDatum.Timestamp.Day - currentDate.Day != 1)
                    {
                        while (tempDate < nextDatum.Timestamp)
                        {
                            var missingDatum = new Datum<int>()
                            {
                                Quality = quality,
                                Timestamp = new DateTime(tempDate.Ticks),
                                Value = value
                            };

                            data.Add(missingDatum);

                            value += valueToAdd;
                            tempDate = tempDate.AddDays(1);
                        }
                    }
                    else
                    {
                        var missingDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Ticks),
                            Quality = quality,
                            Value = (nextDatum.Value - previousDatum.Value) / 2
                        };

                        if (nextDatum.Quality == Quality.None)
                        {
                            missingDatum.Value = 0;
                            missingDatum.Quality = Quality.None;
                        }


                        data.Add(missingDatum);
                    }
                }

                currentDate = currentDate.AddDays(1);
            }
        }

        private static void FillYearGranularityData(Signal signal, SignalsDomainService service, List<Datum<int>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<int>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<int>(signal, currentDate, 1);
                    Datum<int> previousDatum = null;
                    Datum<int> nextDatum = null;


                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year - 1, currentDate.Month, currentDate.Day),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0)
                    {
                        nextDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year + 1, currentDate.Month, currentDate.Day),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var tempDate = new DateTime(currentDate.Ticks); 

                    var yearsDifference = currentDate.Year - previousDatum.Timestamp.Year;
                    var valueToAdd = (nextDatum.Value - previousDatum.Value) / yearsDifference;
                    var value = valueToAdd;
                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);

                    if (nextDatum.Timestamp.Year - currentDate.Year != 1)
                    {
                        while (tempDate < nextDatum.Timestamp)
                        {
                            var missingDatum = new Datum<int>()
                            {
                                Quality = quality,
                                Timestamp = new DateTime(tempDate.Ticks),
                                Value = value
                            };

                            data.Add(missingDatum);

                            value += valueToAdd;
                            tempDate = tempDate.AddYears(1);
                        }
                    }
                    else
                    {
                        var missingDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Ticks),
                            Quality = quality,
                            Value = (nextDatum.Value - previousDatum.Value) / 2
                        };

                        if (nextDatum.Quality == Quality.None)
                        {
                            missingDatum.Value = 0;
                            missingDatum.Quality = Quality.None;
                        }


                        data.Add(missingDatum);
                    }
                }

                currentDate = currentDate.AddYears(1);
            }
        }

        private static void FillHourGranularityData(Signal signal, SignalsDomainService service, List<Datum<int>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<int>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<int>(signal, currentDate, 1);
                    Datum<int> previousDatum = null;
                    Datum<int> nextDatum = null;


                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour,
                                currentDate.Hour - 1, currentDate.Second),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0)
                    {
                        nextDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour,
                                currentDate.Hour + 1, currentDate.Second),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var tempDate = new DateTime(currentDate.Ticks);

                    var timeDifference = currentDate - previousDatum.Timestamp;

                    var hoursDifference = (int)timeDifference.TotalHours;
                    var valueToAdd = (nextDatum.Value - previousDatum.Value) / hoursDifference;
                    var value = valueToAdd;
                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);

                    if (nextDatum.Timestamp.Hour - currentDate.Hour != 1 || currentDate.Day > nextDatum.Timestamp.Day)
                    {
                        while (tempDate < nextDatum.Timestamp)
                        {
                            var missingDatum = new Datum<int>()
                            {
                                Quality = quality,
                                Timestamp = new DateTime(tempDate.Ticks),
                                Value = value
                            };

                            data.Add(missingDatum);

                            value += valueToAdd;
                            tempDate = tempDate.AddHours(1);
                        }
                    }
                    else
                    {
                        var missingDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Ticks),
                            Quality = quality,
                            Value = (nextDatum.Value - previousDatum.Value) / 2
                        };

                        if (nextDatum.Quality == Quality.None)
                        {
                            missingDatum.Value = 0;
                            missingDatum.Quality = Quality.None;
                        }


                        data.Add(missingDatum);
                    }
                }

                currentDate = currentDate.AddHours(1);
            }
        }

        private static void FillMinuteGranularityData(Signal signal, SignalsDomainService service, List<Datum<int>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<int>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<int>(signal, currentDate, 1);
                    Datum<int> previousDatum = null;
                    Datum<int> nextDatum = null;


                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour,
                                currentDate.Minute - 1, currentDate.Second),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0)
                    {
                        nextDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour,
                                currentDate.Minute + 1, currentDate.Second),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var tempDate = new DateTime(currentDate.Ticks);

                    var timeDifference = currentDate - previousDatum.Timestamp;

                    var minutesDifference = (int)timeDifference.TotalMinutes;
                    var valueToAdd = (nextDatum.Value - previousDatum.Value) / minutesDifference;
                    var value = valueToAdd;
                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);

                    if (nextDatum.Timestamp.Minute - currentDate.Minute != 1 || currentDate.Hour < nextDatum.Timestamp.Hour)
                    {
                        while (tempDate < nextDatum.Timestamp)
                        {
                            var missingDatum = new Datum<int>()
                            {
                                Quality = quality,
                                Timestamp = new DateTime(tempDate.Ticks),
                                Value = value
                            };

                            data.Add(missingDatum);

                            value += valueToAdd;
                            tempDate = tempDate.AddMinutes(1);
                        }
                    }
                    else
                    {
                        var missingDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Ticks),
                            Quality = quality,
                            Value = (nextDatum.Value - previousDatum.Value) / 2
                        };

                        if (nextDatum.Quality == Quality.None)
                        {
                            missingDatum.Value = 0;
                            missingDatum.Quality = Quality.None;
                        }


                        data.Add(missingDatum);
                    }
                }

                currentDate = currentDate.AddMinutes(1);
            }
        }

        private static void FillSecondGranularityData(Signal signal, SignalsDomainService service, List<Datum<int>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<int>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<int>(signal, currentDate, 1);
                    Datum<int> previousDatum = null;
                    Datum<int> nextDatum = null;


                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour,
                                currentDate.Minute, currentDate.Second - 1),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0)
                    {
                        nextDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, currentDate.Hour,
                                currentDate.Minute, currentDate.Second + 1),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var tempDate = new DateTime(currentDate.Ticks);

                    var timeDifference = currentDate - previousDatum.Timestamp;

                    var secondsDifference = (int)timeDifference.TotalSeconds;
                    var valueToAdd = (nextDatum.Value - previousDatum.Value) / secondsDifference;
                    var value = valueToAdd;
                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);

                    if (nextDatum.Timestamp.Second - currentDate.Second != 1 || currentDate.Minute < nextDatum.Timestamp.Minute)
                    {
                        while (tempDate < nextDatum.Timestamp)
                        {
                            var missingDatum = new Datum<int>()
                            {
                                Quality = quality,
                                Timestamp = new DateTime(tempDate.Ticks),
                                Value = value
                            };

                            data.Add(missingDatum);

                            value += valueToAdd;
                            tempDate = tempDate.AddSeconds(1);
                        }
                    }
                    else
                    {
                        var missingDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Ticks),
                            Quality = quality,
                            Value = (nextDatum.Value - previousDatum.Value) / 2
                        };

                        if (nextDatum.Quality == Quality.None)
                        {
                            missingDatum.Value = 0;
                            missingDatum.Quality = Quality.None;
                        }


                        data.Add(missingDatum);
                    }
                }

                currentDate = currentDate.AddSeconds(1);
            }
        }


        private static void FillMonthGranularityData(Signal signal, SignalsDomainService service, List<Datum<int>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<int>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<int>(signal, currentDate, 1);
                    Datum<int> previousDatum = null;
                    Datum<int> nextDatum = null;


                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year, currentDate.Month - 1, currentDate.Day),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0)
                    {
                        nextDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Year, currentDate.Month + 1, currentDate.Day),
                            Value = 0,
                            Quality = Quality.None
                        };
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var tempDate = new DateTime(currentDate.Ticks);

                    var timeDifference = currentDate - previousDatum.Timestamp;

                    var monthsDifference = (int)timeDifference.TotalDays / 30;
                    var valueToAdd = (nextDatum.Value - previousDatum.Value) / monthsDifference;
                    var value = valueToAdd;
                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);

                    if (nextDatum.Timestamp.Month - currentDate.Month != 1 || currentDate.Month > nextDatum.Timestamp.Month)
                    {
                        while (tempDate < nextDatum.Timestamp)
                        {
                            var missingDatum = new Datum<int>()
                            {
                                Quality = quality,
                                Timestamp = new DateTime(tempDate.Ticks),
                                Value = value
                            };

                            data.Add(missingDatum);

                            value += valueToAdd;
                            tempDate = tempDate.AddMonths(1);
                        }
                    }
                    else
                    {
                        var missingDatum = new Datum<int>()
                        {
                            Timestamp = new DateTime(currentDate.Ticks),
                            Quality = quality,
                            Value = (nextDatum.Value - previousDatum.Value) / 2
                        };

                        if (nextDatum.Quality == Quality.None)
                        {
                            missingDatum.Value = 0;
                            missingDatum.Quality = Quality.None;
                        }


                        data.Add(missingDatum);
                    }
                }

                currentDate = currentDate.AddMonths(1);
            }

        }

        private static Quality determineQuality(Quality q1, Quality q2)
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


    }
}
