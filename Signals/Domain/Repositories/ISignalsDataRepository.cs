using System;
using System.Collections.Generic;

namespace Domain.Repositories
{
    public interface ISignalsDataRepository
    {
        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc);

        IEnumerable<Datum<T>> GetDataOlderThan<T>(Signal signal, DateTime excludedUtc, int maxSampleCount);

        void SetData<T>(IEnumerable<Datum<T>> data);
    }
}
