using System;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class ZeroOrderPolicyTests : MissingValuePolicyTestsBase
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
            GivenASignalWith(Granularity.Day);

            WithMissingValuePolicy(new Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<int>());
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenNoData_ReturnsNoneQualityForTheWholeRange()
        {
            GivenNoData();

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumAtTheBeginning_FillsRemainingRangeWithThatValue()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Fair, Value = 1410, Timestamp = BeginTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(1410, Quality.Fair, BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumAfterTheBeginning_FillsRemainingRangeWithThatValue()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Fair, Value = 1410, Timestamp = BeginTimestamp.AddDays(1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(1410, Quality.Fair, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithNoneQuality());
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumAtTheEnd_ReturnsThatSingleValue()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Fair, Value = 1410, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWith(1410, Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumAfterTheEnd_ReturnsNoneQualitytForRange()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Fair, Value = 1410, Timestamp = EndTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumInTheMiddle_ReturnsNoneQualityBeforeMiddleAndGivenValueAfter()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Fair, Value = 1410, Timestamp = MiddleTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithNoneQualityForRange(BeginTimestamp, MiddleTimestamp, Granularity.Day)
                .FollowedBy(DatumArray<int>
                    .WithSpecificValueAndQualityForRange(1410, Quality.Fair, MiddleTimestamp, EndTimestamp, Granularity.Day)));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenDatumAtTheBeginingAndInTheMiddle_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenData(new Datum<int>() { Quality = Quality.Poor, Value = 753, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.Fair, Value = 1410, Timestamp = MiddleTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(753, Quality.Poor, BeginTimestamp, MiddleTimestamp, Granularity.Day)
                .FollowedBy(DatumArray<int>
                    .WithSpecificValueAndQualityForRange(1410, Quality.Fair, MiddleTimestamp, EndTimestamp, Granularity.Day)));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenDatumAtTheBeginingAndNoneTheMiddle_ReturnsFirstValueBeforeMiddleAndNoneForRestOfRange()
        {
            GivenData(new Datum<int>() { Quality = Quality.Poor, Value = 753, Timestamp = BeginTimestamp },
                      new Datum<int>() { Quality = Quality.None, Timestamp = MiddleTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(753, Quality.Poor, BeginTimestamp, MiddleTimestamp, Granularity.Day)
                .FollowedBy(DatumArray<int>
                    .WithNoneQualityForRange(MiddleTimestamp, EndTimestamp, Granularity.Day)));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumBeforeTheBegining_ReturnsItValueForTheWholeRange()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Fair, Value = 1410, Timestamp = BeginTimestamp.AddDays(-10) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<int>
                .WithSpecificValueAndQualityForRange(1410, Quality.Fair, BeginTimestamp, EndTimestamp, Granularity.Day));
        }
    }
}
