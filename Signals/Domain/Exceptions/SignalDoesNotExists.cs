using System;

namespace Domain.Exceptions
{
    public class SignalDoesNotExists : Exception
    {
        public SignalDoesNotExists()
            : base("You cannot delete a signal that does not exists")
        {
        }
    }
}