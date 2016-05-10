using Domain.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy : MissingValuePolicy
    {
        public override IEnumerable<Datum<T>> FillMissingData<T>(TimeEnumerator timeEnumerator, IEnumerable<Datum<T>> readData)
        {
            var readDataDict = readData.ToDictionary(d => d.Timestamp, d => d);

            return timeEnumerator
                .Select(ts => readDataDict.ContainsKey(ts) ? readDataDict[ts] : Datum<T>.CreateNone(Signal, ts))
                .ToArray();
        }
    }
}
