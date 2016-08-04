using System;
using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


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
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenGettingByAnyPath_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, null);
                var signalWebService = new SignalsWebService(signalDomainService);
                var dummyPath = Path.FromString("root/signal");

                signalWebService.Get(dummyPath.ToDto<Dto.Path>());
                
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingByPathWhichDoesNotExistInRepository_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, null);
                var signalWebService = new SignalsWebService(signalDomainService);
                signalWebService.Add(new Dto.Signal()
                {
                    DataType = Dto.DataType.Decimal,
                    Granularity = Dto.Granularity.Hour,
                    Path = new Dto.Path() { Components = new string[] { "root", "signal1" } }
                });
                var dummyPath = Path.FromString("root/signal2");

                signalWebService.Get(dummyPath.ToDto<Dto.Path>());
            }

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

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenGettingMissingValuePolicyOfAnySignal_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                signalRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns<Signal>(signal => signal);
                signalRepositoryMock.Setup(sr => sr.Get(It.IsAny<Path>())).Returns<Signal>(signal => signal);
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, null);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.GetMissingValuePolicy(1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingMissingValuePolicyOfSignalWhichDoesNotExistInRepository_ThrowedIsArgumentException()
            {
                var signalRepositoryMock = new Mock<ISignalsRepository>();
                signalRepositoryMock.Setup(sr => sr.Get(1)).Returns<Signal>(signal => signal);
                var signalDomainService = new SignalsDomainService(signalRepositoryMock.Object, null, null);
                var signalWebService = new SignalsWebService(signalDomainService);

                signalWebService.GetMissingValuePolicy(1);
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

            private Mock<ISignalsRepository> signalsRepositoryMock;
        }


    }
}