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
        public void GivenASignal_GetData_FillsMissingData()
        {
            SetupWebService();

            int id = 1;

            var returnedSignal = new Signal() { Id = id, Granularity = Granularity.Month, DataType = DataType.Double };

            Mock<ZeroOrderMissingValuePolicy<double>> zeroOrderMvpMock = new Mock<ZeroOrderMissingValuePolicy<double>>();

            signalsRepoMock.Setup(sr => sr.Get(id)).Returns(returnedSignal);
            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(zeroOrderMvpMock.Object);

            dataRepoMock.Setup(d => d.GetData<double>(returnedSignal, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)))
                .Returns(new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }

                });


            var result = signalsWebService.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

            var filledDatum = result.ElementAt(1);

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(Dto.Quality.Good, filledDatum.Quality);
            Assert.AreEqual(1.5, filledDatum.Value);
            

        }

        [TestMethod]
        public void NoPreviousDatum_DefaultDatumInserted()
        {
            SetupWebService();
            int id = 1;

            var returnedSignal = new Signal() { Id = id, Granularity = Granularity.Month, DataType = DataType.Double };

            Mock<ZeroOrderMissingValuePolicy<double>> zeroOrderMvpMock = new Mock<ZeroOrderMissingValuePolicy<double>>();

            signalsRepoMock.Setup(sr => sr.Get(id)).Returns(returnedSignal);
            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(zeroOrderMvpMock.Object);

            dataRepoMock.Setup(d => d.GetData<double>(returnedSignal, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)))
                .Returns(new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
                });


            var result = signalsWebService.GetData(id, new DateTime(2000, 2, 1), new DateTime(2000, 4, 1));

            var filledDatum = result.ElementAt(0);

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(Dto.Quality.None, filledDatum.Quality);
            Assert.AreEqual((double)0, filledDatum.Value);


        }

        private void SetupWebService()
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);
        }


        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> dataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

    }
}
