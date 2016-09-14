using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using DataAccess.GenericInstantiations;
using Domain.MissingValuePolicy;
using System;
using Domain.Exceptions;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;

            [TestMethod]
            public void GivenASignalWithSpecificDataAndZOMVP_WhenGettingData_ReturnedIsExpectedResult()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Boolean, Granularity.Day, Domain.Path.FromString("root/signal")));
                GivenMissingValuePolicy(signalId, new ZeroOrderMissingValuePolicyBoolean());

                var data = new Datum<bool>[] 
                {
                    new Datum<bool>()
                    {
                        Quality = Quality.Bad,
                        Timestamp = new DateTime(2000, 1, 1),
                        Value = false
                    },
                    new Datum<bool>()
                    {
                        Quality = Quality.Bad,
                        Timestamp = new DateTime(2000, 1, 5),
                        Value = true
                    }
                };

                GivenDatum(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 10), data);

                signalsDataRepoMock.Setup(sdr => sdr.GetDataOlderThan<bool>(It.Is<Signal>
                    (s => s.Id.Value == signalId),
                    It.Is<DateTime>(dt => dt >= new DateTime(2000, 1, 2) && dt <= new DateTime(2000, 1, 5)), 1))
                    .Returns(new Datum<bool>[] { new Datum<bool>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = false } });

                signalsDataRepoMock.Setup(sdr => sdr.GetDataOlderThan<bool>(It.Is<Signal>
                    (s => s.Id.Value == signalId),
                    It.Is<DateTime>(dt => dt >= new DateTime(2000, 1, 6) && dt <= new DateTime(2000, 1, 10)), 1))
                .Returns(new Datum<bool>[] { new Datum<bool>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 5), Value = true } });

                var expectedResult = new Datum<bool>[]
                {
                    new Datum<bool>() { Timestamp = new DateTime(2000,1,1), Quality = Quality.Bad, Value = false },
                    new Datum<bool>() { Timestamp = new DateTime(2000,1,2), Quality = Quality.Bad, Value = false },
                    new Datum<bool>() { Timestamp = new DateTime(2000,1,3), Quality = Quality.Bad, Value = false },
                    new Datum<bool>() { Timestamp = new DateTime(2000,1,4), Quality = Quality.Bad, Value = false },
                    new Datum<bool>() { Timestamp = new DateTime(2000,1,5), Quality = Quality.Bad, Value = true },
                    new Datum<bool>() { Timestamp = new DateTime(2000,1,6), Quality = Quality.Bad, Value = true },
                    new Datum<bool>() { Timestamp = new DateTime(2000,1,7), Quality = Quality.Bad, Value = true },
                    new Datum<bool>() { Timestamp = new DateTime(2000,1,8), Quality = Quality.Bad, Value = true },
                    new Datum<bool>() { Timestamp = new DateTime(2000,1,9), Quality = Quality.Bad, Value = true },
                };

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 10)).ToArray();

                for (int i = 0; i < result.Length; i++)
                {
                    Assert.AreEqual(expectedResult[i].Timestamp, result[i].Timestamp);
                    Assert.AreEqual(expectedResult[i].Quality, result[i].Quality.ToDomain<Domain.Quality>());
                    Assert.AreEqual(expectedResult[i].Value, result[i].Value);
                }
            }

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
            public void GivenNoSignals_WhenGettingById_DoesNotThrow()
            {
                GivenNoSignals();

                signalsWebService.GetById(0);
            }


            [TestMethod]
            public void GivenASignal_WhenGettingByItsId_ReturnsIt()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Year,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.GetById(signalId);

                Assert.AreEqual(signalId, result.Id);
                Assert.AreEqual(Dto.DataType.String, result.DataType);
                Assert.AreEqual(Dto.Granularity.Year, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
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
            public void GivenNoSignals_WhenGettingById_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.GetById(0);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingByPath_DoesNotThrow()
            {
                GivenNoSignals();

                signalsWebService.Get(new Dto.Path() { Components = new[] { "root", "signal" } });
            }
            [TestMethod]
            public void GivenASignal_WhenGettingByPath_ReturnsTheSignalWithSamePath()
            {
                int id = 1;
                var path = new Dto.Path() { Components = new[] { "root", "signal" } };
                var signal = new Signal()
                {
                    Id = id,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day,
                    Path = path.ToDomain<Domain.Path>()
                };

                GivenASignal(signal);

                var result = signalsWebService.Get(path);
                Assert.IsNotNull(result);
                CollectionAssert.AreEqual(path.Components.ToArray(), result.Path.Components.ToArray());
            }

            [ExpectedException(typeof(SignalNotFoundException))]
            [TestMethod]
            public void GivenNoSignals_WhenSettingDataForSignal_ThrowsSignalNotFoundException()
            {
                GivenNoSignals();
                int id = 5;
                signalsWebService.SetData(id, null);
            }

            [TestMethod]
            public void GivenASignalAndDataOfDoubles_WhenSettingDataForSignal_DataRepositorySetDataIsCalled()
            {
                int id = 1;
                Dto.Datum[] dtoData;
                GivenASignalAndDataOf(DataType.Double, 1.0, out dtoData, id);

                signalsWebService.SetData(id, dtoData);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<double>>>()));
            }

            [TestMethod]
            public void GivenASignalAndNoData_WhenSettingDataForSignal_ArgumentNullExceptionIsNotThrown()
            {
                int id = 1;
                var path = new Dto.Path() { Components = new[] { "root", "signal" } };
                var signal = SignalWith(id, DataType.Double, Granularity.Day, path.ToDomain<Domain.Path>());

                GivenASignal(signal);

                signalsWebService.SetData(id, null);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<double>>>()));
            }

            [TestMethod]
            public void GivenASignalAndDataOfBools_WhenSettingDataForSignal_DoesNotThrow()
            {
                int id = 1;
                Dto.Datum[] dtoData;
                GivenASignalAndDataOf(DataType.Boolean, false, out dtoData, id);

                signalsWebService.SetData(id, dtoData);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<bool>>>()));
            }

            [TestMethod]
            public void GivenASignalAndDataOfIntegers_WhenSettingDataForSignal_DoesNotThrow()
            {
                int id = 1;
                Dto.Datum[] dtoData;
                GivenASignalAndDataOf(DataType.Integer, 1, out dtoData, id);

                signalsWebService.SetData(id, dtoData);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<int>>>()));
            }

            [TestMethod]
            public void GivenASignalAndDataOfDecimals_WhenSettingDataForSignal_DoesNotThrow()
            {
                int id = 1;
                Dto.Datum[] dtoData;
                GivenASignalAndDataOf(DataType.Decimal, 99m, out dtoData, id);

                signalsWebService.SetData(id, dtoData);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<decimal>>>()));
            }

            [TestMethod]
            public void GivenASignalAndDataOfStrings_WhenSettingDataForSignal_DoesNotThrow()
            {
                int id = 1;
                Dto.Datum[] dtoData;
                GivenASignalAndDataOf(DataType.String, "str", out dtoData, id);

                signalsWebService.SetData(id, dtoData);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<string>>>()));
            }
            [TestMethod]
            public void GivenASignalAndEmptyData_WhenSettingDataForSignal_doesNotThorw()
            {
                GivenASignal(SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("x")));

                signalsWebService.SetData(1, new Dto.Datum[0]);
            }


            [ExpectedException(typeof(SignalNotFoundException))]
            [TestMethod]
            public void GivenNoSignals_WhenGettingData_ThrowsSignalNotFoundException()
            {
                GivenNoSignals();

                int id = 5;
                DateTime dateFrom = new DateTime(2000, 1, 1), dateTo = new DateTime(2000, 3, 1);

                signalsWebService.GetData(id, dateFrom, dateTo);
            }

            [TestMethod]
            public void GivenASignalAndDataOfIntegers_WhenGettingItsData_DataRepositoryGetDataIsCalled()
            {
                int id = 1;
                DateTime dateFrom = new DateTime(2000, 1, 1), dateTo = new DateTime(2000, 3, 1);

                var signal = GivenASignalAndDataRepositoryWithSetups(dateFrom, DataType.Integer, 1, id);

                signalsWebService.GetData(id, dateFrom, dateTo);
                signalsDataRepoMock.Verify(sd => sd.GetData<int>(It.IsAny<Domain.Signal>(), dateFrom, dateTo));
            }

            [TestMethod]
            public void GivenASignalAndDataOfIntegers_WhenGettingItsData_ResultsTimestampsAreEqualToSignalsDataTimestamps()
            {
                int id = 1;
                int numberOfDatums = 3;
                int numberOfDatumsFromPeriod = numberOfDatums - 1;
                DateTime dateFrom = new DateTime(2000, 1, 1), dateTo = dateFrom.AddMonths(numberOfDatumsFromPeriod);

                var signal = GivenASignalAndDataRepositoryWithSetups(dateFrom, DataType.Integer, 1, id, numberOfDatums);

                var result = signalsWebService.GetData(id, dateFrom, dateTo);

                Assert.IsNotNull(result);
                Assert.AreEqual(numberOfDatumsFromPeriod, result.Count());

                var result_array = result.ToArray();
                for (int i = 0; i < numberOfDatumsFromPeriod; ++i)
                    Assert.AreEqual(dateFrom.AddMonths(i), result_array[i].Timestamp);
            }

            [TestMethod]
            public void GivenASignalAndDataOfDoubles_WhenGettingItsData_ResultsTimestampsAreEqualToSignalsDataTimestamps()
            {
                int id = 1;
                int numberOfDatums = 3;
                int numberOfDatumsFromPeriod = numberOfDatums - 1;
                DateTime dateFrom = new DateTime(2000, 1, 1), dateTo = dateFrom.AddMonths(numberOfDatumsFromPeriod);

                var signal = GivenASignalAndDataRepositoryWithSetups(dateFrom, DataType.Double, 1.0, id, numberOfDatums);

                var result = signalsWebService.GetData(id, dateFrom, dateTo);

                Assert.IsNotNull(result);
                Assert.AreEqual(numberOfDatumsFromPeriod, result.Count());

                var result_array = result.ToArray();
                for (int i = 0; i < numberOfDatumsFromPeriod; ++i)
                    Assert.AreEqual(dateFrom.AddMonths(i), result_array[i].Timestamp);
            }

            [TestMethod]
            public void GivenASignalAndDataOfBooleans_WhenGettingItsData_DoesNotThrow()
            {
                int id = 1;
                int numberOfDatums = 3;
                int numberOfDatumsFromPeriod = numberOfDatums - 1;
                DateTime dateFrom = new DateTime(2000, 1, 1), dateTo = dateFrom.AddMonths(numberOfDatums);

                var signal = GivenASignalAndDataRepositoryWithSetups(dateFrom, DataType.Boolean, true, id, numberOfDatums);

                var result = signalsWebService.GetData(id, dateFrom, dateTo);
            }

            [TestMethod]
            public void GivenASignalAndDataOfDecimals_WhenGettingItsData_DoesNotThrow()
            {
                int id = 1;
                int numberOfDatums = 3;
                int numberOfDatumsFromPeriod = numberOfDatums - 1;
                DateTime dateFrom = new DateTime(2000, 1, 1), dateTo = dateFrom.AddMonths(numberOfDatums);

                var signal = GivenASignalAndDataRepositoryWithSetups(dateFrom, DataType.Decimal, 99m, id, numberOfDatums);

                var result = signalsWebService.GetData(id, dateFrom, dateTo);
            }

            [TestMethod]
            public void GivenASignalAndDataOfString_WhenGettingItsData_DoesNotThrow()
            {
                int id = 1;
                int numberOfDatums = 3;
                int numberOfDatumsFromPeriod = numberOfDatums - 1;
                DateTime dateFrom = new DateTime(2000, 1, 1), dateTo = dateFrom.AddMonths(numberOfDatums);

                var signal = GivenASignalAndDataRepositoryWithSetups(dateFrom, DataType.String, "str", id, numberOfDatums);

                var result = signalsWebService.GetData(id, dateFrom, dateTo);
            }

            [ExpectedException(typeof(SignalNotFoundException))]
            [TestMethod]
            public void GivenNoSignals_SettingMissingValuePolicy_ThrowsSignalNotFoundException()
            {
                int id = 5;
                GivenNoSignals();

                signalsWebService.SetMissingValuePolicy(id, null);
            }

            [TestMethod]
            public void GivenASignalAndAPolicy_SettingMissingValuePolicy_MVPReposSetIsCalled()
            {
                int signalId = 1;
                var path = new Dto.Path() { Components = new[] { "root", "signal" } };
                var signal = SignalWith(signalId, DataType.Integer, Granularity.Day, path.ToDomain<Domain.Path>());

                GivenASignal(signal);

                var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
                signalsWebService.SetMissingValuePolicy(signalId, policy);

                mvpRepositoryMock.Verify(
                    m => m.Set(
                        It.Is<Domain.Signal>(s => s.Id == signalId),
                        It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()
                    )
                );
            }

            [ExpectedException(typeof(SignalNotFoundException))]
            [TestMethod]
            public void GivenNoSignals_GettingMissingValuePolicy_ThrowsSignalNotFoundException()
            {
                int id = 5;
                GivenNoSignals();

                signalsWebService.GetMissingValuePolicy(id);
            }

            [TestMethod]
            public void GivenASignal_GettingItsMissingValuePolicy_MVPRepositorysGetIsCalled()
            {
                int signalId = 1;
                var path = new Dto.Path() { Components = new[] { "root", "signal" } };
                var signal = SignalWith(signalId, DataType.Integer, Granularity.Day, path.ToDomain<Domain.Path>());

                GivenASignal(signal);

                signalsWebService.GetMissingValuePolicy(signalId);
                mvpRepositoryMock.Verify(m => m.Get(It.IsAny<Signal>()));
            }

            [TestMethod]
            public void GivenASignalWithAPolicy_GettingItsMissingValuePolicy_ReturnsNotNull()
            {
                int signalId = 1;
                var path = new Dto.Path() { Components = new[] { "root", "signal" } };
                var signal = SignalWith(signalId, DataType.Integer, Granularity.Day, path.ToDomain<Domain.Path>());

                GivenASignal(signal);

                mvpRepositoryMock
                    .Setup(m => m.Get(It.Is<Signal>(s => s.Id == signalId)))
                    .Returns(new SpecificValueMissingValuePolicyInteger());

                var result = signalsWebService.GetMissingValuePolicy(signalId);

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_SignalsMVPIsSetToNoneQualityMVP()
            {
                var signalId = 1;

                GivenNoSignals();
                GivenRepositoryThatAssigns(id: signalId);

                var signal = signalsWebService.Add(new Dto.Signal() { DataType = Dto.DataType.Boolean });

                mvpRepositoryMock.Verify(mvp => mvp.Set(It.Is<Domain.Signal>(s => s.Id == signalId), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<bool>>()));
            }

            [TestMethod]
            public void GivenASignalAndDataAndMVPByDay_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP()
            {
                int signalId = 1;
                GivenASignal(SignalWith(
                    signalId,
                    DataType.Boolean,
                    Granularity.Day,
                    Path.FromString("")));

                GivenData(signalId, new[]
                {
                    new Datum<bool> {Quality = Quality.Fair, Timestamp = new DateTime() },
                    new Datum<bool> {Quality = Quality.Good, Timestamp = new DateTime().AddDays(2) }
                });

                GivenMissingValuePolicy(signalId, new NoneQualityMissingValuePolicyBoolean());

                var result = signalsWebService.GetData(signalId, new DateTime(), new DateTime().AddDays(3));

                Assert.IsTrue(result.Count() == 3);
                Assert.IsTrue(result.Any(d => d.Quality == Dto.Quality.None));
                CollectionAssert.AreEquivalent(
                    new[]
                    {
                        new DateTime(),
                        new DateTime().AddDays(1),
                        new DateTime().AddDays(2),
                    },
                    result.Select(d => d.Timestamp).ToArray());
            }

            [TestMethod]
            public void GivenASignalAndDataAndMVPByMonth_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP()
            {
                int signalId = 1;
                GivenASignal(SignalWith(
                    signalId,
                    DataType.Boolean,
                    Granularity.Month,
                    Path.FromString("")));

                GivenData(signalId, new[]
                {
                    new Datum<bool> {Quality = Quality.Fair, Timestamp = new DateTime() },
                    new Datum<bool> {Quality = Quality.Good, Timestamp = new DateTime().AddMonths(3) }
                });

                GivenMissingValuePolicy(signalId, new NoneQualityMissingValuePolicyBoolean());

                var result = signalsWebService.GetData(signalId, new DateTime(), new DateTime().AddMonths(4));

                Assert.IsTrue(result.Count() == 4);
                Assert.IsTrue(result.Any(d => d.Quality == Dto.Quality.None));
                CollectionAssert.AreEquivalent(
                    new[]
                    {
                        new DateTime(),
                        new DateTime().AddMonths(1),
                        new DateTime().AddMonths(2),
                        new DateTime().AddMonths(3)
                    },
                    result.Select(d => d.Timestamp).ToArray());
            }

            [TestMethod]
            public void GivenASignalAndDataAndMVPBySecond_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP()
            {
                GivenASignalAndDataAndMVP_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP
                    (Granularity.Second, (i) => new DateTime().AddSeconds(i));
            }

            [TestMethod]
            public void GivenASignalAndDataAndMVPByMinute_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP()
            {
                GivenASignalAndDataAndMVP_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP
                    (Granularity.Minute, (i) => new DateTime().AddMinutes(i));
            }

            [TestMethod]
            public void GivenASignalAndDataAndMVPByHour_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP()
            {
                GivenASignalAndDataAndMVP_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP
                    (Granularity.Hour, (i) => new DateTime().AddHours(i));
            }

            [TestMethod]
            public void GivenASignalAndDataAndMVPByWeek_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP()
            {
                GivenASignalAndDataAndMVP_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP
                    (Granularity.Week, (i) => new DateTime().AddDays(i * 7));
            }

            [TestMethod]
            public void GivenASignalAndDataAndMVPByYear_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP()
            {
                GivenASignalAndDataAndMVP_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP
                    (Granularity.Year, (i) => new DateTime().AddYears(i));
            }            

            [TestMethod]
            public void GivenASignalAndData_WhenGettingData_ReturnsSortedByDate()
            {
                int signalId = 1;
                GivenASignal(SignalWith(
                    signalId,
                    DataType.Boolean,
                    Granularity.Month,
                    Path.FromString("")));

                GivenData(signalId, new[]
                {
                    new Datum<bool> {Quality = Quality.Fair, Timestamp = new DateTime() },
                    new Datum<bool> {Quality = Quality.Bad, Timestamp = new DateTime().AddMonths(2) },
                    new Datum<bool> {Quality = Quality.Good, Timestamp = new DateTime().AddMonths(1) }
                });

                var result = signalsWebService.GetData(signalId, new DateTime(), new DateTime().AddMonths(3));

                CollectionAssert.AreEqual(new[]
                {
                    new DateTime(),
                    new DateTime().AddMonths(1),
                    new DateTime().AddMonths(2)
                },
                result.Select(datum => datum.Timestamp).ToArray());
            }

            [TestMethod]
            public void GivenASignalAndData_WhenCallingGetDataWithTheSameDateInBothArguments_ReturnsOneDatumWithThatDate()
            {
                int signalId = 1;
                GivenASignal(SignalWith(
                    signalId,
                    DataType.Boolean,
                    Granularity.Month,
                    Path.FromString("")));

                DateTime thatDate = new DateTime(2000, 1, 1);
                GivenData<bool>(signalId, 3, thatDate);

                signalsDataRepoMock
                    .Setup(sd => sd.GetData<bool>(It.Is<Signal>(s => s.Id == signalId), thatDate, thatDate))
                    .Returns(new Datum<bool>[] 
                    {
                        new Datum<bool>() {Timestamp = thatDate }
                    });
                var resulttmp = signalsDataRepoMock.Object.GetData<bool>(SignalWith(signalId, DataType.Boolean, Granularity.Month, Path.FromString("")),thatDate,thatDate);

                var result = signalsWebService.GetData(signalId, thatDate, thatDate);

                Assert.AreEqual(1, result.Count());
                Assert.AreEqual(thatDate, result.First().Timestamp);
            }

            [TestMethod]
            public void GivenASignalAndNoData_WhenCallingGetDataWithTheSameDateInBothArguments_ReturnsNotEmptyResult()
            {
                int signalId = 1;
                GivenASignal(SignalWith(
                    signalId,
                    DataType.Boolean,
                    Granularity.Month,
                    Path.FromString("")));

                DateTime thatDate = new DateTime(2000, 1, 1);

                signalsDataRepoMock
                    .Setup(sd => sd.GetData<bool>(It.Is<Signal>(s => s.Id == signalId), thatDate, thatDate))
                    .Returns(new Datum<bool>[] { });

                var result = signalsWebService.GetData(signalId, thatDate, thatDate);

                Assert.AreEqual(1, result.Count());
            }

            [TestMethod]
            public void GivenASignalAndDataWithSpecificValueMVP_CallingGetData_ReturnsMissingDataThatArentOfNoneQuality()
            {
                int signalId = 1;
                GivenASignal(SignalWith(
                    signalId,
                    DataType.Double,
                    Granularity.Month,
                    Path.FromString("")));
                
                Func<int,DateTime> timeChange = (i) => new DateTime(2000, 1, 1).AddMonths(i);

                GivenData(signalId, new[]
                {
                    new Datum<double> {Quality = Quality.Fair, Timestamp = timeChange(0), Value = 1.0},
                    new Datum<double> {Quality = Quality.Good, Timestamp = timeChange(2), Value = 5.0}
                });                
                var mvp = new SpecificValueMissingValuePolicyDouble()
                {
                    Quality = Quality.Bad,
                    Value = 3.0
                };
                GivenMissingValuePolicy(signalId, mvp);

                var result = signalsWebService.GetData(signalId, timeChange(0), timeChange(3));

                Assert.AreEqual(3, result.Count());
                Assert.IsFalse(result.Any(d => d.Quality == Dto.Quality.None));
                CollectionAssert.AreEqual(new[]
                {
                    timeChange(0),
                    timeChange(1),
                    timeChange(2)
                },
                result.Select(datum => datum.Timestamp).ToArray());
            }

            [ExpectedException(typeof(IdNotNullException))]
            [TestMethod]
            public void GivenNoSignals_AddingASignalWithAnId_ThrowsIdNotNullException()
            {
                GivenNoSignals();
                int signalId = 5;
                var sig = SignalWith(Dto.DataType.Boolean, 
                    Dto.Granularity.Day, 
                    new Dto.Path() { Components = new[] { "root", "signal" } });

                sig.Id = signalId;
                signalsWebService.Add(sig);
            }

            [TestMethod]
            public void GivenASignalOfStrings_SettingDataOfNullValue_DoesNotThrow()
            {
                int id = 1;
                Dto.Datum[] dtoData;
                GivenASignalAndDataOf(DataType.String, null, out dtoData, id, 10);

                signalsWebService.SetData(id, dtoData);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<string>>>()));                
            }

            [TestMethod]
            public void GivenNoSignals_GettingPathEntry_CallsRepositoryGetAllWithPathPrefix()
            {
                GivenNoSignals();

                var prefix = new Dto.Path() { Components = new[] { "root" } };
                signalsWebService.GetPathEntry(prefix);

                signalsRepositoryMock.Verify(d => d.GetAllWithPathPrefix(prefix.ToDomain<Domain.Path>()));
            }

            [TestMethod]
            public void GivenSignalsWithTheSamePath_GettingPathEntry_ReturnsNotNull()
            {
                var path = new Dto.Path() { Components = new[] { "root" } };
                var signals = GivenMultipleSignals(path);

                var result = signalsWebService.GetPathEntry(path);
                Assert.IsNotNull(result);                
            }

            [TestMethod]
            public void GivenSignalsInDifferentSubpaths_GettingPathEntry_ReturnsPathEntryWithNotNullSubpaths()
            {
                var path = new Dto.Path() { Components = new[] { "root" } };
                var signals = GivenMultipleSignals(path, true);

                var result = signalsWebService.GetPathEntry(path);
                Assert.IsNotNull(result.SubPaths);
            }

            [TestMethod]
            public void GivenSignalsInDifferentSubpaths_GettingPathEntry_ReturnsPathEntryWithSignalsInAskedPath()
            {
                var path = new Dto.Path() { Components = new[] { "root" } };
                var signals = GivenMultipleSignals(path, true);

                var result = signalsWebService.GetPathEntry(path).Signals.ToArray();
                const int number_of_signals_in_path = 3;

                Assert.AreEqual(number_of_signals_in_path, result.Length);
                foreach (var signal in result)
                    CollectionAssert.AreEqual(signal.Path.Components.Take(path.Components.Count()).ToArray(), path.Components.ToArray());
            }

            [TestMethod]
            public void GivenSignalsInDifferentSubpaths_GettingPathEntry_ReturnsPathEntryWithSignalsInAskedPathAndAllSubpaths()
            {
                var path = new Dto.Path() { Components = new[] { "root" } };
                var signals = GivenMultipleSignals(path, true);

                var result = signalsWebService.GetPathEntry(path).SubPaths.ToArray();
                var subpaths = new Path[] {
                    path.ToDomain<Domain.Path>() + "sub1",
                    path.ToDomain<Domain.Path>() + "sub2"
                };
                
                Assert.AreEqual(subpaths.Length, result.Length);
                for(int i = 0; i < subpaths.Length; ++i)
                    CollectionAssert.AreEqual(subpaths[i].Components.ToArray(),result[i].Components.ToArray());
            }

            [TestMethod]
            [ExpectedException(typeof(DatumTimestampException))]
            public void GivenASignal_HavingInAproppriateDatumTimestamp_SetData_ThrowException()
            {
                int signalId = 1;

                var someSignal = new Signal() { Id = signalId, DataType = DataType.Double, Granularity = Granularity.Month, Path = Domain.Path.FromString("root/s1") };
                var someDatum = new Dto.Datum[] { new Dto.Datum { Quality = Dto.Quality.Fair, Value = 1.5, Timestamp = new DateTime(2000, 1, 1, 12, 0, 0) } };

                GivenASignal(someSignal);

                this.signalsWebService.SetData(signalId, someDatum);
            }

            [TestMethod]
            [ExpectedException(typeof(DatumTimestampException))]
            public void GivenASignal_HavingInAproppriateDatumTimestamp_GetData_ThrowException()
            {
                int signalId = 1;

                var someSignal = new Signal() { Id = signalId, DataType = DataType.Double, Granularity = Granularity.Month, Path = Domain.Path.FromString("root/s1") };

                GivenASignal(someSignal);

                GivenData(signalId, new[]
               {
                    new Datum<double> {Quality = Quality.Fair, Timestamp = new DateTime(2000,1,1,0,0,0), Value = 1.0},
                    new Datum<double> {Quality = Quality.Good, Timestamp = new DateTime(2000,2,1,0,0,0), Value = 5.0}
                });

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1, 15, 0, 0), new DateTime(2000, 5, 1, 0, 0, 0));
            }

            [TestMethod]
            [ExpectedException(typeof(DatumTimestampException))]
            public void GivenASignal_HavingInAproppriateDatumTimestamp_GetData_ThrowException1()
            {
                int signalId = 1;

                var someSignal = new Signal() { Id = signalId, DataType = DataType.Double, Granularity = Granularity.Week, Path = Domain.Path.FromString("root/s1") };

                GivenASignal(someSignal);

                GivenData(signalId, new Datum<double>[0]);

                var result = signalsWebService.GetData(signalId, new DateTime(2018, 1, 2), new DateTime(2018, 1, 8));
            }

            [TestMethod]
            public void GivenASignal_HavingZeroOrderMVPAdded_GetData_ReturnsProperData()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Month, Path.FromString("root/signal")));

                GivenMissingValuePolicy(signalId, new ZeroOrderMissingValuePolicyDouble());

                signalsDataRepoMock
                    .Setup(sdr => sdr.GetData<double>(It.Is<Signal>(s => s.Id == signalId), new DateTime(2000, 1, 1), new DateTime(2000, 5, 1)))
                    .Returns(new Datum<double>[] { new Datum<double> {Quality = Quality.Fair, Timestamp = new DateTime(2000,1,1), Value = 1.0},
                                                                                                       new Datum<double> {Quality = Quality.Good, Timestamp = new DateTime(2000,3,1), Value = 5.0} });

                signalsDataRepoMock.Setup(sdr => sdr.GetDataOlderThan<double>(It.Is<Signal>
                    (s => s.Id == signalId), It.Is<DateTime>(dt => dt > new DateTime(2000, 1, 1) && dt <= new DateTime(2000, 3, 1)), 1))
                .Returns(new Datum<double>[] { new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 1.0 } });

                signalsDataRepoMock.Setup(sdr => sdr.GetDataOlderThan<double>(It.Is<Signal>
                    (s => s.Id == signalId), It.Is<DateTime>(dt => dt > new DateTime(2000, 3, 1) && dt <= new DateTime(2000, 5, 1)), 1))
                .Returns(new Datum<double>[] { new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 5.0 } });

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));

                Assert.AreEqual(4, result.Count());
                Assert.AreEqual(Dto.Quality.Fair, result.ElementAt(1).Quality);
                Assert.AreEqual(5.0, result.ElementAt(3).Value);
            }

            [TestMethod]
            public void GivenASignal_GivenNOData_HavingZeroOrderMVPAdded_GetData_ReturnsDefaultData()
            {
                int signalId = 1;
                var someSignal = new Signal() { Id = signalId, DataType = DataType.Double, Granularity = Granularity.Month, Path = Domain.Path.FromString("root/s1") };

                GivenASignal(someSignal);
                GivenData(signalId, new Datum<double>[0]);
                GivenMissingValuePolicy(signalId, new ZeroOrderMissingValuePolicyDouble());
                
                var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
                signalsWebService.SetMissingValuePolicy(signalId, policy);

                var result = signalsWebService.GetData(signalId, new DateTime(2018, 1, 1), new DateTime(2018, 2, 1));

                Assert.AreEqual(Dto.Quality.None, result.ElementAt(0).Quality);
            }

            [TestMethod]
            public void Delete_GivenASignal_WhenDeleteSignal_DeleteIsCalled()
            {
                GivenASignal(SignalWith(1 ,Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("x")));
                signalsWebService.Delete(1);
                signalsRepositoryMock.Verify(c => c.Delete(It.IsAny<Signal>()));
            }

            [TestMethod]
            public void Delete_GivenASignal_WhenDeleteSignal_SetMissingValueForNull()
            {
                GivenASignal(SignalWith(1, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("x")));
                signalsWebService.Delete(1);

                mvpRepositoryMock.Verify(c => c.Set(It.IsAny<Signal>(), null));
            }

            [TestMethod]
            public void Delete_GivenASignal_WhenDeleteSignal_DeleteAllDatumsForThisSignal()
            {
                GivenASignal(SignalWith(1, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("x")));
                signalsWebService.Delete(1);

                signalsDataRepoMock.Verify(c => c.DeleteData<double>(It.IsAny<Signal>()));
            }

            [TestMethod]
            public void GetData_FirstOrderMissingValuePolicy_WhenTimeIsOutOfBounds_SetNoneQualityValue()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Month, Domain.Path.FromString("root/s1")));

                var datums = new Datum<double>[]
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1.0 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2.0 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5.0 }
                };
                GivenData(signalId, datums);
                GivenMissingValuePolicy(signalId, new FirstOrderMissingValuePolicyDouble());

                var from = new DateTime(1999, 11, 1);
                var to = new DateTime(2000, 11, 1);

                var result = signalsWebService.GetData(signalId, from, to);

                Assert.AreEqual(Dto.Quality.None, result.ElementAt(0).Quality);
                Assert.AreEqual(0.0, result.ElementAt(0).Value);
            }            

            [TestMethod]
            public void GetData_FirstOrderMissingValuePolicy_WhenTimeIsOutOfBounds_SetValidValue()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Month, Domain.Path.FromString("root/s1")));

                var from = new DateTime(1999, 11, 1);
                var to = new DateTime(2000, 11, 1);

                var datums = new Datum<double>[]
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1.0 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2.0 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5.0 }
                };
                
                signalsDataRepoMock.Setup(sdr => sdr.GetDataOlderThan<double>(It.Is<Signal>(s => s.Id == signalId), It.Is<DateTime>(dt => dt > new DateTime(2000, 1, 1) && dt < new DateTime(2000, 5, 1)), 1))
                    .Returns(new Datum<double>[] { new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1.0 } });
                signalsDataRepoMock.Setup(sdr => sdr.GetDataNewerThan<double>(It.Is<Signal>(s => s.Id == signalId), It.Is<DateTime>(dt => dt > new DateTime(2000, 1, 1) && dt < new DateTime(2000, 5, 1)), 1))
                    .Returns(new Datum<double>[] { new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2.0 } });
                signalsDataRepoMock.Setup(sdr => sdr.GetDataOlderThan<double>(It.Is<Signal>(s => s.Id == signalId), It.Is<DateTime>(dt => dt > new DateTime(2000, 5, 1) && dt < new DateTime(2000, 8, 1)), 1))
                    .Returns(new Datum<double>[] { new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2.0 } });
                signalsDataRepoMock.Setup(sdr => sdr.GetDataNewerThan<double>(It.Is<Signal>(s => s.Id == signalId), It.Is<DateTime>(dt => dt > new DateTime(2000, 5, 1) && dt < new DateTime(2000, 8, 1)), 1))
                    .Returns(new Datum<double>[] { new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5.0 } });

                signalsDataRepoMock.Setup(sdr => sdr.GetData<double>(It.Is<Signal>(s => s.Id == signalId), from, to)).Returns(datums);

                GivenMissingValuePolicy(signalId, new FirstOrderMissingValuePolicyDouble());

                var result = signalsWebService.GetData(signalId, from, to);

                Assert.AreEqual(0.0, result.ElementAt(0).Value);
                Assert.AreEqual(1.0, result.ElementAt(2).Value);
                Assert.AreEqual(3.0, result.ElementAt(7).Value);
            }

            [TestMethod]
            public void GetData_FirstOrderMissingValuePolicy_WhenTimeIsBbetweenDatums_SetValidValue()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Month, Domain.Path.FromString("root/s1")));

                var from = new DateTime(2000, 6, 1);
                var to = new DateTime(2000, 7, 1);

                var datums = new Datum<double>[]
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1.0 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2.0 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5.0 }
                };

                signalsDataRepoMock.Setup(sdr => sdr.GetDataOlderThan<double>(It.Is<Signal>(s => s.Id == signalId), It.Is<DateTime>(dt => dt > new DateTime(2000, 5, 1) && dt < new DateTime(2000, 8, 1)), 1))
                    .Returns(new Datum<double>[] { new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2.0 } });
                signalsDataRepoMock.Setup(sdr => sdr.GetDataNewerThan<double>(It.Is<Signal>(s => s.Id == signalId), It.Is<DateTime>(dt => dt > new DateTime(2000, 5, 1) && dt < new DateTime(2000, 8, 1)), 1))
                    .Returns(new Datum<double>[] { new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5.0 } });

                signalsDataRepoMock.Setup(sdr => sdr.GetData<double>(It.Is<Signal>(s => s.Id == signalId), from, to)).Returns(datums);

                mvpRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>(s => s.Id == signalId))).Returns(new FirstOrderMissingValuePolicyDouble());

                var result = signalsWebService.GetData(signalId, from, to);

                Assert.AreEqual(3.0, result.ElementAt(0).Value);
            }

            [TestMethod]
            public void GetData_FirstOrderMissingValuePolicy_WhenDatumsNotSorted_SetValidValue()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Month, Domain.Path.FromString("root/s1")));

                var datums = new Datum<double>[]
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = 1.0 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2.0 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 5.0 }
                };
                GivenData(signalId, datums);
                GivenMissingValuePolicy(signalId, new FirstOrderMissingValuePolicyDouble());

                var from = new DateTime(1999, 11, 1);
                var to = new DateTime(2000, 11, 1);

                var result = signalsWebService.GetData(signalId, from, to);

                Assert.AreEqual(0.0, result.ElementAt(0).Value);
                Assert.AreEqual(5.0, result.ElementAt(2).Value);
                Assert.AreEqual(1.0, result.ElementAt(9).Value);
            }

            [TestMethod]
            public void GetData_FirstOrderMissingValuePolicy_WhenDatumsHaveGranularityWeek_SetValidValue()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Week, Domain.Path.FromString("root/s1")));

                var datums = new Datum<double>[]
                {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 8, 29), Value = 1.0 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 9, 15), Value = 2.0 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 10, 1), Value = 5.0 }
                };
                GivenData(signalId, datums);
                GivenMissingValuePolicy(signalId, new FirstOrderMissingValuePolicyDouble());

                var from = new DateTime(2016, 8, 22);
                var to = new DateTime(2016, 10, 8);

                var result = signalsWebService.GetData(signalId, from, to);

                Assert.AreEqual(1.0, result.ElementAt(1).Value);
                Assert.AreEqual(2.0, result.ElementAt(3).Value);
            }

            [TestMethod]
            [ExpectedException(typeof(IncompatibleShadowSignalException))]
            public void GivenASignalWithGranularitySecondAndDatatypeBool_WhenAssigningToTheSignalSMVPWithSignalWithOtherGranularityAndOtherDatatype_ThrowedIsArgumentException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Boolean, Granularity.Second, Domain.Path.FromString("root/s1")));

                signalsWebService.SetMissingValuePolicy(signalId,
                    new Dto.MissingValuePolicy.ShadowMissingValuePolicy()
                    {
                        DataType = Dto.DataType.Integer,
                        ShadowSignal = new Dto.Signal()
                        {
                            DataType = Dto.DataType.Decimal,
                            Granularity = Dto.Granularity.Day }
                    });
            }

            [TestMethod]
            public void GivenASignal_WhenAssigningToTheSignalSMVPWithSignalWithGranularityAndDatatypeSameAsInGivenSignal_CalledIsMVPRepositorySetMethod()
            {
                int signalId = 1;
                int shadowId = 2;                
                var signals = new Signal[]
                {
                    SignalWith(signalId, DataType.Boolean, Granularity.Second, Domain.Path.FromString("root/s1")),
                    SignalWith(shadowId, DataType.Boolean, Granularity.Second, Domain.Path.FromString("root/shadow"))
                };
                GivenExisitingSignals(signals);
                GivenMissingValuePolicy(2, null);

                var mvp = new ShadowMissingValuePolicy<bool>()
                {
                    Signal = signals[0],
                    ShadowSignal = signals[1]
                };

                signalsWebService.SetMissingValuePolicy(signalId, mvp.ToDto<Dto.MissingValuePolicy.ShadowMissingValuePolicy>());

                mvpRepositoryMock.Verify(mvpr => mvpr.Set(It.Is<Signal>(s => s.Id == signalId), It.IsAny<ShadowMissingValuePolicy<bool>>()));
            }

            [TestMethod]
            public void GivenAnySignalWithSMVPAndAnyShadow_WhenGettingDataByFromDatetimeLaterThanToDatetime_ReturnedIsBlankEnumerable()
            {
                int signalId = 1, shadowId = 2;
                var signals = new Signal[]
                {
                    SignalWith(signalId, DataType.Decimal, Granularity.Day, Path.FromString("root/signal")),
                    SignalWith(shadowId, DataType.Decimal, Granularity.Day, Path.FromString("root/shadow")),
                };
                GivenExisitingSignals(signals);
                GivenMissingValuePolicy(signalId, new ShadowMissingValuePolicyDecimal() { Signal = signals[0], ShadowSignal = signals[1] });

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 2), new DateTime(2000, 1, 1));

                Assert.AreEqual(0, result.ToArray().Length);
            }

            [TestMethod]
            public void GivenASignalWithSMVPAndNoDataAndAShadowWithNoData_WhenGettingDataByFromDatetimeEqualToToDatetime_ReturnedIsEnumerableWithOneElementWithNoneQualityAndDefaultValue()
            {
                int signalId = 1, shadowId = 2;
                var signals = new Signal[]
                {
                    SignalWith(signalId, DataType.Decimal, Granularity.Hour, Path.FromString("root/signal")),
                    SignalWith(shadowId, DataType.Decimal, Granularity.Hour, Path.FromString("root/shadow")),
                };
                GivenExisitingSignals(signals);
                GivenMissingValuePolicy(signalId, new ShadowMissingValuePolicyDecimal() { Signal = signals[0], ShadowSignal = signals[1] });

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));
                
                Assert.AreEqual(Dto.Quality.None, result.ToArray()[0].Quality);
                Assert.AreEqual(0m, result.ToArray()[0].Value);
            }

            [TestMethod]
            public void GivenASignalWithSMVPAndNoDataAndAShadowWithData_WhenGettingDataByFromDatetimeEqualToToDatetimeAndEqualToOneOfShadowsDatumTimestamp_ReturnedIsEnumerableWithOneElementFromShadowsData()
            {
                int signalId = 1, shadowId = 2;
                var signals = new Signal[]
                {
                    SignalWith(signalId, DataType.Decimal, Granularity.Hour, Path.FromString("root/signal")),
                    SignalWith(shadowId, DataType.Decimal, Granularity.Hour, Path.FromString("root/shadow")),
                };
                GivenExisitingSignals(signals);
                GivenMissingValuePolicy(signalId, new ShadowMissingValuePolicyDecimal() { Signal = signals[0], ShadowSignal = signals[1] });

                var shadowData = new Datum<decimal>[] { new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1) },
                                                        new Datum<decimal>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 2) } };

                GivenDatum(shadowId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1), shadowData);

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1)).ToArray()[0];

                Assert.AreEqual(Dto.Quality.Fair, result.Quality);
                Assert.AreEqual(new DateTime(2000, 1, 1), result.Timestamp);
            }

            [TestMethod]
            public void GivenASignalWithDataAndAShadowWithData_WhenGettingDataByFromDatetimeEqualToToDatetimeAndEqualToOneOfSignalsDatumTimestamp_ReturnedIsEnumerableWithOneDatumEqualToOneOfGivenSignalsDatum()
            {
                int signalId = 1, shadowId = 2;
                var signals = new Signal[]
                {
                    SignalWith(signalId, DataType.Decimal, Granularity.Hour, Path.FromString("root/signal")),
                    SignalWith(shadowId, DataType.Decimal, Granularity.Hour, Path.FromString("root/shadow")),
                };
                GivenExisitingSignals(signals);
                GivenMissingValuePolicy(signalId, new ShadowMissingValuePolicyDecimal() { Signal = signals[0], ShadowSignal = signals[1] });

                var date = new DateTime(2000, 1, 1);
                var signalData = new Datum<decimal>[] { new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1) },
                                                        new Datum<decimal>() { Quality = Quality.None, Timestamp = new DateTime(2000, 1, 2) } };
                var shadowData = new Datum<decimal>[] { new Datum<decimal>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1) },
                                                        new Datum<decimal>() { Quality = Quality.None, Timestamp = new DateTime(2000, 1, 2) } };
                GivenDatum(signalId, date, date, signalData);
                GivenDatum(shadowId, date, date, shadowData);

                var result = signalsWebService.GetData(signalId, date, date).ToArray()[0];

                Assert.AreEqual(Dto.Quality.Good, result.Quality);
                Assert.AreEqual(new DateTime(2000, 1, 1), result.Timestamp);
            }

            [TestMethod]
            public void GivenASignalWithNoDataAndAShadowWithNoData_WhenGettingDataByFromDatetimeEarlierThanTo_ReturnedIsEnumerableWhereAllDatumsAreWithNoneQualityAndDefaultValue()
            {
                int signalId = 1, shadowId = 2;
                var signals = new Signal[]
                {
                    SignalWith(signalId, DataType.Decimal, Granularity.Month, Path.FromString("root/signal")),
                    SignalWith(shadowId, DataType.Decimal, Granularity.Month, Path.FromString("root/shadow")),
                };
                GivenExisitingSignals(signals);
                GivenMissingValuePolicy(signalId, new ShadowMissingValuePolicyDecimal() { Signal = signals[0], ShadowSignal = signals[1] });

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                Assert.AreEqual(3, result.ToArray().Length);
                foreach (var datum in result)
                {
                    Assert.AreEqual(Dto.Quality.None, datum.Quality);
                    Assert.AreEqual(0m, datum.Value);
                }
            }

            [TestMethod]
            public void GivenASignalWithSMVPAndDataAndShadowWithNoData_WhenGettingDataByFromdatetimeEarlierThanTo_ReturnedIsDataWhereEveryDatumIsDefaultOrComesFromSignalsData()
            {
                int signalId = 1, shadowId = 2;
                var signals = new Signal[]
                {
                    SignalWith(signalId, DataType.Decimal, Granularity.Month, Path.FromString("root/signal")),
                    SignalWith(shadowId, DataType.Decimal, Granularity.Month, Path.FromString("root/shadow")),
                };
                GivenExisitingSignals(signals);
                GivenMissingValuePolicy(signalId, new ShadowMissingValuePolicyDecimal() { Signal = signals[0], ShadowSignal = signals[1] });

                var data = new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000, 1, 1), Value = 10m, Quality = Quality.Good } };
                var fromDate = new DateTime(2000, 1, 1);
                var toDate = new DateTime(2000, 4, 1);
                GivenDatum(signalId, fromDate, toDate, data);

                var expectedResult = new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000,1,1), Value = 10m, Quality = Quality.Good},
                                                            new Datum<decimal>() { Timestamp = new DateTime(2000,2,1), Value = 0m, Quality = Quality.None},
                                                            new Datum<decimal>() { Timestamp = new DateTime(2000,3,1), Value = 0m, Quality = Quality.None} };

                var result = signalsWebService.GetData(signalId, fromDate, toDate).ToArray();

                for (int i = 0; i < result.Length; i++)
                {
                    Assert.AreEqual(expectedResult[i].Timestamp, result[i].Timestamp);
                    Assert.AreEqual(expectedResult[i].Value, result[i].Value);
                    Assert.AreEqual(expectedResult[i].Quality, result[i].Quality.ToDomain<Domain.Quality>());
                }
            }

            [TestMethod]
            public void GivenASignalWithDataAndAShadowWithData_WhenGettingDataByFromDatetimeEarlierThanTo_ReturnedIsDataWhereEveryDatumIsDefaultOrComesFromsSignalsDataOrComesFromShadowsData()
            {
                int signalId = 1, shadowId = 2;
                var signals = new Signal[]
                {
                    SignalWith(signalId, DataType.Decimal, Granularity.Month, Path.FromString("root/signal")),
                    SignalWith(shadowId, DataType.Decimal, Granularity.Month, Path.FromString("root/shadow")),
                };
                GivenExisitingSignals(signals);
                GivenMissingValuePolicy(signalId, new ShadowMissingValuePolicyDecimal() { Signal = signals[0], ShadowSignal = signals[1] });

                var fromDate = new DateTime(2000, 1, 1);
                var toDate = new DateTime(2000, 4, 1);
                
                var signalData = new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000, 1, 1), Value = 10m, Quality = Quality.Good } };
                var shadowData = new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000, 1, 1), Value = 10m, Quality = Quality.Bad },
                                                        new Datum<decimal>() { Timestamp = new DateTime(2000, 2, 1), Value = 20m, Quality = Quality.Good }};
                GivenDatum(signalId, fromDate, toDate, signalData);
                GivenDatum(shadowId, fromDate, toDate, shadowData);

                var expectedResult = new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000,1,1), Value = 10m, Quality = Quality.Good},
                                                            new Datum<decimal>() { Timestamp = new DateTime(2000,2,1), Value = 20m, Quality = Quality.Good},
                                                            new Datum<decimal>() { Timestamp = new DateTime(2000,3,1), Value = 0m, Quality = Quality.None} };

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)).ToArray();

                for (int i = 0; i < result.Length; i++)
                {
                    Assert.AreEqual(expectedResult[i].Timestamp, result[i].Timestamp);
                    Assert.AreEqual(expectedResult[i].Value, result[i].Value);
                    Assert.AreEqual(expectedResult[i].Quality, result[i].Quality.ToDomain<Domain.Quality>());
                }
            }

            [TestMethod]
            public void GivenASignalWithSMVPAndDataAndAShadowWithData_WhenGettingDataByFromDatetimeEarlierThanToAndFromAndToDatetimesAreEqualToSomeOfSignalOrShadowDatumsTimestamps_ReturnedIsExpectedResult()
            {
                int signalId = 1, shadowId = 2;
                var signals = new Signal[]
                {
                    SignalWith(signalId, DataType.Decimal, Granularity.Month, Path.FromString("root/signal")),
                    SignalWith(shadowId, DataType.Decimal, Granularity.Month, Path.FromString("root/shadow")),
                };
                GivenExisitingSignals(signals);
                GivenMissingValuePolicy(signalId, new ShadowMissingValuePolicyDecimal() { Signal = signals[0], ShadowSignal = signals[1]});

                var signalData = new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000, 1, 1), Value = 10m, Quality = Quality.Good } };
                var shadowData = new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000, 4, 1), Value = 10m, Quality = Quality.Bad } };

                var fromDate = new DateTime(2000, 1, 1);
                var toDate = new DateTime(2000, 4, 1);
                GivenDatum(signalId, fromDate, toDate, signalData);
                GivenDatum(shadowId, fromDate, toDate, signalData);

                var expectedResult = new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2000,1,1), Value = 10m, Quality = Quality.Good},
                                                            new Datum<decimal>() { Timestamp = new DateTime(2000,2,1), Value = 0m, Quality = Quality.None},
                                                            new Datum<decimal>() { Timestamp = new DateTime(2000,3,1), Value = 0m, Quality = Quality.None} };

                var result = signalsWebService.GetData(signalId, fromDate, toDate).ToArray();

                for (int i = 0; i < result.Length; i++)
                {
                    Assert.AreEqual(expectedResult[i].Timestamp, result[i].Timestamp);
                    Assert.AreEqual(expectedResult[i].Value, result[i].Value);
                    Assert.AreEqual(expectedResult[i].Quality, result[i].Quality.ToDomain<Domain.Quality>());
                }
            }

            [TestMethod]
            [ExpectedException(typeof(DatumTimestampException))]
            public void GivenASignal_WhenSettingDataWhenDataGranularityIsWeekAndTimestampOfOneDatumIsNotMonday_ThrowedIsException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Week, Path.FromString("root/signal")));

                signalsWebService.SetData(signalId, new Dto.Datum[]
                { new Dto.Datum() { Value = (double)1, Timestamp = new DateTime(2018, 1, 2) },
                  new Dto.Datum() { Value = (double)2, Timestamp = new DateTime(2018, 1, 8) }});
            }


            [TestMethod]
            public void GivenASignalWithSpecificDataAndFOMVP_WhenGettingData_ReturnedIsExpectedResult()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Decimal, Granularity.Day, Path.FromString("root/signal")));

                var policy = new FirstOrderMissingValuePolicyDecimal();
                GivenMissingValuePolicy(signalId, policy);

                var fromDate = new DateTime(2018, 1, 1);
                var toDate = new DateTime(2018, 1, 6);
                var data = new Datum<decimal>[] 
                {
                    new Datum<decimal>()
                    {
                        Quality = Quality.Bad,
                        Timestamp = new DateTime(2017, 12, 31),
                        Value = 10m
                    },
                    new Datum<decimal>()
                    {
                        Quality = Quality.Bad,
                        Timestamp = new DateTime(2018, 1, 2),
                        Value = 30m
                    }
                };

                GivenData(signalId, data);

                var expectedResult = new Datum<decimal>[] { new Datum<decimal>() { Timestamp = new DateTime(2018,1,1), Value = 20m, Quality = Quality.Bad},
                                                            new Datum<decimal>() { Timestamp = new DateTime(2018,1,2), Value = 30m, Quality = Quality.Bad },
                                                            new Datum<decimal>() { Timestamp = new DateTime(2018,1,3), Value = 0m, Quality = Quality.None },
                                                            new Datum<decimal>() { Timestamp = new DateTime(2018,1,4), Value = 0m, Quality = Quality.None },
                                                            new Datum<decimal>() { Timestamp = new DateTime(2018,1,5), Value = 0m, Quality = Quality.None } };

                var result = signalsWebService.GetData(signalId, fromDate, toDate).ToArray();

                for (int i = 0; i < result.Length; i++)
                {
                    Assert.AreEqual(expectedResult[i].Timestamp, result[i].Timestamp);
                    Assert.AreEqual(expectedResult[i].Value, result[i].Value);
                    Assert.AreEqual(expectedResult[i].Quality, result[i].Quality.ToDomain<Domain.Quality>());
                }
            }

            [TestMethod]
            public void GivenASignalWithNoDataAndFOMVP_WhenGettingData_ThereIsNotThrowedException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Decimal, Granularity.Minute, Path.FromString("root/signal")));

                var fromDate = new DateTime(2018, 1, 1, 0, 0, 0);
                var toDate = new DateTime(2018, 1, 1, 0, 2, 0);
                GivenDatum<decimal>(signalId, fromDate, toDate);

                var policy = new FirstOrderMissingValuePolicyDecimal();
                GivenMissingValuePolicy(signalId, policy);

                signalsWebService.GetData(signalId, fromDate, toDate);
            }

            [TestMethod]
            public void GivenASignalWithNoDataAndSpecificValueMVP_WhenGettingDataUsingTheSameDate_ReturnsDatumAccordingToSVMVP()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Month, Path.FromString("root/signal")));

                var mvp = new SpecificValueMissingValuePolicyDouble()
                {
                    Quality = Quality.Bad,
                    Value = 3.0
                };
                GivenMissingValuePolicy(signalId, mvp);

                var date = new DateTime(2000, 2, 1);
                var result = signalsWebService.GetData(signalId, date, date).ToArray();

                Assert.AreEqual(1, result.Length);
                Assert.AreEqual(mvp.Quality.ToDto<Dto.Quality>(), result[0].Quality);
                Assert.AreEqual(mvp.Value, result[0].Value);
            }

            [ExpectedException(typeof(DatumTimestampException))]
            [TestMethod]
            public void GivenASignalAndInvalidTimestamp_WhenSettingData_ThrowsDatumTimestampException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Week, Path.FromString("root/signal")));

                signalsWebService.SetData(signalId, new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2018, 1, 2), Value = 2.2} });
            }

            [ExpectedException(typeof(ShadowDependencyCycleException))]
            [TestMethod]
            public void GivenMultipleSignals_SettingShadowMVPCreatingADependencyCycle_ThrowsException()
            {
                var signal_ids = new int[] { 1, 2, 3 };
                var signals = new Signal[] {
                    new Signal() { Id = signal_ids[0], DataType = DataType.Double, Granularity = Granularity.Month, Path = Path.FromString("root/s1") },
                    new Signal() { Id = signal_ids[1], DataType = DataType.Double, Granularity = Granularity.Month, Path = Path.FromString("root/s2") },
                    new Signal() { Id = signal_ids[2], DataType = DataType.Double, Granularity = Granularity.Month, Path = Path.FromString("root/s3") },
                };
                GivenExisitingSignals(signals);
                GivenMissingValuePolicy(signal_ids[0], new ShadowMissingValuePolicyDouble { Signal = signals[0], ShadowSignal = signals[1] });
                GivenMissingValuePolicy(signal_ids[1], new ShadowMissingValuePolicyDouble { Signal = signals[1], ShadowSignal = signals[2] });

                var policy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy
                {
                    DataType = Dto.DataType.Double,
                    Signal = signals[2].ToDto<Dto.Signal>(),
                    ShadowSignal = signals[0].ToDto<Dto.Signal>()
                };

                signalsWebService.SetMissingValuePolicy(signal_ids[2], policy);
            }

            [TestMethod]
            [ExpectedException(typeof(SignalNotFoundException))]
            public void GivenNoSignals_GettingCoarseData_ThrowsSignalNotFoundException()
            {
                GivenNoSignals();
                signalsWebService.GetCoarseData(10, Dto.Granularity.Month, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));
            }

            [TestMethod]
            public void GivenASignal_GettingCoarseDataWithFromDateOlderThanToDate_ReturnsEmptyDatumContainer()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Day, Path.FromString("root/signal")));

                var result = signalsWebService.GetCoarseData(1, Dto.Granularity.Month, new DateTime(2000, 2, 1), new DateTime(2000, 1, 1));

                Assert.AreEqual(0, result.Count());
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidCoarseGranularityException))]
            public void GivenASignal_GettingCoarseDataForGranularitySmallerThanSignals_ThrowsInvalidCoarseGranularityException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Day, Path.FromString("root/signal")));

                var result = signalsWebService.GetCoarseData(1, Dto.Granularity.Second, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidSignalTypeException))]
            public void GivenBooleanAndStringSignals_GettingCoarseData_ThrowsInvalidSignalTypeException()
            {
                int id1 = 1, id2 = 2;
                var signals = new Signal[]
                {
                    SignalWith(id1, DataType.String, Granularity.Day, Path.FromString("root/str")),
                    SignalWith(id2, DataType.Boolean, Granularity.Day, Path.FromString("root/bool")),
                };
                GivenExisitingSignals(signals);

                signalsWebService.GetCoarseData(id1, Dto.Granularity.Month, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));
                signalsWebService.GetCoarseData(id2, Dto.Granularity.Month, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidCoarseGranularityException))]
            public void GivenASignal_GettingCoarseDataWithIrregularTimePeriod_ThrowsException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Day, Path.FromString("root/signal")));

                signalsWebService.GetCoarseData(1, Dto.Granularity.Week, new DateTime(2000, 1, 1), new DateTime(2000, 1, 3));
            }

            [TestMethod]
            public void GivenASignalWithData_GettingCoarseData_ReturnsCoarseData()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Integer, Granularity.Day, Path.FromString("root/signal")));

                Func<int, DateTime> timeChange = (i) => new DateTime().AddDays(i);
                var data = new List<Datum<int>>();
                for(int i = 0; i < 14; ++i)                
                    data.Add(new Datum<int>() { Id = i + 1, Quality = Quality.Bad, Timestamp = timeChange(i), Value = i+1 });

                GivenDatum(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 15), data);
                var result = signalsWebService
                    .GetCoarseData(1, Dto.Granularity.Week, new DateTime(2000, 1, 1), new DateTime(2000, 1, 15))
                    .ToArray();

                Assert.AreEqual(2, result.Length);
                Assert.AreEqual(data[0].Quality.ToDto<Dto.Quality>(), result[0].Quality);
                Assert.AreEqual((1 + 2 + 3 + 4 + 5 + 6 + 7)/7, result[0].Value);
                Assert.AreEqual((8 + 9 + 10 + 11 + 12 + 13 + 14)/7, result[1].Value);
            }

            [TestMethod]
            public void GivenASignalWithData_GettingCoarseDataWithSameTimestamp_ReturnsCoarseDataOfOneElement()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Integer, Granularity.Day, Path.FromString("root/signal")));

                Func<int, DateTime> timeChange = (i) => new DateTime().AddDays(i);
                var data = new List<Datum<int>>();
                for (int i = 0; i < 14; ++i)
                    data.Add(new Datum<int>() { Id = i + 1, Quality = Quality.Bad, Timestamp = timeChange(i), Value = i + 1 });

                GivenDatum(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 15), data);
                var result = signalsWebService
                    .GetCoarseData(1, Dto.Granularity.Week, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1))
                    .ToArray();

                Assert.AreEqual(1, result.Length);
                Assert.AreEqual(data[0].Quality.ToDto<Dto.Quality>(), result[0].Quality);
                Assert.AreEqual((1 + 2 + 3 + 4 + 5 + 6 + 7) / 7, result[0].Value);
            }

            private void GivenExisitingSignals(IEnumerable<Signal> signals)
            {
                GivenNoSignals();

                foreach(var existingSignal in signals)
                {
                    signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);

                    signalsRepositoryMock
                        .Setup(sr => sr.Get(existingSignal.Path))
                        .Returns(existingSignal);
                }
            }

            private void GivenDatum<T>(int signalId, DateTime fromDate, DateTime toDate, IEnumerable<Datum<T>> datum = null)
            {
                signalsDataRepoMock
                    .Setup(sd => sd.GetData<T>(It.Is<Signal>(s => s.Id == signalId), fromDate, toDate))
                    .Returns(datum ?? new List<Datum<T>>());
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

            private Dto.Datum[] DatumWith(object value, DateTime timestamp, int numberOfElements = 1, Dto.Quality quality = Dto.Quality.Fair)
            {
                var dtoData = new Dto.Datum[numberOfElements];
                for (int i = 0; i < numberOfElements; ++i)
                {
                    dtoData[i] = new Dto.Datum()
                    {
                        Quality = quality,
                        Timestamp = timestamp.AddMonths(i),
                        Value = value
                    };
                }
                return dtoData;
            }

            private void GivenNoSignals()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);

                signalsDataRepoMock = new Mock<ISignalsDataRepository>();

                mvpRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                mvpRepositoryMock
                    .Setup(mvp => mvp.Get(It.IsAny<Signal>()))
                    .Returns<Signal>(signal =>
                    {
                        var policy = NoneQualityMissingValuePolicy(signal.DataType);
                        policy.Signal = signal;
                        return policy;
                    });

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, signalsDataRepoMock.Object, mvpRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private MissingValuePolicyBase NoneQualityMissingValuePolicy(DataType dataType)
            {
                switch (dataType)
                {
                    case DataType.Boolean:
                        return new NoneQualityMissingValuePolicyBoolean();
                    case DataType.Integer:
                        return new NoneQualityMissingValuePolicyInteger();
                    case DataType.Double:
                        return new NoneQualityMissingValuePolicyDouble();
                    case DataType.Decimal:
                        return new NoneQualityMissingValuePolicyDecimal();
                    case DataType.String:
                        return new NoneQualityMissingValuePolicyString();
                    default:
                        return null;
                }
            }

            private void GivenASignal(Domain.Signal existingSignal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);
                signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Path))
                    .Returns(existingSignal);
            }

            private List<Signal> GivenMultipleSignals(Dto.Path path, bool addSubSignals = false)
            {
                GivenNoSignals();

                var path_domain = path.ToDomain<Domain.Path>();
                int id = 1;

                var signals = new List<Signal> {
                    SignalWith(id++, DataType.Boolean, Granularity.Day,   path_domain + "s1"),
                    SignalWith(id++, DataType.Double,  Granularity.Week,  path_domain + "s2"),
                    SignalWith(id++, DataType.String,  Granularity.Month, path_domain + "s3")
                };

                signalsRepositoryMock
                    .Setup(s => s.GetAllWithPathPrefix(path_domain))
                    .Returns(signals);

                if(addSubSignals)
                {
                    signals.Add(SignalWith(id++, DataType.String,  Granularity.Month, (path_domain + "sub1") + "s4"));
                    signals.Add(SignalWith(id++, DataType.Decimal, Granularity.Year,  (path_domain + "sub2") + "s5"));
                }

                foreach (Signal existingSignal in signals)
                {
                    signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);
                    signalsRepositoryMock
                        .Setup(sr => sr.Get(existingSignal.Path))
                        .Returns(existingSignal);
                }

                return signals;
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

            private void GivenData<T>(int signalId, IEnumerable<Datum<T>> datums)
            {
                signalsDataRepoMock
                    .Setup(sd => sd.GetData<T>(It.Is<Signal>(s => s.Id == signalId), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(datums);

                signalsDataRepoMock
                    .Setup(q => q.GetData<T>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns<Signal, DateTime, DateTime>((s, fromE, toE) =>
                    {
                        return datums.Where(q => q.Timestamp >= fromE && q.Timestamp <= toE); //new List<Datum<double>>();                            
                    });

                signalsDataRepoMock
                    .Setup(q => q.GetDataNewerThan<T>(It.IsAny<Signal>(), It.IsAny<DateTime>(), 1))
                    .Returns<Signal, DateTime, int>((s, d, m) => {
                        return new[] { datums.FirstOrDefault(q => q.Timestamp >= d) };
                    });

                signalsDataRepoMock
                    .Setup(q => q.GetDataOlderThan<T>(It.IsAny<Signal>(), It.IsAny<DateTime>(), 1))
                    .Returns<Signal, DateTime, int>((s, d, m) => {
                        return new[] { datums.FirstOrDefault(q => q.Timestamp <= d) };
                    });
            }

            private void GivenData<T>(int signalId, int numberOfDatums, DateTime baseDate)
            {
                GivenData<T>(signalId, Enumerable.Range(0, numberOfDatums)
                    .Select(i => new Datum<T> { Id = i, Timestamp = baseDate.AddMonths(i) }));
            }

            private void GivenASignalAndDataOf(DataType dataType, object datumValue, out Dto.Datum[] dtoData, int signalId = 1, int numberOfDatums = 1)
            {
                var path = Path.FromString("root/signal");
                var signal = SignalWith(signalId, dataType, Granularity.Month, path);
                var date = new DateTime(2000, 1, 1);

                GivenASignal(signal);
                dtoData = DatumWith(datumValue, date, numberOfDatums);
            }

            private Signal GivenASignalAndDataRepositoryWithSetups(DateTime date, DataType dataType, object datumValue, int signalId = 1, int numberOfDatums = 1)
            {
                var path = Path.FromString("root/signal");
                var signal = SignalWith(signalId, dataType, Granularity.Month, path);

                GivenASignal(signal);

                switch (dataType)
                {
                    case DataType.Boolean:
                        GivenData<bool>(signalId, numberOfDatums, date);
                        break;
                    case DataType.Decimal:
                        GivenData<decimal>(signalId, numberOfDatums, date);
                        break;
                    case DataType.Double:
                        GivenData<double>(signalId, numberOfDatums, date);
                        break;
                    case DataType.Integer:
                        GivenData<int>(signalId, numberOfDatums, date);
                        break;
                    case DataType.String:
                        GivenData<string>(signalId, numberOfDatums, date);
                        break;
                }

                return signal;
            }

            private void GivenMissingValuePolicy(int signalId, Domain.MissingValuePolicy.MissingValuePolicyBase policy)
            {
                mvpRepositoryMock
                    .Setup(mvp => mvp.Get(It.Is<Signal>(s => s.Id == signalId)))
                    .Returns(policy);
            }

            private void GivenASignalAndDataAndMVP_WhenGettingData_ReturnsMissingValuesAccordingToNoneQualityMVP
                (Granularity granularity, Func<int, DateTime> timeChange)
            {
                int signalId = 1;
                GivenASignal(SignalWith(
                    signalId,
                    DataType.Boolean,
                    granularity,
                    Path.FromString("")));

                GivenData(signalId, new[]
                {
                    new Datum<bool> {Quality = Quality.Fair, Timestamp = timeChange(0) },
                    new Datum<bool> {Quality = Quality.Good, Timestamp = timeChange(1) }
                });

                GivenMissingValuePolicy(signalId, new NoneQualityMissingValuePolicyBoolean());

                var result = signalsWebService.GetData(signalId, timeChange(0), timeChange(2));

                Assert.IsTrue(result.Count() == 2);
                Assert.IsTrue(result.Any(d => d.Quality != Dto.Quality.None));
                CollectionAssert.AreEquivalent(
                    new[]
                    {
                        timeChange(0),
                        timeChange(1)
                    },
                    result.Select(d => d.Timestamp).ToArray());
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepoMock;
            private Mock<IMissingValuePolicyRepository> mvpRepositoryMock;
        }
    }
}