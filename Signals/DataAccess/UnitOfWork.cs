using System;
using System.Data;
using NHibernate;

namespace DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private ISession session;
        private ITransaction transaction;

        public event EventHandler Closed;

        public ISession Session
        {
            get { return session; }
        }

        public bool ReadOnly { get; private set; }

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

            transaction = session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void Commit()
        {
            if (ReadOnly)
            {
                throw new InvalidOperationException("Cannot commit read-only transaction.");
            }

            transaction.Commit();
        }

        public void Flush()
        {
            if (ReadOnly)
            {
                throw new InvalidOperationException("Cannot flush read-only session.");
            }

            session.Flush();
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
                try
                {
                    if (!transaction.WasCommitted && !transaction.WasRolledBack)
                    {
                        if (transaction.IsActive)
                        {
                            transaction.Rollback();
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

        private void RaiseClosed()
        {
            if (Closed != null)
            {
                Closed(this, EventArgs.Empty);
            }
        }
    }
}