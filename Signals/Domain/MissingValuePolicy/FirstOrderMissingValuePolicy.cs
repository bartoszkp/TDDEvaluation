using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class FirstOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        [NHibernateIgnore]
        public override int OlderDataSampleCountNeeded { get { return 1; } }

        [NHibernateIgnore]
        public override int NewerDataSampleCountNeeded { get { return 1; } }

        public override IEnumerable<Datum<T>> FillMissingData(
            TimeEnumerator timeEnumerator,
            IEnumerable<Datum<T>> readData,
            IEnumerable<Datum<T>> additionalOlderData,
            IEnumerable<Datum<T>> additionalNewerData)
        {
            var readEnumerator = readData.GetEnumerator();
            var dummyDataOnEnd = new Datum<T> { Quality = Quality.None, Timestamp = timeEnumerator.ToExcludedUtcUtc };
            var lastData = additionalNewerData.DefaultIfEmpty(dummyDataOnEnd).Single();
            var olderData = additionalOlderData.DefaultIfEmpty(new Datum<T> { Quality = Quality.None }).Single();
            var newerData = readEnumerator.MoveNext() ? readEnumerator.Current : lastData;

            foreach (var ts in timeEnumerator)
            {
                if (ts == newerData.Timestamp)
                {
                    olderData = newerData;
                    newerData = readEnumerator.MoveNext() ? readEnumerator.Current : lastData;
                    yield return olderData;
                }
                else
                    yield return Interpolate(olderData, newerData, ts);
            }
        }

        private Quality GetMinQuality(Quality a, Quality b)
        {
            // TODO should we add numbers to Quality? Those would probably go into Dto and suggest some impl...
            if (a == Quality.None || b == Quality.None)
                return Quality.None;
            return (Quality)Math.Max((int)a, (int)b);
        }

        private Datum<T> Interpolate(Datum<T> older, Datum<T> newer, DateTime currentTs)
        {
            var q = GetMinQuality(older.Quality, newer.Quality);
            if (q == Quality.None)
                return Datum<T>.CreateNone(Signal, currentTs);
            var timeSpan = (newer.Timestamp - older.Timestamp).TotalSeconds;
            var valueSpan = (dynamic)newer.Value - (dynamic)older.Value; // yuck!
            var step = valueSpan / timeSpan;
            var currentValue = Convert.ChangeType(
                (currentTs - older.Timestamp).TotalSeconds * step + older.Value,
                typeof(T));
            return new Datum<T> { Value = currentValue, Timestamp = currentTs, Quality = q, Signal = Signal };
        }
    }
}
