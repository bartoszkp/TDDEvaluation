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

        [TestMethod]
        public void GivenASecondSignal_WhenGettingDataFromMoreThanOneStepOlder_WithZeroPolicy_ItCorrectlyFillsMissingData()
        {
            SetupMockRepositories(Granularity.Second, 1, new DateTime(2000, 1, 1, 0, 0, 1), new DateTime(2000, 1, 1, 0, 0, 5),
                new List<Datum<double>>() { new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 1), Value = 2.5 } },
                new DateTime(2000, 1, 1, 0, 0, 1));
            
            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 1), new DateTime(2000, 1, 1, 0, 0, 5));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 1), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 2), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 3), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 4), Value = (double)2.5 },
            };

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAMinuteSignal_WhenGettingDataFromMoreThanOneStepOlder_WithZeroPolicy_ItCorrectlyFillsMissingData()
        {
            SetupMockRepositories(Granularity.Minute, 1, new DateTime(2000, 1, 1, 0, 1, 0), new DateTime(2000, 1, 1, 0, 5, 0),
                new List<Datum<double>>() { new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 1, 0), Value = 2.5 } },
                new DateTime(2000, 1, 1, 0, 1, 0));

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 1, 0), new DateTime(2000, 1, 1, 0, 5, 0));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 1, 0), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 2, 0), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 3, 0), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 4, 0), Value = (double)2.5 },
            };

            AssertEqual(expectedDatum, result);
        }

        private void SetupMockRepositories(Granularity granularity, int maxSampleCount,
            DateTime fromIncluded, DateTime toExluded, 
            List<Datum<double>> actualDatumsToBeReturnedByMockGetDataOlderThan, DateTime dateTimeForGettingOlderThan)
        {
            SetupWebService();
            var returnedSignal = new Signal() { Id = 1, Granularity = granularity, DataType = DataType.Double };

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);
            mvpRepoMock
                .Setup(mvp => mvp.Get(returnedSignal))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble()
                {
                    Id = 1,
                    Signal = returnedSignal
                });
            dataRepoMock
                .Setup(drm => drm.GetData<double>(returnedSignal, fromIncluded, toExluded))
                .Returns(new List<Datum<double>>());
            dataRepoMock
                .Setup(drm => drm.GetDataOlderThan<double>(returnedSignal, dateTimeForGettingOlderThan, maxSampleCount))
                .Returns(actualDatumsToBeReturnedByMockGetDataOlderThan);
        }

        private void AssertEqual(List<Dto.Datum> expectedDatums, IEnumerable<Dto.Datum> actualDatums)
        {
            int i = 0;

            Assert.AreEqual(4, actualDatums.Count());
            foreach (var actualData in actualDatums)
            {
                Assert.AreEqual(expectedDatums[i].Quality, actualData.Quality);
                Assert.AreEqual(expectedDatums[i].Timestamp, actualData.Timestamp);
                Assert.AreEqual(expectedDatums[i].Value, actualData.Value);

                i++;
            }
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
