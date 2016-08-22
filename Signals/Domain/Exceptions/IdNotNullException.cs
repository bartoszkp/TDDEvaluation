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

    public class TimestampHaveWrongFormatException: Exception
    {
        public TimestampHaveWrongFormatException()
            :base("Timestamp have wrong timestamp format")
        {

        }
    }
}
