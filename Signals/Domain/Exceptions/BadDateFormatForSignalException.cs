using System;

namespace Domain.Exceptions
{
    public class BadDateFormatForSignalException : Exception
    {
        public BadDateFormatForSignalException()
            : base("When using Date Time Objects, date should have a correct format!")
        {
        }
    }
}
