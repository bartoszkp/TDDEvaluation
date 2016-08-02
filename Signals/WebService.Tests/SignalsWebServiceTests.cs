using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
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
            public void GivenASignal_WhenGettingByPath_ReturnsIt()
            {
                Dto.Path path = new Dto.Path() { Components = new[] { "x", "y" } };
                GivenASignal(SignalWith(
                    id: 1,
                    dataType: DataType.Boolean,
                    granularity: Granularity.Month,
                    path: path.ToDomain<Domain.Path>()
                    ));

                var result = signalsWebService.Get(path.ToDto<Dto.Path>());

                Assert.AreEqual(Dto.DataType.Boolean, result.DataType);
                Assert.AreEqual(Dto.Granularity.Month, result.Granularity);
                CollectionAssert.AreEqual(path.Components.ToArray(), result.Path.Components.ToArray());
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignal_WhenGettingByNonExistentPath_ReturnException()
            {
                GivenNoSignals();
                signalsWebService.Get(null);
            }

            [TestMethod]
            public void GivenASignal_WhenSetMissingValuePolicy_CalledMissingValuePolicyRepository()
            {
                GivenASignal(GetTestSignal());

                Dto.MissingValuePolicy.MissingValuePolicy mvpDTO = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
                
                signalsWebService.SetMissingValuePolicy(1, mvpDTO);

                missingValuePolicyRepositoryMock.Verify(sr => sr.Set(It.IsAny<Domain.Signal>(), It.IsAny<MissingValuePolicyBase>()));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSetMissingValuePolicyWithInvalidId_ReturnException()
            {
                int correctId = 1;
                int invalidId = 123;

                GivenASignal(GetTestSignal());

                signalsWebService.SetMissingValuePolicy(invalidId, new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy());
            }
            [TestMethod]
            public void GivenASignal_WhenGetMissingValuePolicy_ColledmissingValuePolicyRepository()
            {
                GivenASignal(GetTestSignal());

                signalsWebService.GetMissingValuePolicy(1);

                missingValuePolicyRepositoryMock.Verify(s => s.Get(It.IsAny<Signal>()));
            }

            [TestMethod]
            public void GivenASignal_WhenGetMissingValuePolicyWithNewSignal_ReturnNull()
            {
                GivenASignal(GetTestSignal());

                var res = signalsWebService.GetMissingValuePolicy(1);

                Assert.IsNull(res);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGetMissingValuePolicyWithInvalidID_ReturnException()
            {
                GivenASignal(GetTestSignal());
                int invalidID = -1;
                var res = signalsWebService.GetMissingValuePolicy(invalidID);
            }

            [TestMethod]
            public void GivenASignal_WhenGetMissingValuePolicy_ReturnCorrectMVP()
            {
                Signal signal = GetTestSignal();
                GivenASignal(signal);
                int id = 1;

                SpecificValueMissingValuePolicy<bool> MVP = new SpecificValueMissingValuePolicy<bool>()
                {
                    Id = id,
                    Quality = Quality.Fair,
                    Signal = signal,
                    Value = true,
                };

                missingValuePolicyRepositoryMock
                    .Setup(s => s.Get(signal))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyBoolean() {
                        Id = id,
                        Quality = Quality.Fair,
                        Signal = signal,
                        Value = true,
                    });

                Dto.MissingValuePolicy.MissingValuePolicy resultMVP = signalsWebService.GetMissingValuePolicy(id);

                Assert.AreEqual(MVP.Id.Value, resultMVP.Id.Value);
                Assert.IsTrue(EqualsSignal(MVP.Signal, resultMVP.Signal.ToDomain<Domain.Signal>()));
                Assert.AreEqual(MVP.NativeDataType.Name.ToString(), resultMVP.DataType.ToString());
            }

            private bool EqualsSignal(Signal a, Signal b)
            {
                if (a.Id != b.Id) return false;
                if (a.DataType != b.DataType) return false;
                if (a.Granularity != b.Granularity) return false;
                if (a.Path.ToString() != b.Path.ToString()) return false;
                return true;
            }
            private Signal GetTestSignal()
            {
                return new Signal()
                {
                    Id = 1,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("x/y"),
                };
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
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Signal signal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Path))
                    .Returns(signal);
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
        }
    }
}