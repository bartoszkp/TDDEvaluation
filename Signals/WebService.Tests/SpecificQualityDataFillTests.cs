using Domain;
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
    public class SpecificQualityDataFillTests
    {
        private SignalsWebService signalsWebService;


        [TestMethod]
        public void SignalHasNoData_GetData_ReturnsFilledData()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Month
            };
            var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
            {
                DataType = Dto.DataType.Integer,
                Quality = Dto.Quality.Bad,
                Value = (int)1
            };
            SetupWebService(signal,policy);
            var domainExistingPolicy = (Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<int>)policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            missingValuePolicyRepositoryMock
                .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.Is<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<int>>(mv =>
                (
                    mv.NativeDataType == domainExistingPolicy.NativeDataType
                    && mv.Quality == domainExistingPolicy.Quality
                    && mv.Value == domainExistingPolicy.Value
                ))));
            
        }


        private void SetupWebService(Signal signal, Dto.MissingValuePolicy.SpecificValueMissingValuePolicy policy)
        {
            signalsRepositoryMock = new Mock<ISignalsRepository>();
            signalsRepositoryMock.Setup(srm => srm.Get(signal.Id.Value))
                .Returns(signal);

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);

            signalsWebService.SetMissingValuePolicy(signal.Id.Value, policy);
        }


        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();


    }
}
