using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Dto;
using System;
using DataAccess.GenericInstantiations;
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
                MakeMocks();

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
                MakeMocks();
                MakeASignalsRepositoryMock(2, Domain.DataType.Double, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                

                MakeASignalsWebService();
               

                ////act
                signalsWebService.SetData(dummyInt, dummyData);

                ////assert

                dataRepositoryMock.Verify(sr => sr.SetData<double>(It.IsAny<System.Collections.Generic.ICollection<Datum<double>>>()));
            }
            private Mock<ISignalsDataRepository> dataRepositoryMock;
            private void MakeMocks()
            {
                dataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();

            }



            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenSignalWithDecimal_WhenSettingDataWithInt_SetDataThrowsException()
            {
                //arrange
                var dummyData = MakeData(Dto.Quality.Fair, new DateTime(2000, 1, 1), 1);
                MakeMocks();
                MakeASignalsRepositoryMock(2, Domain.DataType.Decimal, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                MakeASignalsWebService();

                //act
                signalsWebService.SetData(dummyInt, dummyData);
                //assert

            }
            [TestMethod]
            public void GivenDataInt_WhenSettingDataWithOneDatum_SetDataIsCalledWithTheSameDatum()
            {
                //arrange

                

                int dummyInt = 2;
                dummyValue = 1;
                
                var dummyData = MakeData(Dto.Quality.Fair, new DateTime(2000, 1, 1), 1);
                MakeMocks();
                MakeASignalsRepositoryMock(2, Domain.DataType.Integer, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
               
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
                MakeMocks();

                MakeASignalsRepositoryMock(2, Domain.DataType.Integer, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
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
                
                signalsWebService = new SignalsWebService(null);
                //act
                var result = signalsWebService.GetData(dummyInt, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
                //assert
            }
           
            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignalWithThatId_WhenGettingADataWithThatId_ThrowsArgumentException()
            {

                //arrange
              var anotherId = 5;
                MakeMocks();
                MakeASignalsRepositoryMockWithCorrectId(2, Domain.DataType.Integer, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                MakeASignalsWebService();
                //act

                var result = signalsWebService.GetData(anotherId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                //assert
              

            }
            [TestMethod]
            public void GivenNoData_WhenGettingAData_GetDataIsCalledWithCorrectDatum()
            {
                //arrange
                MakeMocks();
                MakeASignalsRepositoryMock(2, Domain.DataType.Integer, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                MakeASignalsWebService();
                //act

                var result = signalsWebService.GetData(dummyInt, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                //assert
                dataRepositoryMock.Verify(sr => sr.GetData<int>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));

            }
            [TestMethod]
            public void GivenNoData_WhenGettingAData_GetDataIsCalledWithCorrectDatumDouble()
            {
                //arrange
                MakeMocks();
                MakeASignalsRepositoryMock(2, Domain.DataType.Double, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                MakeASignalsWebService();
                //act

                var result = signalsWebService.GetData(dummyInt, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                //assert
                dataRepositoryMock.Verify(sr => sr.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));

            }

            [TestMethod]
            public void GetData_SignalWithGivenIdExist_ReturnDatumOrderedByDate()
            {
                MakeMocks();
                MakeASignalsRepositoryMock(1, Domain.DataType.Double, Domain.Granularity.Year, Domain.Path.FromString("x/y"));
                MakeASignalsWebService();
                var datum = new Dto.Datum[] {
                         new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                         new Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                         new Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } };

                dataRepositoryMock.Setup(x => x.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(datum.ToArray().ToDomain<IEnumerable<Domain.Datum<double>>>());

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
                var expectedResult = datum.OrderBy(x => x.Timestamp);

                Assert.IsTrue(CompareTwoDatum(result, expectedResult));
            }

            [TestMethod]
            public void GetByPath_SignalWithGivenPathDoesntExist_ReturnNull()
            {
                MakeMocks();
                MakeASignalsWebService();
                signalsRepositoryMock.Setup(x => x.Get(Domain.Path.FromString("bad/path"))).Returns((Domain.Signal)null);

                var result = signalsWebService.Get(new Dto.Path() { Components = new[] { "bad", "path" }});

                Assert.AreEqual(null, result);
            }

            [TestMethod]
            public void GetById_SignalWithGivenIdDoesntExist_ReturnNull()
            {
                MakeMocks();
                MakeASignalsWebService();
                signalsRepositoryMock.Setup(x => x.Get(1)).Returns((Domain.Signal)null);

                var result = signalsWebService.GetById(1);

                Assert.AreEqual(null, result);
            }

            [TestMethod]
            public void Add_NoneQualityMissingValuePolicyShouldBeTheDefault_SetMissingPolicyCalled()
            {
                var signal = new Domain.Signal() { Id = 1, DataType = Domain.DataType.Double, Granularity = Domain.Granularity.Month, Path = Domain.Path.FromString("x/y") };
                var noneQualiyMissingValuePolicyDouble = new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble();

                MakeMocks();
                MakeASignalsWebService();
                signalsRepositoryMock.Setup(x => x.Add(It.IsAny<Domain.Signal>())).Returns((Domain.Signal s)=>s);
                missingValuePolicyRepositoryMock.Setup(x => x.Set(signal, noneQualiyMissingValuePolicyDouble));

                signalsWebService.Add(signal.ToDto<Dto.Signal>());

                missingValuePolicyRepositoryMock.Verify(x=>x.Set(signal,noneQualiyMissingValuePolicyDouble));
            }


            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignal_WhenSettingMissingValue_DoThrowArgumentException()
            {
                //arrange
                dummyInt = 2;
                int anotherId = 3;
                MakeASignalsRepositoryMockWithCorrectId(dummyInt, Domain.DataType.Boolean, Domain.Granularity.Day, Domain.Path.FromString("x/y"));
                MakeAMissingValuePolicyRepositoryMock();
                var policy = new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy();
                //act

                signalsWebService.SetMissingValuePolicy(anotherId, policy);
                //assert

            }

            [TestMethod]
            
            public void GivenSignal_WhenSettingMissingValue_SetMissingValuePolicyIsCalled()
            {
                //arrange
                dummyInt = 2;
               
                MakeASignalsRepositoryMockWithCorrectId(dummyInt, Domain.DataType.Boolean, Domain.Granularity.Day, Domain.Path.FromString("x/y"));
                MakeAMissingValuePolicyRepositoryMock();
                var policy = new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy();
                //act

                signalsWebService.SetMissingValuePolicy(dummyInt, policy);
                //assert
                missingValuePolicyRepositoryMock.Verify(sr => sr.Set(It.Is<Domain.Signal>(s=>s.Id==dummyInt), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));


            }

            [TestMethod]
            public void GivenSignal_WhenSettingMissingValue_SetIsCalledWithDecimalValue()
            {
                //arrange
                dummyInt = 2;

                MakeASignalsRepositoryMockWithCorrectId(dummyInt, Domain.DataType.Decimal, Domain.Granularity.Day, Domain.Path.FromString("x/y"));
                MakeAMissingValuePolicyRepositoryMock();
                var policy = new Dto.MissingValuePolicy.FirstOrderMissingValuePolicy() {DataType=Dto.DataType.Decimal };
                //act

                signalsWebService.SetMissingValuePolicy(dummyInt, policy);
                //assert
                missingValuePolicyRepositoryMock.Verify(sr => sr.Set(It.Is<Domain.Signal>(s => s.Id == dummyInt), It.IsAny<Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<Decimal>>()));

            }
            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicy_SetIsCalledWithTheSameMissingValuePolicy()
            {
                //arrange
                dummyInt = 2;

                MakeASignalsRepositoryMockWithCorrectId(dummyInt, Domain.DataType.Decimal, Domain.Granularity.Day, Domain.Path.FromString("x/y"));
                MakeAMissingValuePolicyRepositoryMock();
                var policy = new Dto.MissingValuePolicy.NoneQualityMissingValuePolicy();
                //act

                signalsWebService.SetMissingValuePolicy(dummyInt, policy);
                //assert
                missingValuePolicyRepositoryMock.Verify(sr => sr.Set(It.Is<Domain.Signal>(s => s.Id == dummyInt), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void ThereIsNoSignal_WhenGettingMissingValuePolicy_GetThrowException()
            {
                //arrange
                dummyInt = 2;
                int anotherId = 3;
                MakeASignalsRepositoryMockWithCorrectId(dummyInt, Domain.DataType.Boolean, Domain.Granularity.Day, Domain.Path.FromString("x/y"));
                MakeAMissingValuePolicyRepositoryMock();

                //act
                var result = signalsWebService.GetMissingValuePolicy(anotherId);
                //assert
            }

            [TestMethod]
            public void ThereIsSignal_WhenGettingMissingValuePolicy_GetMissingValuePolicyIsCalled()
            {
                //arrange
                dummyInt = 2;
                MakeMocks();
                MakeASignalsRepositoryMockWithCorrectId(dummyInt, Domain.DataType.Boolean, Domain.Granularity.Day, Domain.Path.FromString("x/y"));
                MakeAMissingValuePolicyRepositoryMock();
                //act

                var result = signalsWebService.GetMissingValuePolicy(dummyInt);
                //assert

                missingValuePolicyRepositoryMock.Verify(sr => sr.Get(It.Is<Domain.Signal>(s => s.Id == dummyInt)));
            }

            [TestMethod]
            public void ThereIsOldSignal_WhenGettingMissingValuePolicy_ReturnNotNull()
            {
                //arrange
                dummyInt = 2;
                MakeMocks();
                MakeASignalsRepositoryMockWithCorrectId(dummyInt, Domain.DataType.Boolean, Domain.Granularity.Day, Domain.Path.FromString("x/y"));
                MakeAMissingValuePolicyRepositoryMock();

                MakeASignalsWebService();

               

             
                //act
                var result=signalsWebService.GetMissingValuePolicy(dummyInt);
                //assert
                Assert.IsNotNull(result);

            }
            [TestMethod]
            public void ThereIsOldSignal_WhenGettingMissingValuePolicy_ReturnCorrectValues()
            {
                //arrange
                dummyInt = 2;
                MakeMocks();
                MakeASignalsRepositoryMockWithCorrectId(dummyInt, Domain.DataType.Boolean, Domain.Granularity.Day, Domain.Path.FromString("x/y"));
                MakeAMissingValuePolicyRepositoryMock();

                MakeASignalsWebService();




                //act
                var result = signalsWebService.GetMissingValuePolicy(dummyInt)as Dto.MissingValuePolicy.FirstOrderMissingValuePolicy;
                //assert
                Assert.IsNotNull(result);
                Assert.AreEqual(Dto.DataType.Boolean, result.DataType);
                Assert.AreEqual(dummyInt, result.Signal.Id);

            }


            [TestMethod]
            public void WhenGettingByPath_FunctionCompile()
            {
                //arrange
                Dto.Path path = ArrangeGetByPath(new[] { "x", "y" });
                MakeMocks();
                SignalsRepositoryMock_GetAllPaths_ReturnsToEachPathASignal();
                MakeASignalsWebService();

                //act
                var result = signalsWebService.Get(path);
                //assert

            }
            private Dto.Path ArrangeGetByPath(string[] s)
            {
                dummyInt = 2;
                MakeMocks();
                SignalsRepositoryMock_GetAllPaths_ReturnsToEachPathASignal();
                return new Dto.Path() { Components = s };
            }


            [TestMethod]
            public void WhenGettingByPath_ReturnsSignal()
            {

                //arrange
                Dto.Path path = ArrangeGetByPath(new[] { "x", "y" });
                SignalsRepositoryMock_GetAllPaths_ReturnsToEachPathASignal();
                MakeASignalsWebService();
                //act
                var result = signalsWebService.Get(path);
                //assert
                Assert.IsInstanceOfType(result, typeof(Dto.Signal));


            }
            [TestMethod]
            public void GivenASignal_WhenGettingSignalByPath_GetIsCalled()
            {
                //arrange
                Dto.Path path = ArrangeGetByPath(new[] { "x", "y" });
                SignalsRepositoryMock_GetAllPaths_ReturnsToEachPathASignal();

                MakeASignalsWebService();

                //act
                var result = signalsWebService.Get(path);
                //assert
                signalsRepositoryMock.Verify(s => s.Get(It.IsAny<Domain.Path>()));


            }
            [TestMethod]
            public void GivenASignal_WhenGettingSignalByPath_GetIsCalledWithCorrectValues()
            {
                //arrange

                Dto.Path path = ArrangeGetByPath(new[] { "x", "y" });


                MakeASignalsWebService();
                //act
                var result = signalsWebService.Get(path);
                //assert
                Assert.AreEqual(Dto.DataType.Boolean, result.DataType);
                Assert.AreEqual(Dto.Granularity.Day, result.Granularity);
                Assert.AreEqual(dummyInt, result.Id);

            }
            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignal_WhenGettingSignalByPath_ThrowsException()
            {

                //arrange
                dummyInt = 2;
                Dto.Path path = MakePathFromString(new[] { "a", "y" });
                MakeMocks();
                SignalsRepositoryMock_GetSpecificPaths_ReturnsToSpecificPathASignal(new[] { "x", "y" });

                MakeASignalsWebService();
                //act
                var result = signalsWebService.Get(path);
                //assert

            }
            [TestMethod]
            public void GivenASignal_WhenGettingSignalBySpecificPath_GetIsCalled()
            {
                //arrange
                dummyInt = 2;
                var correctPath = new[] { "x", "y" };
                Dto.Path path = MakePathFromString(correctPath);
                MakeMocks();
                SignalsRepositoryMock_GetSpecificPaths_ReturnsToSpecificPathASignal(correctPath);

                MakeASignalsWebService();
                //act
                var result = signalsWebService.Get(path);
                //assert
                signalsRepositoryMock.Verify(s => s.Get(It.Is<Domain.Path>(d => d.Components.ToArray().SequenceEqual(new[] { "x", "y" }))));
               
            }

            private void SignalsRepositoryMock_GetAllPaths_ReturnsToEachPathASignal()
            {
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<Domain.Path>()))
                    .Returns(SignalWith(id: dummyInt, dataType: Domain.DataType.Boolean, granularity: Domain.Granularity.Day, path: Domain.Path.FromString("x/y")));
            }
            private void SignalsRepositoryMock_GetSpecificPaths_ReturnsToSpecificPathASignal(string[] correctPath)
            {
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.Is<Domain.Path>(s => s.Components.ToArray().SequenceEqual(correctPath))))
                    .Returns(SignalWith(id: dummyInt, dataType: Domain.DataType.Boolean, granularity: Domain.Granularity.Day, path: Domain.Path.FromString(correctPath.ToString())));
            }
            private static Dto.Path MakePathFromString(string [] str)
            {
                return new Dto.Path() { Components = str };
            }

            private void SignalsRepositoryMock_GetAllPaths_ReturnsToEachSignal()
            {
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.IsAny<Domain.Path>()))
                    .Returns(SignalWith(id: dummyInt, dataType: Domain.DataType.Boolean, granularity: Domain.Granularity.Day, path: Domain.Path.FromString("x/y")));
            }
            private void SignalsRepositoryMock_GetSpecificPath_ReturnsToEachSignal(Domain.Path path)
            {
                signalsRepositoryMock
                    .Setup(sr => sr.Get(It.Is<Domain.Path>(s=>s.Components==path.Components)))
                    .Returns(SignalWith(id: dummyInt, dataType: Domain.DataType.Boolean, granularity: Domain.Granularity.Day, path: Domain.Path.FromString("x/y")));
            }
            private void MakeAMissingValuePolicyRepositoryMock()
            {
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
                missingValuePolicyRepositoryMock.Setup(sr => sr.Get(It.IsAny<Domain.Signal>())).Returns(new FirstOrderMissingValuePolicyDecimal() { Signal= new Domain.Signal() { Id=dummyInt} });
                signalsWebService = new SignalsWebService(signalsDomainService);
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
            private void MakeASignalsWebService()
            {


                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, dataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
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
                 .Setup(sr => sr.Get(It.Is<int>(s => s == dummyInt)))
                 .Returns(SignalWith(id: dummyInt,
                    dataType: dataType,
                    granularity: granularity,
                    path: path));

            }

            private bool CompareTwoDatum(IEnumerable<Dto.Datum> datum1, IEnumerable<Dto.Datum> datum2)
            {
                if (datum1.Count() != datum2.Count()) return false;

                for (int i = 0; i < datum1.Count(); i++)
                {

                    if (datum1.ToList()[i].Quality != datum2.ToList()[i].Quality ||
                        DateTime.Equals(datum1.ToList()[i].Timestamp, datum2.ToList()[i].Timestamp) == false ||
                        datum1.ToList()[i].Value.ToString() != datum2.ToList()[i].Value.ToString()
                     )
                        return false;
                }
                return true;
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
        }
    }
}