using System;
using System.Linq;
using Domain;
using Domain.Infrastructure;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class NoneQualityPolicyTests<T> : MissingValuePolicyTestsBase<T>
    {
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
        [TestCategory("issue6")]
        public void GivenNoData_ReturnsNoneQualityForTheWholeRange()
        {
            ForAllGranularities(granularity
                =>
            {
                GivenNoData();

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumAtBeginning_FillsRemainingRangeWithNoneQuality()
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
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .StartingWith(Value(42), quality));
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumBeforeBeginning_ReturnsNoneQualityForTheWholeRange()
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
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumAtTheEnd_FillsRemainingRangeWithNoneQuality()
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
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .EndingWith(Value(42), quality));
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumAfterTheEnd_ReturnsNoneQualityForTheWholeRange()
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
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
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
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .WithValueAt(Value(42), quality, UniversalMiddleTimestamp(granularity)));
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
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
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
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

                WithMissingValuePolicy(new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<T>());

                test(granularity);
            }
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
