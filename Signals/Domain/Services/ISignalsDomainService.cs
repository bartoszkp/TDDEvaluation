using System;
using System.Collections.Generic;
using Domain.MissingValuePolicy;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        void SetData(int signalId, IEnumerable<Datum<object>> data);

        IEnumerable<Datum<object>> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domainMvp);

        MissingValuePolicyBase GetMissingValuePolicy(int signalId);

        Signal GetByPath(Path domainPath);
    }
}
