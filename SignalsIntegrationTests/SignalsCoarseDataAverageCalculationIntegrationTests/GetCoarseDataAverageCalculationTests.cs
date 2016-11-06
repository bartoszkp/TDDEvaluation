using Domain;
using Domain.MissingValuePolicy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class GetCoarseDataAverageCalculationTests<T> : GenericTestBase<T>
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
    public class GetCoarseDataAverageCalculationIntTests : GetCoarseDataAverageCalculationTests<int>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataAverageCalculationTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataAverageCalculationTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataAverageCalculationDecimalTests : GetCoarseDataAverageCalculationTests<decimal>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataAverageCalculationTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataAverageCalculationTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataAverageCalculationDoubleTests : GetCoarseDataAverageCalculationTests<double>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataAverageCalculationTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataAverageCalculationTests<double>.ClassCleanup();
        }
    }
}
