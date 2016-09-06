using Domain.Exceptions;

namespace Domain.MissingValuePolicy
{
    public class ShadowMissingValuePolicy<T> : MissingValuePolicy<T>
    {
        public virtual Signal ShadowSignal { get; set; }

        public override void CheckGranularityAndDataType(Signal signal)
        {
            if (ShadowSignal.DataType != signal.DataType)
                throw new IncompatibleDataTypes();
            else if (ShadowSignal.Granularity != signal.Granularity)
                throw new IncompatibleGranularities();
        }
    }
}
