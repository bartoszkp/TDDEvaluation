using System;
using System.Collections;
using System.Collections.Generic;

namespace Domain.Infrastructure
{
    public class TimeEnumerator : IEnumerator<DateTime>, IEnumerable<DateTime>
    {
        public DateTime FromIncluded { get; private set; }

        public DateTime ToExcluded { get; private set; }

        public long Steps
        {
            get
            {
                return (ToExcluded - FromIncluded).Ticks / granularityTimeSpan[Granularity].Ticks;
            }
        }

        public Granularity Granularity { get; private set; }

        private DateTime? current;
        public DateTime Current
        {
            get
            {
                return current.Value;
            }
        }

        public long CurrentStep
        {
            get
            {
                return (Current - FromIncluded).Ticks / granularityTimeSpan[Granularity].Ticks;
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
            this.ToExcluded = fromIncluded + TimeSpan.FromTicks(granularityTimeSpan[granularity].Ticks * steps);
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

            this.current = this.current.Value + granularityTimeSpan[this.Granularity];

            return this.current.Value < this.ToExcluded;
        }

        public void Reset()
        {
            this.current = null;
        }

        private static Dictionary<Granularity, TimeSpan> InitializeGranularityTimeSpan()
        {
            return new Dictionary<Granularity, TimeSpan> {
                { Granularity.Second, TimeSpan.FromSeconds(1) },
                { Granularity.Minute, TimeSpan.FromMinutes(1) },
                { Granularity.Hour, TimeSpan.FromHours(1) },
                { Granularity.Day, TimeSpan.FromDays(1) },
                { Granularity.Week, TimeSpan.FromDays(7) } };
        }

        public IEnumerator<DateTime> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        private static Dictionary<Granularity, TimeSpan> granularityTimeSpan = InitializeGranularityTimeSpan();
    }
}
