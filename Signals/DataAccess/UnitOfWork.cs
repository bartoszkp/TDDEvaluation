using System;
using System.Data;
using NHibernate;

namespace DataAccess
{
    public class UnitOfWork : UnitOfWorkBase
    {
        private ISession session;

        public override ISession Session
        {
            get { return session; }
        }

        public UnitOfWork(ISession session, bool readOnly)
        {
            this.session = session;
            ReadOnly = readOnly;

            if (readOnly)
            {
                session.FlushMode = FlushMode.Never;
            }
            else
            {
                session.FlushMode = FlushMode.Commit;
            }

            Transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
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
                    session.Close();
                    RaiseClosed();
                }
            }
        }
    }
}