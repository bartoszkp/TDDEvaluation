using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override void FillDatums(ref List<Datum<T>> datumsList, DateTime time, DateTime toExcludedUtc, Signal signal,
            MissingValuePolicy<T> mvp = null, object additionalDatums = null)
        {
            IEnumerable<Datum<T>> convertedAdditionalDatums = (IEnumerable<Datum<T>>)additionalDatums;
            var olderData = convertedAdditionalDatums.ElementAt(0);
            var newerData = convertedAdditionalDatums.ElementAt(1);

            if (olderData == null || newerData == null)
                datumsList.Add(new Datum<T>()
                {
                    Quality = Quality.None,
                    Timestamp = time,
                    Value = default(T),
                    Signal = signal
                });
            else
                datumsList.Add(new Datum<T>()
                {
                    Quality = olderData.Quality > newerData.Quality ? olderData.Quality : newerData.Quality,
                    Timestamp = time,
                    Value = CalculateInterpolatedValue(olderData, newerData, time, signal),
                    Signal = signal
                });
        }

        private T CalculateInterpolatedValue<T>(Datum<T> olderData, Datum<T> newerData, DateTime time, Signal signal)
        {
            int currentTimeDiff = GetNumberOfTimeStepsBetween(signal.Granularity, olderData.Timestamp, time);
            long wholeTimeDiff = GetNumberOfTimeStepsBetween(signal.Granularity, olderData.Timestamp, newerData.Timestamp);

            T addedValue = ((dynamic)newerData.Value - (dynamic)olderData.Value) * currentTimeDiff / wholeTimeDiff;
            return (dynamic)olderData.Value + addedValue;

        }

        private int GetNumberOfTimeStepsBetween(Granularity granularity, DateTime olderTime, DateTime newerTime)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return (int)(newerTime - olderTime).TotalSeconds;
                case Granularity.Minute:
                    return (int)(newerTime - olderTime).TotalMinutes;
                case Granularity.Hour:
                    return (int)(newerTime - olderTime).TotalHours;
                case Granularity.Day:
                    return (int)(newerTime - olderTime).TotalDays;
                case Granularity.Week:
                    return (int)(newerTime - olderTime).TotalDays / 7;
                case Granularity.Month:
                    return (newerTime.Year - olderTime.Year) * 12 + (newerTime.Month - olderTime.Month);
                case Granularity.Year:
                    return (int)(newerTime.Year - olderTime.Year);
                default:
                    return 0;
            }
        }
    }
}
