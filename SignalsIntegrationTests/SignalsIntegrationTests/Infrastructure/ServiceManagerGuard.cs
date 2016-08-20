using System;
using System.Threading;

namespace SignalsIntegrationTests.Infrastructure
{
    public class ServiceManagerGuard
    {
        private static ServiceManager instance = new ServiceManager();
        private static int counter = 0;
        private static object paddle = new object();

        static ServiceManagerGuard()
        {
            ServiceManager.RebuildDatabase();
            instance.StartService();
        }

        public static IDisposable Attach()
        {
            return new ReferenceCounter();
        }

        public static void EnsureRunning()
        {
            if (!instance.IsAlive())
            {
                lock (paddle)
                {
                    if (!instance.IsAlive())
                    {
                        instance.RestartService();
                    }
                }
            }
        }

        private static void Detach()
        {
            if (counter == 0)
            {
                instance.StopService();
                instance = null;
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
