using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Domain.Exceptions;
using Domain.MissingValuePolicy;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;

            #region test for method Add
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

            #endregion


            #region tests for method GetById
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

            #endregion


            #region tests for method Get
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


            #endregion


            #region tests for methods SetData
            [TestMethod]
            [ExpectedException(typeof(IdNotNullException))]
            public void EmptyRepository_SetData_SignalDesentExistException()
            {
                GiveNoSignalData();

                signalsWebService.SetData(1, new List<Dto.Datum>());
            }

            [TestMethod]
            public void RepositoryWithSignal_SetData_DataSaved()
            {
                GiveNoSignalData();

                var item = new List<Dto.Datum>()
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp =new DateTime(2000,1,1), Value = 1.0 },
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000,2,1), Value = 2.0 },
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000,3,1), Value = 3.0 }
                };

                signalsRepositoryMock.Setup(x => x.Get(It.IsAny<int>()))
                    .Returns(new Signal() { DataType = DataType.Double });




                signalsWebService.SetData(1, item);

                signalDataRepositoryMock.Verify(x => x.SetData<double>(
                    It.Is<IEnumerable<Datum<double>>>(d => DataDtoCompareDouble(d))));


            }


            [TestMethod]
            public void DatumTypeOfDouble_WhenSettingData_RepoIsCalled()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Double,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("root/signal1")
                };
                var existingDatum = new Dto.Datum[]
                {

                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 }
                };
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<double>(It.IsAny<IEnumerable<Datum<double>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum);

                signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>();
                int i = 0;

                foreach (var ed in datum)
                {
                    signalDataRepositoryMock.Verify(sdrm => sdrm.SetData<double>(It.Is<IEnumerable<Datum<double>>>(d =>
                    (
                        d.ElementAt(i).Quality == ed.Quality
                        && d.ElementAt(i).Timestamp == ed.Timestamp
                        && d.ElementAt(i).Value == ed.Value))));
                    i++;
                }
            }

            [TestMethod]
            public void DatumTypeOfDecimal_WhenSettingData_RepoIsCalled()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Decimal,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("root/signal1")
                };
                var existingDatum = new Dto.Datum[]
                {

                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (decimal)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (decimal)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (decimal)2 }
                };
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<decimal>(It.IsAny<IEnumerable<Datum<decimal>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum);

                signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<decimal>>>();
                int i = 0;

                foreach (var ed in datum)
                {
                    signalDataRepositoryMock.Verify(sdrm => sdrm.SetData<decimal>(It.Is<IEnumerable<Datum<decimal>>>(d =>
                    (
                        d.ElementAt(i).Quality == ed.Quality
                        && d.ElementAt(i).Timestamp == ed.Timestamp
                        && d.ElementAt(i).Value == ed.Value))));
                    i++;
                }
            }

            [TestMethod]
            public void DatumTypeOfInt_WhenSettingData_RepoIsCalled()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("root/signal1")
                };
                var existingDatum = new Dto.Datum[]
                {

                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (int)2 }
                };
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<int>(It.IsAny<IEnumerable<Datum<int>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum);

                signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>();
                int i = 0;

                foreach (var ed in datum)
                {
                    signalDataRepositoryMock.Verify(sdrm => sdrm.SetData<int>(It.Is<IEnumerable<Datum<int>>>(d =>
                    (
                        d.ElementAt(i).Quality == ed.Quality
                        && d.ElementAt(i).Timestamp == ed.Timestamp
                        && d.ElementAt(i).Value == ed.Value))));
                    i++;
                }
            }

            [TestMethod]
            public void DatumTypeOfBool_WhenSettingData_RepoIsCalled()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Boolean,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("root/signal1")
                };
                var existingDatum = new Dto.Datum[]
                {

                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (bool)true },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (bool)false },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (bool)false }
                };
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<bool>(It.IsAny<IEnumerable<Datum<bool>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum);

                signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<bool>>>();
                int i = 0;

                foreach (var ed in datum)
                {
                    signalDataRepositoryMock.Verify(sdrm => sdrm.SetData<bool>(It.Is<IEnumerable<Datum<bool>>>(d =>
                    (
                        d.ElementAt(i).Quality == ed.Quality
                        && d.ElementAt(i).Timestamp == ed.Timestamp
                        && d.ElementAt(i).Value == ed.Value))));
                    i++;
                }
            }

            [TestMethod]
            public void DatumTypeOfString_WhenSettingData_RepoIsCalled()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.String,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("root/signal1")
                };
                var existingDatum = new Dto.Datum[]
                {

                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (string)"a" },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (string)"b" },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (string)"c" }
                };
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<string>(It.IsAny<IEnumerable<Datum<string>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum);

                signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<string>>>();
                int i = 0;

                foreach (var ed in datum)
                {
                    signalDataRepositoryMock.Verify(sdrm => sdrm.SetData<string>(It.Is<IEnumerable<Datum<string>>>(d =>
                    (
                        d.ElementAt(i).Quality == ed.Quality
                        && d.ElementAt(i).Timestamp == ed.Timestamp
                        && d.ElementAt(i).Value == ed.Value))));
                    i++;
                }
            }

            [TestMethod]
            public void SettingEmptyDataShouldNotThrow()
            {
                GiveNoSignalData();
                var newSignal = new Domain.Signal()
                {
                    Id = 25253,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("root/signal1")
                };
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<int>()))
                    .Returns(newSignal);
            }

            [TestMethod]
            public void SetDataAddindSignalsToDatum()
            {
                GiveNoSignalData();
                var newSignal = new Domain.Signal()
                {
                    Id = 7878,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day,
                    Path = Path.FromString("signal1")
                };

                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<int>()))
                    .Returns(newSignal);
                signalDataRepositoryMock
                    .Setup(sdr => sdr.SetData(It.IsAny<IEnumerable<Datum<bool>>>()));

                signalsWebService.SetData(
                newSignal.Id.Value,
                 new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = false } });

                signalDataRepositoryMock.Verify(sdr => sdr.SetData<Boolean>(It.Is<IEnumerable<Datum<bool>>>
                    (d => d.First().Signal == newSignal)));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void SetDataWithIncorrectTimestampsShouldThrowsException()
            {
                GiveNoSignalData();
                int signalId = 465;
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<int>()))
                    .Returns(new Signal()
                    {
                        Id = signalId,
                        DataType = DataType.Boolean,
                        Granularity = Granularity.Minute,
                        Path = Path.FromString("sfdsfd/sfdad")
                    });

                signalsWebService.SetData(signalId, new Dto.Datum[]
                {
                    new Dto.Datum() {Quality = Dto.Quality.None, Timestamp = new DateTime(2132, 8, 21, 14, 58, 45), Value= false }
                });
            }

            #endregion


            #region tests for method GetData
            [TestMethod]
            [ExpectedException(typeof(IdNotNullException))]
            public void EmptyRepository_GetData_SignalDesentExistException()
            {
                GiveNoSignalData();

                signalsWebService.GetData(1, new DateTime(), new DateTime());
            }

            [TestMethod]
            public void RepositoryWithSignalAndData_GetData_ReturnSortedCollection()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Double,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {

                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 }
                };

                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));

                GivenASignal(existingSignal);
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(x => x.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());


                var signalsDomainService = new SignalsDomainService
                    (signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);


                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                var existingSortedDatum = existingDatum.OrderBy(x => x.Timestamp);

                for (int i = 0; i < result.Count(); i++)
                {
                    Assert.AreEqual(existingSortedDatum.ElementAt(i).Timestamp, result.ElementAt(i).Timestamp);
                }
            }

            [TestMethod]
            public void RepositoryWithSignalAndData_WhenGet_DataIsFilled()
            {
                var existingSignal = new Signal()
                {
                    Id = 1111,
                    DataType = Domain.DataType.Double,
                    Granularity = Granularity.Month,
                    Path = Domain.Path.FromString("signal")
                };
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1)},
                };
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));

                GivenASignal(existingSignal);


                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                     .Setup(mvp => mvp.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));
                missingValuePolicyRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());


                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);


                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                var expectedDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1)},
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1)},
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1)}
                };
                for (int i = 0; i < expectedDatum.Length; i++)
                {
                    Assert.AreEqual(expectedDatum.ElementAt(i).Timestamp, result.ElementAt(i).Timestamp);
                }
            }
            
            [TestMethod]
            public void GivenASignal_GetData_ReturnAllNoneQualityMissingValueElements()
            {
                var existingSignal = new Signal()
                {
                    Id = 999,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Second,
                    Path = Domain.Path.FromString("signal")
                };

                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                GivenASignal(existingSignal);
                missingValuePolicyRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDecimal() { Signal = existingSignal });

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1, 0, 1, 0));
                Assert.AreEqual(60, result.Count());
            }

            [TestMethod]
            public void GetDataReturnsOneDatumWhenTimestampsAreEqual()
            {
                GiveNoSignalData();
                var newSignal = new Domain.Signal()
                {
                    Id = 355,
                    DataType = Domain.DataType.Double,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("signal1")
                };
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<int>()))
                    .Returns(newSignal);
                missingValuePolicyRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                signalDataRepositoryMock
                    .Setup(sr => sr.GetData<double>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new Datum<double>[] {
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 } });

                var result = signalsWebService.GetData(21, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));
                Assert.AreEqual(new DateTime(2000, 1, 1), result.First().Timestamp);
            }

            [TestMethod]
            public void ZeroOrderMissingValuePolicyCorrectlyFillDataWhenFirstDatumExist()
            {
                GiveNoSignalData();
                int singalId = 143;
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<int>()))
                    .Returns(new Signal()
                    {
                        Id = singalId,
                        DataType = DataType.Double,
                        Granularity = Granularity.Month,
                        Path = Path.FromString("sfda")
                    });
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());
                signalDataRepositoryMock
                    .Setup(sdr => sdr.GetData<double>(It.IsAny<Signal>(), It.IsAny<System.DateTime>(), It.IsAny<System.DateTime>()))
                    .Returns(new Datum<double>[]
                        {
                            new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                            new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
                        });

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
            }

            [TestMethod]
            public void ZeroOrderMissingValuePolicyCorrectlyFillDataWhenFirstDatumNonExist()
            {
                GiveNoSignalData();
                int singalId = 143;
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<int>()))
                    .Returns(new Signal()
                    {
                        Id = singalId,
                        DataType = DataType.Double,
                        Granularity = Granularity.Month,
                        Path = Path.FromString("sfda")
                    });
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());
                signalDataRepositoryMock
                    .Setup(sdr => sdr.GetData<double>(It.IsAny<Signal>(), It.IsAny<System.DateTime>(), It.IsAny<System.DateTime>()))
                    .Returns(new Datum<double>[]
                        {
                            new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
                        });

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GetDataWithIncorrectTimestampsShouldThrowsException()
            {
                GiveNoSignalData();
                int signalId = 243;
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<int>()))
                    .Returns(new Signal()
                    {
                        Id = signalId,
                        DataType = DataType.String,
                        Granularity = Granularity.Week,
                        Path = Path.FromString("sfdsfd/sfdad")
                    });

                signalsWebService.GetData(signalId, new DateTime(2016, 08, 23), DateTime.MaxValue);
            }

            #endregion


            #region for methods MissingValuePolicy
            [TestMethod]
            public void GivenASignal_WhenAddingSingal_ThenSettingNoneQualityMissingValuePolicy()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var sig = signalsWebService.Add(new Dto.Signal()
                {
                    DataType = Dto.DataType.Double,
                    Granularity = Dto.Granularity.Month,
                    Path = new Dto.Path() { Components = new[] { "root", "signal" } }
                });

                missingValuePolicyRepositoryMock.Verify(mvpr => mvpr.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>()));
            }

            [TestMethod]
            public void GivenASignal_SetMissingValuePolicy_RepoIsCalled()
            {
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

                GivenASignal(SignalWith(
                    1, DataType.Double, Granularity.Day, Path.FromString("root/signal1")));

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
                var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
                signalsWebService.SetMissingValuePolicy(1, policy);

                signalsRepositoryMock.Verify(srm => srm.Get(1));
                missingValuePolicyRepositoryMock.Verify(mvp => mvp.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

            }

            [TestMethod]
            [ExpectedException(typeof(SignalIsNullException))]
            public void GivenASignal_SetMissingValuePolicyWhenSignalNonExist_ReturnException()
            {
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

                GivenASignal(SignalWith(
                    1, DataType.Double, Granularity.Day, Path.FromString("root/signal1")));

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
                var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();

                signalsWebService.SetMissingValuePolicy(2, policy);
            }

            [TestMethod]
            public void GivenASignalAndMissingValuePolicy_GetMissingValuePolicy_RepoIsCalled()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Double,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("root/signal1")
                };

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble());

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.GetMissingValuePolicy(existingSignal.Id.Value);

                missingValuePolicyRepositoryMock.Verify(mvp => mvp.Get(It.Is<Domain.Signal>(s => (
                    existingSignal.Id == existingSignal.Id
                    && existingSignal.DataType == s.DataType
                    && existingSignal.Granularity == s.Granularity
                    && existingSignal.Path == s.Path))));
            }

            [TestMethod]
            [ExpectedException(typeof(SignalIsNullException))]
            public void GivenASignal_GetMissingValuePolicyWhenSignalNonExist_ReturnException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Double,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingPolicy = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                {
                    Id = 1,
                    Quality = Domain.Quality.Bad,
                    Value = (double)1.5
                };

                int wrongId = 3;

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble());
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.GetMissingValuePolicy(wrongId).ToDomain<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>();

            }


            [TestMethod]
            public void RepositoryWithNoSignal_WhenAdd_PolicyIsAdded()
            {
                GivenNoSignals();

                var item = SignalWith(null, Dto.DataType.Boolean, Dto.Granularity.Day, new Dto.Path() { Components = new[] { "x", "z" } });

                signalsWebService.Add(item);

                missingValuePolicyRepositoryMock.Verify(x => x.Set(
                    It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<bool>>()));
            }

            #endregion


            #region test for GetPathEntry()
            [TestMethod]
            public void GivenNoSignals_GetPathentry_RepoIsCalled()
            {
                GivenNoSignals();
                List<Signal> listSignal = new List<Signal>();
                listSignal.Add(new Signal() { Id = 1, DataType = DataType.Boolean, Granularity = Granularity.Minute, Path = Path.FromString("s0") });
                listSignal.Add(new Signal() { Id = 2, DataType = DataType.Boolean, Granularity = Granularity.Minute, Path = Path.FromString("root/s1") });
                listSignal.Add(new Signal() { Id = 3, DataType = DataType.Boolean, Granularity = Granularity.Minute, Path = Path.FromString("root/podkatalog/s2") });
                listSignal.Add(new Signal() { Id = 1, DataType = DataType.Boolean, Granularity = Granularity.Minute, Path = Path.FromString("root / podkatalog / s3") });
                listSignal.Add(new Signal() { Id = 2, DataType = DataType.Boolean, Granularity = Granularity.Minute, Path = Path.FromString("root/podkatalog/podpodkatalog/s4") });
                listSignal.Add(new Signal() { Id = 3, DataType = DataType.Boolean, Granularity = Granularity.Minute, Path = Path.FromString("root/podkatalog2/s5") });



                signalsRepositoryMock
                    .Setup(sr => sr.GetAllWithPathPrefix(It.IsAny<Path>()))
                    .Returns(listSignal);

                signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Double,
                    granularity: Dto.Granularity.Month,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }
                    ));

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root" } });


                signalsRepositoryMock.Verify(s => s.GetAllWithPathPrefix(It.IsAny<Path>()));
            }

            [TestMethod]
            public void GivenListOfSignals_WhenGettingPathEntry_ReturnsPathWithCollectionOfSignalsFromMainDirectoryAndSubpathsFromMainDirectory()
            {
                List<Signal> signalsList = new List<Signal>()
                {
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/s1")},
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("s0") },
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/sub/s2") },
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/sub/s3") },
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/subsub/s4") },
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/sub/s5") }
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                signalsRepositoryMock
                    .Setup(srm => srm.GetAllWithPathPrefix(It.IsAny<Path>()))
                    .Returns(signalsList.AsEnumerable);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "root" } });

                Assert.AreEqual(1, result.Signals.Count());
                Assert.AreEqual(signalsList.First().ToDto<Dto.Signal>().DataType, result.Signals.First().DataType);
                CollectionAssert.AreEqual(signalsList.First().ToDto<Dto.Signal>().Path.Components.ToArray(),
                    result.Signals.First().Path.Components.ToArray());

                Assert.AreEqual(2, result.SubPaths.Count());
                CollectionAssert.AreEqual(new[] { "root", "sub" }, result.SubPaths.First().Components.ToArray());
                CollectionAssert.AreEqual(new[] { "root", "subsub" }, result.SubPaths.Last().Components.ToArray());
            }
            #endregion


            #region tests for Delete() method
            [TestMethod]
            public void GivenASignal_WhenDeletingASignal_ProperSignalRepositoryDeleteIsCalled()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                signalsRepositoryMock
                    .Setup(srm => srm.Delete(existingSignal));

                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Delete(existingSignal.Id.Value);

                signalsRepositoryMock
                    .Verify(srm => srm.Delete(existingSignal));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SignalDoesNotExist))]
            public void GivenASignal_WhenDeletingSignalWithWrongId_ExceptionIsThrown()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);
                
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Delete(2);
            }

            [TestMethod]
            public void GivenASignal_WhenDeletingThisSignal_CheckIfItsMissingValuePolicyIsSetToNull()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);
                
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Set(existingSignal, null));

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Delete(existingSignal.Id.Value);

                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(existingSignal, null));
            }

            [TestMethod]
            public void GivenADoubleSignal_WhenDeletingThisSignal_CheckIfItsDataIsDeleted()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.DeleteData<double>(existingSignal));

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Delete(existingSignal.Id.Value);

                signalDataRepositoryMock
                    .Verify(sdrm => sdrm.DeleteData<double>(existingSignal));
            }

            [TestMethod]
            public void GivenAnIntegerSignal_WhenDeletingThisSignal_CheckIfItsDataIsDeleted()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Integer
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.DeleteData<int>(existingSignal));

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Delete(existingSignal.Id.Value);

                signalDataRepositoryMock
                    .Verify(sdrm => sdrm.DeleteData<int>(existingSignal));
            }

            [TestMethod]
            public void GivenADecimalSignal_WhenDeletingThisSignal_CheckIfItsDataIsDeleted()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Decimal
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.DeleteData<decimal>(existingSignal));

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Delete(existingSignal.Id.Value);

                signalDataRepositoryMock
                    .Verify(sdrm => sdrm.DeleteData<decimal>(existingSignal));
            }

            [TestMethod]
            public void GivenABooleanSignal_WhenDeletingThisSignal_CheckIfItsDataIsDeleted()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Boolean
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.DeleteData<bool>(existingSignal));

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Delete(existingSignal.Id.Value);

                signalDataRepositoryMock
                    .Verify(sdrm => sdrm.DeleteData<bool>(existingSignal));
            }
            [TestMethod]
            public void GivenAStringSignal_WhenDeletingThisSignal_CheckIfItsDataIsDeleted()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.String
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.DeleteData<string>(existingSignal));

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Delete(existingSignal.Id.Value);

                signalDataRepositoryMock
                    .Verify(sdrm => sdrm.DeleteData<string>(existingSignal));
            }
            #endregion


            #region tests for FirstOrderPolicy
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
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1),  Value = 2m},
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
                    .Setup(sdrm => sdrm.GetData<decimal>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<decimal>>>);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataNewerThan<decimal>(existingSignal, It.IsAny<DateTime>(), 1))
                    .Returns(newerDatum);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataOlderThan<decimal>(existingSignal, It.IsAny<DateTime>(), 1))
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
            public void GivenADecimalSignalAndMonthDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_FillsDataWhenThereIsNoOlderData()
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
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5m }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 5, 1),  Value = 0m},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 6, 1),  Value = 0m },
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 7, 1),  Value = 0m },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1),  Value = 5m }
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
                    .Setup(sdrm => sdrm.GetData<decimal>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<decimal>>>);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataNewerThan<decimal>(existingSignal, It.IsAny<DateTime>(), 1))
                    .Returns(newerDatum);

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
            public void GivenADecimalSignalAndMonthDatumWithMissingDataAndFirstOrderMissingValuePolicy_WhenGettingData_FillsDataWhenThereIsNoNewerData()
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
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1),  Value = 2m},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 6, 1),  Value = 0m },
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 7, 1),  Value = 0m },
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 8, 1),  Value = 0m }
                };

                var olderDatum = new Datum<decimal>[]
                {
                    new Datum<decimal>() {Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m }
                };
                
                var firstTimestamp = new DateTime(2000, 5, 1);
                var lastTimestamp = new DateTime(2000, 9, 1);

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<decimal>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<decimal>>>);
                
                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataOlderThan<decimal>(existingSignal, It.IsAny<DateTime>(), 1))
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
                    .Setup(sdrm => sdrm.GetData<double>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataNewerThan<double>(existingSignal, It.IsAny<DateTime>(), 1))
                    .Returns(newerDatum);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataOlderThan<double>(existingSignal, It.IsAny<DateTime>(), 1))
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
                    .Setup(sdrm => sdrm.GetData<int>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataNewerThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
                    .Returns(newerDatum);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataOlderThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
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

                signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1 ,1), new DateTime(2000, 2, 1));
            }

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
                    .Setup(sdrm => sdrm.GetData<int>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataNewerThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
                    .Returns(newerDatum);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataOlderThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
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
                    .Setup(sdrm => sdrm.GetData<int>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataNewerThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
                    .Returns(newerDatum);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataOlderThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
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
                    .Setup(sdrm => sdrm.GetData<int>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataNewerThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
                    .Returns(newerDatum);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataOlderThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
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
                    .Setup(sdrm => sdrm.GetData<int>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataNewerThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
                    .Returns(newerDatum);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataOlderThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
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
                    .Setup(sdrm => sdrm.GetData<int>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataNewerThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
                    .Returns(newerDatum);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataOlderThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
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
                    .Setup(sdrm => sdrm.GetData<int>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataNewerThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
                    .Returns(newerDatum);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataOlderThan<int>(existingSignal, It.IsAny<DateTime>(), 1))
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
            public void GivenAnExampleSignal_WhenGettingData_ForSpecificTimestamp_FirstOrderCorrectlyFillsMissingData()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Integer,
                    Granularity = Granularity.Month,
                    Path = Domain.Path.FromString("example/signal")
                };

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2018, 2, 1), Value = (int)10 },
                        new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2018, 4, 1), Value = (int)30 }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2018, 1, 1),  Value = (int)0 },
                        new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2018, 2, 1),  Value = (int)10 },
                        new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2018, 3, 1),  Value = (int)20 },
                        new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2018, 4, 1),  Value = (int)30 },
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2018, 5, 1),  Value = (int)0 },
                };

                var firstTimestamp = new DateTime(2018, 1, 1);
                var lastTimestamp = new DateTime(2018, 6, 1);

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<int>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataNewerThan<int>(existingSignal, new DateTime(2018, 3, 1), 1))
                    .Returns(new Datum<int>[]
                        {
                            new Datum<int>() {Quality = Quality.Bad, Timestamp = new DateTime(2018, 3, 1), Value = 30 }
                        });

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataOlderThan<int>(existingSignal, new DateTime(2018, 3, 1), 1))
                    .Returns(new Datum<int>[]
                        {
                            new Datum<int>() {Quality = Quality.Bad, Timestamp = new DateTime(2018, 1, 1), Value = 10 }
                        });

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
            #endregion


            #region tests for GetData() method
            [TestMethod]
            public void GivenASignalAndData_WhenGettingDataFromEmptyRange_ReturnsEmptyResult()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Month
                });
                
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));
                
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(1, new DateTime(2000, 3 , 1), new DateTime(2000,1,1));

                Assert.AreEqual(0, result.Count());
            }

            [TestMethod]
            public void GivenASignalAndDatum_WhenGettingDataFromRangeThatIsMoreThanOneStepNewerThanExistingData_ReturnsCorrectlyPropagatedData()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Day
                };

                GivenASignal(existingSignal);

                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                var existingDatum = new Datum<double>[]
                {
                    new Datum<double>() {Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 }
                };

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetDataOlderThan<double>(existingSignal, new DateTime(2000, 1, 10), 1))
                    .Returns(existingDatum);

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(existingSignal, new DateTime(2000, 1, 10), new DateTime(2000, 1 ,11)))
                    .Returns(Enumerable.Empty<Datum<double>>);
                
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Get(existingSignal))
                    .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 10), new DateTime(2000, 1, 11)).ToArray().ToDomain<IEnumerable<Domain.Datum<double>>>();

                Assert.AreEqual(existingDatum.First().Quality, result.First().Quality);
                Assert.AreEqual(new DateTime(2000, 1 ,10), result.First().Timestamp);
                Assert.AreEqual(existingDatum.First().Value, result.First().Value);
            }
            #endregion

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

            private bool DataDtoCompareDouble(IEnumerable<Datum<double>> data)
            {
                var list = data.ToList();
                if (list[0].Value == 1 && list[1].Value == 2 & list[2].Value == 3)
                    return true;
                else
                    return false;
            }



            private void GiveNoSignalData()
            {
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalDomianService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalDomianService);


            }


            private void GivenNoSignals()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
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
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
        }
    }
}
