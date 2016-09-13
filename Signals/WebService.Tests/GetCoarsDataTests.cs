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
    public class GetCoarsDataTests
    {
        private SignalsWebService signalsWebService;

        [TestMethod]
        [ExpectedException(typeof(NoSuchSignalException))]
        public void GivenASignal_WhenGettingCoarseDataByWrongId_ExceptionIsThrown()
        {
            GivenASignal(new Signal() { Id = 1, DataType = DataType.Decimal });

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);

            signalsWebService = new SignalsWebService(signalsDomainService);

            signalsWebService.GetCoarseData(2, Dto.Granularity.Day, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));
        }

        [TestMethod]
        public void GivenASignalAndDatum_WhenFromTimestampIsGreaterThanToTimestamp_ReturnsEmptyResult()
        {
            var existingSignal = new Signal() { Id = 1, DataType = DataType.Double, Granularity = Granularity.Month };

            GivenASignal(existingSignal);

            mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

            mvpRepoMock
                .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());

            var existingDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 }
            };

            var coarseDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.None, Timestamp = new DateTime(2001, 1, 1), Value = (double)0 }
            };

            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(existingDatum.ToDomain<IEnumerable<Datum<double>>>());

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpRepoMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var result = signalsWebService.GetCoarseData(1, Dto.Granularity.Year, new DateTime(2001, 1, 1), new DateTime(2000, 1, 1));

            Assert.AreEqual(coarseDatum.First().Quality, result.First().Quality);
            Assert.AreEqual(coarseDatum.First().Timestamp, result.First().Timestamp);
            Assert.AreEqual(coarseDatum.First().Value, result.First().Value);
        }

        [TestMethod]
        public void GivenASignalAndDatum_WhenGettingCoarseDataAndFromTimestampIsEqualToToTimestamp_SingleDatumIsReturned()
        {
            var existingSignal = new Signal() { Id = 1, DataType = DataType.Double, Granularity = Granularity.Month };

            GivenASignal(existingSignal);

            mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

            mvpRepoMock
                .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());

            var existingDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)1.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = (double)1.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = (double)1.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 6, 1), Value = (double)1.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 7, 1), Value = (double)1.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = (double)1.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 9, 1), Value = (double)1.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 10, 1), Value = (double)1.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 11, 1), Value = (double)1.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 12, 1), Value = (double)1.5 },
            };

            var coarseDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 }
            };

            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(existingDatum.ToDomain<IEnumerable<Datum<double>>>());

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpRepoMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var result = signalsWebService.GetCoarseData(1, Dto.Granularity.Year, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

            Assert.AreEqual(coarseDatum.First().Quality, result.First().Quality);
            Assert.AreEqual(coarseDatum.First().Timestamp, result.First().Timestamp);
            Assert.AreEqual(coarseDatum.First().Value, result.First().Value);
        }

        [TestMethod]
        public void GivenADoubleSignalAndDatum_WhenGettingCoarseData_CorrectDatumIsReturned()
        {
            var existingSignal = new Signal() { Id = 1, DataType = DataType.Double, Granularity = Granularity.Month };

            GivenASignal(existingSignal);

            mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

            mvpRepoMock
                .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());

            var firstPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)2 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = (double)3 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = (double)3.5 },
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 6, 1), Value = (double)4 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 7, 1), Value = (double)4.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = (double)5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 9, 1), Value = (double)5.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 10, 1), Value = (double)6 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 11, 1), Value = (double)6.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 12, 1), Value = (double)7 }
            };
            var secondPartDatum = new Dto.Datum[] 
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1), Value = (double)8.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 2, 1), Value = (double)9 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 3, 1), Value = (double)9.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 4, 1), Value = (double)10 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 5, 1), Value = (double)10.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 6, 1), Value = (double)11 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 7, 1), Value = (double)11.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 8, 1), Value = (double)12 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 9, 1), Value = (double)12.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 10, 1), Value = (double)13 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 11, 1), Value = (double)13.5 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 12, 1), Value = (double)14 }
            };

            var coarseDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = (double)4.25 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1), Value = (double)11.25 }
            };

            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), new DateTime(2000, 1, 1), new DateTime(2001, 1, 1)))
                .Returns(firstPartDatum.ToDomain<IEnumerable<Datum<double>>>());

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), new DateTime(2001, 1, 1), new DateTime(2002, 1, 1)))
                .Returns(secondPartDatum.ToDomain<IEnumerable<Datum<double>>>());


            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpRepoMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var result = signalsWebService.GetCoarseData(1, Dto.Granularity.Year, new DateTime(2000, 1, 1), new DateTime(2002, 1, 1));

            AssertDatum(result, coarseDatum);
        }

        [TestMethod]
        public void GivenAIntegerSignalAndDatum_WhenGettingCoarseData_CorrectDatumIsReturned()
        {
            var existingSignal = new Signal() { Id = 1, DataType = DataType.Integer, Granularity = Granularity.Month };

            GivenASignal(existingSignal);

            mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

            mvpRepoMock
                .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger());

            var firstPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = 2 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 3},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 4},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 5},
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 6, 1), Value = 6},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 7, 1), Value = 7},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = 8},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 9, 1), Value = 9},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 10, 1), Value = 10},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 11, 1), Value = 11},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 12, 1), Value = 12}
            };
            var secondPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1), Value = 13},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 2, 1), Value = 14 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 3, 1), Value = 15},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 4, 1), Value = 16},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 5, 1), Value = 17},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 6, 1), Value = 18},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 7, 1), Value = 19},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 8, 1), Value = 20},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 9, 1), Value = 21},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 10, 1), Value = 22},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 11, 1), Value = 23},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 12, 1), Value = 24}
            };

            var coarseDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = 6 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1), Value = 18 }
            };

            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), new DateTime(2000, 1, 1), new DateTime(2001, 1, 1)))
                .Returns(firstPartDatum.ToDomain<IEnumerable<Datum<int>>>());

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), new DateTime(2001, 1, 1), new DateTime(2002, 1, 1)))
                .Returns(secondPartDatum.ToDomain<IEnumerable<Datum<int>>>());


            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpRepoMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var result = signalsWebService.GetCoarseData(1, Dto.Granularity.Year, new DateTime(2000, 1, 1), new DateTime(2002, 1, 1));

            AssertDatum(result, coarseDatum);
        }

        [TestMethod]
        public void GivenADecimalSignalAndDatum_WhenGettingCoarseData_CorrectDatumIsReturned()
        {
            var existingSignal = new Signal() { Id = 1, DataType = DataType.Decimal, Granularity = Granularity.Month };

            GivenASignal(existingSignal);

            mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

            mvpRepoMock
                .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());

            var firstPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1.5m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = 2m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 2.5m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 3m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 3.5m },
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 6, 1), Value = 4m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 7, 1), Value = 4.5m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = 5m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 9, 1), Value = 5.5m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 10, 1), Value = 6m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 11, 1), Value = 6.5m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 12, 1), Value = 7m }
            };
            var secondPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1), Value = 8.5m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 2, 1), Value = 9m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 3, 1), Value = 9.5m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 4, 1), Value = 10m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 5, 1), Value = 10.5m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 6, 1), Value = 11m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 7, 1), Value = 11.5m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 8, 1), Value = 12m},
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 9, 1), Value = 12.5m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 10, 1), Value = 13m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 11, 1), Value = 13.5m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 12, 1), Value = 14m }
            };

            var coarseDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = 4.25m },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1), Value = 11.25m }
            };

            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<decimal>(It.IsAny<Domain.Signal>(), new DateTime(2000, 1, 1), new DateTime(2001, 1, 1)))
                .Returns(firstPartDatum.ToDomain<IEnumerable<Datum<decimal>>>());

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<decimal>(It.IsAny<Domain.Signal>(), new DateTime(2001, 1, 1), new DateTime(2002, 1, 1)))
                .Returns(secondPartDatum.ToDomain<IEnumerable<Datum<decimal>>>());


            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpRepoMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var result = signalsWebService.GetCoarseData(1, Dto.Granularity.Year, new DateTime(2000, 1, 1), new DateTime(2002, 1, 1));

            AssertDatum(result, coarseDatum);
        }

        [TestMethod]
        public void GivenASignalAndSecondDatum_WhenGettingCoarseData_CorrectDatumIsReturned()
        {
            var existingSignal = new Signal() { Id = 1, DataType = DataType.Integer, Granularity = Granularity.Second };

            GivenASignal(existingSignal);

            mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

            mvpRepoMock
                .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger());

            var firstPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1,1,1,0), Value = 1 },
                
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,1,1,59), Value = 1}
            };
            var secondPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,1,2,0), Value = 2},
                
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,1,2,59), Value = 2}
            };

            var coarseDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1,1,1,0), Value = 1 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,1,2,0), Value = 2 }
            };

            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), new DateTime(2000, 1, 1,1,1,0), new DateTime(2000, 1, 1,1,2,0)))
                .Returns(firstPartDatum.ToDomain<IEnumerable<Datum<int>>>());

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), new DateTime(2000, 1, 1,1,2,0), new DateTime(2000, 1, 1,1,3,0)))
                .Returns(secondPartDatum.ToDomain<IEnumerable<Datum<int>>>());
            
            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpRepoMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var result = signalsWebService.GetCoarseData(1, Dto.Granularity.Minute, new DateTime(2000, 1, 1,1,1,0), new DateTime(2000, 1, 1,1,3,0));

            AssertDatum(result, coarseDatum);
        }

        [TestMethod]
        public void GivenASignalAndMinuteDatum_WhenGettingCoarseData_CorrectDatumIsReturned()
        {
            var existingSignal = new Signal() { Id = 1, DataType = DataType.Integer, Granularity = Granularity.Minute };

            GivenASignal(existingSignal);

            mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

            mvpRepoMock
                .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger());

            var firstPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1,1,0,0), Value = 1 },

                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,1,59,0), Value = 1}
            };
            var secondPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,2,0,0), Value = 2},

                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,2,59,0), Value = 2}
            };

            var coarseDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1,1,0,0), Value = 1 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,2,0,0), Value = 2 }
            };

            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), new DateTime(2000, 1, 1, 1, 0, 0), new DateTime(2000, 1, 1, 2, 0, 0)))
                .Returns(firstPartDatum.ToDomain<IEnumerable<Datum<int>>>());

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), new DateTime(2000, 1, 1, 2, 0, 0), new DateTime(2000, 1, 1, 3, 0, 0)))
                .Returns(secondPartDatum.ToDomain<IEnumerable<Datum<int>>>());

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpRepoMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var result = signalsWebService.GetCoarseData(1, Dto.Granularity.Hour, new DateTime(2000, 1, 1, 1, 0, 0), new DateTime(2000, 1, 1, 3, 0, 0));

            AssertDatum(result, coarseDatum);
        }

        [TestMethod]
        public void GivenASignalAndHourDatum_WhenGettingCoarseData_CorrectDatumIsReturned()
        {
            var existingSignal = new Signal() { Id = 1, DataType = DataType.Integer, Granularity = Granularity.Hour };

            GivenASignal(existingSignal);

            mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

            mvpRepoMock
                .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger());

            var firstPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1,0,0,0), Value = 1 },

                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,23,0,0), Value = 1}
            };
            var secondPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2,0,0,0), Value = 2},

                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2,23,0,0), Value = 2}
            };

            var coarseDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1,0,0,0), Value = 1 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2,0,0,0), Value = 2 }
            };

            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 2, 0, 0, 0)))
                .Returns(firstPartDatum.ToDomain<IEnumerable<Datum<int>>>());

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), new DateTime(2000, 1, 2, 0, 0, 0), new DateTime(2000, 1, 3, 0, 0, 0)))
                .Returns(secondPartDatum.ToDomain<IEnumerable<Datum<int>>>());

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpRepoMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var result = signalsWebService.GetCoarseData(1, Dto.Granularity.Day, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 3, 0,0 , 0));

            AssertDatum(result, coarseDatum);
        }

        [TestMethod]
        public void GivenASignalAndDayDatum_WhenGettingWeeklyCoarseData_CorrectDatumIsReturned()
        {
            var existingSignal = new Signal() { Id = 1, DataType = DataType.Integer, Granularity = Granularity.Day };

            GivenASignal(existingSignal);

            mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

            mvpRepoMock
                .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger());

            var firstPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2016, 1, 4), Value = 1 },

                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 1, 10), Value = 1}
            };
            var secondPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 1, 11), Value = 2},

                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 1, 17), Value = 2}
            };

            var coarseDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2016, 1, 4), Value = 1 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 1, 11), Value = 2 }
            };

            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), new DateTime(2016, 1, 4), new DateTime(2016, 1, 11)))
                .Returns(firstPartDatum.ToDomain<IEnumerable<Datum<int>>>());

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), new DateTime(2016, 1, 11), new DateTime(2016, 1, 18)))
                .Returns(secondPartDatum.ToDomain<IEnumerable<Datum<int>>>());

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpRepoMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var result = signalsWebService.GetCoarseData(1, Dto.Granularity.Week, new DateTime(2016, 1, 4), new DateTime(2016, 1, 25));

            AssertDatum(result, coarseDatum);
        }

        [TestMethod]
        public void GivenASignalAndDayDatum_WhenGettingMonthlyCoarseData_CorrectDatumIsReturned()
        {
            var existingSignal = new Signal() { Id = 1, DataType = DataType.Integer, Granularity = Granularity.Day };

            GivenASignal(existingSignal);

            mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

            mvpRepoMock
                .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger());

            var firstPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2016, 1, 1), Value = 1 },

                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 1, 30), Value = 1}
            };
            var secondPartDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 2, 1), Value = 2},

                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 2, 29), Value = 2}
            };

            var coarseDatum = new Dto.Datum[]
            {
                new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2016, 1, 1), Value = 1 },
                new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 2, 1), Value = 2 }
            };

            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), new DateTime(2016, 1, 1), new DateTime(2016, 2, 1)))
                .Returns(firstPartDatum.ToDomain<IEnumerable<Datum<int>>>());

            signalsDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.IsAny<Domain.Signal>(), new DateTime(2016, 2, 1), new DateTime(2016, 3, 1)))
                .Returns(secondPartDatum.ToDomain<IEnumerable<Datum<int>>>());

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpRepoMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var result = signalsWebService.GetCoarseData(1, Dto.Granularity.Month, new DateTime(2016, 1, 1), new DateTime(2016, 3, 1));

            AssertDatum(result, coarseDatum);
        }

        private void GivenNoSignals()
        {
            signalsRepositoryMock = new Mock<ISignalsRepository>();
            signalsRepositoryMock
                .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                .Returns<Domain.Signal>(s => s);
            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        private void GivenASignal(Domain.Signal existingSignal)
        {
            GivenNoSignals();
            signalsRepositoryMock
                .Setup(sr => sr.Get(existingSignal.Id.Value))
                .Returns(existingSignal);
        }

        private void AssertDatum(IEnumerable<Dto.Datum> result, Dto.Datum[] filledDatum)
        {
            int index = 0;
            foreach (var fd in filledDatum)
            {
                Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality);
                Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp);
                Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                index++;
            }
        }

        private Mock<ISignalsRepository> signalsRepositoryMock;
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
        private Mock<IMissingValuePolicyRepository> mvpRepoMock;
    }
}
