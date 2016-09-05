using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebService.Tests.WebService.Tests;

namespace WebService.Tests
{
    [TestClass]
    public class SignalDeletionTests
    {
        private SignalsWebService signalsWebService;

        [TestMethod]
        [ExpectedException(typeof(NoSuchSignalException))]
        public void SignalNotExists_DeleteSignal_ExceptionThrown()
        {
            SignalsDomainService domainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(domainService);

            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns((Signal)null);

            signalsWebService.Delete(1);

        }

        [TestMethod]
        public void DeletingSignalShouldAlsoDeleteItsData()
        {
            var signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
            var signalsDomainService = new SignalsDomainService(
                signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);

            signalsRepositoryMock
                .Setup(sr => sr.Get(It.IsAny<int>()))
                .Returns(new Signal() { DataType = DataType.Integer });

            signalsWebService.Delete(465);

            signalsDataRepositoryMock.Verify(sdr => sdr.DeleteData<int>(It.Is<Signal>(s => s.DataType == DataType.Integer)));
        }


        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

    }
}
