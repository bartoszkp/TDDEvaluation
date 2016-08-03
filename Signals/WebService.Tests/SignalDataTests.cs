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
            Mock<IEnumerable<Domain.Datum<double>>> resultDataMock = new Mock<IEnumerable<Datum<double>>>();

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(new Signal() { Id = 1, DataType = DataType.Double });

            signalsDataRepoMock.Setup(sd => sd.GetData<double>(It.Is<Signal>(s => s.Id == 1),
                                                               It.IsAny<DateTime>(),
                                                               It.IsAny<DateTime>()))
                                                                .Returns(resultDataMock.Object);

            var result = signalsWebService.GetData(1, new DateTime(), new DateTime());

            Assert.IsNotNull(result);
        }


        [TestMethod]
        [ExpectedException(typeof(NoSuchSignalException))]
        public void SignalNotExists_SetSignalData_ThrowsException()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns((Signal)null);

            Mock<IEnumerable<Dto.Datum>> signalDataMock = new Mock<IEnumerable<Dto.Datum>>();

            signalsWebService.SetData(1, signalDataMock.Object);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullData_SetSignalData_ThrowsException()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns(new Signal() { Id = 1 });
            signalsWebService.SetData(1, null);
        }


        [TestMethod]
        public void SetSignalData_SameDataReturned_WhenGettingIt()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns(new Signal() { Id = 1, DataType = DataType.Integer });

            Mock<IEnumerable<Datum<int>>> returnedDataMock = new Mock<IEnumerable<Datum<int>>>();
            Mock<IEnumerable<Dto.Datum>> signalDataMock = new Mock<IEnumerable<Dto.Datum>>();

            signalsDataRepoMock.Setup(sd => sd.GetData<int>
                        (It.Is<Signal>(s => s.Id == 1),
                        It.IsAny<DateTime>(),
                        It.IsAny<DateTime>())).Returns(returnedDataMock.Object);



            signalsWebService.SetData(1, signalDataMock.Object);

            var createdSignalData = signalsWebService.GetData(1, new DateTime(), new DateTime());

            Assert.IsNotNull(createdSignalData);
            Assert.IsInstanceOfType(createdSignalData, typeof(IEnumerable<Dto.Datum>));


        }





        private void SetupWebService()
        {
            signalsDataRepoMock = new Mock<ISignalsDataRepository>();
            signalsRepoMock = new Mock<ISignalsRepository>();
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, signalsDataRepoMock.Object, null);
            signalsWebService = new SignalsWebService(domainService);
        }


        private Mock<ISignalsDataRepository> signalsDataRepoMock;
        private Mock<ISignalsRepository> signalsRepoMock;

    }
}
