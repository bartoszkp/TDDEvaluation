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
    public class SpecificQualityDataFillTests
    {
        private SignalsWebService signalsWebService;


        [TestMethod]
        public void SignalHasNoData_SetPolicy_ReturnsPolicy()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Month
            };
            var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
            {
                DataType = Dto.DataType.Integer,
                Quality = Dto.Quality.Bad,
                Value = (int)1
            };
            SetupPolicy(signal,policy);
            var domainExistingPolicy = (Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<int>)policy.ToDomain<Domain.MissingValuePolicy.MissingValuePolicyBase>();
            missingValuePolicyRepositoryMock
                .Verify(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.Is<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<int>>(mv =>
                (
                    mv.NativeDataType == domainExistingPolicy.NativeDataType
                    && mv.Quality == domainExistingPolicy.Quality
                    && mv.Value == domainExistingPolicy.Value
                ))));
        }

        [TestMethod]
        public void TestFixedBug_SpecificValuepolicyWork()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Boolean,
                Granularity = Granularity.Week,
                Path = Path.FromString("r/vbc")
            };

            var policy = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyBoolean()
            {   Value = false,
                Quality = Quality.Bad
            };
            var existingDatum = new Dto.Datum[]
                {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2018, 1, 1),  Value = (bool)true }
                };

            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2018, 1, 8),  Value = (bool)false }
            };
            SetupGetDataWithPolicy(signal, existingDatum, new DateTime(2018, 1, 8), new DateTime(2018, 1, 15), policy, filledDatum);
        }


        private void SetupPolicy(Signal signal, Dto.MissingValuePolicy.SpecificValueMissingValuePolicy policy)
        {
            signalsRepositoryMock = new Mock<ISignalsRepository>();
            signalsRepositoryMock.Setup(srm => srm.Get(signal.Id.Value))
                .Returns(signal);

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);

            signalsWebService.SetMissingValuePolicy(signal.Id.Value, policy);
        }

        private void SetupGetDataWithPolicy(Signal signal, Dto.Datum[] existingDatum,
                DateTime fromIncludedUtc, DateTime toExcludedUtc,
                Domain.MissingValuePolicy.MissingValuePolicyBase policy,
                Dto.Datum[] filledDatum)
        {
            signalsRepositoryMock = new Mock<ISignalsRepository>();
            GivenASignal(signal);
            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
            choiseDataType(signal, existingDatum,fromIncludedUtc, toExcludedUtc);
            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(policy);
            var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetData(signal.Id.Value,  fromIncludedUtc, toExcludedUtc);
            AssertDatum(result, filledDatum);
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
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();


    }
}
