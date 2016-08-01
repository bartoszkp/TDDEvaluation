using NHibernate;

namespace DataAccess
{
    public abstract class RepositoryBase
    {
        private readonly ISessionProvider sessionProvider;

        public RepositoryBase(ISessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

        protected ISession Session {  get { return this.sessionProvider.Session; } }
    }
}
