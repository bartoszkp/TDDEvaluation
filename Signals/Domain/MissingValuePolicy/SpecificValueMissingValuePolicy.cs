using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }

        public virtual IEnumerable<Datum<T>> SetMissingValue(IEnumerable<Datum<T>> data, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            Datum<T>[] parameterArray = data.ToArray();
            Datum<T>[] filledArray = null;
            Granularity granularity = data.First().Signal.Granularity;
            DateTime tmp = fromIncludedUtc;
            int timeSpan = 0;

            switch (granularity)
            {
                case Granularity.Second:

                    timeSpan = toExcludedUtc.Second - fromIncludedUtc.Second;
                    filledArray = new Datum<T>[timeSpan];

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>() { Timestamp = tmp, Quality = Quality, Value = Value };
                        tmp = new DateTime(tmp.Year, tmp.Month, tmp.Day, tmp.Hour, tmp.Minute, tmp.Second + 1);
                    }

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (filledArray[i].Timestamp == parameterArray[j].Timestamp) filledArray[i] = parameterArray[j];
                        }
                    }

                    break;

                case Granularity.Minute:

                    timeSpan = toExcludedUtc.Minute - fromIncludedUtc.Minute;
                    filledArray = new Datum<T>[timeSpan];

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>() { Timestamp = tmp, Quality = Quality, Value = Value };
                        tmp = new DateTime(tmp.Year, tmp.Month, tmp.Day, tmp.Hour, tmp.Minute + 1, tmp.Second);
                    }

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (filledArray[i].Timestamp == parameterArray[j].Timestamp) filledArray[i] = parameterArray[j];
                        }
                    }

                    break;

                case Granularity.Hour:

                    timeSpan = toExcludedUtc.Hour - fromIncludedUtc.Hour;
                    filledArray = new Datum<T>[timeSpan];

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>() { Timestamp = tmp, Quality = Quality, Value = Value };
                        tmp = new DateTime(tmp.Year, tmp.Month, tmp.Day, tmp.Hour+1 , tmp.Minute, tmp.Second);
                    }

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (filledArray[i].Timestamp == parameterArray[j].Timestamp) filledArray[i] = parameterArray[j];
                        }
                    }

                    break;

                case Granularity.Day:

                    timeSpan = toExcludedUtc.Day - fromIncludedUtc.Day;
                    filledArray = new Datum<T>[timeSpan];

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>() { Timestamp = tmp, Quality = Quality, Value = Value };
                        tmp = new DateTime(tmp.Year, tmp.Month, tmp.Day + 1);
                    }

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (filledArray[i].Timestamp == parameterArray[j].Timestamp) filledArray[i] = parameterArray[j];
                        }
                    }

                    break;

                case Granularity.Week:

                    timeSpan = (toExcludedUtc.Day - fromIncludedUtc.Day) / 7;
                    filledArray = new Datum<T>[timeSpan];

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>() { Timestamp = tmp, Quality = Quality, Value = Value };
                        tmp = new DateTime(tmp.Year, tmp.Month, tmp.Day + 1);
                    }

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (filledArray[i].Timestamp == parameterArray[j].Timestamp) filledArray[i] = parameterArray[j];
                        }
                    }

                    break;

                case Granularity.Month:

                    timeSpan = toExcludedUtc.Month - fromIncludedUtc.Month;
                    filledArray = new Datum<T>[timeSpan];

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>() { Timestamp = tmp, Quality = Quality, Value = Value };
                        tmp = new DateTime(tmp.Year, tmp.Month + 1, tmp.Day);
                    }

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (filledArray[i].Timestamp == parameterArray[j].Timestamp) filledArray[i] = parameterArray[j];
                        }
                    }

                    break;

                case Granularity.Year:

                    timeSpan = toExcludedUtc.Year - fromIncludedUtc.Year;
                    filledArray = new Datum<T>[timeSpan];

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        filledArray[i] = new Datum<T>() { Timestamp = tmp, Quality = Quality, Value = Value };
                        tmp = new DateTime(tmp.Year + 1, tmp.Month, tmp.Day);
                    }

                    for (int i = 0; i < filledArray.Length; i++)
                    {
                        for (int j = 0; j < parameterArray.Length; j++)
                        {
                            if (filledArray[i].Timestamp == parameterArray[j].Timestamp) filledArray[i] = parameterArray[j];
                        }
                    }

                    break;

                default:

                    break;
            }

            return filledArray;
            
        }
    }
}
