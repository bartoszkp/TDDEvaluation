using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Dto;
using Dto.MissingValuePolicy;
using Domain.MissingValuePolicy;
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
                    => passedSignal.DataType == Domain.DataType.Double
                        && passedSignal.Granularity == Domain.Granularity.Month
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
                    dataType: Domain.DataType.Integer,
                    granularity: Domain.Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.GetById(signalId);

                Assert.AreEqual(signalId, result.Id);
                Assert.AreEqual(Dto.DataType.Integer, result.DataType);
                Assert.AreEqual(Dto.Granularity.Second, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SignalNotExistException))]
            public void GivenNoSignals_WhenSetData_ExpectedException()
            {
                int signalId = 0;

                Setup_SignalsRepo(signalId);        
                
                signalsWebService.SetData(signalId, new Dto.Datum[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (bool)true },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (bool)false },
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (bool)true } });

            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SignalNotExistException))]
            public void GivenNoSingnals_GetDataByIdAndTime_ExpectedException()
            {
                int signalId = 0;

                Setup_SignalsRepo(signalId);

                signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
            }
                      

            [TestMethod]
            public void GivenCollectionOfDatumsAndASignal_GetDataBySignalId_ReturnDatumsCollection()
            {               
                var signalId = 2;

                Domain.Signal domainSignal = new Domain.Signal()
                {
                    Id = signalId,
                    DataType = Domain.DataType.Double,
                    Granularity = Domain.Granularity.Hour,
                    Path = Domain.Path.FromString("root/signal44")
                };

                List<Domain.Datum<object>> addedCollection = new List<Datum<object>>(new Datum<object>[] {
                new Datum<object>() { Signal = domainSignal, Quality = Domain.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = (int)5 },
                new Datum<object>() { Signal = domainSignal, Quality = Domain.Quality.Good, Timestamp = new DateTime(2005, 3, 1), Value = (int)7, } });

                Setup_SignalsRepoAndSignalsDataRepo(domainSignal);         

                GivenAColletionOfDatums(addedCollection, domainSignal, new DateTime(), new DateTime());

                List<Dto.Datum> result = signalsWebService.GetData(signalId, new DateTime(), new DateTime()).ToList();

                List<Dto.Datum> expectedResult = new List<Dto.Datum>(new Dto.Datum[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = (int)5},
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 3, 1), Value = (int)7 } });

                Assert.IsTrue(expectedResult.Count == result.Count);
                Assert.IsTrue(AssertDtoLists(expectedResult, result));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SignalNotExistException))]
            public void GivenNoSignals_SetMissingValuePolicy_ExpectedException()
            {
                int signalId = 5;

                Setup_SignalsRepo(signalId);

                var result = new SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = (double)2.5 };

                signalsWebService.SetMissingValuePolicy(signalId, result);
            }

            [TestMethod]
            public void GivenASignal_SetMissingValuePolicy_CheckIsSetted()
            {
                int signalId = 5;

                Domain.Signal signal = (SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Minute,
                    path: Domain.Path.FromString("root/signal8")));

                Setup_SignalsRepoAndMissingValuePolicyRepo(signal);

                var result = new SpecificValueMissingValuePolicy() { DataType = Dto.DataType.Boolean, Quality = Dto.Quality.Good, Value = (double)1 };

                signalsWebService.SetMissingValuePolicy(signalId, result);

                missingValuePolicyRepoMock.Verify(mvp => mvp.Set(signal, It.IsAny<MissingValuePolicyBase>()));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SignalNotExistException))]
            public void GivenNoSignals_GetMissingValuePolicy_ExpectedException()
            {
                int signalId = 2;

                Setup_SignalsRepo(signalId);

                signalsWebService.GetMissingValuePolicy(signalId);
            }

            [TestMethod]
            public void GivenASignalNotAddedAnyConfiguration_GetMissingValuePolicy_ReturnsNull()
            {
                var signalId = 3;
                Domain.Signal signal = (SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Minute,
                    path: Domain.Path.FromString("root/signal8")));

                Setup_SignalsRepoAndMissingValuePolicyRepo(signal);
             
                var result = signalsWebService.GetMissingValuePolicy(signalId);
                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignalAddedConfiguration_GetMissingValuePolicy_ReturnsIt()
            {
                var signalId = 3;
                double value = 88.5;

                Domain.Signal signal = (SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Minute,
                    path: Domain.Path.FromString("root/signal8")));

                SpecificValueMissingValuePolicyDouble returnedMvp =
                 new SpecificValueMissingValuePolicyDouble()
                 {
                     Quality = Domain.Quality.Fair,
                     Signal = signal,
                     Value = value
                 };

                Setup_SignalsRepoAndMissingValuePolicyRepo_SetupGetMethod(signal, returnedMvp);                            

                var result = (Dto.MissingValuePolicy.SpecificValueMissingValuePolicy)signalsWebService.GetMissingValuePolicy(signalId);

                Assert.AreEqual(value, result.Value);           
                Assert.AreEqual(Dto.Quality.Fair, result.Quality);
                CheckIfDtoSignalAreEqual(
                    result.Signal, signalId, Dto.DataType.Double, Dto.Granularity.Minute, new Dto.Path() { Components = new[] { "root", "signal8" } } );

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

            private void GivenASignal(Domain.Signal signal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);
            }

            private void Setup_SignalsRepo(int signalId)
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signalId)).Returns<Domain.Signal>(null);
            }

            private void Setup_SignalsRepoAndSignalsDataRepo(Domain.Signal signal)
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                
                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenAColletionOfDatums(IEnumerable<Datum<object>> data, Domain.Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
            {
                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetData<object>(signal, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(data);
            }


            //collectionAssert doesn't work couse must convert property Value: (object) to (int)
            private bool AssertDtoLists(List<Datum> expectedResult, List<Datum> result)
            {
                bool isTrue = true;

                for (int i = 0; i < expectedResult.Count; i++)
                {
                    if (expectedResult[i].Quality != result[i].Quality)
                    {
                        isTrue = false;
                        break;
                    }
                    if (expectedResult[i].Timestamp != result[i].Timestamp)
                    {
                        isTrue = false;
                        break;
                    }
                    if ((int)expectedResult[i].Value != (int)result[i].Value)
                    {
                        isTrue = false;
                        break;
                    }
                }
                return isTrue;
            }

            private void Setup_SignalsRepoAndMissingValuePolicyRepo(Domain.Signal signal)
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);

                missingValuePolicyRepoMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepoMock
                    .Setup(mvp => mvp.Set(signal, It.IsAny<MissingValuePolicyBase>()));

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepoMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }


            private void Setup_SignalsRepoAndMissingValuePolicyRepo_SetupGetMethod(Domain.Signal signal, SpecificValueMissingValuePolicyDouble returnedMvp)
            {
                Setup_SignalsRepoAndMissingValuePolicyRepo(signal);

                missingValuePolicyRepoMock.Setup(mvp => mvp.Get(signal))
                .Returns(returnedMvp);
            }

            private void CheckIfDtoSignalAreEqual(Dto.Signal signal, int signalId, Dto.DataType datatype, Dto.Granularity granularity, Dto.Path path)
            {
                Assert.AreEqual(signal.Id, signalId);
                Assert.AreEqual(signal.DataType, datatype);
                Assert.AreEqual(signal.Granularity, granularity);
                CollectionAssert.AreEquivalent(signal.Path.Components.ToArray(), path.Components.ToArray());
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepoMock;
        }
    }
}