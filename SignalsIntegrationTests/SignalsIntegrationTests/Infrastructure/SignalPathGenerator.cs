using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
