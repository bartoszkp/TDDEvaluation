using System;
using System.Linq;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class GetDataArgumentsTests : TestsBase
    {
        private DateTime FromTimestamp { get; } = new DateTime(2029, 1, 1, 0, 0, 0, 0);
        private DateTime ToTimestamp { get; } = new DateTime(2029, 1, 8, 0, 0, 0, 0);

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
        public void GivenNoSignals_WhenGettingDataWithInvalidSignalId_Throws()
        {
            GivenNoSignals();

            WhenGettigData(FromTimestamp, ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAStringSignal_WhenSettingNullValue_ShouldNotThrow()
        {
            GivenASignalWith(DataType.String, Granularity.Hour);

            client.SetData(signalId, new[] { new Dto.Datum() { Timestamp = new DateTime(2000, 1, 1) } });
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAStringSignalWithNullValue_WhenReadingData_ShouldReturnNullValue()
        {
            var timestamp = new DateTime(2000, 1, 1);

            GivenASignalWith(DataType.String, Granularity.Hour);
            GivenData(new Dto.Datum() { Timestamp = timestamp });

            var result = client.GetData(signalId, timestamp, timestamp)
                .SingleOrDefault();

            Assert.IsNotNull(result);
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASignal_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            ForAllSignalTypes((dataType, granularity, message)
            =>
            {
                GivenASignalWith(dataType, granularity);
                GivenData(new Dto.Datum() { Timestamp = UniversalBeginTimestamp, Quality = Dto.Quality.Good });

                WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

                ThenResultIsEmpty();
            });
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForSecondGranularityRequiresZeroMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Second);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMinuteGranularityRequiresZeroMillisecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Minute);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMinuteGranularityRequiresZeroSecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Minute);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForHourGranularityRequiresZeroMillisecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Hour);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForHourGranularityRequiresZeroSecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Hour);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForHourGranularityRequiresZeroMinutesInFromTimestamp()
        {
            GivenASignalWith(Granularity.Hour);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForDayGranularityRequiresZeroMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForDayGranularityRequiresZeroecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForDayGranularityRequiresZeroMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForDayGranularityRequiresZeroHoursInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresZeroMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresZeroSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresZeroMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresZeroHoursInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresMondayInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddDays(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresZeroMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresZeroSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresZeroMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresZeroHoursInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresFirstDayOfMonthInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddDays(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresZeroMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresZeroSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresZeroMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresZeroHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresFirstDayOfMonthInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddDays(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresFirstMonthInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddMonths(1), ToTimestamp);

            ThenRequestThrows();
        }

        private void WhenGettigData(DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            getData = () => client.GetData(signalId, fromIncludedUtc, toExcludedUtc);
        }

        private void ThenRequestThrows()
        {
            Assertions.AssertThrows(() => getData());
        }

        private void ThenResultIsEmpty()
        {
            var result = getData();

            Assert.IsFalse(result.Any());
        }

        private Func<Dto.Datum[]> getData;
    }
}
