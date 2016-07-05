using System;
using Domain;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class SignalDataTests : TestsBase
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
        public void GetDataUsingIncompleteSignalsThrows()
        {
            int dummySignalId = 0;

            Assertions.AssertThrows(() => client.GetData(dummySignalId, new DateTime(2016, 12, 10), new DateTime(2016, 12, 14)));
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
        public void SetDataOverridesExistingData()
        {
            var timestamp = new DateTime(2019, 1, 1);
            var signal = AddNewIntegerSignal(Granularity.Day);

            var originalData = new[] { new Datum<int>() { Timestamp = timestamp.AddDays(0), Value = 1, Quality = Quality.Fair },
                                       new Datum<int>() { Timestamp = timestamp.AddDays(1), Value = 2, Quality = Quality.Fair },
                                       new Datum<int>() { Timestamp = timestamp.AddDays(2), Value = 3, Quality = Quality.Fair },
                                       new Datum<int>() { Timestamp = timestamp.AddDays(3), Value = 4, Quality = Quality.Fair },
                                     };

            var newData      = new[] { new Datum<int>() { Timestamp = timestamp.AddDays(1), Value = 8, Quality = Quality.Good},
                                       new Datum<int>() { Timestamp = timestamp.AddDays(2), Value = 9, Quality = Quality.Good},
                                     };

            var expectedData = new[] { new Datum<int>() { Timestamp = timestamp.AddDays(0), Value = 1, Quality = Quality.Fair },
                                       new Datum<int>() { Timestamp = timestamp.AddDays(1), Value = 8, Quality = Quality.Good},
                                       new Datum<int>() { Timestamp = timestamp.AddDays(2), Value = 9, Quality = Quality.Good},
                                       new Datum<int>() { Timestamp = timestamp.AddDays(3), Value = 4, Quality = Quality.Fair },
                                     };

            client.SetData(signal.Id.Value, originalData.ToDto<Dto.Datum[]>());
            client.SetData(signal.Id.Value, newData.ToDto<Dto.Datum[]>());

            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddDays(4)).ToDomain<Domain.Datum<int>[]>();
            Assertions.AssertEqual(expectedData, retrievedData);
        }

        // TODO GetData range validation
    }
}
