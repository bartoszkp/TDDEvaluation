using Domain.Infrastructure;
using Domain.Repositories;
using System;
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

        public override IEnumerable<Datum<T>> GetDataAndFillMissingSamples(TimeEnumerator timeEnumerator, ISignalsDataRepository repository)
        {
            throw new NotImplementedException();
        }
    }
}
