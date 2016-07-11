using System;
using Domain;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class SetDataArgumentsTests : TestsBase
    {
        private DateTime Timestamp { get; } = new DateTime(2029, 1, 1, 0, 0, 0, 0);

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
            GivenNoSignal();

            WhenSettigDataFor(Timestamp);

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForSecondGranularityRequiresZerosMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Second);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForMinuteGranularityRequiresZerosMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Minute);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForMinuteGranularityRequiresZerosSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Minute);

            WhenSettigDataFor(Timestamp.AddSeconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForHourGranularityRequiresZerosMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Hour);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForHourGranularityRequiresZerosSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Hour);

            WhenSettigDataFor(Timestamp.AddSeconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForHourGranularityRequiresZerosMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Hour);

            WhenSettigDataFor(Timestamp.AddMinutes(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForDayGranularityRequiresZerosMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForDayGranularityRequiresZerosSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenSettigDataFor(Timestamp.AddSeconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForDayGranularityRequiresZerosMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenSettigDataFor(Timestamp.AddMinutes(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForDayGranularityRequiresZerosHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenSettigDataFor(Timestamp.AddHours(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForWeekGranularityRequiresZerosMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForWeekGranularityRequiresZerosSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(Timestamp.AddSeconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForWeekGranularityRequiresZerosMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(Timestamp.AddMinutes(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForWeekGranularityRequiresZerosHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(Timestamp.AddHours(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForWeekGranularityRequiresMondayInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(Timestamp.AddDays(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForMonthGranularityRequiresZerosMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForMonthGranularityRequiresZerosSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(Timestamp.AddSeconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForMonthGranularityRequiresZerosMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(Timestamp.AddMinutes(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForMonthGranularityRequiresZerosHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(Timestamp.AddHours(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForMonthGranularityRequiresFirstDayOfMonthInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(Timestamp.AddDays(1));

            ResultThrows();
        }


        [TestMethod]
        public void SetDataForYearGranularityRequiresZerosMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForYearGranularityRequiresZerosSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(Timestamp.AddSeconds(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForYearGranularityRequiresZerosMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(Timestamp.AddMinutes(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForYearGranularityRequiresZerosHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(Timestamp.AddHours(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForYearGranularityRequiresFirstDayOfMonthInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(Timestamp.AddDays(1));

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForYearGranularityRequiresFirstMonthInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(Timestamp.AddMonths(1));

            ResultThrows();
        }

        private int signalId;
        private Action setDataAction;

        private void GivenASignalWith(Granularity granularity)
        {
            signalId = AddNewIntegerSignal(granularity).Id.Value;
        }

        private void GivenNoSignal()
        {
            signalId = 0;
        }

        private void WhenSettigDataFor(DateTime dataTimestampUtc)
        {
            var data = new[]
            {
                new Datum<int>()
                {
                    Timestamp = dataTimestampUtc,
                    Value = 42,
                    Quality = Quality.Good
                }
            };

            setDataAction = () => client.SetData(signalId, data.ToDto<Dto.Datum[]>());
        }

        private void ResultThrows()
        {
            Assertions.AssertThrows(setDataAction);
        }
    }
}
