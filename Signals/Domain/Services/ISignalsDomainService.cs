using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);
        Signal Get(Path pathDomain);
        void SetMissingValuePolicy(int signalId, Domain.MissingValuePolicy.MissingValuePolicyBase policy);
        void GetMissingValuePolicy(int signalId);
    }
}
