using System;
using System.Collections.Generic;
using Domain.MissingValuePolicy;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal Get(Path newPath);

        void SetMissingValuePolicy(Signal signal, MissingValuePolicyBase mvpDomain);

        MissingValuePolicyBase GetMissingValuePolicy(int signalId);

        IEnumerable<Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetData<T>(IEnumerable<Datum<T>> domianModel);
    }
}
