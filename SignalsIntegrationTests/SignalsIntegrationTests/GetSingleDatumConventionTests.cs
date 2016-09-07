using System.Linq;
using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class GetSingleBooleanDatumConventionTests : GenericTestBase<bool>
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
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Second);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Minute);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Hour);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Day);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Week);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Month);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Year);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Second);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Minute);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Hour);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Day);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Week);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Month);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Year);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = false });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Second);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Minute);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Hour);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Day);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Week);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Month);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            GivenASignalWith(DataType.Boolean, Granularity.Year);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Boolean });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });
            var datum = new Datum<bool>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Boolean, granularity);
            var shadow = AddNewSignal(DataType.Boolean, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Boolean, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<bool>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = false
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }
    }

    [TestClass]
    public class GetSingleIntegerDatumConventionTests : GenericTestBase<int>
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
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Integer, Quality = Dto.Quality.Good, Value = 0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });
            var datum = new Datum<int>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Integer, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Integer });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<int>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }
    }

    [TestClass]
    public class GetSingleDoubleDatumConventionTests : GenericTestBase<double>
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
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Double, Quality = Dto.Quality.Good, Value = (double)0 });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });
            var datum = new Datum<double>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Double, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Double });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<double>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = (double)0
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }
    }

    [TestClass]
    public class GetSingleDecimalDatumConventionTests : GenericTestBase<decimal>
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
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Decimal, Quality = Dto.Quality.Good, Value = 0.0m });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });
            var datum = new Datum<decimal>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.Decimal, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() { DataType = Dto.DataType.Decimal });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<decimal>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = 0.0m
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }
    }

    [TestClass]
    public class GetSingleStringDatumConventionTests : GenericTestBase<string>
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
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithNoneMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithSpecificMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { DataType = Dto.DataType.String, Quality = Dto.Quality.Good, Value = null });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.Good,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithZeroOrderMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy() { DataType = Dto.DataType.String });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleBadDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSinglePoorDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleFairDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithSingleGoodDatumWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsTheDatum()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });
            var datum = new Datum<string>() { Timestamp = UniversalBeginTimestamp, Value = Value(10), Quality = quality };
            GivenSingleDatum(datum);

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenASecondSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMinuteSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAHourSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenADaySignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAWeekSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAMonthSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }

        [TestMethod]
        [TestCategory("issue17")]
        public void GivenAYearSignalWithNoDataWithShadowMVP_WhenGettingDataUsingSingleTimestamp_ReturnsDefaultData()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(DataType.String, granularity);
            var shadow = AddNewSignal(DataType.String, granularity);
            GivenMissingValuePolicy(new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.String, ShadowSignal = shadow });

            WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

            var expected = new Datum<string>
            {
                Quality = Quality.None,
                Timestamp = UniversalBeginTimestamp,
                Value = null
            };

            Assertions.AreEqual(expected, whenReadingDataResult.SingleOrDefault());
        }
    }
}
