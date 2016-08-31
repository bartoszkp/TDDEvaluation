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

        void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domainPolicyBase);

        MissingValuePolicyBase GetMissingValuePolicy(Signal signalDomain);

        void Delete(Signal signal);

        void SetData<T>(Signal signal, IEnumerable<Datum<T>> data);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUTC, DateTime toExcludedUTC);

        PathEntry GetPathEntry(Path path);

        IEnumerable<Datum<T>> GetDataOlderThan<T>(Signal signal, DateTime excludedUtc, int maxSampleCount);

        IEnumerable<Datum<T>> GetDataNewerThan<T>(Signal signal, DateTime excludedUtc, int maxSampleCount);

    }
}
