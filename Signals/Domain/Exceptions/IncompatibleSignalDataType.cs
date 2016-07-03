using System;

namespace Domain.Exceptions
{
    public class IncompatibleSignalDataType : Exception
    {
        public IncompatibleSignalDataType()
            : base("Cannot set a missing value policy for a signal with incompatible data type")
        {
        }
    }
}
