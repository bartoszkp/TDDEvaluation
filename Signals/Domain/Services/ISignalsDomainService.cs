using System;
using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISignalsDomainService
    {
        Signal Get(Path path);

        Signal Add(Signal signal);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded);

        void SetData<T>(Signal signal, DateTime fromIncluded, IEnumerable<Datum<T>> data);
    }
}
