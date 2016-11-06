using Domain;
using Domain.MissingValuePolicy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class GetCoarseDataTests<T> : GenericTestBase<T>
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

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignalWithoutData_WhenReadingWithMinuteGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Second, Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignalWithoutData_WhenReadingWithHourGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Second, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignalWithoutData_WhenReadingWithDayGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Second, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignalWithoutData_WhenReadingWithHourGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Minute, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignalWithoutData_WhenReadingWithDayGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Minute, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignalWithoutData_WhenReadingWithWeekGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Minute, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignalWithoutData_WhenReadingWithMonthGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Minute, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignalWithoutData_WhenReadingWithDayGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Hour, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignalWithoutData_WhenReadingWithWeekGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Hour, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignalWithoutData_WhenReadingWithMonthGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Hour, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignalWithoutData_WhenReadingWithYearGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Hour, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignalWithoutData_WhenReadingWithWeekGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Day, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignalWithoutData_WhenReadingWithMonthGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Day, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignalWithoutData_WhenReadingWithYearGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Day, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthSignalWithoutData_WhenReadingWithYearGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Month, Granularity.Year);
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

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingMinuteData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Second, Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingMinuteData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Second, Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingHourData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Second, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingHourData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Second, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingDayData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Second, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenASecondSignal_WhenReadingDayData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Second, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingHourData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Minute, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingHourData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Minute, Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingDayData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Minute, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingDayData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Minute, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingWeekData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Minute, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingWeekData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Minute, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingMonthData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Minute, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMinuteSignal_WhenReadingMonthData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Minute, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingDayData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Hour, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingDayData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Hour, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingWeekData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Hour, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingWeekData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Hour, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingMonthData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Hour, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingMonthData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Hour, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingYearData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Hour, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingYearData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Hour, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingWeekData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Day, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingWeekData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Day, Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingMonthData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Day, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingMonthData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Day, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingYearData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Day, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingYearData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Day, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthSignal_WhenReadingYearData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Month, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAMonthSignal_WhenReadingYearData_ThenMinimalQualityIsUsedInAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity.Month, Granularity.Year);
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

        private void GivenASignalWithoutData_WhenReadingWithCoarserGranularity_ThenMissingValuePolicyIsUsed(
            Granularity granularity, 
            Granularity coarseGranularity)
        {
            GivenASignal(granularity);
            WithMissingValuePolicy(new SpecificValueMissingValuePolicy<T>() { Value = Value(753), Quality = Quality.Poor });

            WhenReadingCoarseData(coarseGranularity, UniversalBeginTimestamp, UniversalEndTimestamp(coarseGranularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(coarseGranularity), coarseGranularity)
                .WithValue(Value(753)).WithQuality(Quality.Poor));
        }

        private void GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity granularity, Granularity coarseGranularity)
        {
            GivenASignal(granularity);
            GivenData(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(coarseGranularity, 1), granularity)
                .WithValue(Value(15))
                .WithQuality(Quality.Good)
                .StartingWithGoodQualityValue(Value(10))
                .EndingWithGoodQualityValue(Value(20)));
            GivenData(DatumArray<T>
                .ForRange(UniversalBeginTimestamp.AddSteps(coarseGranularity, 1), UniversalBeginTimestamp.AddSteps(coarseGranularity, 2), granularity)
                .WithValue(Value(25))
                .WithQuality(Quality.Good)
                .StartingWithGoodQualityValue(Value(30))
                .EndingWithGoodQualityValue(Value(20)));

            WhenReadingCoarseData(coarseGranularity, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(coarseGranularity, 2));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(coarseGranularity, 2), coarseGranularity)
                .StartingWithGoodQualityValue(Value(15))
                .EndingWithGoodQualityValue(Value(25)));
        }

        private void GivenASignal_WhenReadingCoarseData_ThenMinimalQualityIsUsedInAllSubranges(Granularity granularity, Granularity coarseGranularity)
        {
            GivenASignal(granularity);
            GivenData(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(coarseGranularity, 1), granularity)
                .WithQuality(Quality.Fair)
                .StartingWith(default(T), Quality.Good)
                .EndingWith(default(T), Quality.Poor));
            GivenData(DatumArray<T>
                .ForRange(UniversalBeginTimestamp.AddSteps(coarseGranularity, 1), UniversalBeginTimestamp.AddSteps(coarseGranularity, 2), granularity)
                .WithQuality(Quality.Good)
                .StartingWith(default(T), Quality.Bad)
                .EndingWith(default(T), Quality.Fair));

            WhenReadingCoarseData(coarseGranularity, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(coarseGranularity, 2));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(coarseGranularity, 2), coarseGranularity)
                .StartingWith(default(T), Quality.Poor)
                .EndingWith(default(T), Quality.Bad));
        }
    }

    [TestClass]
    public class GetCoarseDataIntTests : GetCoarseDataTests<int>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataDecimalTests : GetCoarseDataTests<decimal>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataDoubleTests : GetCoarseDataTests<double>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataTests<double>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataStringTests : GenericTestBase<string>
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
        public void GivenAStringSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Day);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }
    }

    [TestClass]
    public class GetCoarseDataBooleanTests : GenericTestBase<bool>
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
        public void GivenABooleanSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Day);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }
    }
}
