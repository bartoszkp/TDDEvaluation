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

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity.Day, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown()
        {
            GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity.Day, Granularity.Year);
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenADaySignal_WhenReadingWeekData_ThenAverageIsCalculatedForAllSubranges()
        {
            GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity.Day, Granularity.Week);
        }

        private void GivenASignal_WhenReadingCoarseData_ThenAverageIsCalculatedForAllSubranges(Granularity granularity, Granularity coarseGranularity)
        {
            GivenASignal(granularity);
            GivenData(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(coarseGranularity, 1), granularity)
                .WithValue(Value(15))
                .WithQuality(Quality.Good)
                .StartingWithGoodQualityValue(Value(10))
                .EndingWithGoodQualityValue(Value(20)));
            GivenData(DatumArray<T>
                .ForRange(UniversalBeginTimestamp.AddSteps(coarseGranularity, 1), UniversalBeginTimestamp.AddSteps(coarseGranularity, 2), granularity)
                .WithValue(Value(25))
                .WithQuality(Quality.Good)
                .StartingWithGoodQualityValue(Value(30))
                .EndingWithGoodQualityValue(Value(20)));

            WhenReadingCoarseData(coarseGranularity, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(coarseGranularity, 2));

            ThenResultEquals(DatumArray<T>
                .ForRange(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(coarseGranularity, 2), coarseGranularity)
                .StartingWithGoodQualityValue(Value(15))
                .EndingWithGoodQualityValue(Value(25)));
        }

        private void GivenASignal_WhenReadingCoarseDataUsingIncorrectBegin_ThenExceptionIsThrown(Granularity granularity, Granularity coarseGranularity)
        {
            GivenASignal(granularity);

            Assertions.AssertThrows(() => WhenReadingCoarseData(coarseGranularity, UniversalBeginTimestamp.AddSeconds(1), UniversalEndTimestamp(coarseGranularity)));
        }

        private void GivenASignal_WhenReadingCoarseDataUsingIncorrectEnd_ThenExceptionIsThrown(Granularity granularity, Granularity coarseGranularity)
        {
            GivenASignal(granularity);

            Assertions.AssertThrows(() => WhenReadingCoarseData(coarseGranularity, UniversalBeginTimestamp, UniversalEndTimestamp(coarseGranularity).AddSeconds(1)));
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

    [TestClass]
    public class GetCoarseDataStringTests : GenericTestBase<string>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GenericTestBase<string>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GenericTestBase<string>.ClassCleanup();
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenAStringSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Day);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }
    }

    [TestClass]
    public class GetCoarseDataBooleanTests : GenericTestBase<bool>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            GenericTestBase<bool>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            GenericTestBase<bool>.ClassCleanup();
        }

        [TestMethod]
        [TestCategory("issueCoarseData")]
        public void GivenABooleanSignal_WhenReadingCoarseData_ExceptionIsThrown()
        {
            GivenASignal(Granularity.Day);

            Assertions.AssertThrows(() => WhenReadingCoarseData(Granularity.Week, UniversalBeginTimestamp, UniversalEndTimestamp(Granularity.Week)));
        }
    }
}
