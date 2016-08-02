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
}
