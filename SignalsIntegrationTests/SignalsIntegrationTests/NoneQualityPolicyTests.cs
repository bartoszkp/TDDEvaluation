using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;
using System;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class NoneQualityPolicyTests : MissingValuePolicyTestsBase
    {
        private MissingValuePolicyValidator validator;
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
        public void InitializeValidator()
        {
            validator = new MissingValuePolicyValidator(this)
            {
                Policy = new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy()
            };
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenNoDataPresent()
        {
            GivenASignal(Granularity.Day);
            WithNoData();
            WithMissingValuePolicy(new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy());

            var result = WhenReadingData(BeginTimestamp, EndTimestamp);

            Then.AssertEqual(
                DatumWithNoneQualityFor(BeginTimestamp, EndTimestamp, Granularity.Day),
                result);
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumAtBeginOfRangePresent()
        {
            validator.WithSingleDatumAtBeginOfRangeExpect(new[]
            {
                new Datum<int> { Quality = Quality.Good, Timestamp = validator.BeginTimestamp, Value = validator.GeneratedSingleValue },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(1) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(2) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(3) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(4) },
            });
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumBeforeBeginOfRangePresent()
        {
            validator.WithSingleDatumBeforeBeginOfRangeExpect(new[]
            {
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp,},
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(1) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(2) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(3) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(4) },
            });
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumAtEndOfRangePresent()
        {
            validator.WithSingleDatumAtEndOfRangeExpect(new[]
            {
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(1) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(2) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(3) },
                new Datum<int> { Quality = Quality.Good, Timestamp = validator.BeginTimestamp.AddDays(4), Value = validator.GeneratedSingleValue }
            });
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumAfterEndOfRangePresent()
        {
            validator.WithSingleDatumAfterEndOfRangeExpect(new[]
            {
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(1) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(2) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(3) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(4) },
            });
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenSingleDatumInMiddleRangePresent()
        {
            validator.WithSingleDatumInMiddleOfRangeExpect(new[]
            {
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(1) },
                new Datum<int> { Quality = Quality.Good, Timestamp = validator.BeginTimestamp.AddDays(2), Value = validator.GeneratedSingleValue },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(3) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(4) },
            });
        }
    }
}
