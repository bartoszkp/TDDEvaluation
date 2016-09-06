using Domain;
using Domain.Exceptions;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
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
    public class FirstOrderMissingValuePolicyTests
    {
        SignalsWebService signalsWebService;

        [TestMethod]
        public void GivenASignalAndDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_FillsDataInsideRange()
        {
            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Second,
                Path = Domain.Path.FromString("root/signal1")
            };

            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 5), Value = (int)2 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 1, 8), Value = (int)5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 5),  Value = (int)2},
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 1, 6),  Value = (int)3 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 1, 7),  Value = (int)4 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 1, 8),  Value = (int)5 }
            };

            var olderDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 5), Value = 2 }
            };

            var newerDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 1, 8), Value = 5 }
            };

            var firstTimestamp = new DateTime(2000, 1, 1, 1, 1, 5);
            var lastTimestamp = new DateTime(2000, 1, 1, 1, 1, 9);

            signalsRepositoryMock = new Mock<ISignalsRepository>();

            GivenASignal(existingSignal);

            signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>);

            signalDataRepositoryMock
                .Setup(sdrm => sdrm.GetDataNewerThan<int>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), 1))
                .Returns(newerDatum);

            signalDataRepositoryMock
                .Setup(sdrm => sdrm.GetDataOlderThan<int>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), 1))
                .Returns(olderDatum);

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyInteger());

            var signalsDomainService = new SignalsDomainService(
                signalsRepositoryMock.Object,
                signalDataRepositoryMock.Object,
                missingValuePolicyRepositoryMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

            int index = 0;
            foreach (var fd in filledDatum)
            {
                Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality);
                Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp);
                Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                index++;
            }
        }

        private void GivenASignal(Signal signal)
        {
            //GivenNoSignals();

            signalsRepositoryMock
                .Setup(sr => sr.Get(signal.Id.Value))
                .Returns(signal);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock= new Mock<IMissingValuePolicyRepository>();

    }
}

