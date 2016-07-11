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
        [TestInitialize]
        public void TestInitialize()
        {
            SetupWebService();
        }

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
        }

        private void GivenASignal(Signal signal)
        {
            signalsRepositoryStub.Setup(r => r.Get(signal.Path)).Returns(signal);
        }

        private Signal WhenGettingSignalByPath(Path path)
        {
            return signalsWebService.Get(path.ToDto<Dto.Path>())?.ToDomain<Signal>();
        }

        private void SetupWebService()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepositoryStub.Object, signalsDataRepositoryStub.Object, missingValuePolicyRepositoryStub.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        private ISignalsWebService signalsWebService;
        private Mock<ISignalsRepository> signalsRepositoryStub = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryStub = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryStub = new Mock<IMissingValuePolicyRepository>();
    }
}