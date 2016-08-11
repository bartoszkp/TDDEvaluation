﻿using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.IdNotNullException))]
            public void GivenNoSignals_WhenAddingASignalWithId_ThrowIdNotNullException()
            {
                GivenNoSignals();

                var result = signalsWebService.Add(SignalWith(id: 1));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsIt()
            {
                GivenNoSignals();

                var result = signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Double,
                    granularity: Dto.Granularity.Month,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }
                    ));

                Assert.AreEqual(Dto.DataType.Double, result.DataType);
                Assert.AreEqual(Dto.Granularity.Month, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_CallsRepositoryAdd()
            {
                GivenNoSignals();

                signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Double,
                    granularity: Dto.Granularity.Month,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }
                    ));

                signalsRepositoryMock.Verify(sr => sr.Add(It.Is<Domain.Signal>(passedSignal
                    => passedSignal.DataType == DataType.Double
                        && passedSignal.Granularity == Granularity.Month
                        && passedSignal.Path.ToString() == "root/signal")));
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.GetById(0);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByItsId_ReturnsIt()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.GetById(signalId);

                Assert.AreEqual(signalId, result.Id);
                Assert.AreEqual(Dto.DataType.Integer, result.DataType);
                Assert.AreEqual(Dto.Granularity.Second, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingByPathWithEmptyContent_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.Get(new Dto.Path() { Components = new[] { "" } });

                Assert.AreEqual(null, result);
            }

            [TestMethod]
            public void GivenASignalFromDomainToDto_WhenTryingToGetByPath_ReturnsTheSameSignal()
            {
                GivenNoSignals();

                GivenASignalWithPath(SignalWith(
                    id: 1,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.Get(new Dto.Path() { Components = new[] { "root", "signal" } });

                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(Dto.DataType.Integer, result.DataType);
                Assert.AreEqual(Dto.Granularity.Second, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void EmptyRepository_SetData_SignalDesentExistException()
            {
                GiveNoSignalData();

                signalsWebService.SetData(1, new List<Dto.Datum>());
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidOperationException))]
            public void EmptyRepository_GetData_SignalDesentExistException()
            {
                GiveNoSignalData();

                signalsWebService.GetData(1, new DateTime(), new DateTime());
            }


            [TestMethod]
            public void RepositoryWithSignal_SetData_DataSaved()
            {
                GiveNoSignalData();

                var item = new List<Dto.Datum>()
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = DateTime.Now, Value = 1.0 },
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = DateTime.Now, Value = 2.0 },
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = DateTime.Now, Value = 3.0 }
                };

                signalsRepositoryMock.Setup(x => x.Get(It.IsAny<int>()))
                    .Returns(new Signal() { DataType = DataType.Double });


                

                signalsWebService.SetData(1, item);

                signalDataRepositoryMock.Verify(x => x.SetData<double>(
                    It.Is<IEnumerable<Datum<double>>>(d => DataDtoCompareDouble(d))));

            }

            private bool DataDtoCompareDouble(IEnumerable<Datum<double>> data)
            {
                var list = data.ToList();
                if (list[0].Value == 1 && list[1].Value == 2 & list[2].Value == 3)
                    return true;
                else
                    return false;
            }

  

            [TestMethod]
            public void RepositoryWithSignalAndData_GetData_DataReturned()
            {
                GiveNoSignalData();

                signalDataRepositoryMock.Setup(x => x.GetData<double>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new List<Datum<double>>()
                    {
                        new Datum<double>(),
                        new Datum<double>()
                    });

                signalsRepositoryMock.Setup(x => x.Get(It.IsAny<int>()))
                    .Returns(new Signal() { DataType = DataType.Double });

                var item = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 2));
            }

            [TestMethod]
            public void RepositoryWithSignalAndNoData_GetData_ReturnEmptyCollection()
            {
                GiveNoSignalData();

                signalsRepositoryMock.Setup(x => x.Get(It.IsAny<int>()))
                    .Returns(new Signal() { DataType = DataType.Double });



                var item = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 2));

                Assert.IsTrue(item.Count() == 0);
            }
            [TestMethod]
            public void RepositoryWithSignalAndData_GetData_ReturnSortedCollection()
            {
                GiveNoSignalData();

                var dbList = new List<Datum<double>>()
                {
                        new Datum<double>() { Timestamp = new DateTime(2000, 2, 1) },
                        new Datum<double>() { Timestamp = new DateTime(2000, 1, 1) }
                };

                signalsRepositoryMock.Setup(x => x.Get(It.IsAny<int>()))
                    .Returns(new Signal() { DataType = DataType.Double });

                signalDataRepositoryMock.Setup(x => x.GetData<double>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(dbList);

                var item = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                Assert.AreEqual(item.First().Timestamp, dbList[1].Timestamp);
            }

            private Dto.Signal SignalWith(
                int? id = null,
                Dto.DataType dataType = Dto.DataType.Boolean,
                Dto.Granularity granularity = Dto.Granularity.Day,
                Dto.Path path = null)
            {
                return new Dto.Signal()
                {
                    Id = id,
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }

            private Domain.Signal SignalWith(
                int id,
                Domain.DataType dataType,
                Domain.Granularity granularity,
                Domain.Path path = null)
            {
                return new Domain.Signal()
                {
                    Id = id,
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }

            private void GiveNoSignalData()
            {
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalDomianService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalDomianService);
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

            private void GivenASignal(Signal signal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
            }
            private void GivenASignalWithPath(Signal signal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Path))
                    .Returns(signal);
            }


            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalDataRepositoryMock;
        }
    }
}