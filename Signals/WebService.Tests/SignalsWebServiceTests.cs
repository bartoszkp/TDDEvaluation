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

            #region Issue #1 ~ #7

            [TestMethod]
            public void Add_NoneQualityMissingValuePolicyShouldBeTheDefault_SetMissingPolicyCalled()
            {
                var signal = new Domain.Signal() { Id = 1, DataType = Domain.DataType.Integer, Granularity = Domain.Granularity.Month, Path = Domain.Path.FromString("x/y") };
                var signalDto = new Dto.Signal() { DataType = Dto.DataType.Integer, Granularity = Dto.Granularity.Month, Path = new Dto.Path() { Components = new[] { "x", "y" } } };

                MakeMocks();
                MakeASignalsWebService();

                signalsRepositoryMock.Setup(x => x.Add(It.IsAny<Domain.Signal>())).Returns(signal);
                signalsRepositoryMock.Setup(x => x.Get(1)).Returns(signal);
                missingValuePolicyRepositoryMock.Setup(x => x.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>()));

                signalsWebService.Add(signalDto);

                missingValuePolicyRepositoryMock.Verify(x => x.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>()));
            }

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
            public void GetById_SignalWithGivenIdDoesntExist_ReturnNull()
            {
                MakeMocks();
                MakeASignalsWebService();
                signalsRepositoryMock.Setup(x => x.Get(1)).Returns((Domain.Signal)null);

                var result = signalsWebService.GetById(1);

                Assert.AreEqual(null, result);
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

                var datum = new Dto.Datum[] {
                         new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int)1.5 } };

                dataRepositoryMock.Setup(x => x.GetData<int>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(datum.ToArray().ToDomain<IEnumerable<Domain.Datum<int>>>());
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

                var datum = new Dto.Datum[] {
                         new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 } };

                dataRepositoryMock.Setup(x => x.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(datum.ToArray().ToDomain<IEnumerable<Domain.Datum<double>>>());
                //act

                var result = signalsWebService.GetData(dummyInt, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                //assert
                dataRepositoryMock.Verify(sr => sr.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));
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
            public void GetData_DataMissOnePoint_ShouldReturnDataWithFilledMissedPoints()
            {
                var datum = new Dto.Datum[] {
                           new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1 },
                           new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 1 }
                };
                var signal = new Domain.Signal() { Id = 1, DataType = Domain.DataType.Integer, Granularity = Domain.Granularity.Month, Path = Domain.Path.FromString("x/y") };
                SetupDataRepositoryMock<int>(signal, datum);

                dataRepositoryMock.Setup(x => x.GetData<int>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(datum.ToDomain<IEnumerable<Domain.Datum<int>>>());

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                Assert.AreEqual(datum.Count() + 1, result.Count());
            }

            [TestMethod]
            public void GetData_DataMissOnePoint_ShouldReturnDataWithCorrectDatumPoint()
            {
                var datum = new Dto.Datum[] {
                           new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1 },
                           new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 1 }
                };
                var signal = new Domain.Signal() { Id = 1, DataType = Domain.DataType.Integer, Granularity = Domain.Granularity.Month, Path = Domain.Path.FromString("x/y") };
                SetupDataRepositoryMock<int>(signal, datum);

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
                var necessaryPoint = result.ToList()[1];

                Assert.AreEqual(necessaryPoint.Quality, Dto.Quality.None);
                Assert.AreEqual(necessaryPoint.Timestamp, result.ToList()[0].Timestamp.AddMonths(1));
                Assert.AreEqual(necessaryPoint.Value, 0);
            }

            [TestMethod]
            public void GetData_DataMissFourPoints_ShouldReturnDataWithCorrectDatumPoints()
            {
                var datum = new Dto.Datum[] {
                           new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,1,0,0), Value = 1 },
                           new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,3,0,0), Value = 3 },
                           new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,5,0,0), Value = 5 },
                           new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,7,0,0), Value = 7 },
                           new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,9,0,0), Value = 9 }
                };
                var signal = new Domain.Signal() { Id = 1, DataType = Domain.DataType.Integer, Granularity = Domain.Granularity.Hour, Path = Domain.Path.FromString("x/y") };
                SetupDataRepositoryMock<int>(signal, datum);

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));

                for (int i = 1; i < 8; i += 2)
                {
                    Assert.AreNotEqual(result.ToList()[i], null);
                }
            }

            [TestMethod]
            public void GetData_SignalWithGivenIdExist_ReturnDatumOrderedByDate()
            {
                MakeMocks();
                MakeASignalsRepositoryMock(1, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("x/y"));
                MakeASignalsWebService();
                var datum = new Dto.Datum[] {
                         new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                         new Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                         new Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } };

                dataRepositoryMock.Setup(x => x.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(datum.ToArray().ToDomain<IEnumerable<Domain.Datum<double>>>());

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
                var expectedResult = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1)).OrderBy(x => x.Timestamp);

                Assert.IsTrue(CompareTwoDatum(result, expectedResult));
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
            public void GetByPath_SignalWithGivenPathDoesntExist_ReturnNull()
            {
                MakeMocks();
                MakeASignalsWebService();
                signalsRepositoryMock.Setup(x => x.Get(Domain.Path.FromString("bad/path"))).Returns((Domain.Signal)null);

                var result = signalsWebService.Get(new Dto.Path() { Components = new[] { "bad", "path" } });

                Assert.AreEqual(null, result);
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
            #endregion
            #region Issue #13 (Bug: GetData)

            [TestMethod]
            public void GivenASignal_WhenGettingDatum_ReturnsDefault()
            {
                var id = 1;
                SetupGetDataDatum(id, new Datum[] { });

                var result = signalsWebService.GetData(id, new DateTime(2000, 5, 1), new DateTime(2000, 8, 1));

                Assert.IsTrue(CompareDatumArrays(result, new Datum[] {
                        new Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 5, 1), Value = 0},
                        new Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 6, 1), Value = 0},
                        new Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 7, 1), Value = 0} }));
            }

            #endregion
            #region Issue #16 (Bug: GetData)

            [TestMethod]
            public void GivenASignal_WhenGettingDatumWithInvalidRange_ReturnsEmpty()
            {
                var id = 1;
                SetupGetDataDatum(id, new Datum[] { });

                var result = signalsWebService.GetData(id, new DateTime(2000, 5, 1), new DateTime(1999, 1, 1));

                Assert.IsTrue(result.Count() == 0);
            }

            #endregion
            #region Issue #15 (Bug: SetData)

            [TestMethod]
            public void GivenASignal_WhenSettingDataWithEmptyDatum_ExpectNoException()
            {
                var id = 1;
                GivenASignal(SignalWith(id, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("a/b/c")));

                signalsWebService.SetData(id, new Datum[0]);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingDataWithNull_ExpectNoException()
            {
                var id = 1;
                GivenASignal(SignalWith(id, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("a/b/c")));

                signalsWebService.SetData(id, null);
            }

            #endregion
            #region Issue #14 (Bug: SetData)

            [TestMethod]
            public void GivenASignal_WhenSettingDataWithNullValue_ExpectNoException()
            {
                var id = 1;
                MakeMocks();
                GivenASignal(SignalWith(id, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("a/b/c")));

                signalsWebService.SetData(id, new Datum[] { new Datum() { Quality = Dto.Quality.Fair, Timestamp = DateTime.Now, Value = null } });
            }

            #endregion
            #region Issue #11 (Bug: Add)
            
            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.IdNotNullException))]
            public void GivenNoSignals_WhenAddingSignalWithId_ExpectException()
            {
                GivenNoSignals();

                var signal = new Dto.Signal() { Id = 1,
                    DataType = Dto.DataType.Decimal,
                    Granularity = Dto.Granularity.Hour,
                    Path = new Dto.Path() { Components = new[] { "a" } }
                };

                signalsWebService.Add(signal);
            }

            #endregion
            #region Issue #10 (Feature: GetData)

            [TestMethod]
            public void GivenASignal_WhenGettingDatumWithSameTimestamp_ReturnsIt()
            {
                var id = 1;
                var data = SetupGetDataDatum(id);

                var result = signalsWebService.GetData(id, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

                Assert.IsTrue(CompareDatum(data.FirstOrDefault(), result.FirstOrDefault()));
            }

            #endregion
            #region Issue #9 (Feature: SetMissingValuePolicy)

            [TestMethod]
            public void GivenASignalWithSpecificMVP_WhenGettingDatum_ReturnsIt()
            {
                var id = 1;
                var data = SetupGetDataDatum(id, MakeData(Dto.Quality.Fair, new DateTime(2016, 2, 1), (double)3.0));
                var mvp = new SpecificValueMissingValuePolicyDouble() { Quality = Domain.Quality.Fair, Value = 45.0 };
                missingValuePolicyRepositoryMock.Setup(f => f.Get(It.IsAny<Domain.Signal>())).Returns(mvp);

                var result = signalsWebService.GetData(id, new DateTime(2016, 1, 1), new DateTime(2016, 3, 1));

                Assert.IsTrue(CompareDatumArrays(result, new Datum[] {
                    new Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 1, 1), Value = 3},
                    new Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 2, 1), Value = 45} }));
            }

            #endregion
            #region Issue #8 (Feature: GetPathEntry)

            [TestMethod]
            public void GivenNoSignals_WhenGettingPathEntry_FuncIsCalled()
            {
                GivenNoSignals();

                signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "a" } });

                signalsRepositoryMock.Verify(f => f.GetAllWithPathPrefix(It.IsAny<Domain.Path>()), Times.Once);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingPathEntry_ReturnsIt()
            {
                GivenNoSignals();
                signalsRepositoryMock
                    .Setup(f => f.GetAllWithPathPrefix(It.IsAny<Domain.Path>()))
                    .Returns(new[] { SignalWith(1, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("a/b")) });

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "a" } });

                Assert.IsTrue(result.Signals.Count() == 1);
                CollectionAssert.AreEqual(new[] { "a", "b" }, result.Signals.First().Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingPathEntries_ReturnsIt()
            {
                GivenNoSignals();
                var signals = new Domain.Signal[]
                {
                    SignalWith(1, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("a/b")),
                    SignalWith(2, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("a/c"))
                };
                signalsRepositoryMock
                    .Setup(f => f.GetAllWithPathPrefix(It.IsAny<Domain.Path>()))
                    .Returns(signals);

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "a" } });

                Assert.IsTrue(result.Signals.Count() == 2);
                Assert.IsTrue(ComparePathArray(new Dto.Path[]
                {
                    new Dto.Path() { Components = new[] { "a", "b" } },
                    new Dto.Path() { Components = new[] { "a", "c" } }
                }, result.Signals.ToArray().Select(s => s.Path).ToArray()));
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingSubPathEntry_ReturnsIt()
            {
                GivenNoSignals();
                signalsRepositoryMock
                    .Setup(f => f.GetAllWithPathPrefix(It.IsAny<Domain.Path>()))
                    .Returns(new[] { SignalWith(1, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("a/b/c")) });

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "a" } });

                Assert.IsTrue(result.SubPaths.Count() == 1);
                CollectionAssert.AreEqual(new[] { "a", "b" }, result.SubPaths.First().Components.ToArray());
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingSubPathEntries_RemoveDuplicates()
            {
                GivenNoSignals();
                signalsRepositoryMock
                     .Setup(f => f.GetAllWithPathPrefix(It.IsAny<Domain.Path>()))
                     .Returns(new[] 
                        { SignalWith(1, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("a/b/c")),
                          SignalWith(2, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("a/b/d"))
                        });

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "a" } });
                Assert.IsTrue(result.SubPaths.Count() == 1);
                CollectionAssert.AreEqual(new[] { "a", "b" }, result.SubPaths.First().Components.ToArray());
            }

            #endregion

            private Datum[] SetupGetDataDatum(int id, Datum[] datum = null)
            {
                MakeMocks();
                var signal = SignalWith(id, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("a/b"));
                GivenASignal(signal);

                if(datum == null)
                    datum = MakeData(Dto.Quality.Fair, new DateTime(2000, 1, 1), (double)0.0);

                SetupDataRepositoryMock<double>(signal, datum);

                return datum;
            }

            private bool CompareDatumArrays(IEnumerable<Datum> a, Datum[] b)
            {
                if (a.Count() != b.Count()) return false;

                foreach (Datum d in a)
                    if (b.Where(f => CompareDatum(f, d)) == null) return false;

                return true;
            }

            private bool ComparePathArray(Dto.Path[] a, Dto.Path[] b)
            {
                if (a.Count() != b.Count()) return false;

                for(int i=0; i<a.Count(); i++)
                    if (string.Join("/", a[i].Components.ToArray()) != string.Join("/", b[i].Components.ToArray()))
                        return false;

                return true;
            }

            private bool CompareDatum(Datum a, Datum b)
            {
                return a.Value.GetType() == b.Value.GetType() &&
                    a.Value.ToString() == b.Value.ToString() &&
                    a.Quality == b.Quality &&
                    a.Timestamp == b.Timestamp;
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
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, dataRepositoryMock?.Object, missingValuePolicyRepositoryMock?.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Domain.Signal existingSignal)
            {
                GivenNoSignals();
                GetSignalSetup(existingSignal);
            }

            private void GetSignalSetup(Domain.Signal existingSignal)
            {
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

            private void SetupDataRepositoryMock<T>(Domain.Signal signal, Dto.Datum[] datum) {
                MakeMocks();
                MakeASignalsWebService();
                signalsRepositoryMock.Setup(x => x.Get(signal.Id.Value)).Returns(signal);
                dataRepositoryMock.Setup(x => x.GetData<T>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(datum.ToDomain<IEnumerable<Domain.Datum<T>>>());
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;

        }
    }
}