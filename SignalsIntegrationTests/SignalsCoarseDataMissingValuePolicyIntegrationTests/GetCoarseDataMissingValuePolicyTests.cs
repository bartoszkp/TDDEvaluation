using Domain;
using Domain.MissingValuePolicy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class GetCoarseDataMissingValuePolicyTests<T> : GenericTestBase<T>
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
    }

    [TestClass]
    public class GetCoarseDataMissingValuePolicyIntTests : GetCoarseDataMissingValuePolicyTests<int>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataMissingValuePolicyTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataMissingValuePolicyTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataMissingValuePolicyDecimalTests : GetCoarseDataMissingValuePolicyTests<decimal>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataMissingValuePolicyTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataMissingValuePolicyTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataMissingValuePolicyDoubleTests : GetCoarseDataMissingValuePolicyTests<double>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataMissingValuePolicyTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataMissingValuePolicyTests<double>.ClassCleanup();
        }
    }
}
