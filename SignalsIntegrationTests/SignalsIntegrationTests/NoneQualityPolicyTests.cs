using Domain;
using Domain.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class NoneQualityPolicyTests<T> : SpecificValuePolicyTestsBase<T>
    {
        protected override T SpecificValue { get { return default(T); } }

        protected override Quality SpecificQuality { get { return Quality.None; } }

        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            SpecificValuePolicyTestsBase<T>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            SpecificValuePolicyTestsBase<T>.ClassCleanup();
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenNoData_ReturnsNoneQualityForTheWholeRange()
        {
            ForAllGranularities(granularity
                =>
            {
                GivenASignalWithNoData_WhenReadingData_ReturnsSpecificValueForTheWholeRange(granularity);
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumAtBeginning_FillsRemainingRangeWithNoneQuality()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
                =>
            {
                GivenASignalWithSingleDatum_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(granularity, quality);
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumBeforeBeginning_ReturnsNoneQualityForTheWholeRange()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
                =>
            {
                GivenASignalWithSingleDatumBeforeBeginning_WhenReadingData_ReturnsSpecificValueForWholeRange(granularity, quality);
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumAtTheEnd_FillsRemainingRangeWithNoneQuality()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
              =>
            {
                GivenASignalWithSingleDatumAtEnd_WhenReadingData_RemainingRangeIsFilledWithSpecificValue(granularity, quality);
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumAfterTheEnd_ReturnsNoneQualityForTheWholeRange()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
                =>
            {
                GivenASignalWithSingleDatumAfterEnd_WhenReadingData_ReturnsSpecificValueForWholeRange(granularity, quality);
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenSingleDatumInTheMiddle_FillsRemainingRangesWithNoneQuality()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
               =>
            {
                GivenASignalWithSingleDatumInMiddle_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(granularity, quality);
            });
        }

        [TestMethod]
        [TestCategory("issue6")]
        public void GivenDatumsAtTheBigginingAndInTheMiddle_FillsRemainingRangesWithNoneQuality()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
               =>
            {
                GivenASignalWithDatumInMiddleAndDifferentDatumAtBeginning_WhenReadingData_RemainingRangesAreFilledWithSpecificValue(granularity, quality);
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
                GivenASignal(granularity);

                test(granularity);
            }
        }

        protected override void GivenASignal(Granularity granularity)
        {
            GivenASignalWith(typeof(T).FromNativeType(), granularity);

            WithMissingValuePolicy(new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<T>());
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
