using Domain.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Domain.MissingValuePolicy
{
    public class NoneQualityMissingValuePolicy : MissingValuePolicy
    {
        protected override IEnumerable<DatumBase> FillMissingData(TimeEnumerator timeEnumerator, IEnumerable<DatumBase> readData)
        {
            var nativeDataType = this.Signal.DataType.GetNativeType();

            var readDataDict = readData.ToDictionary(d => d.Timestamp, d => d);

            return timeEnumerator
                .Select(ts => readDataDict.ContainsKey(ts) ? readDataDict[ts] : DatumBase.CreateNone(nativeDataType, Signal, ts))
                .ToArray();
        }
    }
}
