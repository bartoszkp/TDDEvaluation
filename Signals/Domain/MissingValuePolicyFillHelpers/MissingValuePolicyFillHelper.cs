using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    [Infrastructure.NHibernateIgnore]
    public class MissingValuePolicyFillHelper
    {

        protected static DateTime AddTime(DateTime time, Granularity granularity, int interval = 1)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return time.AddSeconds(interval);

                case Granularity.Minute:
                    return time.AddMinutes(interval);

                case Granularity.Hour:
                    return time.AddHours(interval);

                case Granularity.Day:
                    return time.AddDays(interval);

                case Granularity.Week:
                    return time.AddDays(interval * 7);

                case Granularity.Month:
                    return time.AddMonths(interval);

                case Granularity.Year:
                    return time.AddYears(interval);
            }
            throw new NotSupportedException("Granularity: " + granularity.ToString() + " is not supported");
        }
    }
}
