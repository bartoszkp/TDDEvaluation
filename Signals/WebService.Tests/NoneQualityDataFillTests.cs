using Domain;
using Domain.Exceptions;
using Domain.MissingValuePolicy;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Tests
{
    [TestClass]
    public class NoneQualityDataFillTests
    {
        private SignalsWebService signalsWebService;

        [TestMethod]
        public void SignalHasNoData_GetData_ReturnsFilledData()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Second
            };

            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(signal);
            Mock<NoneQualityMissingValuePolicy<int>> mvp = new Mock<NoneQualityMissingValuePolicy<int>>();

            mvpRepoMock.Setup(m => m.Get(signal)).Returns(mvp.Object);
            signalsDataRepositoryMock.Setup(s => s.GetData<int>(signal, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1, 0, 1, 0)))
                .Returns(new List<Datum<int>>());

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1, 0, 1, 0));

            Assert.AreEqual(60, result.Count());
        }

        [TestMethod]
        public void GivenSignal_GetData_GetsItAndFillsMissingParts()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Month
            };

            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns(signal);
            Mock<NoneQualityMissingValuePolicy<int>> mvp = new Mock<NoneQualityMissingValuePolicy<int>>();

            mvpRepoMock.Setup(m => m.Get(signal)).Returns(mvp.Object);
            signalsDataRepositoryMock.Setup(s => s.GetData<int>(signal, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1)))
                .Returns(new List<Datum<int>>()
                {
                    new Datum<int>() {Quality = Quality.None,Value = (int)0,Timestamp =  new DateTime(2000, 1, 1)}
                });

            var expectedFilledDatum = new Datum<int>() { Quality = Quality.None, Value = (int)0, Timestamp = new DateTime(2000, 2, 1) };

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(expectedFilledDatum.Timestamp, new DateTime(2000, 2, 1));
            Assert.AreEqual(expectedFilledDatum.Quality, Quality.None);
            Assert.AreEqual(expectedFilledDatum.Value, 0);

        }

        [TestMethod]
        public void GivenASignal_WhenGetDataWithTimestampGreater_ReturnElementNoneQuality()
        {
            var existingSignal = SignalWith(1, DataType.String, Granularity.Day, Path.FromString("day"));
            var existingDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (string)"first" }
            };

            var filledDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 2),  Value = default(string)}
            };

            signalsRepositoryMock = new Mock<ISignalsRepository>();
            GivenASignal(existingSignal);
            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
            choiseDataType(existingSignal, existingDatum, new DateTime(2000, 1, 2), new DateTime(2000, 1, 3));
            mvpRepoMock = new Mock<IMissingValuePolicyRepository>();
            mvpRepoMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyString());
            var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 1, 2), new DateTime(2000, 1, 3));
            Assert.AreEqual(filledDatum.ElementAt(0).Quality, result.ElementAt(0).Quality);
            Assert.AreEqual(filledDatum.ElementAt(0).Timestamp, result.ElementAt(0).Timestamp);
            Assert.AreEqual(filledDatum.ElementAt(0).Value, result.ElementAt(0).Value);
        }

        private void SetupWebService()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        private void choiseDataType(Signal existingSignal, Dto.Datum[] existingDatum, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var choiseTypeOf = new Dictionary<DataType, Action>()
                {
                    {DataType.Boolean,()=> signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<bool>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<bool>>>)},
                    {DataType.Decimal,()=> signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<decimal>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<decimal>>>)},
                    {DataType.Double,()=> signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<double>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>)},
                    {DataType.Integer,()=> signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<int>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>)},
                    {DataType.String,()=> signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<string>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<string>>>)}
                };
            choiseTypeOf[existingSignal.DataType].Invoke();
        }

        private void GivenASignal(Domain.Signal existingSignal)
        {
            GivenNoSignals();
            signalsRepositoryMock
                .Setup(sr => sr.Get(existingSignal.Id.Value))
                .Returns(existingSignal);
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

        private void AssertDatum(IEnumerable<Dto.Datum> result, Dto.Datum[] filledDatum)
        {
            int index = 0;
            foreach (var fd in filledDatum)
            {
                Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality);
                Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp);
                Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                index++;
            }
        }
        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

    }
}
