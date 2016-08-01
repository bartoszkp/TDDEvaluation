using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal GetByPath(Path signalPath);

        Signal GetById(int signalId);

        Signal Add(Signal newSignal);
    }
}
