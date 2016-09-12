using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);
        Signal GetById(int signalId);
        Signal Get(Path pathDomain);
        void SetMissingValuePolicy(int signalId, Domain.MissingValuePolicy.MissingValuePolicyBase policy);
        Domain.MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(int signalId);
        void SetData<T>(Signal signal, IEnumerable<Datum<T>> dataDomain);
        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);
        IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc);
        PathEntry GetByPrefixPath(Path path);

        void Delete(int signalId);
    }
}
