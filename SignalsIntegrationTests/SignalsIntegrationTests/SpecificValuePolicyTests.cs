using System;
using System.Linq;
using Domain;
using Domain.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class SpecificValuePolicyTests<T> : MissingValuePolicyTestsBase<T>
    {
        private T SpecificValue { get { return Value(1410); } }

        private Quality SpecificQuality { get { return Quality.Fair; } }

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
        public void GivenNoData_ReturnsSpecificValueForTheWholeRange()
        {
            ForAllGranularities(granularity
               =>
            {
                GivenNoData();

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
            });
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenSingleDatumAtBeginning_FillsRemainingRangeWithSpecificValue()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
               =>
            {
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
            });
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenSingleDatumBeforeBeginning_ReturnsSpecificValueForTheWholeRange()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
             =>
            {
                GivenSingleDatum(new Datum<T>()
                {
                    Quality = quality,
                    Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1),
                    Value = Value(42)
                });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
            });
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenSingleDatumAtTheEnd_FillsRemainingRangeWithSpecificValue()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
             =>
            {
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
            });
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenSingleDatumAfterTheEnd_ReturnsSpecificValueForTheWholeRange()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
              =>
            {
                GivenSingleDatum(new Datum<T>()
                {
                    Quality = quality,
                    Timestamp = UniversalEndTimestamp(granularity),
                    Value = Value(42)
                });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
            });
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenSingleDatumInTheMiddle_FillsRemainingRangesWithNoneQuality()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
              =>
            {
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
            });
        }

        [TestMethod]
        [TestCategory("issue8")]
        public void GivenDatumsAtTheBigginingAndInTheMiddle_FillsRemainingRangesWithNoneQuality()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
               =>
            {
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
            });
        }

        private void ForAllGranularitiesAndQualities(Action<Granularity, Quality> test)
        {
            foreach (var quality in Enum.GetValues(typeof(Quality)).Cast<Quality>())
            {
                ForAllGranularities(granularity => test(granularity, quality));
            }
        }

        private void ForAllGranularities(Action<Granularity> test)
        {
            foreach (var granularity in Enum.GetValues(typeof(Granularity)).Cast<Granularity>())
            {
                GivenASignalWith(typeof(T).FromNativeType(), granularity);

                WithMissingValuePolicy(new Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<T>()
                {
                    Quality = SpecificQuality,
                    Value = SpecificValue
                });

                test(granularity);
            }
        }
    }

    [TestClass]
    public class SpecificValuePolicyIntTests : SpecificValuePolicyTests<int>
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
    public class SpecificValuePolicyDecimalTests : SpecificValuePolicyTests<decimal>
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
    public class SpecificValuePolicyDoubleTests : SpecificValuePolicyTests<double>
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

    [TestClass]
    public class SpecificValuePolicyStringTests : SpecificValuePolicyTests<string>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            FirstOrderPolicyTests<string>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            FirstOrderPolicyTests<string>.ClassCleanup();
        }
    }

    [TestClass]
    public class SpecificValuePolicyBooleanTests : SpecificValuePolicyTests<bool>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            FirstOrderPolicyTests<bool>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            FirstOrderPolicyTests<bool>.ClassCleanup();
        }
    }
}
