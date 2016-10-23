using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class ZeroOrderPolicyTests<T> : GenericTestBase<T>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GenericTestBase<T>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GenericTestBase<T>.ClassCleanup();
        }

        protected override void GivenASignal(Granularity granularity)
        {
            base.GivenASignal(granularity);

            WithMissingValuePolicy(new Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<T>());
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Second);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Minute);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Hour);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Week);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Year);
        }

        private void GivenASignalWithNoData_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity granularity)
        {
            GivenASignal(granularity);
            GivenNoData();

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleBadDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSinglePoorDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleFairDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleGoodDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleBadDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSinglePoorDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleFairDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleGoodDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleBadDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSinglePoorDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleFairDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleGoodDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleBadDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSinglePoorDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleFairDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleGoodDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleBadDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSinglePoorDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleFairDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleGoodDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleBadDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSinglePoorDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleFairDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleGoodDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleBadDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSinglePoorDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleFairDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleGoodDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue()
        {
            GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity.Year, Quality.Good);
        }

        private void GivenASignalWithSingleDatumAtBeginnig_WhenReadingData_RemainingRangeIsFilledWithDatumsValue(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Value = Value(1410),
                Timestamp = UniversalBeginTimestamp,
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(1410), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumAfterBeginning_FillsRemainingRangeWithThatValue()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;

            GivenASignal(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Value = Value(1410),
                Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(1410), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithNoneQuality());
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleBadDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSinglePoorDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleFairDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleGoodDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleBadDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSinglePoorDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleFairDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleGoodDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleBadDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSinglePoorDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleFairDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleGoodDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleBadDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSinglePoorDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleFairDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleGoodDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleBadDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSinglePoorDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleFairDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleGoodDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleBadDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSinglePoorDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleFairDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleGoodDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleBadDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSinglePoorDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleFairDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleGoodDatumAtEnd_WhenReadingData_ReturnsThatSingleValue()
        {
            GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity.Year, Quality.Good);
        }

        private void GivenASignalWithSingleDatumAtEnd_WhenReadingData_ReturnsThatSingleValue(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Value = Value(1410),
                Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleBadDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSinglePoorDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleFairDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleGoodDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange()
        {
            GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity.Year, Quality.Good);
        }

        private void GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsNoneQualityForWholeRange(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Value = Value(1410),
                Timestamp = UniversalEndTimestamp(granularity)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleBadDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSinglePoorDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleFairDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleGoodDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleBadDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSinglePoorDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleFairDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleGoodDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleBadDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSinglePoorDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleFairDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleGoodDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleBadDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSinglePoorDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleFairDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleGoodDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleBadDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSinglePoorDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleFairDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleGoodDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleBadDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSinglePoorDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleFairDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleGoodDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleBadDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSinglePoorDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleFairDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleGoodDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter()
        {
            GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity.Year, Quality.Good);
        }

        private void GivenASignalWithSingleDatumInMiddle_WhenReadingData_ReturnsNoneBeforeMiddleAndGivenValueAfter(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Value = Value(1410),
                Timestamp = UniversalMiddleTimestamp(granularity)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalMiddleTimestamp(granularity), granularity)
                .FollowedBy(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(1410), quality, UniversalMiddleTimestamp(granularity), UniversalEndTimestamp(granularity), granularity)));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithBadDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithPoorDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithFairDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithGoodDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithBadDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithPoorDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithFairDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithGoodDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithBadDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithPoorDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithFairDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithGoodDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithBadDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithPoorDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithFairDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithGoodDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithBadDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithPoorDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithFairDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithGoodDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithBadDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithPoorDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithFairDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithGoodDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithBadDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithPoorDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithFairDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithGoodDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity.Year, Quality.Good);
        }

        private void GivenASignalWithDatumAtBeginningAndDifferentInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndSecondValueAfter(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenData(new Datum<T>() { Quality = OtherThan(quality), Value = Value(753), Timestamp = UniversalBeginTimestamp },
                      new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalMiddleTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(753), OtherThan(quality), UniversalBeginTimestamp, UniversalMiddleTimestamp(granularity), granularity)
                .FollowedBy(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(1410), quality, UniversalMiddleTimestamp(granularity), UniversalEndTimestamp(granularity), granularity)));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithBadDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithPoorDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithFairDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithGoodDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithBadDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithPoorDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithFairDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithGoodDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithBadDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithPoorDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithFairDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithGoodDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithBadDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithPoorDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithFairDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithGoodDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithBadDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithPoorDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithFairDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithGoodDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithBadDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithPoorDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithFairDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithGoodDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithBadDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithPoorDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithFairDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithGoodDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter()
        {
            GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity.Year, Quality.Good);
        }

        private void GivenASignalWithDatumAtBeginningAndNoneInMiddle_WhenReadingData_ReturnsFirstValueBeforeMiddleAndNoneAfter(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenData(new Datum<T>() { Quality = quality, Value = Value(753), Timestamp = UniversalBeginTimestamp },
                      new Datum<T>() { Quality = Quality.None, Timestamp = UniversalMiddleTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(753), quality, UniversalBeginTimestamp, UniversalMiddleTimestamp(granularity), granularity)
                .FollowedBy(DatumArray<T>
                    .WithNoneQualityForRange(UniversalMiddleTimestamp(granularity), UniversalEndTimestamp(granularity), granularity)));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Second, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Second, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Second, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenASecondSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Second, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Minute, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Minute, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Minute, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMinuteSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Minute, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Hour, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Hour, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Hour, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAHourSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Hour, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Day, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Day, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Day, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenADaySignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Day, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Week, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Week, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Week, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAWeekSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Week, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Month, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Month, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Month, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAMonthSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Month, Quality.Good);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleBadDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Year, Quality.Bad);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSinglePoorDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Year, Quality.Poor);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleFairDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Year, Quality.Fair);
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenAYearSignalWithSingleGoodDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange()
        {
            GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity.Year, Quality.Good);
        }

        private void GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsItsValueForWholeRange(Granularity granularity, Quality quality)
        {
            GivenASignal(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Value = Value(1410),
                Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -10)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(1410), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }
    }

    [TestClass]
    public class ZeroOrderPolicyIntTests : ZeroOrderPolicyTests<int>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ZeroOrderPolicyTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ZeroOrderPolicyTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class ZeroOrderPolicyDecimalTests : ZeroOrderPolicyTests<decimal>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ZeroOrderPolicyTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ZeroOrderPolicyTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class ZeroOrderPolicyDoubleTests : ZeroOrderPolicyTests<double>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ZeroOrderPolicyTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ZeroOrderPolicyTests<double>.ClassCleanup();
        }
    }

    [TestClass]
    public class ZeroOrderPolicyBoolTests : ZeroOrderPolicyTests<bool>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ZeroOrderPolicyTests<bool>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ZeroOrderPolicyTests<bool>.ClassCleanup();
        }
    }

    [TestClass]
    public class ZeroOrderPolicyStringTests : ZeroOrderPolicyTests<string>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ZeroOrderPolicyTests<string>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ZeroOrderPolicyTests<string>.ClassCleanup();
        }
    }
}
