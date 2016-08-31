using Domain.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    static class FirstOrderDataFillHelperDouble
    {
        public static List<Datum<double>> FillMissingData(Signal signal, SignalsDomainService service,
            List<Datum<double>> data, DateTime from, DateTime to)
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

        private static void FillWeekGranularityData(Signal signal, SignalsDomainService service, List<Datum<double>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<double>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<double>(signal, currentDate, 1);
                    Datum<double> previousDatum = null;
                    Datum<double> nextDatum = null;
                    bool previousDatumExisted = true;

                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<double>()
                        {
                            Timestamp = currentDate.AddDays(-7),
                            Value = 0,
                            Quality = Quality.None
                        };
                        previousDatumExisted = false;
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0 || !previousDatumExisted)
                    {
                        var timestamp = currentDate.AddDays(7);

                        nextDatum = new Datum<double>()
                        {
                            Timestamp = timestamp,
                            Value = 0,
                            Quality = Quality.None
                        };
                        if (nextDatum.Timestamp < toExcluded)
                            data.Add(nextDatum);
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);
                    var timeDifference = nextDatum.Timestamp - previousDatum.Timestamp;
                    var weeksDifference = (int)timeDifference.TotalDays / 7;

                    if (weeksDifference > 2)
                    {
                        var tempDate = new DateTime(currentDate.Ticks);
                        var valueToAdd = (nextDatum.Value - previousDatum.Value) / weeksDifference;
                        var value = valueToAdd + previousDatum.Value;


                        while (tempDate < nextDatum.Timestamp && tempDate < toExcluded)
                        {
                            var missingDatum = new Datum<double>()
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
                        var missingDatum = new Datum<double>()
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

        private static void FillDayGranularityData(Signal signal, SignalsDomainService service, List<Datum<double>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<double>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<double>(signal, currentDate, 1);
                    Datum<double> previousDatum = null;
                    Datum<double> nextDatum = null;
                    bool previousDatumExisted = true;

                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<double>()
                        {
                            Timestamp = currentDate.AddDays(-1),
                            Value = 0,
                            Quality = Quality.None
                        };
                        previousDatumExisted = false;
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0 || !previousDatumExisted)
                    {
                        var timestamp = currentDate.AddDays(1);

                        nextDatum = new Datum<double>()
                        {
                            Timestamp = timestamp,
                            Value = 0,
                            Quality = Quality.None
                        };
                        if (nextDatum.Timestamp < toExcluded)
                            data.Add(nextDatum);
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);
                    var timeDifference = nextDatum.Timestamp - previousDatum.Timestamp;
                    var daysDifference = (int)timeDifference.TotalDays;

                    if (daysDifference > 2)
                    {
                        var tempDate = new DateTime(currentDate.Ticks);
                        var valueToAdd = (nextDatum.Value - previousDatum.Value) / daysDifference;
                        var value = valueToAdd + previousDatum.Value;


                        while (tempDate < nextDatum.Timestamp && tempDate < toExcluded)
                        {
                            var missingDatum = new Datum<double>()
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
                        var missingDatum = new Datum<double>()
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

        private static void FillYearGranularityData(Signal signal, SignalsDomainService service, List<Datum<double>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<double>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<double>(signal, currentDate, 1);
                    Datum<double> previousDatum = null;
                    Datum<double> nextDatum = null;
                    bool previousDatumExisted = true;

                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<double>()
                        {
                            Timestamp = currentDate.AddYears(-1),
                            Value = 0,
                            Quality = Quality.None
                        };
                        previousDatumExisted = false;
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0 || !previousDatumExisted)
                    {
                        var timestamp = currentDate.AddYears(1);

                        nextDatum = new Datum<double>()
                        {
                            Timestamp = timestamp,
                            Value = 0,
                            Quality = Quality.None
                        };
                        if (nextDatum.Timestamp < toExcluded)
                            data.Add(nextDatum);
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);
                    var timeDifference = nextDatum.Timestamp - previousDatum.Timestamp;
                    var yearsDifference = (int)timeDifference.TotalDays / 30 / 12;

                    if (yearsDifference > 2)
                    {
                        var tempDate = new DateTime(currentDate.Ticks);
                        var valueToAdd = (nextDatum.Value - previousDatum.Value) / yearsDifference;
                        var value = valueToAdd + previousDatum.Value;


                        while (tempDate < nextDatum.Timestamp && tempDate < toExcluded)
                        {
                            var missingDatum = new Datum<double>()
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
                        var missingDatum = new Datum<double>()
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

        private static void FillHourGranularityData(Signal signal, SignalsDomainService service, List<Datum<double>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<double>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<double>(signal, currentDate, 1);
                    Datum<double> previousDatum = null;
                    Datum<double> nextDatum = null;
                    bool previousDatumExisted = true;

                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<double>()
                        {
                            Timestamp = currentDate.AddHours(-1),
                            Value = 0,
                            Quality = Quality.None
                        };
                        previousDatumExisted = false;
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0 || !previousDatumExisted)
                    {
                        var timestamp = currentDate.AddHours(1);

                        nextDatum = new Datum<double>()
                        {
                            Timestamp = timestamp,
                            Value = 0,
                            Quality = Quality.None
                        };
                        if (nextDatum.Timestamp < toExcluded)
                            data.Add(nextDatum);
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);
                    var timeDifference = nextDatum.Timestamp - previousDatum.Timestamp;
                    var hoursDifference = (int)timeDifference.TotalHours;

                    if (hoursDifference > 2)
                    {
                        var tempDate = new DateTime(currentDate.Ticks);
                        var valueToAdd = (nextDatum.Value - previousDatum.Value) / hoursDifference;
                        var value = valueToAdd + previousDatum.Value;


                        while (tempDate < nextDatum.Timestamp && tempDate < toExcluded)
                        {
                            var missingDatum = new Datum<double>()
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
                        var missingDatum = new Datum<double>()
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

        private static void FillMinuteGranularityData(Signal signal, SignalsDomainService service, List<Datum<double>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<double>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<double>(signal, currentDate, 1);
                    Datum<double> previousDatum = null;
                    Datum<double> nextDatum = null;
                    bool previousDatumExisted = true;

                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<double>()
                        {
                            Timestamp = currentDate.AddMinutes(-1),
                            Value = 0,
                            Quality = Quality.None
                        };
                        previousDatumExisted = false;
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0 || !previousDatumExisted)
                    {
                        var timestamp = currentDate.AddMinutes(-1);

                        nextDatum = new Datum<double>()
                        {
                            Timestamp = timestamp,
                            Value = 0,
                            Quality = Quality.None
                        };
                        if (nextDatum.Timestamp < toExcluded)
                            data.Add(nextDatum);
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);
                    var timeDifference = nextDatum.Timestamp - previousDatum.Timestamp;
                    var minutesDifference = (int)timeDifference.TotalMinutes;

                    if (minutesDifference > 2)
                    {
                        var tempDate = new DateTime(currentDate.Ticks);
                        var valueToAdd = (nextDatum.Value - previousDatum.Value) / minutesDifference;
                        var value = valueToAdd + previousDatum.Value;


                        while (tempDate < nextDatum.Timestamp && tempDate < toExcluded)
                        {
                            var missingDatum = new Datum<double>()
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
                        var missingDatum = new Datum<double>()
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

        private static void FillSecondGranularityData(Signal signal, SignalsDomainService service, List<Datum<double>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<double>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<double>(signal, currentDate, 1);
                    Datum<double> previousDatum = null;
                    Datum<double> nextDatum = null;
                    bool previousDatumExisted = true;

                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<double>()
                        {
                            Timestamp = currentDate.AddSeconds(-1),
                            Value = 0,
                            Quality = Quality.None
                        };
                        previousDatumExisted = false;
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0 || !previousDatumExisted)
                    {
                        var timestamp = currentDate.AddSeconds(1);

                        nextDatum = new Datum<double>()
                        {
                            Timestamp = timestamp,
                            Value = 0,
                            Quality = Quality.None
                        };
                        if (nextDatum.Timestamp < toExcluded)
                            data.Add(nextDatum);
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);
                    var timeDifference = nextDatum.Timestamp - previousDatum.Timestamp;
                    var secondsDifference = (int)timeDifference.TotalSeconds;

                    if (secondsDifference > 2)
                    {
                        var tempDate = new DateTime(currentDate.Ticks);
                        var valueToAdd = (nextDatum.Value - previousDatum.Value) / secondsDifference;
                        var value = valueToAdd + previousDatum.Value;


                        while (tempDate < nextDatum.Timestamp && tempDate < toExcluded)
                        {
                            var missingDatum = new Datum<double>()
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
                        var missingDatum = new Datum<double>()
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


        private static void FillMonthGranularityData(Signal signal, SignalsDomainService service, List<Datum<double>> data,
            DateTime fromIncluded, DateTime toExcluded)
        {
            var currentDate = new DateTime(fromIncluded.Ticks);

            while (currentDate < toExcluded)
            {
                if (data.Find(d => DateTime.Compare(d.Timestamp, currentDate) == 0) == null)
                {
                    var previousDatumCollection = service.GetDataOlderThan<double>(signal, currentDate, 1);
                    var nextDatumCollection = service.GetDataNewerThan<double>(signal, currentDate, 1);
                    Datum<double> previousDatum = null;
                    Datum<double> nextDatum = null;
                    bool previousDatumExisted = true;

                    if (previousDatumCollection.Count() == 0)
                    {
                        previousDatum = new Datum<double>()
                        {
                            Timestamp = currentDate.AddMonths(-1),
                            Value = 0,
                            Quality = Quality.None
                        };
                        previousDatumExisted = false;
                    }
                    else previousDatum = previousDatumCollection.ElementAt(0);


                    if (nextDatumCollection.Count() == 0 || !previousDatumExisted)
                    {
                        var timestamp = currentDate.AddMonths(1);

                        nextDatum = new Datum<double>()
                        {
                            Timestamp = timestamp,
                            Value = 0,
                            Quality = Quality.None
                        };
                        if (nextDatum.Timestamp < toExcluded)
                            data.Add(nextDatum);
                    }
                    else nextDatum = nextDatumCollection.ElementAt(0);

                    var quality = determineQuality(previousDatum.Quality, nextDatum.Quality);
                    var timeDifference = nextDatum.Timestamp - previousDatum.Timestamp;
                    var monthsDifference = (int)timeDifference.TotalDays / 30;

                    if (monthsDifference > 2)
                    {
                        var tempDate = new DateTime(currentDate.Ticks);
                        var valueToAdd = (nextDatum.Value - previousDatum.Value) / monthsDifference;
                        var value = valueToAdd + previousDatum.Value;


                        while (tempDate < nextDatum.Timestamp && tempDate < toExcluded)
                        {
                            var missingDatum = new Datum<double>()
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
                        var missingDatum = new Datum<double>()
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
