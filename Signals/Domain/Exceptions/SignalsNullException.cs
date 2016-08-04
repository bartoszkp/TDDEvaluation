using System;

namespace Domain.Exceptions
{
    public class SignalIsNullException : Exception
    {
        public SignalIsNullException()
            : base("When we use missing value policy, siganl can't be null")
        {
        }
    }
}
