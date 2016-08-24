using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories;
using Domain.Services.Implementation;
using Moq;

namespace WebService.Tests
{
    public abstract class SignalsWebServiceRepository
    {
        protected ISignalsWebService signalsWebService { get; private set; }

        public SignalsWebServiceRepository()
        {
            signalsRepositoryMock = new Mock<ISignalsRepository>();
            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
            missingValuePolicyRepoMock = new Mock<IMissingValuePolicyRepository>();

            var signalsDomainService = new SignalsDomainService(
                signalsRepositoryMock.Object,
                signalsDataRepositoryMock.Object,
                missingValuePolicyRepoMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        protected abstract void Setup(params object[] param);

        protected Mock<ISignalsRepository> signalsRepositoryMock;
        protected Mock<ISignalsDataRepository> signalsDataRepositoryMock;
        protected Mock<IMissingValuePolicyRepository> missingValuePolicyRepoMock;
    }
}
