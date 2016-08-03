using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

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
                    Id = 100,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day,
                    Path = Path.FromString("root/signal"),
                };
                signalsRepositoryMock.Setup(srm => srm.Get(It.Is<Domain.Path>(s => s.ToString() == signalDomain.Path.ToString())))
                    .Returns(signalDomain);

                var signalDto = signalDomain.ToDto<Dto.Signal>();
                var result = signalsWebService.Get(signalDto.Path);

                MatchSignals(signalDto.ToDomain<Domain.Signal>(), result.ToDomain<Domain.Signal>());
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.PathNotExistException))]
            public void WhenGettingPathIsNotExistOrIsNullOrIsIsEmpty_ReportException()
            {
                GivenNoSignals();
                signalsWebService.Get(null);
            }


            [TestMethod]
            public void WhenSetMissingValuePolicy_RepositoryGet()
            {
                SetupSignalsWebServiceAndMissingValue();
                var signal = new Domain.Signal()
                {
                    Id = 101,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Minute,
                    Path = Path.FromString("path/pat/pa")
                };
                signalsRepositoryMock.Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Set(It.Is<Domain.Signal>(s => s == signal),
                                            It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

                var mvp = new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy()
                {
                    DataType = Dto.DataType.Decimal,
                    Signal = signal.ToDto<Dto.Signal>()
                };
                var mvpDomain = mvp.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

                signalsWebService.SetMissingValuePolicy(signal.Id.Value, mvp);

                missingValuePolicyRepositoryMock.Verify(mvpr => mvpr.Set(
                    It.Is<Domain.Signal>(s => s == signal),
                    It.Is<Domain.MissingValuePolicy.MissingValuePolicyBase>(
                        m => m.NativeDataType == mvpDomain.NativeDataType
                        && VerifyMissingValue(m,signal))));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SignalIsNotException))]
            public void WhenSettingMissingValuePolicyWhenNotExis_ReportException()
            {
                SetupSignalsWebServiceAndMissingValue();
                int signalId = 101;
                signalsRepositoryMock.Setup(sr => sr.Get(signalId))
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



            private bool MatchSignals(Signal signal, Signal result)
            {
                return ((signal.Id == result.Id) && (signal.DataType == result.DataType)
                    && (signal.Granularity == result.Granularity) && (signal.Path.ToString() == result.Path.ToString()));
            }

            private bool VerifyMissingValue(Domain.MissingValuePolicy.MissingValuePolicyBase missingValuePolicyBase, Signal signal)
            {
                return ((signal.Id == missingValuePolicyBase.Signal.Id) && (signal.DataType == missingValuePolicyBase.Signal.DataType)
                    && (signal.Granularity == missingValuePolicyBase.Signal.Granularity) && (signal.Path.ToString() == missingValuePolicyBase.Signal.Path.ToString()));
            }

            private void SetupSignalsWebServiceAndMissingValue()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }
            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;

        }
    }
}