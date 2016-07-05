using Domain.Repositories;
using Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Domain.Tests
{
    [TestClass]
    public class SignalsDomainServiceTests
    {
        private ISignalsDomainService signalsDomainService;

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
            signalsRepository = MockRepository.GenerateStub<ISignalsRepository>();
            signalsDataRepository = MockRepository.GenerateStub<ISignalsDataRepository>();
            missingValuePolicyRepository = MockRepository.GenerateStub<IMissingValuePolicyRepository>();

            signalsDomainService = new Domain.Services.Implementation.SignalsDomainService(signalsRepository, signalsDataRepository, missingValuePolicyRepository);
        }

        private void GivenASignal(Signal s)
        {
            GivenNoSignals();

            signalsRepository.Stub(sr => sr.Get(s.Path)).Return(s);
        }

        private Signal WhenGettingSignalByPath(Path path)
        {
            return signalsDomainService.Get(path);
        }

        private ISignalsRepository signalsRepository;
        private ISignalsDataRepository signalsDataRepository;
        private IMissingValuePolicyRepository missingValuePolicyRepository;
    }
}
