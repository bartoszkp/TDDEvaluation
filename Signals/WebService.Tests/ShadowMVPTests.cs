using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.MissingValuePolicy;
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
    public class ShadowMVPTests
    {
        SignalsWebService signalsWebService;

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenSettingShadowMVP_WithNoMatchingDataType_ThrowsException()
        {
            var signal = new Domain.Signal()
            {
                Id = 243,
                DataType = Domain.DataType.String,
                Granularity = Domain.Granularity.Second
            };

            var shadowMVP = new ShadowMissingValuePolicy()
            {
                DataType = Dto.DataType.Integer,
                ShadowSignal = new Dto.Signal()
                {
                    Id = 5234,
                    DataType = Dto.DataType.Integer,
                    Granularity = Dto.Granularity.Second
                }
            };

            setupWebService();
            setupGet(signal);

            signalsWebService.SetMissingValuePolicy(signal.Id.Value, shadowMVP);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenSettingShadowMVP_WithNoMatchingGranularity_ThrowsException()
        {
            var signal = new Domain.Signal()
            {
                Id = 243,
                DataType = Domain.DataType.Boolean,
                Granularity = Domain.Granularity.Day
            };

            var shadowMVP = new ShadowMissingValuePolicy()
            {
                DataType = Dto.DataType.Boolean,
                ShadowSignal = new Dto.Signal()
                {
                    Id = 5234,
                    DataType = Dto.DataType.Boolean,
                    Granularity = Dto.Granularity.Year
                }
            };

            setupWebService();
            setupGet(signal);

            signalsWebService.SetMissingValuePolicy(signal.Id.Value, shadowMVP);
        }

        [TestMethod]
        public void ShadowMVPCorrectlyFillMissingData()
        {
            var signal = new Signal()
            {
                Id = 904,
                DataType = DataType.Decimal,
                Granularity = Granularity.Month,
            };

            var shadowSignal = new Signal()
            {
                Id = 2435,
                DataType = DataType.Decimal,
                Granularity = Granularity.Month,
            };

            var datums = new Datum<decimal>[]
            {
                new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1m },
                new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m },
                new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = 5m }
            };
            
            var shadowDatums = new Datum<decimal>[]
            {
                new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = 1.4m },
                new Datum<decimal>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 5, 1), Value = 0.0m },
                new Datum<decimal>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 9, 1), Value = 7.0m }
            };

            setupWebService();
            setupGet(signal);
            setupGet(shadowSignal);
            setupGetData(signal, datums);
            setupGetData(shadowSignal, shadowDatums);

            missingValuePolicyRepositoryMock
                .Setup(mvpr => mvpr.Get(It.Is<Signal>(s => s.Id == signal.Id)))
                .Returns(new DataAccess.GenericInstantiations.ShadowMissingValuePolicyDecimal() { ShadowSignal = shadowSignal });

            missingValuePolicyRepositoryMock
                .Setup(mvpr => mvpr.Get(It.Is<Signal>(s => s.Id == shadowSignal.Id)))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDecimal());

            var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 9, 1)).ToArray();

            var correctData = new Dto.Datum[]
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1m },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 2, 1), Value = default(decimal) },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = 1.4m },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 4, 1), Value = default(decimal) },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 6, 1), Value = default(decimal) },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 7, 1), Value = default(decimal) },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = 5m },
                new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 9, 1), Value = 7.0m }
            };

            for (int i=0; i<result.Length; ++i)
            {
                Assert.AreEqual(correctData[i].Quality, result[i].Quality);
                Assert.AreEqual(correctData[i].Timestamp, result[i].Timestamp);
                Assert.AreEqual(correctData[i].Value, result[i].Value);
            }
        }

        private void setupWebService()
        {
            var signalsDomainService = new SignalsDomainService(
                signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        private void setupGet(Domain.Signal signal)
        {
            signalsRepositoryMock
                .Setup(sr => sr.Get(signal.Id.Value))
                .Returns(signal);
        }

        private void setupGetData<T>(Signal signal, Datum<T>[] datums)
        {
            signalsDataRepositoryMock
                .Setup(sdr => sdr.GetData<T>(
                    It.Is<Signal>(s => s.Id == signal.Id),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()))
                .Returns(datums);
        }
        
        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
    }
}
