using Domain.MissingValuePolicy;
using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal GetByPath(Path signalPath);

        MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(int signalId);

        void SetMissingValuePolicy(int signalId, MissingValuePolicyBase policy);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetData<T>(IEnumerable<Datum<T>> data,Signal signal);

        PathEntry GetPathEntry(Path path);
        void Delete(int signalId);
    }
}
