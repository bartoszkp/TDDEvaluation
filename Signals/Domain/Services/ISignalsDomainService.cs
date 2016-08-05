using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal GetByPath(Path path);

        Domain.MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(int signalId);
    }
}
