using Domain;
using Domain.Exceptions;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
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
    public class ShadowMissingValuePolicyTests
    {
        public SignalsWebService signalsWebService;

        [TestMethod]
        public void GivenASignalAndShadowMissingValuePolicy_WhenSettingPolicy_RepositorySetIsCalled()
        {
            signalsRepositoryMock = new Mock<ISignalsRepository>();

            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Day
            };

            GivenASignal(existingSignal);

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

            var existingShadowPolicy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Double, ShadowSignal = existingSignal.ToDto<Dto.Signal>()};

            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<ShadowMissingValuePolicy<double>>()));

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var domainExistingShadowPolicy = existingShadowPolicy.ToDomain<ShadowMissingValuePolicy<double>>();

            signalsWebService.SetMissingValuePolicy(1, existingShadowPolicy);

            missingValuePolicyRepositoryMock
                .Verify(mvprm => mvprm.Set(existingSignal, It.Is<ShadowMissingValuePolicy<double>>(smvp => 
                (
                    smvp.ShadowSignal.DataType ==  domainExistingShadowPolicy.ShadowSignal.DataType
                    && smvp.ShadowSignal.Granularity == domainExistingShadowPolicy.ShadowSignal.Granularity
                    && smvp.ShadowSignal.Id == domainExistingShadowPolicy.ShadowSignal.Id
                ))));
        }

        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.IncompatibleDataTypes))]
        public void GivenASignalAndShadowMissingValuePolicyWithSignalWithDifferentDataType_WhenSettingPolicy_ExceptionIsThrown()
        {
            signalsRepositoryMock = new Mock<ISignalsRepository>();

            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Day
            };

            var shadowSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Decimal,
                Granularity = Granularity.Day
            };

            GivenASignal(existingSignal);

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

            var existingShadowPolicy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Double, ShadowSignal = shadowSignal.ToDto<Dto.Signal>() };

            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<ShadowMissingValuePolicy<double>>()));

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);
            
            signalsWebService.SetMissingValuePolicy(1, existingShadowPolicy);
            
        }

        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.IncompatibleGranularities))]
        public void GivenASignalAndShadowMissingValuePolicyWithSignalWithDifferentGranularity_WhenSettingPolicy_ExceptionIsThrown()
        {
            signalsRepositoryMock = new Mock<ISignalsRepository>();

            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Day
            };

            var shadowSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Hour
            };

            GivenASignal(existingSignal);

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

            var existingShadowPolicy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Double, ShadowSignal = shadowSignal.ToDto<Dto.Signal>() };

            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<ShadowMissingValuePolicy<double>>()));

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);
            
            signalsWebService.SetMissingValuePolicy(1, existingShadowPolicy);
        }

        private void GivenASignal(Signal signal)
        {
            signalsRepositoryMock
                .Setup(sr => sr.Get(signal.Id.Value))
                .Returns(signal);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

    }
}
