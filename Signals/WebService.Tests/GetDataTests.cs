using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Domain.Repositories;
using Domain.Services.Implementation;
using Domain;
using DataAccess.GenericInstantiations;

namespace WebService.Tests
{
    [TestClass]
    public class GetDataTests
    {
        private SignalsWebService signalsWebService;


        [TestMethod]
        public void GivenNoSignals_GetData_WithSameTimeStamps_NoneQualityMvpFillsSingleDatum()
        {
            SetupWebService();
            var ts = new DateTime(2018, 12, 1);
            var returnedSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Month
            };


            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);
            missingValuePolicyRepositoryMock.Setup(m => m.Get(returnedSignal)).Returns(new NoneQualityMissingValuePolicyInteger());
            signalsDataRepositoryMock.Setup(s => s.GetData<int>(returnedSignal, ts, ts)).Returns(new List<Datum<int>>());

            var result = signalsWebService.GetData(1, ts, ts);
            var filledDatum = result.ElementAt(0);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Dto.Quality.None, filledDatum.Quality);
            Assert.AreEqual(0, filledDatum.Value);


        }



        private void SetupWebService()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();


    }
}
