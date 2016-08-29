using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal Get(Path pathDomain);

        void SetMissingValuePolicy(int signalId, MissingValuePolicy.MissingValuePolicyBase policy);

        void SetMissingValuePolicy(MissingValuePolicy.MissingValuePolicyBase policy);

        MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(int signalID);

        void SetData<T>(IEnumerable<Datum<T>> data);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        IEnumerable<Datum<T>> GetDataOlderThan<T>(Signal signal, DateTime excludedUtc, int maxSampleCount);

        PathEntry GetPathEntry(Path path);
    }
}