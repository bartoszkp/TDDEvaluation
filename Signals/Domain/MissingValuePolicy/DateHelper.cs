using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.MissingValuePolicy
{
    internal static class DateHelper
    {
        public static DateTime NextDate(DateTime date, Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return date.AddSeconds(1);
                case Granularity.Minute:
                    return date.AddMinutes(1);
                case Granularity.Hour:
                    return date.AddHours(1);
                case Granularity.Day:
                    return date.AddDays(1);
                case Granularity.Week:
                    return date.AddDays(7);
                case Granularity.Month:
                    return date.AddMonths(1);
                case Granularity.Year:
                    return date.AddYears(1);
                default: return date;
            }
        }
    }
}
