using System;

namespace Domain.Exceptions
{
    public class InvalidAverageOperationException : Exception
    {
        public InvalidAverageOperationException(string dataType)
            : base("Unable to calculate average for signal of data type: " + dataType)
        {
        }
    }
}
