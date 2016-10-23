using System;

namespace Domain.Exceptions
{
    public class FinerGranularityException : Exception
    {
        public FinerGranularityException()
            : base("Cannot get data using finer granularity")
        {
        }
    }
}
