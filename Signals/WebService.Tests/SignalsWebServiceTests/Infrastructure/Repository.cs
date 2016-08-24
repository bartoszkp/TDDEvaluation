using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories;
using Domain.Services.Implementation;
using Moq;

namespace WebService.Tests.SignalsWebServiceTests.Infrastructure
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

        protected void SetupAdd(int signalId)
        {
            signalsRepositoryMock
                .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                .Returns<Domain.Signal>(signal => { signal.Id = signalId;  return signal; });
        }

        protected void SetupGet()
        {
            signalsRepositoryMock
                .Setup(sr => sr.Get(It.IsAny<int>()))
                .Returns<Domain.Signal>(null);
        }

        protected void SetupGet(Domain.Signal signal)
        {
            signalsRepositoryMock
                .Setup(sr => sr.Get(It.Is<int>(id => id == signal.Id)))
                .Returns(signal);

            signalsRepositoryMock
                .Setup(sr => sr.Get(It.Is<Domain.Path>(p => Utils.ComparePath(p, signal.Path))))
                .Returns(signal);
        }
        
        protected void SetupGetData<T>(IEnumerable<Domain.Datum<T>> data)
        {
            signalsDataRepositoryMock
                .Setup(sdr => sdr.GetData<T>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(data);
        }

        protected void SetupMVPGet(Domain.MissingValuePolicy.MissingValuePolicyBase missingValuePolicy)
        {
            missingValuePolicyRepoMock
                .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                .Returns(missingValuePolicy);
        }

        protected void SetupMVPSet()
        {
            missingValuePolicyRepoMock
                .Setup(mvp => mvp.Set(It.IsAny<Domain.Signal>(), 
                    It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));
        }

        protected Mock<ISignalsRepository> signalsRepositoryMock;
        protected Mock<ISignalsDataRepository> signalsDataRepositoryMock;
        protected Mock<IMissingValuePolicyRepository> missingValuePolicyRepoMock;
    }
}
