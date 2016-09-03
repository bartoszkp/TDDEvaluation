using System;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class ZeroOrderPolicyTests<T> : MissingValuePolicyTestsBase<T>
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
            GivenASignalWith(Granularity.Day);

            WithMissingValuePolicy(new Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<T>());
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenNoData_ReturnsNoneQualityForTheWholeRange()
        {
            GivenNoData();

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumAtBeginning_FillsRemainingRangeWithThatValue()
        {
            GivenSingleDatum(new Datum<T>() { Quality = Quality.Fair, Value = Value(Value(1410)), Timestamp = BeginTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(1410), Quality.Fair, BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumAfterBeginning_FillsRemainingRangeWithThatValue()
        {
            GivenSingleDatum(new Datum<T>() { Quality = Quality.Fair, Value = Value(1410), Timestamp = BeginTimestamp.AddDays(1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(1410), Quality.Fair, BeginTimestamp, EndTimestamp, Granularity.Day)
                .StartingWithNoneQuality());
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumAtTheEnd_ReturnsThatSingleValue()
        {
            GivenSingleDatum(new Datum<T>() { Quality = Quality.Fair, Value = Value(1410), Timestamp = EndTimestamp.AddDays(-1) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day)
                .EndingWith(Value(1410), Quality.Fair));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumAfterTheEnd_ReturnsNoneQualitytForRange()
        {
            GivenSingleDatum(new Datum<T>() { Quality = Quality.Fair, Value = Value(1410), Timestamp = EndTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(BeginTimestamp, EndTimestamp, Granularity.Day));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumInTheMiddle_ReturnsNoneQualityBeforeMiddleAndGivenValueAfter()
        {
            GivenSingleDatum(new Datum<T>() { Quality = Quality.Fair, Value = Value(1410), Timestamp = MiddleTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithNoneQualityForRange(BeginTimestamp, MiddleTimestamp, Granularity.Day)
                .FollowedBy(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(1410), Quality.Fair, MiddleTimestamp, EndTimestamp, Granularity.Day)));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenDatumAtBeginningAndInTheMiddle_ReturnsFirstValueBeforeMiddleAndSecondValueAfter()
        {
            GivenData(new Datum<T>() { Quality = Quality.Poor, Value = Value(753), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.Fair, Value = Value(1410), Timestamp = MiddleTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(753), Quality.Poor, BeginTimestamp, MiddleTimestamp, Granularity.Day)
                .FollowedBy(DatumArray<T>
                    .WithSpecificValueAndQualityForRange(Value(1410), Quality.Fair, MiddleTimestamp, EndTimestamp, Granularity.Day)));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenDatumAtBeginningAndNoneTheMiddle_ReturnsFirstValueBeforeMiddleAndNoneForRestOfRange()
        {
            GivenData(new Datum<T>() { Quality = Quality.Poor, Value = Value(753), Timestamp = BeginTimestamp },
                      new Datum<T>() { Quality = Quality.None, Timestamp = MiddleTimestamp });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(753), Quality.Poor, BeginTimestamp, MiddleTimestamp, Granularity.Day)
                .FollowedBy(DatumArray<T>
                    .WithNoneQualityForRange(MiddleTimestamp, EndTimestamp, Granularity.Day)));
        }

        [TestMethod]
        [TestCategory("issue10")]
        public void GivenSingleDatumBeforeBeginning_ReturnsItValueForTheWholeRange()
        {
            GivenSingleDatum(new Datum<T>() { Quality = Quality.Fair, Value = Value(1410), Timestamp = BeginTimestamp.AddDays(-10) });

            WhenReadingData(BeginTimestamp, EndTimestamp);

            ThenResultEquals(DatumArray<T>
                .WithSpecificValueAndQualityForRange(Value(1410), Quality.Fair, BeginTimestamp, EndTimestamp, Granularity.Day));
        }
    }

    [TestClass]
    public class ZeroOrderPolicyBoolTests : ZeroOrderPolicyTests<bool>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ZeroOrderPolicyTests<bool>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ZeroOrderPolicyTests<bool>.ClassCleanup();
        }
    }

    [TestClass]
    public class ZeroOrderPolicyIntTests : ZeroOrderPolicyTests<int>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ZeroOrderPolicyTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ZeroOrderPolicyTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class ZeroOrderPolicyDoubleTests : ZeroOrderPolicyTests<double>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ZeroOrderPolicyTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ZeroOrderPolicyTests<double>.ClassCleanup();
        }
    }

    [TestClass]
    public class ZeroOrderPolicyDecimalTests : ZeroOrderPolicyTests<decimal>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ZeroOrderPolicyTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ZeroOrderPolicyTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class ZeroOrderPolicyStringTests : ZeroOrderPolicyTests<string>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            ZeroOrderPolicyTests<string>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            ZeroOrderPolicyTests<string>.ClassCleanup();
        }
    }
}
