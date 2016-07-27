using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class NoneQualityPolicyTests : MissingValuePolicyTestsBase
    {
        private MissingValuePolicyValidator validator;

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
                Policy = new NoneQualityMissingValuePolicy()
            };
        }

        [TestMethod]
        public void NoneQualityPolicyFillsMissingDataWhenNoDataPresent()
        {
            validator.WithoutSignalDataExpect(new[]
            {
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(1) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(2) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(3) },
                new Datum<int> { Quality = Quality.None, Timestamp = validator.BeginTimestamp.AddDays(4) },
            });
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
