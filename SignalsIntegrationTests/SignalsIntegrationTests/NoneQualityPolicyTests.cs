using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;
using System;
using System.Linq;

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
        public void NoneQualityPolicyFillsMissingDataWhenNoDataPresent()
        {
            GivenNoData();

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumWithNoneQualityFor(BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumAtBeginOfRangePresent()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = BeginTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumWithSingleValueFollowedByNoneQualityFor(42, BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumBeforeBeginOfRangePresent()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = BeginTimestamp.AddDays(-1) });


            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumWithNoneQualityFor(BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumAtEndOfRangePresent()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumWithSingleValuePrecededByNoneQualityFor(42, BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumAfterEndOfRangePresent()
        {
            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = EndTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumWithNoneQualityFor(BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumInMiddleRangePresent()
        {
            var middleTimestamp = BeginTimestamp.AddDays(2);

            GivenSingleDatum(new Datum<int>() { Quality = Quality.Good, Value = 42, Timestamp = middleTimestamp });

            var expected = DatumWithNoneQualityFor(BeginTimestamp, EndTimestamp, Granularity.Day);

            expected.SingleOrDefault(e => e.Timestamp == middleTimestamp).Quality = Quality.Good;
            expected.SingleOrDefault(e => e.Timestamp == middleTimestamp).Value = 42;

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(expected);
        }
    }
}
