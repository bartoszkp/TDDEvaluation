using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Add(Signal newSignal);

        Signal GetById(int signalId);

        Signal Get(Path path);

        void SetData<T>(IEnumerable<Datum<T>> domain_data);
    }
}
