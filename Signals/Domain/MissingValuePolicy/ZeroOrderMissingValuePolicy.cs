using Domain.Infrastructure;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public class ZeroOrderMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> GetDataAndFillMissingSamples(TimeEnumerator timeEnumerator, ISignalsDataRepository repository)
        {
            var originalData = repository.GetData<T>(Signal, timeEnumerator.FromIncludedUtc, timeEnumerator.ToExcludedUtcUtc);
            var olderData = repository.GetDataOlderThan<T>(Signal, timeEnumerator.FromIncludedUtc, maxSampleCount: 1);
            var newerData = repository.GetDataNewerThan<T>(Signal, timeEnumerator.ToExcludedUtcUtc, maxSampleCount: 1);
            return FillMissingData(timeEnumerator, originalData, olderData, newerData);
        }

        private IEnumerable<Datum<T>> FillMissingData(
           TimeEnumerator timeEnumerator,
           IEnumerable<Datum<T>> readData,
           IEnumerable<Datum<T>> additionalOlderData,
           IEnumerable<Datum<T>> additionalNewerData)
        {
            var readEnumerator = readData.GetEnumerator();
            var datumBeforeRange = additionalOlderData.DefaultIfEmpty(new Datum<T> { Quality = Quality.None }).Single();
            var lastQuality = datumBeforeRange.Quality;
            var lastValue = datumBeforeRange.Value;
            var nextValueTs = readEnumerator.MoveNext() ? readEnumerator.Current.Timestamp : timeEnumerator.ToExcludedUtcUtc;

            foreach (var ts in timeEnumerator)
            {
                if (ts == nextValueTs && readEnumerator.Current != null)
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
