﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Infrastructure
{
    public class TimeEnumerator : IEnumerator<DateTime>, IEnumerable<DateTime>
    {
        public DateTime FromIncluded { get; private set; }

        public DateTime ToExcluded { get; private set; }

        public Granularity Granularity { get; private set; }

        private DateTime? current;
        public DateTime Current
        {
            get
            {
                return current.Value;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public TimeEnumerator(DateTime fromIncluded, DateTime toExcluded, Granularity granularity)
        {
            this.FromIncluded = fromIncluded;
            this.ToExcluded = toExcluded;
            this.Granularity = granularity;
            this.Reset();
        }

        public TimeEnumerator(DateTime fromIncluded, int steps, Granularity granularity)
        {
            this.FromIncluded = fromIncluded;
            this.ToExcluded = Enumerable.Aggregate(Enumerable.Repeat(this.FromIncluded, steps), (result, step) => granularityTimeSteps[granularity](result));
            this.Granularity = granularity;
            this.Reset();
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (this.current == null)
            {
                this.current = FromIncluded;
                return true;
            }

            this.current = granularityTimeSteps[this.Granularity](this.current.Value);

            return this.current.Value < this.ToExcluded;
        }

        public void Reset()
        {
            this.current = null;
        }

        private static Dictionary<Granularity, Func<DateTime, DateTime>> InitializeGranularityTimeSteps()
        {
            return new Dictionary<Granularity, Func<DateTime, DateTime>> {
                { Granularity.Second, timestamp => timestamp + TimeSpan.FromSeconds(1) },
                { Granularity.Minute, timestamp => timestamp + TimeSpan.FromMinutes(1) },
                { Granularity.Hour, timestamp => timestamp  + TimeSpan.FromHours(1) },
                { Granularity.Day, timestamp => timestamp + TimeSpan.FromDays(1) },
                { Granularity.Week, timestamp => timestamp + TimeSpan.FromDays(7) },
                { Granularity.Month, timestamp => timestamp.AddMonths(1) },
                { Granularity.Year, timestamp => timestamp.AddYears(1) } };
        }

        public IEnumerator<DateTime> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        private static Dictionary<Granularity, Func<DateTime, DateTime>> granularityTimeSteps = InitializeGranularityTimeSteps();
    }
}
