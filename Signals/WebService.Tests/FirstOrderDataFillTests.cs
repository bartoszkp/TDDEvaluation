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
    public class FirstOrderDataFillTests
    {

        private SignalsWebService signalsWebService;

        [TestMethod]
        public void GetData_FillsDecimalData_WithMonthGranularity()
        {
            SetupWebService();

            var returnedSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Decimal,
                Granularity = Granularity.Month
            };

            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<decimal>(returnedSignal,
                It.Is<DateTime>(d => d.Month == 6), 1)).Returns(new List<Datum<decimal>>()
                {
                    new Datum<decimal> { Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5m }
                });

            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<decimal>(returnedSignal,
                It.Is<DateTime>(d => d.Month == 6), 1)).Returns(new List<Datum<decimal>>()
                {
                    new Datum<decimal> { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m }
                });


            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            var genericInstance = new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDecimal();

            missingValuePolicyRepositoryMock.Setup(m => m.Get(returnedSignal)).Returns(genericInstance);

            var from = new DateTime(2000, 6, 1);
            var to = new DateTime(2000, 7, 1);

            signalsDataRepositoryMock.Setup(s => s.GetData<decimal>(returnedSignal, from, to))
               .Returns(new List<Datum<decimal>>()
               {
                   new Datum<decimal> { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1m },
                   new Datum<decimal> { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m },
                   new Datum<decimal> { Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5m }
               });

            var result = signalsWebService.GetData(1, from, to);
            var resultDatum = result.ElementAt(0);


            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(3m, resultDatum.Value);
            Assert.AreEqual(Dto.Quality.Fair, resultDatum.Quality);

        }

        [TestMethod]
        public void GetData_FillsIntegerDataWithDayGranularity()
        {
            SetupWebService();

            var returnedSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Day
            };

            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<int>(returnedSignal,
                It.Is<DateTime>(d => d.Day == 6), 1)).Returns(new List<Datum<int>>()
                {
                    new Datum<int> { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 8), Value = 5 }
                });

            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<int>(returnedSignal,
                It.Is<DateTime>(d => d.Day == 6), 1)).Returns(new List<Datum<int>>()
                {
                    new Datum<int> { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 5), Value = 2 }
                });


            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            var genericInstance = new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyInteger();

            missingValuePolicyRepositoryMock.Setup(m => m.Get(returnedSignal)).Returns(genericInstance);

            var from = new DateTime(2000, 5, 6);
            var to = new DateTime(2000, 5, 7);

            signalsDataRepositoryMock.Setup(s => s.GetData<int>(returnedSignal, from, to))
               .Returns(new List<Datum<int>>()
               {
                   new Datum<int> { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 1 },
                   new Datum<int> { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 5), Value = 2 },
                   new Datum<int> { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 8), Value = 5 }
               });

            var result = signalsWebService.GetData(1, from, to);
            var resultDatum = result.ElementAt(0);


            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(3, resultDatum.Value);
            Assert.AreEqual(Dto.Quality.Fair, resultDatum.Quality);
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
