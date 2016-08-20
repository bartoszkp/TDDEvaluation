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

        [NHibernateIgnore]
        public override IEnumerable<Type> CompatibleNativeTypes
        {
            get
            {
                return base.CompatibleNativeTypes.Except(new[] { typeof(string), typeof(bool) });
            }
        }

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
            if (a == Quality.None || b == Quality.None)
                return Quality.None;

            return Enum
                .GetValues(typeof(Quality))
                .Cast<Quality>()
                .Last(q => q == a || q == b);
        }

        private Datum<T> Interpolate(Datum<T> older, Datum<T> newer, DateTime currentTs)
        {
            var resultQuality = GetMinQuality(older.Quality, newer.Quality);

            if (resultQuality == Quality.None)
                return Datum<T>.CreateNone(Signal, currentTs);

            var totalSpanTimeEnumerator = new TimeEnumerator(older.Timestamp, newer.Timestamp, Signal.Granularity);
            var fromOlderToCurrentSpanTimeEnumerator = new TimeEnumerator(older.Timestamp, currentTs, Signal.Granularity);

            var totalStepCount = (decimal)totalSpanTimeEnumerator.Count();
            var fromOlderToCurrentStepCount = (decimal)fromOlderToCurrentSpanTimeEnumerator.Count();

            var valueSpan = Convert.ChangeType((dynamic)newer.Value - (dynamic)older.Value, typeof(decimal));
            var step = valueSpan / totalStepCount;

            var increase = Convert.ChangeType(
                fromOlderToCurrentStepCount * step,
                typeof(T));

            return new Datum<T> { Value = older.Value + increase, Timestamp = currentTs, Quality = resultQuality, Signal = Signal };
        }
    }
}
