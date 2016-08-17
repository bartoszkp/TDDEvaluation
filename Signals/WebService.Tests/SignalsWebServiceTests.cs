using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Domain.MissingValuePolicy;
using System.Collections.Generic;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.IdNotNullException))]
            public void GivenNoSignals_WhenAddingASignalWithId_ThrowIdNotNullException()
            {
                GivenNoSignals();

                var result = signalsWebService.Add(SignalWith(id: 1));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsIt()
            {
                GivenNoSignals();

                var result = signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Double,
                    granularity: Dto.Granularity.Month,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }
                    ));

                Assert.AreEqual(Dto.DataType.Double, result.DataType);
                Assert.AreEqual(Dto.Granularity.Month, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_CallsRepositoryAdd()
            {
                GivenNoSignals();

                signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Double,
                    granularity: Dto.Granularity.Month,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }
                    ));

                signalsRepositoryMock.Verify(sr => sr.Add(It.Is<Domain.Signal>(passedSignal
                    => passedSignal.DataType == DataType.Double
                        && passedSignal.Granularity == Granularity.Month
                        && passedSignal.Path.ToString() == "root/signal")));
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.GetById(0);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByItsId_ReturnsIt()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.GetById(signalId);

                Assert.AreEqual(signalId, result.Id);
                Assert.AreEqual(Dto.DataType.Integer, result.DataType);
                Assert.AreEqual(Dto.Granularity.Second, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByPath_ReturnsIt()
            {
                Dto.Path path = new Dto.Path() { Components = new[] { "x", "y" } };
                GivenASignal(SignalWith(
                    id: 1,
                    dataType: DataType.Boolean,
                    granularity: Granularity.Month,
                    path: path.ToDomain<Domain.Path>()
                    ));

                var result = signalsWebService.Get(path.ToDto<Dto.Path>());

                Assert.AreEqual(Dto.DataType.Boolean, result.DataType);
                Assert.AreEqual(Dto.Granularity.Month, result.Granularity);
                CollectionAssert.AreEqual(path.Components.ToArray(), result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenNoSignal_WhenGettingByNonExistentPath_ReturnNull()
            {
                GivenNoSignals();
                var result = signalsWebService.Get(null);
                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenSetMissingValuePolicy_CalledMissingValuePolicyRepository()
            {
                GivenASignal(SignalWith());

                Dto.MissingValuePolicy.MissingValuePolicy mvpDTO = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();

                signalsWebService.SetMissingValuePolicy(1, mvpDTO);

                missingValuePolicyRepositoryMock.Verify(sr => sr.Set(It.IsAny<Domain.Signal>(), It.IsAny<MissingValuePolicyBase>()));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSetMissingValuePolicyWithInvalidId_ReturnException()
            {
                int correctId = 1;
                int invalidId = 123;

                GivenASignal(SignalWith());

                signalsWebService.SetMissingValuePolicy(invalidId, new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy());
            }
            [TestMethod]
            public void GivenASignal_WhenGetMissingValuePolicy_ColledmissingValuePolicyRepository()
            {
                GivenASignal(SignalWith());

                signalsWebService.GetMissingValuePolicy(1);

                missingValuePolicyRepositoryMock.Verify(s => s.Get(It.IsAny<Signal>()));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGetMissingValuePolicyWithInvalidID_ReturnException()
            {
                GivenASignal(SignalWith());
                int invalidID = -1;
                var res = signalsWebService.GetMissingValuePolicy(invalidID);
            }

            [TestMethod]
            public void GivenASignal_WhenGetMissingValuePolicy_ReturnCorrectMVP()
            {
                Signal signal = SignalWith();
                GivenASignal(signal);
                int id = 1;

                SpecificValueMissingValuePolicy<bool> MVP = new SpecificValueMissingValuePolicy<bool>()
                {
                    Id = id,
                    Quality = Quality.Fair,
                    Signal = signal,
                    Value = true,
                };

                missingValuePolicyRepositoryMock
                    .Setup(s => s.Get(signal))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyBoolean()
                    {
                        Id = id,
                        Quality = Quality.Fair,
                        Signal = signal,
                        Value = true,
                    });

                Dto.MissingValuePolicy.MissingValuePolicy resultMVP = signalsWebService.GetMissingValuePolicy(id);

                Assert.AreEqual(MVP.Id.Value, resultMVP.Id.Value);
                Assert.IsTrue(EqualsSignal(MVP.Signal, resultMVP.Signal.ToDomain<Domain.Signal>()));
                Assert.AreEqual(MVP.NativeDataType.Name.ToString(), resultMVP.DataType.ToString());
            }

            [TestMethod]
            public void GivenASignal_WhenSetDataIsInvokedWithVariousTypes_CalledDataRepisitory()
            {
                GivenASignal(SignalWith(DataType.Boolean));
                SetDataCalledVerify<bool>();
                GivenASignal(SignalWith(DataType.Decimal));
                SetDataCalledVerify<decimal>();
                GivenASignal(SignalWith(DataType.Double));
                SetDataCalledVerify<double>();
                GivenASignal(SignalWith(DataType.Integer));
                SetDataCalledVerify<int>();
                GivenASignal(SignalWith(DataType.String));
                SetDataCalledVerify<string>();
            }
            [TestMethod]
            [ExpectedException(typeof(InvalidCastException))]
            public void GivenASignal_WhenSetDataTypesSignalAndDatumAreDiferent_ReturnException()
            {
                GivenASignal(SignalWith(DataType.Integer));
                SetDataCalledVerify<bool>();
            }
            private void SetDataCalledVerify<T>()
            {
                signalsWebService.SetData(1, GetDtoDatum<T>());
                signalsDataRepositoryMock.Verify(s => s.SetData<T>(It.IsAny<IEnumerable<Domain.Datum<T>>>()));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSetDataWithInvalidId_ReturnException()
            {
                int invalidID = 999;
                GivenASignal(SignalWith());
                signalsWebService.SetData(invalidID, GetDtoDatum<bool>());
            }

            [TestMethod]
            public void GivenASignal_WhenGetData_CalledDataRepository()
            {
                GivenASignal(SignalWith(DataType.Double));
                signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
                signalsDataRepositoryMock.Verify(s => s.GetData<double>(It.IsAny<Domain.Signal>(),
                                                                   It.IsAny<DateTime>(),
                                                                   It.IsAny<DateTime>()
                                                                   ));
            }

            [TestMethod]
            public void GivenASignal_WhenGetData_ReturnSortedByDateDatums()
            {
                GivenASignal(SignalWith(DataType.Boolean));
                SetupDataRepository<bool>();
                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1)).ToArray<Dto.Datum>().ToArray();

                bool sortedStatus = true;

                for (int i = 0; i < result.Length - 1; i++)
                {
                    int compareResult = DateTime.Compare(result[i].Timestamp, result[i + 1].Timestamp);
                    if (compareResult > 0) sortedStatus = false;
                }
                Assert.IsTrue(sortedStatus);

                Assert.IsNotNull(result[0].Value);
            }

            [TestMethod]
            public void GivenASignal_WhenGetData_ReturnDatumsWithMissingValues()
            {
                GivenASignal(SignalWith(DataType.Double));
                SetupDataRepository<double>();

                missingValuePolicyRepositoryMock
                    .Setup(s => s.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1)).ToArray<Dto.Datum>().ToArray();

                DateTime timeStampMin = result.Min(d => d.Timestamp);
                DateTime timeStampMax = result.Max(d => d.Timestamp);

                DateTime currentTimeSamp = timeStampMin;
                while (currentTimeSamp != timeStampMax)
                {
                    currentTimeSamp = currentTimeSamp.AddMonths(1);
                    var currentDatum = (from x in result
                                        where x.Timestamp == currentTimeSamp
                                        select x).FirstOrDefault();

                    if (currentDatum == null)
                    {
                        Assert.Fail("Result have missing values");
                    }
                }
            }

            [TestMethod]
            public void GivenASignal_WhenAddNewSignal_CalledSetMissingValuePolicy()
            {
                GivenASignal(SignalWith());

                var newSignal = SignalWith(id: null);

                var result = signalsWebService.Add(newSignal);

                missingValuePolicyRepositoryMock.Verify(s => s.Set(It.IsAny<Domain.Signal>(), It.IsAny<MissingValuePolicyBase>()));
            }

            [TestMethod]
            public void GivenASignal_WhenAddNewSignal_SetNoneQualityMissingValuePolicyForDefault()
            {
                GivenASignal(SignalWith());

                var newSignal = SignalWith(id: null);

                var result = signalsWebService.Add(newSignal);

                var mvp = signalsWebService.GetMissingValuePolicy(result.Id.Value);

                Assert.AreEqual("NoneQualityMissingValuePolicy", mvp.GetType().Name);
            }

            [TestMethod]
            public void GivenASignalWithSetNoneQualityMVP_WhenGettingDataWhenThereIsSingleDatumOfSignalInTheMiddleOfTheRange_RetrunedIsFilledArray()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Month, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(dummySignal.Id.Value)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Components.ToArray() == dummySignal.Path.Components.ToArray()))).Returns(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>
                    (s => s.Id == dummySignal.Id && s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDecimal());

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<decimal>(It.Is<Signal>
                    (s => s.Id == dummySignal.Id && s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))
                    , new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)))
                .Returns(new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000, 2, 1), Quality = Quality.Fair, Value = 10M } });

                var expectedArray = new Dto.Datum[] { new Dto.Datum() { Timestamp = new DateTime(2000, 1, 1), Quality = Dto.Quality.None, Value = default(decimal) },
                                                      new Dto.Datum() { Timestamp = new DateTime(2000, 2, 1), Quality = Dto.Quality.Fair, Value = 10M },
                                                      new Dto.Datum() { Timestamp = new DateTime(2000, 3, 1), Quality = Dto.Quality.None, Value = default(decimal) } };

                var resultArray = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)).ToArray();

                for (int i = 0; i < resultArray.Length; i++)
                {
                    Assert.AreEqual(expectedArray[i].Timestamp, resultArray[i].Timestamp);
                    Assert.AreEqual(expectedArray[i].Quality, resultArray[i].Quality);
                    Assert.AreEqual(expectedArray[i].Value, resultArray[i].Value);
                }
            }

            [TestMethod]
            public void GivenASignalWithSetNoneQualityMVP_WhenGettingDataWhenToDatetimeIsLessThanFromDatetime_ReturnedIsBlankListOfData()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Hour, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(dummySignal.Id.Value)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Components.ToArray() == dummySignal.Path.Components.ToArray()))).Returns(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>
                    (s => s.Id == dummySignal.Id && s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDecimal());

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(1999, 1, 1));
            }

            [TestMethod]
            public void GivenASignalWithSetNoneQualityMVPAndNoSetData_WhenGettingDataWhenTheGetDataTimeRangeIsCorrect_ReturnedIsNotBlankArray()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Month, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(dummySignal.Id.Value)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Components.ToArray() == dummySignal.Path.Components.ToArray()))).Returns(dummySignal);

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<decimal>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new Datum<decimal>[] { });

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>
                  (s => s.Id == dummySignal.Id && s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDecimal());

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2001, 1, 1));

                Assert.IsFalse(result.ToArray().Length == 0);
            }

            private void SetupDataRepository<T>()
            {
                signalsDataRepositoryMock
                    .Setup(s => s.GetData<T>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(GetDomainDatum<T>);
            }
            private IEnumerable<Dto.Datum> GetDtoDatum<T>()
            {
                return new Dto.Datum[] {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = default(T) },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = default(T) },
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2001, 1, 1), Value = default(T) } };
            }
            private IEnumerable<Domain.Datum<T>> GetDomainDatum<T>()
            {
                return new Domain.Datum<T>[] {
                    new Domain.Datum<T>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = default(T), Signal = SignalWith() },
                    new Domain.Datum<T>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = default(T), Signal = SignalWith() },
                    new Domain.Datum<T>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2001, 1, 1), Value = default(T), Signal = SignalWith() } };
            }
            private bool EqualsSignal(Signal a, Signal b)
            {
                if (a.Id != b.Id) return false;
                if (a.DataType != b.DataType) return false;
                if (a.Granularity != b.Granularity) return false;
                if (a.Path.ToString() != b.Path.ToString()) return false;
                return true;
            }
            private Signal SignalWith()
            {
                return new Signal()
                {
                    Id = 1,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("x/y"),
                };
            }
            private Signal SignalWith(DataType dataType)
            {
                return new Signal()
                {
                    Id = 1,
                    DataType = dataType,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("x/y"),
                };
            }
            private Dto.Signal SignalWith(
                int? id = null,
                Dto.DataType dataType = Dto.DataType.Boolean,
                Dto.Granularity granularity = Dto.Granularity.Month,
                Dto.Path path = null)
            {
                return new Dto.Signal()
                {
                    Id = id,
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }
            private Domain.Signal SignalWith(
                int id,
                Domain.DataType dataType,
                Domain.Granularity granularity,
                Domain.Path path
                )
            {
                return new Domain.Signal()
                {
                    Id = id,
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }

            private void GivenNoSignals()
            {
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s =>
                    {
                        s.Id = 1;
                        return s;
                    });

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Signal signal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Path))
                    .Returns(signal);

                missingValuePolicyRepositoryMock
                    .Setup(s => s.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger());
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
        }
    }
}