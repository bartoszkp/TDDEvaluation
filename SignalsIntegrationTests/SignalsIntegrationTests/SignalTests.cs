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
        public void SetDataUsingIncompleteSignalsThrows()
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
            // TODO this test can be removed after extensive MissingDataPolicyConfig tests
            var signal = AddNewIntegerSignal(Granularity.Day);

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
        public void SetDataForSecondGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Second);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 44, 123),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMinuteGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Minute);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 123),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMinuteGranularityRequiresZerosSecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Minute);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForHourGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Hour);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 10),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForHourGranularityRequiresZerosSecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Hour);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForHourGranularityRequiresZerosMinutesInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Hour);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true )]
        public void SetDataForDayGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Day);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 10),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForDayGranularityRequiresZerosSecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Day);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForDayGranularityRequiresZerosMinutesInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Day);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForDayGranularityRequiresZerosHoursInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Day);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 0, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForWeekGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Week);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 10),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForWeekGranularityRequiresZerosSecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Week);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForWeekGranularityRequiresZerosMinutesInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Week);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForWeekGranularityRequiresZerosHoursInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Week);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 0, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForWeekGranularityRequiresMondayInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Week);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 0, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMonthGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Month);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 10),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMonthGranularityRequiresZerosSecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Month);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMonthGranularityRequiresZerosMinutesInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Month);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMonthGranularityRequiresZerosHoursInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Month);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 0, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForMonthGranularityRequiresFirstDayOfMonthInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Month);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }


        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForYearGranularityRequiresZerosMillisecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 10),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForYearGranularityRequiresZerosSecondsInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 10, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForYearGranularityRequiresZerosMinutesInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            signal = client.Add(signal);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForYearGranularityRequiresZerosHoursInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 0, 0, 0),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForYearGranularityRequiresFirstDayOfMonthInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
        public void SetDataForYearGranularityRequiresFirstMonthInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 2, 1),
                }
            };

            client.SetData(signal.ToDto<Dto.Signal>(), data.ToDto());
        }

        [TestMethod]
        public void NewSignalHasNoneQualityMissingValuePolicy()
        {
            var signal = AddNewIntegerSignal();

            var result = client.GetMissingValuePolicyConfig(signal);

            Assert.AreEqual(Dto.MissingValuePolicy.NoneQuality, result.Policy);
        }

        private Dto.Signal AddNewIntegerSignal(Domain.Granularity granularity = Granularity.Second)
        {
            var signal = new Signal()
            {
                Path = GenerateUniqueSignalPath(),
                Granularity = granularity,
                DataType = DataType.Integer,
            };

            return client.Add(signal.ToDto<Dto.Signal>());
        }

        [TestMethod]
        public void MissingValuePolicyCanBeSetForSignal()
        {
            var signal = AddNewIntegerSignal();

            var newConfig = new MissingValuePolicyConfig() { Policy = MissingValuePolicy.SpecificValue };

            client.SetMissingValuePolicyConfig(signal, newConfig.ToDto<Dto.MissingValuePolicyConfig>());
            var result = client.GetMissingValuePolicyConfig(signal);

            Assert.AreEqual(newConfig.Policy, result.ToDomain<Domain.MissingValuePolicyConfig>().Policy);
        }
        /* TODO bad timestamps in GetData
                Second,
                Minute,
                Hour,
                Day,
                Week,
                Month,
                Year
        */

        /* TODO correct timestamps in GetData (?)
                Second,
                Minute, 
                Hour,
                Day,    
                Week,
                Month,
                Year
        */

        /* TODO correct timestamps in SetData (?)
                    Second,
                    Minute,
                    Hour,
                    Day,
                    Week,
                    Month,
                    Year
        */

        // TODO SetMissing.... validates Params (?)
        // TODO GetData range validation

        // TODO GetData with different MissingValuePolicy

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
