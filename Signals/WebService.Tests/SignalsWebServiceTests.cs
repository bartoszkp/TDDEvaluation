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
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

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

                List<Domain.Datum<double>> addedCollection = new List<Datum<double>>(new Datum<double>[] {
                new Datum<double>() { Signal = domainSignal, Quality = Domain.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = (double)5 },
                new Datum<double>() { Signal = domainSignal, Quality = Domain.Quality.Good, Timestamp = new DateTime(2005, 3, 1), Value = (double)7.5, } });

                Setup_SignalsRepoAndSignalsDataRepo(domainSignal);         

                GivenAColletionOfDatums(addedCollection, domainSignal, new DateTime(), new DateTime());

                List<Dto.Datum> result = signalsWebService.GetData(signalId, new DateTime(), new DateTime()).ToList();

                List<Dto.Datum> expectedResult = new List<Dto.Datum>(new Dto.Datum[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = (double)5},
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 3, 1), Value = (double)7.5 } });

                Assert.IsTrue(expectedResult.Count == result.Count);
                Assert.IsTrue(AssertDtoLists(expectedResult, result));
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

            private void GivenAColletionOfDatums(IEnumerable<Datum<double>> data, Domain.Signal signal, DateTime fromIncludedUtc, DateTime toExcludedUtc)
            {
                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetData<double>(signal, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(data);
            }


            //collectionAssert doesn't work couse must convert property Value: (object) to (double)
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
                    if ((double)expectedResult[i].Value != (double)result[i].Value)
                    {
                        isTrue = false;
                        break;
                    }
                }
                return isTrue;
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
        }
    }
}