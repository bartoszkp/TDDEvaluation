using System;

namespace Domain.Infrastructure
{
    public static class GranularityUtils
    {
        private static void requireZeroMilliseconds(DateTime timestamp)
        {
            if (timestamp.Millisecond != 0)
                throw new ArgumentException("Non zero Millisecond value", "timestamp");
        }

        private static void requireZeroSeconds(DateTime timestamp)
        {
            if (timestamp.Second != 0)
                throw new ArgumentException("Non zero Second value", "timestamp");
        }

        private static void requireZeroMinutes(DateTime timestamp)
        {
            if (timestamp.Minute!= 0)
                throw new ArgumentException("Non zero Minute value", "timestamp");
        }

        private static void requireZeroHours(DateTime timestamp)
        {
            if (timestamp.Hour != 0)
                throw new ArgumentException("Non zero Hour value", "timestamp");
        }

        private static void requireMonday(DateTime timestamp)
        {
            if (timestamp.DayOfWeek != DayOfWeek.Monday)
                throw new ArgumentException("Non Monday in DayOfWeek", "timestamp");
        }

        private static void requireFirstDayOfMonth(DateTime timestamp)
        {
            if (timestamp.Day != 1)
                throw new ArgumentException("Not first Day of Month", "timestamp");
        }

        private static void requireFirstMonth(DateTime timestamp)
        {
            if (timestamp.Month != 1)
                throw new ArgumentException("Not first Month", "timestamp");
        }

        public static void ValidateTimestamp(this Granularity @this, DateTime timestamp)
        {
            switch (@this)
            {
                case Granularity.Second:
                    requireZeroMilliseconds(timestamp);
                    break;
                case Granularity.Minute:
                    requireZeroMilliseconds(timestamp);
                    requireZeroSeconds(timestamp);
                    break;
                case Granularity.Hour:
                    requireZeroMilliseconds(timestamp);
                    requireZeroSeconds(timestamp);
                    requireZeroMinutes(timestamp);
                    break;
                case Granularity.Day:
                    requireZeroMilliseconds(timestamp);
                    requireZeroSeconds(timestamp);
                    requireZeroMinutes(timestamp);
                    requireZeroHours(timestamp);
                    break;
                case Granularity.Week:
                    requireZeroMilliseconds(timestamp);
                    requireZeroSeconds(timestamp);
                    requireZeroMinutes(timestamp);
                    requireZeroHours(timestamp);
                    requireMonday(timestamp);
                    break;
                case Granularity.Month:
                    requireZeroMilliseconds(timestamp);
                    requireZeroSeconds(timestamp);
                    requireZeroMinutes(timestamp);
                    requireZeroHours(timestamp);
                    requireFirstDayOfMonth(timestamp);
                    break;
                case Granularity.Year:
                    requireZeroMilliseconds(timestamp);
                    requireZeroSeconds(timestamp);
                    requireZeroMinutes(timestamp);
                    requireZeroHours(timestamp);
                    requireFirstDayOfMonth(timestamp);
                    requireFirstMonth(timestamp);
                    break;
            }
        }
    }
}
