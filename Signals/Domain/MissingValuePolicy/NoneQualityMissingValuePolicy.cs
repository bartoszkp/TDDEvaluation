using System.Collections.Generic;
using System.Linq;
using Domain.Infrastructure;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public override IEnumerable<Datum<T>> FillMissingData(
            TimeEnumerator timeEnumerator,
            IEnumerable<Datum<T>> readData,
            IEnumerable<Datum<T>> additionalOlderData,
            IEnumerable<Datum<T>> additionalNewerData)
        {
            var readDataDict = readData.ToDictionary(d => d.Timestamp, d => d);

            return timeEnumerator
                .Select(ts => readDataDict.ContainsKey(ts) ? readDataDict[ts] : Datum<T>.CreateNone(Signal, ts))
                .ToArray();
        }
    }
}
