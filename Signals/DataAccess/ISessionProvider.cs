using NHibernate;

namespace DataAccess
{
    public interface ISessionProvider
    {
        ISession Session { get; }
    }
}
