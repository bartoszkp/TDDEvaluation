using System;
using NHibernate;

namespace DataAccess
{
    public abstract class UnitOfWorkBase : IDisposable
    {
        public event EventHandler Closed;

        public bool ReadOnly { get; protected set; }

        public abstract ISession Session { get; }

        public abstract void Dispose();

        protected ITransaction Transaction { get; set; }

        public void Commit()
        {
            if (ReadOnly)
            {
                throw new InvalidOperationException("Cannot commit read-only transaction.");
            }

            Transaction.Commit();
        }

        public void Flush()
        {
            if (ReadOnly)
            {
                throw new InvalidOperationException("Cannot flush read-only session.");
            }

            Session.Flush();
        }

        public void Clear()
        {
            Session.Clear();
        }

        protected void RaiseClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}
