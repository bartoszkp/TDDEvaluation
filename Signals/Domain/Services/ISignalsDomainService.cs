using System;
using System.Collections.Generic;
using Domain.MissingValuePolicy;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal GetByPath(Path path);

        IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetData<T>(int signalId, IEnumerable<Datum<T>> enumerable);

        MissingValuePolicyBase GetMissingValuePolicy(int signalId);

        void SetMissingValuePolicy(int signalId, MissingValuePolicyBase missingValuePolicyBase);

        PathEntry GetPathEntry(Path path);
    }
}
