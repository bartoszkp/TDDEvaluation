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

                var sig = signalsWebService.Add(new Dto.Signal()
                {
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
                    granularity: Granularity.Month,
                    path: Domain.Path.FromString("root/signal")),
                    new[] { new Domain.Datum<int> { Value = datumValue, Timestamp = new DateTime(2000, 1, 1) } });

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Domain.Signal>())).Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), DateTime.Now);

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
                    granularity: Granularity.Month,
                    path: Domain.Path.FromString("root/signal")),
                    testDatumData);
                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Domain.Signal>())).Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

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
            public void GivenData_WhenGettingSignalDataWithSpecificValuePolycy_ReturnsDataWithEmptyDataSpecyficValues()
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

                var Speycficmvp = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble() { Value = 44.12, Quality = Quality.Good };

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Domain.Signal>())).Returns(Speycficmvp);

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 2, 1), new DateTime(2000, 4, 1));

                Assert.AreEqual(44.12, result.ElementAt(1).Value);
                Assert.AreEqual(Dto.Quality.Good, result.ElementAt(1).Quality);
            }

            [TestMethod]
            public void GivenData_WhenGettingSignalDataWithTheSameDateTame_ReturnData()
            {
                int signalId = 1;

                var testDatumData = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double> { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                    new Domain.Datum<double> { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 }
                };

                GivenData(SignalWith(
                    id: signalId,
                    dataType: DataType.Double,
                    granularity: Granularity.Month,
                    path: Domain.Path.FromString("root/signal")),
                    testDatumData);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Domain.Signal>())).Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

                Assert.IsTrue(result.Count() != 0);
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
                List<Dto.Signal> correctSignals = new List<Dto.Signal> { SignalWith(id: 0, path: new Dto.Path() { Components = new[] { "root", "s1" } }) };
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
                                          resultSignals.Select(s => new { path = s.Path.ToString(), granularity = s.Granularity, dataType = s.DataType }).ToArray());

                CollectionAssert.AreEqual(correctSubPath, resultSubPath.Select(s => s.Components.Last()).ToList());

            }

            [TestMethod]
            public void GivenASignal_WhenSettingDataOfTheSignalByCorrectTimestamps_CalledIsSignalsDataRepositoryMockSetData()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
                var dummySignal = new Signal() { Id = 1, Granularity = Granularity.Minute, DataType = DataType.Decimal };
                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);

                signalsWebService.SetData(1, new Dto.Datum[] { new Dto.Datum() { Timestamp = new DateTime(2016, 8, 22, 1, 1, 0) } });

                signalsDataRepositoryMock.Verify(sdr => sdr.SetData(It.IsAny<IEnumerable<Datum<decimal>>>()));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingDataOfTheSignalByIncorrectTimestamps_ThrowedIsArgumentException()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
                var dummySignal = new Signal() { Id = 1, Granularity = Granularity.Minute, DataType = DataType.Decimal };
                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);

                signalsWebService.SetData(1, new Dto.Datum[] { new Dto.Datum() { Timestamp = new DateTime(2016, 8, 22, 1, 1, 1) } });

            }

            [TestMethod]
            public void GivenASignal_WhenGettingDataOfTheSignalByCorrectTime_CalledIsSignalsDataRepositoryMockGetData()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
                var dummySignal = new Signal() { Id = 1, Granularity = Granularity.Week, DataType = DataType.Decimal };
                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);

                signalsWebService.GetData(1, new DateTime(2016, 8, 22), new DateTime(2016, 8, 31));

                signalsDataRepositoryMock.Verify(sdr => sdr.GetData<decimal>(It.Is<Signal>(s =>
                    s.Id == dummySignal.Id && s.Granularity == dummySignal.Granularity && s.DataType == dummySignal.DataType),
                    new DateTime(2016, 8, 22), new DateTime(2016, 8, 31)));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingDataOfTheSignalByIncorrectTime_ThrowedIsArgumentException()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
                var dummySignal = new Signal() { Id = 1, Granularity = Granularity.Week, DataType = DataType.Decimal };
                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);

                signalsWebService.GetData(1, new DateTime(2016, 8, 23), new DateTime(2016, 8, 31));
            }

            [TestMethod]
            public void GivenASignalWithZOMVPAndNoData_WhenGettingDataByTwoSameDates_ReturnedIsListWithDefaultDatum()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Hour, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(Path.FromString("root/signal"))))).Returns(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>(s =>
                        s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))))
                    .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());

                var expectedList = new List<Dto.Datum>() { new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = default(decimal) } };

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

                Assert.AreEqual(expectedList[0].Quality, result.ToList()[0].Quality);
                Assert.AreEqual(expectedList[0].Timestamp, result.ToList()[0].Timestamp);
                Assert.AreEqual(expectedList[0].Value, result.ToList()[0].Value);
            }

            [TestMethod]
            public void GivenASIgnalWithZOMVPAndOneDatum_WhenGettingDataByTwoSameDatesWhichAreEqualToTheDatumsTimestamp_ReturnedIsListWithTheDatum()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Hour, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(Path.FromString("root/signal"))))).Returns(dummySignal);

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<decimal>(It.Is<Signal>(s =>
                    s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                    new DateTime(2000, 1, 1), new DateTime(2000, 1, 1)))
                    .Returns(new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000, 1, 1), Quality = Quality.Fair, Value = 10M } });

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>(s =>
                        s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))))
                    .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());

                var expectedList = new List<Dto.Datum>() { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 10M } };

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

                Assert.AreEqual(expectedList[0].Quality, result.ToList()[0].Quality);
                Assert.AreEqual(expectedList[0].Timestamp, result.ToList()[0].Timestamp);
                Assert.AreEqual(expectedList[0].Value, result.ToList()[0].Value);
            }

            [TestMethod]
            public void GivenASIgnalWithZOMVPAndOneDatum_WhenGettingDataFromDateEqualGivenDatumsTimestampToLaterDate_ReturnedIsExpectedList()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Month, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(Path.FromString("root/signal"))))).Returns(dummySignal);

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<decimal>(It.Is<Signal>(s =>
                    s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                    new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)))
                    .Returns(new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000, 1, 1), Quality = Quality.Fair, Value = 10M } });

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>(s =>
                        s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))))
                    .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());

                var expectedList = new List<Dto.Datum>()
                { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 10M },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = 10M },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = 10M } };

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                for (int i = 0; i < result.ToArray().Length; i++)
                {
                    Assert.AreEqual(expectedList[i].Quality, result.ToList()[i].Quality);
                    Assert.AreEqual(expectedList[i].Timestamp, result.ToList()[i].Timestamp);
                    Assert.AreEqual(expectedList[i].Value, result.ToList()[i].Value);

                }
            }

            [TestMethod]
            public void GivenASIgnalWithZOMVPAndOneDatum_WhenGettingDataFromDateEarlierThanTheDatumsTimestampToDateLaterThanTheDatumsTimestamp_ReturnedIsExpectedList()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Month, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(Path.FromString("root/signal"))))).Returns(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>(s =>
                       s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))))
                   .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<decimal>(It.Is<Signal>(s =>
                        s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                        new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)))
                    .Returns(new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000, 2, 1), Quality = Quality.Fair, Value = 10M } });

                var expectedList = new List<Dto.Datum>()
                { new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = 0M },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = 10M },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = 10M } };

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                for (int i = 0; i < result.ToArray().Length; i++)
                {
                    Assert.AreEqual(expectedList[i].Quality, result.ToList()[i].Quality);
                    Assert.AreEqual(expectedList[i].Timestamp, result.ToList()[i].Timestamp);
                    Assert.AreEqual(expectedList[i].Value, result.ToList()[i].Value);

                }
            }

            [TestMethod]
            public void GivenASignalWithZOMVPAndOneDatum_WhenGettingDataFromDateEarlierThenGivenDatumsTimestampToDateOneMonthEarlierThanTheDatumsTimestamp_ReturnedIsExpectedList()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Month, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(Path.FromString("root/signal"))))).Returns(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>(s =>
                       s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))))
                   .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<decimal>(It.Is<Signal>(s =>
                        s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                        new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)))
                    .Returns(new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000, 3, 1), Quality = Quality.Fair, Value = 10M } });

                var expectedList = new List<Dto.Datum>
                { new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = 0M },
                  new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 2, 1), Value = 0M },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = 10M } };

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                for (int i = 0; i < result.ToArray().Length; i++)
                {
                    Assert.AreEqual(expectedList[i].Quality, result.ToList()[i].Quality);
                    Assert.AreEqual(expectedList[i].Timestamp, result.ToList()[i].Timestamp);
                    Assert.AreEqual(expectedList[i].Value, result.ToList()[i].Value);

                }
            }

            [TestMethod]
            public void GivenASignalWithZOMVPAndTwoDatums_WhenGettingDataFromDateEarlierThanGivenDatumsTimestampsToDateLaterThanGivenDatumsTimestamps_ReturnedIsExpectedList()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Month, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(Path.FromString("root/signal"))))).Returns(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>(s =>
                       s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))))
                   .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<decimal>(It.Is<Signal>(s =>
                        s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                        new DateTime(2000, 1, 1), new DateTime(2000, 5, 1)))
                    .Returns(new Datum<decimal>[] { new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = 1M },
                                                    new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 2M } });

                var expectedList = new List<Dto.Datum>
                { new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = 0M },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = 1M },
                  new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 2M },
                  new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 2M } };

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));

                for (int i = 0; i < result.ToArray().Length; i++)
                {
                    Assert.AreEqual(expectedList[i].Quality, result.ToList()[i].Quality);
                    Assert.AreEqual(expectedList[i].Timestamp, result.ToList()[i].Timestamp);
                    Assert.AreEqual(expectedList[i].Value, result.ToList()[i].Value);
                }
            }

            [TestMethod]
            public void GivenASignalWithZOMVPAndTwoDatums_WHenGettingDataFromDateEqualFirstDatumsTimestampToDateMonthEarlierEqualSecondDatumsTimestamp_ReturnedIsExpectedList()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Month, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(Path.FromString("root/signal"))))).Returns(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>(s =>
                       s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))))
                   .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<decimal>(It.Is<Signal>(s =>
                        s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                        new DateTime(2000, 1, 1), new DateTime(2000, 5, 1)))
                    .Returns(new Datum<decimal>[] { new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 1M },
                                                    new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 2M } });
                var expectedList = new List<Dto.Datum>
                { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 1M },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = 1M },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = 1M },
                  new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 2M } };

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));

                for (int i = 0; i < result.ToArray().Length; i++)
                {
                    Assert.AreEqual(expectedList[i].Quality, result.ToList()[i].Quality);
                    Assert.AreEqual(expectedList[i].Timestamp, result.ToList()[i].Timestamp);
                    Assert.AreEqual(expectedList[i].Value, result.ToList()[i].Value);
                }

            }

            [TestMethod]
            public void GivenASignalWithZOMVPAndOneDatum_WhenGettingDataOutOfThisDatumsRange_ReturnsCorrectData()
            {
                var dummySignal = new Signal()
                {
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Id = 1
                };
                GivenASignal(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                   .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());

                var date = new DateTime(2000, 1, 1);
                var data = new[] { new Datum<double> { Quality = Quality.Fair, Value = 2.5, Timestamp = date } };

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<double>(dummySignal, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new Datum<double>[0]);

                signalsDataRepositoryMock.Setup(sdr => sdr.GetDataOlderThan<double>(dummySignal, It.Is<DateTime>(d => d > date), 1))
                    .Returns(data);

                var result = signalsWebService.GetData(1, date.AddMonths(1), date.AddMonths(2));

                foreach (var datum in result)
                {
                    Assert.AreEqual(Dto.Quality.Fair, datum.Quality);
                    Assert.AreEqual(2.5, datum.Value);
                }
            }

            [TestMethod]
            public void GivenASignalWithFOMVPAndTwoDatums_WhenGettingDataBetweenThem_ReturnsInterpolatedData()
            {
                var dummySignal = new Signal()
                {
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Id = 1
                };
                GivenASignal(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                   .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDouble());

                var date = new DateTime(2000, 1, 1);

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<double>(dummySignal, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new[] {
                        new Datum<double>
                        {
                            Quality = Quality.Fair,
                            Timestamp = date,
                            Value = 1.0
                        },
                        new Datum<double>
                        {
                            Quality = Quality.Bad,
                            Timestamp = date.AddMonths(5),
                            Value = 6.0
                        } });

                var result = signalsWebService.GetData(1, date, date.AddMonths(6)).ToArray();

                var expectedResult = new[]
                {
                    new Dto.Datum
                    {
                        Quality = Dto.Quality.Fair,
                        Timestamp = date,
                        Value = 1.0
                    },
                    new Dto.Datum
                    {
                        Quality = Dto.Quality.Bad,
                        Timestamp = date.AddMonths(1),
                        Value = 2.0
                    },
                    new Dto.Datum
                    {
                        Quality = Dto.Quality.Bad,
                        Timestamp = date.AddMonths(2),
                        Value = 3.0
                    },
                    new Dto.Datum
                    {
                        Quality = Dto.Quality.Bad,
                        Timestamp = date.AddMonths(3),
                        Value = 4.0
                    },
                    new Dto.Datum
                    {
                        Quality = Dto.Quality.Bad,
                        Timestamp = date.AddMonths(4),
                        Value = 5.0
                    },
                    new Dto.Datum
                    {
                        Quality = Dto.Quality.Bad,
                        Timestamp = date.AddMonths(5),
                        Value = 6.0
                    }
                };

                for (int i = 0; i < expectedResult.Length; i++)
                {
                    Assert.AreEqual(expectedResult[i].Quality, result[i].Quality);
                    Assert.AreEqual(expectedResult[i].Value, result[i].Value);
                }
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.InvalidPolicyDataTypeException))]
            public void GivenASignalWithFOMVPOfTypeString_WhenGettingData_ThrowsException()
            {
                var dummySignal = new Signal()
                {
                    DataType = DataType.String,
                    Granularity = Granularity.Month,
                    Id = 1
                };
                GivenASignal(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                   .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyString());

                var date = new DateTime(2000, 1, 1);
                var result = signalsWebService.GetData(1, date, date.AddMonths(6));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.InvalidPolicyDataTypeException))]
            public void GivenASignalWithFOMVPOfTypeBoolean_WhenGettingData_ThrowsException()
            {
                var dummySignal = new Signal()
                {
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Month,
                    Id = 1
                };
                GivenASignal(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                   .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyBoolean());

                var date = new DateTime(2000, 1, 1);
                var result = signalsWebService.GetData(1, date, date.AddMonths(6));
            }

            private void setupGetByPathEntry(IEnumerable<string> paths)
            {
                paths = paths.ToList();
                IEnumerable<Signal> signals = paths.Select((s, i) => new Signal()
                {
                    Id = i,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Month,
                    Path = Path.FromString(s)
                });

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