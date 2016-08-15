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

            WhenSettigDataFor(UniversalBeginTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForSecondGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Second);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMinuteGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Minute);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMinuteGranularityRequiresZeroSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Minute);

            WhenSettigDataFor(UniversalBeginTimestamp.AddSeconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForHourGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Hour);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForHourGranularityRequiresZeroSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Hour);

            WhenSettigDataFor(UniversalBeginTimestamp.AddSeconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForHourGranularityRequiresZeroMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Hour);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMinutes(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForDayGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForDayGranularityRequiresZeroSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenSettigDataFor(UniversalBeginTimestamp.AddSeconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForDayGranularityRequiresZeroMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMinutes(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForDayGranularityRequiresZeroHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenSettigDataFor(UniversalBeginTimestamp.AddHours(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForWeekGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForWeekGranularityRequiresZeroSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(UniversalBeginTimestamp.AddSeconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForWeekGranularityRequiresZeroMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMinutes(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForWeekGranularityRequiresZeroHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(UniversalBeginTimestamp.AddHours(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForWeekGranularityRequiresMondayInTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenSettigDataFor(UniversalBeginTimestamp.AddDays(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMonthGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMonthGranularityRequiresZeroSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(UniversalBeginTimestamp.AddSeconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMonthGranularityRequiresZeroMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMinutes(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMonthGranularityRequiresZeroHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(UniversalBeginTimestamp.AddHours(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForMonthGranularityRequiresFirstDayOfMonthInTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenSettigDataFor(UniversalBeginTimestamp.AddDays(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresZeroMillisecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMilliseconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresZeroSecondsInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(UniversalBeginTimestamp.AddSeconds(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresZeroMinutesInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMinutes(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresZeroHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(UniversalBeginTimestamp.AddHours(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresFirstDayOfMonthInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(UniversalBeginTimestamp.AddDays(1));

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresFirstMonthInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenSettigDataFor(UniversalBeginTimestamp.AddMonths(1));

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
