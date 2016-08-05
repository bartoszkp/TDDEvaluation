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

        void SetMissingValuePolicy(int signalId, MissingValuePolicyBase mvpDomain);

        MissingValuePolicyBase GetMissingValuePolicy(int signalId);

        void SetData(int signalId, IEnumerable<Datum<double>> dataDomain);
    }
}
