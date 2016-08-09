using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Tests
{
    [TestClass]
    public class SignalDataTests
    {
        private SignalsWebService signalsWebService;


        [TestMethod]
        [ExpectedException(typeof(NoSuchSignalException))]
        public void SignalNotExists_GetData_ThrowsException()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns((Signal)null);

            var result = signalsWebService.GetData(1, new DateTime(), new DateTime());

        }

        [TestMethod]
        public void SignalHasNoData_GetData_NullIsReturned()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(new Signal() { Id = 1, DataType = DataType.Double });

            signalsDataRepoMock.Setup(sd => sd.GetData<double>(It.Is<Signal>(s => s.Id == 1),
                                                               It.IsAny<DateTime>(),
                                                               It.IsAny<DateTime>()))
                                                                .Returns((IEnumerable<Datum<double>>)null);


            var result = signalsWebService.GetData(1, new DateTime(2016, 1, 1), new DateTime(2016, 3, 1));

            Assert.IsNull(result);

        }

        [TestMethod]
        public void SignalHasData_GetData_DataIsReturned()
        {
            SetupWebService();
            IEnumerable<Datum<double>> resultData = new[] { new Domain.Datum<double>() { Quality = Quality.Fair,
                                                                 Timestamp = new DateTime(2000, 1, 1),
                                                                 Value = 1.2 },

                                                                 new Domain.Datum<double>() { Quality = Quality.Fair,
                                                                 Timestamp = new DateTime(2000, 2, 1),
                                                                 Value = 1.5 },

                                                                 new Domain.Datum<double>() { Quality = Quality.Fair,
                                                                 Timestamp = new DateTime(2000, 1, 1),
                                                                 Value = 2.4 }
                                                          };

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(new Signal() { Id = 1, DataType = DataType.Double });

            signalsDataRepoMock.Setup(sd => sd.GetData<double>(It.Is<Signal>(s => s.Id == 1 && s.DataType == DataType.Double),
                                                               It.IsAny<DateTime>(),
                                                               It.IsAny<DateTime>()))
                                                                .Returns(resultData);

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Dto.Datum>));
        }


        [TestMethod]
        [ExpectedException(typeof(NoSuchSignalException))]
        public void SignalNotExists_SetSignalData_ThrowsException()
        {
            SetupWebService();

            Mock<IEnumerable<Dto.Datum>> signalDataMock = new Mock<IEnumerable<Dto.Datum>>();

            signalsWebService.SetData(1, signalDataMock.Object);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullData_SetSignalData_ThrowsException()
        {
            SetupWebService(new Signal() { Id = 1 });
            signalsWebService.SetData(1, null);
        }


        [TestMethod]
        public void SignalExists_SetSignalData_WithNonNullData()
        {
            var returnedSignal = new Signal() { Id = 1, DataType = DataType.Integer };
            SetupWebService(returnedSignal);

            IEnumerable<Dto.Datum> dtoSignalData = new[] { new Dto.Datum() { Quality = Dto.Quality.Fair,
                                                                 Timestamp = new DateTime(2000, 1, 1),
                                                                 Value = 1 },

                                                           new Dto.Datum() { Quality = Dto.Quality.Good,
                                                                 Timestamp = new DateTime(2000, 2, 1),
                                                                 Value = 1 },

                                                           new Dto.Datum() { Quality = Dto.Quality.Poor,
                                                                 Timestamp = new DateTime(2000, 3, 1),
                                                                 Value = 2 } };

            signalsWebService.SetData(1, dtoSignalData);
            signalsDataRepoMock.Verify(sr => sr.SetData(It.IsAny<IEnumerable<Datum<int>>>()), Times.Once);


        }


        [TestMethod]
        public void WhenGettingSignalData_ReturnsItSortedByDate()
        {
            var datums = new[]
            {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new System.DateTime(2000, 2, 1), Value = 1.51 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new System.DateTime(2000, 1, 1), Value = 1.45 },
                    new Datum<double>() { Quality = Quality.Poor, Timestamp = new System.DateTime(2000, 3, 1), Value = 2.47 }
                };
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = Path.FromString("sfd/pk")
            };

            SetupWebService(signal);
            signalsDataRepoMock
                .Setup(sdr => sdr.GetData<double>(It.IsAny<Domain.Signal>(),
                It.IsAny<System.DateTime>(), It.IsAny<System.DateTime>()))
                .Returns(datums);

            var result = signalsWebService.GetData(1, System.DateTime.MinValue, System.DateTime.MaxValue);
            var datum = datums[1];
            datums[1] = datums[0];
            datums[0] = datum;

            int i = 0;
            foreach (var d in result)
            {
                Assert.AreEqual(datums[i++].Timestamp, d.Timestamp);
            }

        }
        
        [TestMethod]
        public void NoneQualityMissingValuePolicy_ShouldFillMissingData()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Month
            };
            var datums = new Datum<int>[]
            {
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 2 }
            };

            SetupWebService(signal);
            missingValueRepoMock
                .Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger());
            signalsDataRepoMock
                .Setup(dr => dr.GetData<int>(It.IsAny<Signal>(), It.IsAny<System.DateTime>(), It.IsAny<System.DateTime>()))
                .Returns(datums);

            var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
            foreach (var d in result)
                if (d.Timestamp == new DateTime(2000, 2, 1))
                    return;

            Assert.Fail();
        }

        private void SetupWebService(Signal signal=null)
        {
            signalsDataRepoMock = new Mock<ISignalsDataRepository>();
            signalsRepoMock = new Mock<ISignalsRepository>();
            missingValueRepoMock = new Mock<IMissingValuePolicyRepository>();
            SignalsDomainService domainService = new SignalsDomainService(
                signalsRepoMock.Object, signalsDataRepoMock.Object, missingValueRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);


            signalsRepoMock
                .Setup(sr => sr.Get(It.IsAny<int>()))
                .Returns(signal);
        }


        private Mock<ISignalsDataRepository> signalsDataRepoMock;
        private Mock<ISignalsRepository> signalsRepoMock;
        private Mock<IMissingValuePolicyRepository> missingValueRepoMock;

    }
}
