using System;

namespace Domain.Exceptions
{

    [Serializable]
    public class TimestampIncorrectException : Exception
    {
        public TimestampIncorrectException(DateTime timestamp, Granularity granularity)
            : base($"Timestamp '{timestamp}' for Granularity.{granularity} is incorrect.")
        { }
    }
}
