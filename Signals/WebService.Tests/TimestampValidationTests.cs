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
        public void SetData_InvalidMonthGranularityTimestamp_ExceptionThrown()
        {
            SetupWebService();
            var returnedSignal = new Domain.Signal() { Id = 1, Granularity = Domain.Granularity.Month, DataType = Domain.DataType.Double };


            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            List<Dto.Datum> data = new List<Dto.Datum>()
                {
                    new Dto.Datum() {Quality = Dto.Quality.Good,Timestamp = new DateTime(2000, 1, 1, 2, 45, 0),Value = (double)1.5  }
                };

            signalsWebService.SetData(1, data);


        }

        [TestMethod]
        [ExpectedException(typeof(InvalidTimestampException))]
        public void WhenSettigDataForMonthlySignal_WithInvalidDataTimestamp_InvalidTimestampExceptionIsThrown()
        {
            SetupWebService();
            var returnedSignal = new Domain.Signal() { Id = 1, Granularity = Domain.Granularity.Month, DataType = Domain.DataType.Integer };

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            List<Dto.Datum> data = new List<Dto.Datum>()
            {
                new Dto.Datum() {Quality = Dto.Quality.Bad,Timestamp = new DateTime(2000, 1, 1, 0, 0, 1, 0),Value = 0  }
            };

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


        private void SetupWebService()
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);
        }

        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> dataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();


    }
}
