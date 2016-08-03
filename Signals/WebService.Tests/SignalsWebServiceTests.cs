﻿using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
                GivenNoSignals_SetupSignalsRepositoryMock();

                var result = signalsWebService.Add(new Dto.Signal());

                Assert.IsNotNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsTheSameSignalExceptForId()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

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
                GivenNoSignals_SetupSignalsRepositoryMock();

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
                GivenNoSignals_SetupSignalsRepositoryMock();
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
                GivenNoSignals_SetupSignalsRepositoryMock();

                signalsWebService.GetById(0);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByItsId_ReturnsIt()
            {
                var signalId = 1;
                GivenASignal_SetupSignalsRepositoryMock(SignalWith(
                    id: signalId,
                    dataType: Domain.DataType.String,
                    granularity: Domain.Granularity.Year,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.GetById(signalId);

                AssertSignalsAreEqual(result, new Dto.Signal()
                {
                    Id = signalId,
                    DataType = Dto.DataType.String,
                    Granularity = Dto.Granularity.Year,
                    Path = new Dto.Path()
                    {
                        Components = new[] {"root", "signal"}
                    }
                });
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_RepositoryGetIsCalledWithGivenId()
            {
                var signalId = 1;
                GivenNoSignals_SetupSignalsRepositoryMock();

                signalsWebService.GetById(signalId);

                signalsRepositoryMock.Verify(sr => sr.Get(signalId));
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_ReturnsNull()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                var result = signalsWebService.GetById(0);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingByPath_DoesNotThrowUnimplementedException()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<Domain.Path>())).Returns(new Domain.Signal());

                signalsWebService.Get(new Dto.Path()
                {
                    Components = new[] { "root", "signal" }
                });
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingByPath_RepositoryGetIsCalledWithGivenPath()
            {
                var pathDto = new Dto.Path()
                {
                    Components = new[] { "root", "signal" }
                };

                var pathDomain = Domain.Path.FromString("root/signal");

                GivenNoSignals_SetupSignalsRepositoryMock();

                signalsRepositoryMock.Setup(sr => sr.Get(It.IsAny<Domain.Path>())).Returns(new Domain.Signal());

                signalsWebService.Get(pathDto);

                signalsRepositoryMock.Verify(srm => srm.Get(pathDomain));
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByItsPath_ReturnsIt()
            {
                var pathDto = new Dto.Path()
                {
                    Components = new[] { "root", "signal" }
                };

                var pathDomain = Domain.Path.FromString("root/signal");

                GivenASignal_SetupSignalsRepositoryMock(SignalWith(
                    DataType.Double,
                    Granularity.Month, 
                    Path.FromString("root/signal"), 
                    1));

                var result = signalsWebService.Get(pathDto);

                AssertSignalsAreEqual(result, SignalWith(
                    Dto.DataType.Double, 
                    Dto.Granularity.Month, 
                    new Dto.Path() { Components = new[] { "root", "signal" } }, 
                    1));
            }

            [TestMethod]
            [ExpectedException(typeof(System.ArgumentException))]
            public void GivenANonExistingPath_WhenGettingByPath_ThrowsArgumentException()
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                var pathDto = new Dto.Path()
                {
                    Components = new[] { "root", "signal" }
                };

                signalsWebService.Get(pathDto);
            }

            [TestMethod]
            public void GivenNoSignals_WhenSettingMissingValuePolicy_DoesNotThrow()
            {
                SetupWebService();

                int signalId = 1;

                signalsRepositoryMock.Setup(srm => srm.Get(signalId)).Returns(new Domain.Signal()
                {
                    Id = signalId
                });

                signalsWebService.SetMissingValuePolicy(signalId, new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy());
            }

            [TestMethod]
            public void GivenNoSignals_WhenSettingMissingValuePolicy_RepositorySetIsCalledWithGivenPolicy()
            {
                SetupWebService();

                int signalId = 1;

                Dto.MissingValuePolicy.MissingValuePolicy mvp = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy() { 
                    Id = signalId,
                    DataType = Dto.DataType.Double,
                    Quality = Dto.Quality.Fair,
                    Value = (double)1.5
                };

                signalsRepositoryMock.Setup(srm => srm.Get(signalId)).Returns(new Domain.Signal()
                {
                    Id = signalId
                });

                signalsWebService.SetMissingValuePolicy(signalId, mvp);

                missingValuePolicyRepositoryMock.Verify(mvprm => mvprm.Set(It.Is<Domain.Signal>(s => s.Id == signalId), 
                    It.Is<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>(svm => 
                    svm.Id == signalId && 
                    svm.Quality == Quality.Fair && 
                    svm.Value == (double)1.5)));
            }

            [TestMethod]
            [ExpectedException(typeof(System.ArgumentException))]
            public void GivenNonExistingSignalId_WhenSettingMissingValuePolicy_ThrowsArgumentException()
            {
                SetupWebService();

                int nonExistingSignalId = 1;

                signalsRepositoryMock.Setup(srm => srm.Get(nonExistingSignalId)).Returns((Domain.Signal)null);

                signalsWebService.SetMissingValuePolicy(nonExistingSignalId, new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy());
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingMissingValuePolicy_DoesNotThrow()
            {
                SetupWebService();

                signalsRepositoryMock.Setup(srm => srm.Get(It.IsAny<int>())).Returns(new Domain.Signal());

                missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>())).Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyInteger());

                signalsWebService.GetMissingValuePolicy(0);
            }

            [TestMethod]
            public void GivenSignalId_WhenGettingMissingValuePolicy_IdIsPassedToDomainToGetSignalById()
            {
                SetupWebService();
                int id = 1;

                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(new Domain.Signal());

                missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>())).Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyInteger());

                signalsWebService.GetMissingValuePolicy(id);

                signalsRepositoryMock.Verify(srm => srm.Get(id), Times.Once);
            }

            [TestMethod]
            public void GivenASignalId_WhenGettingMissingValuePolicy_RepositoryGetIsCalledWithGivenSignal()
            {
                SetupWebService();
                int id = 1;

                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(new Domain.Signal()
                {
                    Id = id
                });

                missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>())).Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyInteger());

                signalsWebService.GetMissingValuePolicy(id);

                missingValuePolicyRepositoryMock.Verify(mvprm => mvprm.Get(It.Is<Domain.Signal>(s => s.Id == id)), Times.Once);
            }

            [TestMethod]
            public void GivenASignalId_WhenGettingMissingValuePolicy_ReturnsCorrectPolicy()
            {
                SetupWebService();
                int id = 5;

                var signal = SignalWith(DataType.Integer, Granularity.Week, Path.FromString("root/signal"), id);

                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(signal);

                var expected = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyInteger()
                {
                    Id = id,
                    Quality = Quality.Good,
                    Signal = signal,
                    Value = (int)2
                };

                missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>())).Returns(expected);

                var result = signalsWebService.GetMissingValuePolicy(id);

                Assert.AreEqual(result.Id, id);
                AssertSignalsAreEqual(result.Signal, SignalWith(
                    Dto.DataType.Integer, 
                    Dto.Granularity.Week, 
                    new Dto.Path()
                    {
                        Components = new[] { "root", "signal" }
                    }, 
                    id));
            }

            [TestMethod]
            [ExpectedException(typeof(System.ArgumentException))]
            public void GivenNonExistingId_WhenGettingMissingValuePolicy_ThrowsArgumentException()
            {
                SetupWebService();

                int nonExistingId = 4;

                signalsRepositoryMock.Setup(srm => srm.Get(nonExistingId)).Returns((Domain.Signal)null);

                signalsWebService.GetMissingValuePolicy(nonExistingId);
            }

            [TestMethod]
            public void GivenNoData_WhenSettingData_DoesNotThrow()
            {
                SetupWebService();

                var dataDto = new Dto.Datum[] {  };

                signalsWebService.SetData(0, dataDto);
            }

            [TestMethod]
            public void GivenNoData_WhenSettingData_CallsGetByIdWithPassedId()
            {
                SetupWebService();

                int id = 1;
                var dataDto = new Dto.Datum[] {  };

                signalsWebService.SetData(id, dataDto);

                signalsRepositoryMock.Verify(srm => srm.Get(id), Times.Once);
            }

            [TestMethod]
            public void GivenData_WhenSettingData_RepositorySetIsCalledWithCorrectData()
            {
                SetupWebService();
                int id = 4;

                var dataDto = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = (int)2 } };

                signalsWebService.SetData(id, dataDto);

                signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<int>(It.Is<IEnumerable<Datum<int>>>(data => 
                    data.ElementAt(0).Quality == Quality.Good &&
                    data.ElementAt(0).Timestamp == new System.DateTime(2000, 1, 1) &&
                    data.ElementAt(0).Value == 2)), 
                    Times.Once);
            }

            [TestMethod]
            public void GivenData_WhenSettingData_SignalFoundByGetByIdIsAssignedToDataPassedToRepository()
            {
                SetupWebService();
                int id = 4;

                var dataDto = new Dto.Datum[] { new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new System.DateTime(2000, 1, 1), Value = (int)2 } };

                var signalDomain = SignalWith(DataType.Integer, Granularity.Day, Path.FromString("root/signal"), id);

                signalsRepositoryMock.Setup(srm => srm.Get(id)).Returns(signalDomain);

                signalsWebService.SetData(id, dataDto);

                signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<int>(It.Is<IEnumerable<Datum<int>>>(data =>
                    data.ElementAt(0).Quality == Quality.Good &&
                    data.ElementAt(0).Timestamp == new System.DateTime(2000, 1, 1) &&
                    data.ElementAt(0).Value == 2 &&
                    data.ElementAt(0).Signal.Id == signalDomain.Id &&
                    data.ElementAt(0).Signal.DataType == signalDomain.DataType &&
                    data.ElementAt(0).Signal.Granularity == signalDomain.Granularity &&
                    data.ElementAt(0).Signal.Path.ToString() == signalDomain.Path.ToString())),
                    Times.Once);
            }

            [TestMethod]
            [ExpectedException(typeof(System.ArgumentException))]
            public void GivenNonExistingSignalId_WhenSettingData_ThrowsArgumentException()
            {
                SetupWebService();
                int nonExistingId = 6;

                var dataDto = new Dto.Datum[] { };

                signalsRepositoryMock.Setup(srm => srm.Get(nonExistingId)).Returns((Domain.Signal)null);

                signalsWebService.SetData(nonExistingId, dataDto);
            }

            private void SetupWebService()
            {
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private Dto.Signal SignalWith(Dto.DataType dataType, Dto.Granularity granularity, Dto.Path path, int? id = null)
            {
                return new Dto.Signal()
                {
                    Id = id,
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }

            private Domain.Signal SignalWith(Domain.DataType dataType, Domain.Granularity granularity, Domain.Path path, int? id = null)
            {
                return new Domain.Signal()
                {
                    Id = id,
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }

            private void GivenNoSignals_SetupSignalsRepositoryMock()
            {
                SetupWebService();

                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);        
            }

            private void GivenASignal_SetupSignalsRepositoryMock(Domain.Signal existingSignal)
            {
                GivenNoSignals_SetupSignalsRepositoryMock();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(existingSignal.Id.Value))
                    .Returns(existingSignal);

                signalsRepositoryMock.Setup(sr => sr.Get(existingSignal.Path)).Returns(existingSignal);
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

            private void AssertSignalsAreEqual(Dto.Signal signal1, Dto.Signal signal2)
            {
                Assert.AreEqual(signal1.Id, signal2.Id);
                Assert.AreEqual(signal1.DataType, signal2.DataType);
                Assert.AreEqual(signal1.Granularity, signal2.Granularity);
                CollectionAssert.AreEqual(signal1.Path.Components.ToArray(), signal2.Path.Components.ToArray());
            }

            private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
        }
    }
}