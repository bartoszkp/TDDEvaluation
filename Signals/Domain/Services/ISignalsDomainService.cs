using System;
using System.Collections.Generic;
using Domain.MissingValuePolicy;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal GetByPath(Path signalPath);

        Signal GetById(int signalId);

        Signal Add(Signal newSignal);

        void SetMissingValuePolicy<T>(int signalId, MissingValuePolicyBase policy);

        MissingValuePolicyBase GetMissingValuePolicy(int signalId);

        void SetData<T>(int signalId, IEnumerable<Datum<T>> data);

        IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        Type GetDataTypeById(int signalId);

        PathEntry GetPathEntry(Path path);

        void Delete(int signalId);

        IEnumerable<Datum<T>> GetCoarseData<T>(int signalId, Granularity granularity, DateTime fromIncludedUtc, DateTime toExcludedUtc);
    }
}
