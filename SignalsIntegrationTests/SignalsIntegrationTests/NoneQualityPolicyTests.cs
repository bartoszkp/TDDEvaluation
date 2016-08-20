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
    public abstract class NoneQualityPolicyTests<T> : MissingValuePolicyTestsBase<T>
    {
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

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenNoData_ReturnsNoneQualityForTheWholeRange()
        {
            ForAllGranularities(granularity
                =>
            {
                GivenNoData();

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
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
                GivenSingleDatum(new Datum<T>() { Quality = quality, Value = value, Timestamp = UniversalBeginTimestamp });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
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
                GivenSingleDatum(new Dto.Datum() { Quality = quality.ToDto<Dto.Quality>(), Value = value, Timestamp = UniversalBeginTimestamp.AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
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
                GivenSingleDatum(new Datum<T>() { Quality = quality, Value = value, Timestamp = UniversalEndTimestamp(granularity).AddSteps(granularity, -1) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
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
                GivenSingleDatum(new Datum<T>() { Quality = Quality.Good, Value = value, Timestamp = UniversalEndTimestamp(granularity) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity));
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
                GivenSingleDatum(new Datum<T>() { Quality = quality, Value = value, Timestamp = UniversalMiddleTimestamp(granularity) });

                WhenReadingData(UniversalBeginTimestamp, UniversalEndTimestamp(granularity));

                ThenResultEquals(DatumArray<T>
                    .WithNoneQualityForRange(UniversalBeginTimestamp, UniversalEndTimestamp(granularity), granularity)
                    .WithValueAt(value, quality, UniversalMiddleTimestamp(granularity)));
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
