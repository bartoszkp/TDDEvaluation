using Domain.Infrastructure;
using System.Collections.Generic;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillMissingData(TimeEnumerator timeEnumerator, IEnumerable<Datum<T>> readData)
        {
            var readEnumerator = readData.GetEnumerator();
            var lastQuality = Quality.None;
            var lastValue = default(T);
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
