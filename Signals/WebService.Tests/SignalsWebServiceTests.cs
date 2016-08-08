using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Dto.MissingValuePolicy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Domain.Exceptions;
using DataAccess.GenericInstantiations;

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
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
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
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;

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
            public void GivenNoSignals_WhenGettingByPath_ReturnsNull()
            {
                GivenNoSignals();
                Dto.Path notExistingPath = new Dto.Path() { Components = new[] { "root" } };

                var result = signalsWebService.Get(notExistingPath);

                Assert.IsNull(result);
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


            [TestMethod]
            public void GivenASignal_WhenGettingData_ReturnsDataOrderedByDate()
            {
                int signalId = 7;
                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                var data = GetDomainDatumDouble();
                GivenData(signalId, data);

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                Assert.AreEqual(2, result.Count());
                Assert.AreEqual(new DateTime(2000, 1, 1), result.First().Timestamp);
                Assert.AreEqual(new DateTime(2000, 2, 1), result.Skip(1).First().Timestamp);
            }

            private Domain.Datum<double>[] GetDomainDatumDouble()
            {
                return new Domain.Datum<double>[]
                    {
                        new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int)5 },
                        new Domain.Datum<double>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (int)1 },
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

            private void GivenData<T>(int signalId, IEnumerable<Domain.Datum<T>> data)
            {
                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetData<T>(It.Is<Domain.Signal>(s => s.Id == signalId),
                    It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns<Domain.Signal, DateTime, DateTime>(
                    (s, from, to) => data.Where(d => (d.Timestamp >= from && d.Timestamp < to)));
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

                GivenData(signalId, GetDomainDatumDouble());

                signalsWebService.GetData(signalId, from, to);

                signalsDataRepositoryMock.Verify(x => x.GetData<double>(
                    It.IsAny<Signal>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()));
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignals_WhenSettingMissingValuePolicy_ThrowsCouldntGetASignalException()
            {
                GivenNoSignals();
                int nonExistingSignalId = 3;
                this.signalsWebService.SetMissingValuePolicy(nonExistingSignalId,null);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicy_DontThrows()
            {
                int signalId = 2;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Integer,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal")));

                this.signalsWebService.SetMissingValuePolicy(signalId, 
                    new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy());
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicy_VaryfingCallRepositorySetFunctions()
            {
                int signalId = 2;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal")));

                var missingValuePolicy = new SpecificValueMissingValuePolicy() {
                    DataType = Dto.DataType.Double, 
                    Quality = Dto.Quality.Fair, 
                    Value = (double)1.5 };


                missingValuePolicyRepositoryMock.Setup(x => x.Set(It.IsAny<Domain.Signal>(),
                    It.IsAny<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<Double>>()));

                this.signalsWebService.SetMissingValuePolicy(signalId, missingValuePolicy);

                missingValuePolicyRepositoryMock.Verify(x => x.Set(It.IsAny<Domain.Signal>(),
                    It.IsAny<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<Double>>()));
            }


            private void GivenASignalForGettingMissingValuePolicy(int signalId)
            {
                Signal signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);

                this.missingValuePolicyRepositoryMock
                    .Setup(x => x.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new SpecificValueMissingValuePolicyDouble());
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignals_WhenGettingMissingValuePolicy_ThrowsCouldntGetASignalException()
            {
                GivenNoSignals();
                int nonExistingSignalId = 3;
                this.signalsWebService.GetMissingValuePolicy(nonExistingSignalId);
            }

            [TestMethod]
            public void GivenASignalForGettingMissingValuePolicy_WhenGettingMissingValuePolicy_ReturnsDefaultMissingValuePolicy()
            {
                int signalId = 2;
                GivenASignalForGettingMissingValuePolicy(signalId);

                if (!(this.signalsWebService.GetMissingValuePolicy(signalId) is Dto.MissingValuePolicy.MissingValuePolicy))
                    Assert.Fail();
            }


            [TestMethod]
            public void GivenASignalForGettingMissingValuePolicy_WhenGettingMissingValuePolicy_VaryfingCallOfRepositoryFunctioGet()
            {
                int signalId = 2;
                GivenASignalForGettingMissingValuePolicy(signalId);

                this.signalsWebService.GetMissingValuePolicy(signalId);

                this.missingValuePolicyRepositoryMock.Verify(x => x.Get(It.IsAny<Domain.Signal>()));
            }


            [TestMethod]
            public void GivenNoSignals_WhenAddingSignal_MissingValuePolicyRepositoryAddWithCorrectArgumentsIsCalled()
            {
                var signal = SignalWith(
                    dataType: Dto.DataType.Integer,
                    granularity: Dto.Granularity.Month,
                    path: new Dto.Path() { Components = new string[] { "root", "signal1" } });
                GivenNoSignals();

                var result = signalsWebService.Add(signal);

                missingValuePolicyRepositoryMock
                    .Verify(mvpr => mvpr.Set(It.Is<Domain.Signal>(s => s.Id == result.Id),
                    It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>()));
            }


            [TestMethod]
            public void GivenNoneQualityMissingValuePolicy_WhenGettingMissingValue_ReturnsNotNull()
            {
                int signalId = 2;
                Signal signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                var mvp = new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>();

                var result = mvp.GetMissingValue(signal, new DateTime(2000, 10, 2));

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenNoneQualityMissingValuePolicy_WhenGettingMissingValue_ReturnsDatumWithNoneQualityAndDefaultValue()
            {
                int signalId = 2;
                Signal signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                var mvp = new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>();

                var result = mvp.GetMissingValue(signal, new DateTime(2000, 10, 2));

                Assert.AreEqual(Quality.None, result.Quality);
                Assert.AreEqual(default(double), result.Value);
            }
        }
    }
}