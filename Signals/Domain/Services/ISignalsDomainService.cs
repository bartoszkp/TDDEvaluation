using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        void SetData(int signalId, IEnumerable<Datum<bool>> data);
        void SetData(int signalId, IEnumerable<Datum<decimal>> data);
        void SetData(int signalId, IEnumerable<Datum<double>> data);
        void SetData(int signalId, IEnumerable<Datum<int>> data);
        void SetData(int signalId, IEnumerable<Datum<string>> data);

        IEnumerable<Domain.Datum<T>> GetData<T>(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        Signal GetByPath(Path path);

        void SetMissingValuePolicy(Signal exampleSignal, Domain.MissingValuePolicy.MissingValuePolicyBase policy);
        MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(Signal signal);
    }
}
