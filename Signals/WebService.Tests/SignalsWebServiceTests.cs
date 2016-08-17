using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;

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
                prepareMissingValuePolicy(1, new Signal()
                {
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path = Domain.Path.FromString("root/signal"),
                });

                signalsRepositoryMock
                    .Setup(srm => srm.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);

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

                prepareMissingValuePolicy(1, new Signal()
                {
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("example/path"),
                });

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
            public void WhenGettingByPath_ReturnsIt()
            {
                GivenNoSignals();
                var signalDomain = new Domain.Signal()
                {
                    Id = 23,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("sfd/klpko"),
                };
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.Is<Domain.Path>(p => p.ToString() == signalDomain.Path.ToString())))
                    .Returns(signalDomain);

                var signalDto = signalDomain.ToDto<Dto.Signal>();
                var result = signalsWebService.Get(signalDto.Path);

                Assert.AreEqual(signalDto.Id, result.Id);
                Assert.AreEqual(signalDto.DataType, result.DataType);
                Assert.AreEqual(signalDto.Granularity, result.Granularity);
                Assert.AreEqual(signalDto.Path.ToString(), result.Path.ToString());
            }

            [TestMethod]
            public void WhenGettingMissingValuePolicyForNewSignal_ReturnsNull()
            {
                var signal = new Domain.Signal()
                {
                    Id = 23
                };
                prepareMissingValuePolicy(signal.Id.Value, signal);
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Get(It.Is<Domain.Signal>(s => s == signal)))
                    .Returns<DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger>(null);

                var result = signalsWebService.GetMissingValuePolicy(signal.Id.Value);
                Assert.IsNull(result);
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.NoSuchSignalException))]
            public void WhenGettingMissingValuePolicyForNonExistSignal_ThrowSignalWithThisIdNonExistException()
            {
                int signalId = 249;
                prepareMissingValuePolicy(signalId, null);
                signalsWebService.GetMissingValuePolicy(signalId);
            }

            [TestMethod]
            public void WhenGettingMissingValuePolicy_ReturnsIt()
            {
                var signal = new Domain.Signal()
                {
                    Id = 65,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Day,
                    Path = Path.FromString("jkl/fg")
                };
                prepareMissingValuePolicy(signal.Id.Value, signal);
                var mvp = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                {
                    Id = 45,
                    Quality = Quality.Fair,
                    Signal = signal,
                    Value = (double)2.25
                };
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Get(It.Is<Domain.Signal>(s => s == signal)))
                    .Returns(mvp);

                var mvpDto = mvp.ToDto<Dto.MissingValuePolicy.SpecificValueMissingValuePolicy>();

                Dto.MissingValuePolicy.MissingValuePolicy result
                    = signalsWebService.GetMissingValuePolicy(signal.Id.Value);
                Assert.AreEqual(mvpDto.Id, result.Id);
                Assert.AreEqual(mvpDto.DataType, result.DataType);
                Assert.AreEqual(mvpDto.Signal.Id, result.Signal.Id);
                Assert.AreEqual(mvpDto.Signal.DataType, result.Signal.DataType);
                Assert.AreEqual(mvpDto.Signal.Granularity, result.Signal.Granularity);
                Assert.AreEqual(mvpDto.Signal.Path.ToString(), result.Signal.Path.ToString());
            }

            [TestMethod]
            public void WhenSettingMissingValuePolicy_CallsRepositoryGet()
            {
                var signal = new Domain.Signal()
                {
                    Id = 23,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("r/vbc")
                };
                prepareMissingValuePolicy(signal.Id.Value, signal);
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Set(It.Is<Domain.Signal>(s => s == signal), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

                var mvp = new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy()
                {
                    DataType = Dto.DataType.Boolean,
                    Signal = signal.ToDto<Dto.Signal>()
                };
                var mvpDomain = mvp.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

                signalsWebService.SetMissingValuePolicy(signal.Id.Value, mvp);

                missingValuePolicyRepositoryMock.Verify(mvpr => mvpr.Set(
                    It.Is<Domain.Signal>(s => s == signal),
                    It.Is<Domain.MissingValuePolicy.MissingValuePolicyBase>(
                        m => m.NativeDataType == mvpDomain.NativeDataType
                        && m.Signal.Id == signal.Id
                        && m.Signal.DataType == signal.DataType
                        && m.Signal.Granularity == signal.Granularity
                        && m.Signal.Path.ToString() == signal.Path.ToString())));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.NoSuchSignalException))]
            public void WhenSettingMissingValuePolicyForNonExistSignal_ThrowSignalWithThisIdNonExistException()
            {
                int signalId = 249;
                prepareMissingValuePolicy(signalId, null);
                signalsWebService.SetMissingValuePolicy(signalId, new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy());
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.NoSuchSignalException))]
            public void WhenGettingDataForNonExistSignal_ThrowSignalWithThisIdNonExistException()
            {
                int signalId = 78;
                prepareDataRepository(signalId, null);
                signalsWebService.GetData(signalId, System.DateTime.MinValue, System.DateTime.Today);
            }

            [TestMethod]
            public void WhenGettingDataForExistSignal_ReturnsData()
            {
                var signal = new Domain.Signal()
                {
                    Id = 978,
                    DataType = DataType.Double,
                    Granularity = Granularity.Week,
                    Path = Path.FromString("ghf/vbc")
                };
                prepareDataRepository(signal.Id.Value, signal);
                var enumerable = new Dto.Datum[] {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 1, 1), Value = (double)1 },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 8), Value = (double)1.5 }};
                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetData<double>(
                        It.Is<Domain.Signal>(s => s == signal),
                        It.Is<System.DateTime>(dt => dt == new DateTime(2000, 1, 1)),
                        It.Is<System.DateTime>(dt => dt == new DateTime(2000, 1, 15))))
                    .Returns(enumerable.ToDomain<System.Collections.Generic.IEnumerable<Datum<double>>>);
                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 15));

                int i = 0;
                foreach (var d in result)
                {
                    Assert.AreEqual(enumerable[i].Quality, d.Quality);
                    Assert.AreEqual(enumerable[i].Timestamp, d.Timestamp);
                    Assert.AreEqual(enumerable[i].Value, d.Value);
                    ++i;
                }
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.NoSuchSignalException))]
            public void WhenSettingDataForNonExistSignal_ThrowSignalWithThisIdNonExistException()
            {
                int signalId = 546;
                prepareDataRepository(signalId, null);
                signalsWebService.SetData(signalId, null);
            }

            [TestMethod]
            public void WhenSettingDataForExistSignal_CallsRepositorySetData()
            {
                var signal = new Domain.Signal()
                {
                    Id = 567,
                    DataType = DataType.Integer,
                    Granularity = Granularity.Week,
                    Path = Path.FromString("sfda/xvbc/jhkl")
                };
                prepareDataRepository(signal.Id.Value, signal);
                signalsDataRepositoryMock
                    .Setup(sdr => sdr.SetData<int>(It.IsAny<IEnumerable<Domain.Datum<int>>>()));

                var enumerable = new Dto.Datum[] {
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2016, 1, 16), Value = 7 },
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new System.DateTime(2020, 2, 21), Value = 2 },
                    new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new System.DateTime(2003, 3, 18), Value = 78 } };
                signalsWebService.SetData(signal.Id.Value, enumerable);

                var enumerableSorted = enumerable.OrderBy(dtoDatum => dtoDatum.Timestamp);

                signalsDataRepositoryMock.Verify(sdr => sdr.SetData<int>(
                    It.Is<IEnumerable<Domain.Datum<int>>>(
                        d => IEnumerableDatumAreEqual(enumerableSorted, d, signal))));
            }

            [TestMethod]
            public void GivenNoSignal_WhenGettingById_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.GetById(1);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GiveNoSignal_WhenGettingByPath_ReturnsNull()
            {
                GivenNoSignals();

                Dto.Path dtoPath = new Dto.Path() { Components = new[] { "not", "existing", "path" } };

                var result = signalsWebService.Get(dtoPath);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenAddingNewSignal_NoneQualityMissingValuePolicy_IsDefault()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>()));

                signalsWebService.Add(new Dto.Signal()
                {
                    DataType = Dto.DataType.Integer,
                    Granularity = Dto.Granularity.Day,
                    Path = new Dto.Path() { Components = new[] { "example", "path" } },
                });

                missingValuePolicyRepositoryMock.Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>()));
            }

            [TestMethod]
            public void GivenNoSignal_WhenGettingData_ReturnsCompletedData_WithFilledMissingData()
            {
                var from = new DateTime(2000, 1, 1, 4, 30, 30);
                var to = new DateTime(2000, 1, 4, 4, 30, 30);

                var expectedResult = new List<Dto.Datum>();
                expectedResult = prepareExpectedResult();

                SetupGetRepositories(from, to, expectedResult);

                var result = signalsWebService.GetData(1, from, to);

                int i = 0;
                foreach (var d in result)
                {
                    Assert.AreEqual(expectedResult[i].Quality, d.Quality);
                    Assert.AreEqual(expectedResult[i].Timestamp, d.Timestamp);
                    Assert.AreEqual(expectedResult[i].Value, d.Value);
                    ++i;
                }
            }
            

            private void SetupGetRepositories(DateTime fromIncluded, DateTime toExcluded, List<Dto.Datum> expectedResult)
            {
                var domainSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("path"),
                };
                prepareDataRepository(domainSignal.Id.Value, domainSignal);

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<bool>(domainSignal, fromIncluded, toExcluded))
                    .Returns(expectedResult.ToDomain<IEnumerable<Datum<bool>>>);

                signalsRepositoryMock
                    .Setup(srm => srm.Get(It.IsAny<int>()))
                    .Returns(domainSignal);
            }

            private List<Dto.Datum> prepareExpectedResult()
            {
                List<Dto.Datum> toReturn = new List<Dto.Datum>();
                toReturn.Add(new Dto.Datum()
                {
                    Quality = Dto.Quality.Good,
                    Timestamp = new DateTime(2000, 1, 1, 4, 30, 30),
                    Value = false,
                });
                toReturn.Add(new Dto.Datum()
                {
                    Quality = Dto.Quality.Fair,
                    Timestamp = new DateTime(2000, 1, 2, 4, 30, 30),
                    Value = false,
                });
                toReturn.Add(new Dto.Datum()
                {
                    Quality = Dto.Quality.Good,
                    Timestamp = new DateTime(2000, 1, 3, 4, 30, 30),
                    Value = false,
                });

                return toReturn;
            }

            private Dto.Signal SignalWith(
                int? id = null,
                Dto.DataType dataType = Dto.DataType.Boolean,
                Dto.Granularity granularity = Dto.Granularity.Day,
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
                Domain.Path path)
            {
                return new Domain.Signal()
                {
                    Id = id,
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }

            private bool IEnumerableDatumAreEqual<T>(IEnumerable<Dto.Datum> datumDto,
                IEnumerable<Domain.Datum<T>> datumDomain,
                Domain.Signal signal)
            {
                foreach (var dt in datumDto.Zip(datumDomain, System.Tuple.Create))
                {
                    var datum = dt.Item2.ToDto<Dto.Datum>();
                    if (!(dt.Item1.Quality == datum.Quality
                        && dt.Item1.Timestamp == datum.Timestamp
                        && signal.Id == dt.Item2.Signal.Id
                        && signal.DataType == dt.Item2.Signal.DataType
                        && signal.Granularity == dt.Item2.Signal.Granularity
                        && signal.Path.ToString() == dt.Item2.Signal.Path.ToString()))
                        return false;
                }
                return true;
            }

            private void GivenNoSignals()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Signal signal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
            }

            private void prepareMissingValuePolicy(int signalId, Domain.Signal signal)
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Get(signalId))
                    .Returns(signal);
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void prepareDataRepository(int signalId, Domain.Signal signal = null)
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Get(signalId))
                    .Returns(signal);
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }
        }
    }
}