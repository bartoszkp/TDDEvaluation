using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Domain.Repositories;
using Domain;
using Domain.Services.Implementation;

namespace WebService.Tests
{
    [TestClass]
    public class PathEntryTests
    {
        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private SignalsWebService signalsWebService;


        [TestMethod]
        public void GivenNoSuchSignals_GetPathEntry_ReturnsNull()
        {
            SetupWebService();

            signalsRepoMock.Setup(sr => sr.GetAllWithPathPrefix(It.IsAny<Path>())).Returns((IEnumerable<Signal>)null);

            var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "x", "y" } });

            Assert.IsNull(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullPath_ExceptionIsThrown()
        {
            SetupWebService();
            signalsWebService.GetPathEntry(null);
        }
        

        [TestMethod]
        public void GivenSignals_GetPathEntry_ReturnsIt()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.GetAllWithPathPrefix(It.IsAny<Path>()))
                .Returns(new[] { new Signal() {Path = Path.FromString("x/y/z") } });

            var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "x", "y" } });

            Assert.IsNotNull(result);

        }
        
        private void SetupWebService()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepoMock.Object, null, null);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }


    }
}
