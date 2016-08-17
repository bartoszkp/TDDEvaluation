using Domain;
using Domain.Exceptions;
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
    public class NoneQualityDataFillTests
    {
        private SignalsWebService signalsWebService;

        [TestMethod]
        public void SignalHasNoData_GetData_ReturnsFilledData()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Second
            };

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(signal);
            Mock<NoneQualityMissingValuePolicy<int>> mvp = new Mock<NoneQualityMissingValuePolicy<int>>();

            mvpRepoMock.Setup(m => m.Get(signal)).Returns(mvp.Object);
            signalsDataRepoMock.Setup(s => s.GetData<int>(signal, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1, 0, 1, 0)))
                .Returns(new List<Datum<int>>());

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1, 0, 1, 0));

            Assert.AreEqual(60, result.Count());
        }

        [TestMethod]
        public void GivenSignal_GetData_GetsItAndFillsMissingParts()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Month
            };

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(signal);
            Mock<NoneQualityMissingValuePolicy<int>> mvp = new Mock<NoneQualityMissingValuePolicy<int>>();

            mvpRepoMock.Setup(m => m.Get(signal)).Returns(mvp.Object);
            signalsDataRepoMock.Setup(s => s.GetData<int>(signal, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1)))
                .Returns(new List<Datum<int>>()
                {
                    new Datum<int>() {Quality = Quality.None,Value = (int)0,Timestamp =  new DateTime(2000, 1, 1)}
                });

            var expectedFilledDatum = new Datum<int>() { Quality = Quality.None, Value = (int)0, Timestamp = new DateTime(2000, 2, 1) };

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(expectedFilledDatum.Timestamp, new DateTime(2000, 2, 1));
            Assert.AreEqual(expectedFilledDatum.Quality, Quality.None);
            Assert.AreEqual(expectedFilledDatum.Value, 0);

        }


        [TestMethod]
        [ExpectedException(typeof(NoSuchSignalException))]
        public void SignalNotInDatabase_GetData_ThrowsException()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns((Signal)null);
            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1, 0, 1, 0));
        }





        private void SetupWebService()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepoMock.Object, signalsDataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }








        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

    }
}
