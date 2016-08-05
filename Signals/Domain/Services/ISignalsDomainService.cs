using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal GetByPath(Path path);

        IEnumerable<Datum<double>> GetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetData(int signalId, Datum<double>[] dataDomain);
    }
}
