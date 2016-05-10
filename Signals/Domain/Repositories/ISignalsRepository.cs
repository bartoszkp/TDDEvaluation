using System;
using System.Collections.Generic;

namespace Domain.Repositories
{
    public interface ISignalsRepository
    {
        Signal Get(Path path);

        Signal Get(int signalId);

        Signal Add(Signal signal);

        IEnumerable<Signal> GetAllWithPathPrefix(Path prefix);

        void Remove(Path path);

        void SetData<T>(IEnumerable<Datum<T>> data);

        IEnumerable<Datum<T>> GetData<T>(Signal signal, DateTime fromIncluded, DateTime toExcluded);

        void Add(MissingValuePolicy.MissingValuePolicy missingValuePolicy);
    }
}
