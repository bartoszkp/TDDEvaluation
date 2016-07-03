using System;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class FirstOrderPolicyTests : MissingValuePolicyTestsBase
    {
        private DateTime BeginTimestamp { get { return new DateTime(2020, 10, 12); } }

        private DateTime EndTimestamp { get { return BeginTimestamp.AddDays(5); } }

        private DateTime MiddleTimestamp { get { return BeginTimestamp.AddDays(2); } }

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            MissingValuePolicyTestsBase.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            MissingValuePolicyTestsBase.ClassCleanup();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            GivenASignal(Granularity.Day);

            WithMissingValuePolicy(new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<int>());
        }

        [TestMethod]
        public void GivenNoData_ReturnsNoneQualityForTheWholeRange()
        {
            GivenNoData();

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        public void GivenSingleDatumAtBegining_FillsRestOfRangeWithNone()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 1410, Timestamp = BeginTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithGoodQualityValue(1410));
        }

        [TestMethod]
        public void GivenSingleDatumAfterBegining_FillsRestOfRangeWithNone()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 1410, Timestamp = BeginTimestamp.AddDays(1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithSingleGoodQualityValueAt(1410, BeginTimestamp.AddDays(1)));
        }

        [TestMethod]
        public void GivenDatumsAtBeginingAndBeforeEnd_InterpolatesValueForTheWholeRange()
        {
            GivenData(new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Good, Value = 30, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Good, BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithValueAt(15, BeginTimestamp.AddDays(1))
                .WithValueAt(20, BeginTimestamp.AddDays(2))
                .WithValueAt(25, BeginTimestamp.AddDays(3))
                .WithValueAt(30, BeginTimestamp.AddDays(4)));
        }

        [TestMethod]
        public void GivenDatumsAfterBeginingAndBeforeEnd_InterpolatesValueForGivenRangeAndInsertsNoneOutsideIt()
        {
            GivenData(new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = BeginTimestamp.AddDays(1) },
                      new Datum<int>() { Quality = Quality.Good, Value = 30, Timestamp = EndTimestamp.AddDays(-2) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithGoodQualityValueAt(10, BeginTimestamp.AddDays(1))
                .WithGoodQualityValueAt(20, BeginTimestamp.AddDays(2))
                .WithGoodQualityValueAt(30, BeginTimestamp.AddDays(3)));
        }

        [TestMethod]
        public void GivenDatumsBeforeAndAfterBegining_InterpolatesValueForRangeBetweenBeginAndGivenDatumAndInsertsNoneAfterIt()
        {
            GivenData(new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = BeginTimestamp.AddDays(-1) },
                      new Datum<int>() { Quality = Quality.Good, Value = 30, Timestamp = BeginTimestamp.AddDays(1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithGoodQualityValueAt(20, BeginTimestamp)
                .WithGoodQualityValueAt(30, BeginTimestamp.AddDays(1)));
        }

        [TestMethod]
        public void GivenDatumsAtBeginingAndAtEnd_InterpolatesForTheWholeRange()
        {
            GivenData(new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Good, Value = 35, Timestamp = EndTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .ForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithGoodQualityValueAt(10, BeginTimestamp.AddDays(0))
                .WithGoodQualityValueAt(15, BeginTimestamp.AddDays(1))
                .WithGoodQualityValueAt(20, BeginTimestamp.AddDays(2))
                .WithGoodQualityValueAt(25, BeginTimestamp.AddDays(3))
                .WithGoodQualityValueAt(30, BeginTimestamp.AddDays(4)));
        }

        [TestMethod]
        public void GivenDatumsAfterBeginingAndAfterEnd_InterpolatesForRangeBetweenGivenDatumAndEnd()
        {
            GivenData(new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = BeginTimestamp.AddDays(1) },
                      new Datum<int>() { Quality = Quality.Good, Value = 35, Timestamp = EndTimestamp.AddDays(1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithGoodQualityValueAt(10, BeginTimestamp.AddDays(1))
                .WithGoodQualityValueAt(15, BeginTimestamp.AddDays(2))
                .WithGoodQualityValueAt(20, BeginTimestamp.AddDays(3))
                .WithGoodQualityValueAt(25, BeginTimestamp.AddDays(4)));
        }

        [TestMethod]
        public void GivenDatumsBeforeBeginingAndAfterEnd_InterpolatesForTheWholeRange()
        {
            GivenData(new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = BeginTimestamp.AddDays(-1) },
                      new Datum<int>() { Quality = Quality.Good, Value = 80, Timestamp = EndTimestamp.AddDays(1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .ForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithGoodQualityValueAt(20, BeginTimestamp.AddDays(0))
                .WithGoodQualityValueAt(30, BeginTimestamp.AddDays(1))
                .WithGoodQualityValueAt(40, BeginTimestamp.AddDays(2))
                .WithGoodQualityValueAt(50, BeginTimestamp.AddDays(3))
                .WithGoodQualityValueAt(60, BeginTimestamp.AddDays(4)));
        }

        [TestMethod]
        public void GivenThreeDatums_ProperlyChangesInterpolation()
        {
            GivenData(new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Good, Value = 30, Timestamp = MiddleTimestamp },
                      new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .ForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithGoodQualityValueAt(10, BeginTimestamp.AddDays(0))
                .WithGoodQualityValueAt(20, BeginTimestamp.AddDays(1))
                .WithGoodQualityValueAt(30, BeginTimestamp.AddDays(2))
                .WithGoodQualityValueAt(20, BeginTimestamp.AddDays(3))
                .WithGoodQualityValueAt(10, BeginTimestamp.AddDays(4)));
        }

        [TestMethod]
        public void GivenDatumsWithGoodAndFairQualities_InterpolatedValuesHaveFairQuality()
        {
            GivenData(new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Fair, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Fair, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithGoodQualityValue(10));
        }

        [TestMethod]
        public void GivenDatumsWithFairAndGoodQualities_InterpolatedValuesHaveFairQuality()
        {
            GivenData(new Datum<int>() { Quality = Quality.Fair, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Fair, BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWithGoodQualityValue(10));
        }

        [TestMethod]
        public void GivenDatumsWithGoodAndPoorQualities_InterpolatedValuesHavePoorQuality()
        {
            GivenData(new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Poor, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Poor, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithGoodQualityValue(10));
        }

        [TestMethod]
        public void GivenDatumsWithPoorAndGoodQualities_InterpolatedValuesHavePoorQuality()
        {
            GivenData(new Datum<int>() { Quality = Quality.Poor, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Poor, BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWithGoodQualityValue(10));
        }

        [TestMethod]
        public void GivenDatumsWithGoodAndBadQualities_InterpolatedValuesHaveBadQuality()
        {
            GivenData(new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Bad, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Bad, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithGoodQualityValue(10));
        }

        [TestMethod]
        public void GivenDatumsWithBadAndGoodQualities_InterpolatedValuesHaveFairQuality()
        {
            GivenData(new Datum<int>() { Quality = Quality.Bad, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Good, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Bad, BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWithGoodQualityValue(10));
        }

        [TestMethod]
        public void GivenDatumsWithFairAndPoorQualities_InterpolatedValuesHavePoorQuality()
        {
            GivenData(new Datum<int>() { Quality = Quality.Fair, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Poor, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Poor, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWith(10, Quality.Fair));
        }

        [TestMethod]
        public void GivenDatumsWithPoorAndFairQualities_InterpolatedValuesHavePoorQuality()
        {
            GivenData(new Datum<int>() { Quality = Quality.Poor, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Fair, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Poor, BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWith(10, Quality.Fair));
        }

        [TestMethod]
        public void GivenDatumsWithFairAndBadQualities_InterpolatedValuesHaveBadQuality()
        {
            GivenData(new Datum<int>() { Quality = Quality.Fair, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Bad, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Bad, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWith(10, Quality.Fair));
        }

        [TestMethod]
        public void GivenDatumsWithBadAndFairQualities_InterpolatedValuesHaveBadQuality()
        {
            GivenData(new Datum<int>() { Quality = Quality.Bad, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Fair, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Bad, BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWith(10, Quality.Fair));
        }

        [TestMethod]
        public void GivenDatumsWithPoorAndBadQualities_InterpolatedValuesHaveBadQuality()
        {
            GivenData(new Datum<int>() { Quality = Quality.Poor, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Bad, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Bad, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWith(10, Quality.Poor));
        }

        [TestMethod]
        public void GivenDatumsWithBadAndPoorQualities_InterpolatedValuesHaveBadQuality()
        {
            GivenData(new Datum<int>() { Quality = Quality.Bad, Value = 10, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Poor, Value = 10, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(10, Quality.Bad, BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWith(10, Quality.Poor));
        }
    }
}
