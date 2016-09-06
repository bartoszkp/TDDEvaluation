using Domain;
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
    public class FirstOrderMisiingValuePolicyTests
    {
        private SignalsWebService signalsWebService;


        [TestMethod]
        public void SignalNoData_GetData_ReturnsNoneElements()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Month
            };

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(signal);
            Mock<FirstOrderMissingValuePolicy<int>> mvp = new Mock<FirstOrderMissingValuePolicy<int>>();

            mvpRepoMock.Setup(m => m.Get(signal)).Returns(mvp.Object);
            signalsDataRepoMock.Setup(s => s.GetData<int>(signal, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1)))
                .Returns(new List<Datum<int>>());

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));
            var fetchedDatumObject = result.ElementAt(0);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Dto.Quality.None, fetchedDatumObject.Quality);

        }

        [TestMethod]
        public void SignalNoData_GetDataIntegerWhenElementNonExist_ReturnElementsInterpolated()
        {
            var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = (double)2 },
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = (double)5 }
            };
            var filledDatum = new Dto.Datum[]
            {
                   new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 6, 1), Value = 3 }
            };
            SetupSignalAndDatumWithPolicyMock(existingSignal, existingDatum, new DateTime(2000, 6, 1), new DateTime(2000, 7, 1), new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDouble());
            var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 6, 1), new DateTime(2000, 7, 1));
            AssertDatum(result, filledDatum); 
        }

        private void SetupSignalAndDatumWithPolicyMock(Signal existingSignal, Dto.Datum[] existingDatum,
                DateTime fromIncludedUtc, DateTime toExcludedUtc, Domain.MissingValuePolicy.MissingValuePolicyBase missingValuePolicyBase)
        {
            signalsRepoMock = new Mock<ISignalsRepository>();
            GivenASignal(existingSignal);
            signalsDataRepoMock = new Mock<ISignalsDataRepository>();
            choiseDataType(existingSignal, existingDatum, fromIncludedUtc, toExcludedUtc);
            mvpRepoMock = new Mock<IMissingValuePolicyRepository>();
            mvpRepoMock
                .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(missingValuePolicyBase);
            var signalsDomainService = new SignalsDomainService(
                    signalsRepoMock.Object,
                    signalsDataRepoMock.Object,
                    mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
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

        private void GivenNoSignals()
        {
            signalsRepoMock = new Mock<ISignalsRepository>();
            signalsRepoMock
                .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                .Returns<Domain.Signal>(s => s);
            var signalsDomainService = new SignalsDomainService(signalsRepoMock.Object, null, null);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        private void GivenASignal(Domain.Signal existingSignal)
        {
            GivenNoSignals();
            signalsRepoMock
                .Setup(sr => sr.Get(existingSignal.Id.Value))
                .Returns(existingSignal);
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

        private void SetupWebService()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepoMock.Object, signalsDataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }

        private void choiseDataType(Signal existingSignal, Dto.Datum[] existingDatum, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            var choiseTypeOf = new Dictionary<DataType, Action>()
                {
                    {DataType.Boolean,()=> signalsDataRepoMock.Setup(sdrm => sdrm.GetData<bool>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<bool>>>)},
                    {DataType.Decimal,()=> signalsDataRepoMock.Setup(sdrm => sdrm.GetData<decimal>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<decimal>>>)},
                    {DataType.Double,()=> signalsDataRepoMock.Setup(sdrm => sdrm.GetData<double>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<double>>>)},
                    {DataType.Integer,()=> signalsDataRepoMock.Setup(sdrm => sdrm.GetData<int>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>)},
                    {DataType.String,()=> signalsDataRepoMock.Setup(sdrm => sdrm.GetData<string>(existingSignal, fromIncludedUtc, toExcludedUtc))
                            .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<string>>>)}
                };
            choiseTypeOf[existingSignal.DataType].Invoke();
        }
        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();
    }
}
