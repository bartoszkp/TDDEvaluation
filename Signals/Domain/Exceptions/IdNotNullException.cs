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
}
