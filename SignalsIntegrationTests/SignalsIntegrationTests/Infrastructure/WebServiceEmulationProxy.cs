using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignalsIntegrationTests.Infrastructure
{
    public class WebServiceEmulationProxy : IInterceptionBehavior
    {
        public bool WillExecute { get { return true; } }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Enumerable.Empty<Type>();
        }

        public WebServiceEmulationProxy(TestContext testContext)
        {
            this.testContext = testContext;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var currentTestCategories = GetCurrentTestCategories();

            var task = Task.Factory.StartNew(() => getNext()(input, getNext));

            var timeout = currentTestCategories.Any(c => c == "issueCoarseData")
                ? 300000
                : 5000;

            if (!task.Wait(timeout))
            {
                TimeoutRegistry.RegisterTimeout(currentTestCategories);
                throw new TimeoutException();
            }
            else if (task.Result.Exception != null)
            {
                throw new FaultException<ExceptionDetail>(new ExceptionDetail(task.Result.Exception), new FaultReason(task.Result.Exception.Message));
            }

            return task.Result;
        }

        private string[] GetCurrentTestCategories()
        {
            var currentMethod = Type.GetType(testContext.FullyQualifiedTestClassName)
                .GetMethod(testContext.TestName);

            return currentMethod
                .GetCustomAttributes(typeof(TestCategoryAttribute), false)
                .Cast<TestCategoryAttribute>()
                .SelectMany(tca => tca.TestCategories)
                .ToArray();
        }

        private TestContext testContext;
    }
}