using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalDomainService
    {
        Signal Get(Path path);

        Signal Add(Signal signal);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded);

        void SetData<T>(Signal signal, IEnumerable<Datum<T>> data);
    }
}
