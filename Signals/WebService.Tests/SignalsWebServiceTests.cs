﻿using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Dto.MissingValuePolicy;
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
                    => passedSignal.DataType == DataType.Decimal
                        && passedSignal.Granularity == Granularity.Week
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


            [TestMethod]
            public void GivenASignalByPath_WhenGettingByPath_ReturnsNotNull()
            {
                var path = GivenASignalByPath();

                var result = signalsWebService.Get(path);
                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenASignalByPath_WhenGettingByPath_ReturnsSignalWithPath()
            {
                var path = GivenASignalByPath();

                var result = signalsWebService.Get(path);
                CollectionAssert.AreEqual(path.Components.ToArray(),result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenASignalByPath_WhenGettingByPath_RepositoryGetIsCalledWithGivenPath()
            {
                var path = GivenASignalByPath();

                var result = signalsWebService.Get(path);
                signalsRepositoryMock.Verify(x => x.Get(It.IsAny<Domain.Path>()));
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingByPath_ReturnsNull()
            {
                GivenNoSignals();
                Dto.Path notExistingPath = new Dto.Path() { Components = new[] { "root" } };

                var result = signalsWebService.Get(notExistingPath);

                Assert.IsNull(result);
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignals_WhenSettingData_ThrowsCouldntGetASignalException()
            {
                GivenNoSignals();

                Dto.Datum[] data = GetDtoDatumDouble();

                int notExistingSignalID = 8;
                signalsWebService.SetData(notExistingSignalID, data);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingData_VerifingRepositoryFunctionsGetAndSetData()
            {
                int signalId = 3;
                GivenASignal(SignalWith(
                   id: signalId,
                   dataType: Domain.DataType.Double,
                   granularity: Domain.Granularity.Month,
                   path: Domain.Path.FromString("root/signal")));

                Dto.Datum[] data = GetDtoDatumDouble();

                signalsDataRepositoryMock.Setup(x => x.SetData(It.IsAny<IEnumerable<Domain.Datum<double>>>()));

                signalsWebService.SetData(signalId, data);

                signalsRepositoryMock.Verify(x => x.Get(It.Is<int>(y => y.Equals(signalId))));
                signalsDataRepositoryMock.Verify(x => x.SetData(It.IsAny<IEnumerable<Domain.Datum<double>>>()));
            }

            [TestMethod]
            public void GivenASignal_WhenSettingData_ChecksIfSignalHasBeenSet()
            {
                int signalId = 7;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal")));

                Dto.Datum[] data = GetDtoDatumDouble();

                signalsDataRepositoryMock
                    .Setup(x => x.SetData(It.IsAny<IEnumerable<Domain.Datum<double>>>()))
                    .Callback<IEnumerable<Domain.Datum<double>>>(x => {
                        foreach(var match in x)
                        {
                            if (match.Signal == null) throw new SignalForDatumHasNotBeenSet();
                        }
                    });

                signalsWebService.SetData(signalId, data);
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignals_WhenGettingData_ThrowsCouldntGetASignalException()
            {
                GivenNoSignals();

                DateTime from = new DateTime(2000, 1, 1),to = new DateTime(2000, 3, 1);
                int notExistingSignalID = 8;
                signalsWebService.GetData(notExistingSignalID, from, to);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingData_ReturnsDatum()
            {
                int signalId = 7;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Integer,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal")));
                GivenMissingValuePolicy(signalId, new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger());
                DateTime from = new DateTime(2000, 1, 1), to = new DateTime(2000, 3, 1);

                if (!(signalsWebService.GetData(signalId, from, to) is IEnumerable<Dto.Datum>))
                    Assert.Fail();
            }


            [TestMethod]
            public void GivenASignal_WhenGettingData_ReturnsDataOrderedByDate()
            {
                int signalId = 7;
                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                GivenMissingValuePolicy(signalId, new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                var data = GetDomainDatumDouble();
                GivenData(signalId, data);

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                Assert.AreEqual(2, result.Count());
                Assert.AreEqual(new DateTime(2000, 1, 1), result.First().Timestamp);
                Assert.AreEqual(new DateTime(2000, 2, 1), result.Skip(1).First().Timestamp);
            }


            [TestMethod]
            public void GivenASignal_WhenGettingData_VerifyDataRepositoryFunctions()
            {
                int signalId = 7;
                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                GivenMissingValuePolicy(signalId, new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                DateTime from = new DateTime(2000, 1, 1), to = new DateTime(2000, 3, 1);

                GivenData(signalId, GetDomainDatumDouble());

                signalsWebService.GetData(signalId, from, to);

                signalsDataRepositoryMock.Verify(x => x.GetData<double>(
                    It.IsAny<Signal>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>()));
            }

            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignals_WhenSettingMissingValuePolicy_ThrowsCouldntGetASignalException()
            {
                GivenNoSignals();
                int nonExistingSignalId = 3;
                this.signalsWebService.SetMissingValuePolicy(nonExistingSignalId,null);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicy_VaryfingCallRepositorySetFunctions()
            {
                int signalId = 2;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal")));

                var missingValuePolicy = new SpecificValueMissingValuePolicy() {
                    DataType = Dto.DataType.Double, 
                    Quality = Dto.Quality.Fair, 
                    Value = (double)1.5 };


                missingValuePolicyRepositoryMock.Setup(x => x.Set(It.IsAny<Domain.Signal>(),
                    It.IsAny<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<Double>>()));

                this.signalsWebService.SetMissingValuePolicy(signalId, missingValuePolicy);

                missingValuePolicyRepositoryMock.Verify(x => x.Set(It.IsAny<Domain.Signal>(),
                    It.IsAny<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<Double>>()));
            }



            [TestMethod]
            [ExpectedException(typeof(CouldntGetASignalException))]
            public void GivenNoSignals_WhenGettingMissingValuePolicy_ThrowsCouldntGetASignalException()
            {
                GivenNoSignals();
                int nonExistingSignalId = 3;
                this.signalsWebService.GetMissingValuePolicy(nonExistingSignalId);
            }

            [TestMethod]
            public void GivenASignalForGettingMissingValuePolicy_WhenGettingMissingValuePolicy_ReturnsDefaultMissingValuePolicy()
            {
                int signalId = 2;
                GivenASignalForGettingMissingValuePolicy(signalId);

                if (!(this.signalsWebService.GetMissingValuePolicy(signalId) is Dto.MissingValuePolicy.MissingValuePolicy))
                    Assert.Fail();
            }


            [TestMethod]
            public void GivenASignalForGettingMissingValuePolicy_WhenGettingMissingValuePolicy_VaryfingCallOfRepositoryFunctioGet()
            {
                int signalId = 2;
                GivenASignalForGettingMissingValuePolicy(signalId);

                this.signalsWebService.GetMissingValuePolicy(signalId);

                this.missingValuePolicyRepositoryMock.Verify(x => x.Get(It.IsAny<Domain.Signal>()));
            }


            [TestMethod]
            public void GivenNoSignals_WhenAddingSignal_MissingValuePolicyRepositoryAddWithCorrectArgumentsIsCalled()
            {
                var signal = SignalWith(
                    dataType: Dto.DataType.Integer,
                    granularity: Dto.Granularity.Month,
                    path: new Dto.Path() { Components = new string[] { "root", "signal1" } });
                GivenNoSignals();

                var result = signalsWebService.Add(signal);

                missingValuePolicyRepositoryMock
                    .Verify(mvpr => mvpr.Set(It.Is<Domain.Signal>(s => s.Id == result.Id),
                    It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>()));
            }

            [TestMethod]
            public void GivenASignal_WhenGettingData_ReturnsCorrectAmount()
            {
                int signalId = 7;
                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                GivenMissingValuePolicy(signalId, new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyString());
                GivenData(signalId, GetDomainStringData());

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 7, 1));

                Assert.AreEqual(6, result.Count());
            }


            [TestMethod]
            public void GivenASignal_WhenGettingDataWithMissingValues_ReturnsSortedData()
            {
                int signalId = 7;
                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                GivenMissingValuePolicy(signalId, new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyString());
                GivenData(signalId, GetDomainStringData());

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 7, 1)).ToArray();

                for (int i = 0; i < result.Length - 1; i++)
                {
                    if (result[i].Timestamp > result[i + 1].Timestamp)
                        Assert.Fail();
                }
            }

            [TestMethod]
            public void GivenASignal_WhenGettingDataWithMissingData_DataHasCorrectTimeStamp()
            {
                int signalId = 7;
                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                GivenMissingValuePolicy(signalId, new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyString());
                GivenData(signalId, GetDomainStringData());

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 7, 1)).ToArray();

                Assert.AreEqual(new DateTime(2000, 1, 1), result[0].Timestamp);
                Assert.AreEqual(new DateTime(2000, 2, 1), result[1].Timestamp);
                Assert.AreEqual(new DateTime(2000, 3, 1), result[2].Timestamp);
                Assert.AreEqual(new DateTime(2000, 4, 1), result[3].Timestamp);
                Assert.AreEqual(new DateTime(2000, 5, 1), result[4].Timestamp);
                Assert.AreEqual(new DateTime(2000, 6, 1), result[5].Timestamp);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingDataWithMissingData_DataHasCorrectValue()
            {
                int signalId = 7;
                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                GivenMissingValuePolicy(signalId, new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyString());
                GivenData(signalId, GetDomainStringData());

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 8, 1)).ToArray();

                Assert.AreEqual("test2", result[0].Value);
                Assert.AreEqual(default(string), result[1].Value);
                Assert.AreEqual("test1", result[2].Value);
                Assert.AreEqual(default(string), result[3].Value);
                Assert.AreEqual(default(string), result[4].Value);
                Assert.AreEqual("test3", result[5].Value);
                Assert.AreEqual(default(string), result[6].Value);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingDataWithMissingData_DataHasCorrectQuality()
            {
                int signalId = 7;
                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                GivenMissingValuePolicy(signalId, new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyString());
                GivenData(signalId, GetDomainStringData());

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1)).ToArray();

                Assert.AreEqual(Dto.Quality.Fair, result[0].Quality);
                Assert.AreEqual(Dto.Quality.None, result[1].Quality);
                Assert.AreEqual(Dto.Quality.Good, result[2].Quality);
                Assert.AreEqual(Dto.Quality.None, result[3].Quality);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingDataWithEqualFromAndToTimestamps_ReturnsCorrectData()
            {
                int signalId = 6;
                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));

                GivenASignal(signal);
                GivenMissingValuePolicy(signalId, new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyString());
                GivenData(signalId, GetDomainDatumDouble());

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 2, 1), new DateTime(2000, 2, 1)).ToArray();

                Assert.AreEqual(Dto.Quality.Good, result[0].Quality);
                Assert.AreEqual(new DateTime(2000, 2, 1), result[0].Timestamp);
                Assert.AreEqual(5.0, result[0].Value);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingPathEntry_DoesNotThrow()
            {
                GivenNoSignals();
                signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "x" } });
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingPathEntry_RepositoryGetAllWithPathPrefixIsCalled()
            {
                GivenNoSignals();

                signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "x" } });

                signalsRepositoryMock.Verify(srm => srm.GetAllWithPathPrefix(It.IsAny<Path>()), Times.Once);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingPathEntry_RepositoryGetAllWithPathPrefixIsCalledWithCorrectPath()
            {
                GivenNoSignals();

                signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "x", "y", "z" } });

                signalsRepositoryMock.Verify(srm => srm.GetAllWithPathPrefix(It.Is<Domain.Path>(p => p.ToString().Equals("x/y/z"))), Times.Once);
            }

            [TestMethod]
            public void GivenSignals_WhenGettingPathEntry_PathEntryWithCorrectSignalsIsReturned()
            {
                GivenSignals(new List<Signal>()
                {
                    new Signal() { Path = Path.FromString("root/s1") },
                    new Signal() { Path = Path.FromString("root/podkatalog/s2") },
                    new Signal() { Path = Path.FromString("root/podkatalog/s3") },
                    new Signal() { Path = Path.FromString("root/podkatalog/podpodkatalog/s4") },
                    new Signal() { Path = Path.FromString("root/podkatalog2/s5") }
                });

                var expectedSignals = new Dto.Signal[] { new Dto.Signal() { Path = new Dto.Path() { Components = new[] { "root", "s1" } } } };
                var expectedPathEntry = new Dto.PathEntry();
                expectedPathEntry.Signals = expectedSignals;

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root" } });

                CollectionAssert.AreEqual(result.Signals.ElementAt(0).Path.Components.ToArray(), expectedPathEntry.Signals.ElementAt(0).Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenSignals_WhenGettingPathEntry_PathEntryWithCorrectSubPathsIsReturned()
            {
                GivenSignals(new List<Signal>()
                {
                    new Signal() { Path = Path.FromString("root/s1") },
                    new Signal() { Path = Path.FromString("root/podkatalog/s2") },
                    new Signal() { Path = Path.FromString("root/podkatalog/s3") },
                    new Signal() { Path = Path.FromString("root/podkatalog/podpodkatalog/s4") },
                    new Signal() { Path = Path.FromString("root/podkatalog2/s5") }
                });

                var expectedSubPaths = new Dto.Path[]
                {
                    new Dto.Path() {Components = new[] {"root", "podkatalog"} },
                    new Dto.Path() {Components = new[] {"root", "podkatalog2"} }
                };
                var expectedPathEntry = new Dto.PathEntry();
                expectedPathEntry.SubPaths = expectedSubPaths;

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root" } });

                CollectionAssert.AreEqual(result.SubPaths.ElementAt(0).Components.ToArray(), expectedPathEntry.SubPaths.ElementAt(0).Components.ToArray());
                CollectionAssert.AreEqual(result.SubPaths.ElementAt(1).Components.ToArray(), expectedPathEntry.SubPaths.ElementAt(1).Components.ToArray());
            }

            [TestMethod]
            public void GivenSignals_GivenPathWithMoreThan1Component_WhenGettingPathEntry_PathEntryWithCorrectSubPathsIsReturned()
            {
                GivenSignals(new List<Signal>()
                {
                    new Signal() { Path = Path.FromString("root/s1") },
                    new Signal() { Path = Path.FromString("root/s1/s2") },
                    new Signal() { Path = Path.FromString("root/s1/s3") }
                });

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root", "s1" } });

                Assert.AreEqual(0, result.SubPaths.Count());
            }

            [TestMethod]
            public void GivenASignalWithIncompleteDataAndSpecificMissingValuePolicy_WhenGettingData_CorrectAmountOfDataIsReturned()
            {
                int signalId = 5;
                var signal = new Signal() { Id = signalId, DataType = DataType.Integer, Granularity = Granularity.Day };
                GivenASignal(signal);
                missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(signal)).Returns(new SpecificValueMissingValuePolicyInteger());
                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<int>(signal, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new Datum<int>[]
                {
                    new Datum<int>() {Signal = signal, Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 2), Value = (int)3 },
                    new Datum<int>() {Signal = signal, Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 4), Value = (int)4 }
                });

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 6));

                Assert.AreEqual(5, result.Count());
            }

            [TestMethod]
            public void GivenASignalWithIncompleteDataAndSpecificMissingValuePolicy_WhenGettingData_CorrectDataIsReturned()
            {
                int signalId = 8;
                var signal = new Signal() { Id = signalId, DataType = DataType.String, Granularity = Granularity.Year };
                GivenASignal(signal);
                missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(signal)).Returns(new SpecificValueMissingValuePolicyString() { Quality = Quality.Good, Signal = signal, Value = "cc" });
                signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<string>(signal, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new Datum<string>[]
                {
                    new Datum<string>() {Signal = signal, Quality = Quality.Good, Timestamp = new DateTime(2003, 1, 1), Value = "aa" },
                    new Datum<string>() {Signal = signal, Quality = Quality.Poor, Timestamp = new DateTime(2004, 1, 1), Value = "bb" }
                });

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2006, 1, 1));

                AssertDefaultDataIsCorrect<string>(Dto.Quality.Good, "cc", result.ElementAt(0), result.ElementAt(1), result.ElementAt(2), result.ElementAt(5));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignalWithId_WhenAddingASignal_ThrowsArgumentException()
            {
                int signalId = 9;
                GivenNoSignals();

                signalsWebService.Add(new Dto.Signal()
                {
                    Id = signalId,
                    DataType = Dto.DataType.Integer,
                    Granularity = Dto.Granularity.Week,
                    Path = new Dto.Path() { Components = new[] {"x", "y"} }
                });
            }


            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignal_WhenSettingDatumWithIncorrectDate_ThrowsArgumentException()
            {
                //arrange
                int dummyId = 5;
                GivenASignal( SignalWith( dummyId,  DataType.Boolean,   Granularity.Month,Path.FromString("x/y") ));
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
                //act

                signalsWebService.SetData(dummyId,new Dto.Datum[] {new  Dto.Datum() { Timestamp = new DateTime(2015, 2, 5) } });

                //assert
            }
            [TestMethod]
            public void GivenASignal_WhenSettingDatumWithCorrectHour_DoNotThrowsException()
            {
                //arrange
                int dummyId = 5;
                GivenASignal(SignalWith(dummyId, DataType.Boolean, Granularity.Day, Path.FromString("x/y")));
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
                //act

                signalsWebService.SetData(dummyId, new Dto.Datum[] { new Dto.Datum() { Timestamp = new DateTime(2015, 2, 5,0,0,0) } });

                //assert
            }
            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignalWithMonthGranularity_WhenSettingDatumWithIncorrectDate_ThrowsArgumentException()
            {
                //arrange
                int dummyId = 5;
                GivenASignal(SignalWith(dummyId, DataType.Boolean, Granularity.Month, Path.FromString("x/y")));
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
                //act

                signalsWebService.SetData(dummyId, new Dto.Datum[] { new Dto.Datum() { Timestamp = new DateTime(2015, 2, 1,1,1,1) } });

                //assert
            }
            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignalWithDayGranularity_WhenSettingDatumWithIncorrectDate_ThrowsArgumentException()
            {
                //arrange
                int dummyId = 5;
                GivenASignal(SignalWith(dummyId, DataType.Boolean, Granularity.Day, Path.FromString("x/y")));
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
                //act

                signalsWebService.SetData(dummyId, new Dto.Datum[] { new Dto.Datum() { Timestamp = new DateTime(2015, 2, 1, 1, 1, 1) } });

                //assert
            }
            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignalWithWeekGranularity_WhenSettingDatumWithIncorrectDate_ThrowsArgumentException()
            {
                //arrange
                int dummyId = 5;
                GivenASignal(SignalWith(dummyId, DataType.Boolean, Granularity.Week, Path.FromString("x/y")));
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
                //act

                signalsWebService.SetData(dummyId, new Dto.Datum[] { new Dto.Datum() { Timestamp =new DateTime(2016,8,23) } });

                //assert
            }
            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenASignalWithMonthGranularity_WhenGettingDatumWithIncorrectDate_ThrowsArgumentException()
            {
                //arrange
                int dummyId = 5;
                GivenASignal(SignalWith(dummyId, DataType.Boolean, Granularity.Week, Path.FromString("x/y")));
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
                //act

                signalsWebService.GetData(dummyId, new DateTime(2015, 1, 23), new DateTime(2016, 2, 1));

                //assert
            }
            [TestMethod]
            public void GivenASignal_WhenGettingDataWithZeroOrderMissingData_ReturnsNotNull()
            {
                int signalId = 7;
                MakeASignalWebServiceForZeroOrderMissingValuePolicy(signalId);

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1)).ToArray();

                Assert.IsNotNull( result[0]);
         
           
            }
            [TestMethod]
            public void GivenASignal_WhenGettingDataWithZeroOrderMissingData_ReturnsValueFromEarlierDatum()
            {
                int signalId = 7;
                MakeASignalWebServiceForZeroOrderMissingValuePolicy(signalId);

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1)).ToArray();

                Assert.AreEqual("test1",result[0].Value);
                Assert.AreEqual("test1", result[1].Value);

            }
            [TestMethod]
            public void GivenASignal_WhenGettingDataWithZeroOrderMissingData_ReturnsDatumWithGoodDate()
            {
                int signalId = 7;
                MakeASignalWebServiceForZeroOrderMissingValuePolicy(signalId);
                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1)).ToArray();

                Assert.AreEqual("test1", result[0].Value);
                Assert.AreEqual("test1", result[1].Value);
                Assert.AreEqual(new DateTime(2000, 1, 1), result[0].Timestamp);
                Assert.AreEqual(new DateTime(2000, 2, 1), result[1].Timestamp);
                Assert.AreEqual(Dto.Quality.Good, result[0].Quality);
                Assert.AreEqual(Dto.Quality.Good, result[1].Quality);

            }

            [TestMethod]
            public void GivenASignal_WhenGettingDataWithZeroOrderMissingValuePolicy_CheckIfReturnsTheOldestData()
            {
                int signalId = 5;
                Domain.Datum<string> givenDatum = new Domain.Datum<string>()
                { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = "first" };

                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Day,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                GivenMissingValuePolicy(signalId, new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyString());
                GivenData(signalId, new Domain.Datum<string>[] { givenDatum });
                SetupSignalsRepoGetDataOlderThan_ReturnsDatum(new Domain.Datum<string>[] { givenDatum }, signalId);

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 10), new DateTime(2000, 1, 11)).ToArray();

                Assert.AreEqual("first", result[0].Value);
                Assert.AreEqual(Dto.Quality.Good, result[0].Quality);
                Assert.AreEqual(new DateTime(2000, 1, 10), result[0].Timestamp);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingDataWithZeroOrderMissingValuePolicy_CheckIfSetDefaultValueWhenEarlierSignalDoesNotExist()
            {
                int signalId = 5;
                List<Domain.Datum<string>> givenDatums = new List<Domain.Datum<string>>()
                { new Domain.Datum<string> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 2), Value = "first" },
                  new Domain.Datum<string> { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 1, 4), Value = "second" } };

                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Day,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                GivenMissingValuePolicy(signalId, new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyString());
                GivenData(signalId, givenDatums);
                SetupSignalsRepoGetDataOlderThan_ReturnsEmptyList(signal);

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 1, 5)).ToArray();

                Assert.AreEqual(default(string), result[0].Value);
                Assert.AreEqual(Dto.Quality.None, result[0].Quality);
                Assert.AreEqual(new DateTime(2000, 1, 1), result[0].Timestamp);
            }

            [TestMethod]
            public void GivenASignalAndMissingValuePolicy_WhenDeletingSignal_CheckIfMVPIsSettedNull()
            {
                int signalId = 6;

                var signal = SignalWith(
                     id: signalId,
                    dataType: Domain.DataType.Decimal,
                    granularity: Domain.Granularity.Day,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);

                signalsWebService.Delete(signalId);

                missingValuePolicyRepositoryMock.Verify(mvprm => mvprm.Set(It.Is<Domain.Signal>(s => s.Id == signalId),
                    It.Is<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>(svm => svm == null)));
            }

            [TestMethod]
            public void GivenASignalAndMissingValuePolicy_WhenDeletingSignal_CheckIfDataIsDeleted()
            {
                int signalId = 6;

                var signal = SignalWith(
                     id: signalId,
                    dataType: Domain.DataType.Decimal,
                    granularity: Domain.Granularity.Day,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);

                signalsWebService.Delete(signalId);

                signalsDataRepositoryMock.Verify(sdr => sdr.DeleteData<decimal>(It.Is<Domain.Signal>(s => s.Id == signalId)));
            }

            [TestMethod]
            public void GivenASignalAndMissingValuePolicy_WhenDeletingSignal_CheckIfSignalIsDeleted()
            {
                int signalId = 6;

                var signal = SignalWith(
                     id: signalId,
                    dataType: Domain.DataType.Decimal,
                    granularity: Domain.Granularity.Day,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);

                signalsWebService.Delete(signalId);

                signalsRepositoryMock.Verify(sr => sr.Delete(It.Is<Domain.Signal>(s => s.Id == signalId)));
            }

            private void SetupSignalsRepoGetDataOlderThan_ReturnsDatum(IEnumerable<Datum<string>> givenDatums, int signalId)
            {
                Datum<string> oneDatum = givenDatums.OrderBy(d => d.Timestamp).LastOrDefault();
                List<Datum<string>> returnDatumList = new List<Datum<string>>(new Datum<string>[] { oneDatum });
                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetDataOlderThan<string>(It.Is<Domain.Signal>(s => s.Id == signalId), It.IsAny<DateTime>(), It.IsAny<int>()))
                    .Returns(returnDatumList);
            }

            private void SetupSignalsRepoGetDataOlderThan_ReturnsEmptyList(Signal signal)
            {
                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetDataOlderThan<string>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                    .Returns(new List<Datum<string>>());

            }
            private void MakeASignalWebServiceForZeroOrderMissingValuePolicy(int signalId)
            {       
                var signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                GivenMissingValuePolicy(signalId, new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyString());
                GivenData(signalId, new Domain.Datum<string>[] { new Domain.Datum<string>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = "test1" } });
            }

            private void GivenSignals(IEnumerable<Signal> signals)
            {
                GivenNoSignals();
                signalsRepositoryMock.Setup(srm => srm.GetAllWithPathPrefix(It.IsAny<Path>())).Returns(signals);
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
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Domain.Signal existingSignal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);
            }

            private void GivenMissingValuePolicy(int signalId, Domain.MissingValuePolicy.MissingValuePolicyBase mvp)
            {
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Get(It.Is<Domain.Signal>(s => s.Id == signalId)))
                    .Returns(mvp);
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

            private Dto.Path GivenASignalByPath()
            {
                GivenNoSignals();

                Domain.Path path = Domain.Path.FromString("root/signal");
                signalsRepositoryMock.Setup(x => x.Get(path)).Returns(new Domain.Signal()
                {
                    Path = path
                });

                return path.ToDto<Dto.Path>();
            }

            private void GivenASignalForGettingMissingValuePolicy(int signalId)
            {
                Signal signal = SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.Double,
                    granularity: Domain.Granularity.Month,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);

                this.missingValuePolicyRepositoryMock
                    .Setup(x => x.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new SpecificValueMissingValuePolicyDouble());
            }

            private void GivenData<T>(int signalId, IEnumerable<Domain.Datum<T>> data)
            {
                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetData<T>(It.Is<Domain.Signal>(s => s.Id == signalId),
                    It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns<Domain.Signal, DateTime, DateTime>(
                    (s, from, to) => data.Where(d => 
                    {   
                        if (!from.Equals(to))
                            return d.Timestamp >= from && d.Timestamp < to;
                        else
                            return d.Timestamp >= from && d.Timestamp <= to;
                    }));
            }

            private Domain.Datum<double>[] GetDomainDatumDouble()
            {
                return new Domain.Datum<double>[]
                    {
                        new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)5 },
                        new Domain.Datum<double>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Domain.Datum<double>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 }
                    };
            }

            private Dto.Datum[] GetDtoDatumDouble()
            {
                return new Dto.Datum[]
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 }
                    };
            }

            public IEnumerable<Datum<string>> GetDomainStringData()
            {
                return new Datum<string>[]
                {
                        new Domain.Datum<string>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = "test1" },
                        new Domain.Datum<string>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = "test2" },
                        new Domain.Datum<string>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 6, 1), Value = "test3" }
                };
            }

            private void AssertDefaultDataIsCorrect<T>(Dto.Quality quality, T value, params Dto.Datum[] data)
            {
                foreach (var datum in data)
                {
                    Assert.AreEqual(datum.Quality, quality);
                    Assert.AreEqual(datum.Value, value);
                }
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;

        }
    }
}