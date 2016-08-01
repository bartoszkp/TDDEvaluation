using Domain.MissingValuePolicy;
using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal Get(Path path);

        void SetData<T>(IEnumerable<Datum<T>> data);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        MissingValuePolicyBase GetMissingValuePolicy(Signal signal);

        void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase missingValuePolicy);
    }
}
