using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;
            private SignalsDomainService signalDomainService;

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
            private Mock<Domain.MissingValuePolicy.MissingValuePolicyBase> policyMock;

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
            public void GivenNoSignals_WhenGettingByIdWithNullValue_ReturnsNull()
            {

                GivenNoSignals();

                signalsWebService = new SignalsWebService(signalDomainService);
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
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
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


            [TestMethod]
            public void GivenASignal_WhenSettingASignalsData_RepositorySetDataIsCalled()
            {
                MockSetup();

                Datum<double>[] dataToSet = new Datum<double>[] {
                        new Datum<double>() { Id = 1, Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Datum<double>() { Id = 2, Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = (double)2 },
                        new Datum<double>() { Id = 3, Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)3 },
                        };

                signalDomainService.SetData(1, dataToSet);
                signalsDataRepositoryMock.Verify(sr => sr.SetData(dataToSet));
            }

            private void MockSetup()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SettingNotExistingSignalDataException))]
            public void GivenASignal_WhenSettingDataForNotExistingSignal_ThrowsNotExistingSignalException()
            {
                MockSetup();
                signalsWebService = new SignalsWebService(signalDomainService);

                Datum<double>[] dataToSet = new Datum<double>[] {
                        new Datum<double>() { Id = 1, Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Datum<double>() { Id = 2, Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = (double)2 },
                        new Datum<double>() { Id = 3, Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)3 },
                        };

                Dto.Datum[] DtoDataToSet = new Dto.Datum[] {
                        new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = (double)2 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)3 },
                        };

                signalsWebService.SetData(100, DtoDataToSet);
                signalDomainService.SetData(1, dataToSet);
                signalsDataRepositoryMock.Verify(sr => sr.SetData(dataToSet));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.GettingDataOfNotExistingSignal))]
            public void GivenASignal_WhenGettingDataForNotExistingSignal_ThrowsGettingDataOfNotExistingSignal()
            {
                MockSetup();
                signalsWebService = new SignalsWebService(signalDomainService);

                var fromIncludedDate = new DateTime(2016, 8, 1);
                var toExcludedDate = new DateTime(2016, 8, 4);

                var result = signalsWebService.GetData(1, fromIncludedDate, toExcludedDate);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingData_ReturnsCorrectData()
            {
                MockSetup();

                List<Domain.Datum<int>> data = new List<Domain.Datum<int>>();
                SetupData(data);

                Signal signal = new Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("example/path"),
                };

                var fromIncludedDate = new DateTime(2016, 8, 1);
                var toExcludedDate = new DateTime(2016, 8, 4);

                signalsDataRepositoryMock
                    .Setup(srm => srm.GetData<int>(signal, fromIncludedDate, toExcludedDate))
                    .Returns(data);

                signalsRepositoryMock
                    .Setup(srm => srm.Add(signal))
                    .Returns(signal);
                signalDomainService.Add(signal);

                signalsRepositoryMock
                    .Setup(srm => srm.Get(signal.Id.Value))
                    .Returns(signal);

                signalDomainService.SetData(1, data.AsEnumerable());
                data.RemoveAt(2);

                var result = signalDomainService.GetData<int>(signal.Id.Value, fromIncludedDate, toExcludedDate);

                CollectionAssert.AreEqual(data, result.ToList<Datum<int>>());
            }

            private void SetupData(List<Domain.Datum<int>> data)
            {
                data.Add(new Domain.Datum<int>()
                {
                    Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2000, 2, 2),
                    Value = 12,
                });

                data.Add(new Domain.Datum<int>()
                {
                    Quality = Domain.Quality.Good,
                    Timestamp = new DateTime(2000, 2, 3),
                    Value = 10,
                });

                data.Add(new Domain.Datum<int>()
                {
                    Quality = Domain.Quality.Poor,
                    Timestamp = new DateTime(2000, 2, 4),
                    Value = 14,
                });
            }

            [TestMethod]
            public void GivenNoSignal_WhenGettingByPath_ReturnsNull()
            {
                MockSetup();

                signalsWebService = new SignalsWebService(signalDomainService);
                var result = signalsWebService.Get(new Dto.Path()
                {
                    Components = new[] { "not/existing/path" },
                });

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByPath_ReturnsCorrectSignal()
            {
                Dto.Path dtoPath = new Dto.Path() { Components = new[] { "example", "path" } };

                GetByPathSetup();
                var returndSignal = signalsWebService.Get(dtoPath);

                Assert.AreEqual(1, returndSignal.Id.Value);
                Assert.AreEqual(Dto.DataType.Boolean, returndSignal.DataType);
                Assert.AreEqual(Dto.Granularity.Day, returndSignal.Granularity);
                CollectionAssert.AreEqual(dtoPath.Components.ToArray(), returndSignal.Path.Components.ToArray());
            }

            private void GetByPathSetup()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(Domain.Path.FromString("example/path")))
                    .Returns(new Signal()
                    {
                        Id = 1,
                        DataType = Domain.DataType.Boolean,
                        Granularity = Domain.Granularity.Day,
                        Path = Domain.Path.FromString("example/path"),
                    });

                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                signalsWebService = new SignalsWebService(signalDomainService);
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SettingPolicyNotExistingSignalException))]
            public void WhenCallingSetPolicy_ProperMethodIsCalled()
            { 
                Mock<Dto.MissingValuePolicy.MissingValuePolicy> policyMock = new Mock<Dto.MissingValuePolicy.MissingValuePolicy>();

                Mock<ISignalsWebService> signalWebServiceMock = new Mock<ISignalsWebService>();
                signalWebServiceMock
                    .Setup(swsm => swsm.SetMissingValuePolicy(1, policyMock.Object));

                MockSetup();
                signalsWebService = new SignalsWebService(signalDomainService);
                signalsWebService.SetMissingValuePolicy(1, policyMock.Object);

                signalWebServiceMock.Verify(swsm => swsm.SetMissingValuePolicy(1, policyMock.Object));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SettingPolicyNotExistingSignalException))]
            public void GivenNoSignal_WhenSetPolicyIsCalled_ItThrowsException()
            {
                Mock<Dto.MissingValuePolicy.MissingValuePolicy> policyMock = new Mock<Dto.MissingValuePolicy.MissingValuePolicy>();
                SetupMissingValuePolicy(policyMock);

                Mock<ISignalsWebService> signalWebServiceMock = new Mock<ISignalsWebService>();
                signalWebServiceMock
                    .Setup(swsm => swsm.SetMissingValuePolicy(1, policyMock.Object));

                MockSetup();
                signalsWebService = new SignalsWebService(signalDomainService);
                signalsWebService.SetMissingValuePolicy(1, policyMock.Object);

                signalWebServiceMock.Verify(swsm => swsm.SetMissingValuePolicy(1, policyMock.Object));
            }

            private void SetupMissingValuePolicy(Mock<Dto.MissingValuePolicy.MissingValuePolicy> policyMock)
            {
                policyMock.Object.DataType = Dto.DataType.Boolean;
                policyMock.Object.Id = 1;
                policyMock.Object.Signal = new Dto.Signal()
                {
                    Id = 1,
                    DataType = Dto.DataType.Boolean,
                    Granularity = Dto.Granularity.Day,
                    Path = new Dto.Path() { Components = new[] { "aaa", "bbb" } },
                };
            }

            [TestMethod]
            public void GivenASignal_WhenSettingAPolicy_CorrectRepositoryMethodIsCalled()
            {
                policyMock = new Mock<Domain.MissingValuePolicy.MissingValuePolicyBase>();
                var exampleSignal = SetupPolicyMock();

                signalDomainService.SetMissingValuePolicy(exampleSignal, policyMock.Object);

                missingValuePolicyRepositoryMock.Verify(sdsm => sdsm.Set(exampleSignal, policyMock.Object));
            }

            private Signal SetupPolicyMock()
            {
                var exampleSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("example/path"),
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(It.IsAny<int>()))
                    .Returns(new Signal());

                Mock<Dto.MissingValuePolicy.MissingValuePolicy> dtoPolicyMock = new Mock<Dto.MissingValuePolicy.MissingValuePolicy>();
                SetupMissingValuePolicy(dtoPolicyMock);

                policyMock = new Mock<Domain.MissingValuePolicy.MissingValuePolicyBase>();

                Mock<Domain.Services.ISignalsDomainService> signalDomainServiceMock = new Mock<Domain.Services.ISignalsDomainService>();
                signalDomainServiceMock
                    .Setup(sdsm => sdsm.SetMissingValuePolicy(exampleSignal, policyMock.Object));

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                   .Setup(mvprm => mvprm.Set(exampleSignal, policyMock.Object));

                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                return exampleSignal;
            }

            [TestMethod]
            public void GivenNoSignal_WhenGettingMissingValuePolicy_ReturnsNull()
            {
                var exampleSignal = SetupGetPolicyMock();

                var result = signalDomainService.GetMissingValuePolicy(exampleSignal.Id.Value);

                Assert.IsNull(result);
            }

            private Signal SetupGetPolicyMock()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                var exampleSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("example/path"),
                };

                policyMock = new Mock<Domain.MissingValuePolicy.MissingValuePolicyBase>();

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(sdsm => sdsm.Get(exampleSignal))
                    .Returns(policyMock.Object);

                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                return exampleSignal;
            }

            [TestMethod]
            public void GivenASignal_WhenGettingPolicy_ReturnsCorrectData()
            {
                var exampleSignal = SetupGetPolicyMock();

                policyMock = new Mock<Domain.MissingValuePolicy.MissingValuePolicyBase>();
                policyMock.Object.Id = 1;
                policyMock.Object.Signal = exampleSignal;

                Mock<Domain.Services.ISignalsDomainService> signalDomainServiceMock = new Mock<Domain.Services.ISignalsDomainService>();
                signalDomainServiceMock
                    .Setup(sdsm => sdsm.GetMissingValuePolicy(It.IsAny<int>()))
                    .Returns(policyMock.Object);

                signalsRepositoryMock
                    .Setup(srm => srm.Get(It.IsAny<int>()))
                    .Returns(exampleSignal);

                var result = signalDomainService.GetMissingValuePolicy(exampleSignal.Id.Value);

                Assert.AreEqual(policyMock.Object.Id, result.Id);
                Assert.AreEqual(policyMock.Object.Signal, result.Signal);
            }
        }
    }
}