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
            private SignalsDomainService signalDomainService;

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.IdNotNullException))]
            public void GivenNoSignals_WhenAddASignalWithId_ThrowIdNotNullException()
            {
                GivenNoSignals();

                var result = signalsWebService.Add(SignalWith(id: 1));
            }
            
            [TestMethod]
            public void GivenNoSignals_WhenAddASignal_ReturnsIt()
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
            public void GivenNoSignals_WhenAddASignal_CallsRepositoryAdd()
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
            public void GivenNoSignals_WhenGetById_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.GetById(0);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGetByItsId_ReturnsIt()
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
            public void WhenGetByPath_ReturnsIt()
            {
                GivenNoSignals();
                Domain.Signal signalDomain = SignalWith(100, DataType.Boolean, Granularity.Day, Path.FromString("root/signal"));
                
                signalsRepositoryMock.Setup(srm => srm.Get(It.Is<Domain.Path>(s => s.ToString() == signalDomain.Path.ToString())))
                    .Returns(signalDomain);

                var signalDto = signalDomain.ToDto<Dto.Signal>();
                var result = signalsWebService.Get(signalDto.Path);

                MatchSignals(signalDto.ToDomain<Domain.Signal>(), result.ToDomain<Domain.Signal>());
            }

            [TestMethod]
            public void WhenGetPathIsNotExistOrIsNullOrIsIsEmpty_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.Get(null);

                Assert.IsNull(result);
            }
            
            [TestMethod]
            public void WhenSetMissingValuePolicy_RepositoryGet()
            {
                SetupSignalsWebServiceAndMissingValue();
                Domain.Signal signal = SignalWith(105, DataType.Decimal, Granularity.Minute, Path.FromString("path/pat/pa"));
                
                signalsRepositoryMock.Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Set(It.Is<Domain.Signal>(s => s == signal),
                                            It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

                var mvp = new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy()
                {
                    DataType = Dto.DataType.Decimal,
                    Signal = signal.ToDto<Dto.Signal>()
                };
                var mvpDomain = mvp.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

                signalsWebService.SetMissingValuePolicy(signal.Id.Value, mvp);

                missingValuePolicyRepositoryMock.Verify(mvpr => mvpr.Set(
                    It.Is<Domain.Signal>(s => s == signal),
                    It.Is<Domain.MissingValuePolicy.MissingValuePolicyBase>(
                        m => m.NativeDataType == mvpDomain.NativeDataType
                        && VerifyMissingValue(m,signal))));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SignalIsNotException))]
            public void WhenSetMissingValuePolicyWhenNotExis_ReportException()
            {
                SetupSignalsWebServiceAndMissingValue();
                int signalId = 101;
                signalsRepositoryMock.Setup(sr => sr.Get(signalId))
                    .Returns<Domain.Signal>(null);
                signalsWebService.SetMissingValuePolicy(signalId, new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy());
            }
            
            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.IdNotNullException))]
            public void WhenGetMissingValueForNewSignal_ReportException()
            {
                var signal = new Domain.Signal(){ Id = 103 };
                SetupSignalsWebServiceAndMissingValue();
                signalsRepositoryMock.Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Domain.Signal>(s => s == signal)))
                    .Returns<DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger>(null);

               signalsWebService.GetMissingValuePolicy(signal.Id.Value);
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.IdNotNullException))]
            public void WhenGetMissingValue_WhenSignalNonExist_ReportException()
            {
                int signalId = 104;
                SetupSignalsWebServiceAndMissingValue();
                signalsRepositoryMock.Setup(sr => sr.Get(signalId))
                    .Returns<Domain.Signal>(null);

                signalsWebService.GetMissingValuePolicy(signalId);
            }

            [TestMethod]
            public void WhenGetMissingValue_ReturnsIt()
            {
                Domain.Signal signal = SignalWith(105, DataType.Integer, Granularity.Month, Path.FromString("root/roo/ro"));
                SetupSignalsWebServiceAndMissingValue();
                signalsRepositoryMock.Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
                var missingValuePolicy = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                {
                    Id = 45,
                    Quality = Quality.Fair,
                    Signal = signal,
                    Value = (double)2.25
                };
                missingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Domain.Signal>(s => s==signal)))
                    .Returns(missingValuePolicy);
                var missingValuePolicyDto = missingValuePolicy.ToDto<Dto.MissingValuePolicy.SpecificValueMissingValuePolicy>();

                Dto.MissingValuePolicy.MissingValuePolicy result
                    = signalsWebService.GetMissingValuePolicy(signal.Id.Value);
                Assert.IsTrue(EqualsMissingValuePolicy(missingValuePolicyDto, result));
            }
            
            [TestMethod]
            [ExpectedException(typeof(System.Collections.Generic.KeyNotFoundException))]
            public void WhenSettingDataForNonExistSignal_ThrowSignalWithThisIdNonExistException()
            {
                int signalId = 105;
                prepareDataRepository();
                signalsRepositoryMock
                    .Setup(sr => sr.Get(signalId))
                    .Returns<Domain.Signal>(null);
                signalsWebService.SetData(signalId, null);
            }

            [TestMethod]
            public void GivenASignalAndDoubleDatum_WhenSettingData_RepositorySetDataIsCalled()
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

                SetupSettingData<double>(existingSignal, existingDatum);
                
                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>();

                VerifySettingGenericData<double>(datum);
            }

            [TestMethod]
            public void GivenASignalAndIntDatum_WhenSettingData_RepositorySetIsCalled()
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

                SetupSettingData<int>(existingSignal, existingDatum);
                
                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>();

                VerifySettingGenericData<int>(datum);
            }

            [TestMethod]
            public void GivenASignalAndBoolDatum_WhenSettingData_RepositorySetIsCalled()
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
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (bool)true },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (bool)false }
                };

                SetupSettingData<bool>(existingSignal, existingDatum);

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<bool>>>();

                VerifySettingGenericData<bool>(datum);
            }

            [TestMethod]
            public void GivenASignalAndDecimalDatum_WhenSettingData_RepositorySetIsCalled()
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

                SetupSettingData<decimal>(existingSignal, existingDatum);
                
                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<decimal>>>();

                VerifySettingGenericData<decimal>(datum);
            }

            [TestMethod]
            public void GivenASignalAndStringDatum_WhenSettingData_RepositorySetIsCalled()
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
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (string)"tak" },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (string)"tak" },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (string)"nie" }
                };

                SetupSettingData<string>(existingSignal, existingDatum);

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<string>>>();

                VerifySettingGenericData<string>(datum);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingDoubleDataForSpecificSignal_ReturnsThisData()
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
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 }
                };

                var firstTimestamp = existingDatum.First().Timestamp;
                var lastTimestamp = existingDatum.Last().Timestamp.AddMonths(1);

                SetupGettingData<double>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble(),
                    firstTimestamp,
                    lastTimestamp);

                var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

                AssertGettingGenericData(existingDatum, result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingIntDataForSpecificSignal_ReturnsThisData()
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
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (int)2 }
                };

                var firstTimestamp = existingDatum.First().Timestamp;
                var lastTimestamp = existingDatum.Last().Timestamp.AddMonths(1);

                SetupGettingData<int>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger(),
                    firstTimestamp,
                    lastTimestamp);

                var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

                AssertGettingGenericData(existingDatum, result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingDecimalDataForSpecificSignal_ReturnsThisData()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Decimal,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (decimal)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (decimal)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (decimal)2 }
                };

                var firstTimestamp = existingDatum.First().Timestamp;
                var lastTimestamp = existingDatum.Last().Timestamp.AddMonths(1);

                SetupGettingData<decimal>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDecimal(),
                    firstTimestamp,
                    lastTimestamp);

                var result = signalsWebService.GetData(existingSignal.Id.Value, existingDatum.First().Timestamp, existingDatum.Last().Timestamp.AddMonths(1));

                AssertGettingGenericData(existingDatum, result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingBoolDataForSpecificSignal_ReturnsThisData()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Boolean,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (bool)true },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (bool)true },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (bool)false }
                };

                var firstTimestamp = existingDatum.First().Timestamp;
                var lastTimestamp = existingDatum.Last().Timestamp.AddMonths(1);

                SetupGettingData<bool>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyBoolean(),
                    firstTimestamp,
                    lastTimestamp);
                
                var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

                AssertGettingGenericData(existingDatum, result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingStringDataForSpecificSignal_ReturnsThisData()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.String,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (string)"tak" },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (string)"tak" },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (string)"nie" }
                };

                var firstTimestamp = existingDatum.First().Timestamp;
                var lastTimestamp = existingDatum.Last().Timestamp.AddMonths(1);

                SetupGettingData<string>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyString(),
                    firstTimestamp,
                    lastTimestamp);
                
                var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

                AssertGettingGenericData(existingDatum, result);
            }

            [TestMethod]
            public void GivenASignalAndNotSortedDatum_WhenSettingData_RepositorySetWithSortedDataIsCalled()
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
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = (double)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = (double)2 }
                };

                SetupSettingData<double>(existingSignal, existingDatum);
                
                var datum = existingDatum.OrderBy(ed => ed.Timestamp).ToArray().ToDomain<IEnumerable<Domain.Datum<double>>>();
                
                VerifySettingGenericData<double>(datum);
            }

            [TestMethod]
            public void GivenASignal_WhenAddingASignal_NoneQualityMissingValuePolicyIsSet()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                signalsRepositoryMock
                    .Setup(srm => srm.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s =>
                    {
                        s.DataType = DataType.Double;
                        return s;
                    });

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>()));

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Add(new Dto.Signal());

                missingValuePolicyRepositoryMock
                    .Verify(mvrpm => mvrpm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>()));
            }

            [TestMethod]
            public void GivenASignalAndDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
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
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1),  Value = (double)3.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 6, 1),  Value = (double)4.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1),  Value = (double)3.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 5, 1),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 6, 1),  Value = (double)4.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 7, 1),  Value = default(double)}
                };

                var firstTimestamp = new DateTime(2000, 1, 1);
                var lastTimestamp = new DateTime(2000, 8, 1);

                SetupGettingData<double>(
                    existingSignal,
                    existingDatum,
                    new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble(),
                    firstTimestamp,
                    lastTimestamp);
                
                var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 8, 1));

                AssertGettingGenericData(filledDatum, result);
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
                Domain.Path path)
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

            private void GivenASignal(Signal signal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
            }
            
            private bool MatchSignals(Signal signal, Signal result)
            {
                return ((signal.Id == result.Id) && (signal.DataType == result.DataType)
                    && (signal.Granularity == result.Granularity) && (signal.Path.ToString() == result.Path.ToString()));
            }

            private bool VerifyMissingValue(Domain.MissingValuePolicy.MissingValuePolicyBase missingValuePolicyBase, Signal signal)
            {
                return ((signal.Id == missingValuePolicyBase.Signal.Id) && (signal.DataType == missingValuePolicyBase.Signal.DataType)
                    && (signal.Granularity == missingValuePolicyBase.Signal.Granularity) && (signal.Path.ToString() == missingValuePolicyBase.Signal.Path.ToString()));
            }
            
            private bool EqualsMissingValuePolicy(Dto.MissingValuePolicy.MissingValuePolicy missingValuePolicySignal, Dto.MissingValuePolicy.MissingValuePolicy missingValuePolicyResult)
            {
                return ((missingValuePolicySignal.Id == missingValuePolicyResult.Id)
                    && (missingValuePolicySignal.DataType == missingValuePolicyResult.DataType)
                    && (missingValuePolicySignal.Signal.Id == missingValuePolicyResult.Signal.Id)
                    && (missingValuePolicySignal.Signal.DataType == missingValuePolicyResult.Signal.DataType)
                    && (missingValuePolicySignal.Signal.Granularity == missingValuePolicyResult.Signal.Granularity)
                    && (missingValuePolicySignal.Signal.Path.ToString() == missingValuePolicyResult.Signal.Path.ToString()));
            }
            
            private bool IEnumerableDatumAreEqual(System.Collections.Generic.IEnumerable<Dto.Datum> datumDto,
                System.Collections.Generic.IEnumerable<Domain.Datum<object>> datumDomain,
                Domain.Signal signal)
            {
                foreach (var dt in datumDto.Zip(datumDomain, System.Tuple.Create))
                {
                    var datum = dt.Item2.ToDto<Dto.Datum>();
                    if (!(dt.Item1.Quality == datum.Quality
                        && dt.Item1.Timestamp == datum.Timestamp
                        && signal.Id == dt.Item2.Signal.Id
                        && signal.DataType == dt.Item2.Signal.DataType
                        && signal.Granularity == dt.Item2.Signal.Granularity
                        && signal.Path.ToString() == dt.Item2.Signal.Path.ToString()))
                        return false;
                }
                return true;
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

            private void VerifySettingGenericData<T>(IEnumerable<Datum<T>> datum)
            {
                int index = 0;
                foreach (var ed in datum)
                {
                    signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<T>(It.Is<IEnumerable<Datum<T>>>(d =>
                    (
                        d.ElementAt(index).Quality == ed.Quality
                        && d.ElementAt(index).Timestamp == ed.Timestamp
                        && d.ElementAt(index).Value.Equals(ed.Value)
                    ))));
                    index++;
                }
            }
            
            private void SetupGettingData<T>(
                Signal existingSignal,
                Dto.Datum[] existingDatum,
                Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<T> genericInstance,
                DateTime firstTimestamp,
                DateTime lastTimestamp)
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<T>(
                        existingSignal,
                        firstTimestamp,
                        lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<T>>>());

                GivenASignal(existingSignal);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(genericInstance);

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void AssertGettingGenericData(Dto.Datum[] existingDatum, IEnumerable<Dto.Datum> result)
            {
                int index = 0;
                foreach (var ed in existingDatum)
                {
                    Assert.AreEqual(ed.Quality, result.ElementAt(index).Quality);
                    Assert.AreEqual(ed.Timestamp, result.ElementAt(index).Timestamp);
                    Assert.AreEqual(ed.Value, result.ElementAt(index).Value);
                    index++;
                }
            }
            
            private void SetupSignalsWebServiceAndMissingValue()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void prepareDataRepository()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
        }
    }
}