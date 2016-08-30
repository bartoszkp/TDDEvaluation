using System;
using System.Linq;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebService.Tests.SignalsWebServiceTests.Infrastructure;
using Domain.Exceptions;
using Domain;

namespace WebService.Tests.SignalsWebServiceTests.Infrastructure
{
    [TestClass]
    public class SignalsWebServiceDeleteIdTests : SignalsWebServiceRepository
    {
        [TestMethod]
        [ExpectedException(typeof(SignalNotExistException))]
        public void Delete_NoSignalWithGivenId_ThrowException()
        {
            signalsWebService.Delete(3);
        }

        [TestMethod]
        public void Delete_SignalWithGivenIdExist_DeleteCalled()
        {
            var signal = new Domain.Signal() { Id = 1, DataType = Domain.DataType.Decimal,
                Granularity = Domain.Granularity.Second, Path = Domain.Path.FromString("x/y") };

            SetupGet(signal);

            signalsWebService.Delete(1);

            signalsRepositoryMock.Verify(x=>x.Delete(It.IsAny<Signal>()), Times.AtLeastOnce);
        }


    }
}
