using DataAccess;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace DatabaseMaintenance
{
    public class DatabaseMaintenance
    {
        private readonly ISessionProvider sessionProvider;

        public DatabaseMaintenance(ISessionProvider sessionProvider)
        {
            this.sessionProvider = sessionProvider;
        }

        public void RebuildDatabase()
        {
            if (!this.sessionProvider.IsSessionOpened)
            {
                new SchemaExport(this.sessionProvider.NHibernateConfiguration)
                    .Execute(true, true, false);
            }
            else
            {
                new SchemaExport(this.sessionProvider.NHibernateConfiguration)
                    .Execute(true, true, false, this.sessionProvider.Session.Connection, null);
            }
        }
    }
}
