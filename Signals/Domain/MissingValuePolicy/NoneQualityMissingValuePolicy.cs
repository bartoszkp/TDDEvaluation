using Domain.Infrastructure;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> GetDataAndFillMissingSamples(TimeEnumerator timeEnumerator, ISignalsDataRepository repository)
        {
            var readData = repository.GetData<T>(Signal, timeEnumerator.FromIncludedUtc, timeEnumerator.ToExcludedUtcUtc);
            var readDataDict = readData.ToDictionary(d => d.Timestamp, d => d);

            return timeEnumerator
                .Select(ts => readDataDict.ContainsKey(ts) ? readDataDict[ts] : Datum<T>.CreateNone(Signal, ts))
                .ToArray();
        }
    }
}
