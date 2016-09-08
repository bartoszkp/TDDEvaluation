using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Infrastructure;
using Domain.Services.Implementation;

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

        public abstract IEnumerable<Datum<T>> FillData(Signal signal, IEnumerable<Datum<T>> data, 
            DateTime fromIncludedUtc, DateTime toExcludedUtc, 
            Datum<T> olderDatum = null, Datum<T> neverDatum = null, SignalsDomainService service = null);

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