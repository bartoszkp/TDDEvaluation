using System;
using System.Collections.Generic;
using Domain.MissingValuePolicy;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);
        Signal GetById(int signalId);
        Signal Get(Path path);
        void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domainPolicyBase);
        MissingValuePolicyBase GetMissingValuePolicy(Signal signalDomain);

        void SetData(Signal signal, IEnumerable<Datum<double>> data);
    }
}
