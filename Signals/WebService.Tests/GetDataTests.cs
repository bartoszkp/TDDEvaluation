using DataAccess.GenericInstantiations;
using Domain;
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
    public class GetDataTests
    {
        private SignalsWebService signalsWebService;

        [TestMethod]
        public void NoneQualityMvp_GetData_WithSameTimestamps_ReturnsSingleDatum() 
        {
            SetupWebService();
            var returnedSignal = new Signal() {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Month
            };

            missingValuePolicyRepositoryMock.Setup(m => m.Get(returnedSignal)).Returns(new NoneQualityMissingValuePolicyDouble() {
                Signal = returnedSignal
            });

            var ts = new DateTime(2000, 2, 1);

            signalDataRepositoryMock.Setup(s => s.GetData<double>(returnedSignal, ts, ts)).Returns(new List<Datum<double>>()
            {
                new Datum<double>() {Quality = Quality.Bad, Timestamp = new DateTime(2000, 2, 1), Value = 1.5 }
            });

            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            var results = signalsWebService.GetData(1, ts, ts);
            var returnedDatum = results.ElementAt(0);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(Dto.Quality.Bad, returnedDatum.Quality);
            Assert.AreEqual(1.5, returnedDatum.Value);

        }

        [TestMethod]
        public void ZeroOrderQualityMvp_GetData_WithSameTimestamps_ReturnsSingleDatum() 
        {
            SetupWebService();
            var returnedSignal = new Signal() {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Month
            };

            missingValuePolicyRepositoryMock.Setup(m => m.Get(returnedSignal)).Returns(new ZeroOrderMissingValuePolicyDouble() {
                Signal = returnedSignal
            });

            var ts = new DateTime(2000, 2, 1);

            signalDataRepositoryMock.Setup(s => s.GetData<double>(returnedSignal, ts, ts)).Returns(new List<Datum<double>>()
            {
                new Datum<double>() {Quality = Quality.Bad, Timestamp = new DateTime(2000, 2, 1), Value = 1.5 }
            });

            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            var results = signalsWebService.GetData(1, ts, ts);
            var returnedDatum = results.ElementAt(0);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(Dto.Quality.Bad, returnedDatum.Quality);
            Assert.AreEqual(1.5, returnedDatum.Value);
        }



        private void SetupWebService() 
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepositoryMock.Object,
                signalDataRepositoryMock.Object, 
                missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(domainService);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

    }
}
