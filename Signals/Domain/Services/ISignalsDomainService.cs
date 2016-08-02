using System;
using System.Collections.Generic;
using Domain.MissingValuePolicy;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);
        Signal Get(Path pathDomain);
        MissingValuePolicyBase GetMissingValuePolicyBase(int signalId);
        void SetMissingValuePolicyBase(int signalId, MissingValuePolicyBase policy);
        IEnumerable<Datum<double>> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);
    }
}
