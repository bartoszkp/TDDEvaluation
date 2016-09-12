
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


            [TestMethod]
            public void GivenASignal_WhenDeletingASignal_DeleteDatumIsCalled()
            {
                //arrange
                var dummyId = 2;
                GivenASignal(new Signal() { Id = dummyId, DataType = DataType.Double, Granularity = Granularity.Month, Path = Path.FromString("x/y") });

                //act
                signalsWebService.Delete(dummyId);
                //assert
                signalsDataRepositoryMock.Verify(sr => sr.DeleteData<double>(It.Is<Domain.Signal>(passedSignal
                   => passedSignal.DataType == DataType.Double
                       && passedSignal.Granularity == Granularity.Month
                       && passedSignal.Path.ToString() == "x/y")));
            }
            [TestMethod]
            public void GivenASignal_WhenDeletingASignal_SetMissingValuePolicyIsCalled()
            {
                //arrange
                var dummyId = 2;
                GivenASignal(new Signal() { Id = dummyId, DataType = DataType.Double, Granularity = Granularity.Month, Path = Path.FromString("x/y") });

                //act
                signalsWebService.Delete(dummyId);
                //assert
                missingValuePolicyRepositoryMock.Verify(sr => sr.Set(It.Is<Domain.Signal>(passedSignal
                   => passedSignal.DataType == DataType.Double
                       && passedSignal.Granularity == Granularity.Month
                       && passedSignal.Path.ToString() == "x/y"),It.Is< Domain.MissingValuePolicy.MissingValuePolicyBase>( mvp=>mvp==null)));
            }
            [TestMethod]
            public void GivenASignal_WhenDeletingASignal_DeleteSignalIsCalled()
            {
                //arrange
                var dummyId = 2;
                GivenASignal(new Signal() { Id = dummyId, DataType = DataType.Double, Granularity = Granularity.Month, Path = Path.FromString("x/y") });

                //act
                signalsWebService.Delete(dummyId);
                //assert
                signalsRepositoryMock.Verify(sr => sr.Delete(It.Is<Domain.Signal>(passedSignal
                   => passedSignal.DataType == DataType.Double
                       && passedSignal.Granularity == Granularity.Month
                       && passedSignal.Path.ToString() == "x/y")));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignal_WhenDeletingASignal_ExceptionIsThrown()
            {
                //arrange
                var dummyId = 2;
                var wrongId = 5;
                GivenASignal(new Signal() { Id = dummyId, DataType = DataType.Double, Granularity = Granularity.Month, Path = Path.FromString("x/y") });

                //act
                signalsWebService.Delete(wrongId);
                //assert
                
            }

            [TestMethod]
            public void GivenAData_WhenGettingDataOutOfDataRange_ReturnsDefaultValuesFirstOrderMissingValuePolicy()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.Id = 5;
                GivenASignal(signal);

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<int>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);
                Domain.Datum<int>[] data = new Domain.Datum<int>[]
              {
                    new Domain.Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(1999, 12, 1), Value = (int)2 },
                    new Domain.Datum<int>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 2, 1), Value = (int)4 },

              };
                GivenData(signal.Id.Value, data);

                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 3, 1), new DateTime(2000, 5, 1));
                int expectedCount = 2;
                Assert.AreEqual(expectedCount, result.Count());

                Dto.Datum[] expectedData = new Dto.Datum[]
                {
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1),Value= default(Int32)},
                      new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 4, 1),Value= default(Int32)}

                };

                AssertDataDtoEquals(expectedData, result.ToArray());
            }

            [TestMethod]
            public void GivenNoData_WhenGettingData_ReturnsDefaultValuesFirstOrderMissingValuePolicy()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Decimal;
                signal.Id = 5;
                GivenASignal(signal);

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<decimal>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);

                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));
                int expectedCount = 1;
                Assert.AreEqual(expectedCount, result.Count());

                Dto.Datum[] expectedData = new Dto.Datum[]
                {
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = default(decimal)},

                };

                AssertDataDtoEquals(expectedData, result.ToArray());
            }

            [TestMethod]
            public void GivenAData_WhenGettingDataWithFromIncludedUtcEqualsToExcluededUtc_ReturnsDataWithFilledFirstOrderMissingValuePolicy()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);
                
                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<double>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);
                Domain.Datum<double>[] data = new Domain.Datum<double>[]
              {
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(1999, 12, 1), Value = (double)2 },
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 2, 1), Value = (double)4 },

              };
                double expectedValue = 3;
                GivenData(signal.Id.Value, data);

                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));
                int expectedCount = 1;
                Assert.AreEqual(expectedCount, result.Count());

                Dto.Datum[] expectedData = new Dto.Datum[]
                {
                   new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = expectedValue},

                };

                AssertDataDtoEquals(expectedData, result.ToArray());
            }

            [TestMethod]
            public void GivenAData_WhenGettingDataWithWithDateTimeOutOfDataRange_ReturnsDataWithFilledFirstOrderMissingValuePolicy()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<double>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);
                Domain.Datum<double>[] data = new Domain.Datum<double>[]
              {
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(1999, 12, 1), Value = (double)2.5 },
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)3 },
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = (double)4.5 },
                    new Domain.Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 6, 1), Value = (double)3.5 },

              };
                double expectedValue = 3;
                GivenData(signal.Id.Value, data);

                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(1999, 11, 1), new DateTime(2000, 8, 1));
                int expectedCount = 9;
                Assert.AreEqual(expectedCount, result.Count());

                Dto.Datum[] expectedData = new Dto.Datum[]
                {
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(1999, 11, 1), Value = default(double)},
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(1999, 12, 1), Value = (double)2.5},
                   new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)3},

                   new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 2, 1), Value = (double)3.5},
                   new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 3, 1), Value = (double)4},
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = (double)4.5},

                   new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 5, 1), Value = (double)4},
                   new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 6, 1), Value = (double)3.5},
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 7, 1), Value = default(double)},
                };

                AssertDataDtoEquals(expectedData, result.ToArray());
            }

            [TestMethod]
            public void GivenASignalWithFirstOrderMissingValuePolicy_WhenGettingData_MissingDataHasWorseQualityFromNeighbours()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<double>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);
                Domain.Datum<double>[] data = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)3 },
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)7 },

                };
                GivenData(signal.Id.Value, data);
                var expectedData = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)3},
                    new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 2, 1), Value = (double)5},
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)7},

                };


                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                AssertDataDtoEquals(expectedData, result.ToArray());
            }



            [TestMethod]
            public void GivenASignalWithFirstOrderMissingValuePolicy_WhenGettingData_ValuesWithNeighboursFromOutsideRangeAreInterpolatedCorrectly()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<double>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);
                Domain.Datum<double>[] data = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(1999, 11, 1), Value = (double)3 },
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(1999, 10, 1), Value = (double)5 },
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)7 },

                };
                GivenData(signal.Id.Value, data);
                var expectedData = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)5},
                    new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 2, 1), Value = (double)6},
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)7},

                };


                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                AssertDataDtoEquals(expectedData, result.ToArray());
            }

            [TestMethod]
            public void GivenASignalWithFirstOrderMissingValuePolicy_WhenGettingDataWithToEarlierThanFrom_ReturnsEmptyResult()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<double>();
                policy.Signal = signal;
                GivenMissingValuePolicy(policy);
                Domain.Datum<double>[] data = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(1999, 11, 1), Value = (double)3 },
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(1999, 10, 1), Value = (double)5 },
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)7 },

                };
                GivenData(signal.Id.Value, data);

                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 4, 1), new DateTime(2000, 1, 1));

                Assert.AreEqual(0, result.Count());
            }

            [TestMethod]
            public void GivenASignalWithShadowMissingValuePolicy_WhenGettingData_OnlyMissingValuesAreCopiedFromShadowSignal()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);
                var shadowSignal = GetDefaultSignal_IntegerMonth();
                shadowSignal.DataType = DataType.Double;
                shadowSignal.Id = 6;

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.ShadowMissingValuePolicy<double>();
                policy.Signal = signal;
                (policy as Domain.MissingValuePolicy.ShadowMissingValuePolicy<double>).ShadowSignal = shadowSignal;
                GivenMissingValuePolicy(policy);
                Domain.Datum<double>[] data = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)3 },
                    new Domain.Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)5 },
                };
                Domain.Datum<double>[] shadowData = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)30 },
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 3, 1), Value = (double)10 },
                    new Domain.Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 4, 1), Value = (double)1 },
                };
                GivenData(signal.Id.Value, data);
                GivenData(shadowSignal.Id.Value, shadowData);
                var expectedData = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)3},
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)30},
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)5},
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 4, 1), Value = (double)1},

                };


                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));

                AssertDataDtoEquals(expectedData, result.ToArray());
            }

            [TestMethod]
            public void GivenASignalWithShadowMissingValuePolicy_WhenGettingDataThatIsMissingFromBothSignals_MissingValuesHaveNoneQualityAndDefaultValue()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);
                var shadowSignal = GetDefaultSignal_IntegerMonth();
                shadowSignal.DataType = DataType.Double;
                shadowSignal.Id = 6;

                Domain.MissingValuePolicy.MissingValuePolicyBase policy = new Domain.MissingValuePolicy.ShadowMissingValuePolicy<double>();
                policy.Signal = signal;
                (policy as Domain.MissingValuePolicy.ShadowMissingValuePolicy<double>).ShadowSignal = shadowSignal;
                GivenMissingValuePolicy(policy);
                Domain.Datum<double>[] data = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)3 },
                    new Domain.Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)5 },
                };
                Domain.Datum<double>[] shadowData = new Domain.Datum<double>[]
                {
                    new Domain.Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 3, 1), Value = (double)10 },
                };
                GivenData(signal.Id.Value, data);
                GivenData(shadowSignal.Id.Value, shadowData);
                var expectedData = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)3},
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 2, 1), Value = default(double)},
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)5},
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 4, 1), Value = default(double)},

                };


                var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));

                AssertDataDtoEquals(expectedData, result.ToArray());
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingShadowPolicyWithDifferentGranularity_ArgumentExceptionIsThrown()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);
                var shadowSignal = SignalWith(6, Dto.DataType.Double, Dto.Granularity.Day);

                Dto.MissingValuePolicy.ShadowMissingValuePolicy policy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy();
                policy.ShadowSignal = shadowSignal;
                policy.DataType = Dto.DataType.Double;

                signalsWebService.SetMissingValuePolicy(signal.Id.Value, policy);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingShadowPolicyWithShadowWithDifferentDataType_ArgumentExceptionIsThrown()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);
                var shadowSignal = SignalWith(6, Dto.DataType.Decimal, Dto.Granularity.Month);

                Dto.MissingValuePolicy.ShadowMissingValuePolicy policy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy();
                policy.ShadowSignal = shadowSignal;
                policy.DataType = Dto.DataType.Double;

                signalsWebService.SetMissingValuePolicy(signal.Id.Value, policy);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingShadowPolicyWithDifferentDataType_ArgumentExceptionIsThrown()
            {
                var signal = GetDefaultSignal_IntegerMonth();
                signal.DataType = DataType.Double;
                signal.Id = 5;
                GivenASignal(signal);
                var shadowSignal = SignalWith(6, Dto.DataType.Double, Dto.Granularity.Month);

                Dto.MissingValuePolicy.ShadowMissingValuePolicy policy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy();
                policy.ShadowSignal = shadowSignal;
                policy.DataType = Dto.DataType.Decimal;

                signalsWebService.SetMissingValuePolicy(signal.Id.Value, policy);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingShadowMVP_IfSignalIsEqualToChosenShadowSignal_ArgumentExceptionIsThrown()
            {
                var signal = new Domain.Signal()
                {
                    Id = 5,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Day,
                    Path = Path.FromString("path")
                };
                GivenASignal(signal);

                missingValuePolicyRepositoryMock.SetupSequence(mvprm => mvprm.Get(It.Is<Domain.Signal>(s => s.Id == 5 &&
                    s.Granularity == Domain.Granularity.Day &&
                    s.DataType == Domain.DataType.Decimal &&
                    s.Path.ToString().Equals("path")))).Returns(new ShadowMissingValuePolicyDecimal()).Returns(new ShadowMissingValuePolicyDecimal());

                signalsWebService.SetMissingValuePolicy(signal.Id.Value, new Dto.MissingValuePolicy.ShadowMissingValuePolicy()
                {
                    DataType = Dto.DataType.Decimal,
                    ShadowSignal = new Dto.Signal()
                    {
                        Id = 5,
                        DataType = Dto.DataType.Decimal,
                        Granularity = Dto.Granularity.Day,
                        Path = new Dto.Path() { Components = new[] {"path"} }
                    }
                });
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingShadowMVP_IfChosenShadowSignalHasItsShadowSignalEqualToFirstSignal_ArgumentExceptionIsThrown()
            {
                var signal2 = new Domain.Signal()
                {
                    Id = 7,
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Month,
                    Path = Domain.Path.FromString("path/2")
                };
                GivenASignal(signal2);
                var signal1 = new Dto.Signal()
                {
                    Id = 6,
                    DataType = Dto.DataType.Integer,
                    Granularity = Dto.Granularity.Month,
                    Path = new Dto.Path()
                    {
                        Components = new[] {"path", "1"}
                    }
                };

                missingValuePolicyRepositoryMock.SetupSequence(mvprm => mvprm.Get(It.Is<Domain.Signal>(s => s.Id == 6 &&
                    s.Granularity == Domain.Granularity.Month &&
                    s.DataType == Domain.DataType.Integer &&
                    s.Path.ToString().Equals("path/1")))).Returns(new ShadowMissingValuePolicyInteger()
                    {
                        ShadowSignal = signal2
                    }).Returns(new ShadowMissingValuePolicyInteger()
                    {
                        ShadowSignal = signal2
                    });

                signalsWebService.SetMissingValuePolicy(signal2.Id.Value, new Dto.MissingValuePolicy.ShadowMissingValuePolicy()
                {
                    DataType = Dto.DataType.Integer,
                    ShadowSignal = signal1 
                });
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignals_WhenGettingCoarseData_ExceptionIsThrown()
            {
                GivenNoSignals();
                signalsWebService.GetCoarseData(1, It.IsAny<Dto.Granularity>(), It.IsAny<DateTime>(), It.IsAny<DateTime>());
            }

            [TestMethod]
            [ExpectedException(typeof(IncorrectDatumTimestampException))]
            public void GivenASignal_WhenGettingCoarseDataWithInvalidTimestamp_ExceptionIsThrown()
            {
                GivenASignal(new Signal()
                {
                    Id = 2,
                    Granularity = Granularity.Hour,
                    DataType = DataType.Decimal,
                    Path = Path.FromString("signal")
                });
                signalsWebService.GetCoarseData(2, Dto.Granularity.Day, new DateTime(2000, 1, 1, 23, 0, 0), new DateTime(2000, 1, 3));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingCoarseDataWithInvalidGranularity_ArgumentExceptionIsThrown()
            {
                GivenASignal(new Signal()
                {
                    Id = 3,
                    Granularity = Granularity.Month,
                    DataType = DataType.Double,
                    Path = Path.FromString("signal")
                });
                signalsWebService.GetCoarseData(3, Dto.Granularity.Day, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignalWithBooleanDatatype_WhenGettingCoarseData_ArgumentExceptionIsThrown()
            {
                GivenASignal(new Signal()
                {
                    Id = 1,
                    Granularity = Granularity.Day,
                    DataType = DataType.Boolean,
                    Path = Path.FromString("signal")
                });
                signalsWebService.GetCoarseData(1, Dto.Granularity.Month, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
            }

            [TestMethod]
            public void GivenASignal_WhenGettingCoarseDataWithToTimestampEarlierThanFromTimestamp_EmptyDataIsReturned()
            {
                GivenASignal(new Signal()
                {
                    Id = 4,
                    Granularity = Granularity.Day,
                    DataType = DataType.Decimal,
                    Path = Path.FromString("signal")
                });

                var result = signalsWebService.GetCoarseData(4, Dto.Granularity.Month, new DateTime(2000, 2, 1), new DateTime(2000, 1, 1));
                Assert.IsFalse(result.Any());
            }

            [TestMethod]
            public void GivenASignal_WhenGettingCoarseDataWithEqualTimestamps_DataWithSingleElementIsReturned()
            {
                var signal = new Signal()
                {
                    Id = 5,
                    Granularity = Granularity.Day,
                    DataType = DataType.Integer,
                    Path = Path.FromString("signal/path")
                };
                GivenASignal(signal);
                var timestamp = new DateTime(2016, 1, 4);

                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<int>(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                    s.Granularity == signal.Granularity &&
                    s.DataType == signal.DataType &&
                    s.Path.ToString().Equals(signal.Path.ToString())), timestamp, timestamp)).Returns(new Datum<int>[]
                    {
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 4), Value = 1 }
                    });
                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<int>(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                    s.Granularity == signal.Granularity &&
                    s.DataType == signal.DataType &&
                    s.Path.ToString().Equals(signal.Path.ToString())), timestamp, new DateTime(2016, 1, 11))).Returns(new Datum<int>[]
                    {
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 4), Value = 1 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 5), Value = 1 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 6), Value = 1 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 7), Value = 1 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 8), Value = 1 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 9), Value = 1 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 10), Value = 1 },
                    });

                var expectedResult = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 1, 4), Value = 1 }
                };
                var result = signalsWebService.GetCoarseData(5, Dto.Granularity.Week, timestamp, timestamp);

                AssertDataDtoEquals(result, expectedResult);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingCoarseData_CorrectDataIsReturned()
            {
                var signal = new Signal()
                {
                    Id = 6,
                    Granularity = Granularity.Day,
                    DataType = DataType.Double,
                    Path = Path.FromString("signal/path")
                };
                GivenASignal(signal);
                var fromTimestamp = new DateTime(2016, 1, 11);
                var toTimestamp = new DateTime(2016, 1, 25);
                var midTimestamp = new DateTime(2016, 1, 18);

                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<double>(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                    s.Granularity == signal.Granularity &&
                    s.DataType == signal.DataType &&
                    s.Path.ToString().Equals(signal.Path.ToString())), fromTimestamp, toTimestamp)).Returns(new Datum<double>[]
                    {
                        new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 1, 11), Value = 5d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 12), Value = 5d },
                        new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 1, 13), Value = 5d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 14), Value = 5d },
                        new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 1, 15), Value = 5d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 16), Value = 2d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 17), Value = 1d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 18), Value = 5d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 19), Value = 5d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 20), Value = 5d },
                        new Datum<double>() { Quality = Quality.Bad,  Timestamp = new DateTime(2016, 1, 21), Value = 5d },
                        new Datum<double>() { Quality = Quality.Bad,  Timestamp = new DateTime(2016, 1, 22), Value = 0d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 23), Value = 1d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 24), Value = 0d }
                    });
                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<double>(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                    s.Granularity == signal.Granularity &&
                    s.DataType == signal.DataType &&
                    s.Path.ToString().Equals(signal.Path.ToString())), fromTimestamp, midTimestamp)).Returns(new Datum<double>[]
                    {
                        new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 1, 11), Value = 5d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 12), Value = 5d },
                        new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 1, 13), Value = 5d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 14), Value = 5d },
                        new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 1, 15), Value = 5d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 16), Value = 2d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 17), Value = 1d }
                    });
                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<double>(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                    s.Granularity == signal.Granularity &&
                    s.DataType == signal.DataType &&
                    s.Path.ToString().Equals(signal.Path.ToString())), midTimestamp, toTimestamp)).Returns(new Datum<double>[]
                    {
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 18), Value = 5d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 19), Value = 5d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 20), Value = 5d },
                        new Datum<double>() { Quality = Quality.Bad,  Timestamp = new DateTime(2016, 1, 21), Value = 5d },
                        new Datum<double>() { Quality = Quality.Bad,  Timestamp = new DateTime(2016, 1, 22), Value = 0d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 23), Value = 1d },
                        new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 24), Value = 0d }
                    });

                var expectedResult = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = fromTimestamp, Value = 4d },
                    new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = midTimestamp, Value = 3d },
                };
                var result = signalsWebService.GetCoarseData(6, Dto.Granularity.Week, fromTimestamp, toTimestamp);

                AssertDataDtoEquals(result, expectedResult);
            }

            [TestMethod]
            public void GivenASignalWithNoneQualityMVP_GivenDataWithMissingSamples_WhenGettingCoarseData_CorrectDataIsReturned()
            {
                var signal = new Signal()
                {
                    Id = 8,
                    Granularity = Granularity.Month,
                    DataType = DataType.Decimal,
                    Path = Path.FromString("signal/path")
                };
                GivenASignal(signal);
                var fromTimestamp = new DateTime(2014, 1, 1);
                var toTimestamp = new DateTime(2016, 1, 1);
                var midTimestamp = new DateTime(2015, 1, 1);

                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<decimal>(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                    s.Granularity == signal.Granularity &&
                    s.DataType == signal.DataType &&
                    s.Path.ToString().Equals(signal.Path.ToString())), fromTimestamp, toTimestamp)).Returns(new Datum<decimal>[]
                    {
                        new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2014, 1, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2014, 3, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2014, 4, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2014, 5, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2014, 8, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2014, 9, 1),  Value = 2m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2014, 10, 1), Value = 1m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2014, 12, 1), Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2015, 1, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2015, 2, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Bad,  Timestamp = new DateTime(2015, 5, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Bad,  Timestamp = new DateTime(2015, 6, 1),  Value = 0m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2015, 9, 1),  Value = 1m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2015, 11, 1), Value = 0m }
                    });
                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<decimal>(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                   s.Granularity == signal.Granularity &&
                   s.DataType == signal.DataType &&
                   s.Path.ToString().Equals(signal.Path.ToString())), fromTimestamp, midTimestamp)).Returns(new Datum<decimal>[]
                   {
                        new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2014, 1, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2014, 3, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2014, 4, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2014, 5, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2014, 8, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2014, 9, 1),  Value = 2m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2014, 10, 1), Value = 1m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2014, 12, 1), Value = 5m }
                   });
                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<decimal>(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                   s.Granularity == signal.Granularity &&
                   s.DataType == signal.DataType &&
                   s.Path.ToString().Equals(signal.Path.ToString())), midTimestamp, toTimestamp)).Returns(new Datum<decimal>[]
                   {
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2015, 1, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2015, 2, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Bad,  Timestamp = new DateTime(2015, 5, 1),  Value = 5m },
                        new Datum<decimal>() { Quality = Quality.Bad,  Timestamp = new DateTime(2015, 6, 1),  Value = 0m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2015, 9, 1),  Value = 1m },
                        new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2015, 11, 1), Value = 2m }
                   });

                var expectedResult = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = fromTimestamp, Value = 2.75m },
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = midTimestamp, Value = 1.5m },
                };
                var result = signalsWebService.GetCoarseData(8, Dto.Granularity.Year, fromTimestamp, toTimestamp);

                AssertDataDtoEquals(result, expectedResult);
            }

            [TestMethod]
            public void GivenASignalWithSpecificMVP_GivenDataWithMissingSamples_WhenGettingCoarseData_CorrectDataIsReturned()
            {
                var signal = new Signal()
                {
                    Id = 9,
                    Granularity = Granularity.Day,
                    DataType = DataType.Integer,
                    Path = Path.FromString("signal/path")
                };
                GivenASignal(signal);
                var fromTimestamp = new DateTime(2016, 1, 11);
                var toTimestamp = new DateTime(2016, 1, 25);
                var midTimestamp = new DateTime(2016, 1, 18);
                var specificMVP = new SpecificValueMissingValuePolicyInteger()
                {
                    Quality = Quality.Good,
                    Value = 7
                };

                missingValuePolicyRepositoryMock.SetupSequence(mvprm => mvprm.Get(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                    s.Granularity == signal.Granularity &&
                    s.DataType == signal.DataType &&
                    s.Path.ToString().Equals(signal.Path.ToString())))).Returns(specificMVP).Returns(specificMVP).Returns(specificMVP);
                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<int>(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                    s.Granularity == signal.Granularity &&
                    s.DataType == signal.DataType &&
                    s.Path.ToString().Equals(signal.Path.ToString())), fromTimestamp, toTimestamp)).Returns(new Datum<int>[]
                    {
                        new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 1, 11), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 12), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 13), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 14), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 16), Value = 2 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 17), Value = 1 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 18), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 19), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 20), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 22), Value = 0 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 23), Value = 1 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 24), Value = 0 }
                    });
                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<int>(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                    s.Granularity == signal.Granularity &&
                    s.DataType == signal.DataType &&
                    s.Path.ToString().Equals(signal.Path.ToString())), fromTimestamp, midTimestamp)).Returns(new Datum<int>[]
                    {
                        new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 1, 11), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 12), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 13), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 14), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 16), Value = 0 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 17), Value = 1 }
                    });
                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<int>(It.Is<Domain.Signal>(s => s.Id == signal.Id &&
                    s.Granularity == signal.Granularity &&
                    s.DataType == signal.DataType &&
                    s.Path.ToString().Equals(signal.Path.ToString())), midTimestamp, toTimestamp)).Returns(new Datum<int>[]
                    {
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 18), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 19), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 20), Value = 5 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 22), Value = 0 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 23), Value = 6 },
                        new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 24), Value = 0 }
                    });

                var expectedResult = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = fromTimestamp, Value = 4 },
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = midTimestamp, Value = 4 },
                };
                var result = signalsWebService.GetCoarseData(9, Dto.Granularity.Week, fromTimestamp, toTimestamp);

                AssertDataDtoEquals(result, expectedResult);
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
                    .Returns<Domain.Signal, DateTime, DateTime>((s, from, to) => 
                        data.Where(d => (d.Timestamp >= from && d.Timestamp < to) || (from == to && d.Timestamp == from)));

                signalsDataRepositoryMock
                  .Setup(sdr => sdr.GetDataNewerThan<T>(It.Is<Domain.Signal>(s => s.Id == 5), It.Is<DateTime>(t => true), It.IsAny<int>()))
                  .Returns<Domain.Signal, DateTime, int>((s, from, i) => data.Where(d => d.Timestamp > from));

                signalsDataRepositoryMock
                .Setup(sdr => sdr.GetDataOlderThan<T>(It.Is<Domain.Signal>(s => s.Id == 5), It.Is<DateTime>(t => true), It.IsAny<int>()))
                .Returns<Domain.Signal, DateTime, int>((s, to, i) => data.Where(d => d.Timestamp < to));

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

