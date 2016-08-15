using System;
using Domain;
using Domain.Infrastructure;

namespace SignalsIntegrationTests.Infrastructure
{
    public static class DateTimeUtils
    {
        public static DateTime AddSteps(this DateTime @this, Granularity granularity, int steps)
        {
            granularity.ValidateTimestamp(@this);

            switch (granularity)
            {
                case Granularity.Second:
                    return @this.AddSeconds(steps);
                case Granularity.Minute:
                    return @this.AddMinutes(steps);
                case Granularity.Hour:
                    return @this.AddHours(steps);
                case Granularity.Day:
                    return @this.AddDays(steps);
                case Granularity.Week:
                    return @this.AddDays(steps * 7);
                case Granularity.Month:
                    return @this.AddMonths(steps);
                case Granularity.Year:
                    return @this.AddYears(steps);
            }

            throw new InvalidOperationException();
        }
    }
}
