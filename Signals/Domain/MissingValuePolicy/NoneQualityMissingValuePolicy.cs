using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data)
        {
            dataDictionary = data.ToDictionary(d => d.Timestamp, d => d);
            
            var datum = DateTimeList(data, signal);
            
            return datum
                .Select(d => dataDictionary.ContainsKey(d) ? dataDictionary[d] : Datum<T>.CreateNone(Signal, d));
        }

        private List<DateTime> DateTimeList(IEnumerable<Datum<T>> data, Signal signal)
        {
            var key = signal.Granularity;

            dictionary = Dictionary();

            DateTime firstTimestamp = data.First().Timestamp;

            datetimeList = new List<DateTime>();
            
            switch (signal.Granularity)
            {
                case Granularity.Second:
                    CreateDateTimeListForSecondGranularity(data, firstTimestamp, key);
                    break;
                case Granularity.Minute:
                    CreateDateTimeListForMinuteGranularity(data, firstTimestamp, key);
                    break;
                case Granularity.Hour:
                    CreateDataTimeListForHourGranularity(data, firstTimestamp, key);
                    break;
                case Granularity.Day:
                    CreateDateTimeListForDayGranularity(data, firstTimestamp, key);
                    break;
                case Granularity.Week:
                    CreateDataTimeListForWeekGranularity(data, firstTimestamp, key);
                    break;
                case Granularity.Month:
                    CreateDateTimeListForMonthGranularity(data, firstTimestamp, key);
                    break;
                case Granularity.Year:
                    CreateDateTimeListForYearGranularity(data, firstTimestamp, key);
                    break;
            }
            
            return datetimeList;
        }

        private void CreateDataTimeListForHourGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        {
            int count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalHours + 1;
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                if (dataDictionary.ContainsKey(firstTimestamp))
                {
                    datetimeList.Add(data.ElementAt(index).Timestamp);
                    index++;
                }
                else
                {
                    datetimeList.Add(dictionary[key](firstTimestamp));
                }
                firstTimestamp = firstTimestamp.AddHours(1);
            }
        }

        private void CreateDateTimeListForDayGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        {
            int count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays + 1;
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                if (dataDictionary.ContainsKey(firstTimestamp))
                {
                    datetimeList.Add(data.ElementAt(index).Timestamp);
                    index++;
                }
                else
                {
                    datetimeList.Add(dictionary[key](firstTimestamp));
                }
                firstTimestamp = firstTimestamp.AddDays(1);
            }
        }

        private void CreateDataTimeListForWeekGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        {
            int count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays / 7 + 1;
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                if (dataDictionary.ContainsKey(firstTimestamp))
                {
                    datetimeList.Add(data.ElementAt(index).Timestamp);
                    index++;
                }
                else
                {
                    datetimeList.Add(dictionary[key](firstTimestamp));
                }
                firstTimestamp = firstTimestamp.AddDays(7);
            }
        }

        private void CreateDateTimeListForMonthGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        {
            int count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays / 30 + 1;
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                if (dataDictionary.ContainsKey(firstTimestamp))
                {
                    datetimeList.Add(data.ElementAt(index).Timestamp);
                    index++;
                }
                else
                {
                    datetimeList.Add(dictionary[key](firstTimestamp));
                }
                firstTimestamp = firstTimestamp.AddMonths(1);
            }
        }

        private void CreateDateTimeListForYearGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        {
            int count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalDays / 365 + 1;
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                if (dataDictionary.ContainsKey(firstTimestamp))
                {
                    datetimeList.Add(data.ElementAt(index).Timestamp);
                    index++;
                }
                else
                {
                    datetimeList.Add(dictionary[key](firstTimestamp));
                }
                firstTimestamp = firstTimestamp.AddYears(1);
            }
        }

        private void CreateDateTimeListForSecondGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        {
            int index = 0;
            int count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalSeconds + 1;
            for (int i = 0; i < count; i++)
            {
                if (dataDictionary.ContainsKey(firstTimestamp))
                {
                    datetimeList.Add(data.ElementAt(index).Timestamp);
                    index++;
                }
                else
                {
                    datetimeList.Add(dictionary[key](firstTimestamp));
                }
                firstTimestamp = firstTimestamp.AddSeconds(1);
            }
        }

        private void CreateDateTimeListForMinuteGranularity(IEnumerable<Datum<T>> data, DateTime firstTimestamp, Granularity key)
        {
            int count = (int)data.Last().Timestamp.Subtract(data.First().Timestamp).TotalMinutes + 1;
            int index = 0;
            for (int i = 0; i < count; i++)
            {
                if (dataDictionary.ContainsKey(firstTimestamp))
                {
                    datetimeList.Add(data.ElementAt(index).Timestamp);
                    index++;
                }
                else
                {
                    datetimeList.Add(dictionary[key](firstTimestamp));
                }
                firstTimestamp = firstTimestamp.AddMinutes(1);
            }
        }

        private List<DateTime> datetimeList;
        private Dictionary<Granularity, Func<DateTime, DateTime>> dictionary;
        private Dictionary<DateTime, Datum<T>> dataDictionary;

        private Dictionary<Granularity, Func<DateTime, DateTime>> Dictionary()
        {
            return new Dictionary<Granularity, Func<DateTime, DateTime>>
            {
                {Granularity.Second, time => time},
                {Granularity.Minute, time => time},
                {Granularity.Hour, time => time},
                {Granularity.Day, time => time },
                {Granularity.Week, time => time},
                {Granularity.Month, time => time },
                {Granularity.Year, time => time}
            };
        }
    }
}
