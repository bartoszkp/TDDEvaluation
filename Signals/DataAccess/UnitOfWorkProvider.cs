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
        private ThreadLocal<UnitOfWork> unitOfWorkLocal = new ThreadLocal<UnitOfWork>(() => null);

        public UnitOfWork CurrentUnitOfWork
        {
            get { return unitOfWorkLocal.Value; }
        }

        public ISession Session
        {
            get
            {
                if (unitOfWorkLocal.Value == null)
                {
                    throw new InvalidOperationException("No transaction opened");
                }

                return unitOfWorkLocal.Value.Session;
            }
        }

        public UnitOfWorkProvider()
        {
            this.sessionFactory = CreateSessionFactory();
        }

        private ISessionFactory CreateSessionFactory()
        {
            var mappings = AutoMap
                .Assemblies(new SignalsAutoMappingConfiguration(), typeof(Domain.Signal).Assembly, typeof(UnitOfWorkProvider).Assembly)
                .IgnoreBase(typeof(Domain.Datum<>))
                .IncludeBase(typeof(Domain.MissingValuePolicy.MissingValuePolicyBase))
                .IgnoreBase(typeof(Domain.MissingValuePolicy.MissingValuePolicy<>))
                .UseOverridesFromAssemblyOf<UnitOfWorkProvider>()
                .Conventions.AddFromAssemblyOf<UnitOfWorkProvider>();
                
            this.NHibernateConfiguration = Fluently
                .Configure()
                .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2012.ConnectionString(
                    b => b.FromConnectionStringWithKey("signals")))
                .Mappings(m => m.AutoMappings.Add(mappings))
                .BuildConfiguration();

            return this.NHibernateConfiguration.BuildSessionFactory();
        }

        private UnitOfWork OpenUnitOfWork(bool readOnly)
        {
            if (unitOfWorkLocal.Value == null)
            {
                var session = sessionFactory.OpenSession();
                var unitOfWork = new UnitOfWork(session, readOnly);
                unitOfWorkLocal.Value = unitOfWork;
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

            unitOfWorkLocal.Value = null;
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
                unitOfWorkLocal.Dispose();
                sessionFactory.Dispose();
            }
        }
    }
}
