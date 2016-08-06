using System;
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
        public void CanWriteAndRetrieveSecondData()
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
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddSeconds(1))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data, retrievedData);
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
        public void CanWriteAndRetrieveMinuteData()
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
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddMinutes(1))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data, retrievedData);
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
        public void CanWriteAndRetrieveHourData()
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
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddHours(1))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data, retrievedData);
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
        public void CanWriteAndRetrieveDayData()
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
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddDays(1))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data, retrievedData);
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
        public void CanWriteAndRetrieveWeekData()
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
                    Timestamp = timestamp,
                    Value = 4,
                    Quality = Quality.Fair
                }
            };

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddDays(7))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data, retrievedData);
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
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddMonths(1))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data, retrievedData);
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
            var retrievedData = client.GetData(signal.Id.Value, timestamp, timestamp.AddDays(1))
                .ToDomain<Domain.Datum<int>[]>();

            Assertions.AreEqual(data, retrievedData);
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
        [TestCategory("issue2")]
        public void GetDataUsingNonExistentSignalsThrows()
        {
            int dummySignalId = 0;

            Assertions.AssertThrows(() => client.GetData(dummySignalId, new DateTime(2016, 12, 10), new DateTime(2016, 12, 14)));
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
