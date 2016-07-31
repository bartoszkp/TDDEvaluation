using System;
using System.Data;
using NHibernate;

namespace DataAccess
{
    public class InMemoryUnitOfWork : UnitOfWorkBase
    {
        private static ISession GlobalSession;

        public static void Initialize(ISession session)
        {
            InMemoryUnitOfWork.GlobalSession = session;
        }

        public override ISession Session
        {
            get { return GlobalSession; }
        }

        public InMemoryUnitOfWork()
        {
            ReadOnly = false;
            Transaction = GlobalSession.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    if (!Transaction.WasCommitted && !Transaction.WasRolledBack)
                    {
                        if (Transaction.IsActive)
                        {
                            Transaction.Rollback();
                        }
                    }
                }
                catch (TransactionException)
                {
                    throw;
                }
                finally
                {
                    RaiseClosed();
                }
            }
        }
    }
}