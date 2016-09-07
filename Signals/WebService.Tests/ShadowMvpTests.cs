using DataAccess.GenericInstantiations;
using Domain;
using Domain.Exceptions;
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
    public class ShadowMvpTests
    {
        private SignalsWebService signalsWebService;

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void SetMvp_WithNullSignalField_ExceptionIsThrown()
        {
            SetupWebService();
            var mvp = new Dto.MissingValuePolicy.ShadowMissingValuePolicy();
            var returnedSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Month
            };


            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);
            signalsWebService.SetMissingValuePolicy(1, mvp);
        }

        [TestMethod]
        [ExpectedException(typeof(ShadowSignalNotMatchingException))]
        public void SetMvp_NotMatchingDataTypesOrGranularities_ExceptionIsThrown()
        {
            SetupWebService();
            var mvp = new Dto.MissingValuePolicy.ShadowMissingValuePolicy();
            mvp.ShadowSignal = new Dto.Signal()
            {
                DataType = Dto.DataType.Decimal,
                Granularity = Dto.Granularity.Day
            };

            var returnedSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Month
            };


            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);
            signalsWebService.SetMissingValuePolicy(1, mvp);

        }

        [TestMethod]
        public void GetData_ShadowMvpFillsData()
        {
            SetupWebService();
            var returnedSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Decimal,
                Granularity = Granularity.Month
            };

            var shadowSignal = new Signal()
            {
                Id = 2,
                DataType = DataType.Decimal,
                Granularity = Granularity.Month
            };

            var from = new DateTime(1999, 11, 1);
            var to = new DateTime(2000, 11, 1);

            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);
            signalsRepositoryMock.Setup(sr => sr.Get(2)).Returns(shadowSignal);

            signalsDataRepositoryMock.Setup(sd => sd.GetData<decimal>(returnedSignal, from, to))
                .Returns(new List<Datum<decimal>>()
                {
                    new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1m },
                    new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m },
                    new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = 5m }
                });

            signalsDataRepositoryMock.Setup(sd => sd.GetData<decimal>(shadowSignal, from, to))
                .Returns(new List<Datum<decimal>>()
                {
                    new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = 1.4m },
                    new Datum<decimal>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 5, 1), Value = 0.0m },
                    new Datum<decimal>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 9, 1), Value = 7.0m }
                });

            var mvp = new ShadowMissingValuePolicyDecimal() { ShadowSignal = shadowSignal, Signal = returnedSignal, Id = 3 };
            
            missingValuePolicyRepositoryMock.Setup(s => s.Get(It.Is<Signal>(signal => signal == returnedSignal)))
           .Returns(mvp);


            var result = signalsWebService.GetData(1, from, to);
            var filledDatumForMarch = result.ElementAt(4);

            Assert.AreEqual(12, result.Count());
            Assert.AreEqual(Dto.Quality.Fair, filledDatumForMarch.Quality);
            Assert.AreEqual(1.4m, filledDatumForMarch.Value);
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
