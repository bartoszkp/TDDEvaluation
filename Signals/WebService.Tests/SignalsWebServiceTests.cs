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
            public void GivenASignalAndData_CallingGetDataWithTheSameDateInBothArguments_ReturnsOneDatumWithThatDate()
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

                var result = signalsWebService.GetData(signalId, thatDate, thatDate);

                Assert.AreEqual(1, result.Count());
                Assert.AreEqual(thatDate, result.First().Timestamp);
            }

            [TestMethod]
            public void GivenASignalAndNoData_CallingGetDataWithTheSameDateInBothArguments_ReturnsNull()
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
                    .Returns<IEnumerable<Datum<bool>>>(null);

                var result = signalsWebService.GetData(signalId, thatDate, thatDate);

                Assert.IsNull(result);
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

            private void GivenData<T>(int signalId, IEnumerable<Datum<T>> data)
            {
                signalsDataRepoMock
                    .Setup(sd => sd.GetData<T>(It.Is<Signal>(s => s.Id == signalId), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(data);
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

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepoMock;
            private Mock<IMissingValuePolicyRepository> mvpRepositoryMock;
        }
    }
}