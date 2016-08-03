using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal Get(Path path);

        void SetData(IEnumerable<Datum<double>> domain_data);

        void SetData(IEnumerable<Datum<bool>> domain_data);
    }
}
