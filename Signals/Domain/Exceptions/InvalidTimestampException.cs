using System;

namespace Domain.Exceptions
{
    public class InvalidTimestampException : Exception
    {
        public InvalidTimestampException()
            : base("Invalid timestamp.")
        {
        }
    }
}
