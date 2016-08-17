﻿using System;
using System.Linq;
using System.Collections.Generic;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DataAccess;
using Domain.Exceptions;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            // -------------------------------------------------------------------------------------------
            // 1st Iteration:
            // -------------------------------------------------------------------------------------------

            ISignalsWebService signalsWebService;
            Mock<ISignalsRepository> signalsRepositoryMock;
            Mock<ISignalsDataRepository> signalsDataRepositoryMock;
            Mock<IMissingValuePolicyRepository> signalsMissingValuePolicyRepositoryMock;

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

                signalsRepositoryMock.Verify(sr => sr.Add(It.Is<Signal>(passedSignal
                    => passedSignal.DataType == DataType.Decimal
                        && passedSignal.Granularity == Granularity.Week
                        && passedSignal.Path.ToString() == "root/signal")));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsIdFromRepository()
            {
                var signalId = 1;
                GivenNoSignals();
                GivenRepositoryThatAssigns(signalId);
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

            private Dto.Signal SignalWith(Dto.DataType dataType, Dto.Granularity granularity, Dto.Path path)
            {
                return new Dto.Signal()
                {
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }

            private Signal SignalWith(int id, Domain.DataType dataType, Domain.Granularity granularity, Domain.Path path)
            {
                return new Signal()
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
                    .Setup(sr => sr.Add(It.IsAny<Signal>()))
                    .Returns<Signal>(s => s);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsMissingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, signalsMissingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Signal existingSignal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);
                signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Path))
                    .Returns(existingSignal);
            }

            private void GivenRepositoryThatAssigns(int id)
            {
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Signal>()))
                    .Returns<Signal>(s =>
                    {
                        s.Id = id;
                        return s;
                    });
            }

            // -------------------------------------------------------------------------------------------
            // 2nd Iteration:
            // -------------------------------------------------------------------------------------------

            [TestMethod]
            public void GivenASignal_WhenGettingByPathWhichExistsInRepository_ReturnedIsTheSignal()
            {
                var dummyPath = Path.FromString("root/signal1");
                var dummySignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Hour,
                    Path = dummyPath
                };
                GivenASignal(dummySignal);

                var result = signalsWebService.Get(dummyPath.ToDto<Dto.Path>());

                Assert.AreEqual(dummySignal.Granularity, result.Granularity.ToDomain<Granularity>());
                Assert.AreEqual(dummySignal.DataType, result.DataType.ToDomain<DataType>());
                CollectionAssert.AreEqual(dummySignal.Path.Components.ToArray(), result.Path.Components.ToArray());
            }

            // -------------------------------------------------------------------------------------------

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenGettingDataOfAnySignal_ThrowedIsArgumentException()
            {
                GivenNoSignals();

                signalsWebService.GetData(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>());
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingDataOfOtherSignal_ThrowedIsArgumentException()
            {
                GivenASignal(new Signal { Id = 1 });

                signalsWebService.GetData(2, It.IsAny<DateTime>(), It.IsAny<DateTime>());
            }

            [TestMethod]
            public void GivenASignal_WhenGettingDataOfTheSignal_ReturnedIsTheData()
            {
                var dummySignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Year,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetData<decimal>(dummySignal, new DateTime(2015, 1, 1), new DateTime(2017, 1, 1)))
                    .Returns(new Datum<decimal>[] { new Datum<decimal>() { Id = 1, Quality = Quality.Fair, Signal = dummySignal, Timestamp = new DateTime(2016, 1, 1), Value = 10M } });
                var expected = new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 1, 1), Value = 10M };

                var result = signalsWebService.GetData(1, new DateTime(2015, 1, 1), new DateTime(2017, 1, 1));

                foreach (var item in result)
                {
                    Assert.AreEqual(expected.Quality, item.Quality);
                    Assert.AreEqual(expected.Timestamp, item.Timestamp);
                    Assert.AreEqual(expected.Value, item.Value);
                }
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenSettingDataOfAnySignal_ThrowedIsArgumentException()
            {
                GivenNoSignals();

                signalsWebService.SetData(1, null);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingDataOfOtherSignal_ReturnedIsArgumentException()
            {
                GivenASignal(new Signal { Id = 1 });

                signalsWebService.SetData(2, null);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingDataOfTheSignalAndGettingTheData_GotIsNotNullData()
            {
                GivenASignal(new Signal { Id = 1, DataType = DataType.Double });

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<double>(It.Is<Signal>(signal => signal.Id == 1), new DateTime(2015, 1, 1), new DateTime(2017, 1, 1)))
                    .Returns(new Datum<double>[] { new Datum<double>() { Timestamp = new DateTime(2016, 1, 1), Value = 10.0, Quality = Quality.Fair } });
                signalsWebService.SetData(1, new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 1, 1), Value = 10.0 } });
                var result = signalsWebService.GetData(1, new DateTime(2015, 1, 1), new DateTime(2017, 1, 1));

                Assert.IsNotNull(result);
            }

            // -------------------------------------------------------------------------------------------



            // -------------------------------------------------------------------------------------------
            // 3rd Iteration:
            // -------------------------------------------------------------------------------------------

            [TestMethod]
            public void GivenNoSignals_WhenGettingByAnyPath_ReturnedIsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.Get(new Dto.Path() { Components = new string[] { "root", "signal" } });

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingByAnyId_ReturnedIsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.GetById(1);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByOtherPath_ReturnedIsNull()
            {
                GivenASignal(new Signal { Id = 1, Path = Path.FromString("root/signal1") });

                var result = signalsWebService.Get(new Dto.Path() { Components = new string[] { "root", "signal2" } });

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByOtherId_ReturnedIsNull()
            {
                GivenASignal(new Signal { Id = 1 });

                var result = signalsWebService.GetById(2);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByTheSignalsPath_ReturnedIsNotNullResult()
            {
                GivenASignal(new Signal { Id = 1, Path = Path.FromString("root/signal1") });

                var result = signalsWebService.Get(new Dto.Path() { Components = new string[] { "root", "signal1" } });

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByTheSignalsId_ReturnedIsNotNullResult()
            {
                GivenASignal(new Signal { Id = 1 });

                var result = signalsWebService.GetById(1);

                Assert.IsNotNull(result);
            }

            // -------------------------------------------------------------------------------------------

            [TestMethod]
            public void GivenSignalWithDataSetNotInDateOrder_WhenGettingDataOfTheSignal_ReturnedIsSortedArray()
            {
                var dummySignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<double>(
                    It.Is<Signal>(s => s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                    It.Is<DateTime>(dt => dt.Year == 2000 && dt.Month == 1 && dt.Day == 1), It.Is<DateTime>(dt => dt.Year == 2000 && dt.Month == 3 && dt.Day == 1)))
                .Returns(new Datum<double>[]
                { new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = (double)1 },
                  new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 }});

                var result = signalsWebService.GetData(dummySignal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                Assert.AreEqual(new DateTime(2000, 1, 1), result.ToArray()[0].Timestamp);
                Assert.AreEqual(new DateTime(2000, 2, 1), result.ToArray()[1].Timestamp);
            }

            // -------------------------------------------------------------------------------------------

            [TestMethod]
            public void GivenNoSignals_WhileAddingASignal_WasCalledSetMissingValuePolicyMethod()
            {
                GivenNoSignals();

                var dummySignal = new Signal()
                {
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("root/signal")
                };

                signalsWebService.Add(dummySignal.ToDto<Dto.Signal>());

                signalsMissingValuePolicyRepositoryMock.Verify(mvpr => mvpr.Set(
                    It.Is<Signal>(s => s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                    It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<bool>>()));
            }

            // -------------------------------------------------------------------------------------------

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenGettingMVPOfAnySignal_ThrowedIsArgumentException()
            {
                GivenNoSignals();

                signalsWebService.GetMissingValuePolicy(1);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenGettingMVPOfOtherSignal_ThrowedIsArgumentException()
            {
                var dummySignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                signalsWebService.GetMissingValuePolicy(2);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingMVPOfTheSignal_ReturnedIsNull()
            {
                var dummySignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                var result = signalsWebService.GetMissingValuePolicy(1);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignalWithSetMVP_WhenGettingMVPOfTheSignal_ReturnedIsTheMVP()
            {

                var dummySignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                var dummyMVP = new Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<bool>()
                {
                    Id = 1,
                    Quality = Quality.Fair,
                    Signal = dummySignal,
                    Value = true
                };

                signalsMissingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.Is<Signal>
                    (s => s.Id == dummySignal.Id && s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.DataType == dummySignal.DataType)))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyBoolean() { Id = 1, Quality = Quality.Fair, Signal = dummySignal, Value = true, });

                var result = signalsWebService.GetMissingValuePolicy(1);

                Assert.AreEqual(dummyMVP.Id, result.Id);
                Assert.AreEqual(dummyMVP.Signal.Id, result.Signal.Id);
                Assert.AreEqual(dummyMVP.Signal.DataType, result.Signal.DataType.ToDomain<Domain.DataType>());
                Assert.AreEqual(dummyMVP.Signal.Granularity, result.Signal.Granularity.ToDomain<Domain.Granularity>());
                CollectionAssert.AreEqual(dummyMVP.Signal.Path.Components.ToArray(), result.Signal.Path.Components.ToArray());
                Assert.AreEqual(Dto.DataType.Boolean, result.DataType);
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignals_WhenSettingMVPOfAnySignal_ThrowedIsArgumentException()
            {
                GivenNoSignals();

                signalsWebService.SetMissingValuePolicy(1, new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy());
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingMVPOfOtherSignal_ThrowedIsArgumentException()
            {
                var dummySignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                signalsWebService.SetMissingValuePolicy(2, new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy());
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMVPOfTheSignal_MVPRepositorySetMethodWasCalled()
            {
                var dummySignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Hour,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                var dummyMVP = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
                {
                    Id = 1,
                    DataType = Dto.DataType.Boolean,
                    Quality = Dto.Quality.Fair,
                    Signal = dummySignal.ToDto<Dto.Signal>(),
                    Value = false
                };

                signalsWebService.SetMissingValuePolicy(1, dummyMVP);

                signalsMissingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Set
                    (It.Is<Signal>(s => s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                    It.Is<Domain.MissingValuePolicy.MissingValuePolicyBase>(mvp => mvp.Id == dummyMVP.Id && mvp.NativeDataType == dummyMVP.Value.GetType())));
            }

            // -------------------------------------------------------------------------------------------

            [TestMethod]
            public void GivenASignalWithDataAndSetNoneQualityMVP_WhenGettingDataOfTheSignal_ReturnedIsFilledArray()
            {
                var dummySignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                signalsMissingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get
                    (It.Is<Signal>(s => s.Id == dummySignal.Id && s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path))))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<double>
                    (It.Is<Signal>(s => s.Id == dummySignal.Id && s.DataType == dummySignal.DataType && s.Granularity == dummySignal.Granularity && s.Path.Equals(dummySignal.Path)),
                    new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)))
                .Returns(new Datum<double>[] { new Datum<double>() { Value = 10.0, Signal = dummySignal, Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Id = 1 } });

                var expectedArray = new Dto.Datum[]
                {
                    new Dto.Datum() {Timestamp = new DateTime(2000,1,1), Quality = Dto.Quality.None, Value = 0.0 },
                    new Dto.Datum() {Timestamp = new DateTime(2000,2,1), Quality = Dto.Quality.Fair, Value = 10.0 },
                    new Dto.Datum() {Timestamp = new DateTime(2000,3,1), Quality=Dto.Quality.None, Value=0.0 }
                };

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                Assert.AreEqual(expectedArray[0].Quality, result.ToArray()[0].Quality);
                Assert.AreEqual(expectedArray[0].Timestamp, result.ToArray()[0].Timestamp);
                Assert.AreEqual(expectedArray[0].Value, result.ToArray()[0].Value);
                Assert.AreEqual(expectedArray[1].Quality, result.ToArray()[1].Quality);
                Assert.AreEqual(expectedArray[1].Timestamp, result.ToArray()[1].Timestamp);
                Assert.AreEqual(expectedArray[1].Value, result.ToArray()[1].Value);
                Assert.AreEqual(expectedArray[2].Quality, result.ToArray()[2].Quality);
                Assert.AreEqual(expectedArray[2].Timestamp, result.ToArray()[2].Timestamp);
                Assert.AreEqual(expectedArray[2].Value, result.ToArray()[2].Value);
            }

            //-------------------------------------------------------------------------

            [TestMethod]
            public void GivenASignalWithData_WhenGettingDataWithIdenticalFromAndToDates_ReturnsTheCorrectItem()
            {
                var dummyId = 1;
                var dummySignal = new Signal()
                {
                    Id = dummyId,
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                signalsMissingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var date = new DateTime(2000, 1, 1);

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<double>(It.IsAny<Signal>(), date, date))
                    .Returns(new Datum<double>[] { new Datum<double> { Value = 10.0, Signal = dummySignal, Quality = Quality.Fair, Timestamp = date, Id = 1 } });

                var result = signalsWebService.GetData(dummyId, date, date);
                var datum = result.Single();

                Assert.AreEqual(Dto.Quality.Fair, datum.Quality);
                Assert.AreEqual(10.0, datum.Value);
                Assert.AreEqual(date, datum.Timestamp);
            }

            [TestMethod]
            public void GivenASignalWithData_WhenGettingDataFromAnEmptyPeriod_ReturnsCorrectAmountOfData()
            {
                var dummyId = 1;
                var dummySignal = new Signal()
                {
                    Id = dummyId,
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                signalsMissingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                signalsDataRepositoryMock.Setup(sdr => sdr.GetData<double>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(new Datum<double>[0]);

                var result = signalsWebService.GetData(dummyId, new DateTime(), new DateTime().AddMonths(2));

                Assert.AreEqual(2, result.Count());
            }

            [TestMethod]
            public void GivenSomeSignals_WhenGettingPathEntry_ReturnsTheCorrectEntry()
            {
                var signals = new[]
                {
                    new Signal {Id = 1, Path = Path.FromString("p") },
                    new Signal {Id = 2, Path = Path.FromString("s/p") },
                    new Signal {Id = 3, Path = Path.FromString("s/s/p") },
                    new Signal {Id = 3, Path = Path.FromString("s/s/p2") }
                };
                GivenSomeSignals(signals);

                signalsRepositoryMock.Setup(sr => sr.GetAllWithPathPrefix(Path.FromString("s")))
                    .Returns(signals.Except(new[] { signals[0] }));

                var result = signalsWebService.GetPathEntry(new Dto.Path { Components = new[] { "s" } });
                
                Assert.AreEqual(1, result.Signals.Count());
                Assert.AreEqual(1, result.SubPaths.Count());
            }

            private void GivenSomeSignals(Signal[] signals)
            {
                GivenNoSignals();

                foreach (var signal in signals)
                {
                    signalsRepositoryMock.Setup(sr => sr.Get(signal.Id.Value)).Returns(signal);
                    signalsRepositoryMock.Setup(sr => sr.Get(signal.Path)).Returns(signal);
                }
            }

            [TestMethod]
            public void GivenASignal_WhenSettingDataOfStringThatIsNull_DoesNotThrow()
            {
                var dummyId = 1;
                var dummySignal = new Signal()
                {
                    Id = dummyId,
                    DataType = DataType.String,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                signalsWebService.SetData(dummyId, new[] { new Dto.Datum { Value = null } });
            }

            [TestMethod]
            public void GivenASignal_WhenSettingData_RepositoryIsCalledWithSignalReferences()
            {
                var dummyId = 1;
                var dummySignal = new Signal()
                {
                    Id = dummyId,
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                signalsWebService.SetData(dummyId, new[] { new Dto.Datum { Value = 10.0 } });

                signalsDataRepositoryMock.Verify(sdr => sdr.SetData(It.Is<IEnumerable<Datum<double>>>(enumerable => enumerable.All(d => d.Signal != null))));
            }

            [TestMethod]
            [ExpectedException(typeof(IdNotNullException))]
            public void GivenNoSignal_WhenAddingASignalWithIdSet_ThrowsException()
            {
                GivenNoSignals();

                signalsWebService.Add(new Dto.Signal { Id = 1 });
            }

            [TestMethod]
            public void GivenASignalAndData_WhenGettingDataWithToDateLessThanFromDate_ReturnEmptyIEnumerable()
            {
                var dummyId = 1;
                var dummySignal = new Signal()
                {
                    Id = dummyId,
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                signalsMissingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetData<double>(dummySignal, It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new[] { new Datum<double> { Value = 1.0 } });

                var result = signalsWebService.GetData(dummyId, new DateTime(2000, 3, 1), new DateTime(2000, 1, 1));

                Assert.AreEqual(0, result.Count());
            }

            [TestMethod]
            public void GivenASignalAndSpecificValueMVP_WhenGettingData_ReturnsCorrectData()
            {
                var dummyId = 1;
                var dummySignal = new Signal()
                {
                    Id = dummyId,
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path = Path.FromString("root/signal")
                };
                GivenASignal(dummySignal);

                signalsMissingValuePolicyRepositoryMock.Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble() {Quality = Quality.Fair, Value = 5.0 });

                var result = signalsWebService.GetData(dummyId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                Assert.AreEqual(2, result.Count());
                foreach (var datum in result)
                {
                    Assert.AreEqual(5.0, datum.Value);
                    Assert.AreEqual(Dto.Quality.Fair, datum.Quality);
                }
            }
        }
    }
}