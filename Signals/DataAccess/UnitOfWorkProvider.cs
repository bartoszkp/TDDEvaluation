using System;
using System.Threading;
using Domain.Infrastructure;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using Microsoft.Practices.Unity;
using NHibernate;
using NHibernate.Cfg;

namespace DataAccess
{
    [UnityRegister(typeof(ContainerControlledLifetimeManager))]
    public class UnitOfWorkProvider : IUnitOfWorkProvider, ISessionProvider, IDisposable
    {
        public Configuration NHibernateConfiguration { get; private set; }

        private readonly ISessionFactory sessionFactory;
        private ThreadLocal<UnitOfWorkBase> unitOfWorkLocal = new ThreadLocal<UnitOfWorkBase>(() => null);

        public bool InMemory { get; private set; }

        public UnitOfWorkBase CurrentUnitOfWork
        {
            get { return unitOfWorkLocal.Value; }
        }

        public bool IsSessionOpened
        {
            get
            {
                return unitOfWorkLocal.Value != null;
            }
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

        public UnitOfWorkProvider(IDatabaseConfigurationProvider databaseConfigurationProvider)
        {
            InMemory = databaseConfigurationProvider.UseInMemoryDatabase;

            if (InMemory)
            {
                this.sessionFactory = CreateSqliteSessionFactory();
                InMemoryUnitOfWork.Initialize(this.sessionFactory.OpenSession());
            }
            else
            {
                this.sessionFactory = CreateMsSqlSessionFactory();
            }
        }

        private ISessionFactory CreateMsSqlSessionFactory()
        {
            var mappings = CreateMappings();
                
            this.NHibernateConfiguration = Fluently
                .Configure()
                .Database(FluentNHibernate.Cfg.Db.MsSqlConfiguration.MsSql2012.ConnectionString(
                    b => b.FromConnectionStringWithKey("signals")))
                .Mappings(m => m.AutoMappings.Add(mappings))
                .BuildConfiguration();

            return this.NHibernateConfiguration.BuildSessionFactory();
        }

        private ISessionFactory CreateSqliteSessionFactory()
        {
            var mappings = CreateMappings();

            this.NHibernateConfiguration = Fluently
                .Configure()
                .Database(FluentNHibernate.Cfg.Db.SQLiteConfiguration.Standard.InMemory())
                .Mappings(m => m.AutoMappings.Add(mappings))
                .BuildConfiguration();

            return this.NHibernateConfiguration.BuildSessionFactory();
        }

        private AutoPersistenceModel CreateMappings()
        {
            return AutoMap
                .Assemblies(new SignalsAutoMappingConfiguration(), typeof(Domain.Signal).Assembly, typeof(UnitOfWorkProvider).Assembly)
                .IgnoreBase(typeof(Domain.Datum<>))
                .IncludeBase(typeof(Domain.MissingValuePolicy.MissingValuePolicyBase))
                .IgnoreBase(typeof(Domain.MissingValuePolicy.MissingValuePolicy<>))
                .UseOverridesFromAssemblyOf<UnitOfWorkProvider>()
                .Conventions.AddFromAssemblyOf<UnitOfWorkProvider>();
        }

        private UnitOfWorkBase OpenUnitOfWork(bool readOnly)
        {
            if (unitOfWorkLocal.Value == null)
            {
                unitOfWorkLocal.Value = BuildUpUnitOfWork(readOnly);
                return unitOfWorkLocal.Value;
            }
            else
            {
                throw new InvalidOperationException("Cannot open new unit of work when previous unit of work for this thread has not been closed.");
            }
        }

        private UnitOfWorkBase BuildUpUnitOfWork(bool readOnly)
        {
            UnitOfWorkBase result;
            if (InMemory)
            {
                result = new InMemoryUnitOfWork();
            }
            else
            {
                var session = sessionFactory.OpenSession();
                result = new UnitOfWork(session, readOnly);
            }

            result.Closed += new EventHandler(OnUnitOfWorkClosed);

            return result;
        }

        public UnitOfWorkBase OpenReadOnlyUnitOfWork()
        {
            return OpenUnitOfWork(true);
        }

        public UnitOfWorkBase OpenUnitOfWork()
        {
            return OpenUnitOfWork(false);
        }

        private void OnUnitOfWorkClosed(object sender, EventArgs e)
        {
            UnitOfWorkBase unitOfWork = (UnitOfWorkBase)sender;
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
