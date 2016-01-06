using Microsoft.VisualStudio.TestTools.UnitTesting;
using Signals.Domain;
using Signals.Dto.Conversions;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class SignalTests
    {
        private static ServiceManager serviceManager;
        private WS.SignalsClient signalsClient;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            serviceManager = new ServiceManager();
            serviceManager.StartService();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            signalsClient = new WS.SignalsClient();
        }

        // TODO Get from empty db

        [TestMethod]
        public void RequestForNonExistingSignalThrowsOrReturnsNull()
        {
            var path = Path.FromString("/non/existent/path");

            Assertions.AssertReturnsNullOrThrows(() => signalsClient.Get(path.ToDto()));
        }

        [TestMethod]
        public void CanAddAndGetSignal()
        {
            var path = Path.FromString(string.Empty);

            var signal = new Signal()
            {
                Path = path,
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            signalsClient.Add(signal.ToDto());

            var result = signalsClient.Get(path.ToDto()).ToDomain();

            Assert.AreEqual(0, result.Id);
            Assert.AreEqual(DataType.Integer, result.DataType);
            Assert.AreEqual(path.ToString(), result.Path.ToString());
            Assert.AreEqual(Granularity.Day, result.Granularity);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            serviceManager.StopService();
        }
    }
}
