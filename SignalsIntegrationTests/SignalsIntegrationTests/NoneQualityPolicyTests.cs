using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;
using System;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class NoneQualityPolicyTests : MissingValuePolicyTestsBase
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
            WithMissingValuePolicy(new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy());
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
        public void GivenSingleDatumAtTheBeginning_FillsRemainingRangeWithNoneQuality()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = BeginTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithGoodQualityValue(42));
        }

        [TestMethod]
        public void GivenSingleDatumBeforeTheBeginning_ReturnsNoneQualityForTheWholeRange()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = BeginTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        public void GivenSingleDatumAtTheEnd_FillsRemainingRangeWithNoneQuality()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWithGoodQualityValue(42));
        }

        [TestMethod]
        public void GivenSingleDatumAfterTheEnd_ReturnsNoneQualityForTheWholeRange()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = EndTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        public void GivenSingleDatumInTheMiddle_FillsRemainingRangesWithNoneQuality()
        {
            var middleTimestamp = BeginTimestamp.AddDays(2);

            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = middleTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .WithSingleGoodQualityValueAt(42, middleTimestamp));
        }
    }
}
