using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.MissingValuePolicy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Tests
{
    [TestClass]
    public class ShadowMVPTests
    {
        SignalsWebService signalsWebService;

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenSettingShadowMVP_WithNoMatchingDataType_ThrowsException()
        {
            var signal = new Domain.Signal()
            {
                Id = 243,
                DataType = Domain.DataType.String,
                Granularity = Domain.Granularity.Second
            };

            var shadowMVP = new ShadowMissingValuePolicy()
            {
                DataType = Dto.DataType.Integer,
                ShadowSignal = new Dto.Signal()
                {
                    Id = 5234,
                    DataType = Dto.DataType.Integer,
                    Granularity = Dto.Granularity.Second
                }
            };

            setupWebService();
            setupGet(signal);

            signalsWebService.SetMissingValuePolicy(signal.Id.Value, shadowMVP);
        }

        private void setupWebService()
        {
            var signalsDomainService = new SignalsDomainService(
                signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        private void setupGet(Domain.Signal signal)
        {
            signalsRepositoryMock
                .Setup(sr => sr.Get(signal.Id.Value))
                .Returns(signal);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
    }
}
