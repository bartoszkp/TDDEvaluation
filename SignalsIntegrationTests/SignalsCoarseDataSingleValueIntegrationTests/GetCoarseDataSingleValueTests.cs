using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class GetCoarseDataSingleValueTests<T> : GenericTestBase<T>
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
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignalWithSingleValueForTheWholeRange_WhenReadingWithMinuteGranularity_SameSingleValueIsReturnedEachMinute()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Second, Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignalWithSingleValueForTheWholeRange_WhenReadingWithHourGranularity_SameSingleValueIsReturnedEachHour()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Second, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignalWithSingleValueForTheWholeRange_WhenReadingWithDayGranularity_SameSingleValueIsReturnedEachDay()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Second, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignalWithSingleValueForTheWholeRange_WhenReadingWithWeekGranularity_SameSingleValueIsReturnedEachWeek()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Second, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignalWithSingleValueForTheWholeRange_WhenReadingWithHourGranularity_SameSingleValueIsReturnedEachHour()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Minute, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignalWithSingleValueForTheWholeRange_WhenReadingWithDayGranularity_SameSingleValueIsReturnedEachDay()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Minute, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignalWithSingleValueForTheWholeRange_WhenReadingWithWeekGranularity_SameSingleValueIsReturnedEachWeek()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Minute, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignalWithSingleValueForTheWholeRange_WhenReadingWithMonthGranularity_SameSingleValueIsReturnedEachMonth()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Minute, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignalWithSingleValueForTheWholeRange_WhenReadingWithDayGranularity_SameSingleValueIsReturnedEachDay()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Hour, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignalWithSingleValueForTheWholeRange_WhenReadingWithWeekGranularity_SameSingleValueIsReturnedEachWeek()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Hour, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignalWithSingleValueForTheWholeRange_WhenReadingWithMonthGranularity_SameSingleValueIsReturnedEachMonth()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Hour, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignalWithSingleValueForTheWholeRange_WhenReadingWithYearGranularity_SameSingleValueIsReturnedEachYear()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Hour, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignalWithSingleValueForTheWholeRange_WhenReadingWithWeekGranularity_SameSingleValueIsReturnedEachWeek()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Day, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignalWithSingleValueForTheWholeRange_WhenReadingWithMonthGranularity_SameSingleValueIsReturnedEachMonth()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Day, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignalWithSingleValueForTheWholeRange_WhenReadingWithYearGranularity_SameSingleValueIsReturnedEachYear()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Day, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthSignalWithSingleValueForTheWholeRange_WhenReadingWithYearGranularity_SameSingleValueIsReturnedEachYear()
        {
            GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Month, Granularity.Year);
        }

        private void GivenASignalWithSingleValueForTheWholeRange_WhenReadingWithCoarserGranularity_SameSingleValueIsReturned(
            Granularity granularity,
            Granularity coarseGranularity)
        {
            GivenASignal(granularity);
            GivenData(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(coarseGranularity, 2), granularity)
                .WithValue(Value(42)).WithQuality(Quality.Fair));

            WhenReadingCoarseData(coarseGranularity, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(coarseGranularity, 2));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(coarseGranularity, 2), coarseGranularity)
                .WithValue(Value(42)).WithQuality(Quality.Fair));
        }
    }

    [TestClass]
    public class GetCoarseDataSingleValueIntTests : GetCoarseDataSingleValueTests<int>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataSingleValueTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataSingleValueTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataSingleValueDecimalTests : GetCoarseDataSingleValueTests<decimal>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataSingleValueTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataSingleValueTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataSingleValueDoubleTests : GetCoarseDataSingleValueTests<double>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataSingleValueTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataSingleValueTests<double>.ClassCleanup();
        }
    }
}
