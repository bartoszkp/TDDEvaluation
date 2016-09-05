using Domain;
using Domain.Exceptions;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Domain.Services.Implementation;
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
    public class ShadowDataFillTests
    {
        private SignalsWebService signalsWebService;

        [TestMethod]
        [ExpectedException(typeof(ShadowSignalNotCorrectlyException))]
        public void WhenShadowMVPHaveSignalWithIncorrectDataType_ThrowExcpetion()
        {
            var signal = new Signal()
            {
                Id = 14,
                DataType = DataType.Decimal,
                Granularity = Granularity.Minute
            };
            SetupMocks(signal);

            var mvp = new Dto.MissingValuePolicy.ShadowMissingValuePolicy()
            {
                DataType = Dto.DataType.Double,
                ShadowSignal = new Dto.Signal()
                {
                    DataType = Dto.DataType.Double,
                    Granularity = Dto.Granularity.Minute
                }
            };

            signalsWebService.SetMissingValuePolicy(signal.Id.Value, mvp);
        }

        [TestMethod]
        public void WhenShadowMVPHaveCorrectSignal_CallsMVPRepository()
        {
            var signal = new Signal()
            {
                Id = 76,
                DataType = DataType.Integer,
                Granularity = Granularity.Hour
            };
            SetupMocks(signal);

            var mvp = new Dto.MissingValuePolicy.ShadowMissingValuePolicy()
            {
                DataType = Dto.DataType.Integer,
                ShadowSignal = new Dto.Signal()
                {
                    DataType = Dto.DataType.Integer,
                    Granularity = Dto.Granularity.Hour
                }
            };

            signalsWebService.SetMissingValuePolicy(signal.Id.Value, mvp);

            mvpMock.Verify(mvpM => mvpM.Set(It.Is<Signal>(s => s == signal), It.Is<MissingValuePolicyBase>(mvpb => mvpb.GetType() == typeof(ShadowMissingValuePolicy<int>))));
        }

        private void SetupMocks(Signal signal, int? signalId = null)
        {
            var signalsDomainService = new SignalsDomainService(
                signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);

            if (signalId == null)
                signalId = signal.Id;

            signalsRepositoryMock
                .Setup(sr => sr.Get(It.Is<int>(id => id == signalId)))
                .Returns(signal);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<IMissingValuePolicyRepository> mvpMock = new Mock<IMissingValuePolicyRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
    }
}
