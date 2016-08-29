using Domain.Repositories;
using Domain.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.MissingValuePolicy;

namespace WebService.Tests
{
    [TestClass]
    public class SpecificDataFillTests
    {

        private SignalsWebService signalsWebService;

        [TestMethod]
        public void GivenASignal_GetData_FillsMissingData()
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);

            int id = 1;

            var returnedSignal = new Signal() { Id = id, Granularity = Granularity.Month, DataType = DataType.Double };

            Mock<SpecificValueMissingValuePolicy<double>> specificMvpMock = new Mock<SpecificValueMissingValuePolicy<double>>();
            specificMvpMock.Object.Value = 42.42;
            specificMvpMock.Object.Quality = Quality.Fair;

            signalsRepoMock.Setup(sr => sr.Get(id)).Returns(returnedSignal);
            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(specificMvpMock.Object);

            dataRepoMock.Setup(d => d.GetData<double>(returnedSignal, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)))
                .Returns(new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1,0,0,0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 3, 1,0,0,0), Value = (double)2.5 }

                });


            var result = signalsWebService.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

            var filledDatum = result.ElementAt(1);

            Assert.AreEqual(3, result.Count());
            Assert.AreEqual(specificMvpMock.Object.Value, filledDatum.Value);


        }

        [TestMethod]
        public void GivenASignal_WhenGettingData_SpecificGoodQualityPolicy_CorrectlyFillsMissingSecondData()
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);

            int id = 1;

            var returnedSignal = new Signal() { Id = id, Granularity = Granularity.Second, DataType = DataType.Double };

            signalsRepoMock.Setup(sr => sr.Get(id)).Returns(returnedSignal);
            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                { Id = 1, Quality = Quality.Good, Value = 42.42, Signal = returnedSignal, });

            dataRepoMock.Setup(d => d.GetData<double>(returnedSignal, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 0, 5)))
                .Returns(new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 1), Value = (double)2.5 }
                });


            var result = signalsWebService.GetData(id, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 0, 5));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 1), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 2), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 3), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 4), Value = (double)42.42 },
            };

            int i = 0;

            Assert.AreEqual(5, result.Count());
            foreach (var actualData in result)
            {
                Assert.AreEqual(expectedDatum[i].Quality, actualData.Quality);
                Assert.AreEqual(expectedDatum[i].Timestamp, actualData.Timestamp);
                Assert.AreEqual(expectedDatum[i].Value, actualData.Value);

                i++;
            }
        }


        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> dataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();




    }
}
