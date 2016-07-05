using System.Collections.Generic;

namespace Domain.Repositories
{
    public interface ISignalsRepository
    {
        Signal Get(Path path);

        Signal Get(int signalId);

        Signal Add(Signal signal);

        void Delete(Signal signal);

        IEnumerable<Signal> GetAllWithPathPrefix(Path prefix);

        void Remove(Path path);
    }
}
