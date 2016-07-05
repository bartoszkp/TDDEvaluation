using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Get(Path path);

        Signal Get(int signalId);

        Signal Add(Signal signal);

        void Delete(int signalId);

        PathEntry GetPathEntry(Path path);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetData<T>(Signal signal, IEnumerable<Datum<T>> data);

        MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal);

        void SetMissingValuePolicy(Signal signal, MissingValuePolicy.MissingValuePolicyBase missingValuePolicy);
    }
}
