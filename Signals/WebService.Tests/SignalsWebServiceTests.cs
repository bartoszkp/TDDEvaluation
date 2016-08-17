using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Domain.Exceptions;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;
            private SignalsDomainService signalDomainService;

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
            private Mock<Domain.MissingValuePolicy.MissingValuePolicyBase> policyMock;

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
            public void GivenNoSignals_WhenGettingByIdWithNullValue_ReturnsNull()
            {

                GivenNoSignals();

                signalsWebService = new SignalsWebService(signalDomainService);
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


            [TestMethod]
            public void GivenASignal_WhenSettingASignalsData_RepositorySetDataIsCalled()
            {
                MockSetup();

                Datum<double>[] dataToSet = new Datum<double>[] {
                        new Datum<double>() { Id = 1, Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Datum<double>() { Id = 2, Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = (double)2 },
                        new Datum<double>() { Id = 3, Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)3 },
                        };

                signalDomainService.SetData(1, dataToSet);
                signalsDataRepositoryMock.Verify(sr => sr.SetData(dataToSet));
            }

            private void MockSetup()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SettingNotExistingSignalDataException))]
            public void GivenASignal_WhenSettingDataForNotExistingSignal_ThrowsNotExistingSignalException()
            {
                MockSetup();
                signalsWebService = new SignalsWebService(signalDomainService);

                Datum<double>[] dataToSet = new Datum<double>[] {
                        new Datum<double>() { Id = 1, Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Datum<double>() { Id = 2, Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = (double)2 },
                        new Datum<double>() { Id = 3, Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)3 },
                        };

                Dto.Datum[] DtoDataToSet = new Dto.Datum[] {
                        new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = (double)2 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)3 },
                        };

                signalsWebService.SetData(100, DtoDataToSet);
                signalDomainService.SetData(1, dataToSet);
                signalsDataRepositoryMock.Verify(sr => sr.SetData(dataToSet));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.GettingDataOfNotExistingSignal))]
            public void GivenASignal_WhenGettingDataForNotExistingSignal_ThrowsGettingDataOfNotExistingSignal()
            {
                MockSetup();
                signalsWebService = new SignalsWebService(signalDomainService);

                var fromIncludedDate = new DateTime(2016, 8, 1);
                var toExcludedDate = new DateTime(2016, 8, 4);

                var result = signalsWebService.GetData(1, fromIncludedDate, toExcludedDate);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingData_ReturnsCorrectData()
            {
                MockSetup();

                List<Domain.Datum<int>> data = new List<Domain.Datum<int>>();
                SetupData(data);

                Signal signal = new Signal()
                {
                    
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("example/path"),
                };

                var fromIncludedDate = new DateTime(2016, 8, 1);
                var toExcludedDate = new DateTime(2016, 8, 4);

                signalsDataRepositoryMock
                    .Setup(srm => srm.GetData<int>(signal, fromIncludedDate, toExcludedDate))
                    .Returns(data);

                GivenRepositoryThatAssigns(1);
                var resultSignal = signalDomainService.Add(signal);

                signalsRepositoryMock
                    .Setup(srm => srm.Get(resultSignal.Id.Value))
                    .Returns(signal);

                signalDomainService.SetData(1, data.AsEnumerable());
                data.RemoveAt(2);

                var result = signalDomainService.GetData<int>(resultSignal.Id.Value, fromIncludedDate, toExcludedDate);

                CollectionAssert.AreEqual(data, result.ToList<Datum<int>>());
            }

            [TestMethod]
            public void GivenASignal_WhenGettingData_ReturnsCorrectDataWchihIsSorted()
            {
                MockSetup();

                List<Domain.Datum<int>> data = new List<Domain.Datum<int>>();

                SetupData(data);

                Signal signal = new Signal()
                {
                    DataType = Domain.DataType.Integer,
                    Granularity = Domain.Granularity.Day,
                    Path = Domain.Path.FromString("example/path"),
                };

                var sorted = data.OrderBy(d => d.Timestamp).ToList();

                var fromIncludedDate = new DateTime(2000, 1, 2);
                var toExcludedDate = new DateTime(2000, 5, 4);

                signalsDataRepositoryMock
                    .Setup(srm => srm.GetData<int>(signal, fromIncludedDate, toExcludedDate))
                    .Returns(sorted);

                GivenRepositoryThatAssigns(1);
                var resultSignal = signalDomainService.Add(signal);

                signalsRepositoryMock
                    .Setup(srm => srm.Get(resultSignal.Id.Value))
                    .Returns(signal);

                signalDomainService.SetData(1, data.AsEnumerable());
                data.RemoveAt(2);
                sorted.RemoveAt(2);

                var result = signalDomainService.GetData<int>(resultSignal.Id.Value, fromIncludedDate, toExcludedDate);

                CollectionAssert.AreEqual(sorted, result.ToList<Datum<int>>());
            }

            private void SetupData(List<Domain.Datum<int>> data)
            {
                data.Add(new Domain.Datum<int>()
                {
                    Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2000, 4, 4),
                    Value = 12,
                });

                data.Add(new Domain.Datum<int>()
                {
                    Quality = Domain.Quality.Good,
                    Timestamp = new DateTime(2000, 3, 3),
                    Value = 10,
                });

                data.Add(new Domain.Datum<int>()
                {
                    Quality = Domain.Quality.Poor,
                    Timestamp = new DateTime(2000, 2, 2),
                    Value = 14,
                });
            }

            [TestMethod]
            public void GivenNoSignal_WhenGettingByPath_ReturnsNull()
            {
                MockSetup();

                signalsWebService = new SignalsWebService(signalDomainService);
                var result = signalsWebService.Get(new Dto.Path()
                {
                    Components = new[] { "not/existing/path" },
                });

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByPath_ReturnsCorrectSignal()
            {
                Dto.Path dtoPath = new Dto.Path() { Components = new[] { "example", "path" } };

                GetByPathSetup();
                var returndSignal = signalsWebService.Get(dtoPath);

                Assert.AreEqual(1, returndSignal.Id.Value);
                Assert.AreEqual(Dto.DataType.Boolean, returndSignal.DataType);
                Assert.AreEqual(Dto.Granularity.Day, returndSignal.Granularity);
                CollectionAssert.AreEqual(dtoPath.Components.ToArray(), returndSignal.Path.Components.ToArray());
            }

            private void GetByPathSetup()
            {
                var signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(Domain.Path.FromString("example/path")))
                    .Returns(new Signal()
                    {
                        Id = 1,
                        DataType = Domain.DataType.Boolean,
                        Granularity = Domain.Granularity.Day,
                        Path = Domain.Path.FromString("example/path"),
                    });

                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);
                signalsWebService = new SignalsWebService(signalDomainService);
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SettingPolicyNotExistingSignalException))]
            public void WhenCallingSetPolicy_ProperMethodIsCalled()
            {
                Mock<Dto.MissingValuePolicy.MissingValuePolicy> policyMock = new Mock<Dto.MissingValuePolicy.MissingValuePolicy>();

                Mock<ISignalsWebService> signalWebServiceMock = new Mock<ISignalsWebService>();
                signalWebServiceMock
                    .Setup(swsm => swsm.SetMissingValuePolicy(1, policyMock.Object));

                MockSetup();
                signalsWebService = new SignalsWebService(signalDomainService);
                signalsWebService.SetMissingValuePolicy(1, policyMock.Object);

                signalWebServiceMock.Verify(swsm => swsm.SetMissingValuePolicy(1, policyMock.Object));
            }

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.SettingPolicyNotExistingSignalException))]
            public void GivenNoSignal_WhenSetPolicyIsCalled_ItThrowsException()
            {
                Mock<Dto.MissingValuePolicy.MissingValuePolicy> policyMock = new Mock<Dto.MissingValuePolicy.MissingValuePolicy>();
                SetupMissingValuePolicy(policyMock);

                Mock<ISignalsWebService> signalWebServiceMock = new Mock<ISignalsWebService>();
                signalWebServiceMock
                    .Setup(swsm => swsm.SetMissingValuePolicy(1, policyMock.Object));

                MockSetup();
                signalsWebService = new SignalsWebService(signalDomainService);
                signalsWebService.SetMissingValuePolicy(1, policyMock.Object);

                signalWebServiceMock.Verify(swsm => swsm.SetMissingValuePolicy(1, policyMock.Object));
            }

            [TestMethod]
            public void GivenASignalAndPolicy_WhenSettingMissingValuePolicy_RepositorySetIsCalledWithProperArguments()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Day
                };

                var existingPolicy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
                {
                    DataType = Dto.DataType.Double,
                    Quality = Dto.Quality.Bad,
                    Value = (double)1.5
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                signalsRepositoryMock
                    .Setup(srm => srm.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetMissingValuePolicy(existingSignal.Id.Value, existingPolicy);

                var domainExistingPolicy = (Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>)existingPolicy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();

                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.Is<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>(mv =>
                    (
                        mv.NativeDataType == domainExistingPolicy.NativeDataType
                        && mv.Quality == domainExistingPolicy.Quality
                        && mv.Value == domainExistingPolicy.Value
                    ))));
            }

            private void SetupMissingValuePolicy(Mock<Dto.MissingValuePolicy.MissingValuePolicy> policyMock)
            {
                policyMock.Object.DataType = Dto.DataType.Boolean;
                policyMock.Object.Id = 1;
                policyMock.Object.Signal = new Dto.Signal()
                {
                    Id = 1,
                    DataType = Dto.DataType.Boolean,
                    Granularity = Dto.Granularity.Day,
                    Path = new Dto.Path() { Components = new[] { "aaa", "bbb" } },
                };
            }

            private void SetupMissingValuePolicyForNoneQuality(Mock<Dto.MissingValuePolicy.NoneQualityMissingValuePolicy> policyMock)
            {
                policyMock.Object.DataType = Dto.DataType.Double;
                policyMock.Object.Id = 2;
                policyMock.Object.Signal = new Dto.Signal()
                {
                    Id = 2,
                    DataType = Dto.DataType.Double,
                    Granularity = Dto.Granularity.Day,
                    Path = new Dto.Path() { Components = new[] { "aaa", "bbb" } },
                };
            }

            private Signal SetupPolicyMock()
            {
                var exampleSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("example/path"),
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(It.IsAny<int>()))
                    .Returns(new Signal());

                Mock<Dto.MissingValuePolicy.MissingValuePolicy> dtoPolicyMock = new Mock<Dto.MissingValuePolicy.MissingValuePolicy>();
                SetupMissingValuePolicy(dtoPolicyMock);

                policyMock = new Mock<Domain.MissingValuePolicy.MissingValuePolicyBase>();

                Mock<Domain.Services.ISignalsDomainService> signalDomainServiceMock = new Mock<Domain.Services.ISignalsDomainService>();
                signalDomainServiceMock
                    .Setup(sdsm => sdsm.SetMissingValuePolicy(exampleSignal, policyMock.Object));

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                   .Setup(mvprm => mvprm.Set(exampleSignal, policyMock.Object));

                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                return exampleSignal;
            }

            [TestMethod]
            public void GivenASignalAndPolicy_WhenGettingMissingValuePolicy_ReturnsThisPolicy()
            {
                var existingSignal = new Domain.Signal()
                {
                    Id = 2,
                    DataType = DataType.Double,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("example/path"),
                };

                var existingPolicy = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                {
                    Id = 1,
                    Quality = Domain.Quality.Bad,
                    Value = (double)1.5
                };

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
                var result = signalsWebService.GetMissingValuePolicy(existingSignal.Id.Value).ToDomain<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>();

                Assert.AreEqual(existingPolicy.Id, result.Id);
                Assert.AreEqual(existingPolicy.Quality, result.Quality);
                Assert.AreEqual(existingPolicy.Value, result.Value);
            }

            private Signal SetupGetPolicyMockForNoneQuality()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                var exampleSignal = new Domain.Signal()
                {
                    Id = 2,
                    DataType = DataType.Double,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("example/path"),
                };

                Mock<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>> policyMock = new Mock<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>();

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(sdsm => sdsm.Get(exampleSignal))
                    .Returns(policyMock.Object);

                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                return exampleSignal;
            }

            private Signal SetupPolicyMockForNoneQuality()
            {
                var exampleSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("example/path"),
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(srm => srm.Get(It.IsAny<int>()))
                    .Returns(new Signal());

                Mock<Dto.MissingValuePolicy.NoneQualityMissingValuePolicy> dtoPolicyMock = new Mock<Dto.MissingValuePolicy.NoneQualityMissingValuePolicy>();
                SetupMissingValuePolicyForNoneQuality(dtoPolicyMock);

                Mock<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>> policyMockForNoneQuality = new Mock<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>();

                Mock<Domain.Services.ISignalsDomainService> signalDomainServiceMock = new Mock<Domain.Services.ISignalsDomainService>();
                signalDomainServiceMock
                    .Setup(sdsm => sdsm.SetMissingValuePolicy(exampleSignal, policyMockForNoneQuality.Object));

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                   .Setup(mvprm => mvprm.Set(exampleSignal, policyMockForNoneQuality.Object));

                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                return exampleSignal;
            }

            private Signal SetupGetPolicyMock()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                var exampleSignal = new Domain.Signal()
                {
                    Id = 1,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("example/path"),
                };

                policyMock = new Mock<Domain.MissingValuePolicy.MissingValuePolicyBase>();

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                missingValuePolicyRepositoryMock
                    .Setup(sdsm => sdsm.Get(exampleSignal))
                    .Returns(policyMock.Object);

                signalDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                return exampleSignal;
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_NoneQualityMissingValuePolicyIsSetAsDeafult()
            {
                var existingSignal = new Domain.Signal()
                {
                    //Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Day
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                signalsRepositoryMock
                    .Setup(srm => srm.Add(It.IsAny<Domain.Signal>()))
                    .Returns(existingSignal);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>()));


                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Add(existingSignal.ToDto<Dto.Signal>());

                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>>()));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingAnIntSignal_NoneQualityMissingValuePolicyIsSetAsDeafult()
            {
                var existingSignal = new Domain.Signal()
                {
                    //Id = 1,
                    DataType = DataType.Integer,
                    Granularity = Granularity.Day
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                signalsRepositoryMock
                    .Setup(srm => srm.Add(It.IsAny<Domain.Signal>()))
                    .Returns(existingSignal);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>()));


                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Add(existingSignal.ToDto<Dto.Signal>());

                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>()));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingADecimalSignal_NoneQualityMissingValuePolicyIsSetAsDeafult()
            {
                var existingSignal = new Domain.Signal()
                {
                    //Id = 1,
                    DataType = DataType.Decimal,
                    Granularity = Granularity.Day
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                signalsRepositoryMock
                    .Setup(srm => srm.Add(It.IsAny<Domain.Signal>()))
                    .Returns(existingSignal);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<decimal>>()));


                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Add(existingSignal.ToDto<Dto.Signal>());

                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<decimal>>()));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingABooleanSignal_NoneQualityMissingValuePolicyIsSetAsDeafult()
            {
                var existingSignal = new Domain.Signal()
                {
                    //Id = 1,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                signalsRepositoryMock
                    .Setup(srm => srm.Add(It.IsAny<Domain.Signal>()))
                    .Returns(existingSignal);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<bool>>()));


                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Add(existingSignal.ToDto<Dto.Signal>());

                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<bool>>()));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingAStringSignal_NoneQualityMissingValuePolicyIsSetAsDeafult()
            {
                var existingSignal = new Domain.Signal()
                {
                    //Id = 1,
                    DataType = DataType.String,
                    Granularity = Granularity.Day
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                signalsRepositoryMock
                    .Setup(srm => srm.Add(It.IsAny<Domain.Signal>()))
                    .Returns(existingSignal);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<string>>()));


                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.Add(existingSignal.ToDto<Dto.Signal>());

                missingValuePolicyRepositoryMock
                    .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<string>>()));
            }

            [TestMethod]
            public void GivenASignalAndSecondDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Second,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 3),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 1, 1, 2),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 3),  Value = (double)2.5 }
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                var firstTimestamp = new DateTime(2000, 1, 1, 1, 1, 1);
                var lastTimestamp = new DateTime(2000, 1, 1, 1, 1, 4);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

                int index = 0;
                foreach (var fd in filledDatum)
                {
                    Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality);
                    Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp);
                    Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                    index++;
                }
            }

            [TestMethod]
            public void GivenASignalAndMinuteDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Minute,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 3, 1),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 1, 2, 1),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 3, 1),  Value = (double)2.5 }
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                var firstTimestamp = new DateTime(2000, 1, 1, 1, 1, 1);
                var lastTimestamp = new DateTime(2000, 1, 1, 1, 4, 1);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

                int index = 0;
                foreach (var fd in filledDatum)
                {
                    Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality);
                    Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp);
                    Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                    index++;
                }
            }

            [TestMethod]
            public void GivenASignalAndHourDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Hour,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 3, 1, 1),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 2, 1, 1),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 3, 1, 1),  Value = (double)2.5 }
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                var firstTimestamp = new DateTime(2000, 1, 1, 1, 1, 1);
                var lastTimestamp = new DateTime(2000, 1, 1, 4, 1, 1);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

                int index = 0;
                foreach (var fd in filledDatum)
                {
                    Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality);
                    Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp);
                    Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                    index++;
                }
            }

            [TestMethod]
            public void GivenASignalAndDayDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 2),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3),  Value = (double)2.5 }
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                var firstTimestamp = new DateTime(2000, 1, 1);
                var lastTimestamp = new DateTime(2000, 1, 4);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

                int index = 0;
                foreach (var fd in filledDatum)
                {
                    Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality);
                    Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp);
                    Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                    index++;
                }
            }

            [TestMethod]
            public void GivenASignalAndWeekDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Week,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 15),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 8),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 15),  Value = (double)2.5 }
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                var firstTimestamp = new DateTime(2000, 1, 1);
                var lastTimestamp = new DateTime(2000, 1, 22);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

                int index = 0;
                foreach (var fd in filledDatum)
                {
                    Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality);
                    Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp);
                    Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                    index++;
                }
            }

            [TestMethod]
            public void GivenASignalAndYearDatumWithMissingData_WhenGettingData_FilledDatumIsReturned()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Year,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (double)2.5 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2001, 1, 1),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (double)2.5 }
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                var firstTimestamp = new DateTime(2000, 1, 1);
                var lastTimestamp = new DateTime(2003, 1, 1);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

                int index = 0;
                foreach (var fd in filledDatum)
                {
                    Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality);
                    Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp);
                    Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                    index++;
                }
            }

            [TestMethod]
            public void GivenASignalAndMonthDatumWithMissingData_WhenGettingData_FillsDataFromIncludedUtcToExcludedUtc()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0 }
                };

                var filledDatum = new Dto.Datum[]
                {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0},
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1),  Value = default(double)}
                };

                var firstTimestamp = new DateTime(2000, 1, 1);
                var lastTimestamp = new DateTime(2000, 4, 1);

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

                int index = 0;
                foreach (var fd in filledDatum)
                {
                    Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality);
                    Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp);
                    Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                    index++;
                }
            }

            [TestMethod]
            public void GivenASignalAndDatum_WhenGettingSingleData_ReturnsThisData()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double
                };

                List<Datum<double>> datumList = new List<Datum<double>> { new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 } };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                DateTime date = new DateTime(2000, 1, 1);

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(It.IsAny<Domain.Signal>(), date, date))
                    .Returns(datumList.AsEnumerable<Datum<double>>);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(1, date, date);

                Assert.AreEqual(datumList.First().ToDto<Dto.Datum>().Quality, result.First().Quality);
                Assert.AreEqual(datumList.First().ToDto<Dto.Datum>().Timestamp, result.First().Timestamp);
                Assert.AreEqual(datumList.First().ToDto<Dto.Datum>().Value, result.First().Value);
            }

            [TestMethod]
            public void GetPathEntry_DoesNotThrow()
            {
                signalsWebService = new SignalsWebService(null);

                var result = signalsWebService.GetPathEntry(null);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenNoPath_WhenGettingPathEntry_RepositoryGetAllWithPathPrefixIsCalled()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                signalsRepositoryMock
                    .Setup(srm => srm.GetAllWithPathPrefix(It.IsAny<Path>()));

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.GetPathEntry(null);

                signalsRepositoryMock
                    .Verify(srm => srm.GetAllWithPathPrefix(It.IsAny<Path>()));
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingPathEntry_ReturnsNull()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                signalsRepositoryMock
                    .Setup(srm => srm.GetAllWithPathPrefix(It.IsAny<Path>()))
                    .Returns<Domain.Signal>(null);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetPathEntry(null);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenListOfSignals_WhenGettingPathEntry_ReturnsPathEntryWithListOfThoseSignal()
            {
                List<Signal> signalsList = new List<Signal>()
                {
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/s1")},
                    new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/sub/s2") }
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                signalsRepositoryMock
                    .Setup(srm => srm.GetAllWithPathPrefix(It.IsAny<Path>()))
                    .Returns(signalsList.AsEnumerable);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "root" } });

                int index = 0;
                foreach (var signal in signalsList)
                {
                    Assert.AreEqual(signal.ToDto<Dto.Signal>().DataType, result.Signals.ElementAt(index).DataType);
                    CollectionAssert.AreEqual(signal.ToDto<Dto.Signal>().Path.Components.ToArray(), result.Signals.ElementAt(index).Path.Components.ToArray());
                    index++;
                }
            }

            //[TestMethod]
            //public void GivenListOfSignals_WhenGettingPathEntry_ReturnsPathEntryWithListOfSignalsFromMainDirectory()
            //{
            //    List<Signal> signalsList = new List<Signal>()
            //    {
            //        new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/s1")},
            //        new Signal() {DataType = DataType.Double, Path = Domain.Path.FromString("root/sub/s2") }
            //    };

            //    signalsRepositoryMock = new Mock<ISignalsRepository>();

            //    signalsRepositoryMock
            //        .Setup(srm => srm.GetAllWithPathPrefix(It.IsAny<Path>()))
            //        .Returns(signalsList.AsEnumerable);

            //    var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);

            //    signalsWebService = new SignalsWebService(signalsDomainService);

            //    var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new string[] { "root" } });

            //    Assert.AreEqual(1, result.Signals.Count());
            //    Assert.AreEqual(signalsList.First().ToDto<Dto.Signal>().DataType, result.Signals.First().DataType);
            //    CollectionAssert.AreEqual(signalsList.First().ToDto<Dto.Signal>().Path.Components.ToArray(),
            //        result.Signals.First().Path.Components.ToArray());
            //}

            //Bug fixing

            [TestMethod]
            public void GivenASignalAndSingleDatumInMiddleOfTheRange_WhenGettingDataFromTheSpecifiedRange_ReturnsFilledData()
            {
                var existingSignal = new Signal()
                {
                    Id = 1,
                    DataType = DataType.Double,
                    Granularity = Granularity.Month,
                    Path = Domain.Path.FromString("root/signal1")
                };

                var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0 }
                };

                var filledDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1),  Value = default(double)}
                };

                var firstTimestamp = new DateTime(2000, 1, 1);
                var lastTimestamp = new DateTime(2000, 4, 1);

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(existingSignal);

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.GetData<double>(existingSignal, firstTimestamp, lastTimestamp))
                    .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>);

                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

                missingValuePolicyRepositoryMock
                    .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                    .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());

                var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.GetData(existingSignal.Id.Value, firstTimestamp, lastTimestamp);

                int index = 0;
                foreach (var fd in filledDatum)
                {
                    Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality);
                    Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp);
                    Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                    index++;

                }
            }

            [TestMethod]
            public void GivenASignal_WhenSettingData_RepositoryDeleteDataIsCalled()
            {
                MockSetup();

                Datum<double>[] dataToSet = new Datum<double>[] {
                        new Datum<double>() { Id = 1, Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                        new Datum<double>() { Id = 2, Quality = Quality.Fair, Timestamp = new DateTime(2000, 2, 1), Value = (double)2 },
                        new Datum<double>() { Id = 3, Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)3 },
                        };

                signalDomainService.SetData(1, dataToSet);

                signalsDataRepositoryMock.Verify(sr => sr.DeleteData<double>(It.IsAny<Domain.Signal>()));
            }

            [TestMethod]
            [ExpectedException(typeof(IdNotNullException))]
            public void GivenASignal_WhenAddinASignalWithSameId_ThrowsException()
            {
                var existingSignal = new Signal()
                {
                    DataType = DataType.Double,
                    Granularity = Granularity.Day,
                    Path = Domain.Path.FromString("root")
                };

                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenRepositoryThatAssigns(1);

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                var result = signalsWebService.Add(existingSignal.ToDto<Dto.Signal>());

                signalsWebService.Add(new Dto.Signal()
                {
                    DataType = Dto.DataType.Double,
                    Granularity = Dto.Granularity.Day,
                    Id = result.Id
                });
            }

            [TestMethod]
            public void GivenASignal_WhenSettingDataWithNullValue_RepositorySetShouldBeCalledWithDataWithNullValue()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();

                GivenASignal(new Signal()
                {
                    Id = 1,
                    DataType = DataType.String
                });

                var existingDatum = new Datum<string>[]
                {
                    new Datum<string>() {Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = null }
                };

                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();

                signalsDataRepositoryMock
                    .Setup(sdrm => sdrm.SetData<string>(It.IsAny<IEnumerable<Datum<string>>>()));

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, null);

                signalsWebService = new SignalsWebService(signalsDomainService);

                signalsWebService.SetData(1, existingDatum.ToDto<IEnumerable<Dto.Datum>>());

                signalsDataRepositoryMock
                    .Verify(sdrm => sdrm.SetData<string>(It.Is<IEnumerable<Domain.Datum<string>>>(passedData =>
                    (
                        passedData.First().Quality == existingDatum.First().Quality
                        && passedData.First().Value == existingDatum.First().Value
                        && passedData.First().Timestamp == existingDatum.First().Timestamp
                    ))));
            }
        }
    }
}