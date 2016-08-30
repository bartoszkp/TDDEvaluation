using System;

namespace Domain
{
    public class Datum<T>
    {
        public static Datum<T> CreateNone(Signal signal, DateTime timestamp)
        {
            return new Datum<T>()
            {
                Id = 0,
                Signal = signal,
                Timestamp = timestamp,
                Value = default(T),
                Quality = Quality.None
            };
        }

        public virtual int Id { get; set; }

        public virtual Signal Signal { get; set; }

        public virtual DateTime Timestamp { get; set; }

        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }

        public virtual T GetFirstOrderValueToAdd(T value, long timestampDifferenceOlderNewerDatum, long timestampDifferenceOlderMissingDatum)
        {
            return default(T);
        }

        public static long GetTimeStampsDifference(Signal signal, DateTime ts1, DateTime ts2)
        {
            TimeSpan ts = ts2.Subtract(ts1);
            switch (signal.Granularity)
            {
                case Granularity.Second:
                    return ts.Seconds;
                case Granularity.Minute:
                    return ts.Minutes;
                case Granularity.Hour:
                    return ts.Hours;
                case Granularity.Day:
                    return ts.Days;
                case Granularity.Week:
                    return ts.Days / 7;
                case Granularity.Month:
                    return (ts2.Year - ts1.Year) * 12 + ts2.Month - ts1.Month;
                case Granularity.Year:
                    return ts2.Year - ts1.Year;
                default:
                    return 0;
            }
        }
    }
}
