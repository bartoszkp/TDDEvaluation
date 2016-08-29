using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal GetById(int signalId);

        Signal Add(Signal newSignal);
        Signal Get(Path pathDto);

        void SetData<T>(IEnumerable<Datum<T>> data);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);
        void SetMissingValuePolicy(int signalId, Domain.MissingValuePolicy.MissingValuePolicyBase domianPolicy);
        MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(int signalId);
        PathEntry GetPathEntry(Path path);
        void Delete(int signalId);
    }
}
