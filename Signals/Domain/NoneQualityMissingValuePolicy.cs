using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Infrastructure;

namespace Domain
{
    public class NoneQualityMissingValuePolicy : MissingValuePolicy
    {
        // TODO it should SOMEHOW go to Implementation
        public override IEnumerable<Datum<T>> FillMissingData<T>(TimeEnumerator timeEnumerator, IEnumerable<Datum<T>> readData)
        {
            var readDataDict = readData.ToDictionary(d => d.Timestamp, d => d);

            return timeEnumerator
                .Select(ts => readDataDict.ContainsKey(ts) ? readDataDict[ts] : Datum<T>.CreateNone(Signal, ts))
                .ToArray();
        }
    }
}
