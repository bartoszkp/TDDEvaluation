using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;

            //[TestMethod]
            //public void GivenNoSignals_WhenAddingASignal_ReturnsNotNull()
            //{
            //    GivenNoSignals();

            //    var result = signalsWebService.Add(new Dto.Signal());

            //    Assert.IsNotNull(result);
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenAddingASignal_ReturnsTheSameSignalExceptForId()
            //{
            //    GivenNoSignals();

            //    var result = signalsWebService.Add(SignalWith(
            //        dataType: Dto.DataType.Decimal,
            //        granularity: Dto.Granularity.Week,
            //        path: new Dto.Path() { Components = new[] { "root", "signal" } }));

            //    Assert.AreEqual(Dto.DataType.Decimal, result.DataType);
            //    Assert.AreEqual(Dto.Granularity.Week, result.Granularity);
            //    CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenAddingASignal_PassesGivenSignalToRepositoryAdd()
            //{
            //    GivenNoSignals();

            //    signalsWebService.Add(SignalWith(
            //        dataType: Dto.DataType.Decimal,
            //        granularity: Dto.Granularity.Week,
            //        path: new Dto.Path() { Components = new[] { "root", "signal" } }));

            //    signalsRepositoryMock.Verify(sr => sr.Add(It.Is<Domain.Signal>(passedSignal
            //        => passedSignal.DataType == DataType.Decimal
            //            && passedSignal.Granularity == Granularity.Week
            //            && passedSignal.Path.ToString() == "root/signal")));
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenAddingASignal_ReturnsIdFromRepository()
            //{
            //    var signalId = 1;
            //    GivenNoSignals();
            //    GivenRepositoryThatAssigns(id: signalId);

            //    var result = signalsWebService.Add(SignalWith(
            //        dataType: Dto.DataType.Decimal,
            //        granularity: Dto.Granularity.Week,
            //        path: new Dto.Path() { Components = new[] { "root", "signal" } }));

            //    Assert.AreEqual(signalId, result.Id);
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenGettingById_DoesNotThrow()
            //{
            //    GivenNoSignals();

            //    signalsWebService.GetById(0);
            //}


            //[TestMethod]
            //public void GivenASignal_WhenGettingByItsId_ReturnsIt()
            //{
            //    var signalId = 1;
            //    GivenASignal(SignalWith(
            //        id: signalId,
            //        dataType: Domain.DataType.String,
            //        granularity: Domain.Granularity.Year,
            //        path: Domain.Path.FromString("root/signal")));

            //    var result = signalsWebService.GetById(signalId);

            //    Assert.AreEqual(signalId, result.Id);
            //    Assert.AreEqual(Dto.DataType.String, result.DataType);
            //    Assert.AreEqual(Dto.Granularity.Year, result.Granularity);
            //    CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenGettingById_RepositoryGetIsCalledWithGivenId()
            //{
            //    var signalId = 1;
            //    GivenNoSignals();

            //    signalsWebService.GetById(signalId);

            //    signalsRepositoryMock.Verify(sr => sr.Get(signalId));
            //}

            //[TestMethod]
            //public void GivenNoSignals_WhenGettingById_ReturnsNull()
            //{
            //    GivenNoSignals();

            //    var result = signalsWebService.GetById(0);

            //    Assert.IsNull(result);
            //}

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
                    dataType: DataType.Boolean,
                    granularity: Granularity.Day,
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
                    dataType: DataType.Boolean,
                    granularity: Granularity.Day,
                    path: Domain.Path.FromString((path))));

                var pathDto = new Dto.Path() { Components = new[] { "root", "signal1" } };

                var result = signalsWebService.Get(pathDto);

                GettingByPathAssertion(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByFalsePath_ThrowsException()
            {
                string path = "root/signal1";

                GivenASignalSetupGetByPath(SignalWith(
                    id: 1,
                    dataType: DataType.Boolean,
                    granularity: Granularity.Day,
                    path: Domain.Path.FromString((path))));

                var pathDto = new Dto.Path() { Components = new[] { "root", "signal3" } };
                
                GettingByFalsePathAssertion(pathDto);
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

            private void GivenASignalSetupGetByPath(Domain.Signal existingSignal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(srm => srm.Get(existingSignal.Path))
                    .Returns(existingSignal);
            }

            private void GettingByPathAssertion(Dto.Signal result)
            {
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(Dto.DataType.Boolean, result.DataType);
                Assert.AreEqual(Dto.Granularity.Day, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal1" }, result.Path.Components.ToArray());
            }

            private void GettingByFalsePathAssertion(Dto.Path pathDto)
            {
                try
                {
                    signalsWebService.Get(pathDto);
                }
                catch (ArgumentException ae)
                {
                    Assert.IsNotNull(ae);
                    return;
                }
                Assert.Fail();
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
        }
    }
}