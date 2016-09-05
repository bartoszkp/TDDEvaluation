using System.Linq;
using System.Collections.Generic;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using DataAccess;
using DataAccess.GenericInstantiations;
using Dto;
using Domain.MissingValuePolicy;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;
            private VerifyOrAssertTestResults verifyOrAssert;

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsNotNull()
            {
                GivenASignalSetupAddSignal();
                var result = signalsWebService.Add(new Dto.Signal());

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsTheSameSignalExceptForId()
            {
                GivenASignalSetupAddSignal();
                var result = signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Decimal,
                    granularity: Dto.Granularity.Week,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }));

                SetupVerifyOrAssert();
                verifyOrAssert.AssertAddingASignalReturnsThisSignal(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_PassesGivenSignalToRepositoryAdd()
            {
                GivenASignalSetupAddSignal();

                signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Decimal,
                    granularity: Dto.Granularity.Week,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }));

                SetupVerifyOrAssert();
                verifyOrAssert.AssertRepositoryAddIsCalled(signalsRepositoryMock);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsIdFromRepository()
            {
                var signalId = 123;
                GivenASignalSetupAddSignal();
                signalsRepositoryMock
                                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                                    .Returns<Domain.Signal>(s => new Domain.Signal() { Id = signalId, DataType = s.DataType, Granularity = s.Granularity, Path = s.Path });


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
            public void GivenASignal_WhenIdIsNotExist_ReturnNull()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Year,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.GetById(999);
                Assert.IsNull(result);
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

                SetupVerifyOrAssert();
                verifyOrAssert.AssertGettingSignalsByItsId(signalId, result);
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
            public void GivenNoSignals_WhenGettingByPath_DoesNotThrow()
            {
                signalsWebService = new SignalsWebService(null);

                signalsWebService.Get(null);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingByPath_ReturnsNull()
            {
                signalsWebService = new SignalsWebService(null);

                var result = signalsWebService.Get(null);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByPath_RepositoryGetIsCalled()
            {
                string path = "root/signal1";

                GivenASignalSetupGetByPath(SignalWith(
                    id: 1,
                    dataType: Domain.DataType.Boolean,
                    granularity: Domain.Granularity.Day,
                    path: Domain.Path.FromString((path))));

                var pathDto = new Dto.Path() { Components = new[] { "root", "signal1" } };

                signalsWebService.Get(pathDto);

                signalsRepositoryMock.Verify(srm => srm.Get(Domain.Path.FromString(path)));
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByPath_ReturnsIt()
            {
                string path = "root/signal1";

                GivenASignalSetupGetByPath(SignalWith(
                    id: 1,
                    dataType: Domain.DataType.Boolean,
                    granularity: Domain.Granularity.Day,
                    path: Domain.Path.FromString((path))));

                var pathDto = new Dto.Path() { Components = new[] { "root", "signal1" } };

                var result = signalsWebService.Get(pathDto);

                SetupVerifyOrAssert();
                verifyOrAssert.GettingByPathAssertion(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByFalsePath_ReturnNull()
            {
                string path = "root/signal1";

                GivenASignalSetupGetByPath(SignalWith(
                    id: 1,
                    dataType: Domain.DataType.Boolean,
                    granularity: Domain.Granularity.Day,
                    path: Domain.Path.FromString((path))));
                
                Assert.IsNull(signalsWebService.Get(new Dto.Path() { Components = new[] { "bad", "path" } }));
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicy_RepositorySetAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                SetupSettingMissingValueTest(existingSignal);

                var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
                signalsWebService.SetMissingValuePolicy(1, policy);

                SetupVerifyOrAssert();
                verifyOrAssert.VerifyRepositorySetAndGetIsCalled(signalsRepositoryMock, missingValuePolicyRepositoryMock);
            }


            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicyForSpecificSignal_RepositorySetAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                SetupSettingMissingValueTest(existingSignal);

                var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
                signalsWebService.SetMissingValuePolicy(1, policy);

                SetupVerifyOrAssert();
                verifyOrAssert.VerifyRepositorySetAndGetIsCalled(existingSignal, signalsRepositoryMock, missingValuePolicyRepositoryMock);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicyForNonExistingSignal_ThrowsException()
            {
                var existingSignal = ExistingSignal();

                SetupSettingMissingValueTest(existingSignal);

                SetupVerifyOrAssert();
                verifyOrAssert.AssertSetMissingValuePolicyIsExceptionThrownWhenInvalidKey(signalsWebService);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingSpecificMissingValuePolicyForSpecificSignal_RepositorySetAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                SetupSettingMissingValueTest(existingSignal);

                var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
                {
                    DataType = Dto.DataType.Double,
                    Quality = Dto.Quality.Fair,
                    Value = (double)1.5
                };

                signalsWebService.SetMissingValuePolicy(1, policy);

                SetupVerifyOrAssert();
                verifyOrAssert.VerifyRepositorySetAndGetIsCalled(
                    existingSignal,
                    policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>(),
                    signalsRepositoryMock, missingValuePolicyRepositoryMock);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingMissingValuePolicy_RepositoryGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(existingSignal);

                signalsWebService.GetMissingValuePolicy(existingSignal.Id.Value);

                SetupVerifyOrAssert();
                verifyOrAssert.VerifyRepositoryGetIsCalled(missingValuePolicyRepositoryMock, existingSignal);
            }

            [TestMethod]
            public void GivenAMissingValuePolicy_WhenGettingMissingValuePolicy_ReturnsThisPolicy()
            {
                var existingSignal = ExistingSignal();

                var existingPolicy = ExistingPolicy();

                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(existingSignal, existingPolicy);

                var result = signalsWebService.GetMissingValuePolicy(existingSignal.Id.Value).ToDomain<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>();


                SetupVerifyOrAssert();
                verifyOrAssert.AssertGetIsReturningSpecificPolicy(existingPolicy, result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingMissingValuePolicyForNonExistingSignal_ThrowsException()
            {
                var existingSignal = ExistingSignal();

                var existingPolicy = ExistingPolicy();

                int wrongId = 3;

                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(existingSignal, existingPolicy, existingSignal.Id.Value);

                SetupVerifyOrAssert();
                verifyOrAssert.AssertGetMissingValuePolicyIsExceptionThrownWhenInvalidKey(signalsWebService, wrongId);
            }

            [TestMethod]
            public void GivenASignalAndDatum_WhenSettingData_RepositorySetDataAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                var existingDatum = ExistingDatum();

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<double>(It.IsAny<IEnumerable<Datum<double>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum);

                signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>();
                int index = 0;

                foreach (var ed in datum)
                {
                    signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<double>(It.Is<IEnumerable<Datum<double>>>(d =>
                    (
                        d.ElementAt(index).Quality == ed.Quality
                        && d.ElementAt(index).Timestamp == ed.Timestamp
                        && d.ElementAt(index).Value == ed.Value
                    ))));
                    index++;
                }
            }

            [TestMethod]
            public void GivenASignalAndIntDatum_WhenSettingData_RepositorySetDataAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (int)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (int)2 }
                };

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<int>(It.IsAny<IEnumerable<Datum<int>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum);

                signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>();
                int index = 0;

                foreach (var ed in datum)
                {
                    signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<int>(It.Is<IEnumerable<Datum<int>>>(d =>
                    (
                        d.ElementAt(index).Quality == ed.Quality
                        && d.ElementAt(index).Timestamp == ed.Timestamp
                        && d.ElementAt(index).Value == ed.Value
                    ))));
                    index++;
                }
            }

            [TestMethod]
            public void GivenASignalAndBoolDatum_WhenSettingData_RepositorySetDataAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (bool)true },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (bool)false },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (bool)false }
                };

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<bool>(It.IsAny<IEnumerable<Datum<bool>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum);

                signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<bool>>>();
                int index = 0;

                foreach (var ed in datum)
                {
                    signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<bool>(It.Is<IEnumerable<Datum<bool>>>(d =>
                    (
                        d.ElementAt(index).Quality == ed.Quality
                        && d.ElementAt(index).Timestamp == ed.Timestamp
                        && d.ElementAt(index).Value == ed.Value
                    ))));
                    index++;
                }
            }

            [TestMethod]
            public void GivenASignalAndStringDatum_WhenSettingData_RepositorySetDataAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (string)"tak" },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (string)"tak" },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (string)"nie" }
                };

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<string>(It.IsAny<IEnumerable<Datum<string>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum);

                signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<string>>>();
                int index = 0;

                foreach (var ed in datum)
                {
                    signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<string>(It.Is<IEnumerable<Datum<string>>>(d =>
                    (
                        d.ElementAt(index).Quality == ed.Quality
                        && d.ElementAt(index).Timestamp == ed.Timestamp
                        && d.ElementAt(index).Value == ed.Value
                    ))));
                    index++;
                }
            }

            [TestMethod]
            public void GivenASignalAndDecimalDatum_WhenSettingData_RepositorySetDataAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (decimal)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (decimal)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (decimal)2 }
                };

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<decimal>(It.IsAny<IEnumerable<Datum<decimal>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum);

                signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<decimal>>>();
                int index = 0;

                foreach (var ed in datum)
                {
                    signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<decimal>(It.Is<IEnumerable<Datum<decimal>>>(d =>
                    (
                        d.ElementAt(index).Quality == ed.Quality
                        && d.ElementAt(index).Timestamp == ed.Timestamp
                        && d.ElementAt(index).Value == ed.Value
                    ))));
                    index++;
                }
            }


            [TestMethod]
            public void GivenASignalAndStringDatym_WhenSettingData_RepositorySetDataAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();
                var value1 = "a";
                var value2 = "b";
                var value3 = "c";
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = value1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = value2 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = value3 }
                };

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<string>(It.IsAny<IEnumerable<Datum<string>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum);

                signalsRepositoryMock.Verify(srm => srm.Get(existingSignal.Id.Value));

                var datum = existingDatum.ToDomain<IEnumerable<Domain.Datum<string>>>();
                int index = 0;

                foreach (var ed in datum)
                {
                    signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<string>(It.Is<IEnumerable<Datum<string>>>(d =>
                    (
                        d.ElementAt(index).Quality == ed.Quality
                        && d.ElementAt(index).Timestamp == ed.Timestamp
                        && d.ElementAt(index).Value == ed.Value
                    ))));
                    index++;
                }
            }

            [TestMethod]
            public void GivenASignal_WhenAddingSingal_ThenSettingNoneQualityMissingValuePolicy()
            {
                GivenASignalSetupAddSignal();
                var sig = signalsWebService.Add(new Dto.Signal()
                {
                    DataType = Dto.DataType.Double,
                    Granularity = Dto.Granularity.Month,
                    Path = new Dto.Path() { Components = new[] { "root", "signal" } }
                });
                missingValuePolicyRepositoryMock.Verify(mvpr => mvpr.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>()));
            }

            [TestMethod]
            public void GivenASignalAndDatum_WhenSettingDataWithWrongSignalId_ExceptionIsThrown()
            {
                var existingSignal = ExistingSignal();
                int wrongSignalId = 3;

                SetupSettingData(existingSignal);

                SetupVerifyOrAssert();
                verifyOrAssert.AssertSetDataIsExceptionThrownWhenInvalidKey(signalsWebService, wrongSignalId);
            }

            

            private void SetupSignalsDataRepositoryAndSignalsRepository(Domain.Signal existingSignal)
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()));

                GivenASignal(existingSignal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingSpecificDataForSpecificSignal_RepositoryGetDataAndGetIsCalled()
            {
                var existingSignal = ExistingSignal();

                var existingDatum = ExistingDatum();

                SetupSignalsDataRepositoryAndSignalsRepository(existingSignal, existingDatum);
                var result = signalsWebService.GetData(existingSignal.Id.Value, existingDatum.First().Timestamp, existingDatum.Last().Timestamp);
                
                SetupVerifyOrAssert();
                verifyOrAssert.VerifyRepositoryGetDataAndGetIsCalled(existingSignal, existingDatum, signalsDataRepositoryMock, signalsRepositoryMock);
            }

            private void SetupSignalsDataRepositoryAndSignalsRepository(Domain.Signal existingSignal, Datum[] existingDatum)
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(
                        existingSignal,
                        existingDatum.First().Timestamp,
                        existingDatum.Last().Timestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>());

                GivenASignal(existingSignal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingSpecificDataForSpecificSignal_ReturnsThisData()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Double,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("root/signal1")
                };
                var existingDatum = new Dto.Datum[]
                {

                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2), Value = (double)1.5 },
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 3), Value = (double)2 }
                };
                SetupSignalsDataRepositoryAndSignalsRepository(existingSignal, existingDatum);
                
                var result = signalsWebService.GetData(existingSignal.Id.Value, existingDatum.First().Timestamp, existingDatum.Last().Timestamp);
                SetupVerifyOrAssert();
                verifyOrAssert.AssertGettingSpecificDataForSpecificSignalReturnsThisData(existingDatum, result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingDataForSignalWithWrongId_ThrowsException()
            {
                var wrongSignalId = 3;

                var existingSignal = ExistingSignal();

                SetupSignalsDataRepositoryAndSignalsRepository(existingSignal);
                
                SetupVerifyOrAssert();
                verifyOrAssert.AssertGetDataExceptionIsThrownWhenInvalidKey(signalsWebService, wrongSignalId);
            }



            [TestMethod]
            public void GivenASignal_WhenGettingNoSortedList_ReturnSortedList()
            {

                var existingSignal = ExistingSignal();

                var existingDatum = ExistingDatumToSorted();

                SetupSignalsDataRepositoryAndSignalsRepository(existingSignal, existingDatum);


                var result = signalsWebService.GetData(existingSignal.Id.Value, existingDatum.First().Timestamp, existingDatum.Last().Timestamp);

                var existingSortedDatum = existingDatum.OrderBy(x => x.Timestamp);
                
                for (int i=0;i<result.Count();i++)
                {
                    Assert.AreEqual(existingSortedDatum.ElementAt(i).Timestamp, result.ElementAt(i).Timestamp);
                }
            }

            [TestMethod]
            public void GivenASignalAndDatumWithGranularityMonth_WhenGettingData_ReturnListWithElementsNone()
            {
                SetupGivenASignalAndatumWithGranularity(Domain.Granularity.Month,
                    new DateTime[] { new DateTime(2000, 1, 1), new DateTime(2000, 3, 1) , new DateTime(2000, 4, 1) },
                    new DateTime[] { new DateTime(2000, 1, 1),new DateTime(2000, 2, 1) ,new DateTime(2000, 3, 1) });
            }

            [TestMethod]
            public void GivenASignalAndDatumWithGranularityDays_WhenGettingData_ReturnListWithElementsNone()
            {
                SetupGivenASignalAndatumWithGranularity(Domain.Granularity.Day,
                    new DateTime[] { new DateTime(2000, 1, 1), new DateTime(2000, 1, 3), new DateTime(2000, 1, 4) },
                    new DateTime[] { new DateTime(2000, 1, 1), new DateTime(2000, 1, 2), new DateTime(2000, 1, 3) });
            }
            

            [TestMethod]
            public void GivenASignalAndatumWithGranularityYear_WhenGettingData_ReturnListWithelementsNone()
            {
                SetupGivenASignalAndatumWithGranularity(Domain.Granularity.Year,
                    new DateTime[] { new DateTime(2001, 1, 1), new DateTime(2003, 1, 1), new DateTime(2004, 1, 1) },
                    new DateTime[] { new DateTime(2001, 1, 1), new DateTime(2002, 1, 1), new DateTime(2003, 1, 1) });
            }


            [TestMethod]
            public void GivenASignalAndatumWithGranularityHour_WhenGettingData_ReturnListWithelementsNone()
            {
                SetupGivenASignalAndatumWithGranularity(Domain.Granularity.Hour,
                    new DateTime[] { new DateTime(2000, 1, 1, 4, 0, 0), new DateTime(2000, 1, 1, 5, 0, 0), new DateTime(2000, 1, 1, 7, 0, 0) },
                    new DateTime[] { new DateTime(2000, 1, 1, 4, 0, 0), new DateTime(2000, 1, 1, 5, 0, 0), new DateTime(2000, 1, 1, 6, 0, 0) });
            }

            [TestMethod]
            public void GivenASignalAndatumWithGranularityMinute_WhenGettingData_ReturnListWithelementsNone()
            {
                SetupGivenASignalAndatumWithGranularity(Domain.Granularity.Minute,
                    new DateTime[] { new DateTime(2000, 1, 1, 4, 1, 0), new DateTime(2000, 1, 1, 4, 2, 0), new DateTime(2000, 1, 1, 4, 4, 0) },
                    new DateTime[] { new DateTime(2000, 1, 1, 4, 1, 0), new DateTime(2000, 1, 1, 4, 2, 0), new DateTime(2000, 1, 1, 4, 3, 0) });
            }


            [TestMethod]
            public void GivenASignalAndatumWithGranularitySecond_WhenGettingData_ReturnListWithelementsNone()
            {
                SetupGivenASignalAndatumWithGranularity(Domain.Granularity.Second,
                    new DateTime[] { new DateTime(2000, 1, 1, 4, 1, 1), new DateTime(2000, 1, 1, 4, 1, 2), new DateTime(2000, 1, 1, 4, 1, 4) },
                    new DateTime[] { new DateTime(2000, 1, 1, 4, 1, 1), new DateTime(2000, 1, 1, 4, 1, 2), new DateTime(2000, 1, 1, 4, 1, 3) });
            }

            [TestMethod]
            public void GivenASignalAndatumWithGranularityWeek_WhenGettingData_ReturnListWithelementsNone()
            {
                SetupGivenASignalAndatumWithGranularity(Domain.Granularity.Week,
                    new DateTime[] { new DateTime(2016, 8, 1), new DateTime(2016, 8, 8), new DateTime(2016, 8, 22) }, 
                    new DateTime[] { new DateTime(2016, 8, 1), new DateTime(2016, 8, 8), new DateTime(2016, 8, 15) });
                
            }

            [TestMethod]
            public void GivenASignalWithEmptyDatum_WhenSet_ReturnNull()
            {
                Domain.Signal signal = this.SignalWith(1, Domain.DataType.Double, Domain.Granularity.Minute, Domain.Path.FromString("x/y"));

                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(signal);

                signalsWebService.SetData(1, new List<Datum>());
            }

            [TestMethod]
            public void GiveenASignalWithNullValueDatum_WhenSet_AddData()
            {
                Domain.Signal signal = this.SignalWith(1, Domain.DataType.String, Domain.Granularity.Minute, Domain.Path.FromString("x/y"));

                SetupSettingData(signal);

                signalsWebService.SetData(1, new List<Datum>()
                {
                    new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000,1,1,2,0,0), Value = null }
                });

                signalsDataRepositoryMock.Verify(x => x.SetData<string>(
                    It.IsAny<IEnumerable<Datum<string>>>()));
            }

            [TestMethod]
            public void GivenASignalWithDatum_WhenSet_SignalIdIsNotNull()
            {
                Domain.Signal signal = this.SignalWith(1, Domain.DataType.String, Domain.Granularity.Minute, Domain.Path.FromString("x/y"));

                SetupSettingData(signal);

                signalsWebService.SetData(1, new List<Datum>()
                {
                    new Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000,1,1,1,1,0), Value = null }
                });

                signalsDataRepositoryMock.Verify(x => x.SetData<string>(
                    It.Is<IEnumerable<Datum<string>>>(z => z.First().Signal != null)));

            }

            [TestMethod]
            public void GivenASignalWithTimeRange_WhenGet_ReturnFilledCollection()
            {
                var signal = SignalWith(1, Domain.DataType.Double, Domain.Granularity.Second, Domain.Path.FromString("x/z"));

                SetupSignalsDataRepositoryAndSignalsRepository(signal);
                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.IsAny<Domain.Signal>())).Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
                signalsDataRepositoryMock.Setup(x => x.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(new List<Datum<double>>());
                var collection = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1, 0, 1, 0));

                Assert.AreEqual(collection.Count(), 60);

            }
            
            [TestMethod]
            public void GivenNullPath_WhenGetPathEntry_ReturnNull()
            {
                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(SignalWith(1, Domain.DataType.Double, Domain.Granularity.Second, Domain.Path.FromString("z/y")));

                var items = signalsWebService.GetPathEntry(null);

                Assert.IsNull(items);
            }

            [TestMethod]
            public void GivenEmptyPath_WhenGetPathEntry_ReturnEmptyPathEntry()
            {
                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(SignalWith(1, Domain.DataType.Double, Domain.Granularity.Second, Domain.Path.FromString("z/y")));

                var items = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "a", "y" } });

                Assert.IsNotNull(items);
            }

            [TestMethod]
            public void GivenAPathEntry_WhenGetPathEntry_RepositoryGetPrefixIsCaled()
            {
                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(SignalWith(1, Domain.DataType.Double, Domain.Granularity.Second, Domain.Path.FromString("z/y")));

                var items = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "a", "y" } });

                signalsRepositoryMock.Verify(x => x.GetAllWithPathPrefix(It.IsAny<Domain.Path>()));
            }

            [TestMethod]
            public void GivenAPathEntryOfSignal_WhenGetPathEntry_GetSignal()
            {
                var path = Domain.Path.FromString("z/y");


                var signal = SignalWith(1, Domain.DataType.Double, Domain.Granularity.Second, path);

                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(signal);

                signalsRepositoryMock.Setup(x => x.GetAllWithPathPrefix(It.IsAny<Domain.Path>()))
                    .Returns(new List<Domain.Signal>()
                {
                    signal
                });

                var item = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "z" } });

                var dbitem = item.Signals.First();

                Assert.AreEqual(dbitem.Id, signal.Id);
            }

            [TestMethod]
            public void GiveAPathEntryOfSignal_WhenGetPathEntry_ReturnAllSignalsOfThatEntry()
            {
                var path = Domain.Path.FromString("root");

                var signal1 = SignalWith(1, Domain.DataType.Double, Domain.Granularity.Second, Domain.Path.FromString("root/signal1"));
                var signal2 = SignalWith(2, Domain.DataType.Double, Domain.Granularity.Second, Domain.Path.FromString("root/signal2"));

                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(signal1);

                signalsRepositoryMock.Setup(x => x.GetAllWithPathPrefix(It.IsAny<Domain.Path>()))
                    .Returns(new List<Domain.Signal>()
                    {
                        signal1,
                        signal2
                    });

                var item = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root" } });

                Assert.AreEqual(item.Signals.Count(), 2);
            }

            [TestMethod]
            public void GivenAPathEntryOfSignals_WhenGetPathEntry_ReturnSignalsInSameDirectory()
            {
                var path = Domain.Path.FromString("root");

                var signal1 = SignalWith(1, Domain.DataType.Double, Domain.Granularity.Second, Domain.Path.FromString("root/signal1"));
                var signal2 = SignalWith(2, Domain.DataType.Double, Domain.Granularity.Second, Domain.Path.FromString("root/signal2"));
                var signal3 = SignalWith(2, Domain.DataType.Double, Domain.Granularity.Second, Domain.Path.FromString("root/subdirectory/signal3"));

                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(signal1);

                signalsRepositoryMock.Setup(x => x.GetAllWithPathPrefix(It.IsAny<Domain.Path>()))
                    .Returns(new List<Domain.Signal>()
                    {
                        signal1,
                        signal2,
                        signal3
                    });

                var item = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root" } });

                Assert.AreEqual(item.Signals.Count(), 2);
            }

            [TestMethod]
            public void GivenAPathEntryOfSignals_WhenGetPathEntry_ReturnSubdirectory()
            {
                var path = Domain.Path.FromString("root");

                var signal1 = SignalWith(1, Domain.DataType.Double, Domain.Granularity.Second, Domain.Path.FromString("root/signal1"));
                var signal2 = SignalWith(2, Domain.DataType.Double, Domain.Granularity.Second, Domain.Path.FromString("root/signal2"));
                var signal3 = SignalWith(2, Domain.DataType.Double, Domain.Granularity.Second, Domain.Path.FromString("root/subdirectory/signal3"));

                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(signal1);

                signalsRepositoryMock.Setup(x => x.GetAllWithPathPrefix(It.IsAny<Domain.Path>()))
                    .Returns(new List<Domain.Signal>()
                    {
                        signal1,
                        signal2,
                        signal3
                    });

                var item = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root" } });

                Assert.AreEqual(item.SubPaths.Count(), 1);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingData_SpecificValueMissingValuePolicyFill()
            {
                var signal1 = SignalWith(1, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("root/signal1"));

                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(signal1);

                Mock<SpecificValueMissingValuePolicy<double>> specificMvpMock = new Mock<SpecificValueMissingValuePolicy<double>>();
                specificMvpMock.Object.Value = 42.52;

                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.Is<Domain.Signal>(z => z.Id == 1)))
                    .Returns(specificMvpMock.Object);

                signalsDataRepositoryMock.Setup(x => x.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(new Datum<double>[]
                        {
                            new Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                            new Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
                        });

                var items = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                Assert.AreEqual(items.Count(), 3);
                Assert.AreEqual(items.Single(z => z.Timestamp == new DateTime(2000, 2, 1)).Value, specificMvpMock.Object.Value);

            }

            [TestMethod]
            public void SpecificValueMissingValuePolicy_WhenGetData_FillInt()
            {
                int value = 4;
                var signal1 = SignalWith(1, Domain.DataType.Integer, Domain.Granularity.Month, Domain.Path.FromString("root/signal1"));

                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(signal1);

                Mock<SpecificValueMissingValuePolicy<int>> specificMvpMock = new Mock<SpecificValueMissingValuePolicy<int>>();
                specificMvpMock.Object.Value = value;

                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.Is<Domain.Signal>(z => z.Id == 1)))
                    .Returns(specificMvpMock.Object);

                signalsDataRepositoryMock.Setup(x => x.GetData<int>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(DetDefaultDatumCollection<int>(new DateTime(2000, 1, 1), new DateTime(2000, 4, 1)));

                var items = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));

                Assert.AreEqual(items.Count(), 3);
                Assert.AreEqual(items.Single(z => z.Timestamp == new DateTime(2000, 2, 1)).Value, specificMvpMock.Object.Value);
            }

            [TestMethod]
            public void NoneQualityMissingValuePolicy_WhenGettingData_FillDatumWithNoneQuality_WhenSignalDataIsEmpty()
            {
                var signal = new Domain.Signal() { Id = 1, DataType = Domain.DataType.Integer };
                SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(signal);

                Mock<NoneQualityMissingValuePolicy<int>> noneMvpMock = new Mock<NoneQualityMissingValuePolicy<int>>();

                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.Is<Domain.Signal>(z => z.Id == 1)))
                    .Returns(noneMvpMock.Object);

                signalsDataRepositoryMock.Setup(x => x.GetData<int>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns(DefaultElementOfCollection<int>(new DateTime(2018, 11, 11)));

                var item = signalsWebService.GetData(1, new DateTime(2018, 11, 11), new DateTime(2018, 11, 11));

                Assert.AreEqual(item.Count(), 1);
                Assert.AreEqual(item.First(f => f.Timestamp == new DateTime(2018, 11, 11)).Quality, noneMvpMock.Object.Quality);
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SignalDoesNotExists))]
            public void GivenNoSignal_WhenDeletingNotExistingSignal_ThrowsException()
            {
                GivenNoSignals();

                signalsWebService.Delete(316);
            }

            [TestMethod]
            public void GivenASignal_WhenDeletingExistingSignal_DataRepositoryDeleteIsCalled()
            {
                SetupMockDeleteFunction();
                
                signalsWebService.Delete(1);

                signalsRepositoryMock.Verify(x => x.Delete(It.IsAny<Domain.Signal>()));
            }

            private void SetupMockDeleteFunction()
            {
                var signalToDelete = new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Boolean,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("example/path")
                };

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.DeleteData<bool>(It.IsAny<Domain.Signal>()));

                GivenASignal(signalToDelete);
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock.Setup(mvp => mvp.Set(It.IsAny<Domain.Signal>(), null));

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private List<Datum<T>> DetDefaultDatumCollection<T>(DateTime startDate, DateTime endDate)
            {
                List<Datum<T>> ListOfitems = new List<Datum<T>>();
                ListOfitems.Add(new Datum<T>()
                {
                    Id = 1,
                    Quality = Domain.Quality.Fair,
                    Timestamp = startDate,
                    Value = default(T)
                });
                ListOfitems.Add(new Datum<T>()
                {
                    Id = 2,
                    Quality = Domain.Quality.Fair,
                    Timestamp = endDate,
                    Value = default(T)
                });
                return ListOfitems;
            }

            private List<Datum<T>> DefaultElementOfCollection<T>(DateTime startDate)
            {
                List<Datum<T>> SingleItem = new List<Datum<T>>();
                SingleItem.Add(new Datum<T>()
                {
                    Id = 1,
                    Quality = Domain.Quality.None,
                    Timestamp = startDate,
                    Value = default(T)
                });
                return SingleItem;
            }

            private void SetupGivenASignalAndatumWithGranularity(Domain.Granularity granulity, DateTime[] existingListDatum, DateTime[] expectedListDatum)
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1111,
                    DataType = Domain.DataType.Double,
                    Granularity = granulity,
                    Path = Domain.Path.FromString("root/signall")
                };
                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = existingListDatum[0], Value = (double)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = existingListDatum[1], Value = (double)2.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = existingListDatum[2], Value = (double)3.1 }
                };
                SetupSignalsDataRepositoryAndSignalsRepository(existingSignal, existingDatum);

                missingValuePolicyRepositoryMock.Setup(x => x.Get(It.IsAny<Domain.Signal>())).Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var result = signalsWebService.GetData(existingSignal.Id.Value, existingDatum.First().Timestamp, existingDatum.Last().Timestamp);

                var expectedDatum = new Dto.Datum[]
                {
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = expectedListDatum[0], Value = (double)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = expectedListDatum[1], Value = 0 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = expectedListDatum[2], Value = (double)2.5 }
                };
                for (int i = 0; i < expectedDatum.Length; i++)
                {
                    Assert.AreEqual(expectedDatum.ElementAt(i).Timestamp, result.ElementAt(i).Timestamp);
                }
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

            private void GivenASignal(Domain.Signal existingSignal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);
            }
            
            private void GivenASignalSetupGetByPath(Domain.Signal existingSignal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(srm => srm
                    .Get(existingSignal.Path))
                    .Returns(existingSignal);
            }
            
            private void GivenASignalSetupAddSignal()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
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

            private void SetupSettingMissingValueTest(Domain.Signal existingSignal)
            {
                SetupDefault();

                GivenASignal(SignalWith(
                        existingSignal.Id.Value,
                        existingSignal.DataType,
                        existingSignal.Granularity,
                        existingSignal.Path));
                
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void SetupDefault()
            {
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

            }

            private Domain.Signal ExistingSignal()
            {
                return new Domain.Signal()
                {
                    Id = 1,
                    DataType = Domain.DataType.Double,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("root/signal1")
                };
            }

            private DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble ExistingPolicy()
            {
                return new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                {
                    Id = 1,
                    Quality = Domain.Quality.Bad,
                    Value = (double)1.5
                };
            }
            
            private Dto.Datum[] ExistingDatum()
            {
                return new Dto.Datum[]
                {

                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 }
                };
            }
            private Dto.Datum[] ExistingDatumToSorted()
            {
                return new Dto.Datum[]
                {

                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 }
                };
            }

            private void SetupVerifyOrAssert()
            {
                verifyOrAssert = new VerifyOrAssertTestResults();
            }

            private void SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(Domain.Signal existingSignal)
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble());

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(Domain.Signal existingSignal,
                SpecificValueMissingValuePolicyDouble existingPolicy)
            {
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                    .Returns(existingPolicy);

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void SetupMissingValuePolicyRepositoryMockAndSignalsRepositoryMock(Domain.Signal existingSignal,
                SpecificValueMissingValuePolicyDouble existingPolicy, int signalId)
            {
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(mvp => mvp.Get(It.IsAny<Domain.Signal>()))
                    .Returns(existingPolicy);

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(signalId))
                    .Returns(existingSignal);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void SetupSettingData(Domain.Signal existingSignal)
            {
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<double>(It.IsAny<IEnumerable<Datum<double>>>()));

                GivenASignal(existingSignal);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
        }
    }
}