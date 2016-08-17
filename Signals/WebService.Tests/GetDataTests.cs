using Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Domain.Services.Implementation;
using Domain;
using Domain.Exceptions;
using Domain.MissingValuePolicy;

namespace WebService.Tests
{
    [TestClass]
    public class GetDataTests
    {

        private SignalsWebService signalsWebService;


        [TestMethod]
        [ExpectedException(typeof(NoSuchSignalException))]
        public void SignalNotInDatabase_GetData_ThrowsException()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns((Signal)null);
            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));
        }



        [TestMethod]
        public void SignalExists_GetData_WithSameTimestaps_DataReturned()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Month
            };

            var timestamp = new DateTime(2000, 1, 1);

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(signal);
            Mock<NoneQualityMissingValuePolicy<int>> mvp = new Mock<NoneQualityMissingValuePolicy<int>>();

            mvpRepoMock.Setup(m => m.Get(signal)).Returns(mvp.Object);


            signalsDataRepoMock.Setup(s => s.GetData<int>(signal, timestamp, timestamp))
                .Returns(new List<Datum<int>>());

            var result = signalsWebService.GetData(1, timestamp, timestamp);

            var fetchedDatumObject = result.ElementAt(0);

            Assert.AreEqual(1,result.Count());
            Assert.AreEqual(Dto.Quality.None, fetchedDatumObject.Quality);
            Assert.AreEqual(0, fetchedDatumObject.Value);
            Assert.AreEqual(timestamp, fetchedDatumObject.Timestamp);

        }


        private void SetupWebService()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepoMock.Object, signalsDataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        private Mock<ISignalsDataRepository> signalsDataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

    }
}
