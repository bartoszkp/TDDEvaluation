using System;
using System.Linq;
using System.Collections.Generic;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DataAccess;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            // -------------------------------------------------------------------------------------------
            // 1st Iteration:
            // -------------------------------------------------------------------------------------------

            ISignalsWebService signalsWebService1stIteration;
            Mock<ISignalsRepository> signalsRepositoryMock1stIteration;

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsNotNull()
            {
                GivenNoSignals();

                var result = signalsWebService1stIteration.Add(new Dto.Signal());

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsTheSameSignalExceptForId()
            {
                GivenNoSignals();

                var result = signalsWebService1stIteration.Add(SignalWith(
                    dataType: Dto.DataType.Decimal,
                    granularity: Dto.Granularity.Week,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }));

                Assert.AreEqual(Dto.DataType.Decimal, result.DataType);
                Assert.AreEqual(Dto.Granularity.Week, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_PassesGivenSignalToRepositoryAdd()
            {
                GivenNoSignals();

                signalsWebService1stIteration.Add(SignalWith(
                    dataType: Dto.DataType.Decimal,
                    granularity: Dto.Granularity.Week,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }));

                signalsRepositoryMock1stIteration.Verify(sr => sr.Add(It.Is<Domain.Signal>(passedSignal
                    => passedSignal.DataType == DataType.Decimal
                        && passedSignal.Granularity == Granularity.Week
                        && passedSignal.Path.ToString() == "root/signal")));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsIdFromRepository()
            {
                var signalId = 1;
                GivenNoSignals();
                GivenRepositoryThatAssigns(signalId);
                var result = signalsWebService1stIteration.Add(SignalWith(
                    dataType: Dto.DataType.Decimal,
                    granularity: Dto.Granularity.Week,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }));

                Assert.AreEqual(signalId, result.Id);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_DoesNotThrow()
            {
                GivenNoSignals();

                signalsWebService1stIteration.GetById(0);
            }


            [TestMethod]
            public void GivenASignal_WhenGettingByItsId_ReturnsIt()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Year,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService1stIteration.GetById(signalId);

                Assert.AreEqual(signalId, result.Id);
                Assert.AreEqual(Dto.DataType.String, result.DataType);
                Assert.AreEqual(Dto.Granularity.Year, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_RepositoryGetIsCalledWithGivenId()
            {
                var signalId = 1;
                GivenNoSignals();

                signalsWebService1stIteration.GetById(signalId);

                signalsRepositoryMock1stIteration.Verify(sr => sr.Get(signalId));
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService1stIteration.GetById(0);

                Assert.IsNull(result);
            }

            private Dto.Signal SignalWith(Dto.DataType dataType, Dto.Granularity granularity, Dto.Path path)
            {
                return new Dto.Signal()
                {
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }

            private Domain.Signal SignalWith(int id, Domain.DataType dataType, Domain.Granularity granularity, Domain.Path path)
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
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock1stIteration = new Mock<ISignalsRepository>();
                signalsRepositoryMock1stIteration
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock1stIteration.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService1stIteration = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Domain.Signal existingSignal)
            {
                GivenNoSignals();

                signalsRepositoryMock1stIteration
                    .Setup(sr => sr.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);
            }

            private void GivenRepositoryThatAssigns(int id)
            {
                signalsRepositoryMock1stIteration
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s =>
                    {
                        s.Id = id;
                        return s;
                    });
            }

            // -------------------------------------------------------------------------------------------
            // 2nd Iteration:
            // -------------------------------------------------------------------------------------------

            [TestMethod]
            public void GivenASignal_WhenGettingByPathWhichExistsInRepository_ReturnedIsTheSignal()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, null);
                var signalWebService = new SignalsWebService(signalDomainService);
                var dummySignal = new Dto.Signal()
                {
                    DataType = Dto.DataType.Decimal,
                    Granularity = Dto.Granularity.Hour,
                    Path = new Dto.Path() { Components = new string[] { "root", "signal1" } }
                };
                var dummyPath = Path.FromString("root/signal1");
                signalRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(path => path.Equals(dummyPath)))).Returns(dummySignal.ToDomain<Domain.Signal>());

                var result = signalWebService.Get(dummyPath.ToDto<Dto.Path>());

                Assert.AreEqual(dummySignal.Granularity, result.Granularity);
                Assert.AreEqual(dummySignal.DataType, result.DataType);
                CollectionAssert.AreEqual(dummySignal.Path.Components.ToArray(), result.Path.Components.ToArray());
            }

            // -------------------------------------------------------------------------------------------

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenGettingDataOfAnySignal_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, signalDataRepositoryMock.Object, null);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.GetData(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>());
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingDataOfOtherSignal_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, signalDataRepositoryMock.Object, null);
                var signalWebService = new SignalsWebService(signalDomainService);
                signalRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal());
                signalRepositoryMock.Setup(sr => sr.Get(2)).Returns<Signal>(null);

                signalWebService.GetData(2, It.IsAny<DateTime>(), It.IsAny<DateTime>());
            }

            [TestMethod]
            public void GivenASignal_WhenGettingDataOfTheSignal_ReturnedIsTheData()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                var signalsWebService = new SignalsWebService(signalsDomainService);

                var dummySignal = new Signal()
                {
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("root/signal")
                };
                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetData<decimal>(dummySignal, new DateTime(2015, 1, 1), new DateTime(2017, 1, 1)))
                    .Returns(new Datum<decimal>[] { new Datum<decimal>() { Id=1 , Quality=Quality.Fair, Signal = dummySignal, Timestamp=new DateTime(2016,1,1), Value=10M } });
                var expected = new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 1, 1), Value = 10M };

                var result = signalsWebService.GetData(1, new DateTime(2015, 1, 1), new DateTime(2017, 1, 1));

                foreach (var item in result)
                {
                    Assert.AreEqual(expected.Quality, item.Quality);
                    Assert.AreEqual(expected.Timestamp, item.Timestamp);
                    Assert.AreEqual(expected.Value, item.Value);
                }
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenSettingDataOfAnySignal_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, signalDataRepositoryMock.Object, null);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.SetData(1, null);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingDataOfOtherSignal_ReturnedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                signalRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal());
                var signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, signalDataRepositoryMock.Object, null);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.SetData(2, null);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingDataOfTheSignalAndGettingTheData_GotIsNotNullData()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                signalRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal());
                var signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalDataRepositoryMock.Setup(sdr => sdr.GetData<double>(It.Is<Signal>(signal => signal.Id == 1), new DateTime(2015, 1, 1), new DateTime(2017, 1, 1)))
                    .Returns(new Datum<double>[] { new Datum<double>() { Timestamp = new DateTime(2016, 1, 1), Value = 10.0, Quality = Quality.Fair } });
                signalWebService.SetData(1, new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 1, 1), Value = 10.0 } });
                var result = signalWebService.GetData(1, new DateTime(2015, 1, 1), new DateTime(2017, 1, 1));

                Assert.IsNotNull(result);
            }

            // -------------------------------------------------------------------------------------------

          

            // -------------------------------------------------------------------------------------------
            // 3rd Iteration:
            // -------------------------------------------------------------------------------------------

            [TestMethod]
            public void GivenNoSignals_WhenGettingByAnyPath_ReturnedIsNull()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<Path>())).Returns<Signal>(null);

                var result = signalsWebService.Get(new Dto.Path() { Components = new string[] { "root", "signal" } });

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingByAnyId_ReturnedIsNull()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns<Signal>(null);

                var result = signalsWebService.GetById(1);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByOtherPath_ReturnedIsNull()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.Is<Path>(path => path.Components.ToArray() == new string[] { "root", "signal1" })))
                    .Returns(new Signal());
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.Is<Path>(path => path.Components.ToArray() == new string[] { "root", "signal2" })))
                    .Returns<Signal>(null);

                var result = signalsWebService.Get(new Dto.Path() { Components = new string[] { "root", "signal2" } });

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByOtherId_ReturnedIsNull()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal());
                signalsRepositoryMock.Setup(sr => sr.Get(2)).Returns<Signal>(null);

                var result = signalsWebService.GetById(2);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByTheSignalsPath_ReturnedIsNotNullResult()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.Is<Path>(path => path.Equals(Path.FromString("root/signal1")))))
                    .Returns(new Signal());

                var result = signalsWebService.Get(new Dto.Path() { Components = new string[] { "root", "signal1" } });

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByTheSignalsId_ReturnedIsNotNullResult()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(new Signal());

                var result = signalsWebService.GetById(1);

                Assert.IsNotNull(result);
            }

            // -------------------------------------------------------------------------------------------

            [TestMethod]
            public void GivenSignalWithDataSetNotInDateOrder_WhenGettingDataOfTheSignal_ReturnedIsSortedArray()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                var dummySignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("root/signal")
                };

                signalsRepositoryMock.Setup(sr => sr.Get(dummySignal.Id.Value)).Returns(dummySignal);

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<double>(
                    It.Is<Signal>(s => s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                    It.Is<DateTime>(dt => dt.Year == 2000 && dt.Month == 1 && dt.Day == 1), It.Is<DateTime>(dt => dt.Year == 2000 && dt.Month == 3 && dt.Day == 1)))
                .Returns(new Datum<double>[]
                { new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = (double)1 },
                  new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 }});

                var result = signalsWebService.GetData(dummySignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                Assert.AreEqual(new DateTime(2000, 1, 1), result.ToArray()[0].Timestamp);
                Assert.AreEqual(new DateTime(2000, 2, 1), result.ToArray()[1].Timestamp);
            }

            // -------------------------------------------------------------------------------------------

            [TestMethod]
            public void GivenANewlyAddedSignal_WhileAddingThatSignal_WasCalledSetMissingValuePolicyMethod()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                var dummySignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("root/signal")
                };
                signalsRepositoryMock.Setup(sr => sr.Add(It.Is<Signal>(s =>
                    s.DataType == dummySignal.DataType &&
                    s.Granularity == dummySignal.Granularity &&
                    s.Path.Equals(dummySignal.Path)))).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);

                signalsWebService.Add(dummySignal.ToDto<Dto.Signal>());

                missingValuePolicyRepositoryMock.Verify(mvpr => mvpr.Set(
                    It.Is<Signal>(s => s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                    It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<bool>>()));
            }

            // -------------------------------------------------------------------------------------------

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenGettingMVPOfAnySignal_ThrowedIsArgumentException()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns<Signal>(null);
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<Path>())).Returns<Signal>(null);

                signalsWebService.GetMissingValuePolicy(1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingMVPOfOtherSignal_ThrowedIsArgumentException()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Hour, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(dummySignal.Path)))).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<int>(i => !(i == dummySignal.Id)))).Returns<Signal>(null);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => !(p.Equals(dummySignal.Path))))).Returns<Signal>(null);

                signalsWebService.GetMissingValuePolicy(2);
            }

            [TestMethod]
            public void GivenANewlyAddedSignal_WhenGettingMVPOfTheSignal_ReturnedIsNull()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Hour, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(dummySignal.Path)))).Returns(dummySignal);

                var result = signalsWebService.GetMissingValuePolicy(1);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignalWithSetMVP_WhenGettingMVPOfTheSignal_ReturnedIsTheMVP()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Hour, Path = Path.FromString("root/signal") };
                var dummyMVP = new Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<bool>() { Id = 1, Quality = Quality.Fair, Signal = dummySignal, Value = true };

                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(dummySignal.Path)))).Returns(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>
                    (s => s.Id == dummySignal.Id && s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.DataType == dummySignal.DataType)))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyBoolean() { Id = 1, Quality = Quality.Fair, Signal = dummySignal, Value = true, });

                var result = signalsWebService.GetMissingValuePolicy(1);

                Assert.AreEqual(dummyMVP.Id, result.Id);
                Assert.AreEqual(dummyMVP.Signal.Id, result.Signal.Id);
                Assert.AreEqual(dummyMVP.Signal.DataType, result.Signal.DataType.ToDomain<Domain.DataType>());
                Assert.AreEqual(dummyMVP.Signal.Granularity, result.Signal.Granularity.ToDomain<Domain.Granularity>());
                CollectionAssert.AreEqual(dummyMVP.Signal.Path.Components.ToArray(), result.Signal.Path.Components.ToArray());
                Assert.AreEqual(Dto.DataType.Boolean, result.DataType);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenSettingMVPOfAnySignal_ThrowedIsArgumentException()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns<Signal>(null);
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<Path>())).Returns<Signal>(null);

                signalsWebService.SetMissingValuePolicy(1, new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy());
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingMVPOfOtherSignal_ThrowedIsArgumentException()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                var signalsWebService = new SignalsWebService(signalsDomainService);
                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Hour, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(dummySignal.Path)))).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<int>(i => !(i == dummySignal.Id)))).Returns<Signal>(null);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => !(p.Equals(dummySignal.Path))))).Returns<Signal>(null);

                signalsWebService.SetMissingValuePolicy(2, new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy());
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMVPOfTheSignal_MVPRepositorySetMethodWasCalled()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                var signalsWebService = new SignalsWebService(signalsDomainService);

                var dummySignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Hour, Path = Path.FromString("root/signal") };
                var dummyMVP = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
                { Id = 1, DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Fair, Signal = dummySignal.ToDto<Dto.Signal>(), Value = false };

                signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(dummySignal.Path)))).Returns(dummySignal);

                signalsWebService.SetMissingValuePolicy(1, dummyMVP);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Set
                    (It.Is<Signal>(s => s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                    It.Is<Domain.MissingValuePolicy.MissingValuePolicyBase>(mvp => mvp.Id == dummyMVP.Id && mvp.NativeDataType == dummyMVP.Value.GetType())));
            }

            // -------------------------------------------------------------------------------------------

            [TestMethod]
            public void GivenASignalWithDataAndSetNoneQualityMVP_WhenGettingDataOfTheSignal_ReturnedIsFilledArray()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                var signalsWebService = new SignalsWebService(signalsDomainService);

                var dummySignal = new Signal() { Id = 1, DataType = DataType.Double, Granularity = Granularity.Month, Path = Path.FromString("root/signal") };

                signalsRepositoryMock.Setup(sr => sr.Get(dummySignal.Id.Value)).Returns(dummySignal);
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<Path>(p => p.Equals(dummySignal.Path)))).Returns(dummySignal);

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get
                    (It.Is<Domain.Signal>(s => s.Id == dummySignal.Id && s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))))
                .Returns(new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>());

                signalDataRepositoryMock.Setup(sdr => sdr.GetData<double>
                    (It.Is<Signal>(s => s.Id == dummySignal.Id && s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                    new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)))
                .Returns(new Datum<double>[] { new Datum<double>() { Value = 10.0, Signal = dummySignal, Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Id = 1 } });

                var expectedArray = new Dto.Datum[]
                {
                    new Dto.Datum() {Timestamp = new DateTime(2000,1,1), Quality = Dto.Quality.None, Value = 0.0 },
                    new Dto.Datum() {Timestamp = new DateTime(2000,2,1), Quality = Dto.Quality.Fair, Value = 10.0 },
                    new Dto.Datum() {Timestamp = new DateTime(2000,3,1), Quality=Dto.Quality.None, Value=0.0 }
                };

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                Assert.AreEqual(expectedArray[0].Quality, result.ToArray()[0].Quality);
                Assert.AreEqual(expectedArray[0].Timestamp, result.ToArray()[0].Timestamp);
                Assert.AreEqual(expectedArray[0].Value, result.ToArray()[0].Value);
                Assert.AreEqual(expectedArray[1].Quality, result.ToArray()[1].Quality);
                Assert.AreEqual(expectedArray[1].Timestamp, result.ToArray()[1].Timestamp);
                Assert.AreEqual(expectedArray[1].Value, result.ToArray()[1].Value);
                Assert.AreEqual(expectedArray[2].Quality, result.ToArray()[2].Quality);
                Assert.AreEqual(expectedArray[2].Timestamp, result.ToArray()[2].Timestamp);
                Assert.AreEqual(expectedArray[2].Value, result.ToArray()[2].Value);
            }

        }
    }
}