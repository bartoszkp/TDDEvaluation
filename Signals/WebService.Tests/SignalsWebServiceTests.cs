using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Domain.Exceptions;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsNotNull()
            {
                GivenNoSignals();

                var result = signalsWebService.Add(new Dto.Signal());

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsTheSameSignalExceptForId()
            {
                GivenNoSignals();

                var result = signalsWebService.Add(SignalWith(
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

                signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Decimal,
                    granularity: Dto.Granularity.Week,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }));

                signalsRepositoryMock.Verify(sr => sr.Add(It.Is<Domain.Signal>(passedSignal
                    => passedSignal.DataType == DataType.Decimal
                        && passedSignal.Granularity == Granularity.Week
                        && passedSignal.Path.ToString() == "root/signal")));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsIdFromRepository()
            {
                var signalId = 1;
                GivenNoSignals();
                GivenRepositoryThatAssigns(id: signalId);

                var result = signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Decimal,
                    granularity: Dto.Granularity.Week,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }));

                Assert.AreEqual(signalId, result.Id);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_DoesNotThrow()
            {
                GivenNoSignals();

                signalsWebService.GetById(0);
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

                var result = signalsWebService.GetById(signalId);

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

                signalsWebService.GetById(signalId);

                signalsRepositoryMock.Verify(sr => sr.Get(signalId));
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.GetById(0);

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
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Domain.Signal existingSignal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);
            }

            private void GivenRepositoryThatAssigns(int id)
            {
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s =>
                    {
                        s.Id = id;
                        return s;
                    });
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;

            private Dto.Path GivenASignalByPath()
            {
                GivenNoSignals();

                Domain.Path path = Domain.Path.FromString("root/signal");
                signalsRepositoryMock.Setup(x => x.Get(path)).Returns(new Domain.Signal()
                {
                    Path = path
                });

                return path.ToDto<Dto.Path>();
            }

            [TestMethod]
            public void GivenASignalByPath_WhenGettingByPath_ReturnsNotNull()
            {
                var path = GivenASignalByPath();

                var result = signalsWebService.Get(path);
                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenASignalByPath_WhenGettingByPath_ReturnsSignalWithPath()
            {
                var path = GivenASignalByPath();

                var result = signalsWebService.Get(path);
                CollectionAssert.AreEqual(path.Components.ToArray(),result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenASignalByPath_WhenGettingByPath_RepositoryGetIsCalledWithGivenPath()
            {
                var path = GivenASignalByPath();

                var result = signalsWebService.Get(path);
                signalsRepositoryMock.Verify(x => x.Get(It.IsAny<Domain.Path>()));
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignals_WhenGettingByPath_ThrowsCouldntGetASignalException()
            {
                GivenNoSignals();

                Dto.Path notExistingPath = new Dto.Path() { Components = new[] { "root" } };
                var result = signalsWebService.Get(notExistingPath);
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignals_WhenSettingData_ThrowsCouldntGetASignalException()
            {
                GivenNoSignals();

                Dto.Datum[] data = GetDtoDatumDouble();

                int notExistingSignalID = 8;
                signalsWebService.SetData(notExistingSignalID, data);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingData_VerifingRepositoryFunctionsGetAndSetData()
            {
                int signalId = 3;
                GivenASignal(SignalWith(
                   id: signalId,
                   dataType: Domain.DataType.Double,
                   granularity: Domain.Granularity.Month,
                   path: Domain.Path.FromString("root/signal")));

                Dto.Datum[] data = GetDtoDatumDouble();

                signalsDataRepositoryMock.Setup(x => x.SetData(It.IsAny<IEnumerable<Domain.Datum<double>>>()));

                signalsWebService.SetData(signalId, data);

                signalsRepositoryMock.Verify(x => x.Get(It.Is<int>(y => y.Equals(signalId))));
                signalsDataRepositoryMock.Verify(x => x.SetData(It.IsAny<IEnumerable<Domain.Datum<double>>>()));
            }

            [TestMethod]
            public void GivenASignal_WhenSettingData_ChecksIfSignalHasBeenSet()
            {
                int signalId = 7;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal")));

                Dto.Datum[] data = GetDtoDatumDouble();

                signalsDataRepositoryMock
                    .Setup(x => x.SetData(It.IsAny<IEnumerable<Domain.Datum<double>>>()))
                    .Callback<IEnumerable<Domain.Datum<double>>>(x => {
                        foreach(var match in x)
                        {
                            if (match.Signal == null) throw new SignalForDatumHasNotBeenSet();
                        }
                    });

                signalsWebService.SetData(signalId, data);
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignals_WhenGettingData_ThrowsCouldntGetASignalException()
            {
                GivenNoSignals();

                DateTime from = new DateTime(2000, 1, 1),to = new DateTime(2000, 3, 1);
                int notExistingSignalID = 8;
                signalsWebService.GetData(notExistingSignalID, from, to);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingData_ReturnsDatum()
            {
                int signalId = 7;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Integer,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal")));

                DateTime from = new DateTime(2000, 1, 1), to = new DateTime(2000, 3, 1);

                if (!(signalsWebService.GetData(signalId, from, to) is IEnumerable<Dto.Datum>))
                    Assert.Fail();
            }

            private Domain.Datum<double>[] GetDomainDatumDouble()
            {
                return new Domain.Datum<double>[]
                    {
                        new Domain.Datum<double>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (int)1 },
                        new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int)5 },
                        new Domain.Datum<double>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (int)2 }
                    };
            }

            private Dto.Datum[] GetDtoDatumDouble()
            {
                return new Dto.Datum[]
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 }
                    };
            }

            [TestMethod]
            public void GivenASignal_WhenGettingData_VerifyDataRepositoryFunctions()
            {
                int signalId = 7;
                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);

                DateTime from = new DateTime(2000, 1, 1), to = new DateTime(2000, 3, 1);

                signalsDataRepositoryMock.Setup(asd => asd.GetData<double>(
                    It.IsAny<Signal>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>())).Returns((Signal p,DateTime d,DateTime e) => {
                        return GetDomainDatumDouble();
                    });

                signalsWebService.GetData(signalId, from, to);

                signalsDataRepositoryMock.Verify(x => x.GetData<double>(
                    It.IsAny<Signal>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()));
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignal_WhenSettingMissingValuePolicy_ThrowsCouldntGetASignalException()
            {
                GivenNoSignals();
                int nonExistingSignalId = 3;
                this.signalsWebService.SetMissingValuePolicy(nonExistingSignalId,null);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicy_DontThrows()
            {
                GivenNoSignals();
                int signalId = 2;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Integer,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal")));

                this.signalsWebService.SetMissingValuePolicy(signalId, null);
            }
        }
    }
}