using System;
using System.Linq;
using Domain;
using Domain.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class FirstOrderPolicyTests<T> : MissingValuePolicyTestsBase<T>
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
        [TestCategory("issue11")]
        public void GivenNoData_ReturnsNoneQualityForTheWholeRange()
        {
            ForAllGranularities((granularity)
                =>
            {
                GivenNoData();

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenSingleDatumAtBegining_FillsRestOfRangeWithNone()
        {
            ForAllGranularities((granularity)
                =>
            {
                GivenSingleDatum(new Datum<T>() { Quality = Quality.Good, Value = Value(1410), Timestamp = UniversalBeginTimestamp });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .StartingWithGoodQualityValue(Value(1410)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenSingleDatumAfterBegining_FillsRestOfRangeWithNone()
        {
            ForAllGranularities((granularity)
               =>
            {
                GivenSingleDatum(new Datum<T>() { Quality = Quality.Good, Value = Value(1410), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .WithSingleGoodQualityValueAt(Value(1410), UniversalBeginTimestamp.AddSteps(granularity, 1)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsAtBeginingAndBeforeEnd_InterpolatesValueForTheWholeRange()
        {
            ForAllGranularities((granularity)
              =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Good, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Good, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .WithValueAt(Value(15), UniversalBeginTimestamp.AddSteps(granularity, 1))
                    .WithValueAt(Value(20), UniversalBeginTimestamp.AddSteps(granularity, 2))
                    .WithValueAt(Value(25), UniversalBeginTimestamp.AddSteps(granularity, 3))
                    .WithValueAt(Value(30), UniversalBeginTimestamp.AddSteps(granularity, 4)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsAfterBeginingAndBeforeEnd_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            ForAllGranularities((granularity)
              =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                    new Datum<T>() { Quality = Quality.Good, Value = Value(30), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -2) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .WithGoodQualityValueAt(Value(10), UniversalBeginTimestamp.AddSteps(granularity, 1))
                    .WithGoodQualityValueAt(Value(20), UniversalBeginTimestamp.AddSteps(granularity, 2))
                    .WithGoodQualityValueAt(Value(30), UniversalBeginTimestamp.AddSteps(granularity, 3)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsBeforeAndAfterBegining_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            ForAllGranularities((granularity)
              =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                    new Datum<T>() { Quality = Quality.Good, Value = Value(30), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .WithGoodQualityValueAt(Value(20), UniversalBeginTimestamp)
                    .WithGoodQualityValueAt(Value(30), UniversalBeginTimestamp.AddSteps(granularity, 1)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsAtBeginingAndAtEnd_InterpolatesForTheWholeRange()
        {
            ForAllGranularities((granularity)
               =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Good, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .WithGoodQualityValueAt(Value(10), UniversalBeginTimestamp.AddSteps(granularity, 0))
                    .WithGoodQualityValueAt(Value(15), UniversalBeginTimestamp.AddSteps(granularity, 1))
                    .WithGoodQualityValueAt(Value(20), UniversalBeginTimestamp.AddSteps(granularity, 2))
                    .WithGoodQualityValueAt(Value(25), UniversalBeginTimestamp.AddSteps(granularity, 3))
                    .WithGoodQualityValueAt(Value(30), UniversalBeginTimestamp.AddSteps(granularity, 4)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsAfterBeginingAndAfterEnd_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            ForAllGranularities((granularity)
               =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1) },
                    new Datum<T>() { Quality = Quality.Good, Value = Value(35), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .WithGoodQualityValueAt(Value(10), UniversalBeginTimestamp.AddSteps(granularity, 1))
                    .WithGoodQualityValueAt(Value(15), UniversalBeginTimestamp.AddSteps(granularity, 2))
                    .WithGoodQualityValueAt(Value(20), UniversalBeginTimestamp.AddSteps(granularity, 3))
                    .WithGoodQualityValueAt(Value(25), UniversalBeginTimestamp.AddSteps(granularity, 4)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsBeforeBeginingAndAfterEnd_InterpolatesForTheWholeRange()
        {
            ForAllGranularities((granularity)
                =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) },
                    new Datum<T>() { Quality = Quality.Good, Value = Value(80), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, 1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .WithGoodQualityValueAt(Value(20), UniversalBeginTimestamp.AddSteps(granularity, 0))
                    .WithGoodQualityValueAt(Value(30), UniversalBeginTimestamp.AddSteps(granularity, 1))
                    .WithGoodQualityValueAt(Value(40), UniversalBeginTimestamp.AddSteps(granularity, 2))
                    .WithGoodQualityValueAt(Value(50), UniversalBeginTimestamp.AddSteps(granularity, 3))
                    .WithGoodQualityValueAt(Value(60), UniversalBeginTimestamp.AddSteps(granularity, 4)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenThreeDatums_ProperlyChangesInterpolation()
        {
            ForAllGranularities((granularity)
                =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Good, Value = Value(30), Timestamp = UniversalMiddleTimestamp(granularity) },
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .WithGoodQualityValueAt(Value(10), UniversalBeginTimestamp.AddSteps(granularity, 0))
                    .WithGoodQualityValueAt(Value(20), UniversalBeginTimestamp.AddSteps(granularity, 1))
                    .WithGoodQualityValueAt(Value(30), UniversalBeginTimestamp.AddSteps(granularity, 2))
                    .WithGoodQualityValueAt(Value(20), UniversalBeginTimestamp.AddSteps(granularity, 3))
                    .WithGoodQualityValueAt(Value(10), UniversalBeginTimestamp.AddSteps(granularity, 4)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithGoodAndFairQualities_InterpolatedValuesHaveFairQuality()
        {
            ForAllGranularities((granularity)
                =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .StartingWithGoodQualityValue(Value(10)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithFairAndGoodQualities_InterpolatedValuesHaveFairQuality()
        {
            ForAllGranularities((granularity)
               =>
            {
                GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                      new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .EndingWithGoodQualityValue(Value(10)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithGoodAndPoorQualities_InterpolatedValuesHavePoorQuality()
        {
            ForAllGranularities((granularity)
               =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .StartingWithGoodQualityValue(Value(10)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithPoorAndGoodQualities_InterpolatedValuesHavePoorQuality()
        {
            ForAllGranularities((granularity)
               =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .EndingWithGoodQualityValue(Value(10)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithGoodAndBadQualities_InterpolatedValuesHaveBadQuality()
        {
            ForAllGranularities((granularity)
               =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .StartingWithGoodQualityValue(Value(10)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithBadAndGoodQualities_InterpolatedValuesHaveFairQuality()
        {
            ForAllGranularities((granularity)
             =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .EndingWithGoodQualityValue(Value(10)));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithFairAndPoorQualities_InterpolatedValuesHavePoorQuality()
        {
            ForAllGranularities((granularity)
                =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .StartingWith(Value(10), Quality.Fair));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithPoorAndFairQualities_InterpolatedValuesHavePoorQuality()
        {
            ForAllGranularities((granularity)
                =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .EndingWith(Value(10), Quality.Fair));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithFairAndBadQualities_InterpolatedValuesHaveBadQuality()
        {
            ForAllGranularities((granularity)
              =>
            {
                GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                          new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .StartingWith(Value(10), Quality.Fair));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithBadAndFairQualities_InterpolatedValuesHaveBadQuality()
        {
            ForAllGranularities((granularity)
                =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .EndingWith(Value(10), Quality.Fair));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithPoorAndBadQualities_InterpolatedValuesHaveBadQuality()
        {
            ForAllGranularities((granularity)
               =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .StartingWith(Value(10), Quality.Poor));
            });
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithBadAndPoorQualities_InterpolatedValuesHaveBadQuality()
        {
            ForAllGranularities((granularity)
               =>
            {
                GivenData(
                    new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = UniversalBeginTimestamp },
                    new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .EndingWith(Value(10), Quality.Poor));
            });
        }

        private void ForAllGranularities(Action<Granularity> test)
        {
            foreach (var granularity in Enum.GetValues(typeof(Granularity)).Cast<Granularity>())
            {
                GivenASignalWith(typeof(T).FromNativeType(), granularity);

                WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());

                test(granularity);
            }
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
