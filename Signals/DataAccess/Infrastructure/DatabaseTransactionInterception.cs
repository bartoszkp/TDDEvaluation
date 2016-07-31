using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace DataAccess.Infrastructure
{
    public class DatabaseTransactionInterception : IInterceptionBehavior
    {
        public bool WillExecute {  get { return true; } }

        public DatabaseTransactionInterception(IUnitOfWorkProvider unitOfWorkProvider)
        {
            this.unitOfWorkProvider = unitOfWorkProvider;
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Enumerable.Empty<Type>();
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            IMethodReturn result = null;

            using (var uow = this.unitOfWorkProvider.OpenUnitOfWork())
            {
                result = getNext()(input, getNext);

                if (result.Exception == null)
                {
                    uow.Commit();
                }

                uow.Clear();
            }

            return result;
        }

        private IUnitOfWorkProvider unitOfWorkProvider;
    }
}
