using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using Dto.Conversions;
using SignalsIntegrationTests.Infrastructure;
using System;
using System.Threading;
using System.ServiceModel;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class SignalTests
    {
        private static int signalCounter = 0;
        private static ServiceManager serviceManager;
        private WS.SignalsWebServiceClient client;

        private static Path GenerateUniqueSignalPath()
        {
            Interlocked.Increment(ref signalCounter);
            return Path.FromString("/new/signal" + signalCounter.ToString());
        }

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
                Path = GenerateUniqueSignalPath(),
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
                Path = GenerateUniqueSignalPath(),
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
                Path = GenerateUniqueSignalPath(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };
            var newSignal2 = new Signal()
            {
                Path = GenerateUniqueSignalPath(),
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
            var path = GenerateUniqueSignalPath();
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

            client.SetData(signal, data.ToDto());
            var retrievedData = client.GetData(signal, timestamp, timestamp.AddDays(1));

            Assert.AreEqual(data.Length, retrievedData.Length);
            Assert.AreEqual(data[0].Value, retrievedData[0].Value);
            Assert.AreEqual(data[0].Timestamp, retrievedData[0].Timestamp);
            Assert.AreEqual(data[0].Quality, retrievedData[0].ToDomain<Domain.Datum<int>>().Quality);
        }

        [TestMethod]
        public void GetDataUsingIncompleteSignalsThrowsOrReturnsNull()
        {
            var newSignal = new Signal()
            {
                Path = GenerateUniqueSignalPath(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            Assertions.AssertReturnsNullOrThrows(() => client.GetData(newSignal.ToDto<Dto.Signal>(), new DateTime(2016, 12, 10), new DateTime(2016, 12, 14)));
         }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataUsingIncompleteSignalsThrowsOrReturnsNull()
        {
            var timestamp = new DateTime(2019, 4, 14);
            var signal = new Signal()
            {
                Path = GenerateUniqueSignalPath(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };
            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        public void TryingToAddSignalWithExistingPathThrowsOrReturnsNull()
        {
            var signal = new Signal()
            {
                Path = GenerateUniqueSignalPath(),
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
                Path = GenerateUniqueSignalPath(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer,
                Id = 42
            };

            Assertions.AssertReturnsNullOrThrows(() => client.Add(signal.ToDto<Dto.Signal>()));
        }

        [TestMethod]
        public void SignalWithoutDataReturnsNoneQualityDatumsForEachTimerangeStep()
        {
            var signal = new Signal()
            {
                Path = GenerateUniqueSignalPath(),
                Granularity = Granularity.Day,
                DataType = DataType.Integer,
            }.ToDto<Dto.Signal>();

            signal = client.Add(signal);

            const int numberOfDays = 5;
            var timestamp = new DateTime(2019, 1, 1);
            var receivedData = client.GetData(signal, timestamp, timestamp.AddDays(numberOfDays));

            Assert.AreEqual(numberOfDays, receivedData.Length);
            foreach (var datum in receivedData)
            {
                Assert.AreEqual(timestamp, datum.Timestamp);
                Assert.AreEqual(Dto.Quality.None, datum.Quality);
                timestamp = timestamp.AddDays(1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataWithSecondGranularityChecksDatumTimestampHasZeroMilliseconds()
        {
            var signal = new Signal()
            {
                Path = GenerateUniqueSignalPath(),
                Granularity = Granularity.Second,
                DataType = DataType.Integer,
            }.ToDto<Dto.Signal>();

            signal = client.Add(signal);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 44, 123),
                    Value = 4
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        /*
    TODO bad timestamps in SetData
    Minute,
    Hour,
    Day,
    Week,
    Month,
    Year
        */

        /*
TODO bad timestamps in GetData
Second,
Minute,
Hour,
Day,
Week,
Month,
Year
*/

        // TODO GetData with different MissingValuePolicy

        // TODO trying to set data with wrong granulation - expected behavior?

        // TODO data outside range
        // TODO different "missing" data  behaviour

        // TODO removing?
        // TODO editing?
        // TODO changing path?

        // TODO persistency tests - problem - sequential run of unit tests...

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
