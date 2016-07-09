using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace WebService.Tests
{
    [TestClass]
    public class SignalsWebServiceTests
    {
        [TestMethod]
        public void GivenNoSignals_WhenGettingSignal_ThrowsKeyNotFoundException()
        {
            GivenNoSignals();

            var result = WhenGettingSignalByPath(Path.FromString(string.Empty));

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GivenASignal_WhenGettingByPath_ReturnsTheCorrectSignal()
        {
            GivenASignal(new Signal() { Path = Path.FromString(string.Empty) });

            var result = WhenGettingSignalByPath(Path.FromString(string.Empty));

            Assert.IsNotNull(result);
        }

        private void GivenNoSignals()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        private void GivenASignal(Signal signal)
        {
            GivenNoSignals();

            signalsRepositoryMock.Setup(r => r.Get(signal.Path)).Returns(signal);
        }

        private Signal WhenGettingSignalByPath(Path path)
        {
            return signalsWebService.Get(path.ToDto<Dto.Path>())?.ToDomain<Signal>();
        }

        private ISignalsWebService signalsWebService;
        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
    }
}