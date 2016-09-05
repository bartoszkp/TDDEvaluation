using Domain;
using Dto.Conversions;
using Domain.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class ShadowPolicyTests<T> : ShadowPolicyTestsBase<T>
    {
        protected override T[] ShadowValues { get { return new[] { Value(1410), Value(-1410), Value(0), Value(123), Value(-14) }; } }

        protected override Quality[] ShadowQualities { get { return new[] { Quality.Good, Quality.Fair, Quality.Bad, Quality.Poor, Quality.Fair }; } }

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ShadowPolicyTestsBase<T>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ShadowPolicyTestsBase<T>.ClassCleanup();
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange(Granularity.Second);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange(Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange(Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange(Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange(Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange(Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange(Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Year, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Year, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity.Year, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity.Year, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithShadow()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Year, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenASecondSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMinuteSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAHourSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenADaySignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAWeekSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAMonthSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issueShadowMVPFill")]
        public void GivenAYearSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity.Year, Quality.Good);
        }

        protected override void GivenASignalWithShadow(Granularity granularity)
        {
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var shadow = AddNewSignal(typeof(T).FromNativeType(), granularity)
                .ToDomain<Domain.Signal>();

            var shadowData = Enumerable
                .Zip(ShadowValues, ShadowQualities, (v, q) => new Dto.Datum()
                {
                    Quality = q.ToDto<Dto.Quality>(),
                    Value = v
                })
                .Zip(
                new Infrastructure.TimeEnumerator(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity),
                (d, ts) => new Dto.Datum() { Quality = d.Quality, Value = d.Value, Timestamp = ts });

            client.SetData(shadow.Id.Value, shadowData.ToArray());

            WithMissingValuePolicy(new Domain.MissingValuePolicy.ShadowMissingValuePolicy<T>
            {
                ShadowSignal = shadow
            });
        }
    }

    [TestClass]
    public class ShadowPolicyIntTests : ShadowPolicyTests<int>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ShadowPolicyTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ShadowPolicyTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class ShadowPolicyDecimalTests : ShadowPolicyTests<decimal>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ShadowPolicyTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ShadowPolicyTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class ShadowPolicyDoubleTests : ShadowPolicyTests<double>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ShadowPolicyTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ShadowPolicyTests<double>.ClassCleanup();
        }
    }

    [TestClass]
    public class ShadowPolicyStringTests : ShadowPolicyTests<string>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ShadowPolicyTests<string>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ShadowPolicyTests<string>.ClassCleanup();
        }
    }

    [TestClass]
    public class ShadowPolicyBooleanTests : ShadowPolicyTests<bool>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ShadowPolicyTests<bool>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ShadowPolicyTests<bool>.ClassCleanup();
        }
    }
}
