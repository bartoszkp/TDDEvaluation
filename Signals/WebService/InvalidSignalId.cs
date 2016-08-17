using System;
using System.Runtime.Serialization;

namespace WebService
{
    [Serializable]
    internal class InvalidSignalId : Exception
    {
        public InvalidSignalId()
        {
        }

        public InvalidSignalId(string message) : base(message)
        {
        }

        public InvalidSignalId(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidSignalId(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}