using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Exceptions;
using Domain.MissingValuePolicy;

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
            [ExpectedException(typeof(IdNotNullException))]
            public void WhenSettingAnIdForSignal_ThrowsIdNotNullException()
            {
                var domainService = new SignalsDomainService(null, null, null);
                signalsWebService = new SignalsWebService(domainService);

                signalsWebService.Add(new Dto.Signal()
                {
                    Id = 2,
                    DataType = Dto.DataType.Boolean,
                    Granularity = Dto.Granularity.Day,
                    Path = new Dto.Path() { Components = new[] { "example", "path" } }
                });
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

            [TestMethod]
            [ExpectedException(typeof(System.ArgumentNullException))]
            public void GettingSignalByPath_GivenNullPath_ThrowsArgumentNullException()
            {
                GivenNoSignals();
                var result = signalsWebService.Get(null);
            }

            [TestMethod]
            public void GivenSignals_GettingSignalByPath_SignalIsReturned()
            {
                var exsistingSignal = SignalWith(1,
                                                dataType: DataType.Integer,
                                                granularity: Granularity.Hour,
                                                path: Path.FromString("x/y"));

                GivenASignal(exsistingSignal);
                var dtoPath = new Dto.Path() { Components = new[] { "x", "y" } };
                var result = signalsWebService.Get(dtoPath);

                Assert.AreEqual(Dto.DataType.Integer, result.DataType);
                Assert.AreEqual(Dto.Granularity.Hour, result.Granularity);
                Assert.AreEqual(exsistingSignal.Id, result.Id);
                CollectionAssert.AreEqual(dtoPath.Components.ToArray(), result.Path.Components.ToArray());


            }

            [TestMethod]
            [ExpectedException(typeof(NoSuchSignalException))]
            public void GivenNoSuchSignal_GettingMissingValuePolicy_ThrowsException()
            {
                SetupWebServiceForMvpOperations();

                var result = signalsWebService.GetMissingValuePolicy(1);
            }

            [TestMethod]
            public void SignalHasNoSpecfiedPolicy_GettingMissingValuePolicy_ReturnsNull()
            {
                SetupWebServiceForMvpOperations();
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns(new Signal());
                missingValueRepoMock.Setup(mv => mv.Get(It.IsAny<Signal>())).Returns((MissingValuePolicyBase)null);

                var result = signalsWebService.GetMissingValuePolicy(1);
                Assert.IsNull(result);

            }

            [TestMethod]
            public void GivenSignal_GettingMissingValuePolicy_SpecificPolicyIsReturned()
            {
                SetupWebServiceForMvpOperations();
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns(new Signal());

                missingValueRepoMock.Setup(mv => mv.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyBoolean());

                var result = signalsWebService.GetMissingValuePolicy(1);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Dto.MissingValuePolicy.SpecificValueMissingValuePolicy));


            }


            [TestMethod]
            [ExpectedException(typeof(NoSuchSignalException))]
            public void NoSuchSignal_SetMissingValuePolicy_ThrowsException()
            {
                SetupWebServiceForMvpOperations();
                Dto.MissingValuePolicy.MissingValuePolicy policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns((Signal)null);

                signalsWebService.SetMissingValuePolicy(1, policy);

            }


            [TestMethod]
            [ExpectedException(typeof(System.ArgumentNullException))]
            public void GivenNullPolicy_SetMissingValuePolicy_ThrowsArgumentNullException()
            {
                SetupWebServiceForMvpOperations();
                signalsWebService.SetMissingValuePolicy(1, null);
            }

            [TestMethod]
            public void CreatedMvpForSignal_WhenGetting_IsReturned()
            {
                SetupWebServiceForMvpOperations();
                Dto.MissingValuePolicy.MissingValuePolicy policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<int>(id => id == 1))).Returns(new Signal() { Id = 1 });

                missingValueRepoMock.Setup(mv => mv.Get(It.Is<Signal>(s => s.Id == 1)))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyString());

                signalsWebService.SetMissingValuePolicy(1, policy);

                var result = signalsWebService.GetMissingValuePolicy(1);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Dto.MissingValuePolicy.SpecificValueMissingValuePolicy));
            }

            [TestMethod]
            public void WhenGettingNoneQualityMissingValuePolicy_ReturnsIt()
            {
                SetupWebServiceForMvpOperations();
                var signal = new Domain.Signal()
                {
                    DataType = Domain.DataType.Double,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("sfda/mko")
                };
                var signalDto = signal.ToDto<Dto.Signal>();

                signalsRepositoryMock
                    .Setup(s => s.Get(It.IsAny<int>()))
                    .Returns(signal);
                missingValueRepoMock
                    .Setup(mvpr => mvpr.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble()
                    {
                        Id = 1,
                        Signal = signal,
                    });

                var result = signalsWebService.GetMissingValuePolicy(1);
                Assert.AreEqual(1, result.Id.Value);
                Assert.AreEqual(signalDto.DataType, result.Signal.DataType);
                Assert.AreEqual(signalDto.Granularity, result.Signal.Granularity);
                Assert.AreEqual(signalDto.Path.ToString(), result.Signal.Path.ToString());
            }

            [TestMethod]
            public void WhenAddingNewSignal_SettingMissingValuePolicyTo_NoneQualityMissingValuePolicy()
            {
                var signal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = DataType.String
                };
                SetupWebServiceForMvpOperations();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns(signal);
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<int>()))
                    .Returns<Domain.Signal>(null);
                missingValueRepoMock
                    .Setup(mvpr => mvpr.Set(It.IsAny<Domain.Signal>(), It.IsAny<NoneQualityMissingValuePolicy<string>>()));

                signalsWebService.Add(new Dto.Signal()
                {
                    DataType = Dto.DataType.String
                });
                missingValueRepoMock.Verify(mvpr => mvpr.Set(It.IsAny<Domain.Signal>(), It.IsAny<NoneQualityMissingValuePolicy<string>>()));
            }

            [TestMethod]
            public void WhenGettingById_ReturnsNull()
            {
                GivenASignal(null, 1);
                var result = signalsWebService.GetById(1);
                Assert.IsNull(result);
            }

            [TestMethod]
            public void WhenGettingByPath_ReturnsNull()
            {
                GivenASignal(null, 1);
                var result = signalsWebService.Get(new Dto.Path() {
                    Components = new[] { "sfda" } });
                Assert.IsNull(result);
            }

            private void SetupWebServiceForMvpOperations()
            {
                missingValueRepoMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValueRepoMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
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
                missingValueRepoMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => AddId(s));
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValueRepoMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Domain.Signal existingSignal, int id = 0)
            {
                GivenNoSignals();
                if (id <= 0)
                    id = existingSignal.Id.Value;

                signalsRepositoryMock
                    .Setup(sr => sr.Get(id))
                    .Returns(existingSignal);

                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<Path>()))
                    .Returns(existingSignal);
            }

            private void GivenRepositoryThatAssigns(int id)
            {
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => AddId(s));
            }

            private Domain.Signal AddId(Domain.Signal signal, int id = 1)
            {
                signal.Id = id;
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<int>()))
                    .Returns(signal);
                return signal;
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValueRepoMock;
        }
    }
}