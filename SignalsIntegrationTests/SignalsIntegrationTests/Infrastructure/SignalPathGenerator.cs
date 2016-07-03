using System.Threading;
using Domain;

namespace SignalsIntegrationTests.Infrastructure
{
    public static class SignalPathGenerator
    {
        private static int signalCounter = 0;

        public static Path Generate()
        {
            Interlocked.Increment(ref signalCounter);
            return Path.FromString("/new/signal" + signalCounter.ToString());
        }
    }
}
