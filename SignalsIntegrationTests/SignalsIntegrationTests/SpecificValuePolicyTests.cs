using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;
using System;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class SpecificValuePolicyTests : MissingValuePolicyTestsBase
    {
        private DateTime BeginTimestamp { get { return new DateTime(2020, 10, 12); } }

        private DateTime EndTimestamp { get { return BeginTimestamp.AddDays(5); } }

        private int SpecificValue { get { return 14; } }

        private Quality SpecificQuality { get { return Quality.Fair; } }

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

            WithMissingValuePolicy(new Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<int>()
            {
                Value = SpecificValue,
                Quality = SpecificQuality
            });
        }

        [TestMethod]
        public void GivenNoData_ReturnsSpecificValueForTheWholeRange()
        {
            GivenNoData();

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        public void GivenSingleDatumAtTheBeginning_FillsRemainingRangeWithSpecificValue()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = BeginTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithGoodQualityValue(42));
        }

        [TestMethod]
        public void GivenSingleDatumBeforeTheBeginning_ReturnsSpecificValueForTheWholeRange()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = BeginTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        public void GivenSingleDatumAtTheEnd_FillsRemainingRangeWithSpecificValue()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWithGoodQualityValue(42));
        }

        [TestMethod]
        public void GivenSingleDatumAfterTheEnd_ReturnsSpecificValueForTheWholeRange()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = EndTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        public void GivenSingleDatumInTheMiddle_FillsRemainingRangesWithNoneQuality()
        {
            var middleTimestamp = BeginTimestamp.AddDays(2);

            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = middleTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(SpecificValue, SpecificQuality, BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithSingleGoodQualityValueAt(42, middleTimestamp));
        }
    }
}
