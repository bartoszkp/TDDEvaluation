using System;

namespace Domain.Exceptions
{
    public class GettingDataOfNotExistingSignal : Exception
    {
        public GettingDataOfNotExistingSignal()
            : base("You cannot get data of signal that doesn't exists")
        {
        }
    }
}