using System.Linq;
using Domain;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public abstract class SignalDataTests<T> : GenericTestBase<T>
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
        [TestCategory("issue2")]
        public void GivenASecondSignal_WhenSettingEmptyData_ShouldNotThrow()
        {
            GivenASignalWith(Granularity.Second);

            client.SetData(signalId, new Dto.Datum[0]);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteSignal_WhenSettingEmptyData_ShouldNotThrow()
        {
            GivenASignalWith(Granularity.Minute);

            client.SetData(signalId, new Dto.Datum[0]);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourSignal_WhenSettingEmptyData_ShouldNotThrow()
        {
            GivenASignalWith(Granularity.Hour);

            client.SetData(signalId, new Dto.Datum[0]);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADaySignal_WhenSettingEmptyData_ShouldNotThrow()
        {
            GivenASignalWith(Granularity.Day);

            client.SetData(signalId, new Dto.Datum[0]);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekSignal_WhenSettingEmptyData_ShouldNotThrow()
        {
            GivenASignalWith(Granularity.Week);

            client.SetData(signalId, new Dto.Datum[0]);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthSignal_WhenSettingEmptyData_ShouldNotThrow()
        {
            GivenASignalWith(Granularity.Month);

            client.SetData(signalId, new Dto.Datum[0]);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearSignal_WhenSettingEmptyData_ShouldNotThrow()
        {
            GivenASignalWith(Granularity.Year);

            client.SetData(signalId, new Dto.Datum[0]);
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenASecondSignal_WhenSettingBadDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Second);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Bad,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenASecondSignal_WhenSettingPoorDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Second);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Poor,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenASecondSignal_WhenSettingFairDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Second);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Fair,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenASecondSignal_WhenSettingGoodDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Second);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Good,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAMinuteSignal_WhenSettingBadDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Minute);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Bad,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAMinuteSignal_WhenSettingPoorDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Minute);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Poor,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAMinuteSignal_WhenSettingFairDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Minute);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Fair,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAMinuteSignal_WhenSettingGoodDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Minute);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Good,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAHourSignal_WhenSettingBadDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Hour);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Bad,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAHourSignal_WhenSettingPoorDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Hour);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Poor,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAHourSignal_WhenSettingFairDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Hour);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Fair,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAHourSignal_WhenSettingGoodDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Hour);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Good,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenADaySignal_WhenSettingBadDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Day);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Bad,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenADaySignal_WhenSettingPoorDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Day);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Poor,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenADaySignal_WhenSettingFairDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Day);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Fair,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenADaySignal_WhenSettingGoodDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Day);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Good,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAWeekSignal_WhenSettingBadDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Week);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Bad,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAWeekSignal_WhenSettingPoorDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Week);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Poor,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAWeekSignal_WhenSettingFairDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Week);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Fair,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAWeekSignal_WhenSettingGoodDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Week);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Good,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAMonthSignal_WhenSettingBadDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Month);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Bad,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAMonthSignal_WhenSettingPoorDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Month);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Poor,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAMonthSignal_WhenSettingFairDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Month);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Fair,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAMonthSignal_WhenSettingGoodDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Month);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Good,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAYearSignal_WhenSettingBadDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Year);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Bad,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAYearSignal_WhenSettingPoorDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Year);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Poor,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAYearSignal_WhenSettingFairDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Year);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Fair,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("unassigned")]
        public void GivenAYearSignal_WhenSettingGoodDatumWithIncorrectType_ShouldThrow()
        {
            GivenASignalWith(Granularity.Year);

            object value = typeof(T).Equals(typeof(double))
                ? (decimal)1 as object
                : (double)1 as object;

            var data = new[] { new Dto.Datum()
            {
                Value = value,
                Quality = Dto.Quality.Good,
                Timestamp = UniversalBeginTimestamp
            } };

            Assertions.AssertThrows(() => client.SetData(signalId, data));
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoSecondSignalsWithBadQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoSecondSignalsWithPoorQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoSecondSignalsWithFairQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoSecondSignalsWithGoodQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoMinuteSignalsWithBadQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoMinuteSignalsWithPoorQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoMinuteSignalsWithFairQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoMinuteSignalsWithGoodQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoHourSignalsWithBadQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoHourSignalsWithPoorQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoHourSignalsWithFairQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoHourSignalsWithGoodQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoDaySignalsWithBadQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoDaySignalsWithPoorQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoDaySignalsWithFairQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoDaySignalsWithGoodQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoWeekSignalsWithBadQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoWeekSignalsWithPoorQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoWeekSignalsWithFairQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoWeekSignalsWithGoodQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoMonthSignalsWithBadQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoMonthSignalsWithPoorQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoMonthSignalsWithFairQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoMonthSignalsWithGoodQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoYearSignalsWithBadQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoYearSignalsWithPoorQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoYearSignalsWithFairQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoYearSignalsWithGoodQualityData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
            var signalId1 = signalId;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
            var signalId2 = signalId;

            var result1 = ClientGetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));
            var result2 = ClientGetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
            Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondSignalWithSingleBadDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondSignalWithSinglePoorDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondSignalWithSingleFairDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondSignalWithSingleGoodDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteSignalWithSingleBadDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteSignalWithSinglePoorDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteSignalWithSingleFairDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteSignalWithSingleGoodDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourSignalWithSingleBadDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourSignalWithSinglePoorDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourSignalWithSingleFairDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourSignalWithSingleGoodDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADaySignalWithSingleBadDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADaySignalWithSinglePoorDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADaySignalWithSingleFairDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADaySignalWithSingleGoodDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekSignalWithSingleBadDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekSignalWithSinglePoorDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekSignalWithSingleFairDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekSignalWithSingleGoodDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthSignalWithSingleBadDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthSignalWithSinglePoorDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthSignalWithSingleFairDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthSignalWithSingleGoodDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearSignalWithSingleBadDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearSignalWithSinglePoorDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearSignalWithSingleFairDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearSignalWithSingleGoodDatum_WhenGettingData_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 1));

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenASecondSignalWithUnorderedDataWithBadQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenASecondSignalWithUnorderedDataWithPoorQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenASecondSignalWithUnorderedDataWithFairQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenASecondSignalWithUnorderedDataWithGoodQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAMinuteSignalWithUnorderedDataWithBadQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAMinuteSignalWithUnorderedDataWithPoorQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAMinuteSignalWithUnorderedDataWithFairQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAMinuteSignalWithUnorderedDataWithGoodQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAHourSignalWithUnorderedDataWithBadQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAHourSignalWithUnorderedDataWithPoorQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAHourSignalWithUnorderedDataWithFairQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAHourSignalWithUnorderedDataWithGoodQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenADaySignalWithUnorderedDataWithBadQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenADaySignalWithUnorderedDataWithPoorQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenADaySignalWithUnorderedDataWithFairQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenADaySignalWithUnorderedDataWithGoodQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAWeekSignalWithUnorderedDataWithBadQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAWeekSignalWithUnorderedDataWithPoorQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAWeekSignalWithUnorderedDataWithFairQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAWeekSignalWithUnorderedDataWithGoodQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAMonthSignalWithUnorderedDataWithBadQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAMonthSignalWithUnorderedDataWithPoorQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAMonthSignalWithUnorderedDataWithFairQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAMonthSignalWithUnorderedDataWithGoodQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAYearSignalWithUnorderedDataWithBadQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAYearSignalWithUnorderedDataWithPoorQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAYearSignalWithUnorderedDataWithFairQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        [TestCategory("issue14")]
        public void GivenAYearSignalWithUnorderedDataWithGoodQuality_WhenGettingData_ReturnsDataSorted()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = GivenTwoUnsortedDatums(granularity, quality);

            WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

            Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondSignalWithDataWithBadQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondSignalWithDataWithPoorQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondSignalWithDataWithFairQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondSignalWithDataWithGoodQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteSignalWithDataWithBadQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteSignalWithDataWithPoorQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteSignalWithDataWithFairQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteSignalWithDataWithGoodQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourSignalWithDataWithBadQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourSignalWithDataWithPoorQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourSignalWithDataWithFairQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourSignalWithDataWithGoodQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADaySignalWithDataWithBadQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADaySignalWithDataWithPoorQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADaySignalWithDataWithFairQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADaySignalWithDataWithGoodQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekSignalWithDataWithBadQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekSignalWithDataWithPoorQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekSignalWithDataWithFairQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekSignalWithDataWithGoodQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthSignalWithDataWithBadQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthSignalWithDataWithPoorQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthSignalWithDataWithFairQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthSignalWithDataWithGoodQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearSignalWithDataWithBadQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearSignalWithDataWithPoorQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearSignalWithDataWithFairQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearSignalWithDataWithGoodQuality_WhenSettingOverlappingData_DataIsOverwritten()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            GivenData(new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(1), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(2), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality }
            });

            client.SetData(
                signalId,
                new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                }.ToDto<Dto.Datum[]>());

            var expectedData = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 0), Value = Value(0), Quality = quality },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 1), Value = Value(8), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 2), Value = Value(9), Quality = OtherThan(quality) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp.AddSteps(granularity, 3), Value = Value(3), Quality = quality },
            };


            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp.AddSteps(granularity, 4));

            Assertions.AreEqual(expectedData, whenReadingDataResult);
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASecondSignal_WhenSettingDataWithDuplicateTimestamps_Throws()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(2) }
            };

            Assertions.AssertThrows(() => client.SetData(
                signalId,
                data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMinuteSignal_WhenSettingDataWithDuplicateTimestamps_Throws()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(2) }
            };

            Assertions.AssertThrows(() => client.SetData(
                signalId,
                data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAHourSignal_WhenSettingDataWithDuplicateTimestamps_Throws()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(2) }
            };

            Assertions.AssertThrows(() => client.SetData(
                signalId,
                data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenADaySignal_WhenSettingDataWithDuplicateTimestamps_Throws()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(2) }
            };

            Assertions.AssertThrows(() => client.SetData(
                signalId,
                data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAWeekSignal_WhenSettingDataWithDuplicateTimestamps_Throws()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(2) }
            };

            Assertions.AssertThrows(() => client.SetData(
                signalId,
                data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAMonthSignal_WhenSettingDataWithDuplicateTimestamps_Throws()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(2) }
            };

            Assertions.AssertThrows(() => client.SetData(
                signalId,
                data.ToDto<Dto.Datum[]>()));
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenAYearSignal_WhenSettingDataWithDuplicateTimestamps_Throws()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(T).FromNativeType(), granularity);
            var data = new[]
            {
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1) },
                new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(2) }
            };

            Assertions.AssertThrows(() => client.SetData(
                signalId,
                data.ToDto<Dto.Datum[]>()));
        }

        private Datum<T>[] GivenTwoUnsortedDatums(Granularity granularity, Quality quality)
        {
            var from = UniversalBeginTimestamp;
            var to = from.AddSteps(granularity, 1);

            var data1 = new Datum<T>()
            {
                Quality = quality,
                Timestamp = to,
                Value = Value(0)
            };
            var data2 = new Datum<T>()
            {
                Quality = quality,
                Timestamp = from,
                Value = Value(1)
            };

            GivenData(data1, data2);

            return new[] { data1, data2 };
        }
    }

    [TestClass]
    public class SignalDataTestsBoolean : SignalDataTests<bool>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            SignalDataTests<bool>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            SignalDataTests<bool>.ClassCleanup();
        }
    }

    [TestClass]
    public class SignalDataTestsInteger : SignalDataTests<int>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            SignalDataTests<int>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            SignalDataTests<int>.ClassCleanup();
        }
    }

    [TestClass]
    public class SignalDataTestsDouble : SignalDataTests<double>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            SignalDataTests<double>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            SignalDataTests<double>.ClassCleanup();
        }
    }

    [TestClass]
    public class SignalDataTestsDecimal : SignalDataTests<decimal>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            SignalDataTests<decimal>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            SignalDataTests<decimal>.ClassCleanup();
        }
    }

    [TestClass]
    public class SignalDataTestsString : SignalDataTests<string>
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            SignalDataTests<string>.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            SignalDataTests<string>.ClassCleanup();
        }
    }
}
