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
                    Id = 21243,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("root/signal1")
                };
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<int>()))
                    .Returns(newSignal);
                signalsWebService.SetData(newSignal.Id.Value, new Dto.Datum[0]);
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
            public void RepositoryWithSignalAndNoData_GetData_ReturnEmptyCollection()
            {
                GiveNoSignalData();

                signalsRepositoryMock.Setup(x => x.Get(It.IsAny<int>()))
                    .Returns(new Signal() { DataType = DataType.Double });

                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.IsAny<Signal>())).Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var item = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 2));

                Assert.IsTrue(item.Count() == 0);
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
                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.IsAny<Signal>())).Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());


                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, null);

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
                    .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())) ;
               
                GivenASignal(existingSignal);

                
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                     .Setup(mvp => mvp.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));
               


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
            public void GivenASignalAndDatum_WhenGetDataWithArgmunetsLessThanFrom_ReturnEmptyResult()
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
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1)},
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1)},
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1)}
                };
                signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));

                GivenASignal(existingSignal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 3, 1), new DateTime(2000, 1, 1));
                Assert.IsNull(result);
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
                GivenASignal(existingSignal);
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1, 0, 1, 0));
                Assert.AreEqual(60, result.Count());
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

            [TestMethod]
            public void GivenNoSignals_GetPathentry_RepoIsCalled()
            {
                GivenNoSignals();
                List<Signal> listSignal = new List<Signal>();
                listSignal.Add(new Signal() { Id = 1, DataType= DataType.Boolean,Granularity= Granularity.Minute,Path= Path.FromString("s0") });
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
 