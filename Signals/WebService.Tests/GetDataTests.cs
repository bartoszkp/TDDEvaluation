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
        public void IncorrectInterval_GetData_ReturnsEmptyCollection()
        {
            SignalsDomainService domainService = new SignalsDomainService(null, null, null);
            signalsWebService = new SignalsWebService(domainService);

            var result =  signalsWebService.GetData(1, new DateTime(2000, 3, 1), new DateTime(2000, 1, 1));

            Assert.AreEqual(0, result.Count());

        }

        [TestMethod]
        public void GetData_ReturnsDataSortedByDate()
        {
            SetupWebService();
            var datums = new Datum<bool>[] {
                new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2018, 2, 1), Value = false },
                new Datum<bool>() { Quality = Quality.Fair, Timestamp = new DateTime(2018, 1, 1), Value = true }
            };
            var signal = new Signal()
            {
                Id = 21,
                DataType = DataType.Boolean,
                Granularity = Granularity.Month
            };

            signalsRepositoryMock
                .Setup(sr => sr.Get(It.IsAny<int>()))
                .Returns(signal);
            missingValuePolicyRepositoryMock
                .Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyBoolean());
            signalsDataRepositoryMock
                .Setup(sdr => sdr.GetData<bool>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(datums);

            var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2018, 1, 1), new DateTime(2018, 3, 1));

            Assert.IsTrue(result.ElementAt(0).Timestamp < result.ElementAt(1).Timestamp);
        }

        private void SetupWebService()
        {
            SignalsDomainService domainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(domainService);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
    }
}
