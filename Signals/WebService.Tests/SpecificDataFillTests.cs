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
        public void GivenASecondSignal_WhenGettingData_SpecificGoodQualityPolicy_CorrectlyFillsMissingData()
        {
            SetupSpecificPolicy(Granularity.Second, Domain.Quality.Fair, 
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 0, 5), new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 1), Value = (double)2.5 }

                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 0, 5));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 1), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 2), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 3), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 4), Value = (double)42.42 },
            };

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenASecondSignal_WhenGettingData_SpecificFairQualityPolicy_CorrectlyFillsMissingData()
        {
            SetupSpecificPolicy(Granularity.Second, Domain.Quality.Fair,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 0, 5), new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 1), Value = (double)2.5 }

                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 0, 5));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 1), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 2), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 3), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 4), Value = (double)42.42 },
            };

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAMinuteSignal_WhenGettingData_SpecificGoodQualityPolicy_CorrectlyFillsMissingData()
        {
            SetupSpecificPolicy(Granularity.Minute, Domain.Quality.Good, 
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 5, 0), new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 1, 0), Value = (double)2.5 }

                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 5, 0));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 1, 0), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 2, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 3, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 4, 0), Value = (double)42.42 },
            };

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAMinuteSignal_WhenGettingData_SpecificFairQualityPolicy_CorrectlyFillsMissingData()
        {
            SetupSpecificPolicy(Granularity.Minute, Domain.Quality.Fair,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 5, 0), new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 1, 0), Value = (double)2.5 }

                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 5, 0));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 1, 0), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 2, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 3, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 4, 0), Value = (double)42.42 },
            };

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnHourSignal_WhenGettingData_SpecificGoodQualityPolicy_CorrectlyFillsMissingData()
        {
            SetupSpecificPolicy(Granularity.Hour, Domain.Quality.Good,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 5, 0, 0), new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0), Value = (double)2.5 }

                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 5, 0, 0));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 2, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 4, 0, 0), Value = (double)42.42 },
            };

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnHourSignal_WhenGettingData_SpecificFairQualityPolicy_CorrectlyFillsMissingData()
        {
            SetupSpecificPolicy(Granularity.Hour, Domain.Quality.Fair,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 5, 0, 0), new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0), Value = (double)2.5 }

                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 5, 0, 0));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 2, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 4, 0, 0), Value = (double)42.42 },
            };

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenADaySignal_WhenGettingData_SpecificGoodQualityPolicy_CorrectlyFillsMissingData()
        {
            SetupSpecificPolicy(Granularity.Day, Domain.Quality.Good,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 6, 0, 0, 0), new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0), Value = (double)2.5 }

                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 6, 0, 0, 0));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 4, 0, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 5, 0, 0, 0), Value = (double)42.42 },
            };

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenADaySignal_WhenGettingData_SpecificFairQualityPolicy_CorrectlyFillsMissingData()
        {
            SetupSpecificPolicy(Granularity.Day, Domain.Quality.Fair,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 6, 0, 0, 0), new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0), Value = (double)2.5 }

                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 6, 0, 0, 0));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 4, 0, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 5, 0, 0, 0), Value = (double)42.42 },
            };

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAWeekSignal_WhenGettingData_SpecificGoodQualityPolicy_CorrectlyFillsMissingData()
        {
            SetupSpecificPolicy(Granularity.Week, Domain.Quality.Good,
                new DateTime(2000, 1, 3, 0, 0, 0), new DateTime(2000, 2, 7, 0, 0, 0), new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 10, 0, 0, 0), Value = (double)2.5 }

                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 3, 0, 0, 0), new DateTime(2000, 2, 7, 0, 0, 0));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 10, 0, 0, 0), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 17, 0, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 24, 0, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 31, 0, 0, 0), Value = (double)42.42 },
            };

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAMonthSignal_WhenGettingData_SpecificGoodQualityPolicy_CorrectlyFillsMissingData()
        {
            SetupSpecificPolicy(Granularity.Month, Domain.Quality.Good,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 6, 1, 0, 0, 0), new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1, 0, 0, 0), Value = (double)2.5 }

                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 6, 1, 0, 0, 0));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1, 0, 0, 0), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1, 0, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1, 0, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1, 0, 0, 0), Value = (double)42.42 },
            };

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAYearSignal_WhenGettingData_SpecificGoodQualityPolicy_CorrectlyFillsMissingData()
        {
            SetupSpecificPolicy(Granularity.Year, Domain.Quality.Good,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2005, 1, 1, 0, 0, 0), new List<Datum<double>>()
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2001, 1, 1, 0, 0, 0), Value = (double)2.5 }

                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2005, 1, 1, 0, 0, 0));
            var expectedDatum = new List<Dto.Datum>()
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1, 0, 0, 0), Value = (double)2.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1, 0, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2003, 1, 1, 0, 0, 0), Value = (double)42.42 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2004, 1, 1, 0, 0, 0), Value = (double)42.42 },
            };

            AssertEqual(expectedDatum, result);
        }

        private void SetupSpecificPolicy(Granularity granularity, Domain.Quality SpecificPolicyQuality,
            DateTime fromIncluded, DateTime toExluded, List<Datum<double>> actualToBeReturnedByMockDatums)
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);

            int id = 1;

            var returnedSignal = new Signal() { Id = id, Granularity = granularity, DataType = DataType.Double };

            signalsRepoMock.Setup(sr => sr.Get(id)).Returns(returnedSignal);
            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                { Id = 1, Quality = SpecificPolicyQuality, Value = 42.42, Signal = returnedSignal, });

            dataRepoMock.Setup(d => d.GetData<double>(returnedSignal, fromIncluded, toExluded))
                .Returns(actualToBeReturnedByMockDatums);
        }

        private void AssertEqual(List<Dto.Datum> expectedDatums, IEnumerable<Dto.Datum> actualDatums)
        {
            int i = 0;

            Assert.AreEqual(5, actualDatums.Count());
            foreach (var actualData in actualDatums)
            {
                Assert.AreEqual(expectedDatums[i].Quality, actualData.Quality);
                Assert.AreEqual(expectedDatums[i].Timestamp, actualData.Timestamp);
                Assert.AreEqual(expectedDatums[i].Value, actualData.Value);

                i++;
            }
        }


        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> dataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();




    }
}
