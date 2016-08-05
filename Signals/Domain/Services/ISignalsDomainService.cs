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
        void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase domainMissingValuePolicy);
        MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal);
        void SetData<T>(IEnumerable<Datum<T>> domainData);
        IEnumerable<Domain.Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);


    }
}
