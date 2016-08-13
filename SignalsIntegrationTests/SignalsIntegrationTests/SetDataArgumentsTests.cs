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
        [TestCategory("issue2")]
        public void GivenNoSignals_WhenSettingDataWithInvalidSignalId_Throws()
        {
            GivenNoSignals();

            WhenSettigDataFor(Timestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForSecondGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Second);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMinuteGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Minute);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMinuteGranularityRequiresZeroSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Minute);

            WhenSettigDataFor(Timestamp.AddSeconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForHourGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Hour);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForHourGranularityRequiresZeroSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Hour);

            WhenSettigDataFor(Timestamp.AddSeconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForHourGranularityRequiresZeroMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Hour);

            WhenSettigDataFor(Timestamp.AddMinutes(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForDayGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForDayGranularityRequiresZeroSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenSettigDataFor(Timestamp.AddSeconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForDayGranularityRequiresZeroMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenSettigDataFor(Timestamp.AddMinutes(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForDayGranularityRequiresZeroHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenSettigDataFor(Timestamp.AddHours(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForWeekGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForWeekGranularityRequiresZeroSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(Timestamp.AddSeconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForWeekGranularityRequiresZeroMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(Timestamp.AddMinutes(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForWeekGranularityRequiresZeroHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(Timestamp.AddHours(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForWeekGranularityRequiresMondayInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(Timestamp.AddDays(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMonthGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMonthGranularityRequiresZeroSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(Timestamp.AddSeconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMonthGranularityRequiresZeroMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(Timestamp.AddMinutes(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMonthGranularityRequiresZeroHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(Timestamp.AddHours(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMonthGranularityRequiresFirstDayOfMonthInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(Timestamp.AddDays(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(Timestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresZeroSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(Timestamp.AddSeconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresZeroMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(Timestamp.AddMinutes(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresZeroHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(Timestamp.AddHours(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresFirstDayOfMonthInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(Timestamp.AddDays(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresFirstMonthInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(Timestamp.AddMonths(1));

            ThenRequestThrows();
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

        private void ThenRequestThrows()
        {
            Assertions.AssertThrows(setDataAction);
        }

        private Action setDataAction;
    }
}
