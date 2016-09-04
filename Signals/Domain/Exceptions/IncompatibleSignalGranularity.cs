using System;

namespace Domain.Exceptions
{
    public class IncompatibleSignalGranularity : Exception
    {
        public IncompatibleSignalGranularity()
            : base("Cannot set a missing value policy for a signal with incompatible granularity")
        {
        }
    }
}
