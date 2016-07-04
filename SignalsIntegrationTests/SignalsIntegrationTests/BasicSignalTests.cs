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
        public void RequestForNonExistingSignalThrowsOrReturnsNull()
        {
            var path = Path.FromString("/non/existent/path");

            Assertions.AssertReturnsNullOrThrows(() => client.Get(path.ToDto<Dto.Path>()));
        }

        [TestMethod]
        public void RequestForSettingFirstOrderMissingValuePolicyForStringThrows()
        {
            var signalId = AddNewStringSignal().Id.Value;

            var firstOrderMissingValuePolicy
                 = (new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<string>())
                 .ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();

            Assertions.AssertThrows(() => client.SetMissingValuePolicy(signalId, firstOrderMissingValuePolicy));
        }

        [TestMethod]
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
            Assert.AreEqual(newSignal2.Path, received2.Path);
            Assert.AreNotEqual(received1.Id, received2.Id);
        }

        [TestMethod]
        public void TryingToAddSignalWithExistingPathThrowsOrReturnsNull()
        {
            var signal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            client.Add(signal.ToDto<Dto.Signal>());

            Assertions.AssertReturnsNullOrThrows(() => client.Add(signal.ToDto<Dto.Signal>()));
        }

        [TestMethod]
        public void TryingToAddSignalWithNotNullIdThrowsOrReturnsNull()
        {
            var signal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer,
                Id = 42
            };

            Assertions.AssertReturnsNullOrThrows(() => client.Add(signal.ToDto<Dto.Signal>()));
        }

        [TestMethod]
        public void NewSignalHasNoneQualityMissingValuePolicy()
        {
            var signal = AddNewIntegerSignal();

            var result = client.GetMissingValuePolicy(signal.Id.Value);

            Assert.IsInstanceOfType(result, typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        public void MissingValuePolicyCanBeSetForSignal()
        {
            var signal1Id = AddNewIntegerSignal().Id.Value;
            var signal2Id = AddNewIntegerSignal().Id.Value;

            var policy1 = new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>(); // TODO another class
            var policy2 = new Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<int>()
                {
                    Value = 42,
                    Quality = Quality.Fair
                };

            client.SetMissingValuePolicy(signal1Id, policy1.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>());
            client.SetMissingValuePolicy(signal2Id, policy2.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>());

            var result1 = client.GetMissingValuePolicy(signal1Id);
            var result2 = client.GetMissingValuePolicy(signal2Id);

            Assert.IsInstanceOfType(result1, typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
            Assert.IsInstanceOfType(result2, typeof(Dto.MissingValuePolicy.SpecificValueMissingValuePolicy));

            var specificMissingValuePolicy = result2.ToDomain<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<int>>();

            Assert.AreEqual(42, specificMissingValuePolicy.Value);
            Assert.AreEqual(Quality.Fair, specificMissingValuePolicy.Quality);
        }

        // TODO removing?
        // TODO editing?
        // TODO changing path?

        // TODO persistency tests - problem - sequential run of unit tests...
    }
}
