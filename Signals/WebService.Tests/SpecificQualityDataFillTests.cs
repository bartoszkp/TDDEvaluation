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
    public class SpecificQualityDataFillTests
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
                Granularity = Domain.Granularity.Month
            };

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(signal);
            Mock<SpecificValueMissingValuePolicy<int>> mvp = new Mock<SpecificValueMissingValuePolicy<int>>();

            mvpRepoMock.Setup(m => m.Get(signal)).Returns(mvp.Object);
            signalsDataRepoMock.Setup(s => s.GetData<int>(signal, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1)))
                .Returns(new List<Datum<int>>());

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));
            var fetchedDatumObject = result.ElementAt(0);
            
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Dto.Quality.Fair, fetchedDatumObject.Quality);

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
