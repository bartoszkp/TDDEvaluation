using Domain;

namespace DataAccess.Repositories
{
    public class SignalRepository : RepositoryBase, Domain.Repositories.ISignalRepository
    {
        public SignalRepository(ISessionProvider sessionProvider)
            : base(sessionProvider)
        {
        }

        public Signal Add(Signal signal)
        {
            Session.SaveOrUpdate(signal);

            return signal;
        }

        public Signal Get(Path path)
        {
            var pathString = path.ToString();

            return Session
                .QueryOver<Signal>()
                .Where(s => s.Path.ToString() == pathString)
                .SingleOrDefault();
        }

        public void Remove(Path path)
        {
            var signal = Get(path);

            Session.Delete(signal);
        }
    }
}
