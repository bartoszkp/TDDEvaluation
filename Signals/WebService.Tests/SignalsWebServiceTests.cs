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
            public void GivenNoSignals_WhenAddASignalWithId_ThrowIdNotNullException()
            {
                GivenNoSignals();

                var result = signalsWebService.Add(SignalWith(id: 1));
            }



            [TestMethod]
            public void GivenNoSignals_WhenAddASignal_ReturnsIt()
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
            public void GivenNoSignals_WhenAddASignal_CallsRepositoryAdd()
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
            public void GivenNoSignals_WhenGetById_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.GetById(0);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGetByItsId_ReturnsIt()
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
            public void WhenGetByPath_ReturnsIt()
            {
                GivenNoSignals();
                Domain.Signal signalDomain = SignalWith(100, DataType.Boolean, Granularity.Day, Path.FromString("root/signal"));
                
                signalsRepositoryMock.Setup(srm => srm.Get(It.Is<Domain.Path>(s => s.ToString() == signalDomain.Path.ToString())))
                    .Returns(signalDomain);

                var signalDto = signalDomain.ToDto<Dto.Signal>();
                var result = signalsWebService.Get(signalDto.Path);

                MatchSignals(signalDto.ToDomain<Domain.Signal>(), result.ToDomain<Domain.Signal>());
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.PathNotExistException))]
            public void WhenGetPathIsNotExistOrIsNullOrIsIsEmpty_ReportException()
            {
                GivenNoSignals();
                signalsWebService.Get(null);
            }



            [TestMethod]
            public void WhenSetMissingValuePolicy_RepositoryGet()
            {
                SetupSignalsWebServiceAndMissingValue();
                Domain.Signal signal = SignalWith(105, DataType.Decimal, Granularity.Minute, Path.FromString("path/pat/pa"));
                
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
            public void WhenSetMissingValuePolicyWhenNotExis_ReportException()
            {
                SetupSignalsWebServiceAndMissingValue();
                int signalId = 101;
                signalsRepositoryMock.Setup(sr => sr.Get(signalId))
                    .Returns<Domain.Signal>(null);
                signalsWebService.SetMissingValuePolicy(signalId, new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy());
            }



            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.IdNotNullException))]
            public void WhenGetMissingValueForNewSignal_ReportException()
            {
                var signal = new Domain.Signal(){ Id = 103 };
                SetupSignalsWebServiceAndMissingValue();
                signalsRepositoryMock.Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Domain.Signal>(s => s == signal)))
                    .Returns<DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger>(null);

               signalsWebService.GetMissingValuePolicy(signal.Id.Value);
            }
           [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.IdNotNullException))]
            public void WhenGetMissingValue_WhenSignalNonExist_ReportException()
            {
                int signalId = 104;
                SetupSignalsWebServiceAndMissingValue();
                signalsRepositoryMock.Setup(sr => sr.Get(signalId))
                    .Returns<Domain.Signal>(null);

                signalsWebService.GetMissingValuePolicy(signalId);
            }

            [TestMethod]
            public void WhenGetMissingValue_ReturnsIt()
            {
                Domain.Signal signal = SignalWith(105, DataType.Integer, Granularity.Month, Path.FromString("root/roo/ro"));
                SetupSignalsWebServiceAndMissingValue();
                signalsRepositoryMock.Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
                var missingValuePolicy = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                {
                    Id = 45,
                    Quality = Quality.Fair,
                    Signal = signal,
                    Value = (double)2.25
                };
                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Domain.Signal>(s => s==signal)))
                    .Returns(missingValuePolicy);
                var missingValuePolicyDto = missingValuePolicy.ToDto<Dto.MissingValuePolicy.SpecificValueMissingValuePolicy>();

                Dto.MissingValuePolicy.MissingValuePolicy result
                    = signalsWebService.GetMissingValuePolicy(signal.Id.Value);
                Assert.IsTrue(EqualsMissingValuePolicy(missingValuePolicyDto, result));
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


            private bool EqualsMissingValuePolicy(Dto.MissingValuePolicy.MissingValuePolicy missingValuePolicySignal, Dto.MissingValuePolicy.MissingValuePolicy missingValuePolicyResult)
            {
                return ((missingValuePolicySignal.Id == missingValuePolicyResult.Id)
                    && (missingValuePolicySignal.DataType == missingValuePolicyResult.DataType)
                    && (missingValuePolicySignal.Signal.Id == missingValuePolicyResult.Signal.Id)
                    && (missingValuePolicySignal.Signal.DataType == missingValuePolicyResult.Signal.DataType)
                    && (missingValuePolicySignal.Signal.Granularity == missingValuePolicyResult.Signal.Granularity)
                    && (missingValuePolicySignal.Signal.Path.ToString() == missingValuePolicyResult.Signal.Path.ToString()));
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

        }
    }
}