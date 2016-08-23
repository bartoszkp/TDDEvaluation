using System;
using System.Collections.Generic;
using Domain.MissingValuePolicy;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        void SetData<T>(int signalId, IEnumerable<Datum<T>> data);

        IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetMissingValuePolicy(int signalId, MissingValuePolicyBase domainMvp);

        MissingValuePolicyBase GetMissingValuePolicy(int signalId);

        Signal GetByPath(Path domainPath);

        PathEntry GetAllWithPathPrefix(Path prefix);

        Type GetSignalType(int signalId);
    }
}
