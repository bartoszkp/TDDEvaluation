using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        void SetData<T>(int signalId, IEnumerable<Datum<T>> data);
        
        IEnumerable<Domain.Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        Signal GetByPath(Path path);

        void SetMissingValuePolicy(Signal exampleSignal, Domain.MissingValuePolicy.MissingValuePolicyBase policy);

        MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal);

        PathEntry GetPathEntry(Path pathDomain);
        void Delete(int signalId);
        IEnumerable<Datum<T>> GetCoarseData<T>(int signalId, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc);
    }
}
