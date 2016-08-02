using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal GetById(int signalId);

        Signal Add(Signal newSignal);

        Signal Get(Path pathDto);

        void SetData(IEnumerable<Datum<double>> newDomainDatum);

        IEnumerable<Datum<double>> GetData(Signal getSignal, DateTime fromIncludedUtc, DateTime toExcludedUtc);
    }
}
