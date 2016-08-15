using System;
using Domain;
using Domain.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class FirstOrderPolicyTests<T> : MissingValuePolicyTestsBase<T>
    {
        private DateTime BeginTimestamp { get { return new DateTime(2020, 10, 12); } }

        private DateTime EndTimestamp { get { return BeginTimestamp.AddDays(5); } }

        private DateTime MiddleTimestamp { get { return BeginTimestamp.AddDays(2); } }

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

        [TestInitialize]
        public void TestInitialize()
        {
            GivenASignalWith(typeof(T).FromNativeType(), Granularity.Day);

            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<T>());
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenNoData_ReturnsNoneQualityForTheWholeRange()
        {
            GivenNoData();

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenSingleDatumAtBegining_FillsRestOfRangeWithNone()
        {
            GivenSingleDatum(new Datum<T>() { Quality = Quality.Good, Value = Value(1410), Timestamp = BeginTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithGoodQualityValue(Value(1410)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenSingleDatumAfterBegining_FillsRestOfRangeWithNone()
        {
            GivenSingleDatum(new Datum<T>() { Quality = Quality.Good, Value = Value(1410), Timestamp = BeginTimestamp.AddDays(1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithSingleGoodQualityValueAt(Value(1410), BeginTimestamp.AddDays(1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsAtBeginingAndBeforeEnd_InterpolatesValueForTheWholeRange()
        {
            GivenData(new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Good, Value = Value(30), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Good, BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithValueAt(Value(15), BeginTimestamp.AddDays(1))
                .WithValueAt(Value(20), BeginTimestamp.AddDays(2))
                .WithValueAt(Value(25), BeginTimestamp.AddDays(3))
                .WithValueAt(Value(30), BeginTimestamp.AddDays(4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsAfterBeginingAndBeforeEnd_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            GivenData(new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = BeginTimestamp.AddDays(1) },
                      new Datum<T>() { Quality = Quality.Good, Value = Value(30), Timestamp = EndTimestamp.AddDays(-2) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithGoodQualityValueAt(Value(10), BeginTimestamp.AddDays(1))
                .WithGoodQualityValueAt(Value(20), BeginTimestamp.AddDays(2))
                .WithGoodQualityValueAt(Value(30), BeginTimestamp.AddDays(3)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsBeforeAndAfterBegining_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            GivenData(new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = BeginTimestamp.AddDays(-1) },
                      new Datum<T>() { Quality = Quality.Good, Value = Value(30), Timestamp = BeginTimestamp.AddDays(1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithGoodQualityValueAt(Value(20), BeginTimestamp)
                .WithGoodQualityValueAt(Value(30), BeginTimestamp.AddDays(1)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsAtBeginingAndAtEnd_InterpolatesForTheWholeRange()
        {
            GivenData(new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Good, Value = Value(35), Timestamp = EndTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .ForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithGoodQualityValueAt(Value(10), BeginTimestamp.AddDays(0))
                .WithGoodQualityValueAt(Value(15), BeginTimestamp.AddDays(1))
                .WithGoodQualityValueAt(Value(20), BeginTimestamp.AddDays(2))
                .WithGoodQualityValueAt(Value(25), BeginTimestamp.AddDays(3))
                .WithGoodQualityValueAt(Value(30), BeginTimestamp.AddDays(4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsAfterBeginingAndAfterEnd_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            GivenData(new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = BeginTimestamp.AddDays(1) },
                      new Datum<T>() { Quality = Quality.Good, Value = Value(35), Timestamp = EndTimestamp.AddDays(1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithGoodQualityValueAt(Value(10), BeginTimestamp.AddDays(1))
                .WithGoodQualityValueAt(Value(15), BeginTimestamp.AddDays(2))
                .WithGoodQualityValueAt(Value(20), BeginTimestamp.AddDays(3))
                .WithGoodQualityValueAt(Value(25), BeginTimestamp.AddDays(4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsBeforeBeginingAndAfterEnd_InterpolatesForTheWholeRange()
        {
            GivenData(new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = BeginTimestamp.AddDays(-1) },
                      new Datum<T>() { Quality = Quality.Good, Value = Value(80), Timestamp = EndTimestamp.AddDays(1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .ForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithGoodQualityValueAt(Value(20), BeginTimestamp.AddDays(0))
                .WithGoodQualityValueAt(Value(30), BeginTimestamp.AddDays(1))
                .WithGoodQualityValueAt(Value(40), BeginTimestamp.AddDays(2))
                .WithGoodQualityValueAt(Value(50), BeginTimestamp.AddDays(3))
                .WithGoodQualityValueAt(Value(60), BeginTimestamp.AddDays(4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenThreeDatums_ProperlyChangesInterpolation()
        {
            GivenData(new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Good, Value = Value(30), Timestamp = MiddleTimestamp },
                      new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .ForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithGoodQualityValueAt(Value(10), BeginTimestamp.AddDays(0))
                .WithGoodQualityValueAt(Value(20), BeginTimestamp.AddDays(1))
                .WithGoodQualityValueAt(Value(30), BeginTimestamp.AddDays(2))
                .WithGoodQualityValueAt(Value(20), BeginTimestamp.AddDays(3))
                .WithGoodQualityValueAt(Value(10), BeginTimestamp.AddDays(4)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithGoodAndFairQualities_InterpolatedValuesHaveFairQuality()
        {
            GivenData(new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithFairAndGoodQualities_InterpolatedValuesHaveFairQuality()
        {
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Fair, BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithGoodAndPoorQualities_InterpolatedValuesHavePoorQuality()
        {
            GivenData(new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithPoorAndGoodQualities_InterpolatedValuesHavePoorQuality()
        {
            GivenData(new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithGoodAndBadQualities_InterpolatedValuesHaveBadQuality()
        {
            GivenData(new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithBadAndGoodQualities_InterpolatedValuesHaveFairQuality()
        {
            GivenData(new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Good, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWithGoodQualityValue(Value(10)));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithFairAndPoorQualities_InterpolatedValuesHavePoorQuality()
        {
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithPoorAndFairQualities_InterpolatedValuesHavePoorQuality()
        {
            GivenData(new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Poor, BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithFairAndBadQualities_InterpolatedValuesHaveBadQuality()
        {
            GivenData(new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithBadAndFairQualities_InterpolatedValuesHaveBadQuality()
        {
            GivenData(new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Fair, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWith(Value(10), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithPoorAndBadQualities_InterpolatedValuesHaveBadQuality()
        {
            GivenData(new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWith(Value(10), Quality.Poor));
        }

        [TestMethod]
        [TestCategory("issue11")]
        public void GivenDatumsWithBadAndPoorQualities_InterpolatedValuesHaveBadQuality()
        {
            GivenData(new Datum<T>() { Quality = Quality.Bad, Value = Value(10), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Poor, Value = Value(10), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(10), Quality.Bad, BeginTimestamp, EndTimestamp, Granularity.Day)
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
