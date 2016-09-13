using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        void SetData<T>(IEnumerable<Datum<T>> data);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc);
        
        void Set<T>(Signal signal, MissingValuePolicy.MissingValuePolicyBase missingValuePolicy);

        MissingValuePolicy.MissingValuePolicyBase Get(Signal signal);

        Signal Get(Path path);

        PathEntry GetPathEntry(Path path);
        void Delete(int signalId);

        bool VerifyTimeStamp(Granularity granularity, DateTime fromIncludedUtc);
    }
}
