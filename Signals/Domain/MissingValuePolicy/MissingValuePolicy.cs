using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public abstract class MissingValuePolicyBase
    {
        public virtual int? Id { get; set; }

        public virtual Signal Signal { get; set; }

        [NHibernateIgnore]
        public abstract Type NativeDataType { get; }
    }

    public abstract class MissingValuePolicy<T> : MissingValuePolicyBase
    {
        [NHibernateIgnore]
        public override Type NativeDataType { get { return typeof(T); } }

        public virtual Datum<T> GetDatumToFill(DateTime timestamp)
        {
            return new Datum<T>()
            {
                Quality = Quality.None,
                Value = default(T),
                Timestamp = timestamp
            };
        }

        public virtual DateTime AddTime(Granularity granularity, DateTime time, int timeToAdd = 1)
        {
            switch (granularity)
            {
                case Granularity.Day:
                    return time.AddDays(timeToAdd);

                case Granularity.Hour:
                    return time.AddHours(timeToAdd);

                case Granularity.Minute:
                    return time.AddMinutes(timeToAdd);

                case Granularity.Month:
                    return time.AddMonths(timeToAdd);

                case Granularity.Second:
                    return time.AddSeconds(timeToAdd);

                case Granularity.Week:
                    return time.AddDays(7 * timeToAdd);

                case Granularity.Year:
                    return time.AddYears(timeToAdd);
            }

            throw new NotSupportedException("Granularity " + granularity.ToString() + " is not supported");
        }

    }
}