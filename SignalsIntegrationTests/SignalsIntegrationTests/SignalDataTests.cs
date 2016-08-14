using System;
using System.Collections.Generic;
using System.Linq;
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
        [TestCategory("issue2")]
        public void GivenASignalWithSingleDatum_WhenGettingData_ReturnsTheDatum()
        {
            ForAllSignalTypes((dataType, granularity, quality, timestamp, message)
            =>
            {
                GivenASignalWith(dataType, granularity);
                var datum = GivenDatumFor(dataType, granularity, quality);

                var retrievedData = client.GetData(signalId, timestamp, timestamp)
                    .SingleOrDefault();

                Assertions.AreEqual(datum, retrievedData, message);
            });
            }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASignalWithUnorderedData_WhenGettingData_ReturnsDataSorted()
        {
            ForAllSignalTypes((dataType, granularity, quality, timestamp, message)
            =>
            {
                GivenASignalWith(dataType, granularity);
                var data = GivenTwoUnsortedDatumsFor(dataType, granularity, quality);

                var retrievedData = client.GetData(signalId, timestamp, GetSecondNextTimestamp(timestamp, granularity));

                Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), retrievedData, message);
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
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
        [TestCategory("issue2")]
        public void GivenSignalWithData_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var timestamp = new DateTime(2019, 1, 1);

            GivenASignalWith(Granularity.Day);

            GivenData(new[] { new Datum<int>() { Timestamp = timestamp.AddDays(0), Value = 1, Quality = Quality.Fair },
                              new Datum<int>() { Timestamp = timestamp.AddDays(1), Value = 2, Quality = Quality.Fair },
                              new Datum<int>() { Timestamp = timestamp.AddDays(2), Value = 3, Quality = Quality.Fair },
                              new Datum<int>() { Timestamp = timestamp.AddDays(3), Value = 4, Quality = Quality.Fair },
                            });


            client.SetData(
                signalId,
                new[] { new Datum<int>() { Timestamp = timestamp.AddDays(1), Value = 8, Quality = Quality.Good},
                        new Datum<int>() { Timestamp = timestamp.AddDays(2), Value = 9, Quality = Quality.Good},
                      }.ToDto<Dto.Datum[]>());

            var expectedData = new[] { new Datum<int>() { Timestamp = timestamp.AddDays(0), Value = 1, Quality = Quality.Fair },
                                       new Datum<int>() { Timestamp = timestamp.AddDays(1), Value = 8, Quality = Quality.Good},
                                       new Datum<int>() { Timestamp = timestamp.AddDays(2), Value = 9, Quality = Quality.Good},
                                       new Datum<int>() { Timestamp = timestamp.AddDays(3), Value = 4, Quality = Quality.Fair },
                                     };

            var retrievedData = client.GetData(signalId, timestamp, timestamp.AddDays(4))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(expectedData, retrievedData);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASignal_WhenSettingDataWithDuplicateTimestamps_Throws()
        {
            GivenASignalWith(Granularity.Day);

            Assertions.AssertThrows(() => client.SetData(
                signalId,
                new[] { new Dto.Datum() { Timestamp = new DateTime(2000, 1, 1) },
                        new Dto.Datum() { Timestamp = new DateTime(2000, 1, 1) } }));
        }

        private void ForAllSignalTypes(Action<DataType, Granularity, Quality, DateTime, string> test)
        {
            foreach (var dataType in Enum.GetValues(typeof(DataType)).Cast<DataType>())
            {
                foreach (var granularity in Enum.GetValues(typeof(Granularity)).Cast<Granularity>())
                {
                    foreach (var quality in Enum.GetValues(typeof(Quality)).Cast<Quality>())
                    { 
                        var message = dataType.ToString() + ", " + granularity.ToString() + ", " + quality.ToString();
                        test(dataType, granularity, quality, timestamps[granularity], message);
                    }
                }
            }
        }

        private Dto.Datum GivenDatumFor(DataType dataType, Granularity granularity, Quality quality)
        {
            var datum = new Dto.Datum()
            {
                Quality = quality.ToDto<Dto.Quality>(),
                Timestamp = timestamps[granularity],
                Value = values[dataType]
            };

            GivenSingleDatum(datum);

            return datum;
        }

        private Dto.Datum[] GivenTwoUnsortedDatumsFor(DataType dataType, Granularity granularity, Quality quality)
        {
            var from = timestamps[granularity];
            var to = GetNextTimestamp(from, granularity);

            var data1 = new Dto.Datum()
            {
                Quality = quality.ToDto<Dto.Quality>(),
                Timestamp = to,
                Value = values[dataType]
            };
            var data2 = new Dto.Datum()
            {
                Quality = quality.ToDto<Dto.Quality>(),
                Timestamp = from,
                Value = values[dataType]
            };

            GivenData(data1, data2);

            return new[] { data1, data2 };
        }

        private DateTime GetNextTimestamp(DateTime dateTime, Granularity granularity)
        {
            var te = new TimeEnumerator(dateTime, 1, granularity);
            return te.ToExcludedUtcUtc;
        }

        private DateTime GetSecondNextTimestamp(DateTime dateTime, Granularity granularity)
        {
            var te = new TimeEnumerator(dateTime, 2, granularity);
            return te.ToExcludedUtcUtc;
        }

        private Dictionary<DataType, object> values = new Dictionary<DataType, object>()
            { { DataType.Boolean, true },
              { DataType.Decimal, 42.0m },
              { DataType.Double, 42.42 },
              { DataType.Integer, 42 },
              { DataType.String, "string" } };
        private Dictionary<Granularity, DateTime> timestamps = new Dictionary<Granularity, DateTime>()
            { { Granularity.Day, new DateTime(2000, 2, 12) },
              { Granularity.Hour, new DateTime(2000, 2, 12, 13, 0, 0) },
              { Granularity.Minute, new DateTime(2000, 2, 12, 13, 14, 0) },
              { Granularity.Month, new DateTime(2000, 2, 1) },
              { Granularity.Second, new DateTime(2000, 2, 12, 13, 14, 15) },
              { Granularity.Week, new DateTime(2000, 2, 7) },
              { Granularity.Year, new DateTime(2000, 1, 1) } };
    }
}
