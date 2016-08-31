using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using System.Collections.Generic;
using Domain.Services.Implementation;
using Moq;
using Domain.Repositories;
using System.Linq;

namespace WebService.Tests
{
    [TestClass]
    public class FirstOrderFillTests
    {
        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> dataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

        private SignalsWebService signalsWebService;

        [TestMethod]
        public void GivenASecondSignal_WhenGettingDataWithCorrectRange_FirstOrderGoodQualityPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Second, Domain.Quality.Fair,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 0, 13), new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 5), Value = (double)11 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 9), Value = (double)5 },
                    new Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 0, 12), Value = (double)20 }
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 0, 13));

            var expectedDatum = GetExpectedDatums();

            AssertEqual(expectedDatum, result);
        }


        private void SetupFirstOrderPolicy(Granularity granularity, Domain.Quality FirstOrderPolicyQuality,
            DateTime fromIncluded, DateTime toExluded, List<Datum<double>> actualToBeReturnedByMockDatums)
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);

            var returnedSignal = new Signal() { Id = 1, Granularity = granularity, DataType = DataType.Double };

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);
            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDouble()
                { Id = 1, Signal = returnedSignal });

            dataRepoMock.Setup(d => d.GetData<double>(returnedSignal, fromIncluded, toExluded))
                .Returns(actualToBeReturnedByMockDatums);

            SetupGetDataOlderAndNewerThanForSecondSignal(returnedSignal);
        }

        private void AssertEqual(List<Dto.Datum> expectedDatums, IEnumerable<Dto.Datum> actualDatums)
        {
            int i = 0;

            Assert.AreEqual(13, actualDatums.Count());
            foreach (var actualData in actualDatums)
            {
                Assert.AreEqual(expectedDatums[i].Quality, actualData.Quality);
                Assert.AreEqual(expectedDatums[i].Timestamp, actualData.Timestamp);
                Assert.AreEqual(expectedDatums[i].Value, actualData.Value);

                i++;
            }
        }

        private List<Dto.Datum> GetExpectedDatums()
        {
            return new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 1), Value = (double)3 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 2), Value = (double)5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 3), Value = (double)7 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 4), Value = (double)9 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 5), Value = (double)11 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 6), Value = (double)9.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 7), Value = (double)8 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 8), Value = (double)6.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 9), Value = (double)5 },
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 0, 10), Value = (double)10 },
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 0, 11), Value = (double)15 },
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 0, 12), Value = (double)20 },
            };
        }

        private void SetupGetDataOlderAndNewerThanForSecondSignal(Signal returnedSignal)
        {
            var firstTimestamp = new DateTime(2000, 1, 1, 0, 0, 1);
            var secondTimestamp = new DateTime(2000, 1, 1, 0, 0, 5);
            var thirdTimestamp = new DateTime(2000, 1, 1, 0, 0, 9);
            var fourthTimestamp = new DateTime(2000, 1, 1, 0, 0, 12);

            dataRepoMock
                .Setup(d => d.GetDataOlderThan<double>(returnedSignal, firstTimestamp, 1))
                .Returns(new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = firstTimestamp.AddSeconds(-1), Value = (double)1 }
                });

            dataRepoMock
                .Setup(d => d.GetDataNewerThan<double>(returnedSignal, firstTimestamp, 1))
                .Returns(new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestamp, Value = (double)11 }
                });


            dataRepoMock
                .Setup(d => d.GetDataOlderThan<double>(returnedSignal, secondTimestamp.AddSeconds(1), 1))
                .Returns(new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestamp, Value = (double)11 }
                });

            dataRepoMock
                .Setup(d => d.GetDataNewerThan<double>(returnedSignal, secondTimestamp.AddSeconds(1), 1))
                .Returns(new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestamp, Value = (double)5 }
                });


            dataRepoMock
                .Setup(d => d.GetDataOlderThan<double>(returnedSignal, thirdTimestamp.AddSeconds(1), 1))
                .Returns(new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestamp, Value = (double)5 }
                });

            dataRepoMock
                .Setup(d => d.GetDataNewerThan<double>(returnedSignal, thirdTimestamp.AddSeconds(1), 1))
                .Returns(new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestamp, Value = (double)20 }
                });


            dataRepoMock
                .Setup(d => d.GetDataOlderThan<double>(returnedSignal, fourthTimestamp.AddSeconds(1), 1))
                .Returns(new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestamp, Value = (double)20 }
                });

            dataRepoMock
                .Setup(d => d.GetDataNewerThan<double>(returnedSignal, fourthTimestamp.AddSeconds(1), 1))
                .Returns(new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.None, Signal = returnedSignal, Timestamp = fourthTimestamp.AddSeconds(1), Value = default(double) }
                });
        }
    }
}
