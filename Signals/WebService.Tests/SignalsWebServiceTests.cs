using System.Linq;
using System.Collections.Generic;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

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
            public void GivenNoSignals_WhenAddingSignal_SetsNoneQualityMissingValuePolicyForIt()
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => new Signal() { Id = 1, DataType = s.DataType, Granularity = s.Granularity, Path = s.Path });

                var sig = signalsWebService.Add(new Dto.Signal() {
                    DataType = Dto.DataType.Double,
                    Granularity = Dto.Granularity.Month,
                    Path = new Dto.Path() { Components = new[] { "root", "signal" } }
                });

                missingValuePolicyRepositoryMock.Verify(mvpr => mvpr.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>()));
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
            [ExpectedException(typeof(SignalNotFoundException))]
            public void GivenNoSignals_WhenSettingSignalData_ThrowSignalNotFoundException()
            {
                GivenNoSignals();

                var dummyId = 1;

                signalsWebService.SetData(dummyId, Enumerable.Empty<Dto.Datum>());
            }

            [TestMethod]
            public void GivenASignal_WhenSettingSignalData_CallDataRepositorySetData()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                signalsWebService.SetData(signalId,
                    new[] {
                        new Dto.Datum {
                            Value = 1,
                            Quality = Dto.Quality.Fair,
                            Timestamp = new System.DateTime()
                        }
                    });

                signalsDataRepositoryMock.Verify(sdr => sdr.SetData(
                    It.Is<IEnumerable<Domain.Datum<int>>>(
                        e =>
                             e.All(datum => datum.Signal.Id == signalId && datum.Value == 1
                                && datum.Quality == Quality.Fair && datum.Timestamp == new System.DateTime())
                        )));
            }

            [TestMethod]
            [ExpectedException(typeof(SignalNotFoundException))]
            public void GivenNoSignals_WhenGettingSignalData_ThrowSignalNotFoundException()
            {
                GivenNoSignals();

                var dummyId = 1;

                signalsWebService.GetData(dummyId, new System.DateTime(), new System.DateTime());
            }

            [TestMethod]
            public void GivenData_WhenGettingSignalData_ReturnsThisData()
            {
                var signalId = 1;
                var datumValue = 5;
                GivenData(SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")),
                    new[] { new Domain.Datum<int> { Value = datumValue } });

                var result = signalsWebService.GetData(signalId, new DateTime(), DateTime.Now);

                Assert.AreEqual(datumValue, result.First().Value);
            }

            [TestMethod]
            public void GivenData_WhenGettingSignalData_ReturnsSortedData()
            {
                var signalId = 1;
                double firstValue = 1;
                var firstTimestamp = new DateTime(2000, 1, 1);
                double lastValue = 2;
                var lastTimestamp = new DateTime(2000, 3, 1);

                var testDatumData = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double> { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                    new Domain.Datum<double> { Quality = Quality.Fair, Timestamp = firstTimestamp, Value = firstValue },
                    new Domain.Datum<double> { Quality = Quality.Poor, Timestamp = lastTimestamp, Value = lastValue }
                };

                GivenData(SignalWith(
                    id: signalId,
                    dataType: DataType.Double,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")),
                    testDatumData);

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 2, 1), new DateTime(2000, 3, 1));

                Assert.AreEqual(firstValue, result.First().Value);
                Assert.AreEqual(firstTimestamp, result.First().Timestamp);
                Assert.AreEqual(lastValue, result.Last().Value);
                Assert.AreEqual(lastTimestamp, result.Last().Timestamp);
            }

            [TestMethod]
            public void GivenData_WhenGettingSignalData_ReturnsDataWithEmptyDataDefaultValues()
            {
                int signalId = 1;

                var testDatumData = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double> { Quality = Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = (double)1.5 },
                    new Domain.Datum<double> { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 }
                };

                GivenData(SignalWith(
                    id: signalId,
                    dataType: DataType.Double,
                    granularity: Granularity.Month,
                    path: Domain.Path.FromString("root/signal")),
                    testDatumData);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Domain.Signal>())).Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 2, 1), new DateTime(2000, 4, 1));

                Assert.AreEqual(new DateTime(2000, 3, 1), result.ElementAt(1).Timestamp);
            }

            [TestMethod]
            [ExpectedException(typeof(SignalNotFoundException))]
            public void GivenNoSignals_WhenSettingMissingValuePolicy_ThrowSignalNotFoundException()
            {
                GivenNoSignals();

                var dummyId = 1;

                signalsWebService.SetMissingValuePolicy(dummyId, new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy());
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMissingPolicy_CallMissingValuePolicyRepositorySet()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                signalsWebService.SetMissingValuePolicy(signalId,
                    new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy { DataType = Dto.DataType.Integer });

                missingValuePolicyRepositoryMock.Verify(mvpr => mvpr.Set(
                    It.Is<Domain.Signal>(s => s.Id == signalId), It.IsAny<Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<int>>()));
            }

            [TestMethod]
            [ExpectedException(typeof(SignalNotFoundException))]
            public void GivenNoSignals_WhenGettingMissingValuePolicy_ThrowSignalNotFoundException()
            {
                GivenNoSignals();

                var dummyId = 1;

                var result = signalsWebService.GetMissingValuePolicy(dummyId);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingMissingValuePolicy_ReturnsNull()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.GetMissingValuePolicy(signalId);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenMissingValuePolicy_WhenGettingMissingValuePolicy_ReturnsThisPolicy()
            {
                var signalId = 1;
                GivenMissingValuePolicy(SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")),
                    new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyInteger());

                var result = signalsWebService.GetMissingValuePolicy(signalId);

                Assert.IsTrue(result is Dto.MissingValuePolicy.FirstOrderMissingValuePolicy);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingByPath_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.Get(new Dto.Path { Components = new string[0] });

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByItsPath_ReturnsIt()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.Get(new Dto.Path { Components = new[] { "root", "signal" } });

                Assert.AreEqual(signalId, result.Id);
                Assert.AreEqual(Dto.DataType.Integer, result.DataType);
                Assert.AreEqual(Dto.Granularity.Second, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByPathEntry_CalledRepository()
            {
                GivenASignal(SignalWith());
                signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root" } });
                signalsRepositoryMock.Verify(s => s.GetAllWithPathPrefix(It.IsAny<Path>()));
            }
            [TestMethod]
            public void GivenASignal_WhenGettingByPathEntry_ReturnCorrectSignalsAndFolders()
            {
                GivenASignal(SignalWith());

                List<string> correctSubPath = new List<string>() { "podkatalog", "podkatalog2" };
                List<Dto.Signal> correctSignals = new List<Dto.Signal> { SignalWith(id: 0, path: new Dto.Path() { Components = new[] { "root", "s1" } })};
                setupGetByPathEntry(new List<string>() { "root/s1"
                                                        , "root/podkatalog/s2"
                                                        , "root/podkatalog/s3"
                                                        , "root/podkatalog/podpodkatalog/s4"
                                                        , "root/podkatalog2/s5" });

                Dto.Path path = new Dto.Path() { Components = new[] { "root" } };
                Dto.PathEntry result = signalsWebService.GetPathEntry(path);
                List<Dto.Signal> resultSignals = result.Signals.ToList();
                List<Dto.Path> resultSubPath = result.SubPaths.ToList();

                CollectionAssert.AreEqual(correctSignals.Select(s => new { path = s.Path.ToString(), granularity = s.Granularity, dataType = s.DataType }).ToArray(), 
                                          resultSignals.Select (s => new { path = s.Path.ToString(), granularity = s.Granularity, dataType = s.DataType }).ToArray());

                CollectionAssert.AreEqual(correctSubPath, resultSubPath.Select(s => s.Components.Last()).ToList());

            }

            private void setupGetByPathEntry(IEnumerable<string> paths)
            {
                paths = paths.ToList();
                IEnumerable<Signal> signals = paths.Select((s, i) => new Signal() { Id = i,
                                                                                    DataType = DataType.Boolean,
                                                                                    Granularity = Granularity.Month,
                                                                                    Path = Path.FromString(s) });

                signalsRepositoryMock.Setup(s => s.GetAllWithPathPrefix(It.IsAny<Path>())).Returns(signals);
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

            private void GivenNoSignals()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
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
            }

            private void GivenData<T>(Signal signal, Datum<T>[] datum)
            {
                GivenASignal(signal);

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<T>(
                    signal, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(datum);
            }

            private void GivenMissingValuePolicy(Signal signal, Domain.MissingValuePolicy.MissingValuePolicyBase policy)
            {
                GivenASignal(signal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(signal)).Returns(policy);
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
        }
    }
}