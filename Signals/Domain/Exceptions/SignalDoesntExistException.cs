using System;
using System.Runtime.Serialization;

namespace Domain.Services.Implementation
{
    [Serializable]
    internal class SignalDoesntExistException : Exception
    {
        public SignalDoesntExistException() : base("Given signalId doesnt exist in database.")
        {
        }
    }
}