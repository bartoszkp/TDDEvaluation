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
    public class ShadowMissingValuePolicyFillTest
    {
        SignalsWebService signalsWebService;
        [TestMethod]
        public void GivenASignalAndDatum_Decimal_ReturnOneElement()
        {
            var existingSignal = SignalWith(1, DataType.Decimal, Granularity.Month, Path.FromString("root/signal1"));
            var shadowSignal = SignalWith(2, DataType.Decimal, Granularity.Month, Path.FromString("root/signal1/shadow"));
            var existingDatum = new Dto.Datum[]

            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (decimal)1m },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1),  Value = (decimal)2m },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1),  Value = (decimal)5m }
            };
            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 6, 1),  Value = (decimal)3m },
            };

            List<Datum<decimal>> existingDatumFirst = new List<Datum<decimal>>();
            existingDatumFirst.Add(new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 6, 1), Value = 3m });

            List<Datum<decimal>> existingDatumSecond = new List<Datum<decimal>>();
            existingDatumSecond.Add(new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1m });
            existingDatumSecond.Add(new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m });
            existingDatumSecond.Add(new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5m });


            signalsRepositoryMock = new Mock<ISignalsRepository>();
            GivenASignal(existingSignal);           
            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
            signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<decimal>(It.Is<Domain.Signal>(x => x.Id == 2), new DateTime(2000, 6, 1), new DateTime(2000, 7, 1))).Returns(existingDatumFirst);
            choiseDataType(existingSignal, existingDatum, new DateTime(2000, 6, 1), new DateTime(2000, 7, 1));
            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ShadowMissingValuePolicyDecimal()
                {
                    ShadowSignal = shadowSignal
                });
            var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);


            SetupZeroQuality(existingSignal, new DateTime(2000, 6, 1), new DateTime(2000, 7, 1),filledDatum);
        }


        [TestMethod]
        public void GivenASignalAndDatum_Boolean_ReturnDatumShadow()
        {
            var existingSignal = SignalWith(1, DataType.Boolean, Granularity.Month, Path.FromString("root/signal1"));
            var shadowSignal = SignalWith(2, DataType.Boolean, Granularity.Month, Path.FromString("root/signal1/shadow"));
            var existingDatum = new Dto.Datum[]

            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (bool)true },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1),  Value = (bool)false },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1),  Value = (bool)false }
            };
            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1),  Value = (bool)false },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 6, 1),  Value = (bool)true },
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 7, 1),  Value = false },
            };

            List<Datum<bool>> existingDatumFirst = new List<Datum<bool>>();
            existingDatumFirst.Add(new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = false });
            existingDatumFirst.Add(new Datum<bool>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 6, 1), Value = true });
            existingDatumFirst.Add(new Datum<bool>() { Quality = Quality.None, Timestamp = new DateTime(2000, 7, 1), Value = false });

            signalsRepositoryMock = new Mock<ISignalsRepository>();
            GivenASignal(existingSignal);
            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
            signalsDataRepositoryMock.Setup(sdrm => sdrm.GetData<bool>(It.Is<Domain.Signal>(x => x.Id == 2), new DateTime(2000, 5, 1), new DateTime(2000, 8, 1))).Returns(existingDatumFirst);
            choiseDataType(existingSignal, existingDatum, new DateTime(2000, 5, 1), new DateTime(2000, 8, 1));
            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ShadowMissingValuePolicyBoolean()
                {
                    ShadowSignal = shadowSignal
                });
            var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);


            SetupZeroQuality(existingSignal, new DateTime(2000, 5, 1), new DateTime(2000, 8, 1), filledDatum);
        }


        private void SetupZeroQuality(Signal existingSignal,
                DateTime fromIncludedUtc, DateTime toExcludedUtc,
                Dto.Datum[] filledDatum)
        {       
            var result = signalsWebService.GetData(existingSignal.Id.Value, fromIncludedUtc, toExcludedUtc);
            AssertDatum(result, filledDatum);
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

        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

    }
}
