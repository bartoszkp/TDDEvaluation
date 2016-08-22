using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Domain.MissingValuePolicy;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;

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
            public void Get_SignalWithThisPathExist_ReturnThisSignal()
            {
                var signal =  SignalWith(null,Dto.DataType.Boolean, Dto.Granularity.Day, new Dto.Path() { Components = new[] { "x", "y" } });
                GivenASignal(signal.ToDomain<Domain.Signal>());
              
                var result = signalsWebService.Get(new Dto.Path() { Components = new[] { "x", "y" } });

                Assert.AreEqual(CompareSignals(signal, result), true);
            }


            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void SetMissingValuePolicy_SignalWithGivenIdDoesntExist_ThrowException()
            {
                GivenNoSignals();
                signalsWebService.SetMissingValuePolicy(1, new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Double });
            }

            [TestMethod]
            public void SetMissingValuePolicy_SignalWithGivenIdExist_SetCalled()
            {
                GivenASignal(new Signal { Id = 1, DataType = DataType.Integer, Granularity = Granularity.Year, Path = Path.FromString("x/y") });

                signalsWebService.SetMissingValuePolicy(1, new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy() { DataType = Dto.DataType.Boolean });

                missingValuePolicyMock.Verify(x => x.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<bool>>()));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GetMissingValuePolicy_NoSignalsWithGivenId_ThrowException()
            {
                GivenNoSignals();
                signalsWebService.GetMissingValuePolicy(1);
            }

            [TestMethod]
            public void GetMissingValuePolicy_SignalWithGivenIdExist_GetCalled()
            {
                var signal = new Signal { Id = 1, DataType = DataType.Integer, Granularity = Granularity.Year, Path = Path.FromString("x/y") };
                GivenASignal(signal);
                SetupMissingValuePolicyMock(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                signalsWebService.GetMissingValuePolicy(1);

                missingValuePolicyMock.Verify(x => x.Get(It.IsAny<Domain.Signal>()));
            }

            [TestMethod]
            public void GettMissingValuePolicy_NewCreatedSignal_ReturnNull()
            {
                var signal = new Signal { Id = 1, DataType = DataType.Boolean, Granularity = Granularity.Day, Path = Path.FromString("x/y") };
                GivenASignal(signal);
                SetupMissingValuePolicyMock(null);

                var result = signalsWebService.GetMissingValuePolicy(1);

                Assert.AreEqual(null, result);
            }

            [TestMethod]
            public void GetMissingValuePolicy_SignalWithGivenIdExist_ReturnMissingValuePolicy()
            {
                var signal = new Signal { Id = 1, DataType = DataType.Integer, Granularity = Granularity.Year, Path = Path.FromString("x/y") };
                GivenASignal(signal);

                SetupMissingValuePolicyMock(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDecimal() { 
                 Id =1,
                 Quality =Quality.Fair,
                 Signal =signal,
                 Value= 10
                });

                var result = signalsWebService.GetMissingValuePolicy(1);

                Assert.AreEqual(result.Id, 1);
                Assert.AreEqual(result.DataType, Dto.DataType.Boolean);
                Assert.AreEqual(CompareSignals(signal.ToDto<Dto.Signal>(), result.Signal.ToDto<Dto.Signal>()), true);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void SetData_NoSignalsWithGivenId_ThrowException()
            {
                GivenNoSignals();
                SetupSignalsDataRepositoryMock<double>();

                signalsWebService.SetData(1, new Dto.Datum[] { });
            }

            [TestMethod]
            public void SetData_SignalWithGivenIdExist_SetDataCalled()
            {
                GivenASignal(new Signal { Id = 1, DataType = DataType.Double, Granularity = Granularity.Year, Path = Path.FromString("x/y") });
                SetupSignalsDataRepositoryMock<double>();

                signalsWebService.SetData(1, new Dto.Datum[] {
                 new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 }
                });

                signalsDataRepositryMock.Verify(x=>x.SetData(It.IsAny<IEnumerable<Domain.Datum<double>>>()));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GetData_NoSignalWithGivenId_ThrowException()
            {
                GivenNoSignals();
                signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));
            }

            [TestMethod]
            public void GetData_SignalWithGivenIdExist_GetDataCalled()
            {
                var signal = new Signal { Id = 1, DataType = DataType.Double, Granularity = Granularity.Year, Path = Path.FromString("x/y") };
                GivenASignal(signal);
                SetupSignalsDataRepositoryMock<double>();

                signalsWebService.GetData(1, new DateTime(2001, 1, 1), new DateTime(2001, 1, 1));

                signalsDataRepositryMock.Verify(x => x.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));
            }
            [TestMethod]
            public void GetData_SignalWithGivenIdExist_ReturnData()
            {
                var signal = new Signal { Id = 1, DataType = DataType.Double, Granularity = Granularity.Year, Path = Path.FromString("x/y") };
                GivenASignal(signal);

                var datum = new Dto.Datum[] {
                   new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                   new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } };

                signalsDataRepositryMock.Setup(x => x.GetData<double>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(datum.ToArray().ToDomain<IEnumerable<Domain.Datum<double>>>());

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                Assert.IsTrue(CompareDatum(datum,result));
            }

            [TestMethod]
            public void GetData_WithNoneQualityMissingValuePolicy_ReturnsIt()
            {
                var signal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("x/y"));
                GivenASignal(signal);
                SetupMissingValuePolicyMock(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var datum = new Datum<double>[] {
                   new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 1 },
                   new Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = 2 } };
                var expectedDatum = new Dto.Datum[] {
                   new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 2, 1), Value = (double)0 },
                   new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 },
                   new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 4, 1), Value = (double)0 } };

                SetupGetData(datum);

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));

                Assert.IsTrue(CompareDatum(expectedDatum, result));
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingSignalById_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.GetById(1337);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingSignalByPath_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.Get(new Dto.Path() { Components = new[] { "a", "b", "c" } });

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingSignalData_ReturnsItSorted()
            {
                GivenASignal(SignalWith(1, DataType.Double, Granularity.Day, Path.FromString("c/b/a")));
                SetupSignalsDataRepositoryMock<double>();
                
                var datum = new Datum<double>[] {
                         new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 2), Value = 1.5 },
                         new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 1 },
                         new Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 3), Value = 2 }
                };
                var datumSorted = new Dto.Datum[]
                {
                    new Dto.Datum() {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2), Value = (double)1.5 },
                    new Dto.Datum() {Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 3), Value = (double)2 }
                };

                SetupGetData(datum);

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 4));
                
                Assert.IsTrue(CompareDatum(datumSorted, result));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingNewSignal_SetMissingValuePolicyIsCalled()
            {
                GivenNoSignals();

                signalsWebService.Add(SignalWith(null, Dto.DataType.Double, 
                    Dto.Granularity.Month, new Dto.Path() { Components = new[] { "m", "v", "p" } }));

                missingValuePolicyMock.Verify(f => f.Set(It.IsAny<Signal>(), It.IsAny<MissingValuePolicyBase>()), Times.Once);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingNewSignal_DefaultMissingValuePolicyIsSet()
            {
                GivenNoSignals();

                signalsWebService.Add(SignalWith(null, Dto.DataType.Double,
                    Dto.Granularity.Month, new Dto.Path() { Components = new[] { "m", "v", "p" } }));

                missingValuePolicyMock.Verify(f => f.Set(It.IsAny<Signal>(), It.Is<MissingValuePolicyBase>(mvp => mvp is NoneQualityMissingValuePolicy<double>)), Times.Once);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingNewSignal_ReturnsDefaultMissingValuePolicy()
            {
                var id = 1;
                var signal = SignalWith(id, DataType.String, Granularity.Hour, Path.FromString("x/y/z"));
                GivenASignal(signal);
                SetupMissingValuePolicyMock(signal, new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyString());

                var mvp = signalsWebService.GetMissingValuePolicy(id);

                Assert.IsInstanceOfType(mvp, typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
            }


            [TestMethod]
            public void GivenASignal_WhenGettingPathEntryWithTheSignalsPrefix_ReturnsEntryWithSignal()
            {
                GivenSignalsForPathEntry(new Signal[]
                {
                    new Signal() {Id = 1, Path = Domain.Path.FromString("root/s1") }
                }, Domain.Path.FromString("root"));

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "root" } });

                CollectionAssert.AreEqual(new string[] { "root", "s1" }, result.Signals.Single().Path.Components.ToArray());
            }


            [TestMethod]
            public void GivenASignal_WhenGettingPathEntryWithTooShortPrefix_ReturnsEntryWithSubfolder()
            {
                GivenSignalsForPathEntry(new Signal[]
                {
                    new Signal() {Id = 1, Path = Domain.Path.FromString("root/subFolder1/s1") }
                }, Domain.Path.FromString("root"));

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "root" } });

                CollectionAssert.AreEqual(new string[] { "root", "subFolder1" }, result.SubPaths.Single().Components.ToArray());
            }


            [TestMethod]
            public void GivenASignal_WehnGettingPathEntryWithWrongPrefix_ReturnsEmptyPathEntry()
            {
                GivenSignalsForPathEntry(new Signal[]
                {

                }, Domain.Path.FromString("root"));

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "root" } });

                Assert.AreEqual(0, result.Signals.Count());
                Assert.AreEqual(0, result.SubPaths.Count());
            }


            [TestMethod]
            public void GivenManySignals_WhenGettingPathEntryWithTooShortPrefix_ReturnsEntryWithDistinctSubfolders()
            {
                GivenSignalsForPathEntry(new Signal[]
                {
                    new Signal() {Id = 1, Path = Domain.Path.FromString("root/subFolder1/s1") },
                    new Signal() {Id = 2, Path = Domain.Path.FromString("root/subFolder1/s2") }
                }, Domain.Path.FromString("root"));

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "root" } });

                CollectionAssert.AreEqual(new string[] { "root", "subFolder1" }, result.SubPaths.Single().Components.ToArray());
            }


            [TestMethod]
            public void GivenASignalThatIsAFolder_WhenGettingPathEntry_ReturnsSignalAsSignalAndAsFolder()
            {
                GivenSignalsForPathEntry(new Signal[]
                {
                    new Signal() {Id = 1, Path = Domain.Path.FromString("root/s1") },
                    new Signal() {Id = 2, Path = Domain.Path.FromString("root/s1/s2") }
                }, Domain.Path.FromString("root"));

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "root" } });

                CollectionAssert.AreEqual(new string[] { "root", "s1" }, result.SubPaths.Single().Components.ToArray());
                Assert.IsTrue(
                    CompareSignals(
                        new Dto.Signal() { Id = 1, Path = new Dto.Path() { Components = new string[] { "root", "s1" } } },
                        result.Signals.Single()));
            }


            [TestMethod]
            public void GivenASignalWithSpecificValueMissingValuePolicy_WhenGettingData_MissingValuesHaveSpecificValue()
            {
                var signal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("x/y"));
                GivenASignal(signal);
                SetupMissingValuePolicyMock(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                {
                    Quality = Quality.Good,
                    Value = 3.1415
                });

                var datum = new Datum<double>[] {
                   new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = 1 },
                   new Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = 2 } };
                var expectedDatum = new Dto.Datum[] {
                   new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = 3.1415 },
                   new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 },
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 3.1415 } };

                SetupGetData(datum);

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1));

                Assert.IsTrue(CompareDatum(expectedDatum, result));
            }

            private void SetupGetData<T>(IEnumerable<Datum<T>> datum)
            {
                signalsDataRepositryMock
                    .Setup(x => x.GetData<T>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(datum);
            }

            private bool CompareDatum(IEnumerable<Dto.Datum> datum1, IEnumerable<Dto.Datum> datum2)
            {
                if (datum1.Count() != datum2.Count()) return false;

                for (int i = 0; i < datum1.Count(); i++)
                {

                    if (datum1.ToList()[i].Quality != datum2.ToList()[i].Quality || 
                        DateTime.Equals(datum1.ToList()[i].Timestamp, datum2.ToList()[i].Timestamp) == false ||
                        datum1.ToList()[i].Value.ToString() !=datum2.ToList()[i].Value.ToString()
                     )
                        return false;
                }
                return true;
            }

            private bool CompareSignal(Signal sig1, Signal sig2)
            {
                return sig1.Id == sig2.Id &&
                    sig1.Granularity == sig2.Granularity &&
                    sig1.DataType == sig2.DataType &&
                    sig1.Path.ToString() == sig1.Path.ToString(); 
            }

            private void SetupSignalsDataRepositoryMock<T>(){
                signalsDataRepositryMock.Setup(x=>x.SetData<T>(It.IsAny<IEnumerable<Domain.Datum<T>>>()));
                signalsDataRepositryMock.Setup(x => x.GetData<T>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));
            }

            private void SetupMissingValuePolicyMock(Signal signal, MissingValuePolicyBase mvp)
            {
                missingValuePolicyMock
                    .Setup(f => f.Get(It.Is<Signal>(sig => CompareSignal(sig, signal))))
                    .Returns(mvp as MissingValuePolicyBase);
            }

            private void SetupMissingValuePolicyMock(Domain.MissingValuePolicy.MissingValuePolicyBase policy)
            {
                missingValuePolicyMock.Setup(x => x.Get(It.IsAny<Domain.Signal>())).Returns(policy);
            }

            private bool CompareSignals(Dto.Signal signal1, Dto.Signal signal2)
            {
                if (signal1.Id == signal2.Id && signal1.DataType == signal2.DataType
                    && signal1.Granularity == signal2.Granularity && signal1.Path.ToString() == signal2.Path.ToString())
                {
                    return true;
                }
                return false;
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
                missingValuePolicyMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositryMock = new Mock<ISignalsDataRepository>();

                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositryMock.Object, missingValuePolicyMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Signal signal)
            {
                GivenNoSignals();

                try
                {
                    signalsRepositoryMock
                        .Setup(sr => sr.Get(signal.Id.Value))
                        .Returns(signal);
                }
                catch { }
                
                    signalsRepositoryMock.Setup(x => x.Get(Path.FromString("x/y"))).Returns(signal.ToDomain<Domain.Signal>());
                
            }

            private void GivenSignalsForPathEntry(IEnumerable<Signal> signals, Path Prefix)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.GetAllWithPathPrefix(It.Is<Path>(p => p.ToString() == Prefix.ToString())))
                    .Returns(signals);
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyMock;
            private Mock<ISignalsDataRepository> signalsDataRepositryMock;
        }
    }
}