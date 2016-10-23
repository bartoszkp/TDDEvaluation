using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignalsIntegrationTests.Infrastructure
{
    [TestClass]
    public abstract class SpecificValuePolicyTestsBase<T> : GenericTestBase<T>
    {
        protected virtual T SpecificValue { get; }

        protected virtual Quality SpecificQuality { get; }

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

        protected void GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(Granularity granularity)
        {
            GivenASignal(granularity);
            GivenNoData();

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
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

        protected void GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(Granularity granularity, Quality quality)
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
    }
}
