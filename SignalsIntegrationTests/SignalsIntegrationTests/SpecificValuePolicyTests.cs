using Domain;
using Domain.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class SpecificValuePolicyTests<T> : MissingValuePolicyTestsBase<T>
    {
        protected virtual T SpecificValue { get { return Value(1410); } }

        protected virtual Quality SpecificQuality { get { return Quality.Fair; } }

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            MissingValuePolicyTestsBase<T>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            MissingValuePolicyTestsBase<T>.ClassCleanup();
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Second);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity.Year);
        }

        protected void GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity granularity)
        {
            GivenASignal(granularity);
            GivenNoData();

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleBadDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSinglePoorDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleFairDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleGoodDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Good);
        }

        protected void GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Timestamp = UniversalBeginTimestamp,
                Value = Value(42)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(42), quality));
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Good);
        }

        protected void GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1),
                Value = Value(42)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleBadDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSinglePoorDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleFairDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleGoodDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Good);
        }

        protected void GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1),
                Value = Value(42)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(42), quality));
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity.Year, Quality.Good);
        }

        protected void GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Timestamp = UniversalEndTimestamp(granularity),
                Value = Value(42)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleBadDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSinglePoorDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleFairDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithSingleGoodDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity.Year, Quality.Good);
        }

        protected void GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Timestamp = UniversalMiddleTimestamp(granularity),
                Value = Value(42)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(42), quality, UniversalMiddleTimestamp(granularity)));
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenASecondSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMinuteSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAHourSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenADaySignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAWeekSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAMonthSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithBadDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithPoorDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithFairDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenAYearSignalWithGoodDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue()
        {
            GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity.Year, Quality.Good);
        }

        protected void GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenData(
                new Datum<T>()
                {
                    Quality = OtherThan(quality),
                    Timestamp = UniversalBeginTimestamp,
                    Value = Value(1410)
                },
                new Datum<T>()
                {
                    Quality = quality,
                    Timestamp = UniversalMiddleTimestamp(granularity),
                    Value = Value(42)
                });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), OtherThan(quality))
                .WithValueAt(Value(42), quality, UniversalMiddleTimestamp(granularity)));
        }

        protected virtual void GivenASignal(Granularity granularity)
        {
            GivenASignalWith(typeof(T).FromNativeType(), granularity);

            WithMissingValuePolicy(new Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<T>()
            {
                Quality = SpecificQuality,
                Value = SpecificValue
            });
        }
    }

    [TestClass]
    public class SpecificValuePolicyIntTests : SpecificValuePolicyTests<int>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            SpecificValuePolicyTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            SpecificValuePolicyTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class SpecificValuePolicyDecimalTests : SpecificValuePolicyTests<decimal>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            SpecificValuePolicyTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            SpecificValuePolicyTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class SpecificValuePolicyDoubleTests : SpecificValuePolicyTests<double>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            SpecificValuePolicyTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            SpecificValuePolicyTests<double>.ClassCleanup();
        }
    }

    [TestClass]
    public class SpecificValuePolicyStringTests : SpecificValuePolicyTests<string>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            SpecificValuePolicyTests<string>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            SpecificValuePolicyTests<string>.ClassCleanup();
        }
    }

    [TestClass]
    public class SpecificValuePolicyBooleanTests : SpecificValuePolicyTests<bool>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            SpecificValuePolicyTests<bool>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            SpecificValuePolicyTests<bool>.ClassCleanup();
        }
    }
}
