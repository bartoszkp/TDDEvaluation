using System.Linq;
using System.Collections.Generic;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using DataAccess;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;
            private VerifyAndAssertTestResults verifyOrAssert;

            //    [TestMethod]
            //    public void GivenNoSignals_WhenAddingASignal_ReturnsNotNull()
            //    {
            //        GivenNoSignals();

            //        var result = signalsWebService.Add(new Dto.Signal());

            //        Assert.IsNotNull(result);
            //    }

            //    [TestMethod]
            //    public void GivenNoSignals_WhenAddingASignal_ReturnsTheSameSignalExceptForId()
            //    {
            //        GivenNoSignals();

            //        var result = signalsWebService.Add(SignalWith(
            //            dataType: Dto.DataType.Decimal,
            //            granularity: Dto.Granularity.Week,
            //            path: new Dto.Path() { Components = new[] { "root", "signal" } }));

            //        Assert.AreEqual(Dto.DataType.Decimal, result.DataType);
            //        Assert.AreEqual(Dto.Granularity.Week, result.Granularity);
            //        CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            //    }

            //    [TestMethod]
            //    public void GivenNoSignals_WhenAddingASignal_PassesGivenSignalToRepositoryAdd()
            //    {
            //        GivenNoSignals();

            //        signalsWebService.Add(SignalWith(
            //            dataType: Dto.DataType.Decimal,
            //            granularity: Dto.Granularity.Week,
            //            path: new Dto.Path() { Components = new[] { "root", "signal" } }));

            //        signalsRepositoryMock.Verify(sr => sr.Add(It.Is<Domain.Signal>(passedSignal
            //            => passedSignal.DataType == DataType.Decimal
            //                && passedSignal.Granularity == Granularity.Week
            //                && passedSignal.Path.ToString() == "root/signal")));
            //    }

            //    [TestMethod]
            //    public void GivenNoSignals_WhenAddingASignal_ReturnsIdFromRepository()
            //    {
            //        var signalId = 1;
            //        GivenNoSignals();
            //        GivenRepositoryThatAssigns(id: signalId);

            //        var result = signalsWebService.Add(SignalWith(
            //            dataType: Dto.DataType.Decimal,
            //            granularity: Dto.Granularity.Week,
            //            path: new Dto.Path() { Components = new[] { "root", "signal" } }));

            //        Assert.AreEqual(signalId, result.Id);
            //    }

            //    [TestMethod]
            //    public void GivenNoSignals_WhenGettingById_DoesNotThrow()
            //    {
            //        GivenNoSignals();

            //        signalsWebService.GetById(0);
            //    }


            //    [TestMethod]
            //    public void GivenASignal_WhenGettingByItsId_ReturnsIt()
            //    {
            //        var signalId = 1;
            //        GivenASignal(SignalWith(
            //            id: signalId,
            //            dataType: Domain.DataType.String,
            //            granularity: Domain.Granularity.Year,
            //            path: Domain.Path.FromString("root/signal")));

            //        var result = signalsWebService.GetById(signalId);

            //        Assert.AreEqual(signalId, result.Id);
            //        Assert.AreEqual(Dto.DataType.String, result.DataType);
            //        Assert.AreEqual(Dto.Granularity.Year, result.Granularity);
            //        CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            //    }

            //    [TestMethod]
            //    public void GivenNoSignals_WhenGettingById_RepositoryGetIsCalledWithGivenId()
            //    {
            //        var signalId = 1;
            //        GivenNoSignals();

            //        signalsWebService.GetById(signalId);

            //        signalsRepositoryMock.Verify(sr => sr.Get(signalId));
            //    }

            //    [TestMethod]
            //    public void GivenNoSignals_WhenGettingById_ReturnsNull()
            //    {
            //        GivenNoSignals();

            //        var result = signalsWebService.GetById(0);

            //        Assert.IsNull(result);
            //    }

            //    [TestMethod]
            //    public void GivenNoSignals_WhenGettingByPath_DoesNotThrow()
            //    {
            //        signalsWebService = new SignalsWebService(null);

            //        signalsWebService.Get(null);
            //    }

            //    [TestMethod]
            //    public void GivenNoSignals_WhenGettingByPath_ReturnsNull()
            //    {
            //        signalsWebService = new SignalsWebService(null);

            //        var result = signalsWebService.Get(null);

            //        Assert.IsNull(result);
            //    }

            //    [TestMethod]
            //    public void GivenASignal_WhenGettingByPath_RepositoryGetIsCalled()
            //    {
            //        string path = "root/signal1";

            //        GivenASignalSetupGetByPath(SignalWith(
            //            id: 1,
            //            dataType: DataType.Boolean,
            //            granularity: Granularity.Day,
            //            path: Domain.Path.FromString((path))));

            //        var pathDto = new Dto.Path() { Components = new[] { "root", "signal1" } };

            //        signalsWebService.Get(pathDto);

            //        signalsRepositoryMock.Verify(srm => srm.Get(Domain.Path.FromString(path)));
            //    }

            [TestMethod]
            public void GivenASignal_WhenGettingByPath_ReturnsIt()
            {
                string path = "root/signal1";

                GivenASignalSetupGetByPath(SignalWith(
                    id: 1,
                    dataType: DataType.Boolean,
                    granularity: Granularity.Day,
                    path: Domain.Path.FromString((path))));

                var pathDto = new Dto.Path() { Components = new[] { "root", "signal1" } };

                var result = signalsWebService.Get(pathDto);

                SetupVerifyOrAssert();
                verifyOrAssert.GettingByPathAssertion(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByFalsePath_ThrowsException()
            {
                string path = "root/signal1";

                GivenASignalSetupGetByPath(SignalWith(
                    id: 1,
                    dataType: DataType.Boolean,
                    granularity: Granularity.Day,
                    path: Domain.Path.FromString((path))));

                var pathDto = new Dto.Path() { Components = new[] { "root", "signal3" } };

                SetupVerifyOrAssert();
                verifyOrAssert.GettingByFalsePathAssertion(pathDto, signalsWebService);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicy_RepositorySetAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                SetupMissingValueTest(existingSignal);

                var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
                signalsWebService.SetMissingValuePolicy(1, policy);

                SetupVerifyOrAssert();
                verifyOrAssert.VerifyRepositorySetAndGetIsCalled(signalsRepositoryMock, missingValuePolicyRepositoryMock);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicyForSpecificSignal_RepositorySetAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                SetupMissingValueTest(existingSignal);

                var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
                signalsWebService.SetMissingValuePolicy(1, policy);

                SetupVerifyOrAssert();
                verifyOrAssert.VerifyRepositorySetAndGetIsCalled(existingSignal, signalsRepositoryMock, missingValuePolicyRepositoryMock);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicyForNonExistingSignal_ThrowsException()
            {
                var existingSignal = ExistingSignal();

                SetupMissingValueTest(existingSignal);

                SetupVerifyOrAssert();
                verifyOrAssert.AssertIsExceptionThrownWhenInvalidKey(signalsWebService);   
            }

            [TestMethod]
            public void GivenASignal_WhenSettingSpecificMissingValuePolicyForSpecificSignal_RepositorySetAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();
                SetupMissingValueTest(existingSignal);

                var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
                {
                    DataType = Dto.DataType.Double,
                    Quality = Dto.Quality.Fair,
                    Value = (double)1.5
                };

                signalsWebService.SetMissingValuePolicy(1, policy);

                SetupVerifyOrAssert();
                verifyOrAssert.VerifyRepositorySetAndGetIsCalled(
                    existingSignal,
                    policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>(),
                    signalsRepositoryMock, missingValuePolicyRepositoryMock);
            }

            [TestMethod]
            public void GetMissingValuePolicy_DoesNotThrow()
            {
                signalsWebService = new SignalsWebService(null);

                signalsWebService.GetMissingValuePolicy(0);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingMissingValuePolicy_ReturnsGetIsCalled()
            {
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()));
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.GetMissingValuePolicy(0);

                missingValuePolicyRepositoryMock.Verify(mvp => mvp.Get(It.IsAny<Domain.Signal>()));
            }

            [TestMethod]
            public void GivenASignal_WhenGettingMissingValuePolicy_RepositoryGetIsCalled()
            {
                var existingSignal = ExistingSignal();

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

                signalsWebService.GetMissingValuePolicy(existingSignal.Id.Value);

                missingValuePolicyRepositoryMock.Verify(mvp => mvp.Get(It.Is<Domain.Signal>(s =>
                (
                    existingSignal.Id == existingSignal.Id
                    && existingSignal.DataType == s.DataType
                    && existingSignal.Granularity == s.Granularity
                    && existingSignal.Path == s.Path
                ))));
            }

            [TestMethod]
            public void GivenAMissingValuePolicy_WhenGettingMissingValuePolicy_ReturnsThisPolicy()
            {
                var existingSignal = ExistingSignal();

                var existingPolicy = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                {
                    Id = 1,
                    Quality = Quality.Bad,
                    Value = (double)1.5
                };

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

                var result = signalsWebService.GetMissingValuePolicy(existingSignal.Id.Value).ToDomain<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>();

                Assert.AreEqual(existingPolicy.Id, result.Id);
                Assert.AreEqual(existingPolicy.Quality, result.Quality);
                Assert.AreEqual(existingPolicy.Value, result.Value);
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

            private void GivenASignalSetupGetByPath(Domain.Signal existingSignal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(srm => srm.Get(existingSignal.Path))
                    .Returns(existingSignal);
            }
            
            private void SetupForDefaultSignal()
            {
                SetupDefault();
                
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void SetupMissingValueTest(Domain.Signal existingSignal)
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

            private void SetupVerifyOrAssert()
            {
                verifyOrAssert = new VerifyAndAssertTestResults();
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
        }
    }
}