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
            [ExpectedException(typeof(System.ArgumentException))]
            public void GivenNonExistingSignalId_WhenSettingMissingValuePolicy_ThrowsArgumentException()
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

                missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>())).Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyInteger());

                signalsWebService.GetMissingValuePolicy(0);
            }

            [TestMethod]
            public void GivenSignalId_WhenGettingMissingValuePolicy_IdIsPassedToDomainToGetSignalById()
            {
                SetupWebService();
                int id = 1;

                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(new Domain.Signal());

                missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>())).Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyInteger());

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

                var expected = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyInteger()
                {
                    Id = id,
                    Quality = Quality.Good,
                    Signal = signal,
                    Value = (int)2
                };

                missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>())).Returns(expected);

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
            [ExpectedException(typeof(System.ArgumentException))]
            public void GivenNonExistingId_WhenGettingMissingValuePolicy_ThrowsArgumentException()
            {
                SetupWebService();

                int nonExistingId = 4;

                signalsRepositoryMock.Setup(srm => srm.Get(nonExistingId)).Returns((Domain.Signal)null);

                signalsWebService.GetMissingValuePolicy(nonExistingId);
            }

            [TestMethod]
            public void GivenNoData_WhenSettingData_DoesNotThrow()
            {
                SetupWebService();

                var dataDto = new Dto.Datum[] {  };

                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Signal());

                signalsWebService.SetData(0, dataDto);
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

                var dataDto = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = (int)2 } };

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

                var dataDto = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = (int)2 } };

                var signalDomain = SignalWith(DataType.Integer, Granularity.Day, Path.FromString("root/signal"), id);

                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(signalDomain);

                signalsWebService.SetData(id, dataDto);

                VerifySetDataCallOnSignalsDataRepositoryMock<int>(signalDomain, new System.DateTime(2000, 1, 1), 2);
            }

            [TestMethod]
            [ExpectedException(typeof(System.ArgumentException))]
            public void GivenNonExistingSignalId_WhenSettingData_ThrowsArgumentException()
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

                var dataDtoDouble = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = (double)2.5 } };

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

                var dataDtoDecimal = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = timeStamp, Value = (decimal)2.5m } };
                var signalDomainDecimal = SignalWith(DataType.Decimal, Granularity.Day, Path.FromString("root/signal"), id);
                GivenASignalAndData_SetupSignalsRepositoryMockAndVerifySetDataCall<decimal>(signalDomainDecimal, dataDtoDecimal, timeStamp, 2.5m);

                var dataDtoBoolean = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = true } };
                var signalDomainBoolean = SignalWith(DataType.Boolean, Granularity.Day, Path.FromString("root/signal"), id);
                GivenASignalAndData_SetupSignalsRepositoryMockAndVerifySetDataCall<bool>(signalDomainBoolean, dataDtoBoolean, timeStamp, true);

                var dataDtoString = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = "aa" } };
                var signalDomainString = SignalWith(DataType.String, Granularity.Day, Path.FromString("root/signal"), id);
                GivenASignalAndData_SetupSignalsRepositoryMockAndVerifySetDataCall<string>(signalDomainString, dataDtoString, timeStamp, "aa");
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
            public void GivenDataAndSignal_WhenSetData_VerifyIfAddedAllDatums_ForDayGranularity()
            {
                int signalId = 6;

                Dto.Datum[] settedData = new Dto.Datum[]{
                     new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1), Value = (double) 1},
                     new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 4, 1, 1, 1), Value = (double) 3}};

                SetupWebService();

                signalsRepositoryMock.Setup(srm => srm.Get(It.Is<int>(id => id == signalId)))
                    .Returns(new Domain.Signal() { DataType = DataType.Double, Granularity = Granularity.Day });

                signalsWebService.SetData(signalId, settedData);

                signalsDataRepositoryMock.Verify(sdr => sdr.SetData<double>(It.Is<IEnumerable<Datum<double>>>(d =>
                d.ElementAt(1).Timestamp == new DateTime(2000, 1, 2, 1, 1, 1)
                && d.ElementAt(1).Quality == Quality.None
                && d.ElementAt(1).Value == (double)0
                && d.ElementAt(2).Timestamp == new DateTime(2000, 1, 3, 1, 1, 1)
                && d.ElementAt(2).Quality == Quality.None
                && d.ElementAt(2).Value == (double)0)));
            }

            [TestMethod]
            public void WhenGettingDataDoesNotThrow()
            {
                SetupWebService();

                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Domain.Signal());

                signalsWebService.GetData(1, System.DateTime.MinValue, System.DateTime.MaxValue);
            }

            [TestMethod]
            public void GivenSignalId_WhenGettingData_CallsGetByIdWithPassedId()
            {
                SetupWebService();
                int id = 3;

                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Domain.Signal());

                signalsWebService.GetData(id, System.DateTime.MinValue, System.DateTime.MaxValue);

                signalsRepositoryMock.Verify(srm => srm.Get(id), Times.Once);
            }

            [TestMethod]
            [ExpectedException(typeof(System.ArgumentException))]
            public void GivenNonExistingId_WhenGettingData_ThrowsArgumentException()
            {
                SetupWebService();
                int nonExistingId = 12;

                signalsRepositoryMock.Setup(srm => srm.Get(nonExistingId)).Returns((Domain.Signal)null);

                signalsWebService.GetData(nonExistingId, System.DateTime.MinValue, System.DateTime.MaxValue);
            }
    
            [TestMethod]
            public void GivenFromDateLaterThanToDate_WhenGettingData_ReturnsEmtptyDataScope()
            {
                int signalId = 5;

                SetupWebService();
                DateTime invalidFromDate = new DateTime(2001, 2, 1);
                DateTime invalidToDate = new DateTime(2000, 1, 1);

                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Domain.Signal());

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

                signalsWebService.GetData(id, DateTime.MinValue, DateTime.MaxValue);

                VerifyGetDataCallOnSignalsDataRepositoryMock<int>(signal, DateTime.MinValue, DateTime.MaxValue);
            }

            [TestMethod]
            public void GivenSignalsOfDifferentDataTypes_WhenGettingData_RepositoryGetDataIsCalledWithCorrectDataType()
            {
                SetupWebService();
                int id = 2;

                DateTime dateFrom = new DateTime(2000, 1, 1);
                DateTime dateTo = new DateTime(2000, 1, 5);

                var signalDouble = SignalWith(DataType.Double, Granularity.Day, Path.FromString("root/signal"), id);
                GivenASignal_SetupSignalsRepositoryMockAndVerifyGetDataCall<double>(signalDouble, dateFrom, dateTo);

                var signalDecimal = SignalWith(DataType.Decimal, Granularity.Day, Path.FromString("root/signal"), id);
                GivenASignal_SetupSignalsRepositoryMockAndVerifyGetDataCall<decimal>(signalDecimal, dateFrom, dateTo);

                var signalBoolean = SignalWith(DataType.Boolean, Granularity.Day, Path.FromString("root/signal"), id);
                GivenASignal_SetupSignalsRepositoryMockAndVerifyGetDataCall<bool>(signalBoolean, dateFrom, dateTo);

                var signalString = SignalWith(DataType.String, Granularity.Day, Path.FromString("root/signal"), id);
                GivenASignal_SetupSignalsRepositoryMockAndVerifyGetDataCall<string>(signalString, dateFrom, dateTo);
            }

            [TestMethod]
            public void GivenASignalIdMatchingSignalOfDataTypeInteger_WhenGettingData_ReturnsExpectedData()
            {
                SetupWebService();
                int id = 6;

                DateTime dateFrom = new DateTime(2001, 1, 1);
                DateTime dateTo = new DateTime(2001, 1, 7);

                var signal = SignalWith(DataType.Integer, Granularity.Month, Path.FromString("root/signalInt"), id);
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

                AssertDataIsEqual(result, expectedData);
            }

            [TestMethod]
            public void GivenSignalIdsMatchingSignalOfDifferentDataTypes_WhenGettingData_ReturnsExpectedData()
            {
                SetupWebService();
                int id = 6;
                DateTime dateFrom = new DateTime(2001, 1, 1);
                DateTime dateTo = new DateTime(2001, 1, 7);

                var signalDouble = SignalWith(DataType.Double, Granularity.Month, Path.FromString("root/signal"), id);
                var expectedDataDouble = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = (double)2.5 } };
                var dataReturnedDouble = new Domain.Datum<double>[] { new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)2.5 } };
                SetupRepositoryMocks_CallGetData_CompareReturnedData<double>(signalDouble, expectedDataDouble, dataReturnedDouble, dateFrom, dateTo);

                var signalDecimal = SignalWith(DataType.Decimal, Granularity.Month, Path.FromString("root/signal"), id);
                var expectedDataDecimal = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = (decimal)2.5m } };
                var dataReturnedDecimal = new Domain.Datum<decimal>[] { new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (decimal)2.5m } };
                SetupRepositoryMocks_CallGetData_CompareReturnedData<decimal>(signalDecimal, expectedDataDecimal, dataReturnedDecimal, dateFrom, dateTo);

                var signalBoolean = SignalWith(DataType.Boolean, Granularity.Month, Path.FromString("root/signal"), id);
                var expectedDataBoolean = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = true } };
                var dataReturnedBoolean = new Domain.Datum<bool>[] { new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = true } };
                SetupRepositoryMocks_CallGetData_CompareReturnedData<bool>(signalBoolean, expectedDataBoolean, dataReturnedBoolean, dateFrom, dateTo);

                var signalString = SignalWith(DataType.String, Granularity.Month, Path.FromString("root/signal"), id);
                var expectedDataString = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = "aa" } };
                var dataReturnedString = new Domain.Datum<string>[] { new Datum<string>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = "aa" } };
                SetupRepositoryMocks_CallGetData_CompareReturnedData<string>(signalString, expectedDataString, dataReturnedString, dateFrom, dateTo);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_ReturnsEmptySignal_ExpectedException()
            {
                int singnalId = 5;
                SetupWebService();
        
                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Domain.Signal());

                var result = signalsWebService.GetMissingValuePolicy(singnalId);
            }

            [TestMethod]
            public void GivenASignal_WhenGetsMissingValuePolicy_ReturnsSignalWithPolicy()
            {
                int signalId = 5;
                Dto.Signal addedSignal = new Dto.Signal()
                {
                    Id = signalId,
                    DataType = Dto.DataType.Boolean,
                    Granularity = Dto.Granularity.Month,
                    Path = new Dto.Path() { Components = new[] { "root", "signal" } }
                };

                GivenAnySignal_SetupSignalsRepositoryMock();

                Dto.Signal returnedSignal = signalsWebService.Add(addedSignal);

                var result = signalsWebService.GetMissingValuePolicy(returnedSignal.Id.Value);
            }

            [TestMethod]
            public void GivenASignal_WhenSetsMissingValuePolicy_VerifyRepo_CheckIfWorksForAllTypes()
            {
                int signalId = 3;
                GivenNoSignals_SetupSignalsRepositoryMock();

                signalsWebService.Add(new Dto.Signal()
                {
                    Id = signalId,
                    DataType = Dto.DataType.Double,
                    Granularity = Dto.Granularity.Hour,
                    Path = new Dto.Path() { Components = new[] { "root", "signal5" } }
                });

                missingValuePolicyRepositoryMock.Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(),
                    It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>()));
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
                AssertDataIsEqual(result, expectedData);
            }

            private void SetupRepositoryMocks_GetData_ReturnsGivenDataCollection<T>(int id, Domain.Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc, IEnumerable<Domain.Datum<T>> dataReturned)
            {
                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(signal);

                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<T>(It.IsAny<Signal>(), fromIncludedUtc, toExcludedUtc)).Returns(dataReturned);
            }

            private void AssertDataIsEqual(IEnumerable<Dto.Datum> data1, IEnumerable<Dto.Datum> data2)
            {
                Assert.AreEqual(data1.Count(), data2.Count());
                for (int i = 0; i < data1.Count(); i++)
                {
                    Assert.AreEqual(data1.ElementAt(i).Quality, data2.ElementAt(i).Quality);
                    Assert.AreEqual(data1.ElementAt(i).Timestamp, data2.ElementAt(i).Timestamp);
                    Assert.AreEqual(data1.ElementAt(i).Value, data2.ElementAt(i).Value);
                }
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

                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Domain.Signal());
                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new NoneQualityMissingValuePolicyBoolean());
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