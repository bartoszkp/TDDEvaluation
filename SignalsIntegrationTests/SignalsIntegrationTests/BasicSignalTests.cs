using System;
using Domain;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class BasicSignalTests : TestsBase
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            TestsBase.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            TestsBase.ClassCleanup();
        }

        [TestMethod]
        [TestCategory("issue13")]
        public void GivenBadPath_WhenGettingSignal_NullIsReturned()
        {
            var path = Path.FromString("/non/existent/path");

            Assert.IsNull(client.Get(path.ToDto<Dto.Path>()));
        }

        [TestMethod]
        [TestCategory("issue13")]
        public void GivenBadId_WhenGettingSignal_NullIsReturned()
        {
            Assert.IsNull(client.GetById(0));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void RequestForSettingFirstOrderMissingValuePolicyForStringThrows()
        {
            var signalId = AddNewStringSignal().Id.Value;

            var firstOrderMissingValuePolicy
                 = (new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<string>())
                 .ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();

            Assertions.AssertThrows(() => client.SetMissingValuePolicy(signalId, firstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void RequestForSettingFirstOrderMissingValuePolicyForBooleanThrows()
        {
            var signalId = AddNewSignal(DataType.Boolean, Granularity.Day).Id.Value;

            var firstOrderMissingValuePolicy
                 = (new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<string>())
                 .ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();

            Assertions.AssertThrows(() => client.SetMissingValuePolicy(signalId, firstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue1")]
        public void AddingSignalSetsItsId()
        {
            var signal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            signal = client.Add(signal.ToDto<Dto.Signal>()).ToDomain<Domain.Signal>();

            Assert.IsNotNull(signal.Id);
        }

        [TestMethod]
        [TestCategory("issue4")]
        public void GivenASignal_WhenGettingByIncompletePath_ReturnsNull()
        {
            GivenASignal(Path.FromString("/root/signal1"));

            var result = client.Get(new Dto.Path() { Components = new[] { "root" } });

            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("issue1")]
        [TestCategory("issue4")]
        public void AddedSignalCanBeRetrieved()
        {
            var newSignal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            client.Add(newSignal.ToDto<Dto.Signal>());

            var received = client.Get(newSignal.Path.ToDto<Dto.Path>()).ToDomain<Domain.Signal>();
            var receivedById = client.GetById(received.Id.Value).ToDomain<Domain.Signal>();

            Assert.AreEqual(newSignal.DataType, received.DataType);
            Assert.AreEqual(newSignal.DataType, receivedById.DataType);
            Assert.AreEqual(newSignal.Path, received.Path);
            Assert.AreEqual(newSignal.Path, receivedById.Path);
            Assert.AreEqual(received.Granularity, received.Granularity);
            Assert.AreEqual(received.Granularity, receivedById.Granularity);
        }

        [TestMethod]
        [TestCategory("issue4")]
        public void MultipleSignalsCanBeStoredSimultanously()
        {
            var newSignal1 = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };
            var newSignal2 = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Hour,
                DataType = DataType.Double
            };

            client.Add(newSignal1.ToDto<Dto.Signal>());
            client.Add(newSignal2.ToDto<Dto.Signal>());
            var received1 = client.Get(newSignal1.Path.ToDto<Dto.Path>()).ToDomain<Domain.Signal>();
            var received2 = client.Get(newSignal2.Path.ToDto<Dto.Path>()).ToDomain<Domain.Signal>();

            Assert.AreEqual(newSignal1.Path, received1.Path);
            Assert.AreEqual(newSignal1.Granularity, received1.Granularity);
            Assert.AreEqual(newSignal1.DataType, received1.DataType);
            Assert.AreEqual(newSignal2.Path, received2.Path);
            Assert.AreEqual(newSignal2.Granularity, received2.Granularity);
            Assert.AreEqual(newSignal2.DataType, received2.DataType);
            Assert.AreNotEqual(received1.Id, received2.Id);
        }

        [TestMethod]
        [TestCategory("issue1")]
        public void TryingToAddSignalWithExistingPathThrows()
        {
            var signal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            client.Add(signal.ToDto<Dto.Signal>());

            Assertions.AssertThrows(() => client.Add(signal.ToDto<Dto.Signal>()));
        }

        [TestMethod]
        [TestCategory("issue1")]
        public void GivenASignal_WhenAddingAnotherWithTheSameId_Throws()
        {
            GivenASignal();

            var newSignal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer,
                Id = signalId
            };

            Assertions.AssertThrows(() => client.Add(newSignal.ToDto<Dto.Signal>()));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenNoSignals_WhenAddingASignalWithNullPath_ServiceThrows()
        {
            var signal = new Signal()
            {
                Path = null,
                Granularity = Granularity.Day,
                DataType = DataType.Integer,
            };

            Assertions.AssertThrows(() => client.Add(signal.ToDto<Dto.Signal>()));
        }

        [TestMethod]
        [TestCategory("issue12")]
        public void WhenDeletingNonExistentSignal_Throws()
        {
            Assertions.AssertThrows(() => client.Delete(0));
        }

        [TestMethod]
        [TestCategory("issue12")]
        public void WhenDeletingExistingSignal_SignalDisappears()
        {
            var signalId = AddNewIntegerSignal(granularity: Granularity.Year).Id.Value;

            client.Delete(signalId);

            Assert.IsNull(client.GetById(signalId));
        }

        [TestMethod]
        [TestCategory("issue12")]
        public void WhenDeletingExistingSignalWithData_BothSignalAndDataDisappear()
        {
            var signalId = AddNewIntegerSignal(granularity: Granularity.Year).Id.Value;

            var ts = new DateTime(2000, 1, 1);

            client.SetData(signalId, new[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = ts, Value = 0 } });

            client.Delete(signalId);

            Assertions.AssertThrows(() => ClientGetData(signalId, ts, ts.AddYears(1)));
        }
    }
}
