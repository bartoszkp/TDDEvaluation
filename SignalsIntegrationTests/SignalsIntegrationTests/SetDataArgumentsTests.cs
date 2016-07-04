using System;
using System.ServiceModel;
using Domain;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class SetDataArgumentsTests : TestsBase
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
        public void SetDataUsingIncompleteSignalsThrows()
        {
            var timestamp = new DateTime(2019, 4, 14);
            int dummySignalId = 0;
            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = timestamp,
                    Value = 4
                }
            };

            Assertions.AssertThrows(() => client.SetData(dummySignalId, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

           Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }


        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
        public void SetDataForYearGranularityRequiresZerosMinutesInTimestamps()
        {
            var signal = AddNewIntegerSignal(Granularity.Year);

            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = new DateTime(2016, 10, 2, 12, 13, 0, 0),
                }
            };

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
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

            Assertions.AssertThrows(() => client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>()));
        }
    }
}
