using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;
using System;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class ZeroOrderPolicyTests : MissingValuePolicyTestsBase
    {
        private DateTime BeginTimestamp { get { return new DateTime(2020, 10, 12); } }

        private DateTime EndTimestamp { get { return BeginTimestamp.AddDays(5); } }

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

            WithMissingValuePolicy(new Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<int>());
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
        public void GivenSingleDatumAtTheBeginning_FillsRemainingRangeWithThatValue()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Fair, Value = 1410, Timestamp = BeginTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(1410, Quality.Fair, BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        public void GivenSingleDatumAfterTheBeginning_FillsRemainingRangeWithThatValue()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Fair, Value = 1410, Timestamp = BeginTimestamp.AddDays(1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(1410, Quality.Fair, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithNoneQuality());
        }

        public void GivenSingleDatumAtTheEnd_ReturnsThatSingleValue()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Fair, Value = 1410, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWith(1410, Quality.Fair));
        }

        public void GivenSingleDatumAfterTheEnd_ReturnsNoneQualitytForRange()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Fair, Value = 1410, Timestamp = EndTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day));
        }
        /*
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
        */
    }
}
