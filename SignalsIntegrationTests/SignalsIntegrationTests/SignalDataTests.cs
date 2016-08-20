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
    public class SignalDataTests<T> : GenericTestBase<T>
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
        public void GivenASignal_WhenSettingEmptyData_ShouldNotThrow()
        {
            ForAllGranularities(granularity
                =>
            {
                client.SetData(signalId, new Dto.Datum[0]);
            });
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASignal_WhenSettingDataWithIncorrectType_ShouldThrow()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
                =>
            {
                object value = typeof(T).Equals(typeof(double)) 
                ? (decimal)1 as object
                : (double)1 as object;

                var data = new[] { new Dto.Datum()
                {
                    Value = value,
                    Quality = quality.ToDto<Dto.Quality>(),
                    Timestamp = UniversalBeginTimestamp
                } };

                Assertions.AssertThrows(() => client.SetData(signalId, data));
            });
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenTwoSignalsWithData_WhenGettingData_ReturnsCorrectDataForBoth()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
            =>
            {
                GivenASignalWith(typeof(T).FromNativeType(), granularity);
                GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(0), Quality = quality });
                var signalId1 = signalId;
                GivenASignalWith(typeof(T).FromNativeType(), granularity);
                GivenData(new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality });
                var signalId2 = signalId;

                var result1 = client.GetData(signalId1, UniversalBeginTimestamp, UniversalBeginTimestamp);
                var result2 = client.GetData(signalId2, UniversalBeginTimestamp, UniversalBeginTimestamp);

                Assert.AreEqual(Value(0), result1.SingleOrDefault()?.Value);
                Assert.AreEqual(Value(1), result2.SingleOrDefault()?.Value);
            });
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASignalWithSingleDatum_WhenGettingData_ReturnsTheDatum()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
            =>
            {
                var datum = new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1), Quality = quality };
                GivenSingleDatum(datum);

                WhenReadingData(UniversalBeginTimestamp, UniversalBeginTimestamp);

                Assertions.AreEqual(datum, whenReadingDataResult.SingleOrDefault());
            });
            }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASignalWithUnorderedData_WhenGettingData_ReturnsDataSorted()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
            =>
            {
                var data = GivenTwoUnsortedDatumsFor(granularity, quality);

                WhenReadingData(data.Min(d => d.Timestamp), data.Max(d => d.Timestamp).AddSteps(granularity, 1));

                Assertions.AreEqual(data.OrderBy(d => d.Timestamp).ToArray(), whenReadingDataResult);
            });
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenSignalWithData_WhenSettingOverlappingData_DataIsOverwritten()
        {
            ForAllGranularitiesAndQualities((granularity, quality)
            =>
            {
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
            });
        }

        [TestMethod]
        [TestCategory("issue2")]
        public void GivenASignal_WhenSettingDataWithDuplicateTimestamps_Throws()
        {
            ForAllGranularities(granularity
                =>
            {
                var data = new[]
                {
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(1) },
                    new Datum<T>() { Timestamp = UniversalBeginTimestamp, Value = Value(2) }
                };

                Assertions.AssertThrows(() => client.SetData(
                    signalId,
                    data.ToDto<Dto.Datum[]>()));
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

                test(granularity);
            }
        }

        private Datum<T>[] GivenTwoUnsortedDatumsFor(Granularity granularity, Quality quality)
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
