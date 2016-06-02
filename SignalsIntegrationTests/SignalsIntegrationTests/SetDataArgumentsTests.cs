using Domain;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;
using System;
using System.ServiceModel;

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
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
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

            client.SetData(dummySignalId, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(FaultException), AllowDerivedTypes = true)]
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
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

            client.SetData(signal.Id.Value, data.ToDto<Dto.Datum[]>());
        }
    }
}
