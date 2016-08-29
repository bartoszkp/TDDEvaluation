using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Domain.Exceptions;
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
            private SignalsDomainService signalDomainService;

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
            #region Add
            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsNotNull()
            {
                GivenNoSignals();
                var result = signalsWebService.Add(new Dto.Signal());
                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsTheSameSignalExceptForId()
            {
                GivenNoSignals();
                var signalDto = SignalWith(
                    dataType: Dto.DataType.Decimal,
                    granularity: Dto.Granularity.Week,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } });
                var result = signalsWebService.Add(signalDto);
                AssertDtoSignals(result, signalDto);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsIdFromRepository()
            {
                var signalId = 1;
                GivenNoSignals();
                GivenRepositoryThatAssigns(id: signalId);

                var result = signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Decimal,
                    granularity: Dto.Granularity.Week,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }));

                Assert.AreEqual(signalId, result.Id);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_PassesGivenSignalToRepositoryAdd()
            {
                GivenNoSignals();
                signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Decimal,
                    granularity: Dto.Granularity.Week,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }));

                signalsRepositoryMock.Verify(sr => sr.Add(It.Is<Domain.Signal>(passedSignal
                    => passedSignal.DataType == DataType.Decimal
                        && passedSignal.Granularity == Granularity.Week
                        && passedSignal.Path.ToString() == "root/signal")));
            }

            [TestMethod]
            [ExpectedException(typeof(IdNotNullException))]
            public void GivenASignal_WhenAddinASignalWithSameId_ThrowsException()
            {
                var existingSignal = new Signal()
                {
                    DataType = DataType.Double,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("root")
                };
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                GivenRepositoryThatAssigns(1);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);

                signalsWebService = new SignalsWebService(signalsDomainService);
                var result = signalsWebService.Add(existingSignal.ToDto<Dto.Signal>());
                signalsWebService.Add(new Dto.Signal()
                {
                    DataType = Dto.DataType.Double,
                    Granularity = Dto.Granularity.Day,
                    Id = result.Id
                });
            }

            #endregion

            #region Delete
            [TestMethod]
            public void GivenASignal_WhenDeletingSignal_RepositoryDeleteIsCalledWithGivenId()
            {
                int signalId = 1;
                var signal = new Domain.Signal
                {
                    Id = signalId
                };
                SetupDelete(signal);

                signalsWebService.Delete(signalId);

                signalsRepositoryMock
                    .Verify(sr => sr.Delete(It.Is<Signal>((s => s.Id == signalId))));
            }

            [TestMethod]
            public void GivenASignal_WhenDeletingSignal_DataRepositoryDeleteIsCalledWithGivenId()
            {
                int signalId = 46486;
                var signal = new Domain.Signal
                {
                    Id = signalId,
                    DataType = DataType.String
                };
                SetupDelete(signal);

                signalsWebService.Delete(signalId);

                signalsDataRepositoryMock
                    .Verify(sr => sr.DeleteData<string>(It.Is<Signal>((s => s.Id == signalId))));
            }
            #endregion

            #region GetById

            [TestMethod]
            public void GivenASignal_WhenGettingByItsId_ReturnsIt()
            {
                var signalId = 1;
                var signalDomain = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Year,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signalDomain);

                var result = signalsWebService.GetById(signalId);
                AssertDtoSignals(signalDomain.ToDto<Dto.Signal>(), result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_RepositoryGetIsCalledWithGivenId()
            {
                var signalId = 1;
                GivenNoSignals();
                signalsWebService.GetById(signalId);

                signalsRepositoryMock.Verify(sr => sr.Get(signalId));
            }


            [TestMethod]
            public void GivenNoSignals_WhenGettingByIdWithNullValue_ReturnsNull()
            {
                GivenNoSignals();
                signalsWebService = new SignalsWebService(signalDomainService);
                Assert.IsNull(signalsWebService.GetById(0));
            }
            #endregion

            #region SetData

            [TestMethod]
            public void GivenASignal_WhenSettingASignalsData_RepositorySetDataIsCalled()
            {
                MockBasicSetup();
                Datum<double>[] dataToSet = new Datum<double>[] {
                        new Datum<double>() { Id = 1, Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Datum<double>() { Id = 2, Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = (double)2 },
                        new Datum<double>() { Id = 3, Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)3 },
                        };
                signalDomainService.SetData(1, dataToSet);
                signalsDataRepositoryMock.Verify(sr => sr.SetData(dataToSet));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SettingNotExistingSignalDataException))]
            public void GivenASignal_WhenSettingDataForNotExistingSignal_ThrowsNotExistingSignalException()
            {
                MockBasicSetup();
                signalsWebService = new SignalsWebService(signalDomainService);

                Datum<double>[] dataToSet = new Datum<double>[] {
                        new Datum<double>() { Id = 1, Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Datum<double>() { Id = 2, Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = (double)2 },
                        new Datum<double>() { Id = 3, Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)3 },
                        };
                signalsWebService.SetData(100, dataToSet.ToDto<Dto.Datum[]>());
                signalDomainService.SetData(1, dataToSet);
                signalsDataRepositoryMock.Verify(sr => sr.SetData(dataToSet));
            }

            #endregion

            #region GetData
            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.GettingDataOfNotExistingSignal))]
            public void GivenASignal_WhenGettingDataForNotExistingSignal_ThrowsGettingDataOfNotExistingSignal()
            {
                MockBasicSetup();
                signalsWebService = new SignalsWebService(signalDomainService);
                var result = signalsWebService.GetData(1, new DateTime(2016, 8, 1), new DateTime(2016, 8, 4));
            }

            [TestMethod]
            public void GivenASignalAndSecondDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Second, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 3),  Value = (double)2.5 }
                };
                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 1, 1, 2),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 3),  Value = (double)2.5 }
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 4), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 4));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndMinuteDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Minute, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 0),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 3, 0),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 0),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 1, 2, 0),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 3, 0),  Value = (double)2.5 }
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1, 1, 1, 0), new DateTime(2000, 1, 1, 1, 4, 0), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1, 1, 1, 0), new DateTime(2000, 1, 1, 1, 4, 0));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndHourDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Hour, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 2, 0, 0),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0),  Value = (double)2.5 }
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1, 1, 0, 0), new DateTime(2000, 1, 1, 4, 0, 0), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1, 1, 0, 0), new DateTime(2000, 1, 1, 4, 0, 0));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndDayDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Day, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 2),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3),  Value = (double)2.5 }
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 1, 4), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 4));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndWeekDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Week, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 15),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2016, 8, 8),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 15),  Value = (double)2.5 }
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2016, 8, 1), new DateTime(2016, 8, 22), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2016, 8, 1), new DateTime(2016, 8, 22));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndYearDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Year, Path.FromString("root/signal1"));

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2001, 1, 1),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (double)2.5 }
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndMonthDatumWithMissingData_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0 }
                };
                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0},
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndDatum_WhenGettingDatumWithGranularityMonth_ReturnThisCollection()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2018, 1, 1,0,0,0), Value = (double)2.0 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2018, 2, 1,0,0,0), Value = (double)2.0  },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2018, 3, 1,0,0,0), Value = (double)2.0 }
                };

                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2018, 1, 1, 0, 0, 0), new DateTime(2018, 3, 1, 0, 0, 0), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());

                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2018, 1, 1, 0, 0, 0), new DateTime(2018, 3, 1, 0, 0, 0));
                for (int i = 0; i < 2; i++)
                {
                    Assert.AreEqual(result.ElementAt(i).Timestamp, existingDatum.ElementAt(i).Timestamp);
                    Assert.AreEqual(result.ElementAt(i).Quality, existingDatum.ElementAt(i).Quality);
                }
            }

            [TestMethod]
            public void GivenASignal_WhenGettingData_ReturnsCorrectData()
            {
                MockBasicSetup();
                List<Domain.Datum<int>> data = new List<Domain.Datum<int>>()
                {
                    new Datum<int>() {Quality = Domain.Quality.Fair,Timestamp = new DateTime(2000, 4, 4),Value = 12 },
                    new Datum<int>() {Quality = Domain.Quality.Good,Timestamp = new DateTime(2000, 3, 3),Value = 10 },
                    new Datum<int>() {Quality = Domain.Quality.Poor,Timestamp = new DateTime(2000, 2, 2),Value = 14 }
                };
                Signal signal = new Signal() { DataType = DataType.Integer, Granularity = Granularity.Day, Path = Path.FromString("example/path") };
                signalsDataRepositoryMock.Setup(srm => srm.GetData<int>(signal, new DateTime(2016, 8, 1), new DateTime(2016, 8, 4)))
                    .Returns(data);
                GivenRepositoryThatAssigns(1);

                var resultSignal = signalDomainService.Add(signal);

                signalsRepositoryMock.Setup(srm => srm.Get(resultSignal.Id.Value))
                    .Returns(signal);
                signalDomainService.SetData(1, data.AsEnumerable());
                data.RemoveAt(2);

                var result = signalDomainService.GetData<int>(resultSignal.Id.Value, new DateTime(2016, 8, 1), new DateTime(2016, 8, 4));
                CollectionAssert.AreEqual(data, result.ToList<Datum<int>>());
            }

            [TestMethod]
            public void GivenASignal_WhenGettingData_ReturnsCorrectDataWchihIsSorted()
            {
                MockBasicSetup();
                List<Domain.Datum<int>> data = new List<Domain.Datum<int>>()
                {
                    new Datum<int>() {Quality = Domain.Quality.Fair,Timestamp = new DateTime(2000, 4, 4),Value = 12 },
                    new Datum<int>() {Quality = Domain.Quality.Good,Timestamp = new DateTime(2000, 3, 3),Value = 10 },
                    new Datum<int>() {Quality = Domain.Quality.Poor,Timestamp = new DateTime(2000, 2, 2),Value = 14 }
                };
                Signal signal = new Signal() { DataType= DataType.Integer ,Granularity=Granularity.Day,Path=Path.FromString("example/path")}; 

                var sorted = data.OrderBy(d => d.Timestamp).ToList();

                signalsDataRepositoryMock.Setup(srm => srm.GetData<int>(signal, new DateTime(2000, 1, 2), new DateTime(2000, 5, 4)))
                    .Returns(sorted);
                GivenRepositoryThatAssigns(1);
                var resultSignal = signalDomainService.Add(signal);
                signalsRepositoryMock.Setup(srm => srm.Get(resultSignal.Id.Value))
                    .Returns(signal);

                signalDomainService.SetData(1, data.AsEnumerable());
                data.RemoveAt(2);
                sorted.RemoveAt(2);
                var result = signalDomainService.GetData<int>(resultSignal.Id.Value, new DateTime(2000, 1, 2), new DateTime(2000, 5, 4));
                CollectionAssert.AreEqual(sorted, result.ToList<Datum<int>>());
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenGettingDataQueriesWithIncorectTimestamp_CallException()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Day, Path.FromString("root/signal1"));

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1,0,0,0),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2,0,0,0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 3,0,0,0),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 3), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 3));
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenGettingDataQueriesWithIncorectTimestampSecond_CallException()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Day, Path.FromString("root/signal1"));

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1,0,0,0),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2,0,0,0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 3,0,0,0),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 1, 3, 1, 1, 1), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 3, 1, 1, 1));
            }

            [TestMethod]
            public void GivenASignalAndDatum_WhenGettingSingleData_ReturnsThisData()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double
                };

                List<Datum<double>> datumList = new List<Datum<double>> { new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 } };
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                GivenASignal(existingSignal);
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                DateTime date = new DateTime(2000, 1, 1);
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), date, date))
                    .Returns(datumList.AsEnumerable<Datum<double>>);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(1, date, date);
                Assert.AreEqual(datumList.First().ToDto<Dto.Datum>().Quality, result.First().Quality);
                Assert.AreEqual(datumList.First().ToDto<Dto.Datum>().Timestamp, result.First().Timestamp);
                Assert.AreEqual(datumList.First().ToDto<Dto.Datum>().Value, result.First().Value);
            }


            [TestMethod]
            public void GivenASignalAndDatum_WhenGettingDatumFromRangeInWhichLastTimestampIsLessThanFirstTimestamp_ReturnsEmptyCollection()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 4, 1), new DateTime(2000, 1, 1), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 4, 1), new DateTime(2000, 1, 1));
                Assert.IsTrue(result.Count() == 0);
            }

            [TestMethod]
            public void GivenASignalAndDatum_WhenGettingDataWithInTheSameTimeStamp_ReturnOneSample()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 2, 1), new DateTime(2000, 2, 1), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 2, 1), new DateTime(2000, 2, 1));
                Assert.AreEqual(result.ElementAt(0).Quality, Dto.Quality.Good);
                Assert.AreEqual(result.ElementAt(0).Timestamp, new DateTime(2000, 2, 1));
                Assert.AreEqual(result.ElementAt(0).Value, (double)2.0);
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenGettingDataQueriesWithIncorrectWithGranularitySeconds_CallException()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Second, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1,12,0,0,1),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,12,0,13,1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1,12,0,14,0),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenGettingDataQueriesWithIncorrectWithGranularityMinutes_CallException()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Minute, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1,12,45,0),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1,13,13,13),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1,14,14,14,10),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenGettingDataQueriesWithIncorrectWithGranularityHours_CallException()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Hour, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1,12,0,0),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1,13,13,0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1,14,14,0,10),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenGettingDataQueriesWithIncorrectWithGranularityDays_CallException()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Day, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1,0,0,0),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2,13,13,0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 3,14,14,0,10),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenGettingDataQueriesWithIncorrectWithGranularityWeeks_CallException()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Week, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2016, 8, 22,0,0,0),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 29,13,13,0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2016, 9, 5,14,14,0,10),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2016, 8, 22), new DateTime(2016, 9, 5), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2016, 8, 22), new DateTime(2016, 9, 5));
            }


            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenGettingDataQueriesWithIncorrectWithGranularityMonths_CallException()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2016, 1, 22),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 2, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2016, 3, 5,14,14,0,10),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2016, 1, 1), new DateTime(2016, 3, 1), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2016, 1, 1), new DateTime(2016, 3, 1));
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenGettingDataQueriesWithIncorrectWithGranularityYears_CallException()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Year, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2016, 1, 1),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2017, 2, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2018, 3, 5,14,14,0,10),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2016, 1, 1), new DateTime(2018, 1, 1), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2016, 1, 1), new DateTime(2018, 1, 1));
            }


            [TestMethod]
            public void GivenASignalAndDatumWithMissingDataAndZeroOrderValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0 },
                        new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 4, 1),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 4, 1),  Value = (double)2.5 }
                };

                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndSecondDatumWithMissingDataAndZeroOrderValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Second, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 2),  Value = (double)2.0 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 4),  Value = (double)3.0}
                };
                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1), Value=(double)0 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 2),  Value = (double)2.0 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 3),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 4),  Value = (double)3.0}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 5), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 5));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndMinuteDatumWithMissingDataAndZeroOrderValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Minute, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 0,0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 2, 0,0),  Value = (double)2.0 }
                };
                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 0,0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 2, 0,0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 3, 0,0),  Value = (double)2.0}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1, 1, 1, 0), new DateTime(2000, 1, 1, 1, 4, 0), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1, 1, 1, 0), new DateTime(2000, 1, 1, 1, 4, 0));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndHourDatumWithMissingDataAndZeroOrderValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Hour, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 2, 0, 0),  Value = (double)2.0 }
                };
                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 2, 0, 0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0),  Value = (double)2.0}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1, 1, 0, 0), new DateTime(2000, 1, 1, 4, 0, 0), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1, 1, 0, 0), new DateTime(2000, 1, 1, 4, 0, 0));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndDayDatumWithMissingDataAndZeroOrderValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Day, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0),  Value = (double)2.0 }
                };
                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0),  Value = (double)2.0}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 1, 4), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 4));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndWeekDatumWithMissingDataAndZeroOrderValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Week, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1, 0,0,0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 8, 0,0,0),  Value = (double)2.0 }
                };
                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1, 0,0,0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 8,0,0,0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 15, 0,0,0),  Value = (double)2.0}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2016, 8, 1, 0, 0, 0), new DateTime(2016, 8, 22, 0, 0, 0), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2016, 8, 1, 0, 0, 0), new DateTime(2016, 8, 22, 0, 0, 0));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndMonthDatumWithMissingDataAndZeroOrderValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0 }
                };
                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1),  Value = (double)2.0}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndYearsDatumWithMissingDataAndZeroOrderValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Year, Domain.Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (double)2.0 }
                };
                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (double)2.0}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingDataWithNullValue_RepositorySetShouldBeCalledWithDataWithNullValue()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                GivenASignal(new Signal() { Id = 1, DataType = DataType.String });
                var existingDatum = new Datum<string>[] { new Datum<string>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = null } };

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<string>(It.IsAny<IEnumerable<Datum<string>>>()));
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);
                signalsWebService.SetData(1, existingDatum.ToDto<IEnumerable<Dto.Datum>>());
                signalsDataRepositoryMock
                    .Verify(sdrm => sdrm.SetData<string>(It.Is<IEnumerable<Domain.Datum<string>>>(passedData =>
                    (
                        passedData.First().Quality == existingDatum.First().Quality
                        && passedData.First().Value == existingDatum.First().Value
                        && passedData.First().Timestamp == existingDatum.First().Timestamp
                    ))));
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenSettingDataQueriesWithIncorrectWithGranularitySeconds_CallException()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                GivenASignal(new Signal() { Id = 1, DataType = DataType.String, Granularity = Granularity.Second });
                var existingDatum = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1, 1), Value = null } };
                SetupSetData(existingDatum);
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenSettingDataQueriesWithIncorrectWithGranularityMinutes_CallException()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                GivenASignal(new Signal() { Id = 1, DataType = DataType.String, Granularity = Granularity.Minute });
                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,1,1,0), Value = null },
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,1,2,1), Value = null }
                };
                SetupSetData(existingDatum);
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenSettingDataQueriesWithIncorrectWithGranularityHours_CallException()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                GivenASignal(new Signal() { Id = 1, DataType = DataType.String, Granularity = Granularity.Hour });
                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,1,0,0), Value = null },
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,2,2,1), Value = null }
                };
                SetupSetData(existingDatum);
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenSettingDataQueriesWithIncorrectWithGranularityDays_CallException()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                GivenASignal(new Signal() { Id = 1, DataType = DataType.String, Granularity = Granularity.Day });
                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,0,0,0), Value = null },
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2,2,2,1), Value = null }
                };
                SetupSetData(existingDatum);
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenSettingDataQueriesWithIncorrectWithGranularityWeeks_CallException()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                GivenASignal(new Signal() { Id = 1, DataType = DataType.String, Granularity = Granularity.Week });
                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1,0,0,0), Value = null },
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 8,2,2,1), Value = null }
                };
                SetupSetData(existingDatum);
            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenSettingDataQueriesWithIncorrectWithGranularityMonths_CallException()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                GivenASignal(new Signal() { Id = 1, DataType = DataType.String, Granularity = Granularity.Day });
                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1,0,0,0), Value = null },
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 9, 8,2,2,1), Value = null }
                };
                SetupSetData(existingDatum);

            }

            [TestMethod]
            [ExpectedException(typeof(TimestampHaveWrongFormatException))]
            public void GivenASignalAndDatum_WhenSettingDataQueriesWithIncorrectWithGranularityYears_CallException()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                GivenASignal(new Signal() { Id = 1, DataType = DataType.String, Granularity = Granularity.Day });
                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 1, 1,0,0,0), Value = null },
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2017, 9, 8,2,2,1), Value = null }
                };
                SetupSetData(existingDatum);
            }

            #endregion

            #region Get
            [TestMethod]
            public void GivenNoSignal_WhenGettingByPath_ReturnsNull()
            {
                MockBasicSetup();
                signalsWebService = new SignalsWebService(signalDomainService);
                var result = signalsWebService.Get(new Dto.Path()
                {
                    Components = new[] { "not/existing/path" },
                });
                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByPath_ReturnsCorrectSignal()
            {
                Dto.Path dtoPath = new Dto.Path() { Components = new[] { "example", "path" } };
                GetByPathSetup();
                var returndSignal = signalsWebService.Get(dtoPath);
                AssertDtoSignals(returndSignal, new Dto.Signal() { Id = 1, DataType = Dto.DataType.Boolean, Granularity = Dto.Granularity.Day, Path = dtoPath });
            }
            #endregion

            #region policy

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_NoneQualityMissingValuePolicyIsSetAsDeafult()
            {
                var existingSignal = new Domain.Signal() { DataType = DataType.Double, Granularity = Granularity.Day };
                SetupSettingNoneQualityMissingValuePolicy(existingSignal);
                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>()));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingAnIntSignal_NoneQualityMissingValuePolicyIsSetAsDeafult()
            {
                var existingSignal = new Domain.Signal() { DataType = DataType.Integer, Granularity = Granularity.Day };
                SetupSettingNoneQualityMissingValuePolicy(existingSignal);
                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>()));
            }


            [TestMethod]
            public void GivenNoSignals_WhenAddingADecimalSignal_NoneQualityMissingValuePolicyIsSetAsDeafult()
            {
                var existingSignal = new Domain.Signal() { DataType = DataType.Decimal, Granularity = Granularity.Day };
                SetupSettingNoneQualityMissingValuePolicy(existingSignal);
                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<decimal>>()));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingABooleanSignal_NoneQualityMissingValuePolicyIsSetAsDeafult()
            {
                var existingSignal = new Domain.Signal() { DataType = DataType.Boolean, Granularity = Granularity.Day };
                SetupSettingNoneQualityMissingValuePolicy(existingSignal);
                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<bool>>()));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingAStringSignal_NoneQualityMissingValuePolicyIsSetAsDeafult()
            {
                var existingSignal = new Domain.Signal() { DataType = DataType.String, Granularity = Granularity.Day };
                SetupSettingNoneQualityMissingValuePolicy(existingSignal);
                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<string>>()));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SettingPolicyNotExistingSignalException))]
            public void GivenNoSignal_WhenSetPolicyIsCalled_ItThrowsException()
            {
                Mock<Dto.MissingValuePolicy.MissingValuePolicy> policyMock = new Mock<Dto.MissingValuePolicy.MissingValuePolicy>();
                SetupMissingValuePolicy(policyMock);

                Mock<ISignalsWebService> signalWebServiceMock = new Mock<ISignalsWebService>();
                signalWebServiceMock
                    .Setup(swsm => swsm.SetMissingValuePolicy(1, policyMock.Object));

                MockBasicSetup();
                signalsWebService = new SignalsWebService(signalDomainService);
                signalsWebService.SetMissingValuePolicy(1, policyMock.Object);

                signalWebServiceMock.Verify(swsm => swsm.SetMissingValuePolicy(1, policyMock.Object));
            }

            [TestMethod]
            public void GivenASignalAndPolicy_WhenSettingMissingValuePolicy_RepositorySetIsCalledWithProperArguments()
            {
                var existingSignal = new Domain.Signal() { Id = 1, DataType = DataType.Double, Granularity = Granularity.Day };
                var existingPolicy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
                {
                    DataType = Dto.DataType.Double,
                    Quality = Dto.Quality.Bad,
                    Value = (double)1.5
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock.Setup(srm => srm.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetMissingValuePolicy(existingSignal.Id.Value, existingPolicy);
                var domainExistingPolicy = (Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>)existingPolicy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.Is<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>(mv =>
                    (
                        mv.NativeDataType == domainExistingPolicy.NativeDataType
                        && mv.Quality == domainExistingPolicy.Quality
                        && mv.Value == domainExistingPolicy.Value
                    ))));
            }

            [TestMethod]
            public void GivenASignalAndPolicy_WhenGettingMissingValuePolicy_ReturnsThisPolicy()
            {
                var existingSignal = SignalWith(2, DataType.Double, Granularity.Day, Path.FromString("example/path"));
                var existingPolicy = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                {
                    Id = 1,
                    Quality = Domain.Quality.Bad,
                    Value = (double)1.5
                };

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock.Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                    .Returns(existingPolicy);

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
                var result = signalsWebService.GetMissingValuePolicy(existingSignal.Id.Value).ToDomain<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>();

                Assert.AreEqual(existingPolicy.Id, result.Id);
                Assert.AreEqual(existingPolicy.Quality, result.Quality);
                Assert.AreEqual(existingPolicy.Value, result.Value);
            }


            [TestMethod]
            public void GivenASignalAndDatumWithMissingDataAndSpecificValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0 },
                        new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 4, 1),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1),  Value = (double)42.42},
                        new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 4, 1),  Value = (double)2.5 }
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1), new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble() { Value = 42.42, Quality = Quality.Good });
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndSecondDatumWithMissingDataAndSpecificValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Second, Path.FromString("root/signal1"));

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 2),  Value = (double)2.0 }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 2),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 3),  Value = (double)42.42}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 4), new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble() { Value = 42.42, Quality = Quality.Good });
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 4));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndSingleDatumInMiddleOfTheRange_WhenGettingDataFromTheSpecifiedRange_ReturnsFilledData()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0 }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1),  Value = default(double)}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1), new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndMinuteDatumWithMissingDataAndSpecificValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Minute, Path.FromString("root/signal1"));

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 0,0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 2, 0,0),  Value = (double)2.0 }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 0,0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 2, 0,0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 3, 0,0),  Value = (double)42.42}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1, 1, 1, 0), new DateTime(2000, 1, 1, 1, 4, 0), new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble() { Value = 42.42, Quality = Quality.Good });
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1, 1, 1, 0), new DateTime(2000, 1, 1, 1, 4, 0));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndHourDatumWithMissingDataAndSpecificValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Hour, Path.FromString("root/signal1"));

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 2, 0, 0),  Value = (double)2.0 }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 2, 0, 0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0),  Value = (double)42.42}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1, 1, 0, 0), new DateTime(2000, 1, 1, 4, 0, 0), new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble() { Value = 42.42, Quality = Quality.Good });
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1, 1, 0, 0), new DateTime(2000, 1, 1, 4, 0, 0));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndDayDatumWithMissingDataAndSpecificValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Day, Path.FromString("root/signal1"));

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0),  Value = (double)2.0 }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0),  Value = (double)42.42}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 1, 4), new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble() { Value = 42.42, Quality = Quality.Good });
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 4));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndWeekDatumWithMissingDataAndSpecificValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Week, Path.FromString("root/signal1"));

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1, 0,0,0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 8, 0,0,0),  Value = (double)2.0 }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1, 0,0,0),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 8,0,0,0),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 15, 0,0,0),  Value = (double)42.42}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2016, 8, 1, 0, 0, 0), new DateTime(2016, 8, 22, 0, 0, 0), new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble() { Value = 42.42, Quality = Quality.Good });
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2016, 8, 1, 0, 0, 0), new DateTime(2016, 8, 22, 0, 0, 0));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndYearDatumWithMissingDataAndSpecificValueMissingValuePolicy_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = SignalWith(1, DataType.Double, Granularity.Year, Path.FromString("root/signal1"));

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (double)2.0 }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (double)42.42}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1), new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble() { Value = 42.42, Quality = Quality.Good });
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndDatumWithPolicyBoolean_WhenGettingData_ReturnThisDatum()
            {
                var existingSignal = SignalWith(1, DataType.Boolean, Granularity.Year, Domain.Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (bool)true },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (bool)false }
                };
                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (bool)true },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (bool)false},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (bool)false}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyBoolean());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndDatumWithPolicyDecimal_WhenGettingData_ReturnThisDatum()
            {
                var existingSignal = SignalWith(1, DataType.Decimal, Granularity.Year, Domain.Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (decimal)11 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (decimal)12 }
                };
                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (decimal)11 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (decimal)12},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (decimal)12}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndDatumWithPolicyInteger_WhenGettingData_ReturnThisDatum()
            {
                var existingSignal = SignalWith(1, DataType.Integer, Granularity.Year, Domain.Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (int)11 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (int)12 }
                };
                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (int)11 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (int)12},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (int)12}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndDatumWithPolicyString_WhenGettingData_ReturnThisDatum()
            {
                var existingSignal = SignalWith(1, DataType.String, Granularity.Year, Domain.Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (string)"11" },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (string)"12" }
                };
                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (string)"11" },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (string)"12"},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (string)"12"}
                };
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1), new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyString());
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1));
                AssertDatum(result, filledDatum);
            }

            [TestMethod]
            public void GivenASignalAndDatumWithZeroOrderMVP_WhenGettingData_MVPCorrectlyPropagateSamplesMoreThanOneStepOld()
            {
                var existingSignal = SignalWith(1, DataType.String, Granularity.Day, Domain.Path.FromString("root/signal1"));
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = "first" },
                };
                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 10),  Value = "first"},
                };
                var fromIncludedUtc = new DateTime(2000, 1, 10);
                var toExcludedUtc = new DateTime(2000, 1, 11);
                
                SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, fromIncludedUtc, toExcludedUtc, new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyString());
                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetDataOlderThan<string>(It.IsAny<Signal>(), fromIncludedUtc, 1))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<string>>>());
                var result = signalsWebService.GetData(existingSignal.Id.Value, fromIncludedUtc, toExcludedUtc);
                AssertDatum(result, filledDatum);
            }

            #endregion

            #region PathEntry
            [TestMethod]
            public void GetPathEntry_DoesNotThrow()
            {
                signalsWebService = new SignalsWebService(null);
                var result = signalsWebService.GetPathEntry(null);
                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenNoPath_WhenGettingPathEntry_RepositoryGetAllWithPathPrefixIsCalled()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock.Setup(srm => srm.GetAllWithPathPrefix(It.IsAny<Path>()));
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
                signalsWebService.GetPathEntry(null);
                signalsRepositoryMock.Verify(srm => srm.GetAllWithPathPrefix(It.IsAny<Path>()));
            }
            [TestMethod]
            public void GivenNoSignals_WhenGettingPathEntry_ReturnsNull()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.GetAllWithPathPrefix(It.IsAny<Path>()))
                    .Returns<Domain.Signal>(null);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
                var result = signalsWebService.GetPathEntry(null);
                Assert.IsNull(result);
            }
            [TestMethod]
            public void GivenListOfSignals_WhenGettingPathEntry_ReturnsPathWithCollectionOfSignalsFromMainDirectoryAndSubpathsFromMainDirectory()
            {
                List<Signal> signalsList = ConstListSignal();
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock.Setup(srm => srm.GetAllWithPathPrefix(It.IsAny<Path>()))
                    .Returns(signalsList.AsEnumerable);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "root" } });
                AssertResultAndPathEntry(result, signalsList);
            }
            #endregion


            private void SetupDelete (Signal signal)
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalDomainService);
                signalsRepositoryMock
                    .Setup(sr => sr.Delete(It.IsAny<Signal>()));
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.Is<int>(s => s == signal.Id.Value)))
                    .Returns(signal);
            }

            private void SetupSetData(Dto.Datum[] existingDatum)
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<string>(It.IsAny<IEnumerable<Datum<string>>>()));
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
                signalsWebService.SetData(1, existingDatum.ToDto<IEnumerable<Dto.Datum>>());
            }

            private void SetupSignalAndDatumWithPolicyMock(Signal existingSignal, Dto.Datum[] existingDatum,
                DateTime fromIncludedUtc, DateTime toExcludedUtc, Domain.MissingValuePolicy.MissingValuePolicyBase missingValuePolicyBase)
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                GivenASignal(existingSignal);
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                choiseDataType(existingSignal, existingDatum, fromIncludedUtc, toExcludedUtc);
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(missingValuePolicyBase);
            var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void choiseDataType(Signal existingSignal, Dto.Datum[] existingDatum, DateTime fromIncludedUtc, DateTime toExcludedUtc)
            {
                var choiseTypeOf =new Dictionary<DataType, Action>()
                {
                    {DataType.Boolean,()=> signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<bool>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<bool>>>)},
                    {DataType.Decimal,()=> signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<decimal>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<decimal>>>)},
                    {DataType.Double,()=> signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<double>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>)},
                    {DataType.Integer,()=> signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<int>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>)},
                    {DataType.String,()=> signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<string>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<string>>>)}
                };
                choiseTypeOf[existingSignal.DataType].Invoke();
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

            private Dto.Signal SignalWith(Dto.DataType dataType, Dto.Granularity granularity, Dto.Path path)
            {
                return new Dto.Signal()
                {
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }

            private Domain.Signal SignalWith(int id, Domain.DataType dataType, Domain.Granularity granularity, Domain.Path path)
            {
                return new Domain.Signal()
                {
                    Id = id,
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
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

            private void GivenRepositoryThatAssigns(int id)
            {
                signalsRepositoryMock.Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s =>
                    {
                        s.Id = id;
                        return s;
                    });
            }
           
            private void SetupMissingValuePolicy(Mock<Dto.MissingValuePolicy.MissingValuePolicy> policyMock)
            {
                policyMock.Object.DataType = Dto.DataType.Boolean;
                policyMock.Object.Id = 1;
                policyMock.Object.Signal = new Dto.Signal()
                {
                    Id = 1,
                    DataType = Dto.DataType.Boolean,
                    Granularity = Dto.Granularity.Day,
                    Path = new Dto.Path() { Components = new[] { "aaa", "bbb" } },
                };
            }

            private void GetByPathSetup()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock.Setup(srm => srm.Get(Domain.Path.FromString("example/path")))
                    .Returns(new Signal()
                    {
                        Id = 1,
                        DataType = Domain.DataType.Boolean,
                        Granularity = Domain.Granularity.Day,
                        Path = Domain.Path.FromString("example/path"),
                    });
                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                signalsWebService = new SignalsWebService(signalDomainService);
            }

            private void MockBasicSetup()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
            }

            private List<Signal> ConstListSignal()
            {
                return new List<Signal>()
                {
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/s1")},
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("s0") },
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/sub/s2") },
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/sub/s3") },
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/subsub/s4") },
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/sub/s5") }
                };
            }

            private void AssertResultAndPathEntry(Dto.PathEntry result, List<Signal> signalsList)
            {
                Assert.AreEqual(1, result.Signals.Count());
                Assert.AreEqual(signalsList.First().ToDto<Dto.Signal>().DataType, result.Signals.First().DataType);
                CollectionAssert.AreEqual(signalsList.First().ToDto<Dto.Signal>().Path.Components.ToArray(),
                    result.Signals.First().Path.Components.ToArray());

                Assert.AreEqual(2, result.SubPaths.Count());
                CollectionAssert.AreEqual(new[] { "root", "sub" }, result.SubPaths.First().Components.ToArray());
                CollectionAssert.AreEqual(new[] { "root", "subsub" }, result.SubPaths.Last().Components.ToArray());
            }

            private void SetupSettingNoneQualityMissingValuePolicy(Signal existingSignal)
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock.Setup(srm => srm.Add(It.IsAny<Domain.Signal>()))
                    .Returns(existingSignal);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var choiseDataType = new Dictionary<DataType, Action>()
                {
                    {DataType.Boolean, ()=>missingValuePolicyRepositoryMock
                        .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<bool>>()))},
                    {DataType.Decimal, ()=>missingValuePolicyRepositoryMock
                        .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<decimal>>()))},
                    {DataType.Double, ()=>missingValuePolicyRepositoryMock
                        .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>()))},
                    {DataType.Integer, ()=>missingValuePolicyRepositoryMock
                        .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>()))},
                    {DataType.String, ()=>missingValuePolicyRepositoryMock
                        .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<string>>()))},
                };
                choiseDataType[existingSignal.DataType].Invoke();

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
                signalsWebService.Add(existingSignal.ToDto<Dto.Signal>());

            }

            private void AssertDtoSignals(Dto.Signal result, Dto.Signal expectedSignal)
            {
                Assert.AreEqual(expectedSignal.DataType, result.DataType);
                Assert.AreEqual(expectedSignal.Granularity, result.Granularity);
                CollectionAssert.AreEqual(expectedSignal.Path.Components.ToArray(), result.Path.Components.ToArray());

            }
        }
    }
}