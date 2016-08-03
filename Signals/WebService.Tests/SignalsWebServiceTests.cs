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
                signalsRepositoryMock.Setup(srm => srm.Get(It.Is<Domain.Path>(p => p.ToString() == signalDomain.Path.ToString())))
                    .Returns(signalDomain);

                var signalDto = signalDomain.ToDto<Dto.Signal>();
                var result = signalsWebService.Get(signalDomain.ToDto<Dto.Signal>().Path);

                Assert.AreEqual(signalDto.Id, result.Id);
                Assert.AreEqual(signalDto.DataType, result.DataType);
                Assert.AreEqual(signalDto.Granularity, result.Granularity);
                Assert.AreEqual(signalDto.Path.ToString(), result.Path.ToString());
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.PathNotExistException))]
            public void WhenGettingPathIsNotExistOrIsNullOrIsIsEmpty_ReportException()
            {
                GivenNoSignals();
                signalsWebService.Get(null);
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

            private Mock<ISignalsRepository> signalsRepositoryMock;
        }
    }
}