using System;

namespace Domain.Exceptions
{
    public class InvalidTimeStampException : Exception
    {
        public InvalidTimeStampException()
            : base("Invalid TimeStamp")
        {
        }

    }

}
