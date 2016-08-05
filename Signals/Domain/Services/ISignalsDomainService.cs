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

        Domain.MissingValuePolicy.MissingValuePolicyBase GetMissingValuePolicy(int signalId);

        IEnumerable<Datum<double>> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetData(int signalId, IEnumerable<Datum<double>> enumerable);

        void SetMissingValuePolicy(int signalId, SpecificValueMissingValuePolicy<double> specificValueMissingValuePolicy);
    }
}
