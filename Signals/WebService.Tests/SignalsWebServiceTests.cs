﻿
using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Domain.Exceptions;
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
            public void GivenASignal_WhenGettingByItsPath_ReturnsIt()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));
                var path = new Dto.Path() { Components = new string[] { "root", "signal" } };

                var result = signalsWebService.Get(path);

                Assert.AreEqual(signalId, result.Id);
                Assert.AreEqual(Dto.DataType.Integer, result.DataType);
                Assert.AreEqual(Dto.Granularity.Second, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenASignal_WhenSettingData_CallsRepositorySetData()
            {
                var signalId = 1;
                var data = new Dto.Datum[] {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } };
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.Double,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                signalsWebService.SetData(signalId, data);

                signalsDataRepositoryMock.Verify(sdr => sdr.SetData(It.Is<IEnumerable<Datum<double>>>(
                    d => d.Count() == 3
                    && DatumDomainEquals(d.First(), Domain.Quality.Fair, new DateTime(2000, 1, 1), 1.0, signalId)
                    && DatumDomainEquals(d.Skip(1).First(), Domain.Quality.Good, new DateTime(2000, 2, 1), 1.5, signalId)
                    && DatumDomainEquals(d.Skip(2).First(), Domain.Quality.Poor, new DateTime(2000, 3, 1), 2.0, signalId)
                    )));
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignal_WhenSettingData_ThrowsCouldntGetASignalException()
            {
                GivenNoSignals();

                var data = new Dto.Datum[] {
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } };

                signalsWebService.SetData(1, data);
            }


            [TestMethod]
            public void GivenData_WhenGettingData_ReturnsIt()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("x/y")));
                GivenData(signalId, new Domain.Datum<double>[] {
                    new Domain.Datum<double>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                Assert.AreEqual(2, result.Count());
                Assert.AreEqual(Dto.Quality.Fair, result.First().Quality);
                Assert.AreEqual(Dto.Quality.Good, result.Skip(1).First().Quality);
                Assert.AreEqual(new DateTime(2000, 1, 1), result.First().Timestamp);
                Assert.AreEqual(new DateTime(2000, 2, 1), result.Skip(1).First().Timestamp);
                Assert.AreEqual(1.0, result.First().Value);
                Assert.AreEqual(1.5, result.Skip(1).First().Value);
            }


            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignal_WhenGettingData_ThrowsCouldntGetASignalException()
            {
                GivenNoSignals();
                GivenData(1, new Domain.Datum<double>[] {
                    new Domain.Datum<double>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
            }

            [TestMethod]
            public void GivenAMissingValuePolicy_WhenGettingMissingValuePolicy_ReturnsIt()
            {
                var signalId = 1;
                var signal = SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                GivenMissingValuePolicy(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyInteger()
                {
                    Signal = signal,
                    Quality = Domain.Quality.Poor,
                    Value = 4
                });

                var result = signalsWebService.GetMissingValuePolicy(signalId) as Dto.MissingValuePolicy.SpecificValueMissingValuePolicy;


                Assert.AreEqual(signalId, result.Signal.Id);
                Assert.AreEqual(Dto.DataType.Integer, result.DataType);
                Assert.AreEqual(4, result.Value);
                Assert.AreEqual(Dto.Quality.Poor, result.Quality);
            }


            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignal_WhenGettingMissingValuePolicy_CouldntGetASignalExceptionIsThrown()
            {
                GivenNoSignals();

                var result = signalsWebService.GetMissingValuePolicy(1);
            }


            [TestMethod]
            public void GivenNoMissingValuePolicy_WhenGettingMissingValuePolicy_ReturnsNull()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.String,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.GetMissingValuePolicy(signalId);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicy_CallsRepositorySet()
            {
                var signalId = 1;
                var signal = SignalWith(
                    id: signalId,
                    dataType: DataType.String,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal"));
                var signalDto = new Dto.Signal()
                {
                    Id = signalId,
                    DataType = Dto.DataType.String,
                    Granularity = Dto.Granularity.Second,
                    Path = new Dto.Path { Components = new string[] { "root", "signal" } }
                };
                GivenASignal(signal);
                var missingValuePolicy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
                {
                    Quality = Dto.Quality.Fair,
                    Value = "Example",
                    DataType = Dto.DataType.String,
                    Signal = signalDto
                };

                signalsWebService.SetMissingValuePolicy(signalId, missingValuePolicy);

                missingValuePolicyRepositoryMock
                    .Verify(mvpr => mvpr.Set(It.Is<Domain.Signal>(s => s.Id == signalId),
                    It.Is<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<string>>(mvp =>
                    mvp.Value == "Example"
                    && mvp.Quality == Quality.Fair
                    && mvp.Signal.Id == signalId)));
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignal_WhenSettingMissingValuePolicy_CouldntGetASignalExceptionIsThrown()
            {
                GivenNoSignals();
                var missingValuePolicy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
                {
                    Quality = Dto.Quality.Fair,
                    Value = "Example",
                    DataType = Dto.DataType.String
                };

                signalsWebService.SetMissingValuePolicy(1, missingValuePolicy);
            }

            [TestMethod]
            public void GivenNoSignal_WhenGettingById_NullIsReturned()
            {
                GivenNoSignals();
                int nonExistingId = 5;

                var result = signalsWebService.GetById(nonExistingId);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenNoSignal_WhenGettingByPath_NullIsReturned()
            {
                GivenNoSignals();
                var nonExistingPathDto = new Dto.Path()
                {
                    Components = new[] { "non", "existing", "path" }
                };

                var result = signalsWebService.Get(nonExistingPathDto);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GetData_NoData_SpecificValueMissingValuePolicyShouldReturnData()
            {
                var signalId = 1;
                var signal = new Signal() { Id = signalId, DataType = DataType.Double, Granularity = Granularity.Month, Path = Path.FromString("x/y") };

                GivenASignal(signal);
                GivenData(signalId, Enumerable.Empty<Datum<double>>());

                GivenMissingValuePolicy(new Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>()
                {
                    Signal = signal,
                    Id = signalId
                });

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 6, 1));

                Assert.AreEqual(5, result.Count());
            }

            [TestMethod]
            public void GetData_TwoMissingPoints_SpecificValueMissingValuePolicyShouldFillMissingData()
            {
                int signalId = 1;
                var signal = SignalWith(signalId, DataType.Integer, Granularity.Day, Path.FromString("x/y"));


                var data = new Domain.Datum<int>[] {
                    new Domain.Datum<int>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = (int)2 },
                    new Domain.Datum<int>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 4), Value = (int)1 },
                };

                GivenASignal(signal);
                GivenData(signalId, data);

                GivenMissingValuePolicy(new Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<int>()
                {
                    Quality = Quality.Good,
                    Value = 30,
                    Signal = signal,
                    Id = signalId
                });

                var expectedResult = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = (int)2 },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2), Value = 30 },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3), Value = 30},
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 4), Value = (int)1 }
                };

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 5));

                AssertDataDtoEquals(expectedResult, result);
            }

            [TestMethod]
            public void GetData_EqualTimestamps_ReturnSingleDatum()
            {
                int signalId = 1;
                var signal = SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("x/y"));
                GivenASignal(signal);
                signalsDataRepositoryMock
                    .Setup(x => x.GetData<double>(It.IsAny<Domain.Signal>(), new DateTime(2000, 2, 1), new DateTime(2000, 2, 1)))
                    .Returns(Enumerable.Repeat(new Domain.Datum<double>() { Signal = signal, Quality = Domain.Quality.Good,
                        Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 }, 1));

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 2, 1), new DateTime(2000, 2, 1));

                var expectedDataDto = Enumerable.Repeat(new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 }, 1);
                AssertDataDtoEquals(result, expectedDataDto);
            }

            [TestMethod]
            public void GivenData_WhenGettingData_DataIsReturnedOrderedByTimestampAscending()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("x/y")));
                GivenData(signalId, new Domain.Datum<double>[] {
                    new Domain.Datum<double>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 }
                     });

                var expectedDataDto = new Dto.Datum[]
                {
                    new Dto.Datum() {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                };

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                AssertDataDtoEquals(expectedDataDto, result);
            }

            [TestMethod]
            public void GivenData_WhenGettingData_GetMissingValuePolicyIsCalled()
            {
                int signalId = 1;
                var signal = SignalWith(signalId, DataType.Double, Granularity.Month, Path.FromString("x/y"));

                GivenASignal(SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("x/y")));
                GivenData(signalId, new Domain.Datum<double>[] {
                    new Domain.Datum<double>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 }
                });

                signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 3));
                missingValuePolicyRepositoryMock.Verify(mvp => mvp.Get(It.Is<Domain.Signal>(s => s.Id == signalId)), Times.Once);
            }


            [TestMethod]
            public void GivenDataMissingMultipleElements_WhenGettingData_DataWithFilledMissingValuesIsReturned()
            {
                int signalId = 1;
                var signal = SignalWith(signalId, DataType.Integer, Granularity.Day, Path.FromString("x/y"));


                var data = new Domain.Datum<int>[] {
                    new Domain.Datum<int>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = (int)2 },
                    new Domain.Datum<int>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 4), Value = (int)1 },
                };

                GivenASignal(signal);
                GivenData(signalId, data);

                GivenMissingValuePolicy(new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>()
                {
                    Signal = signal,
                    Id = signalId
                });

                var expectedResult = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1), Value = (int)2 },
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 2), Value = default(int) },
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 3), Value = default(int)},
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 4), Value = (int)1 }
                };

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 5));

                AssertDataDtoEquals(expectedResult, result);
            }


            [TestMethod]
            public void GetData_NoData_NoneQualityMissingValuePolicyShoulReturnData()
            {
                var signalId = 1;
                var signal = new Signal() { Id = signalId, DataType = DataType.Double, Granularity = Granularity.Month, Path = Path.FromString("x/y") };

                GivenASignal(signal);
                GivenData(signalId, Enumerable.Empty<Datum<double>>());

                GivenMissingValuePolicy(new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>()
                {
                    Signal = signal,
                    Id = signalId
                });

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 6, 1));

                Assert.AreEqual(5, result.Count());
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_SetMissingValuePolicyIsCalled()
            {
                GivenNoSignals();

                signalsWebService.Add(SignalWith(null, Dto.DataType.Integer, Dto.Granularity.Minute, new Dto.Path()
                {
                    Components = new[] { "root", "signal1" }
                }));

                missingValuePolicyRepositoryMock.Verify(mvp => mvp.Set(It.Is<Domain.Signal>(s => s.DataType == DataType.Integer &&
                    s.Granularity == Granularity.Minute &&
                    s.Path.ToString().Equals("root/signal1")),
                    It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>()),
                    Times.Once);
            }

            [TestMethod]
            public void GetByPathEntry_SignalsWithThisPrefixExist_ReturnCorrectPathEntry()
            {
                List<Signal> signals = new List<Signal>()
                {
                    new Signal() {Id=1, DataType= DataType.Double, Granularity=Granularity.Day, Path=Path.FromString("root/s1") },
                    new Signal() {Id=2, DataType= DataType.Double, Granularity=Granularity.Day, Path=Path.FromString("root/podkatalog/s2") },
                    new Signal() {Id=3, DataType= DataType.Double, Granularity=Granularity.Day, Path=Path.FromString("root/podkatalog/s3") },
                    new Signal() {Id=4, DataType= DataType.Double, Granularity=Granularity.Day, Path=Path.FromString("root/podkatalog/podpodkatalog/s4") },
                    new Signal() {Id=5, DataType= DataType.Double, Granularity=Granularity.Day, Path=Path.FromString("root/podkatalog2/s5") },
                };

                GivenNoSignals();
                GivenASignal(new Signal() { Id = 1, DataType = DataType.Double, Granularity = Granularity.Day, Path = Path.FromString("root/s1") });
                signalsRepositoryMock.Setup(x => x.GetAllWithPathPrefix(It.IsAny<Path>())).Returns(signals);

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root" } });

                Assert.AreEqual(string.Join("/", result.Signals.ToList()[0].Path.Components), "root/s1");
                Assert.AreEqual(string.Join("/", result.SubPaths.ToList()[0].Components), "root/podkatalog");
                Assert.AreEqual(string.Join("/", result.SubPaths.ToList()[1].Components), "root/podkatalog2");
            }

            [TestMethod]
            public void GetByPathEntry_SubPathIsAlsoSignal_ReturnCorrectPathEntry()
            {
                List<Signal> signals = new List<Signal>()
                {
                    new Signal() {Id=1, DataType= DataType.Double, Granularity=Granularity.Day, Path=Path.FromString("root/s1") },
                    new Signal() {Id=2, DataType= DataType.Double, Granularity=Granularity.Day, Path=Path.FromString("root/s1/s2") }
                };

                GivenNoSignals();
                GivenASignal(new Signal() { Id = 1, DataType = DataType.Double, Granularity = Granularity.Day, Path = Path.FromString("root/s1/s2") });
                signalsRepositoryMock.Setup(x => x.GetAllWithPathPrefix(It.IsAny<Path>())).Returns(signals);

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root/s1" } });

                Assert.AreEqual(string.Join("/", result.Signals.ToList()[0].Path.Components), "root/s1/s2");
            }

            [TestMethod]
            [ExpectedException(typeof(IncorrectDatumTimestampException))]
            public void GivenASignal_WhenSettingData_ThrowsIncorrectDatumTimestampException()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.Id = 5;
                GivenASignal(signal);

                Dto.Datum[] data = new Dto.Datum[]
                {
                    new Dto.Datum() {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2001,4,3), Value = default(int) }
                };

                signalsWebService.SetData(signal.Id.Value,data);
            }

            [TestMethod]
            [ExpectedException(typeof(IncorrectDatumTimestampException))]
            public void GivenNoData_WhenGettingData_ThrowsIncorrectDatumTimestampException()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.Id = 5;
                GivenASignal(signal);

                signalsWebService.GetData(signal.Id.Value, new DateTime(2001, 3, 2), new DateTime(2001, 5, 4));
            }

            [TestMethod]
            [ExpectedException(typeof(IncorrectDatumTimestampException))]
            public void GivenASignal_WhenGettingData_ThrowsIncorrectDatumTimestampException()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.Id = 5;
                GivenASignal(signal);

                Domain.Datum<int>[] data = new Domain.Datum<int>[]
                {
                    new Domain.Datum<int>() {
                        Quality = Domain.Quality.Bad,
                        Timestamp = new DateTime(2001,4,3),
                        Value = default(int),
                        Signal = signal
                    }
                };

                GivenData(signal.Id.Value,data);

                signalsWebService.GetData(signal.Id.Value, new DateTime(2001, 3, 1), new DateTime(2001,5,1));
            }


            [TestMethod]
            public void GivenAData_WhenGettingData_ReturnsDataWithFilledZeroOrderMissingValuePolicy()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);

                Domain.Datum<double>[] data = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
                };

                GivenData(signal.Id.Value,data);

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<double>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);

                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
                int expectedCount = 3;
                Assert.AreEqual(expectedCount,result.Count());

                Dto.Datum[] expectedData = new Dto.Datum[]
                {
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
                };

                AssertDataDtoEquals(expectedData,result.ToArray());
            }
            [TestMethod]
            [ExpectedException(typeof(IncorrectDatumTimestampException))]
            public void GivenASignal_WhenGettingDataWithWeekTimestamp_ThrowsIncorrectDatumTimestampException()
            {
                var signal = new Signal() {DataType=DataType.Integer,Granularity=Granularity.Week,Path=Path.FromString("x/y") };
                signal.Id = 5;
                GivenASignal(signal);

                Domain.Datum<int>[] data = new Domain.Datum<int>[]
                {
                    new Domain.Datum<int>() {
                        Quality = Domain.Quality.Bad,
                        Timestamp = new DateTime(2016,8,29),
                        Value = default(int),
                        Signal = signal
                    }
                };

                GivenData(signal.Id.Value, data);

                signalsWebService.GetData(signal.Id.Value, new DateTime(2016, 8, 28), new DateTime(2016, 9, 4));
            }

            [TestMethod]
            public void GivenAData_WhenGettingDataWithNoFirstDatum_ReturnsDataWithFilledZeroOrderMissingValuePolicy()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);

                Domain.Datum<double>[] data = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
                };

                GivenData(signal.Id.Value, data);

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<double>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);

                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
                int expectedCount = 3;
                Assert.AreEqual(expectedCount, result.Count());

                Dto.Datum[] expectedData = new Dto.Datum[]
                {
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = default(double)},
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 2, 1), Value = default(double) },
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
                };

                AssertDataDtoEquals(expectedData, result.ToArray());
            }


            [TestMethod]
            public void GivenAData_WhenGettingADataWithWeekTimeStamp_ReturnsData()
            {
                //arrange
                var dummyId = 1;
                Domain.Signal signal = new Signal() { DataType = DataType.Double, Granularity = Granularity.Week ,Id=dummyId,Path=Path.FromString("x/y")};

                Domain.Datum<double>[] data = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 8, 22), Value = (double)2.5 },
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(2016, 8, 29), Value = (double)5.4 }
                };
                GivenASignal(signal);
                GivenData(dummyId, data);

                //act
                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2016,8,22), new DateTime(2016, 9, 5)).ToArray();

                //assert
                Assert.AreEqual(data.Length, result.Count());
                Assert.AreEqual(Dto.Quality.Good, result[0].Quality);
                Assert.AreEqual(new DateTime(2016, 8, 22), result[0].Timestamp);
                Assert.AreEqual(2.5, result[0].Value);
                Assert.AreEqual(Dto.Quality.Bad, result[1].Quality);
                Assert.AreEqual(new DateTime(2016, 8, 29), result[1].Timestamp);
                Assert.AreEqual(5.4, result[1].Value);
            }
            [TestMethod]
            public void GivenNoData_WhenGettingDataWithFromIncludedUtcEqualsToExcluededUtc_ReturnsDataWithFilledZeroOrderMissingValuePolicy()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<double>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);

                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));
                int expectedCount = 1;
                Assert.AreEqual(expectedCount, result.Count());

                Dto.Datum[] expectedData = new Dto.Datum[]
                {
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = default(double)},
                
                };

                AssertDataDtoEquals(expectedData, result.ToArray());
            }

            [TestMethod]
            public void GivenAData_WhenGettingData_ReturnsDataWithFilledZeroOrderMissingValuePolicyInEveryTimeStamp()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);

                Domain.Datum<double>[] data = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 4, 1), Value = (double)2.5 }
                };

                GivenData(signal.Id.Value, data);

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<double>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);

                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 6, 1));
                int expectedCount = 5;
                Assert.AreEqual(expectedCount, result.Count());

                Dto.Datum[] expectedData = new Dto.Datum[]
                {
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)1.5 },
                   new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 4, 1), Value = (double)2.5 },
                   new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 5, 1), Value = (double)2.5 },
                };

                AssertDataDtoEquals(expectedData, result.ToArray());
            }
            [TestMethod]
            public void GivenNoData_WhenGettingData_ReturnsDataWithFilledZeroOrderMissingValuePolicy()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);

                Domain.Datum<double>[] data = new Domain.Datum<double>[]
                {
                };

                GivenData(signal.Id.Value, data);

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<double>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);

                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 6, 1));
                int expectedCount = 5;
                Assert.AreEqual(expectedCount, result.Count());

                Dto.Datum[] expectedData = new Dto.Datum[]
                {
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = default(double) },
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 2, 1), Value = default(double) },
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1), Value = default(double)},
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 4, 1), Value = default(double) },
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 5, 1), Value = default(double)},
                };

                AssertDataDtoEquals(expectedData, result.ToArray());
            }

            [TestMethod]
            public void GivenAData_WhenGettingData_ReturnsDataWithZeroOrderMissingValuePolicyFromOlderDatum()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);

                Domain.Datum<double>[] data = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                   
                };

                GivenData(signal.Id.Value, data);

                signalsDataRepositoryMock
                  .Setup(sdr => sdr.GetDataOlderThan<Double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                 .Returns<Domain.Signal,DateTime,int>((s, from, to) => data);

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<double>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);

                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 5, 1), new DateTime(2000, 6, 1));
                int expectedCount = 1;
                Assert.AreEqual(expectedCount, result.Count());

                Dto.Datum[] expectedData = new Dto.Datum[]
                {
                 
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = (double)1.5 },
                };

                AssertDataDtoEquals(expectedData, result.ToArray());
            }

            private Signal GetDefaultSignal_IntegerMonth()
            {
                return new Signal()
                {
                    DataType = DataType.Integer,
                    Granularity = Granularity.Month,
                    Path = Domain.Path.FromString("x/y")
                };
            }

            private void VerifySetDataCallToFillSingleMissingData<T>(int signalId, DateTime timeStamp, Quality quality = Quality.None, T value = default(T))
            {
                signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<T>(It.Is<IEnumerable<Datum<T>>>(data => data.Count().Equals(1) &&
                    data.ElementAt(0).Signal.Id == signalId &&
                    data.ElementAt(0).Quality == quality &&
                    data.ElementAt(0).Timestamp == timeStamp &&
                    data.ElementAt(0).Value.Equals(value))), Times.Once);
            }

            private void GivenMissingValuePolicy(Domain.MissingValuePolicy.MissingValuePolicyBase missingValuePolicy)
            {
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Get(It.Is<Domain.Signal>(s => s.Id == missingValuePolicy.Signal.Id)))
                    .Returns<Domain.Signal>(s => missingValuePolicy);
            }

            private void AssertDataDtoEquals(IEnumerable<Dto.Datum> data1, IEnumerable<Dto.Datum> data2)
            {
                bool countsAreEqual = data1.Count() == data2.Count();

                Assert.IsTrue(countsAreEqual);

                if (countsAreEqual)
                {
                    for (int i = 0; i < data1.Count(); i++)
                    {
                        Assert.AreEqual(data1.ElementAt(i).Quality, data2.ElementAt(i).Quality);
                        Assert.AreEqual(data1.ElementAt(i).Timestamp, data2.ElementAt(i).Timestamp);
                        Assert.AreEqual(data1.ElementAt(i).Value, data2.ElementAt(i).Value);
                    }
                }
            }

            private bool DatumDomainEquals<T>(Domain.Datum<T> datum, Domain.Quality quality, DateTime timeStamp, T value, int signalId)
            {
                return datum.Quality == quality && datum.Timestamp == timeStamp
                    && datum.Value.Equals(value) && datum.Signal.Id == signalId;
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

            private void GivenData<T>(int signalId, IEnumerable<Domain.Datum<T>> data)
            {
                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetData<T>(It.Is<Domain.Signal>(s => s.Id == signalId), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns<Domain.Signal, DateTime, DateTime>((s, from, to) => data.Where(d => d.Timestamp >= from && d.Timestamp < to));
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
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Signal signal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);

                signalsRepositoryMock
                .Setup(sr => sr.Get(It.Is<Domain.Path>(p => p.ToString() == signal.Path.ToString())))
                .Returns(signal);
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
        }
    }
}

