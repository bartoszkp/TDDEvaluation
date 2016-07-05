using System;
using System.ServiceModel;
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
        public void CanWriteAndRetrieveData()
        {
            var path = SignalPathGenerator.Generate();
            var timestamp = new DateTime(2019, 4, 14);

            var newSignal1 = new Signal()
            {
                Path = path,
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            var signal = client.Add(newSignal1.ToDto<Dto.Signal>());

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddDays(1));

            Assert.AreEqual(data.Length, retrievedData.Length);
            Assert.AreEqual(data[0].Value, retrievedData[0].Value);
            Assert.AreEqual(data[0].Timestamp, retrievedData[0].Timestamp);
            Assert.AreEqual(data[0].Quality, retrievedData[0].ToDomain<Domain.Datum<int>>().Quality);
        }

        [TestMethod]
        public void CanWriteAndRetrieveMonthlyData()
        {
            var path = SignalPathGenerator.Generate();
            var timestamp = new DateTime(2019, 4, 1);

            var newSignal1 = new Signal()
            {
                Path = path,
                Granularity = Granularity.Month,
                DataType = DataType.Integer
            };

            var signal = client.Add(newSignal1.ToDto<Dto.Signal>());

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddDays(1));

            Assert.AreEqual(data.Length, retrievedData.Length);
            Assert.AreEqual(data[0].Value, retrievedData[0].Value);
            Assert.AreEqual(data[0].Timestamp, retrievedData[0].Timestamp);
            Assert.AreEqual(data[0].Quality, retrievedData[0].ToDomain<Domain.Datum<int>>().Quality);
        }

        [TestMethod]
        public void CanWriteAndRetrieveYearlyData()
        {
            var path = SignalPathGenerator.Generate();
            var timestamp = new DateTime(2019, 1, 1);

            var newSignal1 = new Signal()
            {
                Path = path,
                Granularity = Granularity.Year,
                DataType = DataType.Integer
            };

            var signal = client.Add(newSignal1.ToDto<Dto.Signal>());

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddDays(1));

            Assert.AreEqual(data.Length, retrievedData.Length);
            Assert.AreEqual(data[0].Value, retrievedData[0].Value);
            Assert.AreEqual(data[0].Timestamp, retrievedData[0].Timestamp);
            Assert.AreEqual(data[0].Quality, retrievedData[0].ToDomain<Domain.Datum<int>>().Quality);
        }

        [TestMethod]
        public void GetDataUsingIncompleteSignalsThrowsOrReturnsNull()
        {
            int dummySignalId = 0;

            Assertions.AssertReturnsNullOrThrows(() => client.GetData(dummySignalId, new DateTime(2016, 12, 10), new DateTime(2016, 12, 14)));
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
        public void SignalWithoutDataReturnsNoneQualityDatumsForEachTimerangeStep()
        {
            var signal = AddNewIntegerSignal(Granularity.Day);

            const int numberOfDays = 5;
            var timestamp = new DateTime(2019, 1, 1);
            var receivedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddDays(numberOfDays));

            Assert.AreEqual(numberOfDays, receivedData.Length);
            foreach (var datum in receivedData)
            {
                Assert.AreEqual(timestamp, datum.Timestamp);
                Assert.AreEqual(Dto.Quality.None, datum.Quality);
                timestamp = timestamp.AddDays(1);
            }
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

        [TestMethod]
        public void WhenDeletingNonExistentSignal_Throws()
        {
            Assertions.AssertThrows(() => client.Delete(0));
        }

        [TestMethod]
        public void WhenDeletingExistingSignal_SignalDisappears()
        {
            var signalId = AddNewIntegerSignal(granularity: Granularity.Year).Id.Value;

            client.Delete(signalId);

            Assertions.AssertThrows(() => client.GetById(signalId));
        }

        [TestMethod]
        public void WhenDeletingExistingSignalWithData_BothSignalAndDataDisappear()
        {
            var signalId = AddNewIntegerSignal(granularity: Granularity.Year).Id.Value;

            var ts = new DateTime(2000, 1, 1);

            client.SetData(signalId, new[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = ts, Value = 0 } });

            client.Delete(signalId);

            Assertions.AssertThrows(() => client.GetData(signalId, ts, ts.AddYears(1)));
        }

        // TODO editing?
        // TODO changing path?

        // TODO persistency tests - problem - sequential run of unit tests...
    }
}
