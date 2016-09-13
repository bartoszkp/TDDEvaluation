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
        void SetMissingValuePolicyBase(int signalId, MissingValuePolicyBase policy);

        IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetData<T>(int signalId, IEnumerable<Datum<T>> dataDomain);

        PathEntry GetPathEntry(Path prefixPath);

        void Delete(int signalId);

        MissingValuePolicyBase GetMissingValuePolicy(Signal signal);

        IEnumerable<Datum<T>> GetCoarseData<T>(Signal signal, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc);
    }
}
