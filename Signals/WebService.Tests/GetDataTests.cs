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

namespace WebService.Tests
{
    [TestClass]
    public class GetDataTests
    {
        private Mock<ISignalsDataRepository> signalsDataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private SignalsWebService signalsWebService;
        

        [TestMethod]
        public void GetSingleDatum_DatumIsReturned()
        {
            SetupWebService();
            var timestamp = new DateTime(2000, 1, 1);
            var signal = new Signal()
            { 
                DataType = DataType.Decimal,
                Granularity = Granularity.Month
            };

            signalsRepoMock.Setup(s => s.Get(1)).Returns(signal);
            
            signalsDataRepoMock.Setup(sr => sr.GetData<decimal>(
                It.Is<Signal>(s => s.DataType == DataType.Decimal && s.Granularity == Granularity.Month), 
                timestamp, 
                timestamp))
                .Returns(new List<Datum<decimal>>()
                {
                    new Datum<decimal>()
                    {
                        Quality = Quality.Fair,
                        Value = default(decimal),
                        Timestamp = timestamp
                    }
                });

            var result = signalsWebService.GetData(1,timestamp,timestamp);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(timestamp, result.First().Timestamp);
        }

        private void SetupWebService()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepoMock.Object, signalsDataRepoMock.Object, null);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }


    }
}
