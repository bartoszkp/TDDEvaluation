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
        public void GivenAnIntegerSignalAndSecondDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_FillsDataInsideRange()
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

        [TestMethod]
        public void GivenAnIntegerSignalAndMinuteDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_FillsDataInsideRange()
        {
            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Minute,
                Path = Domain.Path.FromString("root/signal1")
            };

            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 5, 0), Value = (int)2 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 8, 0), Value = (int)5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 5, 0),  Value = (int)2},
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 6, 0),  Value = (int)3 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 7, 0),  Value = (int)4 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 8, 0),  Value = (int)5 }
            };

            var olderDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 5, 0), Value = 2 }
            };

            var newerDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 1, 8, 0), Value = 5 }
            };

            var firstTimestamp = new DateTime(2000, 1, 1, 1, 5, 0);
            var lastTimestamp = new DateTime(2000, 1, 1, 1, 9, 0);

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

        [TestMethod]
        public void GivenAnIntegerSignalAndHourDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_FillsDataInsideRange()
        {
            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Hour,
                Path = Domain.Path.FromString("root/signal1")
            };

            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 5, 0, 0), Value = (int)2 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 8, 0, 0), Value = (int)5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 5, 0, 0),  Value = (int)2},
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 6, 0, 0),  Value = (int)3 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 7, 0, 0),  Value = (int)4 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 8, 0, 0),  Value = (int)5 }
            };

            var olderDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 5, 0, 0), Value = 2 }
            };

            var newerDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 8, 0, 0), Value = 5 }
            };

            var firstTimestamp = new DateTime(2000, 1, 1, 5, 0, 0);
            var lastTimestamp = new DateTime(2000, 1, 1, 9, 0, 0);

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

        [TestMethod]
        public void GivenAnIntegerSignalAndDayDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_FillsDataInsideRange()
        {
            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Day,
                Path = Domain.Path.FromString("root/signal1")
            };

            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 5), Value = (int)2 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 8), Value = (int)5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 5),  Value = (int)2},
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 6),  Value = (int)3 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 7),  Value = (int)4 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 8),  Value = (int)5 }
            };

            var olderDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 5), Value = 2 }
            };

            var newerDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 8), Value = 5 }
            };

            var firstTimestamp = new DateTime(2000, 1, 5);
            var lastTimestamp = new DateTime(2000, 1, 9);

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

        [TestMethod]
        public void GivenAnIntegerSignalAndMonthDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_FillsDataInsideRange()
        {
            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Month,
                Path = Domain.Path.FromString("root/signal1")
            };

            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = (int)2 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = (int)5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1),  Value = (int)2},
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 6, 1),  Value = (int)3 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 7, 1),  Value = (int)4 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1),  Value = (int)5 }
            };

            var olderDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2 }
            };

            var newerDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5 }
            };

            var firstTimestamp = new DateTime(2000, 5, 1);
            var lastTimestamp = new DateTime(2000, 9, 1);

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

        [TestMethod]
        public void GivenAnIntegerSignalAndWeekDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_FillsDataInsideRange()
        {
            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Week,
                Path = Domain.Path.FromString("root/signal1")
            };

            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1), Value = (int)2 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 8), Value = (int)5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1),  Value = (int)2},
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 8, 8),  Value = (int)3 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 8, 15),  Value = (int)4 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 8, 22),  Value = (int)5 }
            };

            var olderDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Good, Timestamp = new DateTime(2016, 8, 1), Value = 2 }
            };

            var newerDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Fair, Timestamp = new DateTime(2016, 8, 22), Value = 5 }
            };

            var firstTimestamp = new DateTime(2016, 8, 1);
            var lastTimestamp = new DateTime(2016, 8, 29);

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

        [TestMethod]
        public void GivenAnIntegerSignalAndYearDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_FillsDataInsideRange()
        {
            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Year,
                Path = Domain.Path.FromString("root/signal1")
            };

            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 1, 1), Value = (int)2 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2008, 1, 1), Value = (int)5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 1, 1),  Value = (int)2},
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2006, 1, 1),  Value = (int)3 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2007, 1, 1),  Value = (int)4 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2008, 1, 1),  Value = (int)5 }
            };

            var olderDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Good, Timestamp = new DateTime(2005, 1, 1), Value = 2 }
            };

            var newerDatum = new Datum<int>[]
            {
                    new Datum<int>() {Quality = Quality.Fair, Timestamp = new DateTime(2008, 1, 1), Value = 5 }
            };

            var firstTimestamp = new DateTime(2005, 1, 1);
            var lastTimestamp = new DateTime(2009, 1, 1);

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

        [TestMethod]
        public void GivenADoubleSignalAndMonthDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_FillsDataInsideRange()
        {
            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = Domain.Path.FromString("root/signal1")
            };

            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = (double)2 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = (double)5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1),  Value = (double)2},
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 6, 1),  Value = (double)3 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 7, 1),  Value = (double)4 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1),  Value = (double)5 }
            };

            var olderDatum = new Datum<double>[]
            {
                    new Datum<double>() {Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = (double)2 }
            };

            var newerDatum = new Datum<double>[]
            {
                    new Datum<double>() {Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = (double)5 }
            };

            var firstTimestamp = new DateTime(2000, 5, 1);
            var lastTimestamp = new DateTime(2000, 9, 1);

            signalsRepositoryMock = new Mock<ISignalsRepository>();

            GivenASignal(existingSignal);

            signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>);

            signalDataRepositoryMock
                .Setup(sdrm => sdrm.GetDataNewerThan<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), 1))
                .Returns(newerDatum);

            signalDataRepositoryMock
                .Setup(sdrm => sdrm.GetDataOlderThan<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), 1))
                .Returns(olderDatum);

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDouble());

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

        [TestMethod]
        public void GivenADecimalSignalAndMonthDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_FillsDataInsideRange()
        {
            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Decimal,
                Granularity = Granularity.Month,
                Path = Domain.Path.FromString("root/signal1")
            };

            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5m }
            };

            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1),  Value = 2m },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 6, 1),  Value = 3m },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 7, 1),  Value = 4m },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1),  Value = 5m }
            };

            var olderDatum = new Datum<decimal>[]
            {
                    new Datum<decimal>() {Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m }
            };

            var newerDatum = new Datum<decimal>[]
            {
                    new Datum<decimal>() {Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5m }
            };

            var firstTimestamp = new DateTime(2000, 5, 1);
            var lastTimestamp = new DateTime(2000, 9, 1);

            signalsRepositoryMock = new Mock<ISignalsRepository>();

            GivenASignal(existingSignal);

            signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<decimal>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<decimal>>>);

            signalDataRepositoryMock
                .Setup(sdrm => sdrm.GetDataNewerThan<decimal>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), 1))
                .Returns(newerDatum);

            signalDataRepositoryMock
                .Setup(sdrm => sdrm.GetDataOlderThan<decimal>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), 1))
                .Returns(olderDatum);

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDecimal());

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GivenAStringSignalAndMonthDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_ThrowsException()
        {
            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.String,
                Granularity = Granularity.Month,
                Path = Domain.Path.FromString("root/signal1")
            };

            signalsRepositoryMock = new Mock<ISignalsRepository>();

            GivenASignal(existingSignal);

            signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyString());

            var signalsDomainService = new SignalsDomainService(
                signalsRepositoryMock.Object,
                signalDataRepositoryMock.Object,
                missingValuePolicyRepositoryMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));
        }

        private void GivenASignal(Signal signal)
        {
            signalsRepositoryMock
                .Setup(sr => sr.Get(signal.Id.Value))
                .Returns(signal);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock= new Mock<IMissingValuePolicyRepository>();

    }
}

