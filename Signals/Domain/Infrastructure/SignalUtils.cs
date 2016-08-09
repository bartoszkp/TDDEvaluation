using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
