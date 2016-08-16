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
            SetupMocks(datums, signal);
            
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
            SetupMocks(datums, signal);

            var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
            foreach (var d in result)
                if (d.Timestamp == new DateTime(2000, 2, 1))
                    return;

            Assert.Fail();
        }

        [TestMethod]
        public void WhenFromIncludedUtcIsNotFirstDayOfYear_GetDataReturnedTrueData()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Year
            };
            var datums = new Datum<bool>[]{};
            SetupMocks(datums, signal);

            var result = signalsWebService.GetData(signal.Id.Value, new DateTime(1999, 5, 1), new DateTime(2000, 4, 1));
            foreach (var d in result)
                Assert.AreEqual(d.Timestamp, new DateTime(2000, 1, 1));
        }

        [TestMethod]
        public void WhenGettingDataForFromUtcSameAsToUtc_ReturnsSingleDatum()
        {
            var datums = new Datum<int>[]
            {
                new Datum<int>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = 1 },
            };
            var signal = new Domain.Signal()
            {
                Id = 1, DataType = DataType.Integer, Granularity = Granularity.Month, Path = Domain.Path.FromString("example/path"),
            };

            SetupMocks<int>(datums, signal);

            var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Dto.Quality.Bad, result.ElementAt(0).Quality);
            Assert.AreEqual(new DateTime(2000, 1, 1), result.ElementAt(0).Timestamp);
            Assert.AreEqual(1, result.ElementAt(0).Value);
        }

        [TestMethod]
        public void WhenGettingSignalsWithGivenPathPrefix_ReturnsAllSignals()
        {
            var domainSignalsToReturn = new Domain.Signal[]
            {
                new Signal() { Id = 1, DataType = DataType.Boolean, Granularity = Granularity.Year, Path = Domain.Path.FromString("root/signals1/signal1") },
                new Signal() { Id = 2, DataType = DataType.Boolean, Granularity = Granularity.Month, Path = Domain.Path.FromString("root/signals1/signal2") },
                new Signal() { Id = 3, DataType = DataType.Boolean, Granularity = Granularity.Month, Path = Domain.Path.FromString("root/signals1/signal3") },
                new Signal() { Id = 4, DataType = DataType.Decimal, Granularity = Granularity.Week, Path = Domain.Path.FromString("root/signals2/signal1") },
            };
            SetupMocksGetPath(domainSignalsToReturn);

            var pathDto = new Dto.Path() { Components = new[] { "root" } };
            var result = signalsWebService.GetPathEntry(pathDto);

            var dtoSignals = new Dto.Signal[]
            {
                new Dto.Signal() { Id = 1, DataType = Dto.DataType.Boolean, Granularity = Dto.Granularity.Year, Path =  new Dto.Path() { Components = new[] { "root", "signals1", "signal1" } } },
                new Dto.Signal() { Id = 2, DataType = Dto.DataType.Boolean, Granularity = Dto.Granularity.Month, Path = new Dto.Path() { Components = new[] { "root", "signals1", "signal2" } } },
                new Dto.Signal() { Id = 3, DataType = Dto.DataType.Boolean, Granularity = Dto.Granularity.Month, Path = new Dto.Path() { Components = new[] { "root", "signals1", "signal3" } } },
                new Dto.Signal() { Id = 4, DataType = Dto.DataType.Decimal, Granularity = Dto.Granularity.Week, Path = new Dto.Path() { Components = new[] { "root", "signals2", "signal1" } } },
            };
            var actualResultA = result.Signals.ToArray();

            int i = 0;
            foreach (var actualItem in actualResultA)
            {
                Assert.AreEqual(dtoSignals[i].DataType, actualItem.DataType);
                Assert.AreEqual(dtoSignals[i].Granularity, actualItem.Granularity);
                Assert.AreEqual(dtoSignals[i].Id, actualItem.Id);
                Assert.AreEqual(dtoSignals[i].Path.Components.ToString(), actualItem.Path.Components.ToString());

                i++;
            }
        }

        [TestMethod]
        public void WhenGettingWithGivenPathPrefix_CorrectlyReturnsSubPaths()
        {
            var domainSignalsToReturn = new Domain.Signal[]
            {
                new Signal() { Id = 1, DataType = DataType.Boolean, Granularity = Granularity.Year, Path = Domain.Path.FromString("root/signals1/signal1") },
                new Signal() { Id = 2, DataType = DataType.Boolean, Granularity = Granularity.Month, Path = Domain.Path.FromString("root/signals1/signal2") },
                new Signal() { Id = 3, DataType = DataType.Boolean, Granularity = Granularity.Month, Path = Domain.Path.FromString("root/signals1/signal3") },
                new Signal() { Id = 4, DataType = DataType.Decimal, Granularity = Granularity.Week, Path = Domain.Path.FromString("root/signals2/signal1") },
            };
            SetupMocksGetPath(domainSignalsToReturn);

            var pathDto = new Dto.Path() { Components = new[] { "root" } };
            var result = signalsWebService.GetPathEntry(pathDto);

            var expectedSubPaths = new Dto.Path[]
            {
                new Dto.Path() {Components = new[] {"root", "signals1"} },
                new Dto.Path() {Components = new[] {"root", "signals2"} },
            };

            var actualResultA = result.SubPaths.ToArray();

            int i = 0;
            foreach (var actualItem in actualResultA)
            {
                Assert.AreEqual(expectedSubPaths[i].Components.ToString(), actualItem.Components.ToString());

                i++;
            }
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

        private void SetupMocks<T>(Datum<T>[] datums, Signal signal)
        {
            SetupWebService(signal);
            missingValueRepoMock
                .Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger());
            signalsDataRepoMock
                .Setup(dr => dr.GetData<T>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(datums);
        }

        private void SetupMocksGetPath(IEnumerable<Signal> domainSignalsToReturn)
        {
            SetupWebService();
            signalsRepoMock
                .Setup(sdrm => sdrm.GetAllWithPathPrefix(It.IsAny<Domain.Path>()))
                .Returns(domainSignalsToReturn);
        }

        private Mock<ISignalsDataRepository> signalsDataRepoMock;
        private Mock<ISignalsRepository> signalsRepoMock;
        private Mock<IMissingValuePolicyRepository> missingValueRepoMock;

    }
}
