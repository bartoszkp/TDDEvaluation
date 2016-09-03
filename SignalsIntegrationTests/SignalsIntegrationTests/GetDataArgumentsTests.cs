using System;
using System.Linq;
using Domain;
using Domain.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class GetDataArgumentsTests : TestsBase
    {
        private DateTime FromTimestamp { get; } = new DateTime(2029, 1, 1, 0, 0, 0, 0);
        private DateTime ToTimestamp { get; } = new DateTime(2029, 1, 8, 0, 0, 0, 0);

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
        [TestCategory("issue2")]
        public void GivenNoSignals_WhenGettingDataWithInvalidSignalId_Throws()
        {
            GivenNoSignals();

            WhenGettigData(FromTimestamp, ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAStringSignal_WhenSettingNullValue_ShouldNotThrow()
        {
            GivenASignalWith(DataType.String, Granularity.Hour);

            client.SetData(signalId, new[] { new Dto.Datum() { Timestamp = new DateTime(2000, 1, 1) } });
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAStringSignalWithNullValue_WhenReadingData_ShouldReturnNullValue()
        {
            var timestamp = new DateTime(2000, 1, 1);

            GivenASignalWith(DataType.String, Granularity.Hour);
            GivenData(new Dto.Datum() { Timestamp = timestamp });

            var result = client.GetData(signalId, timestamp, timestamp.AddSteps(Granularity.Hour, 1))
                .SingleOrDefault();

            Assert.IsNotNull(result);
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondBoolSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteBoolSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourBoolSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayBoolSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekBoolSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthBoolSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearBoolSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondBoolSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteBoolSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourBoolSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayBoolSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekBoolSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthBoolSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearBoolSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondBoolSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteBoolSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourBoolSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayBoolSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekBoolSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthBoolSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearBoolSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondBoolSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteBoolSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourBoolSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayBoolSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekBoolSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthBoolSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearBoolSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);
            GivenData(new Datum<bool>() { Value = GenericTestBase<bool>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondIntSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteIntSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourIntSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayIntSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekIntSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthIntSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearIntSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondIntSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteIntSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourIntSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayIntSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekIntSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthIntSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearIntSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondIntSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteIntSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourIntSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayIntSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekIntSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthIntSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearIntSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondIntSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteIntSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourIntSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayIntSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekIntSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthIntSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearIntSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);
            GivenData(new Datum<int>() { Value = GenericTestBase<int>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondDoubleSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteDoubleSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourDoubleSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayDoubleSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekDoubleSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthDoubleSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearDoubleSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondDoubleSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteDoubleSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourDoubleSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayDoubleSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekDoubleSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthDoubleSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearDoubleSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondDoubleSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteDoubleSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourDoubleSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayDoubleSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekDoubleSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthDoubleSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearDoubleSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondDoubleSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteDoubleSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourDoubleSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayDoubleSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekDoubleSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthDoubleSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearDoubleSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);
            GivenData(new Datum<double>() { Value = GenericTestBase<double>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondDecimalSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteDecimalSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourDecimalSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayDecimalSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekDecimalSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthDecimalSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearDecimalSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondDecimalSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteDecimalSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourDecimalSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayDecimalSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekDecimalSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthDecimalSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearDecimalSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondDecimalSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteDecimalSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourDecimalSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayDecimalSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekDecimalSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthDecimalSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearDecimalSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondDecimalSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteDecimalSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourDecimalSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayDecimalSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekDecimalSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthDecimalSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearDecimalSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);
            GivenData(new Datum<decimal>() { Value = GenericTestBase<decimal>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondStringSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteStringSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourStringSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayStringSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekStringSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthStringSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearStringSignalWithBadDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Bad;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondStringSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteStringSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourStringSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayStringSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekStringSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthStringSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearStringSignalWithPoorDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Poor;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondStringSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteStringSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourStringSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayStringSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekStringSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthStringSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearStringSignalWithFairDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Fair;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondStringSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteStringSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourStringSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADayStringSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekStringSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthStringSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearStringSignalWithGoodDatum_WhenGettingDataWithReversedTimestamps_ReturnsEmpty()
        {
            var quality = Quality.Good;
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);
            GivenData(new Datum<string>() { Value = GenericTestBase<string>.Value(1410), Timestamp = UniversalBeginTimestamp, Quality = quality });

            WhenGettigData(UniversalEndTimestamp(granularity), UniversalBeginTimestamp);

            ThenResultIsEmpty();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForSecondGranularityRequiresZeroMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Second);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMinuteGranularityRequiresZeroMillisecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Minute);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMinuteGranularityRequiresZeroSecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Minute);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForHourGranularityRequiresZeroMillisecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Hour);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForHourGranularityRequiresZeroSecondsInFromTimestamp()
        {
            GivenASignalWith(Granularity.Hour);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForHourGranularityRequiresZeroMinutesInFromTimestamp()
        {
            GivenASignalWith(Granularity.Hour);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForDayGranularityRequiresZeroMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForDayGranularityRequiresZeroecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForDayGranularityRequiresZeroMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForDayGranularityRequiresZeroHoursInFromTimestamps()
        {
            GivenASignalWith(Granularity.Day);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresZeroMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresZeroSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresZeroMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresZeroHoursInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForWeekGranularityRequiresMondayInFromTimestamps()
        {
            GivenASignalWith(Granularity.Week);

            WhenGettigData(FromTimestamp.AddDays(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresZeroMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresZeroSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresZeroMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresZeroHoursInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForMonthGranularityRequiresFirstDayOfMonthInFromTimestamps()
        {
            GivenASignalWith(Granularity.Month);

            WhenGettigData(FromTimestamp.AddDays(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresZeroMillisecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddMilliseconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresZeroSecondsInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddSeconds(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresZeroMinutesInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddMinutes(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void SetDataForYearGranularityRequiresZeroHoursInTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddHours(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresFirstDayOfMonthInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddDays(1), ToTimestamp);

            ThenRequestThrows();
        }

        [TestMethod]
        [TestCategory("issue9")]
        public void GetDataForYearGranularityRequiresFirstMonthInFromTimestamps()
        {
            GivenASignalWith(Granularity.Year);

            WhenGettigData(FromTimestamp.AddMonths(1), ToTimestamp.AddYears(1));

            ThenRequestThrows();
        }

        private void WhenGettigData(DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            getData = () => client.GetData(signalId, fromIncludedUtc, toExcludedUtc);
        }

        private void ThenRequestThrows()
        {
            Assertions.AssertThrows(() => getData());
        }

        private void ThenResultIsEmpty()
        {
            var result = getData();

            Assert.IsFalse(result.Any());
        }

        private Func<Dto.Datum[]> getData;
    }
}
