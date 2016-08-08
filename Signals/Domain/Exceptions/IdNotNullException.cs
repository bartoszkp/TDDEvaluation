using System;

namespace Domain.Exceptions
{
    public class IdNotNullException : Exception
    {
        public IdNotNullException()
            : base("When adding a new signal, Id must be null")
        {
        }
    }

    public class InvalidPathArgument : Exception
    {
        public InvalidPathArgument()
            : base("Cannot read Signal with current path. Typed Path probably doesnt exists.")
        {
        }
    }

    public class InvalidSignalId : Exception
    {
        public InvalidSignalId()
            : base("Cannot read Signal with current id. Signal with typed Id probably doesnt exists.")
        {
        }
    }

    public class InvalidValueType : Exception
    {
        public InvalidValueType()
            : base("Cannot SET/GET datum. Probably typed datum type is not supported. Supported types are: int32, decimal, double, string.")
        {
        }
    }

    public class UnsupportedTypeForMVP : Exception
    {
        public UnsupportedTypeForMVP()
            :base("Trying to set a default MVPolicy for an unsupported type. Supported types are: int32, decimal, double, string.")
        {

        }
    }

}
