using System;
using System.Threading;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignalsIntegrationTests.Infrastructure
{
    public static class ServerManagerGuard
    {
        private static WebService.ISignalsWebService instance = null;
        private static IUnityContainer unityContainer = null;
        private static int counter = 0;
        private static object @lock = new object();

        public static WebService.ISignalsWebService Client
        {
            get
            {
                return instance;
            }
        }

        private static void Initialize(TestContext testContext)
        {
            unityContainer = new UnityContainer();
            (new Bootstrapper.Bootstrapper()).Run(unityContainer);

            (new DatabaseMaintenance.DatabaseMaintenance(unityContainer.Resolve<DataAccess.ISessionProvider>()))
                .RebuildDatabase();

            instance = Intercept.ThroughProxy<WebService.ISignalsWebService>(
                unityContainer.Resolve<WebService.ISignalsWebService>(),
                new InterfaceInterceptor(),
                new[] { new WebServiceEmulationProxy(testContext) });
        }

        public static IDisposable Attach(TestContext testContext)
        {
            if (instance == null)
            {
                lock (@lock)
                {
                    if (instance == null)
                    {
                        Initialize(testContext);
                    }
                }
            }

            return new ReferenceCounter();
        }

        private static void Detach()
        {
            if (counter == 0)
            {
                unityContainer.Dispose();
                instance = null;
            }
        }

        private class ReferenceCounter : IDisposable
        {
            public ReferenceCounter()
            {
                Interlocked.Increment(ref ServerManagerGuard.counter);
            }

            ~ReferenceCounter()
            {
                Dispose();
            }

            public void Dispose()
            {
                Interlocked.Decrement(ref ServerManagerGuard.counter);
                ServerManagerGuard.Detach();
            }
        }
    }
}
