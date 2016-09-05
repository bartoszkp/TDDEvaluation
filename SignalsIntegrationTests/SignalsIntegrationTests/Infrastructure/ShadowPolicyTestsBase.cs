using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignalsIntegrationTests.Infrastructure
{
    [TestClass]
    public abstract class ShadowPolicyTestsBase<T> : MissingValuePolicyTestsBase<T>
    {
        protected virtual T[] ShadowValues { get; }

        protected virtual Quality[] ShadowQualities { get; }

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

        protected void GivenASignalWithNoData_WhenReadingData_ReturnsShadowValuesForTheWholeRange(Granularity granularity)
        {
            GivenASignalWithShadow(granularity);
            GivenNoData();

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValuesAndQualitiesForRange(ShadowValues, ShadowQualities, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        protected void GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity granularity, Quality quality)
        {
            GivenASignalWithShadow(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Timestamp = UniversalBeginTimestamp,
                Value = Value(42)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValuesAndQualitiesForRange(ShadowValues, ShadowQualities, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(42), quality));
        }

        protected void GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity granularity, Quality quality)
        {
            GivenASignalWithShadow(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1),
                Value = Value(42)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValuesAndQualitiesForRange(ShadowValues, ShadowQualities, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        protected void GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithShadow(Granularity granularity, Quality quality)
        {
            GivenASignalWithShadow(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1),
                Value = Value(42)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValuesAndQualitiesForRange(ShadowValues, ShadowQualities, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(42), quality));
        }

        protected void GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsShadowValuesForWholeRange(Granularity granularity, Quality quality)
        {
            GivenASignalWithShadow(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Timestamp = UniversalEndTimestamp(granularity),
                Value = Value(42)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValuesAndQualitiesForRange(ShadowValues, ShadowQualities, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        protected void GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity granularity, Quality quality)
        {
            GivenASignalWithShadow(granularity);
            GivenSingleDatum(new Datum<T>()
            {
                Quality = quality,
                Timestamp = UniversalMiddleTimestamp(granularity),
                Value = Value(42)
            });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValuesAndQualitiesForRange(ShadowValues, ShadowQualities, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(42), quality, UniversalMiddleTimestamp(granularity)));
        }

        protected void GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithShadow(Granularity granularity, Quality quality)
        {
            GivenASignalWithShadow(granularity);
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
                .WithSpecificValuesAndQualitiesForRange(ShadowValues, ShadowQualities, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), OtherThan(quality))
                .WithValueAt(Value(42), quality, UniversalMiddleTimestamp(granularity)));
        }

        protected abstract void GivenASignalWithShadow(Granularity granularity);
    }
}
