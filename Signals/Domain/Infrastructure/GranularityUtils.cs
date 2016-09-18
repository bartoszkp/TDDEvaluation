using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domain.Infrastructure
{
    public static class GranularityUtils
    {
        public static void ValidateTimestamp(this Granularity @this, DateTime timestamp)
        {
            switch (@this)
            {
                case Granularity.Second:
                    RequireZero(timestamp, ts => ts.Millisecond);
                    break;
                case Granularity.Minute:
                    RequireAllZero(timestamp, ts => ts.Millisecond, ts => ts.Second);
                    break;
                case Granularity.Hour:
                    RequireAllZero(timestamp, ts => ts.Millisecond, ts => ts.Second, ts => ts.Minute);
                    break;
                case Granularity.Day:
                    RequireAllZero(timestamp, ts => ts.Millisecond, ts => ts.Second, ts => ts.Minute, ts => ts.Hour);
                    break;
                case Granularity.Week:
                    RequireAllZero(timestamp, ts => ts.Millisecond, ts => ts.Second, ts => ts.Minute, ts => ts.Hour);
                    RequireMonday(timestamp);
                    break;
                case Granularity.Month:
                    RequireAllZero(timestamp, ts => ts.Millisecond, ts => ts.Second, ts => ts.Minute, ts => ts.Hour);
                    RequireFirstDayOfMonth(timestamp);
                    break;
                case Granularity.Year:
                    RequireAllZero(timestamp, ts => ts.Millisecond, ts => ts.Second, ts => ts.Minute, ts => ts.Hour);
                    RequireFirstDayOfMonth(timestamp);
                    RequireFirstMonth(timestamp);
                    break;
            }
        }

        private static void RequireAllZero(DateTime timestamp, params Expression<Func<DateTime, int>>[] expressions)
        {
            foreach (var expression in expressions)
            {
                RequireZero(timestamp, expression);
            }
        }

        private static void RequireZero(DateTime timestamp, Expression<Func<DateTime, int>> expression)
        {
            var compiled = expression.Compile();

            if (compiled(timestamp) != 0)
                throw new ArgumentException(string.Format("Non zero DateTime property: {0}.", ReflectionUtils.GetMemberInfo(expression).Name), "timestamp");
        }

        private static void RequireMonday(DateTime timestamp)
        {
            if (timestamp.DayOfWeek != DayOfWeek.Monday)
                throw new ArgumentException("Non Monday in DayOfWeek", "timestamp");
        }

        private static void RequireFirstDayOfMonth(DateTime timestamp)
        {
            if (timestamp.Day != 1)
                throw new ArgumentException("Not first Day of Month", "timestamp");
        }

        private static void RequireFirstMonth(DateTime timestamp)
        {
            if (timestamp.Month != 1)
                throw new ArgumentException("Not first Month", "timestamp");
        }
    }
}
