using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Dto.Conversions;
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

            Assertions.AssertReturnsNullOrThrows(() => signalsClient.Get(path.ToDto<Dto.Path>()));
        }

        [TestMethod]
        public void AddingSignalSetsItsId()
        {
            var signal = new Signal()
            {
                Path = Path.FromString("/new/signal"),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            signal = signalsClient.Add(signal.ToDto<Dto.Signal>()).ToDomain<Domain.Signal>();

            Assert.IsNotNull(signal.Id);
        }

        [TestMethod]
        public void AddedSignalCanBeRetrieved()
        {
            var newSignal = new Signal()
            {
                Path = Path.FromString("/new/signal"),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            signalsClient.Add(newSignal.ToDto<Dto.Signal>());
            var received = signalsClient.Get(newSignal.Path.ToDto<Dto.Path>()).ToDomain<Domain.Signal>();

            Assert.AreEqual(newSignal.DataType, received.DataType);
            Assert.AreEqual(newSignal.Path, received.Path);
            Assert.AreEqual(received.Granularity, received.Granularity);
        }

        [TestMethod]
        public void MultipleSignalsCanBeStoredSimultanously()
        {
            var newSignal1 = new Signal()
            {
                Path = Path.FromString("/new/signal/1"),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };
            var newSignal2 = new Signal()
            {
                Path = Path.FromString("/new/signal/2"),
                Granularity = Granularity.Hour,
                DataType = DataType.Double
            };

            signalsClient.Add(newSignal1.ToDto<Dto.Signal>());
            signalsClient.Add(newSignal2.ToDto<Dto.Signal>());
            var received1 = signalsClient.Get(newSignal1.Path.ToDto<Dto.Path>()).ToDomain<Domain.Signal>();
            var received2 = signalsClient.Get(newSignal2.Path.ToDto<Dto.Path>()).ToDomain<Domain.Signal>();

            Assert.AreEqual(newSignal1.Path, received1.Path);
            Assert.AreEqual(newSignal2.Path, received2.Path);
            Assert.AreNotEqual(received1.Id, received2.Id);
        }

        // TODO multiple times adding same signal (expected behaviour?)
        // TODO removing?
        // TODO editing?
        // TODO changing path?

            // TODO persistency tests

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
