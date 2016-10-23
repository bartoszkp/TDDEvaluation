using System;
using System.Linq;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class FirstOrderPolicyTests<T> : GenericTestBase<T>
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

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithoutData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenNoData();

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithoutData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenNoData();

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithoutData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenNoData();

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithoutData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenNoData();

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithoutData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenNoData();

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithoutData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenNoData();

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithoutData_WhenReadingData_ReturnsNoneQualityForTheWholeRange()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenNoData();

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithSingleBadDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithSinglePoorDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithSingleFairDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithSingleGoodDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithSingleBadDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithSinglePoorDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithSingleFairDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithSingleGoodDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithSingleBadDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithSinglePoorDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithSingleFairDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithSingleGoodDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithSingleBadDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithSinglePoorDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithSingleFairDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithSingleGoodDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithSingleBadDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithSinglePoorDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithSingleFairDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithSingleGoodDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithSingleBadDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithSinglePoorDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithSingleFairDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithSingleGoodDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithSingleBadDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithSinglePoorDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithSingleFairDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithSingleGoodDatumAtBeginning_WhenReadingData_FillsRemainingRangeWithNone()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(1410), quality));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithSingleBadDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithSinglePoorDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithSingleFairDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithSingleGoodDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithSingleBadDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithSinglePoorDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithSingleFairDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithSingleGoodDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithSingleBadDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithSinglePoorDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithSingleFairDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithSingleGoodDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithSingleBadDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithSinglePoorDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithSingleFairDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithSingleGoodDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithSingleBadDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithSinglePoorDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithSingleFairDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithSingleGoodDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithSingleBadDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithSinglePoorDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithSingleFairDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithSingleGoodDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithSingleBadDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithSinglePoorDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithSingleFairDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithSingleGoodDatumAfterBeginning_WhenReadingData_FillsRestOfRangeWithNone()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenSingleDatum(new Datum<T>() { Quality = quality, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(1410), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithBadDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithPoorDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithFairDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithGoodDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithBadDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithPoorDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithFairDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithGoodDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithBadDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithPoorDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithFairDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithGoodDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithBadDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithPoorDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithFairDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithGoodDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithBadDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithPoorDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithFairDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithGoodDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithBadDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithPoorDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithFairDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithGoodDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithBadDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithPoorDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithFairDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithGoodDatumsAtBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), quality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithBadDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithPoorDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithFairDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithGoodDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithBadDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithPoorDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithFairDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithGoodDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithBadDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithPoorDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithFairDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithGoodDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithBadDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithPoorDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithFairDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithGoodDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithBadDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithPoorDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithFairDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithGoodDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithBadDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithPoorDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithFairDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithGoodDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithBadDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithPoorDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithFairDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithGoodDatumsAfterBeginningAndBeforeEnd_WhenReadingData_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithBadDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithPoorDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithFairDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithGoodDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithBadDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithPoorDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithFairDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithGoodDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithBadDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithPoorDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithFairDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithGoodDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithBadDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithPoorDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithFairDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithGoodDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithBadDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithPoorDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithFairDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithGoodDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithBadDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithPoorDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithFairDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithGoodDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithBadDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithPoorDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithFairDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithGoodDatumsBeforeAndAfterBeginning_WhenReadingData_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp)
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithBadDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithPoorDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithFairDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithGoodDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithBadDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithPoorDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithFairDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithGoodDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithBadDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithPoorDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithFairDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithGoodDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithBadDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithPoorDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithFairDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithGoodDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithBadDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithPoorDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithFairDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithGoodDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithBadDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithPoorDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithFairDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithGoodDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithBadDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithPoorDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithFairDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithGoodDatumsAtBeginningAndAtEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithBadDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithPoorDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithFairDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithGoodDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithBadDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithPoorDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithFairDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithGoodDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithBadDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithPoorDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithFairDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithGoodDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithBadDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithPoorDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithFairDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithGoodDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithBadDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithPoorDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithFairDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithGoodDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithBadDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithPoorDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithFairDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithGoodDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithBadDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithPoorDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithFairDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithGoodDatumsAfterBeginningAndAfterEnd_WhenReadingData_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                new Datum<T>() { Quality = quality, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(15), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(25), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithBadDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithPoorDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithFairDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithGoodDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithBadDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithPoorDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithFairDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithGoodDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithBadDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithPoorDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithFairDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithGoodDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithBadDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithPoorDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithFairDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithGoodDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithBadDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithPoorDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithFairDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithGoodDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithBadDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithPoorDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithFairDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithGoodDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithBadDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithPoorDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithFairDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithGoodDatumsBeforeBeginningAndAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithTwoBadDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithTwoPoorDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithTwoFairDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithTwoGoodDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithTwoBadDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithTwoPoorDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithTwoFairDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithTwoGoodDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithTwoBadDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithTwoPoorDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithTwoFairDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithTwoGoodDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithTwoBadDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithTwoPoorDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithTwoFairDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithTwoGoodDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithTwoBadDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithTwoPoorDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithTwoFairDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithTwoGoodDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithTwoBadDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithTwoPoorDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithTwoFairDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithTwoGoodDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithTwoBadDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithTwoPoorDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithTwoFairDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithTwoGoodDatumsBeforeBeginningAndOneAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(20), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithSingleBadDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithSinglePoorDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithSingleFairDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithSingleGoodDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithSingleBadDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithSinglePoorDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithSingleFairDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithSingleGoodDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithSingleBadDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithSinglePoorDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithSingleFairDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithSingleGoodDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithSingleBadDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithSinglePoorDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithSingleFairDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithSingleGoodDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithSingleBadDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithSinglePoorDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithSingleFairDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithSingleGoodDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithSingleBadDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithSinglePoorDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithSingleFairDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithSingleGoodDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithSingleBadDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithSinglePoorDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithSingleFairDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithSingleGoodDatumBeforeBeginningAndTwoAfterEnd_WhenReadingData_InterpolatesForTheWholeRange()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                new Datum<T>() { Quality = quality, Value = Value(40), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 2) },
                new Datum<T>() { Quality = quality, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(40), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(50), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(60), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithThreeBadDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithThreePoorDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithThreeFairDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithThreeGoodDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithThreeBadDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithThreePoorDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithThreeFairDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithThreeGoodDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithThreeBadDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithThreePoorDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithThreeFairDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithThreeGoodDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithThreeBadDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithThreePoorDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithThreeFairDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithThreeGoodDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithThreeBadDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithThreePoorDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithThreeFairDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithThreeGoodDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithThreeBadDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithThreePoorDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithThreeFairDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithThreeGoodDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithThreeBadDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithThreePoorDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithThreeFairDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithThreeGoodDatums_WhenReadingData_ProperlyChangesInterpolation()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = quality, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                new Datum<T>() { Quality = quality, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 0))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 1))
                .WithValueAt(Value(30), quality, UniversalBeginTimestamp.AddSteps(granularity, 2))
                .WithValueAt(Value(20), quality, UniversalBeginTimestamp.AddSteps(granularity, 3))
                .WithValueAt(Value(10), quality, UniversalBeginTimestamp.AddSteps(granularity, 4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithDatumsWithGoodAndFairQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithDatumsWithGoodAndFairQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithDatumsWithGoodAndFairQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithDatumsWithGoodAndFairQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithDatumsWithGoodAndFairQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithDatumsWithGoodAndFairQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithDatumsWithGoodAndFairQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithDatumsWithFairAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                  new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithDatumsWithFairAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                  new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithDatumsWithFairAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                  new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithDatumsWithFairAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                  new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithDatumsWithFairAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                  new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithDatumsWithFairAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                  new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithDatumsWithFairAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                  new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithDatumsWithGoodAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithDatumsWithGoodAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithDatumsWithGoodAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithDatumsWithGoodAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithDatumsWithGoodAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithDatumsWithGoodAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithDatumsWithGoodAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithDatumsWithPoorAndGoodQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithDatumsWithPoorAndGoodQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithDatumsWithPoorAndGoodQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithDatumsWithPoorAndGoodQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithDatumsWithPoorAndGoodQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithDatumsWithPoorAndGoodQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithDatumsWithPoorAndGoodQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithDatumsWithGoodAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithDatumsWithGoodAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithDatumsWithGoodAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithDatumsWithGoodAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithDatumsWithGoodAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithDatumsWithGoodAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithDatumsWithGoodAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithDatumsWithBadAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithDatumsWithBadAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithDatumsWithBadAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithDatumsWithBadAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithDatumsWithBadAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithDatumsWithBadAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithDatumsWithBadAndGoodQualities_WhenReadingData_InterpolatedValuesHaveFairQuality()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithDatumsWithFairAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithDatumsWithFairAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithDatumsWithFairAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithDatumsWithFairAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithDatumsWithFairAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithDatumsWithFairAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithDatumsWithFairAndPoorQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithDatumsWithPoorAndFairQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithDatumsWithPoorAndFairQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithDatumsWithPoorAndFairQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithDatumsWithPoorAndFairQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithDatumsWithPoorAndFairQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithDatumsWithPoorAndFairQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithDatumsWithPoorAndFairQualities_WhenReadingData_InterpolatedValuesHavePoorQuality()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithDatumsWithFairAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                      new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithDatumsWithFairAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                      new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithDatumsWithFairAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                      new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithDatumsWithFairAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                      new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithDatumsWithFairAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                      new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithDatumsWithFairAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                      new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithDatumsWithFairAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                      new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithDatumsWithBadAndFairQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithDatumsWithBadAndFairQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithDatumsWithBadAndFairQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithDatumsWithBadAndFairQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithDatumsWithBadAndFairQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithDatumsWithBadAndFairQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithDatumsWithBadAndFairQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithDatumsWithPoorAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithDatumsWithPoorAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithDatumsWithPoorAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithDatumsWithPoorAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithDatumsWithPoorAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithDatumsWithPoorAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithDatumsWithPoorAndBadQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .StartingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenASecondSignalWithDatumsWithBadAndPoorQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMinuteSignalWithDatumsWithBadAndPoorQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAHourSignalWithDatumsWithBadAndPoorQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenADaySignalWithDatumsWithBadAndPoorQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAWeekSignalWithDatumsWithBadAndPoorQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAMonthSignalWithDatumsWithBadAndPoorQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenAYearSignalWithDatumsWithBadAndPoorQualities_WhenReadingData_InterpolatedValuesHaveBadQuality()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
            GivenData(
                new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

            WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                .EndingWith(Value(10), Quality.Poor));
        }
    }

    [TestClass]
    public class FirstOrderPolicyIntTests : FirstOrderPolicyTests<int>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            FirstOrderPolicyTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            FirstOrderPolicyTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class FirstOrderPolicyDecimalTests : FirstOrderPolicyTests<decimal>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            FirstOrderPolicyTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            FirstOrderPolicyTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class FirstOrderPolicyDoubleTests : FirstOrderPolicyTests<double>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            FirstOrderPolicyTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            FirstOrderPolicyTests<double>.ClassCleanup();
        }
    }
}
