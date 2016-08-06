using System;
using System.Linq;
using Domain;
using Dto.Conversions;
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
        public void GetDataUsingIncompleteSignalsThrows()
        {
            GivenNoSignal();

            WhenGettigData(FromTimestamp, ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForSecondGranularityRequiresZerosMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Second);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GetDataForSecondGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Second);

            WhenGettigData(FromTimestamp, FromTimestamp.AddSeconds(-1));

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMinuteGranularityRequiresZerosMillisecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Minute);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMinuteGranularityRequiresZerosSecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Minute);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GetDataForMinuteGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Minute);

            WhenGettigData(FromTimestamp, FromTimestamp.AddMinutes(-1));

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForHourGranularityRequiresZerosMillisecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Hour);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForHourGranularityRequiresZerosSecondsInFromTimestamp()
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
        [TestCategory("issue2")]
        public void GetDataForHourGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Hour);

            WhenGettigData(FromTimestamp, FromTimestamp.AddHours(-1));

            ThenResultIsEmpty();
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
        public void GetDataForDayGranularityRequiresZeroSecondsInFromTimestamps()
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
        public void GetDataForDayGranularityRequiresZerosHoursInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GetDataForDayGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp, FromTimestamp.AddDays(-1));

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresZerosMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresZerosSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresZerosMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresZerosHoursInFromTimestamps()
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
        [TestCategory("issue2")]
        public void GetDataForWeekGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp, FromTimestamp.AddDays(-7));

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresZerosMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresZerosSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresZerosMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresZerosHoursInFromTimestamps()
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
        public void GetDataForMonthGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp, FromTimestamp.AddDays(-31));

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresZerosMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresZerosSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresZerosMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresZerosHoursInTimestamps()
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

        [TestMethod]
        [TestCategory("issue2")]
        public void GetDataForYearGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp, FromTimestamp.AddYears(-1));

            ThenResultIsEmpty();
        }

        private void GivenASignalWith(Granularity granularity)
        {
            signalId = AddNewIntegerSignal(granularity).Id.Value;
        }

        private void GivenNoSignal()
        {
            signalId = 0;
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

        private int signalId;
        private Func<Dto.Datum[]> getData;
    }
}
