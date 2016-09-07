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
    public class ShadowDataFillTests
    {
        private SignalsWebService signalsWebService;

        [TestMethod]
        [ExpectedException(typeof(ShadowSignalNotCorrectlyException))]
        public void WhenShadowMVPHaveSignalWithIncorrectDataType_ThrowExcpetion()
        {
            var signal = new Signal()
            {
                Id = 14,
                DataType = DataType.Decimal,
                Granularity = Granularity.Minute
            };
            SetupMocks(signal);

            var mvp = new Dto.MissingValuePolicy.ShadowMissingValuePolicy()
            {
                DataType = Dto.DataType.Double,
                ShadowSignal = new Dto.Signal()
                {
                    DataType = Dto.DataType.Double,
                    Granularity = Dto.Granularity.Minute
                }
            };

            signalsWebService.SetMissingValuePolicy(signal.Id.Value, mvp);
        }

        [TestMethod]
        public void WhenShadowMVPHaveCorrectSignal_CallsMVPRepository()
        {
            var signal = new Signal()
            {
                Id = 76,
                DataType = DataType.Integer,
                Granularity = Granularity.Hour
            };
            SetupMocks(signal);

            var mvp = new Dto.MissingValuePolicy.ShadowMissingValuePolicy()
            {
                DataType = Dto.DataType.Integer,
                ShadowSignal = new Dto.Signal()
                {
                    DataType = Dto.DataType.Integer,
                    Granularity = Dto.Granularity.Hour
                }
            };

            signalsWebService.SetMissingValuePolicy(signal.Id.Value, mvp);

            mvpMock.Verify(mvpM => mvpM.Set(It.Is<Signal>(s => s == signal), It.Is<MissingValuePolicyBase>(mvpb => mvpb.GetType() == typeof(ShadowMissingValuePolicy<int>))));
        }

        [TestMethod]
        public void ShadowMVPCorrectlyFillMissingData()
        {
            var signal = new Signal()
            {
                Id = 243,
                DataType = DataType.Decimal,
                Granularity = Granularity.Month,
                Path = Path.FromString("asdf")
            };
            var shadow = new Signal()
            {
                Id = 543,
                DataType = DataType.Decimal,
                Granularity = Granularity.Month,
                Path = Path.FromString("kpdsfh")
            };

            var data = new Datum<decimal>[]
            {
                new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1m },
                new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m },
                new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = 5m }
            };
            var shadowData = new Datum<decimal>[]
            {
                new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = 1.4m },
                new Datum<decimal>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 5, 1), Value = 0.0m },
                new Datum<decimal>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 9, 1), Value = 7.0m }
            };

            SetupMocks(signal);
            setupGet(shadow);
            setupGetData(signal, data);
            setupGetData(shadow, shadowData);
            mvpMock.Setup(mvpm => mvpm.Get(It.Is<Signal>(s => s.Id == signal.Id)))
                .Returns(new DataAccess.GenericInstantiations.ShadowMissingValuePolicyDecimal() { ShadowSignal = shadow, Signal = signal, Id = 786 });
            mvpMock.Setup(mvpm => mvpm.Get(It.Is<Signal>(s => s.Id == shadow.Id)))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDecimal());

            var result = signalsWebService.GetData(signal.Id.Value, new DateTime(1999, 11, 1), new DateTime(2000, 11, 1));

            var correctData = new Dto.Datum[]
            {
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(1999, 11, 1), Value = 0m },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(1999, 12, 1), Value = 0m },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1m },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 2, 1), Value = 0m },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = 1.4m },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 4, 1), Value = 0m },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 6, 1), Value = 0m },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 7, 1), Value = 0m },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = 5m },
                new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 9, 1), Value = 7.0m },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 10, 1), Value = 0m }
            };

            int i = 0;
            foreach (var d in result)
            {
                Assert.AreEqual(correctData[i].Quality, d.Quality);
                Assert.AreEqual(correctData[i].Timestamp, d.Timestamp);
                Assert.AreEqual(correctData[i].Value, d.Value);
                ++i;
            }

        }

        private void SetupMocks(Signal signal, int? signalId = null)
        {
            var signalsDomainService = new SignalsDomainService(
                signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);

            setupGet(signal, signalId);
        }

        private void setupGet(Signal signal, int? signalId = null)
        {
            if (signalId == null)
                signalId = signal.Id;

            signalsRepositoryMock
                .Setup(sr => sr.Get(It.Is<int>(id => id == signalId)))
                .Returns(signal);
        }

        private void setupGetData<T>(Signal signal, Datum<T>[] data)
        {
            signalsDataRepositoryMock
                .Setup(sdr => sdr.GetData<T>(It.Is<Signal>(s => s.Id == signal.Id),
                    It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(data);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<IMissingValuePolicyRepository> mvpMock = new Mock<IMissingValuePolicyRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
    }
}
