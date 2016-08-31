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

        MissingValuePolicyBase GetMissingValuePolicy(Domain.Signal signal);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetData<T>(Signal signal, IEnumerable<Datum<T>> datum);

        PathEntry GetPathEntry(Path pathDto);

        void Delete(int signalId);
    }
}
