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

namespace WebService.Tests
{
    [TestClass]
    public class SignalDataTests
    {
        private SignalsWebService signalsWebService;


        [TestMethod]
        [ExpectedException(typeof(NoSuchSignalException))]
        public void SignalNotExists_GetData_ThrowsException()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns((Signal)null);


            var result = signalsWebService.GetData(1, new DateTime(), new DateTime());

        }


        private void SetupWebService()
        {
            signalsDataRepoMock = new Mock<ISignalsDataRepository>();
            signalsRepoMock = new Mock<ISignalsRepository>();
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, signalsDataRepoMock.Object, null);
            signalsWebService = new SignalsWebService(domainService);

        }


        private Mock<ISignalsDataRepository> signalsDataRepoMock;
        private Mock<ISignalsRepository> signalsRepoMock;
    }
}
