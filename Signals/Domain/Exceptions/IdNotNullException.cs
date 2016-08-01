using System;

namespace Domain.Exceptions
{
    public class IdNotNullException : Exception
    {
        public IdNotNullException()
            : base("When adding new signal, Id must be null")
        {
        }
    }
}
