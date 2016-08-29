using Domain;
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
    public class ZeroOrderDataFillTests
    {
        private SignalsWebService signalsWebService;

        [TestMethod]
        public void GetData_OlderSamplesArePropagated()
        {
            SignalsDomainService domainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(domainService);

            signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns(new Signal()
            {
                DataType = DataType.String,
                Granularity = Granularity.Day
            });

            var mvpMock = new Mock<ZeroOrderMissingValuePolicy<string>>();
            missingValuePolicyRepositoryMock.Setup(m => m.Get(It.IsAny<Signal>())).Returns(mvpMock.Object);

            var from = new DateTime(2000, 1, 10);
            var to = new DateTime(2000, 1, 11);

            signalsDataRepositoryMock.Setup(s => s.GetData<string>(It.IsAny<Signal>(), from, to))
               .Returns(new List<Datum<string>>
               {
                 //  new Datum<string>() { Timestamp = new DateTime(2000, 1, 1), Value = "first", Quality = Quality.Good }
               });


            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 10), new DateTime(2000, 1, 11));

            var fetchedDatum = result.ElementAt(0);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(from, fetchedDatum.Timestamp);
            Assert.AreEqual(Dto.Quality.None, fetchedDatum.Quality);

        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
    }
}
