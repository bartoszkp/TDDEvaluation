using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal GetById(int signalId);

        Signal Add(Signal newSignal);
    }
}
