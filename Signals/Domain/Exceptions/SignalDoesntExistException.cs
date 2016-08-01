using System;
using System.Runtime.Serialization;

namespace Domain.Services.Implementation
{
    [Serializable]
    internal class SignalDoesntExistException : Exception
    {
        public SignalDoesntExistException() : base("Couldnt Get/Set signal policy on non-existing signal.")
        {
        }
    }
}