using System;
using System.Runtime.Serialization;

namespace Domain.Services.Implementation
{
    [Serializable]
    internal class PathDoesntExistException : Exception
    {
        public PathDoesntExistException() : base("Given path doesnt exist in database.")
        {
        }
    }
}