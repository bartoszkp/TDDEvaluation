using Domain.Infrastructure;
using Domain.Repositories;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override IEnumerable<Type> CompatibleNativeTypes
        {
            get
            {
                return new[] { ShadowSignal.DataType.GetNativeType() };
            }
        }

        public override IEnumerable<Granularity> CompatibleGranularities
        {
            get
            {
                return new[] { ShadowSignal.Granularity };
            }
        }

        public override IEnumerable<Datum<T>> GetDataAndFillMissingSamples(
            TimeEnumerator timeEnumerator, 
            ISignalsDataRepository repository)
        {
            var readData = repository.GetData<T>(Signal, timeEnumerator.FromIncludedUtc, timeEnumerator.ToExcludedUtcUtc);
            var shadowData = repository.GetData<T>(ShadowSignal, timeEnumerator.FromIncludedUtc, timeEnumerator.ToExcludedUtcUtc);
            var readDataDict = readData.ToDictionary(d => d.Timestamp, d => d);
            var shadowDataDict = shadowData.ToDictionary(d => d.Timestamp, d => d);

            return timeEnumerator
                .Select(ts => readDataDict.ContainsKey(ts) 
                     ? readDataDict[ts]
                     : shadowDataDict.ContainsKey(ts)
                     ? shadowDataDict[ts]
                     : Datum<T>.CreateNone(Signal, ts))
                .ToArray();
        }
    }
}
