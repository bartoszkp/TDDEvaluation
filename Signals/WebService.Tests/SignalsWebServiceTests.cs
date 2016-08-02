using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Dto;
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
                    => passedSignal.DataType == Domain.DataType.Decimal
                        && passedSignal.Granularity == Domain.Granularity.Week
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

            private int dummyInt;
            private object dummyValue;
            [TestMethod]
            public void GivenNoData_WhenSettingData_DoesNotThrowException()
            {
                //arrange
                var dummyData = MakeData(Dto.Quality.Fair, new DateTime(2000, 1, 1), 1.0);
                signalsWebService = new SignalsWebService(null);
                //act
                signalsWebService.SetData(dummyInt,dummyData);
                //assert
            }
            [TestMethod]
           [ExpectedException(typeof(ArgumentException))]
            public void GivenNoId_WhenSettingData_ThrowsException()
            {
                var dummyData = MakeData(Dto.Quality.Fair, new DateTime(2000, 1, 1), 1.0);
                MakeASignalsRepositoryMockWithCorrectId(2, Domain.DataType.Double, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                MakeADataRepositoryMock(2, Domain.DataType.Double, Domain.Granularity.Year, Domain.Path.FromString("x/y"));

                MakeASignalsWebService();


                ////act
                signalsWebService.SetData(dummyInt+5, dummyData);

                ////assert

             
            }

        [TestMethod]
            public void GivenDataDouble_WhenSettingData_SetDataIsCalled()
            {
                //arrange

                var dummyData = MakeData(Dto.Quality.Fair, new DateTime(2000, 1, 1), 1.0);
                MakeASignalsRepositoryMock(2, Domain.DataType.Double, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                MakeADataRepositoryMock(2, Domain.DataType.Double, Domain.Granularity.Year, Domain.Path.FromString("x/y"));

                MakeASignalsWebService();
               

                ////act
                signalsWebService.SetData(dummyInt, dummyData);

                ////assert

                dataRepositoryMock.Verify(sr => sr.SetData<double>(It.IsAny<System.Collections.Generic.ICollection<Datum<double>>>()));
            }
            private Mock<ISignalsDataRepository> dataRepositoryMock;
            private void MakeADataRepositoryMock(int dummyInt, Domain.DataType dataType, Domain.Granularity granularity, Domain.Path path)
            {
                dataRepositoryMock = new Mock<ISignalsDataRepository>();
            

            }
            private void MakeASignalsWebService()
            {
                 
               
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, dataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);

              
            }
            private void MakeASignalsRepositoryMock(int dummyInt, Domain.DataType dataType, Domain.Granularity granularity, Domain.Path path)
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                 .Setup(sr => sr.Get(It.IsAny<int>()))
                 .Returns(SignalWith(id: dummyInt,
                    dataType: dataType,
                    granularity: granularity,
                    path: path));

            }
            private void MakeASignalsRepositoryMockWithCorrectId(int dummyInt, Domain.DataType dataType, Domain.Granularity granularity, Domain.Path path)
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                 .Setup(sr => sr.Get(It.Is<int>(s=>s==dummyInt)))
                 .Returns(SignalWith(id: dummyInt,
                    dataType: dataType,
                    granularity: granularity,
                    path: path));

            }


            [TestMethod]
            public void GivenDataInt_WhenSettingDataWithOneDatum_SetDataIsCalledWithTheSameDatum()
            {
                //arrange

                

                int dummyInt = 2;
                dummyValue = 1;
                
                var dummyData = MakeData(Dto.Quality.Fair, new DateTime(2000, 1, 1), 1);

                MakeASignalsRepositoryMock(2, Domain.DataType.Integer, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                MakeADataRepositoryMock(2, Domain.DataType.Integer, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                MakeASignalsWebService();

          

                ////act
                signalsWebService.SetData(dummyInt, dummyData);

                ////assert

                dataRepositoryMock.Verify(sr => sr.SetData<int>(
                    It.Is<System.Collections.Generic.ICollection<Datum<int>>>(s =>
                    s.ToArray()[0].Value == (int)dummyValue &&
                    s.ToArray()[0].Quality == Domain.Quality.Fair &&
                    s.ToArray()[0].Timestamp == new DateTime(2000, 1, 1)
                )));
            }



            [TestMethod]
            public void GivenDataInt_WhenSettingDataWithOneDatum_SetDataIsCalledSignalIsCorrect()
            {
                //arrange



                int dummyInt = 2;
                dummyValue = 1;

                var dummyData = MakeData(Dto.Quality.Fair, new DateTime(2000, 1, 1), 1);

                MakeASignalsRepositoryMock(2, Domain.DataType.Integer, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                MakeADataRepositoryMock(2, Domain.DataType.Integer, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                MakeASignalsWebService();



                ////act
                signalsWebService.SetData(dummyInt, dummyData);

                ////assert

                dataRepositoryMock.Verify(sr => sr.SetData<int>(
                    It.Is<System.Collections.Generic.ICollection<Datum<int>>>(s =>
                    s.ToArray()[0].Value == (int)dummyValue &&
                    s.ToArray()[0].Quality == Domain.Quality.Fair &&
                    s.ToArray()[0].Timestamp == new DateTime(2000, 1, 1)&&
                    s.ToArray()[0].Signal.Id == dummyInt
                )));
            }


            [TestMethod]
            public void GivenNoData_WhenGettingAData_DoNotThrowException()
            {
                //arrange
                dataRepositoryMock = new Mock<ISignalsDataRepository>();
                var signalsDomainService = new SignalsDomainService(null, dataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
                //act
                var result = signalsWebService.GetData(dummyInt, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
                //assert
            }
            [TestMethod]
            public void GivenNoData_WhenGettingAData_GetDataIsCalled()
            {
                //arrange
                MakeADataRepositoryMock(2, Domain.DataType.Integer, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                MakeASignalsRepositoryMock(2, Domain.DataType.Integer, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, dataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
                //act
                var result = signalsWebService.GetData(dummyInt, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
                //assert
                dataRepositoryMock.Verify(sr => sr.GetData<double>(new Domain.Signal(), new DateTime(), new DateTime()));

            }
            private  Datum[] MakeData(Dto.Quality quality,DateTime date, object value  )
            {
                
                return new Datum[] { new Datum() { Quality = quality, Timestamp = date, Value = value } };
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
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }


            private void GivenASignal(Domain.Signal existingSignal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Id.Value))
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

            private Mock<ISignalsRepository> signalsRepositoryMock;
        }
    }
}