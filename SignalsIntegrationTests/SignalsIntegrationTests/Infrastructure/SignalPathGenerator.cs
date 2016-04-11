using System.Threading;
using Domain;

namespace SignalsIntegrationTests.Infrastructure
{
    public class SignalPathGenerator
    {
        private static int signalCounter = 0;

        public static Path Generate()
        {
            Interlocked.Increment(ref signalCounter);
            return Path.FromString("/new/signal" + signalCounter.ToString());
        }

        private SignalPathGenerator()
        {
        }
    }
}
