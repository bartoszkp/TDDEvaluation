using System.Linq;
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

            [TestMethod]
            public void GivenNoSignals_WhenGettingByPath_DoesNotThrow()
            {
                GivenNoSignals();

                signalsWebService.Get(new Dto.Path() { Components = new[] { "root", "signal" } });
            }
            [TestMethod]
            public void GivenASignal_WhenGettingByPath_ReturnsTheSignalWithSamePath()
            {
                int id = 1;
                var path = new Dto.Path() { Components = new[] { "root", "signal" } };
                var signal = new Signal()
                {
                    Id = id,
                    DataType = DataType.Boolean,
                    Granularity = Granularity.Day,
                    Path = path.ToDomain<Domain.Path>()
                };

                GivenASignal(signal);

                var result = signalsWebService.Get(path);
                Assert.IsNotNull(result);
                CollectionAssert.AreEqual(path.Components.ToArray(), result.Path.Components.ToArray());
            }

            [ExpectedException(typeof(SignalNotFoundException))]
            [TestMethod]
            public void GivenNoSignals_WhenSettingDataForSignal_ThrowsSignalNotFoundException()
            {
                GivenNoSignals();
                int id = 5;
                signalsWebService.SetData(id, null);
            }

            [TestMethod]
            public void GivenASignalAndDataOfDoubles_WhenSettingDataForSignal_DataRepositorySetDataIsCalled()
            {
                int id = 1;
                Dto.Datum[] dtoData;
                GivenASignalAndDataOf(DataType.Double, 1.0, out dtoData, id);

                signalsWebService.SetData(id, dtoData);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<double>>>()));
            }

            [TestMethod]
            public void GivenASignalAndNoData_WhenSettingDataForSignal_ArgumentNullExceptionIsNotThrown()
            {
                int id = 1;
                var path = new Dto.Path() { Components = new[] { "root", "signal" } };
                var signal = SignalWith(id, DataType.Double, Granularity.Day, path.ToDomain<Domain.Path>());

                GivenASignal(signal);

                signalsWebService.SetData(id, null);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<double>>>()));
            }

            [TestMethod]
            public void GivenASignalAndDataOfBools_WhenSettingDataForSignal_DoesNotThrow()
            {
                int id = 1;
                Dto.Datum[] dtoData;
                GivenASignalAndDataOf(DataType.Boolean, false, out dtoData, id);

                signalsWebService.SetData(id, dtoData);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<bool>>>()));
            }

            [TestMethod]
            public void GivenASignalAndDataOfIntegers_WhenSettingDataForSignal_DoesNotThrow()
            {
                int id = 1;
                Dto.Datum[] dtoData;
                GivenASignalAndDataOf(DataType.Integer, 1, out dtoData, id);

                signalsWebService.SetData(id, dtoData);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<int>>>()));
            }

            [TestMethod]
            public void GivenASignalAndDataOfDecimals_WhenSettingDataForSignal_DoesNotThrow()
            {
                int id = 1;
                Dto.Datum[] dtoData;
                GivenASignalAndDataOf(DataType.Decimal, 99m, out dtoData, id);

                signalsWebService.SetData(id, dtoData);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<decimal>>>()));
            }

            [TestMethod]
            public void GivenASignalAndDataOfStrings_WhenSettingDataForSignal_DoesNotThrow()
            {
                int id = 1;
                Dto.Datum[] dtoData;
                GivenASignalAndDataOf(DataType.String, "str", out dtoData, id);

                signalsWebService.SetData(id, dtoData);
                signalsDataRepoMock.Verify(sd => sd.SetData(It.IsAny<IEnumerable<Datum<string>>>()));
            }

            [ExpectedException(typeof(SignalNotFoundException))]
            [TestMethod]
            public void GivenNoSignals_WhenGettingData_ThrowsSignalNotFoundException()
            {
                int id = 5;
                System.DateTime dateFrom = new System.DateTime(2000, 1, 1), dateTo = new System.DateTime(2000, 3, 1);

                signalsWebService.GetData(id, dateFrom, dateTo);
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

            private Dto.Datum[] DatumWith(object value, System.DateTime timestamp, int numberOfElements = 1, Dto.Quality quality = Dto.Quality.Fair)
            {
                var dtoData = new Dto.Datum[numberOfElements];
                for (int i = 0; i < numberOfElements; ++i)
                {
                    dtoData[i] = new Dto.Datum()
                    {
                        Quality = quality,
                        Timestamp = timestamp.AddMonths(i),
                        Value = value
                    };
                }
                return dtoData;
            }

            private void GivenNoSignals()
            {
                signalsRepositoryMock = new Mock<ISignalsRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);
                signalsDataRepoMock = new Mock<ISignalsDataRepository>();

                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepoMock.Object, null);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Domain.Signal existingSignal)
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
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s =>
                    {
                        s.Id = id;
                        return s;
                    });
            }      
            
            private void GivenASignalAndDataOf(DataType dataType, object datumValue, out Dto.Datum[] dtoData, int signalId = 1, int numberOfDatums = 1)
            {
                var path = new Dto.Path() { Components = new[] { "root", "signal" } };
                var signal = SignalWith(signalId, dataType, Granularity.Day, path.ToDomain<Domain.Path>());                

                GivenASignal(signal);
                dtoData = DatumWith(datumValue, new System.DateTime(2000, 1, 1), numberOfDatums);
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepoMock;
        }
    }
}