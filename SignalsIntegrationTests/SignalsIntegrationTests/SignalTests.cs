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
        private WS.SignalsWebServiceClient client;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            ServiceManager.RebuildDatabase();

            serviceManager = new ServiceManager();
            serviceManager.StartService();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            client = new WS.SignalsWebServiceClient();
        }

        // TODO Get from empty db

        [TestMethod]
        public void RequestForNonExistingSignalThrowsOrReturnsNull()
        {
            var path = Path.FromString("/non/existent/path");

            Assertions.AssertReturnsNullOrThrows(() => client.Get(path.ToDto<Dto.Path>()));
        }

        [TestMethod]
        public void AddingSignalSetsItsId()
        {
            var signal = new Signal()
            {
                Path = Path.FromString("/new/signal1"),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            signal = client.Add(signal.ToDto<Dto.Signal>()).ToDomain<Domain.Signal>();

            Assert.IsNotNull(signal.Id);
        }

        [TestMethod]
        public void AddedSignalCanBeRetrieved()
        {
            var newSignal = new Signal()
            {
                Path = Path.FromString("/new/signal2"),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            client.Add(newSignal.ToDto<Dto.Signal>());
            var received = client.Get(newSignal.Path.ToDto<Dto.Path>()).ToDomain<Domain.Signal>();

            Assert.AreEqual(newSignal.DataType, received.DataType);
            Assert.AreEqual(newSignal.Path, received.Path);
            Assert.AreEqual(received.Granularity, received.Granularity);
        }

        [TestMethod]
        public void MultipleSignalsCanBeStoredSimultanously()
        {
            var newSignal1 = new Signal()
            {
                Path = Path.FromString("/new/signal/3"),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };
            var newSignal2 = new Signal()
            {
                Path = Path.FromString("/new/signal/4"),
                Granularity = Granularity.Hour,
                DataType = DataType.Double
            };

            client.Add(newSignal1.ToDto<Dto.Signal>());
            client.Add(newSignal2.ToDto<Dto.Signal>());
            var received1 = client.Get(newSignal1.Path.ToDto<Dto.Path>()).ToDomain<Domain.Signal>();
            var received2 = client.Get(newSignal2.Path.ToDto<Dto.Path>()).ToDomain<Domain.Signal>();

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
