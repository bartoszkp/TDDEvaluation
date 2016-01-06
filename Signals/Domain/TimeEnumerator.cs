using System;
using System.Collections;
using System.Collections.Generic;

namespace Signals.Domain
{
    public class TimeEnumerator : IEnumerator<DateTime>, IEnumerable<DateTime>
    {
        public DateTime FromIncluded { get; private set; }

        public int Steps { get; private set; }

        public DateTime? ToExcluded { get; private set; }

        public Granularity Granularity { get; private set; }

        private int currentStep;
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
            this.ToExcluded = null;
            this.Steps = steps;
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

            this.currentStep += 1;
            this.current = this.current.Value + granularityTimeSpan[this.Granularity];

            if (this.ToExcluded.HasValue)
            {
                return this.current.Value < this.ToExcluded;
            }
            else
            {
                return this.currentStep < this.Steps;
            }
        }

        public void Reset()
        {
            this.current = null;
            this.currentStep = 0;
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
