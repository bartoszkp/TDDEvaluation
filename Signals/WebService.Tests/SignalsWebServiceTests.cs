using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Exceptions;
using Domain.MissingValuePolicy;
using System.Collections.Generic;
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
            [ExpectedException(typeof(IdNotNullException))]
            public void WhenSettingAnIdForSignal_ThrowsIdNotNullException()
            {
                var domainService = new SignalsDomainService(null, null, null);
                signalsWebService = new SignalsWebService(domainService);

                signalsWebService.Add(new Dto.Signal()
                {
                    Id = 2,
                    DataType = Dto.DataType.Boolean,
                    Granularity = Dto.Granularity.Day,
                    Path = new Dto.Path() { Components = new[] { "example", "path" } }
                });
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
            [ExpectedException(typeof(System.ArgumentNullException))]
            public void GettingSignalByPath_GivenNullPath_ThrowsArgumentNullException()
            {
                GivenNoSignals();
                var result = signalsWebService.Get(null);
            }

            [TestMethod]
            public void GivenSignals_GettingSignalByPath_SignalIsReturned()
            {
                var exsistingSignal = SignalWith(1,
                                                dataType: DataType.Integer,
                                                granularity: Granularity.Hour,
                                                path: Path.FromString("x/y"));

                GivenASignal(exsistingSignal);
                var dtoPath = new Dto.Path() { Components = new[] { "x", "y" } };
                var result = signalsWebService.Get(dtoPath);

                Assert.AreEqual(Dto.DataType.Integer, result.DataType);
                Assert.AreEqual(Dto.Granularity.Hour, result.Granularity);
                Assert.AreEqual(exsistingSignal.Id, result.Id);
                CollectionAssert.AreEqual(dtoPath.Components.ToArray(), result.Path.Components.ToArray());


            }

            [TestMethod]
            [ExpectedException(typeof(NoSuchSignalException))]
            public void GivenNoSuchSignal_GettingMissingValuePolicy_ThrowsException()
            {
                SetupWebServiceForMvpOperations();

                var result = signalsWebService.GetMissingValuePolicy(1);
            }

            [TestMethod]
            public void SignalHasNoSpecfiedPolicy_GettingMissingValuePolicy_ReturnsNull()
            {
                SetupWebServiceForMvpOperations();
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns(new Signal());
                missingValueRepoMock.Setup(mv => mv.Get(It.IsAny<Signal>())).Returns((MissingValuePolicyBase)null);

                var result = signalsWebService.GetMissingValuePolicy(1);
                Assert.IsNull(result);

            }

            [TestMethod]
            public void GivenSignal_GettingMissingValuePolicy_SpecificPolicyIsReturned()
            {
                SetupWebServiceForMvpOperations();
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns(new Signal());

                missingValueRepoMock.Setup(mv => mv.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyBoolean());

                var result = signalsWebService.GetMissingValuePolicy(1);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Dto.MissingValuePolicy.SpecificValueMissingValuePolicy));
            }


            [TestMethod]
            [ExpectedException(typeof(NoSuchSignalException))]
            public void NoSuchSignal_SetMissingValuePolicy_ThrowsException()
            {
                SetupWebServiceForMvpOperations();
                Dto.MissingValuePolicy.MissingValuePolicy policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<int>())).Returns((Signal)null);

                signalsWebService.SetMissingValuePolicy(1, policy);

            }


            [TestMethod]
            [ExpectedException(typeof(System.ArgumentNullException))]
            public void GivenNullPolicy_SetMissingValuePolicy_ThrowsArgumentNullException()
            {
                SetupWebServiceForMvpOperations();
                signalsWebService.SetMissingValuePolicy(1, null);
            }

            [TestMethod]
            public void CreatedMvpForSignal_WhenGetting_IsReturned()
            {
                SetupWebServiceForMvpOperations();
                Dto.MissingValuePolicy.MissingValuePolicy policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
                signalsRepositoryMock.Setup(sr => sr.Get(It.Is<int>(id => id == 1))).Returns(new Signal() { Id = 1 });

                missingValueRepoMock.Setup(mv => mv.Get(It.Is<Signal>(s => s.Id == 1)))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyString());

                signalsWebService.SetMissingValuePolicy(1, policy);

                var result = signalsWebService.GetMissingValuePolicy(1);

                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Dto.MissingValuePolicy.SpecificValueMissingValuePolicy));
            }

            [TestMethod]
            public void WhenGettingNoneQualityMissingValuePolicy_ReturnsIt()
            {
                SetupWebServiceForMvpOperations();
                var signal = new Domain.Signal()
                {
                    DataType = Domain.DataType.Double,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("sfda/mko")
                };
                var signalDto = signal.ToDto<Dto.Signal>();

                signalsRepositoryMock
                    .Setup(s => s.Get(It.IsAny<int>()))
                    .Returns(signal);
                missingValueRepoMock
                    .Setup(mvpr => mvpr.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble()
                    {
                        Id = 1,
                        Signal = signal,
                    });

                var result = signalsWebService.GetMissingValuePolicy(1);
                Assert.AreEqual(1, result.Id.Value);
                Assert.AreEqual(signalDto.DataType, result.Signal.DataType);
                Assert.AreEqual(signalDto.Granularity, result.Signal.Granularity);
                Assert.AreEqual(signalDto.Path.ToString(), result.Signal.Path.ToString());
            }

            [TestMethod]
            public void WhenAddingNewSignal_SettingMissingValuePolicyTo_NoneQualityMissingValuePolicy()
            {
                var signal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = DataType.String
                };
                SetupWebServiceForMvpOperations();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns(signal);
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<int>()))
                    .Returns<Domain.Signal>(null);
                missingValueRepoMock
                    .Setup(mvpr => mvpr.Set(It.IsAny<Domain.Signal>(), It.IsAny<NoneQualityMissingValuePolicy<string>>()));

                signalsWebService.Add(new Dto.Signal()
                {
                    DataType = Dto.DataType.String
                });
                missingValueRepoMock.Verify(mvpr => mvpr.Set(It.IsAny<Domain.Signal>(), It.IsAny<NoneQualityMissingValuePolicy<string>>()));
            }

            [TestMethod]
            public void WhenGettingById_ReturnsNull()
            {
                GivenASignal(null, 1);
                var result = signalsWebService.GetById(1);
                Assert.IsNull(result);
            }

            [TestMethod]
            public void WhenGettingByPath_ReturnsNull()
            {
                GivenASignal(null, 1);
                var result = signalsWebService.Get(new Dto.Path() {
                    Components = new[] { "sfda" } });
                Assert.IsNull(result);
            }


            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.BadDateFormatForSignalException))]
            public void WhenSettingDatumWithNotExistingData_ForYearSignal_ThenThrowingBadDateFormatForSignalException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Year,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 3, 1, 12, 34, 31), Value = (int)1 },
                };

                SetupSettingData<int>(existingSignal, existingDatum);

                var compareResult = TimestampCorrectCheckerForYear(existingDatum, existingSignal);

                if (compareResult == "Year signal with bad month" || compareResult == "Year signal with bad day" || compareResult == "Year signal with bad hour" 
                    || compareResult == "Year signal with bad minute" || compareResult == "Year signal with bad second")
                {
                    throw new Domain.Exceptions.BadDateFormatForSignalException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.SetData(1, existingDatum);
                }
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.BadDateFormatForSignalException))]
            public void WhenSettingDatumWithNotExistingData_ForMonthSignal_ThenThrowingBadDateFormatForSignalException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 1, 1, 0, 0, 13), Value = (int)1 },
                };

                SetupSettingData<int>(existingSignal, existingDatum);

                var compareResult = TimestampCorrectCheckerForMonth(existingDatum, existingSignal);

                if (compareResult == "Month signal with bad day" || compareResult == "Month signal with bad hour"
                    || compareResult == "Month signal with bad minute" || compareResult == "Month signal with bad second")
                {
                    throw new Domain.Exceptions.BadDateFormatForSignalException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.SetData(1, existingDatum);
                }
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.BadDateFormatForSignalException))]
            public void WhenSettingDatumWithNotExistingData_ForWeekSignal_ThenThrowingBadDateFormatForSignalException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Week,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000,4,(int)System.DayOfWeek.Thursday, 12, 34, 12), Value = (int)1 },
                };

                SetupSettingData<int>(existingSignal, existingDatum);

                var compareResult = TimestampCorrectCheckerForWeek(existingDatum, existingSignal);

                if (compareResult == "Week signal with bad day" || compareResult == "Week signal with bad hour"
                    || compareResult == "Week signal with bad minute" || compareResult == "Week signal with bad second")
                {
                    throw new Domain.Exceptions.BadDateFormatForSignalException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.SetData(1, existingDatum);
                }
                
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.BadDateFormatForSignalException))]
            public void WhenSettingDatumWithNotExistingData_ForDaySignal_ThenThrowingBadDateFormatForSignalException()
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
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 3, 23, 0, 23, 12), Value = (int)1 },
                };

                SetupSettingData<int>(existingSignal, existingDatum);


                var compareResult = TimestampCorrectCheckerForDay(existingDatum, existingSignal);

                if (compareResult == "Day signal with bad hour" || compareResult == "Day signal with bad minute" 
                    || compareResult == "Day signal with bad second")
                {
                    throw new Domain.Exceptions.BadDateFormatForSignalException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.SetData(1, existingDatum);
                }
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.BadDateFormatForSignalException))]
            public void WhenSettingDatumWithNotExistingData_ForHourSignal_ThenThrowingBadDateFormatForSignalException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Hour,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 11, 24, 7, 0, 14), Value = (int)1 },
                };

                SetupSettingData<int>(existingSignal, existingDatum);


                var compareResult = TimestampCorrectCheckerForHour(existingDatum, existingSignal);

                if (compareResult == "Hour signal with bad minute" || compareResult == "Hour signal with bad second")
                {
                    throw new Domain.Exceptions.BadDateFormatForSignalException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.SetData(1, existingDatum);
                }

            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.BadDateFormatForSignalException))]
            public void WhenSettingDatumWithNotExistingData_ForMinuteSignal_ThenThrowingBadDateFormatForSignalException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Minute,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 4, 5, 6, 25, 0, 13), Value = (int)1 },
                };

                SetupSettingData<int>(existingSignal, existingDatum);

                var compareResult = TimestampCorrectCheckerForMinute(existingDatum, existingSignal);

                if (compareResult == "Minute signal with bad second" || compareResult == "Minute signal with bad milisecond")
                {
                    throw new Domain.Exceptions.BadDateFormatForSignalException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.SetData(1, existingDatum);
                }
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.BadDateFormatForSignalException))]
            public void WhenSettingDatumWithNotExistingData_ForSecondsSignal_ThenThrowingBadDateFormatForSignalException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Second,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 4, 5, 6, 25, 56, 13), Value = (int)1 },
                };

                SetupSettingData<int>(existingSignal, existingDatum);

                var compareResult = TimestampCorrectCheckerForSecond(existingDatum, existingSignal);

                if (compareResult == "Second signal with bad milisecond")
                {
                    throw new Domain.Exceptions.BadDateFormatForSignalException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.SetData(1, existingDatum);
                }

            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException))]
            public void WhenGettingDatumWithIncorrectData_ForYearSignal_ThenThrowingQuerryAboutDateWithIncorrectFormatException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Year,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 2, 24, 14, 25, 56, 13), Value = (int)1 },
                };

                var firstTimestamp = existingDatum.ElementAt(0).Timestamp;

                SetupGettingData<int>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger(),
                    firstTimestamp);

                var compareResult = TimestampCorrectCheckerForYear(existingDatum, existingSignal);

                if (compareResult == "Year signal with bad month" || compareResult == "Year signal with bad day" 
                    || compareResult == "Year signal with bad hour"
                    || compareResult == "Year signal with bad minute" 
                    || compareResult == "Year signal with bad second")
                {
                    throw new Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.GetData(1, firstTimestamp, firstTimestamp);
                }
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException))]
            public void WhenGettingDatumWithIncorrectData_ForMonthSignal_ThenThrowingQuerryAboutDateWithIncorrectFormatException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 11, 1, 13, 25, 56), Value = (int)1 },
                };

                var firstTimestamp = existingDatum.ElementAt(0).Timestamp;

                SetupGettingData<int>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger(),
                    firstTimestamp);

                var compareResult = TimestampCorrectCheckerForMonth(existingDatum, existingSignal);

                if (compareResult == "Month signal with bad day" || compareResult == "Month signal with bad hour"
                    || compareResult == "Month signal with bad minute" 
                    || compareResult == "Month signal with bad second")
                {
                    throw new Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.GetData(1, firstTimestamp, firstTimestamp);
                }
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException))]
            public void WhenGettingDatumWithIncorrectData_ForWeekSignal_ThenThrowingQuerryAboutDateWithIncorrectFormatException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                 {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000,4,(int)System.DayOfWeek.Thursday, 12, 34, 12), Value = (int)1 },
                 };

                var firstTimestamp = existingDatum.ElementAt(0).Timestamp;

                SetupGettingData<int>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger(),
                    firstTimestamp);

                var compareResult = TimestampCorrectCheckerForWeek(existingDatum, existingSignal);

                if (compareResult == "Week signal with bad day" || compareResult == "Week signal with bad hour"
                     || compareResult == "Week signal with bad minute" || compareResult == "Week signal with bad second")
                {
                    throw new Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.GetData(1, firstTimestamp, firstTimestamp);
                }

            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException))]
            public void WhenGettingDatumWithIncorrectData_ForDaySignal_ThenThrowingQuerryAboutDateWithIncorrectFormatException()
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
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 11, 5, 13, 25, 13), Value = (int)1 },
                };

                var firstTimestamp = existingDatum.ElementAt(0).Timestamp;

                SetupGettingData<int>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger(),
                    firstTimestamp);

                var compareResult = TimestampCorrectCheckerForDay(existingDatum, existingSignal);

                if (compareResult == "Day signal with bad hour" || compareResult == "Day signal with bad minute"
                    || compareResult == "Day signal with bad second")
                {
                    throw new Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.GetData(1, firstTimestamp, firstTimestamp);
                }
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException))]
            public void WhenGettingDatumWithIncorrectData_ForHourSignal_ThenThrowingQuerryAboutDateWithIncorrectFormatException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Hour,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 11, 5, 13, 25, 13), Value = (int)1 },
                };

                var firstTimestamp = existingDatum.ElementAt(0).Timestamp;

                SetupGettingData<int>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger(),
                    firstTimestamp);

                var compareResult = TimestampCorrectCheckerForHour(existingDatum, existingSignal);

                if (compareResult == "Hour signal with bad minute" || compareResult == "Hour signal with bad second")
                {
                    throw new Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.GetData(1, firstTimestamp, firstTimestamp);
                }
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException))]
            public void WhenGettingDatumWithIncorrectData_ForMinuteSignal_ThenThrowingQuerryAboutDateWithIncorrectFormatException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Minute,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 11, 5, 11, 12, 9, 8), Value = (int)1 },
                };

                var firstTimestamp = existingDatum.ElementAt(0).Timestamp;

                SetupGettingData<int>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger(),
                    firstTimestamp);

                var compareResult = TimestampCorrectCheckerForMinute(existingDatum, existingSignal);

                if (compareResult == "Minute signal with bad second" || compareResult == "Minute signal with bad milisecond")
                {
                    throw new Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.GetData(1, firstTimestamp, firstTimestamp);
                }
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException))]
            public void WhenGettingDatumWithIncorrectData_ForSecondSignal_ThenThrowingQuerryAboutDateWithIncorrectFormatException()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Second,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new System.DateTime(2000, 11, 5, 13, 25, 13, 12), Value = (int)1 },
                };

                var firstTimestamp = existingDatum.ElementAt(0).Timestamp;

                SetupGettingData<int>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger(),
                    firstTimestamp);

                var compareResult = TimestampCorrectCheckerForSecond(existingDatum, existingSignal);

                if (compareResult == "Second signal with bad milisecond")
                {
                    throw new Domain.Exceptions.QuerryAboutDateWithIncorrectFormatException();
                }
                else if (compareResult == "Correct data")
                {
                    signalsWebService.GetData(1, firstTimestamp, firstTimestamp);
                }
            }


            [TestMethod]
            public void GivenASignal_WhenDeleteSignal_DataIsDeleted_Double()
            {
                SetupAllSerivce();

                var signal = SignalWith(1, DataType.Double, Granularity.Minute, Path.FromString("z/a"));

                signalsRepositoryMock.Setup(x => x.Get(1)).Returns(signal);

                signalsWebService.Delete(1);

                signalsDataRepositoryMock.Verify(x => x.DeleteData<double>(signal));

            }

            [TestMethod]
            public void GivenASignal_WhenDeleteSignal_DataIsDeleted_Int()
            {
                SetupAllSerivce();

                var signal = SignalWith(1, DataType.Integer, Granularity.Minute, Path.FromString("z/a"));

                signalsRepositoryMock.Setup(x => x.Get(1)).Returns(signal);

                signalsWebService.Delete(1);

                signalsDataRepositoryMock.Verify(x => x.DeleteData<double>(signal));
            }

            [TestMethod]
            public void GivenASignal_WhenDeleteSignal_DataIsDeleted_Dec()
            {
                SetupAllSerivce();

                var signal = SignalWith(1, DataType.Decimal, Granularity.Minute, Path.FromString("z/a"));

                signalsRepositoryMock.Setup(x => x.Get(1)).Returns(signal);

                signalsWebService.Delete(1);

                signalsDataRepositoryMock.Verify(x => x.DeleteData<double>(signal));
            }
            [TestMethod]
            public void GivenASignal_WhenDeleteSignal_DataIsDeleted_String()
            {
                SetupAllSerivce();

                var signal = SignalWith(1, DataType.String, Granularity.Minute, Path.FromString("z/a"));

                signalsRepositoryMock.Setup(x => x.Get(1)).Returns(signal);

                signalsWebService.Delete(1);

                signalsDataRepositoryMock.Verify(x => x.DeleteData<double>(signal));
            }

            [ExpectedException(typeof(NoSuchSignalException))]
            [TestMethod]
            public void NoSignal_WhenDeleteSignal_Exception()
            {
                SetupAllSerivce();

                signalsWebService.Delete(1);

            }

            [TestMethod]
            public void GivenASignal_WhenDeleteSignal_MVPIsCleared()
            {
                SetupAllSerivce();

                var signal = SignalWith(1, DataType.Double, Granularity.Minute, Path.FromString("z/a"));

                signalsRepositoryMock.Setup(x => x.Get(1)).Returns(signal);

                signalsWebService.Delete(1);

                missingValueRepoMock.Verify(x => x.Set(signal, null));


            }

            [TestMethod]
            public void GivenASignal_WhenDeleteSignal_DeleteFromRepository()
            {
                {
                    SetupAllSerivce();

                    var signal = SignalWith(1, DataType.Double, Granularity.Minute, Path.FromString("z/a"));

                    signalsRepositoryMock.Setup(x => x.Get(1)).Returns(signal);

                    signalsWebService.Delete(1);

                    signalsRepositoryMock.Verify(x => x.Delete(signal));


                }
            }

            private void SetupAllSerivce()
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValueRepoMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                var domian = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValueRepoMock.Object);

                signalsWebService = new SignalsWebService(domian);
            }

            private void SetupSettingData<T>(Signal existingSignal, Dto.Datum[] existingDatum)
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<T>(It.IsAny<IEnumerable<Datum<T>>>()));

                GivenASignal(existingSignal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum);
            }

            private void SetupGettingData<T>(
                Signal existingSignal,
                Dto.Datum[] existingDatum,
                Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<T> genericInstance,
                DateTime firstTimestamp)
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<T>(
                        existingSignal,
                        firstTimestamp,
                        firstTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<T>>>());

                GivenASignal(existingSignal);

                missingValueRepoMock = new Mock<IMissingValuePolicyRepository>();

                missingValueRepoMock
                    .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(genericInstance);

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValueRepoMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void SetupWebServiceForMvpOperations()
            {
                missingValueRepoMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValueRepoMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
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
                missingValueRepoMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => AddId(s));
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValueRepoMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Domain.Signal existingSignal, int id = 0)
            {
                GivenNoSignals();
                if (id <= 0)
                    id = existingSignal.Id.Value;

                signalsRepositoryMock
                    .Setup(sr => sr.Get(id))
                    .Returns(existingSignal);

                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<Path>()))
                    .Returns(existingSignal);
            }

            private void GivenRepositoryThatAssigns(int id)
            {
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => AddId(s));
            }

            private Domain.Signal AddId(Domain.Signal signal, int id = 1)
            {
                signal.Id = id;
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<int>()))
                    .Returns(signal);
                return signal;
            }

            private string TimestampCorrectCheckerForMonth(Dto.Datum[] existingDatum, Domain.Signal existingSignal)
            {
                var TimestampDay = existingDatum.ToList().ElementAt(0).Timestamp.Day;
                var TimestampHour = existingDatum.ToList().ElementAt(0).Timestamp.Hour;
                var TimestampMinute = existingDatum.ToList().ElementAt(0).Timestamp.Minute;
                var TimestampSecond = existingDatum.ToList().ElementAt(0).Timestamp.Second;

                if (existingSignal.Granularity == Granularity.Month && TimestampDay != 1 && TimestampHour >= 0
                    && TimestampMinute >= 0 && TimestampSecond >= 0)
                {
                    return "Month signal with bad day";
                }
                else if (existingSignal.Granularity == Granularity.Month && TimestampDay == 1 && TimestampHour != 0
                    && TimestampMinute >= 0 && TimestampSecond >= 0)
                {
                    return "Month signal with bad hour";
                }
                else if (existingSignal.Granularity == Granularity.Month && TimestampDay == 1 && TimestampHour == 0
                    && TimestampMinute != 0 && TimestampSecond >= 0)
                {
                    return "Month signal with bad minute";
                }
                else if (existingSignal.Granularity == Granularity.Month && TimestampDay == 1 && TimestampHour == 0
                    && TimestampMinute == 0 && TimestampSecond != 0)
                {
                    return "Month signal with bad second";
                }
                else return "Correct data";
            }

            private string TimestampCorrectCheckerForYear(Dto.Datum[] existingDatum, Domain.Signal existingSignal)
            {
                var TimestampMonth = existingDatum.ToList().ElementAt(0).Timestamp.Month;
                var TimestampDay = existingDatum.ToList().ElementAt(0).Timestamp.Day;
                var TimestampHour = existingDatum.ToList().ElementAt(0).Timestamp.Hour;
                var TimestampMinute = existingDatum.ToList().ElementAt(0).Timestamp.Minute;
                var TimestampSecond = existingDatum.ToList().ElementAt(0).Timestamp.Second;

                if (existingSignal.Granularity == Granularity.Year && TimestampMonth != 1 && TimestampDay >= 1 && TimestampHour >= 0
                    && TimestampMinute >= 0 && TimestampSecond >= 0)
                {
                    return "Year signal with bad month";
                }
                else if (existingSignal.Granularity == Granularity.Year && TimestampMonth == 1 && TimestampDay != 1 && TimestampHour >= 0
                    && TimestampMinute >= 0 && TimestampSecond >= 0)
                {
                    return "Year signal with bad day";
                }
                else if (existingSignal.Granularity == Granularity.Year && TimestampMonth == 1 && TimestampDay == 1 && TimestampHour != 0
                    && TimestampMinute != 0 && TimestampSecond >= 0)
                {
                    return "Year signal with bad hour";
                }
                else if (existingSignal.Granularity == Granularity.Year && TimestampMonth != 1 && TimestampDay == 1 && TimestampHour == 0
                    && TimestampMinute != 0 && TimestampSecond >= 0)
                {
                    return "Year signal with bad minute";
                }
                else if (existingSignal.Granularity == Granularity.Year && TimestampMonth != 1 && TimestampDay == 1 && TimestampHour == 0
                    && TimestampMinute == 0 && TimestampSecond != 0)
                {
                    return "Year signal with bad second";
                }
                else return "Correct data";
            }

            private string TimestampCorrectCheckerForWeek(Dto.Datum[] existingDatum, Domain.Signal existingSignal)
            {
                var TimestampDayOfTheWeek = existingDatum.ToList().ElementAt(0).Timestamp.DayOfWeek;
                var TimestampHour = existingDatum.ToList().ElementAt(0).Timestamp.Hour;
                var TimestampMinute = existingDatum.ToList().ElementAt(0).Timestamp.Minute;
                var TimestampSecond = existingDatum.ToList().ElementAt(0).Timestamp.Second;

                if (existingSignal.Granularity == Granularity.Week && TimestampDayOfTheWeek != System.DayOfWeek.Monday && TimestampHour >= 0
                    && TimestampMinute >= 0 && TimestampSecond >= 0)
                {
                    return "Week signal with bad day";
                }
                else if (existingSignal.Granularity == Granularity.Week && TimestampDayOfTheWeek == System.DayOfWeek.Monday && TimestampHour != 0
                    && TimestampMinute >= 0 && TimestampSecond >= 0)
                {
                    return "Week signal with bad hour";
                }
                else if (existingSignal.Granularity == Granularity.Week && TimestampDayOfTheWeek != System.DayOfWeek.Monday && TimestampHour == 0
                    && TimestampMinute != 0 && TimestampSecond >= 0)
                {
                    return "Week signal with bad minute";
                }
                else if (existingSignal.Granularity == Granularity.Week && TimestampDayOfTheWeek != System.DayOfWeek.Monday && TimestampHour == 0
                    && TimestampMinute == 0 && TimestampSecond != 0)
                {
                    return "Week signal with bad second";
                }
                else return "Correct data";
            }

            private string TimestampCorrectCheckerForDay(Dto.Datum[] existingDatum, Domain.Signal existingSignal)
            {
                var TimestampHour = existingDatum.ToList().ElementAt(0).Timestamp.Hour;
                var TimestampMinute = existingDatum.ToList().ElementAt(0).Timestamp.Minute;
                var TimestampSecond = existingDatum.ToList().ElementAt(0).Timestamp.Second;

                if (existingSignal.Granularity == Granularity.Day && TimestampHour != 0 && TimestampMinute >= 0 && TimestampSecond >= 0)
                {
                    return "Day signal with bad hour";
                }
                else if (existingSignal.Granularity == Granularity.Day && TimestampHour == 0
                    && TimestampMinute != 0 && TimestampSecond >= 0)
                {
                    return "Day signal with bad minute";
                }
                else if (existingSignal.Granularity == Granularity.Day && TimestampHour == 0
                    && TimestampMinute == 0 && TimestampSecond != 0)
                {
                    return "Day signal with bad second";
                }
                else return "Correct data";
            }

            private string TimestampCorrectCheckerForHour(Dto.Datum[] existingDatum, Domain.Signal existingSignal)
            {
                var TimestampMinute = existingDatum.ToList().ElementAt(0).Timestamp.Minute;
                var TimestampSecond = existingDatum.ToList().ElementAt(0).Timestamp.Second;

                if (existingSignal.Granularity == Granularity.Hour && TimestampMinute != 0 && TimestampSecond >= 0)
                {
                    return "Hour signal with bad minute";
                }
                else if (existingSignal.Granularity == Granularity.Hour && TimestampMinute == 0 && TimestampSecond != 0)
                {
                    return "Hour signal with bad second";
                }
                else return "Correct data";
            }

            private string TimestampCorrectCheckerForMinute(Dto.Datum[] existingDatum, Domain.Signal existingSignal)
            {
                var TimestampSecond = existingDatum.ToList().ElementAt(0).Timestamp.Second;
                var TimestampMilisecond = existingDatum.ToList().ElementAt(0).Timestamp.Millisecond;

                if (existingSignal.Granularity == Granularity.Minute && TimestampSecond != 0)
                {
                    return "Minute signal with bad second";
                }
                else if (existingSignal.Granularity == Granularity.Minute && TimestampSecond == 0 && TimestampMilisecond != 0)
                {
                    return "Minute signal with bad milisecond";
                }
                else return "Correct data";
            }

            private string TimestampCorrectCheckerForSecond(Dto.Datum[] existingDatum, Domain.Signal existingSignal)
            {
                var TimestampMilisecond = existingDatum.ToList().ElementAt(0).Timestamp.Millisecond;

                if (existingSignal.Granularity == Granularity.Second && TimestampMilisecond != 0)
                {
                    return "Second signal with bad milisecond";
                }
                else return "Correct data";
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValueRepoMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
        }
    }
}