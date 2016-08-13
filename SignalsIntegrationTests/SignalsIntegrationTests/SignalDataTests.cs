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
        private DateTime timestamp = new DateTime(2019, 4, 14);

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
            var values = new Dictionary<DataType, object>()
            { { DataType.Boolean, true },
              { DataType.Decimal, 42.0m },
              { DataType.Double, 42.42 },
              { DataType.Integer, 42 },
              { DataType.String, "string" } };
            var timestamps = new Dictionary<Granularity, DateTime>()
            { { Granularity.Day, new DateTime(2000, 2, 12) },
              { Granularity.Hour, new DateTime(2000, 2, 12, 13, 0, 0) },
              { Granularity.Minute, new DateTime(2000, 2, 12, 13, 14, 0) },
              { Granularity.Month, new DateTime(2000, 2, 1) },
              { Granularity.Second, new DateTime(2000, 2, 12, 13, 14, 15) },
              { Granularity.Week, new DateTime(2000, 2, 7) },
              { Granularity.Year, new DateTime(2000, 1, 1) } };

            foreach (var dataType in Enum.GetValues(typeof(DataType)).Cast<DataType>())
            {
                foreach (var granularity in Enum.GetValues(typeof(Granularity)).Cast<Granularity>())
                {
                    foreach (var quality in Enum.GetValues(typeof(Quality)).Cast<Quality>())
                    {
                        GivenASignalWith(dataType, granularity);

                        var datum = new Dto.Datum()
                        {
                            Quality = quality.ToDto<Dto.Quality>(),
                            Timestamp = timestamps[granularity],
                            Value = values[dataType]
                        };

                        var te = new Domain.Infrastructure.TimeEnumerator(timestamps[granularity], 1, granularity);
                        te.MoveNext();
                        te.MoveNext();
                        
                        GivenSingleDatum(datum);

                        var retrievedData = client.GetData(signalId, timestamps[granularity], te.Current)
                            .SingleOrDefault();

                        var message = dataType.ToString() + ", " + granularity.ToString() + ", " + quality.ToString();
                        Assert.IsNotNull(retrievedData, message);
                        Assert.AreEqual(datum.Quality, retrievedData.Quality, message);
                        Assert.AreEqual(datum.Timestamp, retrievedData.Timestamp, message);
                        Assert.AreEqual(datum.Value, retrievedData.Value, message);
                        Assert.AreEqual(datum.Value.GetType(), retrievedData.Value.GetType(), message);
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void CanWriteAndRetrieveSecondDataIgnoringOrder()
        {
            var path = SignalPathGenerator.Generate();
            var timestamp = new DateTime(2019, 4, 14);

            var newSignal1 = new Signal()
            {
                Path = path,
                Granularity = Granularity.Second,
                DataType = DataType.Integer
            };

            var signal = client.Add(newSignal1.ToDto<Dto.Signal>());

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = timestamp.AddSeconds(1),
                    Value = 5,
                    Quality = Quality.Fair
                },
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddSeconds(2))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp), retrievedData);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void CanWriteAndRetrieveMinuteDataIgnoringOrder()
        {
            var path = SignalPathGenerator.Generate();
            var timestamp = new DateTime(2019, 4, 14);

            var newSignal1 = new Signal()
            {
                Path = path,
                Granularity = Granularity.Minute,
                DataType = DataType.Integer
            };

            var signal = client.Add(newSignal1.ToDto<Dto.Signal>());

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = timestamp.AddMinutes(1),
                    Value = 5,
                    Quality = Quality.Fair
                },
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddMinutes(2))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp), retrievedData);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void CanWriteAndRetrieveHourDataIgnoringOrder()
        {
            var path = SignalPathGenerator.Generate();
            var timestamp = new DateTime(2019, 4, 14);

            var newSignal1 = new Signal()
            {
                Path = path,
                Granularity = Granularity.Hour,
                DataType = DataType.Integer
            };

            var signal = client.Add(newSignal1.ToDto<Dto.Signal>());

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = timestamp.AddHours(1),
                    Value = 5,
                    Quality = Quality.Fair
                },
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddHours(2))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp), retrievedData);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void CanWriteAndRetrieveDayDataIgnoringOrder()
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
                    Timestamp = timestamp.AddDays(1),
                    Value = 5,
                    Quality = Quality.Fair
                },
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddDays(2))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp), retrievedData);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void CanWriteAndRetrieveWeekDataIgnoringOrder()
        {
            var path = SignalPathGenerator.Generate();
            var timestamp = new DateTime(2019, 1, 7);

            var newSignal1 = new Signal()
            {
                Path = path,
                Granularity = Granularity.Week,
                DataType = DataType.Integer
            };

            var signal = client.Add(newSignal1.ToDto<Dto.Signal>());

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = timestamp.AddDays(7),
                    Value = 5,
                    Quality = Quality.Fair
                },
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddDays(14))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp), retrievedData);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void CanWriteAndRetrieveMonthlyDataIgnoringOrder()
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
                    Timestamp = timestamp.AddMonths(1),
                    Value = 5,
                    Quality = Quality.Fair
                },
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddMonths(2))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp), retrievedData);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void CanWriteAndRetrieveYearlyDataIgnoringOrder()
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
                    Timestamp = timestamp.AddYears(1),
                    Value = 5,
                    Quality = Quality.Fair
                },
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddYears(2))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp), retrievedData);
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
            Assertions.AreEqual(expectedData, retrievedData);
        }
    }
}
