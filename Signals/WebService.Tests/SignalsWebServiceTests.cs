﻿using System.Linq;
using System.Collections.Generic;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using DataAccess;
using DataAccess.GenericInstantiations;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;
            private VerifyOrAssertTestResults verifyOrAssert;

            //[TestMethod]
            //public void GivenNoSignals_WhenAddingASignal_ReturnsNotNull()
            //{
            //    GivenNoSignals();

            //    var result = signalsWebService.Add(new Dto.Signal());

            //    Assert.IsNotNull(result);
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenAddingASignal_ReturnsTheSameSignalExceptForId()
            //{
            //    GivenNoSignals();

            //    var result = signalsWebService.Add(SignalWith(
            //        dataType: Dto.DataType.Decimal,
            //        granularity: Dto.Granularity.Week,
            //        path: new Dto.Path() { Components = new[] { "root", "signal" } }));

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.AssertAddingASignalReturnsThisSignal(result);
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenAddingASignal_PassesGivenSignalToRepositoryAdd()
            //{
            //    GivenNoSignals();

            //    signalsWebService.Add(SignalWith(
            //        dataType: Dto.DataType.Decimal,
            //        granularity: Dto.Granularity.Week,
            //        path: new Dto.Path() { Components = new[] { "root", "signal" } }));

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.AssertRepositoryAddIsCalled(signalsRepositoryMock);
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenAddingASignal_ReturnsIdFromRepository()
            //{
            //    var signalId = 1;
            //    GivenNoSignals();
            //    GivenRepositoryThatAssigns(id: signalId);

            //    var result = signalsWebService.Add(SignalWith(
            //        dataType: Dto.DataType.Decimal,
            //        granularity: Dto.Granularity.Week,
            //        path: new Dto.Path() { Components = new[] { "root", "signal" } }));

            //    Assert.AreEqual(signalId, result.Id);
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenGettingById_DoesNotThrow()
            //{
            //    GivenNoSignals();

            //    signalsWebService.GetById(0);
            //}


            //[TestMethod]
            //public void GivenASignal_WhenGettingByItsId_ReturnsIt()
            //{
            //    var signalId = 1;
            //    GivenASignal(SignalWith(
            //        id: signalId,
            //        dataType: Domain.DataType.String,
            //        granularity: Domain.Granularity.Year,
            //        path: Domain.Path.FromString("root/signal")));

            //    var result = signalsWebService.GetById(signalId);

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.AssertGettingSignalsByItsId(signalId, result);
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenGettingById_RepositoryGetIsCalledWithGivenId()
            //{
            //    var signalId = 1;
            //    GivenNoSignals();

            //    signalsWebService.GetById(signalId);

            //    signalsRepositoryMock.Verify(sr => sr.Get(signalId));
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenGettingById_ReturnsNull()
            //{
            //    GivenNoSignals();

            //    var result = signalsWebService.GetById(0);

            //    Assert.IsNull(result);
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenGettingByPath_DoesNotThrow()
            //{
            //    signalsWebService = new SignalsWebService(null);

            //    signalsWebService.Get(null);
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenGettingByPath_ReturnsNull()
            //{
            //    signalsWebService = new SignalsWebService(null);

            //    var result = signalsWebService.Get(null);

            //    Assert.IsNull(result);
            //}

            //[TestMethod]
            //public void GivenASignal_WhenGettingByPath_RepositoryGetIsCalled()
            //{
            //    string path = "root/signal1";

            //    GivenASignalSetupGetByPath(SignalWith(
            //        id: 1,
            //        dataType: DataType.Boolean,
            //        granularity: Granularity.Day,
            //        path: Domain.Path.FromString((path))));

            //    var pathDto = new Dto.Path() { Components = new[] { "root", "signal1" } };

            //    signalsWebService.Get(pathDto);

            //    signalsRepositoryMock.Verify(srm => srm.Get(Domain.Path.FromString(path)));
            //}

            //[TestMethod]
            //public void GivenASignal_WhenGettingByPath_ReturnsIt()
            //{
            //    string path = "root/signal1";

            //    GivenASignalSetupGetByPath(SignalWith(
            //        id: 1,
            //        dataType: DataType.Boolean,
            //        granularity: Granularity.Day,
            //        path: Domain.Path.FromString((path))));

            //    var pathDto = new Dto.Path() { Components = new[] { "root", "signal1" } };

            //    var result = signalsWebService.Get(pathDto);

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.GettingByPathAssertion(result);
            //}

            //[TestMethod]
            //public void GivenASignal_WhenGettingByFalsePath_ThrowsException()
            //{
            //    string path = "root/signal1";

            //    GivenASignalSetupGetByPath(SignalWith(
            //        id: 1,
            //        dataType: DataType.Boolean,
            //        granularity: Granularity.Day,
            //        path: Domain.Path.FromString((path))));

            //    var pathDto = new Dto.Path() { Components = new[] { "root", "signal3" } };

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.GettingByFalsePathAssertion(pathDto, signalsWebService);
            //}

            //[TestMethod]
            //public void GivenASignal_WhenSettingMissingValuePolicy_RepositorySetAndGetIsCalled()
            //{
            //    var existingSignal = ExistingSignal();

            //    SetupSettingMissingValueTest(existingSignal);

            //    var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
            //    signalsWebService.SetMissingValuePolicy(1, policy);

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.VerifyRepositorySetAndGetIsCalled(signalsRepositoryMock, missingValuePolicyRepositoryMock);
            //}

            //[TestMethod]
            //public void GivenASignal_WhenSettingMissingValuePolicyForSpecificSignal_RepositorySetAndGetIsCalled()
            //{
            //    var existingSignal = ExistingSignal();

            //    SetupSettingMissingValueTest(existingSignal);

            //    var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
            //    signalsWebService.SetMissingValuePolicy(1, policy);

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.VerifyRepositorySetAndGetIsCalled(existingSignal, signalsRepositoryMock, missingValuePolicyRepositoryMock);
            //}

            //[TestMethod]
            //public void GivenASignal_WhenSettingMissingValuePolicyForNonExistingSignal_ThrowsException()
            //{
            //    var existingSignal = ExistingSignal();

            //    SetupSettingMissingValueTest(existingSignal);

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.AssertSetMissingValuePolicyIsExceptionThrownWhenInvalidKey(signalsWebService);
            //}

            //[TestMethod]
            //public void GivenASignal_WhenSettingSpecificMissingValuePolicyForSpecificSignal_RepositorySetAndGetIsCalled()
            //{
            //    var existingSignal = ExistingSignal();

            //    SetupSettingMissingValueTest(existingSignal);

            //    var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
            //    {
            //        DataType = Dto.DataType.Double,
            //        Quality = Dto.Quality.Fair,
            //        Value = (double)1.5
            //    };

            //    signalsWebService.SetMissingValuePolicy(1, policy);

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.VerifyRepositorySetAndGetIsCalled(
            //        existingSignal,
            //        policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>(),
            //        signalsRepositoryMock, missingValuePolicyRepositoryMock);
            //}

            //[TestMethod]
            //public void GivenASignal_WhenGettingMissingValuePolicy_RepositoryGetIsCalled()
            //{
            //    var existingSignal = ExistingSignal();

            //    SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(existingSignal);

            //    signalsWebService.GetMissingValuePolicy(existingSignal.Id.Value);

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.VerifyRepositoryGetIsCalled(missingValuePolicyRepositoryMock, existingSignal);
            //}

            //[TestMethod]
            //public void GivenAMissingValuePolicy_WhenGettingMissingValuePolicy_ReturnsThisPolicy()
            //{
            //    var existingSignal = ExistingSignal();

            //    var existingPolicy = ExistingPolicy();

            //    SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(existingSignal, existingPolicy);

            //    var result = signalsWebService.GetMissingValuePolicy(existingSignal.Id.Value).ToDomain<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>();


            //    SetupVerifyOrAssert();
            //    verifyOrAssert.AssertGetIsReturningSpecificPolicy(existingPolicy, result);
            //}

            //[TestMethod]
            //public void GivenASignal_WhenGettingMissingValuePolicyForNonExistingSignal_ThrowsException()
            //{
            //    var existingSignal = ExistingSignal();

            //    var existingPolicy = ExistingPolicy();

            //    int wrongId = 3;

            //    SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(existingSignal, existingPolicy, existingSignal.Id.Value);

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.AssertGetMissingValuePolicyIsExceptionThrownWhenInvalidKey(signalsWebService, wrongId);
            //}

            //[TestMethod]
            //public void GivenASignalAndDatum_WhenSettingData_RepositorySetDataAndGetIsCalled()
            //{
            //    var existingSignal = ExistingSignal();

            //    var existingDatum = ExistingDatum();

            //    SetupSettingData(existingSignal);

            //    signalsWebService.SetData(1, existingDatum);

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.VerifyRepositoryGetDataAndGetIsCalled(existingSignal, existingDatum, signalsDataRepositoryMock, signalsRepositoryMock);
            //}

            //[TestMethod]
            //public void GivenASignalAndDatum_WhenSettingDataWithWrongSignalId_ExceptionIsThrown()
            //{
            //    var existingSignal = ExistingSignal();
            //    int wrongSignalId = 3;

            //    SetupSettingData(existingSignal);

            //    SetupVerifyOrAssert();
            //    verifyOrAssert.AssertSetDataIsExceptionThrownWhenInvalidKey(signalsWebService, wrongSignalId);
            //}

            [TestMethod]
            public void GetData_DoesNotThrow()
            {
                signalsWebService = new SignalsWebService(null);

                signalsWebService.GetData(0, new DateTime(), new DateTime());
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingData_RepositoryGetDataIsCalled()
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));

                var signalsDomainService = new SignalsDomainService(null, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.GetData(0, new DateTime(), new DateTime());

                signalsDataRepositoryMock.Verify(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));
            }

            //[TestMethod]
            //public void GivenASignal_WhenGettingDataFromSpecificSignal_RepositoryGetDataAndGetIsCalled()
            //{
            //    var existingSignal = ExistingSignal();

            //    GivenASignal(existingSignal);

            //    signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

            //    signalsDataRepositoryMock
            //        .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));

            //    var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

            //    signalsWebService = new SignalsWebService(signalsDomainService);

            //    signalsWebService.GetData(existingSignal.Id.Value, new DateTime(), new DateTime());

            //    signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));
            //    signalsDataRepositoryMock.Verify(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));
            //}

            [TestMethod]
            public void GivenASignal_WhenGettingSpecificDataFromSpecificSignal_RepositoryGetDataAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                var existingDatum = ExistingDatum();

                GivenASignal(existingSignal);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(
                        existingSignal,
                        existingDatum.First().Timestamp,
                        existingDatum.Last().Timestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>());

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.GetData(existingSignal.Id.Value, existingDatum.First().Timestamp, existingDatum.Last().Timestamp);

                signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));

                signalsDataRepositoryMock.Verify(sdrm => sdrm.GetData<double>(
                    existingSignal,
                    existingDatum.First().Timestamp,
                    existingDatum.Last().Timestamp));
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

            private void GivenASignal(Domain.Signal existingSignal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);
            }
            
            private void GivenASignalSetupGetByPath(Domain.Signal existingSignal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(srm => srm.Get(existingSignal.Path))
                    .Returns(existingSignal);
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

            private void SetupSettingMissingValueTest(Domain.Signal existingSignal)
            {
                SetupDefault();

                GivenASignal(SignalWith(
                        existingSignal.Id.Value,
                        existingSignal.DataType,
                        existingSignal.Granularity,
                        existingSignal.Path));
                
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void SetupDefault()
            {
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

            }

            private Domain.Signal ExistingSignal()
            {
                return new Domain.Signal()
                {
                    Id = 1,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("root/signal1")
                };
            }

            private DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble ExistingPolicy()
            {
                return new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                {
                    Id = 1,
                    Quality = Quality.Bad,
                    Value = (double)1.5
                };
            }
            
            private Dto.Datum[] ExistingDatum()
            {
                return new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 }
                };
            }

            private void SetupVerifyOrAssert()
            {
                verifyOrAssert = new VerifyOrAssertTestResults();
            }

            private void SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(Signal existingSignal)
            {
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble());

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(Signal existingSignal,
                SpecificValueMissingValuePolicyDouble existingPolicy)
            {
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                    .Returns(existingPolicy);

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(Signal existingSignal,
                SpecificValueMissingValuePolicyDouble existingPolicy, int signalId)
            {
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                    .Returns(existingPolicy);

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(signalId))
                    .Returns(existingSignal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void SetupSettingData(Signal existingSignal)
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<double>(It.IsAny<IEnumerable<Datum<double>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
        }
    }
}