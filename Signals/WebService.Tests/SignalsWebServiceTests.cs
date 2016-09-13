using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Domain.MissingValuePolicy;
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
            public void GivenASignal_WhenGettingByPath_ReturnsIt()
            {
                Dto.Path path = new Dto.Path() { Components = new[] { "x", "y" } };
                GivenASignal(SignalWith(
                    id: 1,
                    dataType: DataType.Boolean,
                    granularity: Granularity.Month,
                    path: path.ToDomain<Domain.Path>()
                    ));

                var result = signalsWebService.Get(path.ToDto<Dto.Path>());

                Assert.AreEqual(Dto.DataType.Boolean, result.DataType);
                Assert.AreEqual(Dto.Granularity.Month, result.Granularity);
                CollectionAssert.AreEqual(path.Components.ToArray(), result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenNoSignal_WhenGettingByNonExistentPath_ReturnNull()
            {
                GivenNoSignals();
                var result = signalsWebService.Get(null);
                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenSetMissingValuePolicy_CalledMissingValuePolicyRepository()
            {
                GivenASignal(SignalWith());

                Dto.MissingValuePolicy.MissingValuePolicy mvpDTO = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();

                signalsWebService.SetMissingValuePolicy(1, mvpDTO);

                missingValuePolicyRepositoryMock.Verify(sr => sr.Set(It.IsAny<Domain.Signal>(), It.IsAny<MissingValuePolicyBase>()));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSetMissingValuePolicyWithInvalidId_ReturnException()
            {
                int invalidId = 123;

                GivenASignal(SignalWith());

                signalsWebService.SetMissingValuePolicy(invalidId, new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy());
            }

            [TestMethod]
            public void GivenASignal_WhenGetMissingValuePolicy_ColledmissingValuePolicyRepository()
            {
                GivenASignal(SignalWith());

                signalsWebService.GetMissingValuePolicy(1);

                missingValuePolicyRepositoryMock.Verify(s => s.Get(It.IsAny<Signal>()));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGetMissingValuePolicyWithInvalidID_ReturnException()
            {
                GivenASignal(SignalWith());
                int invalidID = -1;
                var res = signalsWebService.GetMissingValuePolicy(invalidID);
            }

            [TestMethod]
            public void GivenASignal_WhenGetMissingValuePolicy_ReturnCorrectMVP()
            {
                Signal signal = SignalWith();
                GivenASignal(signal);
                int id = 1;

                SpecificValueMissingValuePolicy<bool> MVP = new SpecificValueMissingValuePolicy<bool>()
                {
                    Id = id,
                    Quality = Quality.Fair,
                    Signal = signal,
                    Value = true,
                };

                var policy = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyBoolean()
                {
                    Id = id,
                    Quality = Quality.Fair,
                    Signal = signal,
                    Value = true,
                };
                GivenMVP(id, policy);

                Dto.MissingValuePolicy.MissingValuePolicy resultMVP = signalsWebService.GetMissingValuePolicy(id);

                Assert.AreEqual(MVP.Id.Value, resultMVP.Id.Value);
                Assert.IsTrue(EqualsSignal(MVP.Signal, resultMVP.Signal.ToDomain<Domain.Signal>()));
                Assert.AreEqual(MVP.NativeDataType.Name.ToString(), resultMVP.DataType.ToString());
            }

            [TestMethod]
            public void GivenASignal_WhenSetDataIsInvokedWithVariousTypes_CalledDataRepisitory()
            {
                GivenASignal(SignalWith(DataType.Boolean));
                SetDataCalledVerify<bool>();
                GivenASignal(SignalWith(DataType.Decimal));
                SetDataCalledVerify<decimal>();
                GivenASignal(SignalWith(DataType.Double));
                SetDataCalledVerify<double>();
                GivenASignal(SignalWith(DataType.Integer));
                SetDataCalledVerify<int>();
                GivenASignal(SignalWith(DataType.String));
                SetDataCalledVerify<string>();
            }

            [TestMethod]
            [ExpectedException(typeof(InvalidCastException))]
            public void GivenASignal_WhenSetDataTypesSignalAndDatumAreDiferent_ReturnException()
            {
                GivenASignal(SignalWith(DataType.Integer));
                SetDataCalledVerify<bool>();
            }
            private void SetDataCalledVerify<T>()
            {
                signalsWebService.SetData(1, GetDtoDatum<T>());
                signalsDataRepositoryMock.Verify(s => s.SetData<T>(It.IsAny<IEnumerable<Domain.Datum<T>>>()));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSetDataWithInvalidId_ReturnException()
            {
                int invalidID = 999;
                GivenASignal(SignalWith());
                signalsWebService.SetData(invalidID, GetDtoDatum<bool>());
            }

            [TestMethod]
            public void GivenASignal_WhenGetData_CalledDataRepository()
            {
                GivenASignal(SignalWith(DataType.Double));
                signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
                signalsDataRepositoryMock.Verify(s => s.GetData<double>(It.IsAny<Domain.Signal>(),
                                                                   It.IsAny<DateTime>(),
                                                                   It.IsAny<DateTime>()
                                                                   ));
            }

            [TestMethod]
            public void GivenASignal_WhenGetData_ReturnSortedByDateDatums()
            {
                GivenASignal(SignalWith(DataType.Boolean));
                SetupDataRepository<bool>();
                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1)).ToArray<Dto.Datum>().ToArray();

                bool sortedStatus = true;

                for (int i = 0; i < result.Length - 1; i++)
                {
                    int compareResult = DateTime.Compare(result[i].Timestamp, result[i + 1].Timestamp);
                    if (compareResult > 0) sortedStatus = false;
                }
                Assert.IsTrue(sortedStatus);

                Assert.IsNotNull(result[0].Value);
            }

            [TestMethod]
            public void GivenASignal_WhenGetData_ReturnDatumsWithMissingValues()
            {
                GivenASignal(SignalWith(DataType.Double));
                SetupDataRepository<double>();

                var policy = new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble();
                GivenMVP(1, policy);

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1)).ToArray<Dto.Datum>().ToArray();

                DateTime timeStampMin = result.Min(d => d.Timestamp);
                DateTime timeStampMax = result.Max(d => d.Timestamp);

                DateTime currentTimeSamp = timeStampMin;
                while (currentTimeSamp != timeStampMax)
                {
                    currentTimeSamp = currentTimeSamp.AddMonths(1);
                    var currentDatum = (from x in result
                                        where x.Timestamp == currentTimeSamp
                                        select x).FirstOrDefault();

                    if (currentDatum == null)
                    {
                        Assert.Fail("Result have missing values");
                    }
                }
            }

            [TestMethod]
            public void GivenASignal_WhenAddNewSignal_CalledSetMissingValuePolicy()
            {
                GivenASignal(SignalWith());

                var newSignal = SignalWith(id: null);

                var result = signalsWebService.Add(newSignal);

                missingValuePolicyRepositoryMock.Verify(s => s.Set(It.IsAny<Domain.Signal>(), It.IsAny<MissingValuePolicyBase>()));
            }

            [TestMethod]
            public void GivenASignal_WhenAddNewSignal_SetNoneQualityMissingValuePolicyForDefault()
            {
                GivenASignal(SignalWith());

                var newSignal = SignalWith(id: null);

                var result = signalsWebService.Add(newSignal);

                var mvp = signalsWebService.GetMissingValuePolicy(result.Id.Value);

                Assert.AreEqual("NoneQualityMissingValuePolicy", mvp.GetType().Name);
            }

            [TestMethod]
            public void GivenASignalWithSetNoneQualityMVP_WhenGettingDataWhenThereIsSingleDatumOfSignalInTheMiddleOfTheRange_RetrunedIsFilledArray()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Decimal, Granularity.Month, Path.FromString("root/signal")));

                var policy = new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDecimal();
                GivenMVP(signalId, policy);
                                
                var data = new Datum<decimal>[] 
                {
                    new Datum<decimal>() { Timestamp = new DateTime(2000, 2, 1), Quality = Quality.Fair, Value = 10M }
                };
                SetupDataRepository(signalId, data);

                var expectedArray = new Dto.Datum[] { new Dto.Datum() { Timestamp = new DateTime(2000, 1, 1), Quality = Dto.Quality.None, Value = default(decimal) },
                                                      new Dto.Datum() { Timestamp = new DateTime(2000, 2, 1), Quality = Dto.Quality.Fair, Value = 10M },
                                                      new Dto.Datum() { Timestamp = new DateTime(2000, 3, 1), Quality = Dto.Quality.None, Value = default(decimal) } };

                var resultArray = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)).ToArray();

                for (int i = 0; i < resultArray.Length; i++)
                {
                    Assert.AreEqual(expectedArray[i].Timestamp, resultArray[i].Timestamp);
                    Assert.AreEqual(expectedArray[i].Quality, resultArray[i].Quality);
                    Assert.AreEqual(expectedArray[i].Value, resultArray[i].Value);
                }
            }

            [TestMethod]
            public void GivenASignalWithSetNoneQualityMVP_WhenGettingDataWhenToDatetimeIsLessThanFromDatetime_ReturnedIsBlankListOfData()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Decimal, Granularity.Hour, Path.FromString("root/signal")));

                var policy = new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDecimal();

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(1999, 1, 1));
            }

            [TestMethod]
            public void GivenASignalWithSetNoneQualityMVPAndNoSetData_WhenGettingDataWhenTheGetDataTimeRangeIsCorrect_ReturnedIsNotBlankArray()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Decimal, Granularity.Month, Path.FromString("root/signal")));

                SetupDataRepository(signalId, new Datum<decimal>[] { });

                var policy = new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDecimal();
                GivenMVP(signalId, policy);

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2001, 1, 1));

                Assert.IsFalse(result.ToArray().Length == 0);
            }

            [TestMethod]
            public void GivenASignalWithSetSVMVPAndSetDataWithMissingValues_WhenGettingDataOfTheSignal_ReturnedIsFilledArray()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Decimal, Granularity.Month, Path.FromString("root/signal")));

                var data = new Datum<decimal>[] 
                {
                    new Datum<decimal>() { Timestamp = new DateTime(2000, 2, 1), Quality = Quality.Poor, Value = 10M }
                };
                SetupDataRepository(signalId, data);

                var policy = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDecimal()
                {
                    Quality = Quality.Good,
                    Value = 5M
                };
                GivenMVP(signalId, policy);

                var expectedArray = new Dto.Datum[] { new Dto.Datum() { Timestamp = new DateTime(2000,1,1), Value = 5M, Quality=Dto.Quality.Good},
                                                      new Dto.Datum() { Timestamp = new DateTime(2000,2,1), Value = 10M, Quality=Dto.Quality.Poor},
                                                      new Dto.Datum() { Timestamp = new DateTime(2000,3,1), Value = 5M, Quality=Dto.Quality.Good} };

                var resultArray = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)).ToArray();

                for (int i = 0; i < resultArray.Length; i++)
                {
                    Assert.AreEqual(expectedArray[i].Timestamp, resultArray[i].Timestamp);
                    Assert.AreEqual(expectedArray[i].Quality, resultArray[i].Quality);
                    Assert.AreEqual(expectedArray[i].Value, resultArray[i].Value);
                }

            }

            [TestMethod]
            public void GivenASignalWithSetSVMVPAndNotSetData_WhenGettingDataOfTheSignalByGivingTwoSameDatesAsParameters_ReturnedIsOneDatum()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Decimal, Granularity.Month, Path.FromString("root/signal")));

                SetupDataRepository(signalId, new Datum<decimal>[] { });

                var policy = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDecimal()
                {
                    Quality = Quality.Good,
                    Value = 5M
                };
                GivenMVP(signalId, policy);

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

                Assert.AreEqual(1, result.ToArray().Length);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingAnyPathEntry_ReturnedIsPathEntryWithNoSignalsAndSubpaths()
            {
                GivenNoSignals();

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "any path" } });

                CollectionAssert.AreEqual(new Dto.Signal[] { }, result.Signals.ToArray());
                CollectionAssert.AreEqual(new Dto.Path[] { }, result.SubPaths.ToArray());
            }

            [TestMethod]
            public void GivenOneSignal_WhenGettingPathEntryByPathWhichDoesNotExistInRepository_ReturnedIsPathEntryWithNoSignalsAndSubpaths()
            {
                GivenNoSignals();

                signalsRepositoryMock.Setup(sr => sr.GetAllWithPathPrefix(It.Is<Path>(p => p.Equals("correctPath"))))
                    .Returns(new Signal[] { new Signal() { Id = 1, Path = Path.FromString("correctPath/s1") } });

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "incorrectPath" } });

                CollectionAssert.AreEqual(new Dto.Signal[] { }, result.Signals.ToArray());
                CollectionAssert.AreEqual(new Dto.Path[] { }, result.SubPaths.ToArray());
            }

            [TestMethod]
            public void GivenSignals_WhenGettingPathEntryByPathWhichExistsInRepository_ReturnedIsExpectedPathEntry()
            {
                GivenNoSignals();

                signalsRepositoryMock.Setup(sr => sr.GetAllWithPathPrefix(It.Is<Path>(p => p.Equals(Path.FromString("correctPath")))))
                    .Returns(new Signal[] { new Signal() { Id = 1, Path = Path.FromString("correctPath/s1") }, new Signal() { Id = 2, Path = Path.FromString("correctPath/s2") } });

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "correctPath" } });

                Assert.AreEqual(new Dto.Signal() { Id = 1, Path = new Dto.Path() { Components = new[] { "correctPath", "s1" } } }.Id, result.Signals.ToArray()[0].Id);
                CollectionAssert.AreEqual(new Dto.Signal() { Id = 1, Path = new Dto.Path() { Components = new[] { "correctPath", "s1" } } }.Path.Components.ToArray()
                    , result.Signals.ToArray()[0].Path.Components.ToArray());

                Assert.AreEqual(new Dto.Signal() { Id = 2, Path = new Dto.Path() { Components = new[] { "correctPath", "s2" } } }.Id, result.Signals.ToArray()[1].Id);
                CollectionAssert.AreEqual(new Dto.Signal() { Id = 2, Path = new Dto.Path() { Components = new[] { "correctPath", "s2" } } }.Path.Components.ToArray()
                    , result.Signals.ToArray()[1].Path.Components.ToArray());

                CollectionAssert.AreEqual(new Path[] { }, result.SubPaths.ToArray());
            }

            [TestMethod]
            public void GivenSignals_WhenGettingPathEntryByPathWhichContainsDelimitersAndExistsInRepository_ReturnedIsExpectedPathEntry()
            {
                GivenNoSignals();

                signalsRepositoryMock.Setup(sr => sr.GetAllWithPathPrefix(It.Is<Path>(p => p.Equals(Path.FromString("correctPath/folder")))))
                    .Returns(new Signal[] { new Signal() { Id = 1, Path = Path.FromString("correctPath/folder/s1") }, new Signal() { Id = 2, Path = Path.FromString("correctPath/folder/s2") } });

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "correctPath", "folder" } });

                Assert.AreEqual(new Dto.Signal() { Id = 1, Path = new Dto.Path() { Components = new[] { "correctPath", "folder", "s1" } } }.Id, result.Signals.ToArray()[0].Id);
                CollectionAssert.AreEqual(new Dto.Signal() { Id = 1, Path = new Dto.Path() { Components = new[] { "correctPath", "folder", "s1" } } }.Path.Components.ToArray()
                    , result.Signals.ToArray()[0].Path.Components.ToArray());

                Assert.AreEqual(new Dto.Signal() { Id = 2, Path = new Dto.Path() { Components = new[] { "correctPath", "folder", "s2" } } }.Id, result.Signals.ToArray()[1].Id);
                CollectionAssert.AreEqual(new Dto.Signal() { Id = 2, Path = new Dto.Path() { Components = new[] { "correctPath", "folder", "s2" } } }.Path.Components.ToArray()
                    , result.Signals.ToArray()[1].Path.Components.ToArray());

                CollectionAssert.AreEqual(new Path[] { }, result.SubPaths.ToArray());
            }

            [TestMethod]
            public void GivenSignals_WhenGettingPathEntryWhichEqualsOneOfSignalsPath_ReturnedIsExpectedPathEntry()
            {
                GivenNoSignals();

                signalsRepositoryMock.Setup(sr => sr.GetAllWithPathPrefix(It.Is<Path>(p => p.Equals(Path.FromString("correctPath/s1")))))
                    .Returns(new Signal[] { new Signal() { Id = 1, Path = Path.FromString("correctPath/s1") }, new Signal() { Id = 2, Path = Path.FromString("correctPath/s1/s2") } });

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "correctPath", "s1" } });

                Assert.AreEqual(2, result.Signals.ToArray()[0].Id);
                CollectionAssert.AreEqual(new string[] { "correctPath", "s1", "s2" }, result.Signals.ToArray()[0].Path.Components.ToArray());

                CollectionAssert.AreEqual(new Path[] { }, result.SubPaths.ToArray());
            }

            [TestMethod]
            public void GivenASignalAndDataAndZeroOrderMVP_WhenGettingData_PolicyIsAppliedCorrectly()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Boolean, Granularity.Month, Path.FromString("root/signal")));

                var policy = new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyBoolean();
                GivenMVP(signalId, policy);

                SetupDataRepository<bool>();

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 6, 1));

                Assert.AreEqual(5, result.Count()); //missing are filled
                Assert.AreEqual(5, result.Select(d => d.Timestamp).Distinct().Count()); //timestamp is not copied

                foreach (var value in result.Select(d => d.Value))
                    Assert.AreEqual(default(bool), value);

                Assert.AreEqual(3, result.Where(d => d.Quality == Dto.Quality.Fair).Count()); //Fair is replicated

                Assert.AreEqual(Dto.Quality.None, result.First().Quality); //When no previous value, default is applied
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.TimestampIncorrectException))]
            public void GivenASignal_WhenSettingDataWithIncorrectTimestampSeconds_ThrowsException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Boolean, Granularity.Second, Path.FromString("root/signal")));

                signalsWebService.SetData(1, new[] { new Dto.Datum { Timestamp = new DateTime(2000, 1, 1, 0, 0, 0, 1) } });
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.TimestampIncorrectException))]
            public void GivenASignal_WhenSettingDataWithIncorrectTimestampMinutes_ThrowsException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Boolean, Granularity.Minute, Path.FromString("root/signal")));

                signalsWebService.SetData(1, new[] { new Dto.Datum { Timestamp = new DateTime(2000, 1, 1, 0, 0, 1, 0) } });
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.TimestampIncorrectException))]
            public void GivenASignal_WhenSettingDataWithIncorrectTimestampHours_ThrowsException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Boolean, Granularity.Hour, Path.FromString("root/signal")));

                signalsWebService.SetData(1, new[] { new Dto.Datum { Timestamp = new DateTime(2000, 1, 1, 0, 1, 0, 0) } });
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.TimestampIncorrectException))]
            public void GivenASignal_WhenSettingDataWithIncorrectTimestampDays_ThrowsException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Boolean, Granularity.Day, Path.FromString("root/signal")));

                signalsWebService.SetData(1, new[] { new Dto.Datum { Timestamp = new DateTime(2000, 1, 1, 1, 0, 0, 0) } });
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.TimestampIncorrectException))]
            public void GivenASignal_WhenSettingDataWithIncorrectTimestampWeeks_ThrowsException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Boolean, Granularity.Week, Path.FromString("root/signal")));

                signalsWebService.SetData(1, new[] { new Dto.Datum { Timestamp = new DateTime(2016, 8, 23) } });
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.TimestampIncorrectException))]
            public void GivenASignal_WhenSettingDataWithIncorrectTimestampMonths_ThrowsException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Boolean, Granularity.Month, Path.FromString("root/signal")));

                signalsWebService.SetData(1, new[] { new Dto.Datum { Timestamp = new DateTime(2000, 1, 2) } });
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.TimestampIncorrectException))]
            public void GivenASignal_WhenSettingDataWithIncorrectTimestampYears_ThrowsException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Boolean, Granularity.Year, Path.FromString("root/signal")));

                signalsWebService.SetData(1, new[] { new Dto.Datum { Timestamp = new DateTime(2000, 2, 1) } });
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.TimestampIncorrectException))]
            public void GivenASignal_WhenGettingDataWithIncorrectFromIncluded_ThrowsException()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Boolean, Granularity.Month, Path.FromString("root/signal")));

                signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 1, 0), new DateTime(2000, 2, 1));
            }

            [TestMethod]
            public void GivenASignalAndDataWithZOMVP_WhenGettingDataUsingDatesAfterTheDatumsLastTimestamp_ReturnsDatumsAccordingToZOMVP()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Day, Path.FromString("root/signal")));

                var addedDate = new DateTime(2000, 1, 1);
                var data = new Datum<double>[]
                {
                    new Datum<double> {Quality = Quality.Fair, Timestamp = addedDate, Value = 1.0}
                };

                SetupDataRepository(signalId, data);

                var policy = new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble();
                GivenMVP(signalId, policy);

                var fromDate = new DateTime(2000, 1, 3);
                SetupGetOlderData<double>(signalId, fromDate, data);

                var result = signalsWebService.GetData(signalId, fromDate, fromDate.AddDays(2)).ToArray();

                const int expectedNumberOfResults = 2;
                Assert.AreEqual(expectedNumberOfResults, result.Length);
                Assert.AreEqual(new DateTime(2000, 1, 3), result[0].Timestamp);
                Assert.AreEqual(new DateTime(2000, 1, 4), result[1].Timestamp);
                for(int i = 0; i < expectedNumberOfResults; ++i)
                {
                    Assert.AreEqual(data[0].Value, result[i].Value);
                    Assert.AreEqual(data[0].Quality.ToDto<Dto.Quality>(), result[i].Quality);
                }                
            }
            
            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SignalNotFoundException))]
            public void GivenNoSignals_WhenDeletingSignal_SignalNotFoundExceptionIsThrown()
            {
                GivenNoSignals();

                signalsWebService.Delete(99);
            }

            [TestMethod]
            public void GivenASignal_WhenDeletingSignal_SignalsRepositoryDeleteIsCalled()
            {
                int signalId = 1;
                var signal = SignalWith(signalId, DataType.Double, Granularity.Day, Path.FromString("root/signal"));
                GivenASignal(signal);
                
                signalsWebService.Delete(signalId);

                signalsRepositoryMock.Verify(s => s.Delete(signal));
            }

            [TestMethod]
            public void GivenASignalWithDataAndFOMVP_WhenGettingDataFromBeforeAndAfterTheirTimestamps_ReturnsDataAccordingToFOMVP()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Month, Path.FromString("root/signal")));

                var data = new Datum<double>[]
                {
                    new Datum<double> {Quality = Quality.Fair, Timestamp = new DateTime(2000,2,1), Value = 1.0},
                    new Datum<double> {Quality = Quality.Fair, Timestamp = new DateTime(2000,4,1), Value = 3.0},
                    new Datum<double> {Quality = Quality.Fair, Timestamp = new DateTime(2000,5,1), Value = 5.0}
                };

                SetupDataRepository(signalId, data);

                var policy = new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDouble();
                GivenMVP(signalId, policy);

                var fromDate = new DateTime(2000, 1, 1);
                var toDate = new DateTime(2000, 7, 1);
                SetupGetOlderData<double>(signalId, fromDate, new Datum<double>[] { });
                SetupGetNewerData<double>(signalId, toDate);

                var expectedResults = new Dto.Datum[] {
                    new Dto.Datum() {Quality = Dto.Quality.None, Value = default(double)},
                    data[0].ToDto<Dto.Datum>(),
                    new Dto.Datum() {Quality = data[1].Quality.ToDto<Dto.Quality>(), Value = (data[0].Value+data[1].Value)/2},
                    data[1].ToDto<Dto.Datum>(),
                    data[2].ToDto<Dto.Datum>(),
                    new Dto.Datum() {Quality = Dto.Quality.None, Value = default(double)}
                };                

                var result = signalsWebService.GetData(signalId, fromDate, toDate).ToArray();

                const int expectedNumberOfResults = 6;
                Assert.AreEqual(expectedNumberOfResults, result.Length);
                for(int i = 0; i < expectedNumberOfResults; ++i)
                {
                    var date = fromDate.AddMonths(i);
                    Assert.AreEqual(date, result[i].Timestamp);
                    Assert.AreEqual(expectedResults[i].Quality, result[i].Quality);
                    Assert.AreEqual(expectedResults[i].Value, result[i].Value);
                }

            }

            [TestMethod]
            public void GivenASignalWithDataAndFOMVP_WhenGettingDataUsingDatesBetweenExistingTimestamps_ReturnsInterpolatedDatums()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Month, Path.FromString("root/signal")));

                var data = new Datum<double>[]
                {
                    new Datum<double> {Quality = Quality.Fair, Timestamp = new DateTime(2000,2,1), Value = 1.0},
                    new Datum<double> {Quality = Quality.Fair, Timestamp = new DateTime(2000,5,1), Value = 5.0}
                };

                SetupDataRepository(signalId, new Datum<double>[] { });

                var policy = new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDouble();
                GivenMVP(signalId, policy);

                var fromDate = new DateTime(2000, 3, 1);
                var toDate = new DateTime(2000, 4, 1);
                SetupGetOlderData<double>(signalId, fromDate, new Datum<double>[] { data[0] });
                SetupGetNewerData<double>(signalId, toDate, new Datum<double>[] { data[1] });

                var delta = (data[1].Value - data[0].Value) / 3;
                var expectedResults = new Dto.Datum[] {
                    new Dto.Datum() {Quality = data[1].Quality.ToDto<Dto.Quality>(), Value = data[0].Value + delta}
                };

                var result = signalsWebService.GetData(signalId, fromDate, toDate).ToArray();

                const int expectedNumberOfResults = 1;
                Assert.AreEqual(expectedNumberOfResults, result.Length);
                Assert.AreEqual(fromDate, result[0].Timestamp);
                Assert.AreEqual(expectedResults[0].Quality, result[0].Quality);
                Assert.AreEqual(Math.Round(Convert  .ToDouble(expectedResults[0].Value),4), result[0].Value);
                
            }

            [TestMethod]
            public void GivenASignalAndDataWithFOMVP_WhenGettingDataUsingTheSameDateBetweenExistingTimestamps_ReturnsOneInterpolatedDatum()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Month, Path.FromString("root/signal")));

                var data = new Datum<double>[]
                {
                    new Datum<double> {Quality = Quality.Fair, Timestamp = new DateTime(2000,2,1), Value = 1.0},
                    new Datum<double> {Quality = Quality.Fair, Timestamp = new DateTime(2000,5,1), Value = 5.0}
                };

                SetupDataRepository(signalId, new Datum<double>[] { });

                var policy = new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDouble();
                GivenMVP(signalId, policy);

                var fromDate = new DateTime(2000, 3, 1);
                var toDate = new DateTime(2000, 3, 1);
                SetupGetOlderData<double>(signalId, fromDate, new Datum<double>[] { data[0] });
                SetupGetNewerData<double>(signalId, toDate, new Datum<double>[] { data[1] });

                var delta = (data[1].Value - data[0].Value) / 3;
                var expectedResults = new Dto.Datum[] {
                    new Dto.Datum() {Quality = data[1].Quality.ToDto<Dto.Quality>(), Value = data[0].Value + delta}
                };

                var result = signalsWebService.GetData(signalId, fromDate, toDate).ToArray();

                const int expectedNumberOfResults = 1;
                Assert.AreEqual(expectedNumberOfResults, result.Length);
                Assert.AreEqual(fromDate, result[0].Timestamp);
                Assert.AreEqual(expectedResults[0].Quality, result[0].Quality);
                Assert.AreEqual(Math.Round(Convert.ToDouble(expectedResults[0].Value),4), result[0].Value);
            }

            [TestMethod]
            public void GivenASignalAndDataWithFOMVP_WhenGettingDataUsingTheSameDateThatsOutsideOfExistingTimestampBounds_ReturnsOneDatumOfNoneQuality()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, DataType.Double, Granularity.Month, Path.FromString("root/signal")));

                var data = new Datum<double>[]
                {
                    new Datum<double> {Quality = Quality.Fair, Timestamp = new DateTime(2000,2,1), Value = 1.0},
                    new Datum<double> {Quality = Quality.Fair, Timestamp = new DateTime(2000,5,1), Value = 5.0}
                };

                SetupDataRepository(signalId, new Datum<double>[] { });

                var policy = new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDouble();
                GivenMVP(signalId, policy);

                var fromDate = new DateTime(2000, 1, 1);
                var toDate = new DateTime(2000, 1, 1);
                SetupGetOlderData<double>(signalId, fromDate);
                SetupGetNewerData<double>(signalId, toDate, new Datum<double>[] { data[0] });
                
                var expectedResults = new Dto.Datum[] {
                    new Dto.Datum() {Quality = Dto.Quality.None, Value = default(double)}
                };

                var result = signalsWebService.GetData(signalId, fromDate, toDate).ToArray();

                const int expectedNumberOfResults = 1;
                Assert.AreEqual(expectedNumberOfResults, result.Length);
                Assert.AreEqual(fromDate, result[0].Timestamp);
                Assert.AreEqual(expectedResults[0].Quality, result[0].Quality);
                Assert.AreEqual(expectedResults[0].Value, result[0].Value);
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.ShadowMissingValuePolicyException))]
            public void GivenASignal_WhenSettingShadowMVPWithIncorrectSignal_ThrowException()
            {
                int dummyId = 1;
                var newSignal = new Signal()
                {
                    Id = dummyId,
                    DataType = DataType.Double,
                    Granularity = Granularity.Day,
                    Path = Path.FromString("shadow/signal")
                };

                var policy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() {
                    Id = dummyId,
                    DataType = Dto.DataType.Double,
                    ShadowSignal = new Dto.Signal()
                    {
                        Id = dummyId,
                        DataType = Dto.DataType.Decimal,
                        Granularity = Dto.Granularity.Day,
                        Path = new Dto.Path() { Components = new[] {"shadow","siangl"}}
                    }
                };


                GivenASignal(newSignal);
                signalsWebService.SetMissingValuePolicy(dummyId, policy);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingShadowMVPWithCorrectSignal_SetMissingValuePolicyIsCalled()
            {
                int dummyId = 1;
                var newSignal = new Signal()
                {
                    Id = dummyId,
                    DataType = DataType.Double,
                    Granularity = Granularity.Day,
                    Path = Path.FromString("shadow/signal")
                };

                var policy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy()
                {
                    Id = dummyId,
                    DataType = Dto.DataType.Double,
                    ShadowSignal = new Dto.Signal()
                    {
                        Id = dummyId,
                        DataType = Dto.DataType.Double,
                        Granularity = Dto.Granularity.Day,
                        Path = new Dto.Path() { Components = new[] { "shadow", "signal" } }
                    }
                };

                GivenASignal(newSignal);
                signalsWebService.SetMissingValuePolicy(dummyId, policy);
                missingValuePolicyRepositoryMock.Verify(s => s.Set(newSignal, It.IsAny<ShadowMissingValuePolicy<double>>()));
            }

            [TestMethod]
            public void GivenASignal_HavingSignalWithShadowMVP_WithDateFromEqualsDateTo_WhenGettingData_RetursIt()
            {
                int dummyId = 1;
                var newSignal = new Signal()
                {
                    Id = dummyId,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("some/signal")
                };

                var shadowSignal = new Signal()
                {
                    Id = 2,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("shadow/signal")
                };

                var signalDatum = new Datum<bool>[]
                {
                    new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = true },
                    new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = false },
                    new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = true }
                };

                var shadowSignalDatum = new Datum<bool>[]
                {
                    new Datum<bool>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = true }
                };

                GivenASignal(newSignal);

                missingValuePolicyRepositoryMock
                    .Setup(s => s.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.ShadowMissingValuePolicyBoolean() {ShadowSignal = shadowSignal });

                signalsDataRepositoryMock
                    .Setup(s => s.GetData<bool>(It.Is<Domain.Signal>((i => i.Id == 1)), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(signalDatum);

                signalsDataRepositoryMock
                    .Setup(s => s.GetData<bool>(It.Is<Domain.Signal>((i => i.Id == 2)), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(shadowSignalDatum);

                var result = signalsWebService.GetData(dummyId, new DateTime(2000, 3, 1), new DateTime(2000, 3, 1));

                Assert.AreEqual(true, result.First().Value);
                Assert.AreEqual(new DateTime(2000, 3, 1), result.First().Timestamp);
                Assert.AreEqual(Dto.Quality.Fair, result.First().Quality);
            }

            [TestMethod]
            public void GivenASignal_HavingSignalWithShadowMVP_WithDateFromEqualsDateTo_WhenGettingData_RetursDefaultDatum()
            {
                int dummyId = 1;
                var newSignal = new Signal()
                {
                    Id = dummyId,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("some/signal")
                };

                var shadowSignal = new Signal()
                {
                    Id = 2,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("shadow/signal")
                };

                var signalDatum = new Datum<bool>[]
                {
                    new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = true },
                    new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = false },
                    new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 8, 1), Value = true }
                };

                var shadowSignalDatum = new Datum<bool>[]
                {
                    new Datum<bool>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = true }
                };

                GivenASignal(newSignal);

                missingValuePolicyRepositoryMock
                    .Setup(s => s.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.ShadowMissingValuePolicyBoolean() { ShadowSignal = shadowSignal });

                signalsDataRepositoryMock
                    .Setup(s => s.GetData<bool>(It.Is<Domain.Signal>((i => i.Id == 1)), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(signalDatum);

                signalsDataRepositoryMock
                    .Setup(s => s.GetData<bool>(It.Is<Domain.Signal>((i => i.Id == 2)), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new List<Datum<bool>>());

                var result = signalsWebService.GetData(dummyId, new DateTime(2000, 3, 1), new DateTime(2000, 3, 1));

                Assert.AreEqual(false, result.First().Value);
                Assert.AreEqual(new DateTime(2000, 3, 1), result.First().Timestamp);
                Assert.AreEqual(Dto.Quality.None, result.First().Quality);
            }

            [TestMethod]
            public void GivenASignal_GetDataWithFirstOrderMVP_ReturnsCorrectlyFillsData()
            {
                int dummyId = 1;
                var newSignal = new Signal()
                {
                    Id = dummyId,
                    DataType = DataType.Integer,
                    Granularity = Granularity.Day,
                    Path = Path.FromString("some/signal/firstOrderTest")
                };

                var signalDatum = new Datum<int>[]
                {
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 2), Value = 10 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 6), Value = 30 }
                };

                var older = new Datum<int>[]
                {
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 2), Value = 10 }
                };

                var newer = new Datum<int>[]
                {
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 6), Value = 30 }
                };

                GivenASignal(newSignal);

                missingValuePolicyRepositoryMock
                    .Setup(s => s.Get(It.Is<Domain.Signal>((i => i.Id == dummyId))))
                    .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyInteger());

                signalsDataRepositoryMock
                    .Setup(s => s.GetData<int>(It.Is<Domain.Signal>((i => i.Id == dummyId)), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(signalDatum);

                signalsDataRepositoryMock.Setup(a => a.GetDataOlderThan<int>(It.Is<Domain.Signal>((i => i.Id == dummyId)), new DateTime(2000, 1, 1), 1)).Returns(new List<Datum<int>>());
                signalsDataRepositoryMock.Setup(a => a.GetDataNewerThan<int>(It.Is<Domain.Signal>((i => i.Id == dummyId)), new DateTime(2000, 1, 5), 1)).Returns(newer);

                var result = signalsWebService.GetData(dummyId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 5));

                Assert.AreEqual(4, result.Count());
                Assert.IsTrue(new DateTime(2000, 1, 1) == result.ElementAt(0).Timestamp && 0 == (int)result.ElementAt(0).Value && Dto.Quality.None == result.ElementAt(0).Quality);
                Assert.IsTrue(new DateTime(2000, 1, 2) == result.ElementAt(1).Timestamp && 10 == (int)result.ElementAt(1).Value && Dto.Quality.Good == result.ElementAt(1).Quality);
                Assert.IsTrue(new DateTime(2000, 1, 3) == result.ElementAt(2).Timestamp && 15 == (int)result.ElementAt(2).Value && Dto.Quality.Good == result.ElementAt(2).Quality);
                Assert.IsTrue(new DateTime(2000, 1, 4) == result.ElementAt(3).Timestamp && 20 == (int)result.ElementAt(3).Value && Dto.Quality.Good == result.ElementAt(3).Quality);
            }


            [TestMethod]
            public void GivenASignal_GetDataWithFirstOrderMVP_WhenDateNotSort_ReturnsCorrectlyFillsData()
            {
                int dummyId = 1;
                var newSignal = new Signal()
                {
                    Id = dummyId,
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("some/signal/firstOrderBugTest")
                };

                var signalDatum = new Datum<double>[]
                {
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 7, 1), Value = (double)2.5 },
                    new Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 }
                };

                var older = new Datum<double>[]
                {
                    new Datum<double>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 }
                };

                var newer = new Datum<double>[]
                {
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 7, 1), Value = (double)2.5 }
                };

                GivenASignal(newSignal);

                missingValuePolicyRepositoryMock
                    .Setup(s => s.Get(It.Is<Domain.Signal>((i => i.Id == dummyId))))
                    .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDouble());

                signalsDataRepositoryMock
                    .Setup(s => s.GetData<double>(It.Is<Domain.Signal>((i => i.Id == dummyId)), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(signalDatum);

                signalsDataRepositoryMock.Setup(a => a.GetDataOlderThan<double>(It.Is<Domain.Signal>((i => i.Id == dummyId)), new DateTime(2000, 2, 1), 1)).Returns(new List<Datum<double>>());
                signalsDataRepositoryMock.Setup(a => a.GetDataNewerThan<double>(It.Is<Domain.Signal>((i => i.Id == dummyId)), new DateTime(2000, 7, 1), 1)).Returns(newer);

                var result = signalsWebService.GetData(dummyId, new DateTime(2000, 1, 1), new DateTime(2000, 6, 1));

                Assert.AreEqual(5, result.Count());
                Assert.AreEqual((double)0,   result.ElementAt(0).Value);
                Assert.AreEqual((double)1.5, result.ElementAt(1).Value);
                Assert.AreEqual((double)1.7, result.ElementAt(2).Value);
                Assert.AreEqual((double)1.9, result.ElementAt(3).Value);
                Assert.AreEqual((double)2.1, result.ElementAt(4).Value);
            }







            private void SetupDataRepository<T>(int signalId = 1, IEnumerable<Datum<T>> data = null)
            {
                if (data == null)
                    data = GetDomainDatum<T>();
                signalsDataRepositoryMock
                    .Setup(s => s.GetData<T>(It.Is<Domain.Signal>(sig => sig.Id == signalId), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(data);
            }
            private void SetupGetOlderData<T>(int signalId, DateTime fromDate, IEnumerable<Datum<T>> data = null)
            {
                if (data == null)
                    data = new Datum<T>[] { };

                signalsDataRepositoryMock
                    .Setup(sd => sd.GetDataOlderThan<T>(It.Is<Signal>(s => s.Id == signalId), It.Is<DateTime>(d => d == fromDate), 1))
                    .Returns(data);
            }
            private void SetupGetNewerData<T>(int signalId, DateTime toDate, IEnumerable<Datum<T>> data = null)
            {
                if (data == null)
                    data = new Datum<T>[] { };
                signalsDataRepositoryMock.Setup(sd => sd.GetDataNewerThan<T>(It.Is<Signal>(s => s.Id == signalId), It.Is<DateTime>(d => d == toDate), 1))
                    .Returns(data);
            }
            private IEnumerable<Dto.Datum> GetDtoDatum<T>()
            {
                return new Dto.Datum[] {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = default(T) },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = default(T) },
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2001, 1, 1), Value = default(T) } };
            }
            private IEnumerable<Domain.Datum<T>> GetDomainDatum<T>()
            {
                return new Domain.Datum<T>[] {
                    new Domain.Datum<T>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 3, 1), Value = default(T), Signal = SignalWith() },
                    new Domain.Datum<T>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = default(T), Signal = SignalWith() },
                    new Domain.Datum<T>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2001, 1, 1), Value = default(T), Signal = SignalWith() } };
            }
            private bool EqualsSignal(Signal a, Signal b)
            {
                if (a.Id != b.Id) return false;
                if (a.DataType != b.DataType) return false;
                if (a.Granularity != b.Granularity) return false;
                if (a.Path.ToString() != b.Path.ToString()) return false;
                return true;
            }
            private Signal SignalWith()
            {
                return new Signal()
                {
                    Id = 1,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("x/y"),
                };
            }
            private Signal SignalWith(DataType dataType)
            {
                return new Signal()
                {
                    Id = 1,
                    DataType = dataType,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("x/y"),
                };
            }
            private Dto.Signal SignalWith(
                int? id = null,
                Dto.DataType dataType = Dto.DataType.Boolean,
                Dto.Granularity granularity = Dto.Granularity.Month,
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
                Domain.Path path
                )
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
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s =>
                    {
                        s.Id = 1;
                        return s;
                    });

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Signal signal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Path))
                    .Returns(signal);

                missingValuePolicyRepositoryMock
                    .Setup(s => s.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger());
            }

            private void GivenMVP(int signalId, MissingValuePolicyBase policy)
            {
                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Get(It.Is<Signal>(s => s.Id == signalId)))
                    .Returns(policy);
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
        }
    }
}