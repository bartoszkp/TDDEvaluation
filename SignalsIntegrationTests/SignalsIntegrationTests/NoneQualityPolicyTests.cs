using System;
using System.Linq;
using Domain;
using Domain.Infrastructure;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class NoneQualityPolicyTests<T> : MissingValuePolicyTestsBase<T>
    {
        private DateTime BeginTimestamp { get { return new DateTime(2018, 1, 1); } }

        private DateTime EndTimestamp(Granularity granularity)
        {
            return BeginTimestamp.AddSteps(granularity, 5);
        }

        private DateTime MiddleTimestamp(Granularity granularity)
        {
            return BeginTimestamp.AddSteps(granularity, 2);
        }

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
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenNoData_ReturnsNoneQualityForTheWholeRange()
        {
            ForAllGranularities(granularity
                =>
            {
                GivenNoData();

                WhenReadingData(BeginTimestamp, EndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(BeginTimestamp, EndTimestamp(granularity), granularity));
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumAtTheBeginning_FillsRemainingRangeWithNoneQuality()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
                =>
            {
                T value = (T)values[typeof(T).FromNativeType()];
                GivenSingleDatum(new Datum<T>() { Quality = quality, Value = value, Timestamp = BeginTimestamp });

                WhenReadingData(BeginTimestamp, EndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(BeginTimestamp, EndTimestamp(granularity), granularity)
                    .StartingWith(value, quality));
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumBeforeTheBeginning_ReturnsNoneQualityForTheWholeRange()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
                =>
            {
                T value = (T)values[typeof(T).FromNativeType()];
                GivenSingleDatum(new Dto.Datum() { Quality = quality.ToDto<Dto.Quality>(), Value = value, Timestamp = BeginTimestamp.AddSteps(granularity, -1) });

                WhenReadingData(BeginTimestamp, EndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(BeginTimestamp, EndTimestamp(granularity), granularity));
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumAtTheEnd_FillsRemainingRangeWithNoneQuality()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
              =>
            {
                T value = (T)values[typeof(T).FromNativeType()];
                GivenSingleDatum(new Datum<T>() { Quality = quality, Value = value, Timestamp = EndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(BeginTimestamp, EndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(BeginTimestamp, EndTimestamp(granularity), granularity)
                    .EndingWith(value, quality));
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumAfterTheEnd_ReturnsNoneQualityForTheWholeRange()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
                =>
            {
                T value = (T)values[typeof(T).FromNativeType()];
                GivenSingleDatum(new Datum<T>() { Quality = Quality.Good, Value = value, Timestamp = EndTimestamp(granularity) });

                WhenReadingData(BeginTimestamp, EndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(BeginTimestamp, EndTimestamp(granularity), granularity));
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumInTheMiddle_FillsRemainingRangesWithNoneQuality()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
               =>
            {
                T value = (T)values[typeof(T).FromNativeType()];
                GivenSingleDatum(new Datum<T>() { Quality = quality, Value = value, Timestamp = MiddleTimestamp(granularity) });

                WhenReadingData(BeginTimestamp, EndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(BeginTimestamp, EndTimestamp(granularity), granularity)
                    .WithValueAt(value, quality, MiddleTimestamp(granularity)));
            });
        }

        private void ForAllGranularitiesAndQualities(Action<Granularity, Quality> test)
        {
            foreach (var quality in Enum.GetValues(typeof(Quality)).Cast<Quality>())
            {
                ForAllGranularities(granularity => test(granularity, quality));
            }
        }

        private void ForAllGranularities(Action<Granularity> test)
        {
            foreach (var granularity in Enum.GetValues(typeof(Granularity)).Cast<Granularity>())
            {
                GivenASignalWith(typeof(T).FromNativeType(), granularity);

                WithMissingValuePolicy(new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<T>());

                test(granularity);
            }
        }
    }

    [TestClass]
    public class NoneQualityPolicyIntTests : NoneQualityPolicyTests<int>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            NoneQualityPolicyTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            NoneQualityPolicyTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class NoneQualityPolicyDecimalTests : NoneQualityPolicyTests<decimal>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            NoneQualityPolicyTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            NoneQualityPolicyTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class NoneQualityPolicyDoubleTests : NoneQualityPolicyTests<double>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            NoneQualityPolicyTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            NoneQualityPolicyTests<double>.ClassCleanup();
        }
    }

    [TestClass]
    public class NoneQualityPolicyBoolTests : NoneQualityPolicyTests<bool>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            NoneQualityPolicyTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            NoneQualityPolicyTests<double>.ClassCleanup();
        }
    }

    [TestClass]
    public class NoneQualityPolicyStringTests : NoneQualityPolicyTests<string>
    {

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            NoneQualityPolicyTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            NoneQualityPolicyTests<double>.ClassCleanup();
        }
    }
}
