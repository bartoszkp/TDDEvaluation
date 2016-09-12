using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System;
using DataAccess.GenericInstantiations;
using Domain.Exceptions;
using Dto.MissingValuePolicy;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsNotNull()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                var result = signalsWebService.Add(new Dto.Signal());

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsTheSameSignalExceptForId()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                var result = signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Decimal,
                    granularity: Dto.Granularity.Week,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }));

                Assert.AreEqual(Dto.DataType.Decimal, result.DataType);
                Assert.AreEqual(Dto.Granularity.Week, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_PassesGivenSignalToRepositoryAdd()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

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
            public void GivenNoSignals_WhenAddingASignal_ReturnsIdFromRepository()
            {
                var signalId = 1;
                GivenNoSignals_SetupSignalsRepositoryMock();
                GivenRepositoryThatAssigns(id: signalId);

                var result = signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Decimal,
                    granularity: Dto.Granularity.Week,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }));

                Assert.AreEqual(signalId, result.Id);
            }

            [TestMethod] 
            public void GivenNoSignals_WhenGettingById_DoesNotThrow()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                signalsWebService.GetById(0);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByItsId_ReturnsIt()
            {
                var signalId = 1;
                GivenASignal_SetupSignalsRepositoryMock(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Year,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.GetById(signalId);

                AssertSignalsAreEqual(result, new Dto.Signal()
                {
                    Id = signalId,
                    DataType = Dto.DataType.String,
                    Granularity = Dto.Granularity.Year,
                    Path = new Dto.Path()
                    {
                        Components = new[] {"root", "signal"}
                    }
                });
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_RepositoryGetIsCalledWithGivenId()
            {
                var signalId = 1;
                GivenNoSignals_SetupSignalsRepositoryMock();

                signalsWebService.GetById(signalId);

                signalsRepositoryMock.Verify(sr => sr.Get(signalId));
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_ReturnsNull()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                var result = signalsWebService.GetById(0);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingByPath_DoesNotThrowUnimplementedException()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<Domain.Path>())).Returns(new Domain.Signal());

                signalsWebService.Get(new Dto.Path()
                {
                    Components = new[] { "root", "signal" }
                });
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingByPath_RepositoryGetIsCalledWithGivenPath()
            {
                var pathDto = new Dto.Path()
                {
                    Components = new[] { "root", "signal" }
                };

                var pathDomain = Domain.Path.FromString("root/signal");

                GivenNoSignals_SetupSignalsRepositoryMock();

                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<Domain.Path>())).Returns(new Domain.Signal());

                signalsWebService.Get(pathDto);

                signalsRepositoryMock.Verify(srm => srm.Get(pathDomain));
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByItsPath_ReturnsIt()
            {
                var pathDto = new Dto.Path()
                {
                    Components = new[] { "root", "signal" }
                };

                var pathDomain = Domain.Path.FromString("root/signal");

                GivenASignal_SetupSignalsRepositoryMock(SignalWith(
                    DataType.Double,
                    Granularity.Month, 
                    Path.FromString("root/signal"), 
                    1));

                var result = signalsWebService.Get(pathDto);

                AssertSignalsAreEqual(result, SignalWith(
                    Dto.DataType.Double, 
                    Dto.Granularity.Month, 
                    new Dto.Path() { Components = new[] { "root", "signal" } }, 
                    1));
            }

            [TestMethod]
            public void GivenANonExistingPath_WhenGettingByPath_ReturnsNull()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                var pathDto = new Dto.Path() { Components = new[] { "root1", "signal88" } };

                Assert.IsNull(signalsWebService.Get(pathDto));
            }

            [TestMethod]
            public void GivenNoSignals_WhenSettingMissingValuePolicy_DoesNotThrow()
            {
                SetupWebService();

                int signalId = 1;

                signalsRepositoryMock.Setup(srm => srm.Get(signalId)).Returns(new Domain.Signal()
                {
                    Id = signalId
                });

                signalsWebService.SetMissingValuePolicy(signalId, new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy());
            }

            [TestMethod]
            public void GivenNoSignals_WhenSettingMissingValuePolicy_RepositorySetIsCalledWithGivenPolicy()
            {
                SetupWebService();

                int signalId = 1;

                Dto.MissingValuePolicy.MissingValuePolicy mvp = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { 
                    Id = signalId,
                    DataType = Dto.DataType.Double,
                    Quality = Dto.Quality.Fair,
                    Value = (double)1.5
                };

                signalsRepositoryMock.Setup(srm => srm.Get(signalId)).Returns(new Domain.Signal()
                {
                    Id = signalId
                });

                signalsWebService.SetMissingValuePolicy(signalId, mvp);

                missingValuePolicyRepositoryMock.Verify(mvprm => mvprm.Set(It.Is<Domain.Signal>(s => s.Id == signalId), 
                    It.Is<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>(svm => 
                    svm.Id == signalId && 
                    svm.Quality == Quality.Fair && 
                    svm.Value == (double)1.5)));
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNonExistingSignalId_WhenSettingMissingValuePolicy_ThrowsCouldntGetASignalException()
            {
                SetupWebService();

                int nonExistingSignalId = 1;

                signalsRepositoryMock.Setup(srm => srm.Get(nonExistingSignalId)).Returns((Domain.Signal)null);

                signalsWebService.SetMissingValuePolicy(nonExistingSignalId, new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy());
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingMissingValuePolicy_DoesNotThrow()
            {
                SetupWebService();

                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Domain.Signal());

                signalsWebService.GetMissingValuePolicy(0);
            }

            [TestMethod]
            public void GivenSignalId_WhenGettingMissingValuePolicy_IdIsPassedToDomainToGetSignalById()
            {
                SetupWebService();
                int id = 1;

                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(new Domain.Signal());

                missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyInteger());

                signalsWebService.GetMissingValuePolicy(id);

                signalsRepositoryMock.Verify(srm => srm.Get(id), Times.Once);
            }

            [TestMethod]
            public void GivenASignalId_WhenGettingMissingValuePolicy_RepositoryGetIsCalledWithGivenSignal()
            {
                SetupWebService();
                int id = 1;

                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(new Domain.Signal()
                {
                    Id = id
                });

                missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>())).Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyInteger());

                signalsWebService.GetMissingValuePolicy(id);

                missingValuePolicyRepositoryMock.Verify(mvprm => mvprm.Get(It.Is<Domain.Signal>(s => s.Id == id)), Times.Once);
            }

            [TestMethod]
            public void GivenASignalId_WhenGettingMissingValuePolicy_ReturnsCorrectPolicy()
            {
                SetupWebService();
                int id = 5;

                var signal = SignalWith(DataType.Integer, Granularity.Week, Path.FromString("root/signal"), id);

                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(signal);

                var expectedPolicy = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyInteger()
                {
                    Id = id,
                    Quality = Quality.Good,
                    Signal = signal,
                    Value = (int)2
                };

                missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>())).Returns(expectedPolicy);

                var result = signalsWebService.GetMissingValuePolicy(id);

                Assert.AreEqual(result.Id, id);
                AssertSignalsAreEqual(result.Signal, SignalWith(
                    Dto.DataType.Integer, 
                    Dto.Granularity.Week, 
                    new Dto.Path()
                    {
                        Components = new[] { "root", "signal" }
                    }, 
                    id));
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNonExistingId_WhenGettingMissingValuePolicy_ThrowsCouldntGetASignalException()
            {
                SetupWebService();

                int nonExistingId = 4;

                signalsRepositoryMock.Setup(srm => srm.Get(nonExistingId)).Returns((Domain.Signal)null);

                signalsWebService.GetMissingValuePolicy(nonExistingId);
            }

            [TestMethod]
            public void GivenNoData_WhenSettingData_CallsGetByIdWithPassedId()
            {
                SetupWebService();

                int id = 1;
                var dataDto = new Dto.Datum[] {  };

                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Signal());

                signalsWebService.SetData(id, dataDto);

                signalsRepositoryMock.Verify(srm => srm.Get(id), Times.Once);
            }

            [TestMethod]
            public void GivenData_WhenSettingData_RepositorySetIsCalledWithCorrectData()
            {
                SetupWebService();
                int id = 4;

                var dataDto = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good,
                    Timestamp = new System.DateTime(2000, 1, 1), Value = (int)2 } };

                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Signal()
                {
                    DataType = DataType.Integer
                });

                signalsWebService.SetData(id, dataDto);

                signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<int>(It.Is<IEnumerable<Datum<int>>>(data => 
                    data.ElementAt(0).Quality == Quality.Good &&
                    data.ElementAt(0).Timestamp == new System.DateTime(2000, 1, 1) &&
                    data.ElementAt(0).Value == 2)), 
                    Times.Once);
            }

            [TestMethod]
            public void GivenData_WhenSettingData_SignalFoundByGetByIdIsAssignedToDataPassedToRepository()
            {
                SetupWebService();
                int id = 4;

                var dataDto = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good,
                    Timestamp = new System.DateTime(2000, 1, 1), Value = (int)2 } };

                var signalDomain = SignalWith(DataType.Integer, Granularity.Day, Path.FromString("root/signal"), id);

                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(signalDomain);

                signalsWebService.SetData(id, dataDto);

                VerifySetDataCallOnSignalsDataRepositoryMock<int>(signalDomain, new System.DateTime(2000, 1, 1), 2);
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNonExistingSignalId_WhenSettingData_ThrowsCouldntGetASignalException()
            {
                SetupWebService();
                int nonExistingId = 6;

                var dataDto = new Dto.Datum[] { };

                signalsRepositoryMock.Setup(srm => srm.Get(nonExistingId)).Returns((Domain.Signal)null);

                signalsWebService.SetData(nonExistingId, dataDto);
            }

            [TestMethod]
            public void GivenDataOfTypeDouble_WhenSettingData_RepositorySetDataIsCalledWithCorrectDataType()
            {
                SetupWebService();
                int id = 1;

                var dataDtoDouble = new Dto.Datum[] { new Dto.Datum() {
                    Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = (double)2.5 } };

                var signalDomainDouble = SignalWith(DataType.Double, Granularity.Day, Path.FromString("root/signal"), id);

                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(signalDomainDouble);

                signalsWebService.SetData(id, dataDtoDouble);

                VerifySetDataCallOnSignalsDataRepositoryMock<double>(signalDomainDouble, new System.DateTime(2000, 1, 1), 2.5);
            }

            [TestMethod]
            public void GivenDataOfDifferentTypes_WhenSettingData_RepositorySetDataIsCalledWithCorrectDataType()
            {
                SetupWebService();
                int id = 1;
                DateTime timeStamp = new DateTime(2000, 1, 1);

                var data = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = timeStamp, Value = (decimal)2.5m } };
                var signal = SignalWith(DataType.Decimal, Granularity.Day, Path.FromString("root/signal"), id);
                GivenASignalAndData_SetupSignalsRepositoryMockAndVerifySetDataCall<decimal>(signal, data, timeStamp, 2.5m);

                data[0].Value= true;
                signal.DataType = DataType.Boolean;
                GivenASignalAndData_SetupSignalsRepositoryMockAndVerifySetDataCall<bool>(signal, data, timeStamp, true);

                data[0].Value = "aa" ;
                signal.DataType = DataType.String;
                GivenASignalAndData_SetupSignalsRepositoryMockAndVerifySetDataCall<string>(signal, data, timeStamp, "aa");
            }

            [TestMethod]
            public void GivenDataAndSignal_WhenSetData_VerifyDataIsSortedAscending()
            {
                int signalId = 3;

                Dto.Datum[] settedData = new Dto.Datum[]{
                     new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 5, 1), Value = (int) 5},
                     new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = (int) 4},
                     new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int) 2},
                     new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (int) 1},
                     new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (int) 3} };

                SetupWebService();

                signalsRepositoryMock.Setup(srm => srm.Get(It.Is<int>(id => id == signalId)))
                    .Returns(new Domain.Signal() { DataType = DataType.Integer });

                signalsWebService.SetData(signalId, settedData);

                signalsDataRepositoryMock.Verify(sdr => sdr.SetData<int>(It.Is<IEnumerable<Datum<int>>>( d => DatumsAreAscending(d))));            
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void SetData_PassDataWithIncorrectTimestampsForMonth_ThrowException()
            {
                int signalId = 1;
                var data = new Dto.Datum[]
                {
                   new Dto.Datum() { Quality = Dto.Quality.Bad, Value = 0, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0,0) }
                };
                SetupWebService();

                signalsRepositoryMock.Setup(x => x.Get(It.Is<int>(id => id == signalId)))
                .Returns(new Domain.Signal() { Id = signalId, DataType = DataType.Integer, Granularity = Granularity.Month });

                signalsWebService.SetData(signalId, data);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void SetData_PassDataWithIncorrectTimepstampsForSecond_ThrowException()
            {
                int signalId = 1;
                var data = new Dto.Datum[]
                {
                   new Dto.Datum() { Quality = Dto.Quality.Bad, Value = 0, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0,1) }
                };
                SetupWebService();

                signalsRepositoryMock.Setup(x => x.Get(It.Is<int>(id => id == signalId)))
                .Returns(new Domain.Signal() { Id = signalId, DataType = DataType.Integer, Granularity = Granularity.Second });

                signalsWebService.SetData(signalId, data);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void SetData_PassDataWithIncorrectTimepstampsForYear_ThrowException()
            {
                int signalId = 1;
                var data = new Dto.Datum[]
                {
                   new Dto.Datum() { Quality = Dto.Quality.Bad, Value = 0, Timestamp = new DateTime(2000,2, 1, 0, 0, 0,0) }
                };
                SetupWebService();

                signalsRepositoryMock.Setup(x => x.Get(It.Is<int>(id => id == signalId)))
                .Returns(new Domain.Signal() { Id = signalId, DataType = DataType.Integer, Granularity = Granularity.Year });

                signalsWebService.SetData(signalId, data);
            }


            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GetData_PassIncorrectTimestampForMonth_ThrowException()
            {
                int signalId = 1;
                SetupWebService();

                signalsRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Domain.Signal() { Id = signalId, Granularity = Granularity.Month });

                signalsWebService.GetData(1, new DateTime(2000, 2, 2, 0, 0, 0), new DateTime(2000, 5, 1, 0, 0, 0));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GetData_PassIncorrectTimestampForWeek_ThrowException()
            {
                int signalId = 1;
                SetupWebService();

                signalsRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Domain.Signal() { Id = signalId, Granularity = Granularity.Week });

                signalsWebService.GetData(1, new DateTime(2016, 8, 23, 0, 0, 0,0), new DateTime(2016, 8, 22, 0, 0, 0));
            }


            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GetData_PassInCorrectTimestampForMinute_ThrowException()
            {
                int signalId = 1;
                SetupWebService();

                signalsRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Domain.Signal() { Id = signalId, Granularity = Granularity.Minute });

                signalsWebService.GetData(1, new DateTime(2016, 8, 23, 1, 1,1), new DateTime(2016, 8, 25, 0, 0, 0));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GetData_PassInCorrectTimestampForHour_ThrowException()
            {
                int signalId = 1;
                SetupWebService();

                signalsRepositoryMock.Setup(x => x.Get(It.IsAny<int>())).Returns(new Domain.Signal() { Id = signalId, Granularity = Granularity.Hour });

                signalsWebService.GetData(1, new DateTime(2016, 8, 23, 1, 1, 0), new DateTime(2016, 8, 25, 0, 0, 0));
            }

            [TestMethod]
            public void GivenSignalId_WhenGettingData_CallsGetByIdWithPassedId()
            {
                SetupWebService();
                int id = 3;

                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Domain.Signal() { Id = id });

                signalsWebService.GetData(id, new DateTime(), new DateTime());

                signalsRepositoryMock.Verify(srm => srm.Get(id));
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNonExistingId_WhenGettingData_ThrowsCouldntGetASignalException()
            {
                SetupWebService();
                int nonExistingId = 12;

                signalsRepositoryMock.Setup(srm => srm.Get(nonExistingId)).Returns((Domain.Signal)null);

                signalsWebService.GetData(nonExistingId, new DateTime(), new DateTime());
            }

            [TestMethod]
            public void GivenFromDateLaterThanToDate_WhenGettingData_ReturnsEmtptyDataScope()
            {
                int signalId = 5;

                SetupWebService();
                DateTime invalidFromDate = new DateTime(2001, 2, 1);
                DateTime invalidToDate = new DateTime(2000, 1, 1);

                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Domain.Signal() { Id = signalId });

                var result = signalsWebService.GetData(signalId, invalidFromDate, invalidToDate);

                Assert.IsTrue(result.Count() == 0);
            }

            [TestMethod]
            public void GivenIdMatchingSignalOfDataTypeInteger_WhenGettingData_RepositoryGetDataIsCalledWithCorrectDataType()
            {
                SetupWebService();
                int id = 5;

                var signal = SignalWith(DataType.Integer, Granularity.Month, Path.FromString("root/signalInt"), id);

                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(signal);

                signalsWebService.GetData(id, new DateTime(), new DateTime());

                VerifyGetDataCallOnSignalsDataRepositoryMock<int>(signal, new DateTime(), new DateTime());
            }

            [TestMethod]
            public void GivenSignalsOfDifferentDataTypes_WhenGettingData_RepositoryGetDataIsCalledWithCorrectDataType()
            {
                SetupWebService();
                int id = 2;

                DateTime dateFrom = new DateTime(2000, 1, 1);
                DateTime dateTo = new DateTime(2000, 1, 5);

                SetupMock_missingValuePolicy_DefaultMissingValuePolicy();

                var signal = SignalWith(DataType.Double, Granularity.Day, Path.FromString("root/signal"), id);
                GivenASignal_SetupSignalsRepositoryMockAndVerifyGetDataCall<double>(signal, dateFrom, dateTo);

                signal.DataType = DataType.Decimal;
                GivenASignal_SetupSignalsRepositoryMockAndVerifyGetDataCall<decimal>(signal, dateFrom, dateTo);

                signal.DataType = DataType.Boolean;
                GivenASignal_SetupSignalsRepositoryMockAndVerifyGetDataCall<bool>(signal, dateFrom, dateTo);

                signal.DataType = DataType.String;
                GivenASignal_SetupSignalsRepositoryMockAndVerifyGetDataCall<string>(signal, dateFrom, dateTo);
            }

            [TestMethod]
            public void GivenASignalIdMatchingSignalOfDataTypeInteger_WhenGettingData_ReturnsExpectedData()
            {
                SetupWebService();
                int id = 6;

                DateTime dateFrom = new DateTime(2000, 1, 1);
                DateTime dateTo = new DateTime(2000, 1, 3);

                var signal = SignalWith(DataType.Integer, Granularity.Day, Path.FromString("root/signalInt"), id);

                var expectedData = new Dto.Datum[] 
                {
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = (int)2 },
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 1, 2), Value = (int)3 }
                };

                var dataReturned = new Domain.Datum<int>[]
                {
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (int)2 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 2), Value = (int)3 }
                };

                SetupRepositoryMocks_GetData_ReturnsGivenDataCollection<int>(id, signal, dateFrom, dateTo, dataReturned);

                var result = signalsWebService.GetData(id, dateFrom, dateTo);

                DatumArraysAreEqual(result.ToArray(), expectedData);
            }

            [TestMethod]
            public void GivenSignalIdsMatchingSignalOfDifferentDataTypes_WhenGettingData_ReturnsExpectedData()
            {
                SetupWebService();
                int id = 6;
                DateTime dateFrom = new DateTime(2000, 1, 1);
                DateTime dateTo = new DateTime(2000, 1, 2);

                var signal = SignalWith(DataType.Double, Granularity.Day, Path.FromString("root/signal"), id);
                var expectedData = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = (double)2.5 } };
                var dataReturnedDouble = new Domain.Datum<double>[] { new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)2.5 } };
                SetupRepositoryMocks_CallGetData_CompareReturnedData<double>(signal, expectedData, dataReturnedDouble, dateFrom, dateTo);

                signal.DataType = DataType.Decimal;
                expectedData[0].Value = (decimal)2.5m;
                var dataReturnedDecimal = new Domain.Datum<decimal>[] { new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (decimal)2.5m } };
                SetupRepositoryMocks_CallGetData_CompareReturnedData<decimal>(signal, expectedData, dataReturnedDecimal, dateFrom, dateTo);

                signal.DataType = DataType.Boolean;
                expectedData[0].Value = true;
                var dataReturnedBoolean = new Domain.Datum<bool>[] { new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = true } };
                SetupRepositoryMocks_CallGetData_CompareReturnedData<bool>(signal, expectedData, dataReturnedBoolean, dateFrom, dateTo);

                signal.DataType = DataType.String;
                expectedData[0].Value = "aa";
                var dataReturnedString = new Domain.Datum<string>[] { new Datum<string>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = "aa" } };
                SetupRepositoryMocks_CallGetData_CompareReturnedData<string>(signal, expectedData, dataReturnedString, dateFrom, dateTo);
            }

            [TestMethod]
            public void GivenASignal_ReturnsEmptySignal_ReturnsDefaultMissingValuePolicy()
            {
                int signalId = 4;
                SetupWebService();
        
                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Signal());

                var result = signalsWebService.GetMissingValuePolicy(signalId);

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGetsMissingValuePolicy_ReturnsSignalWithPolicy()
            {
                Dto.Signal addedSignal = new Dto.Signal()
                {
                    DataType = Dto.DataType.Boolean,
                    Granularity = Dto.Granularity.Month,
                    Path = new Dto.Path() { Components = new[] { "root", "signal" } }
                };

                GivenAnySignal_SetupSignalsRepositoryMock();

                Dto.Signal returnedSignal = signalsWebService.Add(addedSignal);

                var result = signalsWebService.GetMissingValuePolicy(returnedSignal.Id.Value);

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenSetsMissingValuePolicy_VerifyRepo_CheckIfWorksForAllTypes()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                signalsWebService.Add(new Dto.Signal()
                {
                    DataType = Dto.DataType.Double,
                    Granularity = Dto.Granularity.Hour,
                    Path = new Dto.Path() { Components = new[] { "root", "signal5" } }
                });

                missingValuePolicyRepositoryMock.Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(),
                    It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>()));
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingPathEntry_ReturnsEmptyPathEntry()
            {
                SetupWebService();

                Dto.Path path = new Dto.Path() { Components = new[] { "x","y" } };
                var result = this.signalsWebService.GetPathEntry(path);

                Assert.IsNotNull(result);

                if (result.GetType() != typeof(Dto.PathEntry))
                    Assert.Fail();
            }

            [TestMethod]
            public void GivenASignal_WhenGettingPathEntry_GettingPathEntryWithOneSingnal()
            {
                SetupWebService();

                Domain.Signal signal = GetDefaultSignal_IntegerMonth();
                Dto.Path path = new Dto.Path() { Components = new[] { "x", "y" } };

                SetupMock_GetAllWithPathPrefix(new[] { signal },path.ToDomain<Domain.Path>());
                var result = this.signalsWebService.GetPathEntry(path);

                int expectedElements = 1;
                Assert.AreEqual(expectedElements,result.Signals.ToArray().Length);
                AssertSignalsAreEqual(signal.ToDto<Dto.Signal>(),result.Signals.ElementAt(0));
            }

            [TestMethod]
            public void GivenASignal_WhenGettingPathEntry_GettingPathEntryWithOneSubPath()
            {
                SetupWebService();

                Domain.Signal signal = GetDefaultSignal_IntegerMonth();
                Dto.Path prefix = new Dto.Path() { Components = new[] { "x" } };

                SetupMock_GetAllWithPathPrefix(new[] { signal }, prefix.ToDomain<Domain.Path>());
                var result = this.signalsWebService.GetPathEntry(prefix);

                int expectedElements = 1;
                Assert.AreEqual(expectedElements, result.SubPaths.ToArray().Length);

                PathEntry expectedPathEntry = new PathEntry(null, new Domain.Path[] {
                    Domain.Path.FromString("x/y") } );
                CollectionAssert.AreEqual(expectedPathEntry.SubPaths.ElementAt(0).Components.ToArray(), 
                    result.SubPaths.ElementAt(0).Components.ToArray());
            }

            [TestMethod]
            public void GivenASignals_WhenGettingPathEntry_GettingPathEntryWithManySignalsAndSubPaths()
            {
                SetupWebService();

                Dto.Path prefix = new Dto.Path() { Components = new[] { "x" } };

                Signal[] signals = new[] {
                    SignalWith(DataType.Boolean, Granularity.Day, Domain.Path.FromString("x/p")),
                    SignalWith(DataType.Boolean, Granularity.Day, Domain.Path.FromString("x/y")),
                    SignalWith(DataType.Boolean, Granularity.Day, Domain.Path.FromString("x/z/c")),
                    SignalWith(DataType.Boolean, Granularity.Day, Domain.Path.FromString("x/z/x")),
                };

                SetupMock_GetAllWithPathPrefix(signals, prefix.ToDomain<Domain.Path>());

                var result = this.signalsWebService.GetPathEntry(prefix);

                int expectedElements = 1;
                Assert.AreEqual(expectedElements, result.SubPaths.ToArray().Length);

                Dto.PathEntry expectedPathEntry = new Dto.PathEntry()
                {
                    Signals = new Dto.Signal[] { signals[0].ToDto<Dto.Signal>(), signals[1].ToDto<Dto.Signal>() },
                    SubPaths = new Dto.Path[] { new Dto.Path() { Components = new[] { "x", "z" } } }
                };

                Assert.AreEqual(expectedPathEntry.Signals.Count(), result.Signals.Count());
                Assert.AreEqual(expectedPathEntry.SubPaths.Count(), result.SubPaths.Count());

                for (int i = 0; i < 1; ++i)
                {
                    CollectionAssert.AreEqual(expectedPathEntry.SubPaths.ElementAt(i).Components.ToArray(),
                        result.SubPaths.ElementAt(i).Components.ToArray());
                }
                for (int i = 0; i < 2; ++i)
                {
                    AssertSignalsAreEqual(expectedPathEntry.Signals.ElementAt(i), result.Signals.ElementAt(i));
                }
            }

            [TestMethod]
            [ExpectedException(typeof(IdNotNullException))]
            public void GivenNoGignals_WhenAddingASignal_ThrowsIdNotNullException()
            {
                SetupWebService();

                Dto.Signal signal = GetDefaultSignal_IntegerMonth().ToDto<Dto.Signal>();
                signal.Id = 1;

                this.signalsWebService.Add(signal);
            }

            [TestMethod]
            public void GivenAData_WhenGettingDataWithTwoTheSameTimestamps_ReturningOnlyOneResult()
            {
                SetupWebService();
                int signalId = 4;

                DateTime timestamp = new DateTime(2000, 1, 1);

                var dataDto = new Dto.Datum[] { new Dto.Datum() {
                    Quality = Dto.Quality.Good,
                    Timestamp = timestamp,
                    Value = (int)2 } };

                var signalDomain = GetDefaultSignal_IntegerMonth();
                signalDomain.Id = signalId;

                SetupMock_missingValuePolicy_DefaultMissingValuePolicy();

                this.signalsRepositoryMock.Setup(x => x.Get(signalId)).Returns(signalDomain);

                this.signalsDataRepositoryMock
                    .Setup(x => x.GetData<int>(signalDomain, timestamp, timestamp))
                    .Returns(dataDto.ToDomain<IEnumerable<Domain.Datum<int>>>());

                var returnedData = this.signalsWebService.GetData(signalId, timestamp, timestamp);

                int expectedLength = 1;
                Assert.AreEqual(expectedLength, returnedData.Count());
            }

            [TestMethod]
            public void GivenAData_WhenGettingData_FillingDataWithNoneQualityMissingPolicy()
            {
                SetupWebService();

                var signal = GetDefaultSignal_IntegerMonth();

                Dto.Datum[] datumArray = new Dto.Datum[]{
                     new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 4, 1), Value = (int) 5},
                     new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int) 2} };

                int signalId = 3;
                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, datumArray);

                IEnumerable<Dto.Datum> returnedData = signalsWebService.GetData(signalId,new DateTime(2000,2,1), 
                    new DateTime(2000,6,1));

                var policy = new SpecificValueMissingValuePolicyInteger();
                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.IsAny<Domain.Signal>()))
                    .Returns(policy.ToDomain<NoneQualityMissingValuePolicyInteger>());

                int expectedArrayLength = 4;
                Assert.AreEqual(expectedArrayLength,returnedData.ToArray().Length);

                Dto.Datum[] expectedResult = new Dto.Datum[]{
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int)2 },
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1), Value = default(int) },
                    new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 4, 1), Value = (int)5 },
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 5, 1), Value = default(int) } };

                DatumArraysAreEqual(expectedResult.ToArray(),returnedData.ToArray());
            }

            [TestMethod]
            public void GivenAData_WhenGettingData_FillingDataWithSpecificValueMissingPolicy()
            {
                SetupWebService();

                var signal = GetDefaultSignal_IntegerMonth();

                Dto.Datum[] datumArray = new Dto.Datum[]{
                     new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 4, 1), Value = (int) 5},
                     new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int) 2} };

                var policy = new SpecificValueMissingValuePolicy()
                {
                    DataType = Dto.DataType.Integer,
                    Value = (int)42,
                    Quality = Dto.Quality.Fair
                };

                int signalId = 3;
                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, datumArray);

                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.IsAny<Domain.Signal>()))
                    .Returns(policy.ToDomain<SpecificValueMissingValuePolicyInteger>());

                IEnumerable<Dto.Datum> returnedData = signalsWebService.GetData(signalId, new DateTime(2000, 2, 1), new DateTime(2000, 6, 1));

                int expectedArrayLength = 4;
                Assert.AreEqual(expectedArrayLength, returnedData.ToArray().Length);

                Dto.Datum[] expectedResult = new Dto.Datum[]{
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int)2 },
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = (int)42 },
                    new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 4, 1), Value = (int)5 },
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 5, 1), Value = (int)42 } };

                DatumArraysAreEqual(expectedResult.ToArray(), returnedData.ToArray());
            }

            [TestMethod]
            public void GetData_DataExist_ZeroOrderMissingValuePolicyShouldCorrectlyFillMissingData()
            {
                SetupWebService();

                var signal = GetDefaultSignal_IntegerMonth();

                Dto.Datum[] datumArray = new Dto.Datum[]{
                     new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 5, 1), Value = (int) 5},
                     new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int) 2} };

                var policy = new ZeroOrderMissingValuePolicy()
                {
                    DataType = Dto.DataType.Integer
                };

                int signalId = 1;
                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, datumArray);

                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.IsAny<Domain.Signal>()))
                    .Returns(policy.ToDomain<ZeroOrderMissingValuePolicyInteger>());

                IEnumerable<Dto.Datum> returnedData = signalsWebService.GetData(signalId, new DateTime(2000, 2, 1), new DateTime(2000, 6, 1));

                int expectedArrayLength = 4;
                Assert.AreEqual(expectedArrayLength, returnedData.ToArray().Length);

                Dto.Datum[] expectedResult = new Dto.Datum[]{
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int)2 },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (int)2 },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = (int)2 },
                    new Dto.Datum() { Quality = Dto.Quality.Bad,  Timestamp = new DateTime(2000, 5, 1), Value = (int)5 } };

                DatumArraysAreEqual(expectedResult.ToArray(), returnedData.ToArray());
            }

            [TestMethod]
            public void GetData_FirstElementOfDataDoesntExist_ZeroOrderMissingValuePolicyShouldCorrectlyFillMissingData()
            {
                SetupWebService();

                var signal = GetDefaultSignal_IntegerMonth();

                Dto.Datum[] datumArray = new Dto.Datum[]{
                     new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 4, 1), Value = (int) 5},
                     new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int) 2} };

                var policy = new ZeroOrderMissingValuePolicy()
                {
                    DataType = Dto.DataType.Integer
                };

                int signalId = 1;
                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, datumArray);

                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.IsAny<Domain.Signal>()))
                    .Returns(policy.ToDomain<ZeroOrderMissingValuePolicyInteger>());

                IEnumerable<Dto.Datum> returnedData = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));

                int expectedArrayLength = 4;
                Assert.AreEqual(expectedArrayLength, returnedData.ToArray().Length);

                Dto.Datum[] expectedResult = new Dto.Datum[]{
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = 0 },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int)2 },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (int)2 },
                    new Dto.Datum() { Quality = Dto.Quality.Bad,  Timestamp = new DateTime(2000, 4, 1), Value = (int)5 } };

                DatumArraysAreEqual(expectedResult.ToArray(), returnedData.ToArray());
            }

            [TestMethod]
            public void GetData_ZeroOrderMVPOlderSamples_ReturnsIt()
            {
                SetupWebService();

                var signal = new Domain.Signal()
                {
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("a/b/c")
                };
                var signalId = 1;

                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, new Dto.Datum[] { });

                missingValuePolicyRepositoryMock
                    .Setup(f => f.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new ZeroOrderMissingValuePolicyInteger());

                signalsDataRepositoryMock
                    .Setup(f => f.GetDataOlderThan<Int32>(It.IsAny<Signal>(), It.IsAny<DateTime>(), 1))
                    .Returns(new[] {
                        new Domain.Datum<Int32>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 32 }
                    });

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 10), new DateTime(2000, 1, 11));

                DatumArraysAreEqual(
                    new[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 10), Value = 32 } },
                    result.ToArray());
            }

            [TestMethod]
            public void GetData_FirstOrderMVPWithGivenValues_ReturnsFilled()
            {
                SetupWebService();

                var signal = new Signal()
                {
                    DataType = DataType.Integer,
                    Granularity = Granularity.Day,
                    Path = Path.FromString("a")
                };
                var signalId = 1;

                var datum = new List<Dto.Datum>
                {
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 3 },
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 4), Value = 6 }
                };

                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, datum.ToArray());

                missingValuePolicyRepositoryMock
                   .Setup(f => f.Get(It.IsAny<Domain.Signal>()))
                   .Returns(new FirstOrderMissingValuePolicyInteger());

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 5));

                datum.Add(new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 2), Value = 4 });
                datum.Add(new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 3), Value = 5 });

                DatumArraysAreEqual(datum.OrderBy(d => d.Timestamp).ToArray(), result.ToArray());
            }

            [TestMethod]
            [ExpectedException(typeof(TypeUnsupportedException))]
            public void GetData_FirstOrderMVPWithInvalidDataType_ExpectedException()
            {
                SetupWebService();
                var signal = new Signal()
                {
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("a")
                };
                var signalId = 1;
                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, new Dto.Datum[] { });
                missingValuePolicyRepositoryMock
                  .Setup(f => f.Get(It.IsAny<Domain.Signal>()))
                  .Returns(new FirstOrderMissingValuePolicyBoolean());

                signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 5));
            }

            [TestMethod]
            [ExpectedException(typeof(TypeUnsupportedException))]
            public void GetData_FirstOrderMVPWithStringDataType_ExpectedException()
            {
                SetupWebService();
                var signal = new Signal()
                {
                    DataType = DataType.String,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("a")
                };
                var signalId = 1;
                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, new Dto.Datum[] { });
                missingValuePolicyRepositoryMock
                  .Setup(f => f.Get(It.IsAny<Domain.Signal>()))
                  .Returns(new FirstOrderMissingValuePolicyString());

                signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 5));
            }

            [TestMethod]
            public void GetData_FirstOrderMVPWithOutBoundValues_ExpectedEmptyDatums()
            {
                SetupWebService();

                var signal = new Signal()
                {
                    DataType = DataType.Integer,
                    Granularity = Granularity.Day,
                    Path = Path.FromString("a")
                };
                var signalId = 1;

                var datum = new List<Dto.Datum>
                {
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3), Value = 3 },
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 5), Value = 5 }
                };

                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, datum.ToArray());

                missingValuePolicyRepositoryMock
                   .Setup(f => f.Get(It.IsAny<Domain.Signal>()))
                   .Returns(new FirstOrderMissingValuePolicyInteger());

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 8));

                datum.Add(new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = 0 });
                datum.Add(new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 2), Value = 0 });
                datum.Add(new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 4), Value = 4 });
                datum.Add(new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 6), Value = 0 });
                datum.Add(new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 7), Value = 0 });

                DatumArraysAreEqual(datum.OrderBy(d => d.Timestamp).ToArray(), result.ToArray());
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void Delete_NotExistingSignal_NoneExceptionIsThrown()
            {
                var id = 1;
                SetupWebService();

                DeleteASignal(id);
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void Delete_NotExistingSignal_ExpectedRepositoryGetCall()
            {
                SetupWebService();

                var signalId = 1;
                DeleteASignal(signalId);

                signalsRepositoryMock.Verify(f => f.Get(It.Is<int>(i => i == signalId)), Times.Once);
            }

            [TestMethod]
            public void Delete_WithExistingSignal_ExpectedRepositoryDeleteCall()
            {
                var signal = PrepareDefaultSignalToGet();

                signalsWebService.Delete(1);

                signalsRepositoryMock
                    .Verify(f => f.Delete(It.Is<Domain.Signal>(sig => CompareSignal(sig, signal))), Times.Once);
            }

            [TestMethod]
            public void Delete_WithExistingSignalAndData_ExpectedDataRepositoryDeleteDataCall()
            {
                var signal = PrepareDefaultSignalToGet();

                signalsWebService.Delete(1);

                signalsDataRepositoryMock
                    .Verify(f => f.DeleteData<Int32>(It.Is<Signal>(sig => CompareSignal(sig, signal))), Times.Once);
            }

            [TestMethod]
            public void Delete_WithExistingSignalAndMVP_ExpectedMVPRepositorySetCallWithNull()
            {
                var signal = PrepareDefaultSignalToGet();

                signalsWebService.Delete(1);

                missingValuePolicyRepositoryMock
                    .Verify(f => f.Set(It.Is<Signal>(sig => CompareSignal(sig, signal)), null), Times.Once);
            }


            [TestMethod]
            public void GivenNoData_WhenGettingADataInPointInTime_ItReturnsOneDatum()
            {
                var signal = MakeDefaultIntegerSignal();

                var datumArray = MakeNoData();

                var policy = new ZeroOrderMissingValuePolicy()
                {
                    DataType = Dto.DataType.Integer
                };

                int signalId = 1;
                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, datumArray);

                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.IsAny<Domain.Signal>()))
                    .Returns(policy.ToDomain<ZeroOrderMissingValuePolicyInteger>());

                IEnumerable<Dto.Datum> returnedData = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

                int expectedArrayLength = 1;
                Assert.AreEqual(expectedArrayLength, returnedData.ToArray().Length);

            }

            [TestMethod]
            public void GivenNoData_WhenGettingADataInPointInTime_ItReturnsDatumFilledWithNoneQualityMissingValuePolicy()
            {

                var signal = MakeDefaultIntegerSignal();

                var datumArray = MakeNoData();

               var policy = new NoneQualityMissingValuePolicy()
                {
                    DataType = Dto.DataType.Integer
                };

                int signalId = 1;
                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, datumArray);

                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.IsAny<Domain.Signal>()))
                   .Returns(policy.ToDomain<NoneQualityMissingValuePolicyInteger>());

                IEnumerable<Dto.Datum> returnedData = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

                int expectedArrayLength = 1;
                Assert.AreEqual(expectedArrayLength, returnedData.ToArray().Length);

                Dto.Datum[] expectedResult = new Dto.Datum[]{
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = (int)0 }
                };

                DatumArraysAreEqual(expectedResult.ToArray(), returnedData.ToArray());

            }

            [TestMethod]
            public void GivenNoData_WhenGettingADataInPointInTime_ItReturnsDatumFilledWithMissingValuePolicyExpectedValues()
            {

                var signal = MakeDefaultIntegerSignal();

                var datumArray = MakeNoData();

                var defaultValue = 5;
                var policy = new SpecificValueMissingValuePolicy()
                {
                    DataType = Dto.DataType.Integer,
                    Value=defaultValue,
                    Quality=Dto.Quality.Bad
                };

                int signalId = 1;
                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, datumArray);

                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.IsAny<Domain.Signal>()))
                   .Returns(policy.ToDomain<SpecificValueMissingValuePolicyInteger>());

                IEnumerable<Dto.Datum> returnedData = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

                int expectedArrayLength = 1;
                Assert.AreEqual(expectedArrayLength, returnedData.ToArray().Length);

                Dto.Datum[] expectedResult = new Dto.Datum[]{
                    new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (int)defaultValue }
                };

                DatumArraysAreEqual(expectedResult.ToArray(), returnedData.ToArray());

            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void Delete_NotExistingSignal_ShouldThrowsException()
            {
                var id = 1;
                Signal sig = null;
                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(sig);
                SetupWebService();
                DeleteASignal( id);

            }

            [TestMethod]
            public void GivenASignal_WhenSetsShadowMissingValuePolicyWithNoData_FilledDefaultValues()
            {

                Dto.Signal shadowSignal = new Dto.Signal() { Id = 2, DataType = Dto.DataType.Integer, Granularity = Dto.Granularity.Month, Path = new Dto.Path() { Components = new[] { "x", "z" } } };
                Signal shadowSignalDomain = new Domain.Signal() { Id = 2, DataType = DataType.Integer, Granularity = Granularity.Month, Path = Path.FromString("x/z") };

               
                SetupWebService();

                var signal = new Signal()
                {
                    DataType = DataType.Integer,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("a")
                };
                var signalId = 1;

                var datum = new List<Dto.Datum>
                {
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 3 },
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 2, 1), Value = 6 }
                };

                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, datum.ToArray());

                var policy = new ShadowMissingValuePolicyInteger();
                policy.ShadowSignal = shadowSignalDomain;

                missingValuePolicyRepositoryMock
                   .Setup(f => f.Get(It.IsAny<Domain.Signal>()))
                   .Returns(policy);

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));

                datum.Add(new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1), Value = (int)0 });
                datum.Add(new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 4, 1), Value = (int)0 });

                DatumArraysAreEqual(datum.OrderBy(d => d.Timestamp).ToArray(), result.ToArray());

            }
            [TestMethod]
            public void GivenASignal_WhenSetsShadowMissingValuePolicyWithData_FilledEmptySpacesWithShadowData()
            {

                Dto.Signal shadowSignal = new Dto.Signal() { Id = 2, DataType = Dto.DataType.Integer, Granularity = Dto.Granularity.Month, Path = new Dto.Path() { Components = new[] { "x", "z" } } };
                Signal shadowSignalDomain = new Domain.Signal() { Id = 2, DataType = DataType.Integer, Granularity = Granularity.Month, Path = Path.FromString("x/z") };


                SetupWebService();

                var signal = new Signal()
                {
                    DataType = DataType.Integer,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("a")
                };
                var signalId = 1;

                var datum = new List<Dto.Datum>
                {
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 3 },
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 2, 1), Value = 6 }
                };

                var shadowDatum = new List<Dto.Datum>
                {
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 71 }
                };

                signalsRepositoryMock
                     .Setup(x => x.Get(It.Is<int>(y => y == signalId)))
                     .Returns<int>(z => {
                         var signal2 = signal;
                         signal.Id = signalId;
                         return signal;
                     });

                SetupMocks_RepositoryAndDataRepository_ForGettingData(signal, signalId, datum.ToArray());

                SetupMocks_RepositoryAndDataRepository_ForGettingData(shadowSignalDomain, 2, shadowDatum.ToArray());


                var policy = new ShadowMissingValuePolicyInteger();
                policy.ShadowSignal = shadowSignalDomain;

                missingValuePolicyRepositoryMock
                   .Setup(f => f.Get(It.Is<Domain.Signal>(s=>s.Id==signal.Id)))
                   .Returns(policy);

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                datum.Add(new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (int)71 });
              

                DatumArraysAreEqual(datum.OrderBy(d => d.Timestamp).ToArray(), result.ToArray());

            }
            [TestMethod]
            [ExpectedException(typeof(WrongTypesException))]
            public void GivenASignalDouble_WhenSetsShadowMissingValuePolicyIntegerWithNoData_ThrowsException()
            {

                Dto.Signal shadowSignal = new Dto.Signal() { Id = 2, DataType = Dto.DataType.Integer, Granularity = Dto.Granularity.Month, Path = new Dto.Path() { Components = new[] { "x", "z" } } };
                Signal shadowSignalDomain = new Domain.Signal() { Id = 2, DataType = DataType.Integer, Granularity = Granularity.Month, Path = Path.FromString("x/z") };


                SetupWebService();

                var signal = new Signal()
                {
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("a")
                };
                var signalId = 1;

                signalsRepositoryMock
                     .Setup(x => x.Get(It.Is<int>(y => y == signalId)))
                     .Returns<int>(z => {
                         var signal2 = signal;
                         signal.Id = signalId;
                         return signal;
                     });

                var policy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { ShadowSignal = shadowSignal, DataType = shadowSignal.DataType };
                var policyDomain = new ShadowMissingValuePolicyInteger();
                policyDomain.ShadowSignal = shadowSignalDomain;

                missingValuePolicyRepositoryMock
                   .Setup(f => f.Get(It.Is<Domain.Signal>(s => s.Id == signal.Id)))
                   .Returns(policyDomain);

                signalsWebService.SetMissingValuePolicy(signalId, policy);

            }

            [TestMethod]
            public void GivenASignalInteger_WhenSetsShadowMissingValuePolicyIntegerWithNoData_DoesNotThrowException()
            {

                Dto.Signal shadowSignal = new Dto.Signal() { Id = 2, DataType = Dto.DataType.Integer, Granularity = Dto.Granularity.Month, Path = new Dto.Path() { Components = new[] { "x", "z" } } };
                Signal shadowSignalDomain = new Domain.Signal() { Id = 2, DataType = DataType.Integer, Granularity = Granularity.Month, Path = Path.FromString("x/z") };


                SetupWebService();

                var signal = new Signal()
                {
                    DataType = DataType.Integer,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("a")
                };
                var signalId = 1;

                signalsRepositoryMock
                 .Setup(x => x.Get(It.Is<int>(y => y == 2)))
                 .Returns<int>(z => 
                      shadowSignalDomain
                 );

                signalsRepositoryMock
                     .Setup(x => x.Get(It.Is<int>(y => y == signalId)))
                     .Returns<int>(z => {
                         var signal2 = signal;
                         signal.Id = signalId;
                         return signal;
                     });

                var policy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { ShadowSignal = shadowSignal, DataType = shadowSignal.DataType };
                var policyDomain = new ShadowMissingValuePolicyInteger();
                policyDomain.ShadowSignal = shadowSignalDomain;

                missingValuePolicyRepositoryMock
                   .Setup(f => f.Get(It.Is<Domain.Signal>(s => s.Id == signal.Id)))
                   .Returns(policyDomain);

                signalsWebService.SetMissingValuePolicy(signalId, policy);

            }

            [TestMethod]
            [ExpectedException(typeof(WrongTypesException))]
            public void GivenASignalMonth_WhenSetsShadowMissingValuePolicyDayWithNoData_ThrowsException()
            {

                Dto.Signal shadowSignal = new Dto.Signal() { Id = 2, DataType = Dto.DataType.Integer, Granularity = Dto.Granularity.Day, Path = new Dto.Path() { Components = new[] { "x", "z" } } };
                Signal shadowSignalDomain = new Domain.Signal() { Id = 2, DataType = DataType.Integer, Granularity = Granularity.Day, Path = Path.FromString("x/z") };


                SetupWebService();

                var signal = new Signal()
                {
                    DataType = DataType.Integer,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("a")
                };
                var signalId = 1;

                signalsRepositoryMock
                 .Setup(x => x.Get(It.Is<int>(y => y == 2)))
                 .Returns<int>(z =>
                      shadowSignalDomain
                 );

                signalsRepositoryMock
                     .Setup(x => x.Get(It.Is<int>(y => y == signalId)))
                     .Returns<int>(z => {
                         var signal2 = signal;
                         signal.Id = signalId;
                         return signal;
                     });

                var policy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { ShadowSignal = shadowSignal, DataType = shadowSignal.DataType };
                var policyDomain = new ShadowMissingValuePolicyInteger();
                policyDomain.ShadowSignal = shadowSignalDomain;

                missingValuePolicyRepositoryMock
                   .Setup(f => f.Get(It.Is<Domain.Signal>(s => s.Id == signal.Id)))
                   .Returns(policyDomain);

                signalsWebService.SetMissingValuePolicy(signalId, policy);

            }


            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignalWithShadowMVP_WhenSettingItselfAsShadow_ThrowsArgumentException()
            {
                var signalId = 1;

                SetupWebService();
                var signalDto = new Dto.Signal()
                {
                    Id = signalId,
                    DataType = Dto.DataType.Integer,
                    Granularity = Dto.Granularity.Month
                };
                var signal = new Signal()
                {
                    Id = signalId,
                    DataType = DataType.Integer,
                    Granularity = Granularity.Month
                };



                signalsRepositoryMock
                     .Setup(x => x.Get(It.Is<int>(y => y == signalId)))
                     .Returns<int>(z => {
                         var signal2 = signal;
                         signal.Id = signalId;
                         return signal;
                     });


                var policy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { ShadowSignal = signalDto, DataType = signalDto.DataType };

                signalsWebService.SetMissingValuePolicy(signalId, policy);

            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignalWithShadowMVP_WhenCreatingDependencyCycle_ThrowsArgumentException()
            {
                int signalId = 2;
                int shadowSignalId = 3;
                int shadowShadowSignalId = 4;

                Dto.Signal signal = new Dto.Signal() { Id = signalId, DataType = Dto.DataType.Integer, Granularity = Dto.Granularity.Day};
                Signal signalDomain = new Domain.Signal() { Id = signalId, DataType = DataType.Integer, Granularity = Granularity.Day};
                Dto.Signal shadowSignal = new Dto.Signal() { Id = shadowSignalId, DataType = Dto.DataType.Integer, Granularity = Dto.Granularity.Day };
                Signal shadowSignalDomain = new Domain.Signal() { Id = shadowSignalId, DataType = DataType.Integer, Granularity = Granularity.Day };
                Dto.Signal shadowShadowSignal = new Dto.Signal() { Id = shadowShadowSignalId, DataType = Dto.DataType.Integer, Granularity = Dto.Granularity.Day };
                Signal shadowShadowSignalDomain = new Domain.Signal() { Id = shadowShadowSignalId, DataType = DataType.Integer, Granularity = Granularity.Day };

                SetupWebServiceWithoutDefaultMVP();

                var policy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { ShadowSignal = shadowSignal, DataType = shadowSignal.DataType };
                var policyDomain = new ShadowMissingValuePolicyInteger();
                policyDomain.ShadowSignal = shadowSignalDomain;
                var shadowPolicy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { ShadowSignal = shadowShadowSignal, DataType = shadowShadowSignal.DataType };
                var shadowPolicyDomain = new ShadowMissingValuePolicyInteger();
                shadowPolicyDomain.ShadowSignal = shadowShadowSignalDomain;
                var shadowShadowPolicy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { ShadowSignal = signal, DataType = signal.DataType };
                var shadowShadowPolicyDomain = new ShadowMissingValuePolicyInteger();
                shadowShadowPolicyDomain.ShadowSignal = signalDomain;

                signalsRepositoryMock
                    .Setup(x => x.Get(It.Is<int>(y => y == signalId)))
                    .Returns<int>(id => signalDomain);
                signalsRepositoryMock
                    .Setup(x => x.Get(It.Is<int>(y => y == shadowSignalId)))
                    .Returns<int>(id => shadowSignalDomain);
                signalsRepositoryMock
                    .Setup(x => x.Get(It.Is<int>(y => y == shadowShadowSignalId)))
                    .Returns<int>(id => shadowShadowSignalDomain);

                missingValuePolicyRepositoryMock
                   .Setup(f => f.Get(It.Is<Domain.Signal>(s => s.Id == shadowSignal.Id)))
                   .Returns(shadowPolicyDomain);
                missingValuePolicyRepositoryMock
                   .Setup(f => f.Get(It.Is<Domain.Signal>(s => s.Id == shadowShadowSignal.Id)))
                   .Returns(shadowShadowPolicyDomain);

                signalsWebService.SetMissingValuePolicy(2, policy);

            }


            [TestMethod]
            public void GivenASignal_WhenGettingCoarseData_ReturnsNotNull()
            {
                SetupWebService();
                int signalId = 4;

                DateTime timestamp = new DateTime(2000, 1, 1);

                var signalDomain = GetDefaultSignal_IntegerMonth();
                signalDomain.Id = signalId;

                SetupMock_missingValuePolicy_DefaultMissingValuePolicy();

                this.signalsRepositoryMock.Setup(x => x.Get(signalId)).Returns(signalDomain);

                var result = signalsWebService.GetCoarseData(signalId, Dto.Granularity.Year, timestamp, timestamp);

                Assert.IsNotNull(result);
            }


            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingCoarseDataWithGranularityNotSmallerThanOriginal_ThrowsArgumentExcpetion()
            {
                SetupWebService();
                int signalId = 4;

                DateTime timestamp = new DateTime(2000, 1, 1);

                var signalDomain = GetDefaultSignal_IntegerMonth();
                signalDomain.Id = signalId;

                SetupMock_missingValuePolicy_DefaultMissingValuePolicy();

                this.signalsRepositoryMock.Setup(x => x.Get(signalId)).Returns(signalDomain);

                var result = signalsWebService.GetCoarseData(signalId, Dto.Granularity.Second, timestamp, timestamp);
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignal_WhenGettingCoarseData_ThrowsCouldntGetSignalException()
            {
                SetupWebService();
                int signalId = 4;

                DateTime timestamp = new DateTime(2000, 1, 1);

                SetupMock_missingValuePolicy_DefaultMissingValuePolicy();


                var result = signalsWebService.GetCoarseData(signalId, Dto.Granularity.Second, timestamp, timestamp);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingCoarseDataWrongToTimeStampForSignal_ThrowsArgumentExcpetion()
            {
                SetupWebService();
                int signalId = 4;

                DateTime timestamp = new DateTime(2000, 1, 4);
                DateTime goodTimestamp = new DateTime(2000, 1, 1);

                var signalDomain = GetDefaultSignal_IntegerMonth();
                signalDomain.Id = signalId;

                SetupMock_missingValuePolicy_DefaultMissingValuePolicy();

                this.signalsRepositoryMock.Setup(x => x.Get(signalId)).Returns(signalDomain);

                var result = signalsWebService.GetCoarseData(signalId, Dto.Granularity.Year, goodTimestamp, timestamp);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingCoarseDataWrongToTimeStampForGivenGranularity_ThrowsArgumentExcpetion()
            {
                SetupWebService();
                int signalId = 4;

                DateTime timestamp = new DateTime(2000, 2, 1);
                DateTime goodTimestamp = new DateTime(2000, 1, 1);

                var signalDomain = GetDefaultSignal_IntegerMonth();
                signalDomain.Id = signalId;

                SetupMock_missingValuePolicy_DefaultMissingValuePolicy();

                this.signalsRepositoryMock.Setup(x => x.Get(signalId)).Returns(signalDomain);

                var result = signalsWebService.GetCoarseData(signalId, Dto.Granularity.Year, goodTimestamp, timestamp);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingCoarseDataWrongFromTimeStampForSignal_ThrowsArgumentExcpetion()
            {
                SetupWebService();
                int signalId = 4;

                DateTime timestamp = new DateTime(2000, 1, 4);
                DateTime goodTimestamp = new DateTime(2000, 1, 1);

                var signalDomain = GetDefaultSignal_IntegerMonth();
                signalDomain.Id = signalId;

                SetupMock_missingValuePolicy_DefaultMissingValuePolicy();

                this.signalsRepositoryMock.Setup(x => x.Get(signalId)).Returns(signalDomain);

                var result = signalsWebService.GetCoarseData(signalId, Dto.Granularity.Year, timestamp, goodTimestamp);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingCoarseDataWrongFromTimeStampForGivenGranularity_ThrowsArgumentExcpetion()
            {
                SetupWebService();
                int signalId = 4;

                DateTime timestamp = new DateTime(2000, 2, 1);
                DateTime goodTimestamp = new DateTime(2000, 1, 1);

                var signalDomain = GetDefaultSignal_IntegerMonth();
                signalDomain.Id = signalId;

                SetupMock_missingValuePolicy_DefaultMissingValuePolicy();

                this.signalsRepositoryMock.Setup(x => x.Get(signalId)).Returns(signalDomain);

                var result = signalsWebService.GetCoarseData(signalId, Dto.Granularity.Year, timestamp, goodTimestamp);
            }

            [TestMethod]
            [ExpectedException(typeof(TypeUnsupportedException))]
            public void GivenASignal_WhenGettingCoarseDataWithBoolData_ThrowsTypeUnsupportedException()
            {
                SetupWebService();
                int signalId = 4;

                DateTime timestamp = new DateTime(2000, 1, 1);

                var signalDomain = GetDefaultSignal_IntegerMonth();
                signalDomain.Id = signalId;
                signalDomain.DataType = DataType.Boolean;

                SetupMock_missingValuePolicy_DefaultMissingValuePolicy();

                this.signalsRepositoryMock.Setup(x => x.Get(signalId)).Returns(signalDomain);

                var result = signalsWebService.GetCoarseData(signalId, Dto.Granularity.Year, timestamp, timestamp);
            }

            [TestMethod]
            [ExpectedException(typeof(TypeUnsupportedException))]
            public void GivenASignal_WhenGettingCoarseDataWithStringData_ThrowsTypeUnsupportedException()
            {
                SetupWebService();
                int signalId = 4;

                DateTime timestamp = new DateTime(2000, 1, 1);

                var signalDomain = GetDefaultSignal_IntegerMonth();
                signalDomain.Id = signalId;
                signalDomain.DataType = DataType.String;

                SetupMock_missingValuePolicy_DefaultMissingValuePolicy();

                this.signalsRepositoryMock.Setup(x => x.Get(signalId)).Returns(signalDomain);

                var result = signalsWebService.GetCoarseData(signalId, Dto.Granularity.Year, timestamp, timestamp);
            }


            [TestMethod]
            public void GivenASignal_WhenGettingCoarseDataWithEqualFromAndToTimeStamps_ReturnsOneDatum()
            {
                SetupWebService();
                int id = 6;

                DateTime dateFrom = new DateTime(2000, 1, 1);
                DateTime dateTo = new DateTime(2000, 1, 1);

                var signal = SignalWith(DataType.Integer, Granularity.Day, Path.FromString("root/signalInt"), id);

                var dataReturned = new Domain.Datum<int>[]
                {
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (int)2 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 2), Value = (int)3 }
                };

                SetupRepositoryMocks_GetData_ReturnsGivenDataCollection<int>(id, signal, dateFrom, dateTo, dataReturned);

                var result = signalsWebService.GetCoarseData(id, Dto.Granularity.Year, dateFrom, dateTo);

                Assert.AreEqual(1, result.Count());
            }

            [TestMethod]
            public void GivenASignal_WhenGettingCoarseDataWithToTimeStampSmallerThanFrom_ReturnsNoDatum()
            {
                SetupWebService();
                int id = 6;

                DateTime dateFrom = new DateTime(2001, 1, 1);
                DateTime dateTo = new DateTime(2000, 1, 1);

                var signal = SignalWith(DataType.Integer, Granularity.Day, Path.FromString("root/signalInt"), id);

                var dataReturned = new Domain.Datum<int>[]
                {
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (int)2 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 2), Value = (int)3 }
                };

                SetupRepositoryMocks_GetData_ReturnsGivenDataCollection<int>(id, signal, dateFrom, dateTo, dataReturned);

                var result = signalsWebService.GetCoarseData(id, Dto.Granularity.Year, dateFrom, dateTo);

                Assert.AreEqual(0, result.Count());
            }

            [TestMethod]
            public void GivenASignal_WhenGettingCoarseDataWithEqualFromAndToTimeStamps_ReturnsOneDatumThatIsAverage()
            {
                SetupWebService();
                int id = 6;

                DateTime dateFrom = new DateTime(2000, 5, 1);
                DateTime dateTo = new DateTime(2000, 5, 1);
                DateTime realDateTo = new DateTime(2000, 5, 8);

                var signal = SignalWith(DataType.Integer, Granularity.Day, Path.FromString("root/signalInt"), id);

                var dataReturned = new Domain.Datum<int>[]
                {
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = (int)2 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 2), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 3), Value = (int)3 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 4), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 5), Value = (int)3 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 6), Value = (int)3 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 7), Value = (int)4 },
                };

                SetupRepositoryMocks_GetData_ReturnsData_WithinTime<int>(id, signal, dateFrom, realDateTo, dataReturned);

                var result = signalsWebService.GetCoarseData(id, Dto.Granularity.Week, dateFrom, dateTo);

                Assert.AreEqual(1, result.Count());
                Assert.AreEqual(3, result.Single().Value);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingCoarseDataWithEqualFromAndToTimeStamps_ReturnsOneDatumWithWorstQualityFromRange()
            {
                SetupWebService();
                int id = 6;

                DateTime dateFrom = new DateTime(2000, 5, 1);
                DateTime dateTo = new DateTime(2000, 5, 1);
                DateTime realDateTo = new DateTime(2000, 5, 8);

                var signal = SignalWith(DataType.Integer, Granularity.Day, Path.FromString("root/signalInt"), id);

                var dataReturned = new Domain.Datum<int>[]
                {
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = (int)2 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 2), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 3), Value = (int)3 },
                    new Datum<int>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 5, 4), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 5), Value = (int)3 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 6), Value = (int)3 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 7), Value = (int)4 },
                };

                SetupRepositoryMocks_GetData_ReturnsData_WithinTime<int>(id, signal, dateFrom, realDateTo, dataReturned);

                var result = signalsWebService.GetCoarseData(id, Dto.Granularity.Week, dateFrom, dateTo);

                Assert.AreEqual(1, result.Count());
                Assert.AreEqual(Dto.Quality.Bad, result.Single().Quality);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingCoarseData_ReturnsAveragesFromRanges()
            {
                SetupWebService();
                int id = 6;

                DateTime dateFrom = new DateTime(2000, 5, 1);
                DateTime dateTo = new DateTime(2000, 5, 15);

                var signal = SignalWith(DataType.Integer, Granularity.Day, Path.FromString("root/signalInt"), id);

                var dataReturned = new Domain.Datum<int>[]
                {
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = (int)2 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 2), Value = (int)0 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 3), Value = (int)3 },
                    new Datum<int>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 5, 4), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 5), Value = (int)3 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 6), Value = (int)4 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 7), Value = (int)4 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 8), Value = (int)28 },
                };

                SetupRepositoryMocks_GetData_ReturnsData_WithinTime<int>(id, signal, dateFrom, dateTo, dataReturned);

                var result = signalsWebService.GetCoarseData(id, Dto.Granularity.Week, dateFrom, dateTo);

                Assert.AreEqual(2, result.Count());
                Assert.AreEqual(3, result.First().Value);
                Assert.AreEqual(4, result.Skip(1).First().Value);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingCoarseData_ReturnsDataWithWorstQualityFromRanges()
            {
                SetupWebService();
                int id = 6;

                DateTime dateFrom = new DateTime(2000, 5, 1);
                DateTime dateTo = new DateTime(2000, 5, 15);

                var signal = SignalWith(DataType.Integer, Granularity.Day, Path.FromString("root/signalInt"), id);

                var dataReturned = new Domain.Datum<int>[]
                {
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = (int)2 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 2), Value = (int)0 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 3), Value = (int)3 },
                    new Datum<int>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 5, 4), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 5), Value = (int)3 },
                    new Datum<int>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 5, 6), Value = (int)4 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 7), Value = (int)4 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 5, 8), Value = (int)28 },
                };

                SetupRepositoryMocks_GetData_ReturnsData_WithinTime<int>(id, signal, dateFrom, dateTo, dataReturned);

                var result = signalsWebService.GetCoarseData(id, Dto.Granularity.Week, dateFrom, dateTo);

                Assert.AreEqual(2, result.Count());
                Assert.AreEqual(Dto.Quality.Bad, result.First().Quality);
                Assert.AreEqual(Dto.Quality.None, result.Skip(1).First().Quality);
            }

            private void DeleteASignal(int id)
            {

                signalsWebService.Delete(id);
            }

            private Dto.Datum[] MakeNoData()
            {
                return  new Dto.Datum[] { };
            }

            private Signal MakeDefaultIntegerSignal()
            {
                SetupWebService();

                var signal = GetDefaultSignal_IntegerMonth();

                

                return signal;
            }

            private Signal PrepareDefaultSignalToGet()
            {
                SetupWebService();

                var signal = GetDefaultSignal_IntegerMonth();
                signalsRepositoryMock
                    .Setup(f => f.Get(It.IsAny<int>()))
                    .Returns(signal);

                return signal;
            }

            private bool CompareSignal(Signal a, Signal b)
            {
                return a.DataType == b.DataType &&
                    a.Granularity == b.Granularity &&
                    a.Path.ToString() == b.Path.ToString() &&
                    a.Id == b.Id;
            }

            private void SetupMocks_RepositoryAndDataRepository_ForGettingData(Signal signal,int signalId,Dto.Datum[] datumArray)
            {
                signalsRepositoryMock
                    .Setup(x => x.Get(It.Is<int>(y => y == signalId)))
                    .Returns<int>(z => {
                        var signal2 = signal;
                        signal.Id = signalId;
                        return signal;
                    });

                signalsDataRepositoryMock
                    .Setup(x => x.GetData<int>(It.Is<Domain.Signal>(s=>CompareSignal(s,signal)), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(datumArray.ToDomain<IEnumerable<Domain.Datum<int>>>());
            }

            private void DatumArraysAreEqual(Dto.Datum[] expected,Dto.Datum[] actual)
            {
                Assert.AreEqual(expected.Length,actual.Length);

                for (int i = 0; i < expected.Length; ++i)
                {
                    Assert.AreEqual(expected.ElementAt(i).Quality,actual.ElementAt(i).Quality);
                    Assert.AreEqual(expected.ElementAt(i).Timestamp, actual.ElementAt(i).Timestamp);
                    Assert.AreEqual(expected.ElementAt(i).Value, actual.ElementAt(i).Value);
                }
            }

            private void SetupMock_GetAllWithPathPrefix(Signal[] signal,Path path)
            {
                this.signalsRepositoryMock
                    .Setup(x => x.GetAllWithPathPrefix(It.Is<Domain.Path>(y => PathsEquals(y, path))))
                    .Returns(signal);
            }

            private Domain.Signal GetDefaultSignal_IntegerMonth()
            {
                return new Domain.Signal()
                {
                    Path = Domain.Path.FromString("x/y/z"),
                    Granularity = Granularity.Month,
                    DataType = DataType.Integer

                };
            }

            private bool PathsEquals(Domain.Path expected, Domain.Path actual)
            {
                if (expected.Components.ToArray().Length != actual.Components.ToArray().Length)
                    return false;

                int size = expected.Components.ToArray().Length;

                for (int i =0; i < size; ++i)
                {
                    if (expected.Components.ElementAt(i) != actual.Components.ElementAt(i))
                        return false;
                }
                return true;
            }

            private void SetupMock_missingValuePolicy_DefaultMissingValuePolicy()
            {
                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.Is<Domain.Signal>(y => y.DataType == DataType.Double)))
                    .Returns(new NoneQualityMissingValuePolicyDouble());
                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.Is<Domain.Signal>(y => y.DataType == DataType.Integer)))
                    .Returns(new NoneQualityMissingValuePolicyInteger());
                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.Is<Domain.Signal>(y => y.DataType == DataType.Decimal)))
                    .Returns(new NoneQualityMissingValuePolicyDecimal());
                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.Is<Domain.Signal>(y => y.DataType == DataType.Boolean)))
                    .Returns(new NoneQualityMissingValuePolicyBoolean());
                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.Is<Domain.Signal>(y => y.DataType == DataType.String)))
                    .Returns(new NoneQualityMissingValuePolicyString());
            }


            private void GivenASignalAndData_SetupSignalsRepositoryMockAndVerifySetDataCall<T>(Signal signal, IEnumerable<Dto.Datum> data, DateTime timeStamp, T value)
            {
                signalsRepositoryMock.Setup(srm => srm.Get(signal.Id.Value)).Returns(signal);
                signalsWebService.SetData(signal.Id.Value, data);
                VerifySetDataCallOnSignalsDataRepositoryMock<T>(signal, timeStamp, value);
            }

            private void GivenASignal_SetupSignalsRepositoryMockAndVerifyGetDataCall<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
            {
                signalsRepositoryMock.Setup(srm => srm.Get(signal.Id.Value)).Returns(signal);
                signalsWebService.GetData(signal.Id.Value, fromIncludedUtc, toExcludedUtc);
                VerifyGetDataCallOnSignalsDataRepositoryMock<T>(signal, fromIncludedUtc, toExcludedUtc);
            }

            private void SetupRepositoryMocks_CallGetData_CompareReturnedData<T>(Signal signal, IEnumerable<Dto.Datum> expectedData, IEnumerable<Domain.Datum<T>> dataReturnedByMock, DateTime fromIncludedUtc, DateTime toExcludedUtc)
            {
                SetupRepositoryMocks_GetData_ReturnsGivenDataCollection<T>(signal.Id.Value, signal, fromIncludedUtc, toExcludedUtc, dataReturnedByMock);
                var result = signalsWebService.GetData(signal.Id.Value, fromIncludedUtc, toExcludedUtc);
                DatumArraysAreEqual(result.ToArray(), expectedData.ToArray());
            }

            private void SetupRepositoryMocks_GetData_ReturnsGivenDataCollection<T>(int id, Domain.Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc, IEnumerable<Domain.Datum<T>> dataReturned)
            {
                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(signal);

                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<T>(It.IsAny<Signal>(), fromIncludedUtc, toExcludedUtc)).Returns(dataReturned);
            }

            private void SetupRepositoryMocks_GetData_ReturnsData_WithinTime<T>(int id, Domain.Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc, IEnumerable<Domain.Datum<T>> dataReturned)
            {
                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(signal);

                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<T>(It.Is<Signal>(s => s.Id == id), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns<Signal, DateTime, DateTime>((s,f,t) => dataReturned.Where(d => (d.Timestamp >= f && d.Timestamp < t) || (t == f && d.Timestamp == f)));
            }

            private void VerifyGetDataCallOnSignalsDataRepositoryMock<T>(Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
            {
                signalsDataRepositoryMock.Verify(sdrm => sdrm.GetData<T>(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                    s.DataType == signal.DataType &&
                    s.Granularity == signal.Granularity &&
                    s.Path.ToString() == signal.Path.ToString()), fromIncludedUtc, toExcludedUtc));
            }

            private void VerifySetDataCallOnSignalsDataRepositoryMock<T>(Signal signal, System.DateTime timeStamp, T value, Domain.Quality quality = Quality.Good, int elementNumber = 0)
            {
                signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<T>(It.Is<IEnumerable<Datum<T>>>(data =>
                    data.ElementAt(elementNumber).Quality == quality &&
                    data.ElementAt(elementNumber).Timestamp == timeStamp &&
                    data.ElementAt(elementNumber).Value.Equals(value) &&
                    data.ElementAt(elementNumber).Signal.Id == signal.Id &&
                    data.ElementAt(elementNumber).Signal.DataType == signal.DataType &&
                    data.ElementAt(elementNumber).Signal.Granularity == signal.Granularity &&
                    data.ElementAt(elementNumber).Signal.Path.ToString() == signal.Path.ToString())),
                    Times.Once);
            }

            private void SetupWebService()
            {
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
                SetupMock_missingValuePolicy_DefaultMissingValuePolicy();
            }

            private void SetupWebServiceWithoutDefaultMVP()
            {
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private Dto.Signal SignalWith(Dto.DataType dataType, Dto.Granularity granularity, Dto.Path path, int? id = null)
            {
                return new Dto.Signal()
                {
                    Id = id,
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }

            private Domain.Signal SignalWith(Domain.DataType dataType, Domain.Granularity granularity, Domain.Path path, int? id = null)
            {
                return new Domain.Signal()
                {
                    Id = id,
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }

            private void GivenNoSignals_SetupSignalsRepositoryMock()
            {
                SetupWebService();

                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);        
            }

            private void GivenASignal_SetupSignalsRepositoryMock(Domain.Signal existingSignal)
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);

                signalsRepositoryMock.Setup(sr => sr.Get(existingSignal.Path)).Returns(existingSignal);
            }

            private void GivenAnySignal_SetupSignalsRepositoryMock()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                int signalId = 3;
                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Domain.Signal() { Id = signalId});

                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new NoneQualityMissingValuePolicyBoolean());

                this.signalsRepositoryMock
                    .Setup(x => x.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>( y => { return new Signal() { Id = signalId }; });
            }

            private void GivenRepositoryThatAssigns(int id)
            {
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s =>
                    {
                        s.Id = id;
                        return s;
                    });
            }

            private void AssertSignalsAreEqual(Dto.Signal signal1, Dto.Signal signal2)
            {
                Assert.AreEqual(signal1.Id, signal2.Id);
                Assert.AreEqual(signal1.DataType, signal2.DataType);
                Assert.AreEqual(signal1.Granularity, signal2.Granularity);
                CollectionAssert.AreEqual(signal1.Path.Components.ToArray(), signal2.Path.Components.ToArray());
            }

            private bool DatumsAreAscending(IEnumerable<Datum<int>> datums)
            {
                var sortedDatums = datums.OrderBy(dat => dat.Timestamp);

                return datums.SequenceEqual(sortedDatums);
            }

            private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
        }
    }
}