using Domain;
using Domain.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class NoneQualityPolicyTests<T> : SpecificValuePolicyTestsBase<T>
    {
        protected override T SpecificValue { get { return default(T); } }

        protected override Quality SpecificQuality { get { return Quality.None; } }

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            SpecificValuePolicyTestsBase<T>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            SpecificValuePolicyTestsBase<T>.ClassCleanup();
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithNoData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Second);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithNoData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithNoData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithNoData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithNoData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithNoData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithNoData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Year, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenASecondSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMinuteSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAHourSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenADaySignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAWeekSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAMonthSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenAYearSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithNoneQuality()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Year, Quality.Good);
        }

        protected override void GivenASignal(Granularity granularity)
        {
            GivenASignalWith(typeof(T).FromNativeType(), granularity);

            WithMissingValuePolicy(new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<T>());
        }
    }

    [TestClass]
    public class NoneQualityPolicyIntTests : NoneQualityPolicyTests<int>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            NoneQualityPolicyTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            NoneQualityPolicyTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class NoneQualityPolicyDecimalTests : NoneQualityPolicyTests<decimal>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            NoneQualityPolicyTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            NoneQualityPolicyTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class NoneQualityPolicyDoubleTests : NoneQualityPolicyTests<double>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            NoneQualityPolicyTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            NoneQualityPolicyTests<double>.ClassCleanup();
        }
    }

    [TestClass]
    public class NoneQualityPolicyBoolTests : NoneQualityPolicyTests<bool>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            NoneQualityPolicyTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            NoneQualityPolicyTests<double>.ClassCleanup();
        }
    }

    [TestClass]
    public class NoneQualityPolicyStringTests : NoneQualityPolicyTests<string>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            NoneQualityPolicyTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            NoneQualityPolicyTests<double>.ClassCleanup();
        }
    }
}
