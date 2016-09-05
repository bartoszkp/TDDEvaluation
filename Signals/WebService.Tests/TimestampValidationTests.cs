using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dto;

namespace WebService.Tests
{
    [TestClass]
    public class TimestampValidationTests
    {
        private SignalsWebService signalsWebService;

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForYearlySignal_WithInvalidSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Year);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 1, 0, 0, 1, 0), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForYearlySignal_WithInvalidMilliSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Year);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 1, 0, 0, 0, 1), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForMonthlySignal_WithInvalidSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Month);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 1, 0, 0, 1, 0), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForMonthlySignal_WithInvalidMilliSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Month);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 1, 0, 0, 0, 1), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForWeeklySignal_WithInvalidSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Week);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 3, 0, 0, 1, 0), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForWeeklySignal_WithInvalidMilliSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Week);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 3, 0, 0, 0, 1), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForDailySignal_WithInvalidSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Day);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 1, 0, 0, 1, 0), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForDailySignal_WithInvalidMilliSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Day);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 1, 0, 0, 0, 1), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForHourSignal_WithInvalidSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Hour);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 1, 0, 0, 1, 0), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForHourSignal_WithInvalidMilliSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Hour);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 1, 0, 0, 0, 1), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForMinuteSignal_WithInvalidSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Minute);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 1, 0, 0, 1, 0), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForMinuteSignal_WithInvalidMilliSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Minute);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 1, 0, 0, 0, 1), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForSecondSignal_WithInvalidMilliSecondTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupSignalWithSpecificGranularity(Domain.Granularity.Second);

            List<Dto.Datum> data = new List<Dto.Datum>()
            { new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 1, 0, 0, 0, 1), Value = 0  } };

            signalsWebService.SetData(1, data);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void GetData_DayGranularity_InvalidTimestamp_ExceptionThrown()
        {
            SetupWebService();

            var returnedSignal = new Domain.Signal() { Id = 1, Granularity = Domain.Granularity.Month, DataType = Domain.DataType.Double };
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            signalsWebService.GetData(1, new DateTime(2000, 1, 1, 3, 0, 0), new DateTime(2000, 3, 1, 0, 0, 0));

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void GetData_WhenFromIncludeEqualsToExclude_AndFromIncludedNotEqualNull_ThrowInvalidTimestampException()
        {
            SetupWebService();
            var returnedSignal = new Domain.Signal() { Id = 1 };
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            var result = signalsWebService.GetData(1, new DateTime(2018, 11, 11), new DateTime(2018, 11, 11));
        }

        private void SetupWebService()
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);
        }

        private void SetupSignalWithSpecificGranularity(Domain.Granularity granularity)
        {
            SetupWebService();
            var returnedSignal = new Domain.Signal() { Id = 1, Granularity = granularity, DataType = Domain.DataType.Integer };

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);
        }

        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> dataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();


    }
}
