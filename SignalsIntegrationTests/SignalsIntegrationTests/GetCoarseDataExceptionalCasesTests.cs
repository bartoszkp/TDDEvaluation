using Domain;
using Domain.MissingValuePolicy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class GetCoarseDataExceptionalCasesTests<T> : GenericTestBase<T>
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
        public void GivenAMinuteSignal_WhenReadingWithSecondGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Minute, Granularity.Second);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingWithSecondGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Hour, Granularity.Second);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingWithSecondGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Day, Granularity.Second);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAWeekSignal_WhenReadingWithSecondGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Week, Granularity.Second);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthSignal_WhenReadingWithSecondGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Month, Granularity.Second);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAYearSignal_WhenReadingWithSecondGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Year, Granularity.Second);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingWithMinuteGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Hour, Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingWithMinuteGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Day, Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAWeekSignal_WhenReadingWithMinuteGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Week, Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthSignal_WhenReadingWithMinuteGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Month, Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAYearSignal_WhenReadingWithMinuteGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Year, Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingWithHourGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Day, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAWeekSignal_WhenReadingWithHourGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Week, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthSignal_WhenReadingWithHourGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Month, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAYearSignal_WhenReadingWithHourGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Year, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAWeekSignal_WhenReadingWithDayGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Week, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthSignal_WhenReadingWithDayGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Month, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAYearSignal_WhenReadingWithDayGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Year, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthSignal_WhenReadingWithWeekGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Month, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAYearSignal_WhenReadingWithWeekGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Year, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAYearSignal_WhenReadingWithMonthGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity.Year, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingAsMinuteWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Second, Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingAsMinuteWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Second, Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingAsHourWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Second, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingAsHourWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Second, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingAsDayWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Second, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingAsDayWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Second, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingAsWeekWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Second, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingAsWeekWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Second, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingAsMonthWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Second, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingAsMonthWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Second, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingAsYearWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Second, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingAsYearWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Second, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingAsHourWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Minute, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingAsHourWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Minute, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingAsDayWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Minute, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingAsDayWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Minute, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingAsWeekWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Minute, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingAsWeekWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Minute, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingAsMonthWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Minute, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingAsMonthWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Minute, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingAsYearWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Minute, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingAsYearWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Minute, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingAsDayWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Hour, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingAsDayWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Hour, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingAsWeekWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Hour, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingAsWeekWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Hour, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingAsMonthWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Hour, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingAsMonthWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Hour, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingAsYearWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Hour, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingAsYearWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Hour, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingAsWeekWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Day, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingAsWeekWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Day, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingAsMonthWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Day, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingAsMonthWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Day, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingAsYearWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Day, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingAsYearWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Day, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAWeekSignal_WhenReadingAsMonthWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Week, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAWeekSignal_WhenReadingAsMonthWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Week, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAWeekSignal_WhenReadingAsYearWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Week, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAWeekSignal_WhenReadingAsYearWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Week, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthSignal_WhenReadingAsYearWihtIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Month, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthSignal_WhenReadingAsYearWithIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Month, Granularity.Year);
        }

        private void GivenASignal_WhenReadingWithFinerGranularity_ThenExceptionIsThrown(Granularity granularity, Granularity fineGranularity)
        {
            GivenASignal(granularity);

            Assertions.AssertThrows(() => WhenReadingCoarseData(fineGranularity, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(fineGranularity, 1)));
        }

        private void GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity granularity, Granularity coarseGranularity)
        {
            GivenASignal(granularity);

            Assertions.AssertThrows(() => WhenReadingCoarseData(coarseGranularity, UniversalBeginTimestamp.AddSeconds(1), UniversalEndTimestamp(coarseGranularity)));
        }

        private void GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity granularity, Granularity coarseGranularity)
        {
            GivenASignal(granularity);

            Assertions.AssertThrows(() => WhenReadingCoarseData(coarseGranularity, UniversalBeginTimestamp, UniversalEndTimestamp(coarseGranularity).AddSeconds(1)));
        }
    }

    [TestClass]
    public class GetCoarseDataExceptionalCasesIntTests : GetCoarseDataExceptionalCasesTests<int>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataExceptionalCasesTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataExceptionalCasesTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataExceptionalCasesDecimalTests : GetCoarseDataExceptionalCasesTests<decimal>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataExceptionalCasesTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataExceptionalCasesTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataExceptionalCasesDoubleTests : GetCoarseDataExceptionalCasesTests<double>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataExceptionalCasesTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataExceptionalCasesTests<double>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataExceptionalCasesStringTests : GenericTestBase<string>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GenericTestBase<string>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GenericTestBase<string>.ClassCleanup();
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondStringSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Second);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteStringSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Minute);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourStringSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Hour);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADayStringSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Day);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAWeekStringSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Week);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthStringSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Month);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAYearStringSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Year);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }
    }

    [TestClass]
    public class GetCoarseDataExceptionalCasesBooleanTests : GenericTestBase<bool>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GenericTestBase<bool>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GenericTestBase<bool>.ClassCleanup();
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondBooleanSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Second);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteBooleanSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Minute);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourBooleanSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Hour);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADayBooleanSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Day);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAWeekBooleanSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Week);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthBooleanSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Month);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAYearBooleanSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Year);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }
    }
}
