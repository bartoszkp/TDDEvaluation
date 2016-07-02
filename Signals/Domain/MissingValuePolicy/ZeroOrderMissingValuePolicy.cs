using Domain.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override int OlderDataSamplesCountNeeded { get { return 1; } }

        public override IEnumerable<Datum<T>> FillMissingData(TimeEnumerator timeEnumerator,
                                                              IEnumerable<Datum<T>> readData,
                                                              IEnumerable<Datum<T>> additionalOlderData)
        {
            var readEnumerator = readData.GetEnumerator();
            var datumBeforeRange = additionalOlderData.DefaultIfEmpty(new Datum<T> { Quality = Quality.None }).Single();
            var lastQuality = datumBeforeRange.Quality;
            var lastValue = datumBeforeRange.Value;
            var nextValueTs = readEnumerator.MoveNext() ? readEnumerator.Current.Timestamp : timeEnumerator.ToExcludedUtcUtc;

            foreach (var ts in timeEnumerator)
            {
                if (ts == nextValueTs)
                {
                    lastQuality = readEnumerator.Current.Quality;
                    lastValue = readEnumerator.Current.Value;
                    nextValueTs = readEnumerator.MoveNext() ? readEnumerator.Current.Timestamp : timeEnumerator.ToExcludedUtcUtc;
                }
                yield return new Datum<T>() { Value = lastValue, Quality = lastQuality, Signal = Signal, Timestamp = ts };
            }
        }
    }
}
