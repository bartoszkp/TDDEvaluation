using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Infrastructure;

namespace DataAccess.Repositories
{
    [UnityRegister]
    public class SignalsRepository : RepositoryBase, Domain.Repositories.ISignalsRepository
    {
        public SignalsRepository(ISessionProvider sessionProvider)
            : base(sessionProvider)
        {
        }

        public Signal Add(Signal signal)
        {
            Session.SaveOrUpdate(signal);
            return signal;
        }

        public void Delete(Signal signal)
        {
            Session.Delete(signal);
        }

        public Signal Get(Path path)
        {
            return Session
                .QueryOver<Signal>()
                .Where(s => s.Path == path)
                .SingleOrDefault();
        }

        public Signal Get(int signalId)
        {
            return Session
                .QueryOver<Signal>()
                .Where(s => s.Id == signalId)
                .SingleOrDefault();
        }

        public IEnumerable<Signal> GetAllWithPathPrefix(Path path)
        {
            return Session
                .QueryOver<Signal>()
                .List<Signal>()
                .Where(s => s.Path.Components.Take(path.Length).SequenceEqual(path.Components))
                .ToArray();
        }

        public void Remove(Path path)
        {
            var signal = Get(path);

            Session.Delete(signal);
        }
    }
}
