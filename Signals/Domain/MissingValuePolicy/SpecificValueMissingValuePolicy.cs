using Domain.Infrastructure;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public class SpecificValueMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual T Value { get; set; }

        public virtual Quality Quality { get; set; }

        public override IEnumerable<Datum<T>> GetDataAndFillMissingSamples(TimeEnumerator timeEnumerator, ISignalsDataRepository repository)
        {
            var readData = repository.GetData<T>(Signal, timeEnumerator.FromIncludedUtc, timeEnumerator.ToExcludedUtcUtc);
            var readDataDict = readData.ToDictionary(d => d.Timestamp, d => d);

            return timeEnumerator
                .Select(ts => readDataDict.ContainsKey(ts) ? readDataDict[ts] : new Datum<T>() { Value = Value, Quality = Quality, Signal = Signal, Timestamp = ts })
                .ToArray();
        }
    }
}
