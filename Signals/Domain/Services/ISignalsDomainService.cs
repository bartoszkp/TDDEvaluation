using System;
using System.Collections.Generic;
using Domain.MissingValuePolicy;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal GetByPath(Path signalPath);

        Signal GetById(int signalId);

        Signal Add(Signal newSignal);

        void SetMissingValuePolicy(int signalId, MissingValuePolicyBase policy);

        MissingValuePolicyBase GetMissingValuePolicy(int signalId);
    }
}
