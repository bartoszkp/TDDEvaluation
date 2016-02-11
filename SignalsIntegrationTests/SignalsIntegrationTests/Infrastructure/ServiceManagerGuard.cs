using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SignalsIntegrationTests.Infrastructure
{
    public class ServiceManagerGuard
    {
        private static ServiceManager instance = null;
        private static object paddle = new object();
        private static int counter = 0;

        static ServiceManagerGuard()
        {
            ServiceManager.RebuildDatabase();
        }

        private ServiceManagerGuard()
        {
        }

        public static IDisposable Attach()
        {
            lock(paddle)
            {
                if (instance == null)
                {
                    instance = new ServiceManager();
                    instance.StartService();
                }
                return new ReferenceCounter();
            }
        }

        private static void Detach()
        {
            lock(paddle)
            {
                if (counter == 0)
                {
                    instance.StopService();
                    instance = null;
                }
            }
        }

        private class ReferenceCounter : IDisposable
        {
            public ReferenceCounter()
            {
                Interlocked.Increment(ref ServiceManagerGuard.counter);
            }

            ~ReferenceCounter()
            {
                Dispose();
            }

            public void Dispose()
            {
                Interlocked.Decrement(ref ServiceManagerGuard.counter);
                ServiceManagerGuard.Detach();
            }
        }

    }
}
