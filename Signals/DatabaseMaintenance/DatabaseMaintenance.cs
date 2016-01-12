using DataAccess;
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
            new SchemaExport(this.sessionProvider.NHibernateConfiguration)
                .Execute(true, true, false);
        }
    }
}
