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
    public class ShadowMissingValuePolicyTests
    {
        public SignalsWebService signalsWebService;

        [TestMethod]
        public void GivenASignalAndShadowMissingValuePolicy_WhenSettingPolicy_RepositorySetIsCalled()
        {
            signalsRepositoryMock = new Mock<ISignalsRepository>();

            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Day
            };

            GivenASignal(existingSignal);

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

            var existingShadowPolicy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Double, ShadowSignal = existingSignal.ToDto<Dto.Signal>()};

            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<ShadowMissingValuePolicy<double>>()));

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);

            var domainExistingShadowPolicy = existingShadowPolicy.ToDomain<ShadowMissingValuePolicy<double>>();

            signalsWebService.SetMissingValuePolicy(1, existingShadowPolicy);

            missingValuePolicyRepositoryMock
                .Verify(mvprm => mvprm.Set(existingSignal, It.Is<ShadowMissingValuePolicy<double>>(smvp => 
                (
                    smvp.ShadowSignal.DataType ==  domainExistingShadowPolicy.ShadowSignal.DataType
                    && smvp.ShadowSignal.Granularity == domainExistingShadowPolicy.ShadowSignal.Granularity
                    && smvp.ShadowSignal.Id == domainExistingShadowPolicy.ShadowSignal.Id
                ))));
        }

        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.IncompatibleDataTypes))]
        public void GivenASignalAndShadowMissingValuePolicyWithSignalWithDifferentDataType_WhenSettingPolicy_ExceptionIsThrown()
        {
            signalsRepositoryMock = new Mock<ISignalsRepository>();

            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Day
            };

            var shadowSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Decimal,
                Granularity = Granularity.Day
            };

            GivenASignal(existingSignal);

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

            var existingShadowPolicy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Double, ShadowSignal = shadowSignal.ToDto<Dto.Signal>() };

            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<ShadowMissingValuePolicy<double>>()));

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);
            
            signalsWebService.SetMissingValuePolicy(1, existingShadowPolicy);
            
        }

        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.IncompatibleGranularities))]
        public void GivenASignalAndShadowMissingValuePolicyWithSignalWithDifferentGranularity_WhenSettingPolicy_ExceptionIsThrown()
        {
            signalsRepositoryMock = new Mock<ISignalsRepository>();

            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Day
            };

            var shadowSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Hour
            };

            GivenASignal(existingSignal);

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

            var existingShadowPolicy = new Dto.MissingValuePolicy.ShadowMissingValuePolicy() { DataType = Dto.DataType.Double, ShadowSignal = shadowSignal.ToDto<Dto.Signal>() };

            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Set(It.IsAny<Domain.Signal>(), It.IsAny<ShadowMissingValuePolicy<double>>()));

            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, null, missingValuePolicyRepositoryMock.Object);

            signalsWebService = new SignalsWebService(signalsDomainService);
            
            signalsWebService.SetMissingValuePolicy(1, existingShadowPolicy);
        }

        [TestMethod]
        public void GivenAnExistingSignalAndDatumAndShadowPolicy_WhenGettingData_ProperlyFilledDatumIsReturned()
        {
            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Month
            };
            
            var existingDatum = new Dto.Datum[]
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (int)2 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 5, 1), Value = (int)5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 9, 1), Value = (int)9 }
            };
            
            var filledDatum = new Dto.Datum[]
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (int)2 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 2, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 4, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 5, 1), Value = (int)5 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 6, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 7, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 8, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 9, 1), Value = (int)9 }
            };

            var firstTimestamp = new DateTime(2000, 1, 1);
            var lastTimestamp = new DateTime(2000, 10, 1);

            signalsRepositoryMock = new Mock<ISignalsRepository>();

            GivenASignal(existingSignal);

            signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.Is<Domain.Signal>(signal => (
                signal.Id == 1
                && signal.DataType == DataType.Integer
                && signal.Granularity == Granularity.Month)), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>);
            
            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ShadowMissingValuePolicyInteger());

            var signalsDomainService = new SignalsDomainService(
                signalsRepositoryMock.Object,
                signalDataRepositoryMock.Object,
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
        public void GivenAnExistingSignalAndDatumAndShadowSignalAndDatum_WhenGettingData_ProperlyFilledDatumIsReturned()
        {
            var existingSignal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Month
            };

            var shadowSignal = new Signal()
            {
                Id = 2,
                DataType = DataType.Integer,
                Granularity = Granularity.Month
            };

            var existingDatum = new Dto.Datum[]
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (int)2 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 5, 1), Value = (int)5 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 9, 1), Value = (int)9 }
            };

            var shadowDatum = new Dto.Datum[]
            {
                new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1),  Value = (int)2},
                new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 5, 1),  Value = (int)0 },
                new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1),  Value = (int)4 }
            };

            var filledDatum = new Dto.Datum[]
            {
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (int)2 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 2, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (int)2 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 4, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 5, 1), Value = (int)5 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 6, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 7, 1), Value = (int)0 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = (int)4 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 9, 1), Value = (int)9 }
            };

            var firstTimestamp = new DateTime(2000, 1, 1);
            var lastTimestamp = new DateTime(2000, 10, 1);

            signalsRepositoryMock = new Mock<ISignalsRepository>();

            GivenASignal(existingSignal);

            GivenASignal(shadowSignal);

            signalDataRepositoryMock = new Mock<ISignalsDataRepository>();

            signalDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.Is<Domain.Signal>(signal => (
                signal.Id == 1
                && signal.DataType == DataType.Integer
                && signal.Granularity == Granularity.Month)), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(existingDatum.ToDomain<IEnumerable<Domain.Datum<int>>>);

            signalDataRepositoryMock
                .Setup(sdrm => sdrm.GetData<int>(It.Is<Domain.Signal>(signal => (
                signal.Id == 2
                && signal.DataType == DataType.Integer
                && signal.Granularity == Granularity.Month)), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(shadowDatum.ToDomain<IEnumerable<Domain.Datum<int>>>);

            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ShadowMissingValuePolicyInteger()
                {
                    ShadowSignal = shadowSignal
                });

            var signalsDomainService = new SignalsDomainService(
                signalsRepositoryMock.Object,
                signalDataRepositoryMock.Object,
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

        private void GivenASignal(Signal signal)
        {
            signalsRepositoryMock
                .Setup(sr => sr.Get(signal.Id.Value))
                .Returns(signal);
        }

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalDataRepositoryMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

    }
}
