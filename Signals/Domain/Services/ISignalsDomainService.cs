using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal Get(Path path);

        void SetData<T>(IEnumerable<Datum<T>> domain_data);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded);

        IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc, int periodSize);

        void SetMissingValuePolicy(Signal signal, MissingValuePolicy.MissingValuePolicyBase missingValuePolicy);

        MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal);

        PathEntry GetPathEntry(Domain.Path path);

        void Delete<T>(int signalId);
    }
}
