using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal GetByPath(Path signalPath);

        void SetData<T>(Signal signalId, IEnumerable<Datum<T>> data);

        IEnumerable<Domain.Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetMissingValuePolicy<T>(Signal signal, Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<T> policy);
    }
}
