﻿using Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Domain.Services.Implementation;
using Domain;
using Domain.Exceptions;
using Domain.MissingValuePolicy;

namespace WebService.Tests
{
    [TestClass]
    public class GetDataTests
    {

        private SignalsWebService signalsWebService;


        [TestMethod]
        [ExpectedException(typeof(NoSuchSignalException))]
        public void SignalNotInDatabase_GetData_ThrowsException()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns((Signal)null);
            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));
        }



        [TestMethod]
        public void SignalExists_GetData_WithSameTimestaps_SingleDatumReturned()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Month
            };

            var timestamp = new DateTime(2000, 1, 1);

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(signal);
            Mock<NoneQualityMissingValuePolicy<int>> mvp = new Mock<NoneQualityMissingValuePolicy<int>>();

            mvpRepoMock.Setup(m => m.Get(signal)).Returns(mvp.Object);


            signalsDataRepoMock.Setup(s => s.GetData<int>(signal, timestamp, timestamp))
                .Returns(new List<Datum<int>>());

            var result = signalsWebService.GetData(1, timestamp, timestamp);

            var fetchedDatumObject = result.ElementAt(0);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Dto.Quality.None, fetchedDatumObject.Quality);
            Assert.AreEqual(0, fetchedDatumObject.Value);
            Assert.AreEqual(timestamp, fetchedDatumObject.Timestamp);

        }


        #region TimeStampVerifyTests
        [ExpectedException(typeof(QuerryAboutDateWithIncorrectFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Second_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Second
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 1, 1, 1, 11, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [ExpectedException(typeof(QuerryAboutDateWithIncorrectFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Minute_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Minute
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 1, 1, 0, 1, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [ExpectedException(typeof(QuerryAboutDateWithIncorrectFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Hour_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Hour
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 1, 0, 0, 1, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [ExpectedException(typeof(QuerryAboutDateWithIncorrectFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Day_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Day
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0, 1, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [ExpectedException(typeof(QuerryAboutDateWithIncorrectFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Month_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Month
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [ExpectedException(typeof(QuerryAboutDateWithIncorrectFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Week_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Week
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [ExpectedException(typeof(QuerryAboutDateWithIncorrectFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Year_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Year
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }


        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Second()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Second
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 1, 1, 1, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Minute()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Minute
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 1, 1, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Hour()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Hour
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Day()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Day
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }


        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Month()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Month
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Week()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Week
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }


        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Year()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Year
            };

            signalsRepoMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        #endregion

        private void SetupWebService()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepoMock.Object, signalsDataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        private Mock<ISignalsDataRepository> signalsDataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

    }
}