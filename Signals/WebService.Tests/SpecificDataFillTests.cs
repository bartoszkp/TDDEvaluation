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
    public class SpecificDataFillTests
    {
        private SignalsWebService signalsWebService;

        [TestMethod]
        public void GetData_HelperIsFillingData()
        {
            SignalsDomainService domainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(domainService);

            var returnedSignal = new Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Month
            };

            signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns(returnedSignal);
            
            var genericInstance = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble();
            genericInstance.Value = 42.42;
            genericInstance.Signal = returnedSignal;
            genericInstance.Quality = Quality.Fair;

            missingValuePolicyRepositoryMock.Setup(m => m.Get(returnedSignal)).Returns(genericInstance);

            

            var from = new DateTime(2000, 1, 1);
            var to = new DateTime(2000, 4, 1);

            signalsDataRepositoryMock.Setup(s => s.GetData<double>(returnedSignal, from, to))
               .Returns(new List<Datum<double>>());


            var result = signalsWebService.GetData(1, from, to);

            var fetchedDatum = result.ElementAt(0);

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(Dto.Quality.Fair, fetchedDatum.Quality);
            Assert.AreEqual(42.42, fetchedDatum.Value);

        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
    }
}
