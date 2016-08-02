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
            public void WhenGettingByPath_ReturnsIt()
            {
                GivenNoSignals();
                var signalDomain = new Domain.Signal()
                {
                    Id = 23,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("sfd/klpko"),
                };
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.Is<Domain.Path>(p => p.ToString() == signalDomain.Path.ToString())))
                    .Returns(signalDomain);

                var signalDto = signalDomain.ToDto<Dto.Signal>();
                var result = signalsWebService.Get(signalDto.Path);

                Assert.AreEqual(signalDto.Id, result.Id);
                Assert.AreEqual(signalDto.DataType, result.DataType);
                Assert.AreEqual(signalDto.Granularity, result.Granularity);
                Assert.AreEqual(signalDto.Path.ToString(), result.Path.ToString());
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.PathIsEmptyException))]
            public void WhenGettingByPathWithEmptyPath_ThrowPathIsEmptyException()
            {
                GivenNoSignals();
                signalsWebService.Get(new Dto.Path());
            }

            [TestMethod]
            public void WhenGettingMissingValuePolicyForNewSignal_ReturnsNull()
            {
                var signal = new Domain.Signal()
                {
                    Id = 23
                };
                prepareMissingValuePolicy();
                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Get(It.Is<Domain.Signal>(s => s == signal)))
                    .Returns<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>(null);

                var result = signalsWebService.GetMissingValuePolicy(signal.Id.Value);
                Assert.IsNull(result);
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SignalWithThisIdNonExistException))]
            public void WhenGettingMissingValuePolicyForNonExistSignal_ThrowSignalWithThisIdNonExistException()
            {
                int signalId = 249;
                prepareMissingValuePolicy();
                signalsRepositoryMock
                    .Setup(sr => sr.Get(signalId))
                    .Returns<Domain.Signal>(null);
                signalsWebService.GetMissingValuePolicy(signalId);
            }

            [TestMethod]
            public void WhenGettingMissingValuePolicy_ReturnsIt()
            {
                var signal = new Domain.Signal()
                {
                    Id = 65,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Day,
                    Path = Path.FromString("jkl/fg")
                };
                prepareMissingValuePolicy();
                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
                var mvp = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
                {
                    Id = 32,
                    DataType = Dto.DataType.Double,
                    Signal = signal.ToDto<Dto.Signal>()
                };
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Get(It.Is<Domain.Signal>(s => s == signal)))
                    .Returns(mvp.ToDomain <Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>());

                Dto.MissingValuePolicy.MissingValuePolicy result 
                    = signalsWebService.GetMissingValuePolicy(signal.Id.Value);
                Assert.AreEqual(mvp.Id, result.Id);
                Assert.AreEqual(mvp.DataType, result.DataType);
                Assert.AreEqual(mvp.Signal.Id, result.Signal.Id);
                Assert.AreEqual(mvp.Signal.DataType, result.Signal.DataType);
                Assert.AreEqual(mvp.Signal.Granularity, result.Signal.Granularity);
                Assert.AreEqual(mvp.Signal.Path.ToString(), result.Signal.Path.ToString());
            }

            [TestMethod]
            public void WhenSettingMissingValuePolicy_CallsRepositoryGet()
            {
                var signal = new Domain.Signal()
                {
                    Id = 23,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("r/vbc")
                };
                prepareMissingValuePolicy();
                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Set(It.Is<Domain.Signal>(s => s == signal), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

                var mvp = new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy()
                {
                    DataType = Dto.DataType.Boolean,
                    Signal = signal.ToDto<Dto.Signal>()
                };
                var mvpDomain = mvp.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

                signalsWebService.SetMissingValuePolicy(signal.Id.Value, mvp);

                missingValuePolicyRepositoryMock.Verify(mvpr => mvpr.Set(
                    It.Is<Domain.Signal>(s => s == signal),
                    It.Is<Domain.MissingValuePolicy.MissingValuePolicyBase>(
                        m => m.NativeDataType == mvpDomain.NativeDataType
                        && m.Signal.Id == signal.Id
                        && m.Signal.DataType == signal.DataType
                        && m.Signal.Granularity == signal.Granularity
                        && m.Signal.Path.ToString() == signal.Path.ToString())));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SignalWithThisIdNonExistException))]
            public void WhenSettingMissingValuePolicyForNonExistSignal_ThrowSignalWithThisIdNonExistException()
            {
                int signalId = 249;
                prepareMissingValuePolicy();
                signalsRepositoryMock
                    .Setup(sr => sr.Get(signalId))
                    .Returns<Domain.Signal>(null);
                signalsWebService.SetMissingValuePolicy(signalId, new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy());
            }

            private Dto.Signal SignalWith(
                int? id = null,
                Dto.DataType dataType = Dto.DataType.Boolean,
                Dto.Granularity granularity = Dto.Granularity.Day,
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
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Signal signal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
            }

            private void prepareMissingValuePolicy()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
        }
    }
}