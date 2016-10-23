using Domain;
using Domain.MissingValuePolicy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class GetCoarseDataTests<T> : GenericTestBase<T>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            TestsBase.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            TestsBase.ClassCleanup();
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignalWithSingleValueOnWholeRange_WhenReadingItWithMonthGranularity_SameSingleValueIsReturnedForMonths()
        {
            GivenASignalWithSingleValueOnWholeRange_WhenReadingItWithCoarserGranularity_SameSingleValueIsReturned(Granularity.Day, Granularity.Month);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignalWithoutData_WhenReadingItWithDayGranularity_ThenMissingValuePolicyIsUsed()
        {
            GivenASignalWithoutData_WhenReadingItWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity.Hour, Granularity.Day);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAHourSignal_WhenReadingItWithMinuteGranularity_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingItWithFinerGranularity_ThenExceptionIsThrown(Granularity.Hour, Granularity.Minute);
        }

        private void GivenASignal_WhenReadingItWithFinerGranularity_ThenExceptionIsThrown(Granularity granularity, Granularity fineGranularity)
        {
            GivenASignal(granularity);

            Assertions.AssertThrows(() => WhenReadingCoarseData(fineGranularity, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(fineGranularity, 1)));
        }

        private void GivenASignalWithoutData_WhenReadingItWithCoarserGranularity_ThenMissingValuePolicyIsUsed(Granularity granularity,
            Granularity coarseGranularity)
        {
            GivenASignal(granularity);
            WithMissingValuePolicy(new SpecificValueMissingValuePolicy<T>() { Value = Value(753), Quality = Quality.Poor });

            WhenReadingCoarseData(coarseGranularity, UniversalBeginTimestamp, UniversalEndTimestamp(coarseGranularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(coarseGranularity), coarseGranularity)
                .WithValue(Value(753)).WithQuality(Quality.Poor));
        }

        private void GivenASignalWithSingleValueOnWholeRange_WhenReadingItWithCoarserGranularity_SameSingleValueIsReturned(Granularity granularity,
            Granularity coarseGranularity)
        {
            GivenASignal(granularity);
            GivenData(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(coarseGranularity), granularity)
                .WithValue(Value(42)).WithQuality(Quality.Fair));

            WhenReadingCoarseData(coarseGranularity, UniversalBeginTimestamp, UniversalEndTimestamp(coarseGranularity));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalEndTimestamp(coarseGranularity), coarseGranularity)
                .WithValue(Value(42)).WithQuality(Quality.Fair));
        }

        private void GivenASignal(Granularity granularity)
        {
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
        }
    }

    [TestClass]
    public class GetCoarseDataIntTests : GetCoarseDataTests<int>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataDecimalTests : GetCoarseDataTests<decimal>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class GetCoarseDataDoubleTests : GetCoarseDataTests<double>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GetCoarseDataTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GetCoarseDataTests<double>.ClassCleanup();
        }
    }
}
