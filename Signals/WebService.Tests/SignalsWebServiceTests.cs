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
            [ExpectedException(typeof(NoSuchSignalException))]
            public void GettingSignalByPath_SignalDoesNotExist_ThrowsException()
            {
                GivenNoSignals();
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<Path>())).Returns((Signal)null);
                var path = new Dto.Path() { Components = new[] { "x", "y" } };

                var result = signalsWebService.Get(path);
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
                SetupWebServiceForMissingValuePolicy();

                var result = signalsWebService.GetMissingValuePolicy(1);
            }

            [TestMethod]
            public void SignalHasNoSpecfiedPolicy_GettingMissingValuePolicy_ReturnsNull()
            {
                SetupWebServiceForMissingValuePolicy();
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns(new Signal());
                missingValueRepoMock.Setup(mv => mv.Get(It.IsAny<Signal>())).Returns((MissingValuePolicyBase)null);

                var result = signalsWebService.GetMissingValuePolicy(1);
                Assert.IsNull(result);

            }

            
            
            private void SetupWebServiceForMissingValuePolicy()
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

                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<Path>()))
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
            private Mock<IMissingValuePolicyRepository> missingValueRepoMock;
        }
    }
}