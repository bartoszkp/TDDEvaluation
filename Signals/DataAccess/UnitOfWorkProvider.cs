using System;
using System.Threading;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;

namespace DataAccess
{
    public class UnitOfWorkProvider : IUnitOfWorkProvider, ISessionProvider, IDisposable
    {
        public Configuration NHibernateConfiguration { get; private set; }

        private readonly ISessionFactory sessionFactory;
        private ThreadLocal<ISession> sessionLocal = new ThreadLocal<ISession>(() => null);

        public UnitOfWorkProvider()
        {
            this.sessionFactory = CreateSessionFactory();
        }

        private ISessionFactory CreateSessionFactory()
        {
            var mappings = AutoMap
                .AssemblyOf<Domain.Signal>(new SignalAutoMappingConfiguration())
                .UseOverridesFromAssemblyOf<UnitOfWorkProvider>();
                
            this.NHibernateConfiguration = Fluently
                .Configure()
                .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2012.ConnectionString(
                    b => b.FromConnectionStringWithKey("signals")))
                .Mappings(m => m.AutoMappings.Add(mappings))
                .BuildConfiguration();

            return this.NHibernateConfiguration.BuildSessionFactory();
        }

        public ISession Session
        {
            get { return sessionLocal.Value; }
        }

        private UnitOfWork OpenUnitOfWork(bool readOnly)
        {
            if (sessionLocal.Value == null)
            {
                sessionLocal.Value = sessionFactory.OpenSession();
                var unitOfWork = new UnitOfWork(sessionLocal.Value, readOnly);
                unitOfWork.Closed += new EventHandler(OnUnitOfWorkClosed);
                return unitOfWork;
            }
            else
            {
                throw new InvalidOperationException("Cannot open new unit of work when previous unit of work for this thread has not been closed.");
            }
        }

        public UnitOfWork OpenReadOnlyUnitOfWork()
        {
            return OpenUnitOfWork(true);
        }

        public UnitOfWork OpenUnitOfWork()
        {
            return OpenUnitOfWork(false);
        }

        private void OnUnitOfWorkClosed(object sender, EventArgs e)
        {
            UnitOfWork unitOfWork = (UnitOfWork)sender;
            unitOfWork.Closed -= OnUnitOfWorkClosed;

            sessionLocal.Value = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                sessionLocal.Dispose();
                sessionFactory.Dispose();
            }
        }
    }
}
