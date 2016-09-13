using Domain;
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
    public class GetDataTests
    {

        private SignalsWebService signalsWebService;

        [TestMethod]
        public void IncorrectInterval_GetData_ReturnsEmptyCollection()
        {
            SignalsDomainService domainService = new SignalsDomainService(null, null, null);
            signalsWebService = new SignalsWebService(domainService);

            var result =  signalsWebService.GetData(1, new DateTime(2000, 3, 1), new DateTime(2000, 1, 1));

            Assert.AreEqual(0, result.Count());

        }

        [TestMethod]
        public void GetData_ReturnsDataSortedByDate()
        {
            SetupWebService();
            var datums = new Datum<bool>[] {
                new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2018, 2, 1), Value = false },
                new Datum<bool>() { Quality = Quality.Fair, Timestamp = new DateTime(2018, 1, 1), Value = true }
            };
            var signal = new Signal()
            {
                Id = 21,
                DataType = DataType.Boolean,
                Granularity = Granularity.Month
            };

            signalsRepositoryMock
                .Setup(sr => sr.Get(It.IsAny<int>()))
                .Returns(signal);
            missingValuePolicyRepositoryMock
                .Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyBoolean());
            signalsDataRepositoryMock
                .Setup(sdr => sdr.GetData<bool>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(datums);

            var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2018, 1, 1), new DateTime(2018, 3, 1));

            Assert.IsTrue(result.ElementAt(0).Timestamp < result.ElementAt(1).Timestamp);
        }
        [TestMethod]
        public void WhenGetData_WithSameTime_ReturnData_FirsOrder()
        {
            SetupWebService();

            var signal = new Domain.Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Second,
                Id = 1,
                Path = Path.FromString("x/z")
            };

            var time = new DateTime(2000, 1, 1);

            Datum<double>[] data =
             {
                new Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 }
            };

            signalsRepositoryMock.Setup(x => x.Get(1)).Returns(signal);

            signalsDataRepositoryMock.Setup(x => x.GetData<double>(signal, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(data);

            missingValuePolicyRepositoryMock.Setup(x => x.Get(signal)).Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDouble());

            var returnedData = signalsWebService.GetData(1, time, time);

            Assert.AreEqual(1, returnedData.Count());
        }

        [TestMethod]
        public void WhenGetData_WithSameTime_ReturnData_Zero()
        {
            SetupWebService();

            var signal = new Domain.Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Second,
                Id = 1,
                Path = Path.FromString("x/z")
            };

            var time = new DateTime(2000, 1, 1);

            Datum<double>[] data =
             {
                new Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 }
            };

            signalsRepositoryMock.Setup(x => x.Get(1)).Returns(signal);

            signalsDataRepositoryMock.Setup(x => x.GetData<double>(signal, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(data);

            missingValuePolicyRepositoryMock.Setup(x => x.Get(signal)).Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());

            var returnedData = signalsWebService.GetData(1, time, time);

            Assert.AreEqual(1, returnedData.Count());
        }


        [TestMethod]
        public void WhenGetData_WithSameTime_ReturnData_None()
        {
            SetupWebService();

            var signal = new Domain.Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Second,
                Id = 1,
                Path = Path.FromString("x/z")
            };

            var time = new DateTime(2000, 1, 1);

            Datum<double>[] data =
             {
                new Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 }
            };

            signalsRepositoryMock.Setup(x => x.Get(1)).Returns(signal);

            signalsDataRepositoryMock.Setup(x => x.GetData<double>(signal, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(data);

            missingValuePolicyRepositoryMock.Setup(x => x.Get(signal)).Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

            var returnedData = signalsWebService.GetData(1, time, time);

            Assert.AreEqual(1, returnedData.Count());
        }

        [TestMethod]
        public void WhenGetData_WithSameTime_ReturnData_Specific()
        {
            SetupWebService();

            var signal = new Domain.Signal()
            {
                DataType = DataType.Double,
                Granularity = Granularity.Second,
                Id = 1,
                Path = Path.FromString("x/z")
            };

            var time = new DateTime(2000, 1, 1);

            Datum<double>[] data =
             {
                new Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 }
            };

            signalsRepositoryMock.Setup(x => x.Get(1)).Returns(signal);

            signalsDataRepositoryMock.Setup(x => x.GetData<double>(signal, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(data);

            missingValuePolicyRepositoryMock.Setup(x => x.Get(signal)).Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble());

            var returnedData = signalsWebService.GetData(1, time, time);

            Assert.AreEqual(1, returnedData.Count());
        }


        private void SetupWebService()
        {
            SignalsDomainService domainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(domainService);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
    }
}
