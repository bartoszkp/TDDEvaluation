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
        public void GetDataUsingIncompleteSignalsThrows()
        {
            GivenNoSignal();

            WhenGettigData(FromTimestamp, ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForSecondGranularityRequiresZerosMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Second);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForSecondGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Second);

            WhenGettigData(FromTimestamp, FromTimestamp.AddSeconds(-1));

            ResultIsEmpty();
        }

        [TestMethod]
        public void GetDataForMinuteGranularityRequiresZerosMillisecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Minute);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForMinuteGranularityRequiresZerosSecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Minute);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForMinuteGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Minute);

            WhenGettigData(FromTimestamp, FromTimestamp.AddMinutes(-1));

            ResultIsEmpty();
        }

        [TestMethod]
        public void GetDataForHourGranularityRequiresZerosMillisecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Hour);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForHourGranularityRequiresZerosSecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Hour);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForHourGranularityRequiresZeroMinutesInFromTimestamp()
        {
            GivenASignalWith(Granularity.Hour);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForHourGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Hour);

            WhenGettigData(FromTimestamp, FromTimestamp.AddHours(-1));

            ResultIsEmpty();
        }

        [TestMethod]
        public void GetDataForDayGranularityRequiresZeroMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForDayGranularityRequiresZeroSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForDayGranularityRequiresZeroMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForDayGranularityRequiresZerosHoursInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForDayGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp, FromTimestamp.AddDays(-1));

            ResultIsEmpty();
        }

        [TestMethod]
        public void GetDataForWeekGranularityRequiresZerosMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForWeekGranularityRequiresZerosSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForWeekGranularityRequiresZerosMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForWeekGranularityRequiresZerosHoursInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForWeekGranularityRequiresMondayInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddDays(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForWeekGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp, FromTimestamp.AddDays(-7));

            ResultIsEmpty();
        }

        [TestMethod]
        public void GetDataForMonthGranularityRequiresZerosMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForMonthGranularityRequiresZerosSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForMonthGranularityRequiresZerosMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForMonthGranularityRequiresZerosHoursInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForMonthGranularityRequiresFirstDayOfMonthInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddDays(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForMonthGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp, FromTimestamp.AddDays(-31));

            ResultIsEmpty();
        }

        [TestMethod]
        public void GetDataForYearGranularityRequiresZerosMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForYearGranularityRequiresZerosSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForYearGranularityRequiresZerosMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void SetDataForYearGranularityRequiresZerosHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForYearGranularityRequiresFirstDayOfMonthInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddDays(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForYearGranularityRequiresFirstMonthInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddMonths(1), ToTimestamp);

            ResultThrows();
        }

        [TestMethod]
        public void GetDataForYearGranularityReturnsEmptyForReversedTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp, FromTimestamp.AddYears(-1));

            ResultIsEmpty();
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

        private void ResultThrows()
        {
            Assertions.AssertThrows(() => getData());
        }

        private void ResultIsEmpty()
        {
            var result = getData();

            Assert.IsFalse(result.Any());
        }

        private int signalId;
        private Func<Dto.Datum[]> getData;
    }
}
