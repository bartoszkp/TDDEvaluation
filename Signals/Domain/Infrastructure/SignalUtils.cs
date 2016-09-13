using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Exceptions;

namespace Domain.Infrastructure
{
    public static class SignalUtils
    {
        public static DateTime GetNextDate(DateTime dt, Granularity granularity)
        {
            switch(granularity)
            {
                case Granularity.Year:
                    return dt.AddYears(1);
                case Granularity.Month:
                    return dt.AddMonths(1);
                case Granularity.Week:
                    return dt.AddDays(7);
                case Granularity.Day:
                    return dt.AddDays(1);
                case Granularity.Hour:
                    return dt.AddHours(1);
                case Granularity.Minute:
                    return dt.AddMinutes(1);
                case Granularity.Second:
                    return dt.AddSeconds(1);
                default:
                    throw new ArgumentException("Unknown Granularity", granularity.ToString());
            }
        }

        public static void CheckCorrectionOfTimestamp(DateTime date, Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Year:
                    if (date.Month != 1)
                    {
                        throw new IncorrectTimeStampException();
                    }
                    goto case Granularity.Month;
                case Granularity.Month:
                    if (date.Day != 1)
                    {
                        throw new IncorrectTimeStampException();
                    }
                    goto case Granularity.Day;
                case Granularity.Week:
                    if (date.DayOfWeek != DayOfWeek.Monday)
                    {
                        throw new IncorrectTimeStampException();
                    }
                    goto case Granularity.Day;
                case Granularity.Day:
                    if (date.Hour != 0)
                    {
                        throw new IncorrectTimeStampException();
                    }
                    goto case Granularity.Hour;
                case Granularity.Hour:
                    if (date.Minute != 0)
                    {
                        throw new IncorrectTimeStampException();
                    }
                    goto case Granularity.Minute;
                case Granularity.Minute:
                    if (date.Second != 0)
                    {
                        throw new IncorrectTimeStampException();
                    }
                    goto case Granularity.Second;
                case Granularity.Second:
                    if (date.Millisecond != 0)
                    {
                        throw new IncorrectTimeStampException();
                    }
                    break;
            }
        }
    }
}
