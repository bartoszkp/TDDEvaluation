using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        void SetData<T>(Signal signal, IEnumerable<Datum<T>> data);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetMissingValuePolicy(Signal signal, MissingValuePolicy.MissingValuePolicyBase policy);

        MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal);
    }
}
