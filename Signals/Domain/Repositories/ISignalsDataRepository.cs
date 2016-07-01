using System;
using System.Collections.Generic;

namespace Domain.Repositories
{
    public interface ISignalsDataRepository
    {
        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        void SetData<T>(IEnumerable<Datum<T>> data);
    }
}
