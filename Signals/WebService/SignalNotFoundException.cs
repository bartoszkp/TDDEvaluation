using System;

namespace WebService
{
    public class SignalNotFoundException : ApplicationException
    {
        public SignalNotFoundException(int signalId)
            : base($"Signal with Id '{signalId}' has not been found.")
        {
        }
    }
}
