using NHibernate;
using NHibernate.Cfg;

namespace DataAccess
{
    public interface ISessionProvider
    {
        ISession Session { get; }

        Configuration NHibernateConfiguration { get; }
    }
}
