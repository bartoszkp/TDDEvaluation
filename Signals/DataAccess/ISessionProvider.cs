using NHibernate;
using NHibernate.Cfg;

namespace DataAccess
{
    public interface ISessionProvider
    {
        bool IsSessionOpened { get; }

        ISession Session { get; }

        Configuration NHibernateConfiguration { get; }
    }
}
