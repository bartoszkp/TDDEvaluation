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
    public class FirstOrderDataFillTests
    {
        SignalsWebService signalsWebService;
        [TestMethod]
        public void GivenASignalAndDatum_Double_ReturnDatumWithFirstOrder()
        {
            var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1),  Value = (double)2 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1),  Value = (double)5 }
            };
           
            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(1999, 11, 1),  Value = (double)0 },
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(1999, 12, 1),  Value = (double)0 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)1.25 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1),  Value = (double)1.75 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1),  Value = (double)2},
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 6, 1),  Value = (double)3},
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 7, 1),  Value = (double)4},
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1),  Value = (double)5},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 9, 1),  Value = (double)0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 10, 1),  Value = (double)0},
            };
            SetupZeroQuality(existingSignal, existingDatum, new DateTime(1999, 11, 1), new DateTime(2000, 11, 1),
                new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDouble(), filledDatum);
        }


        [TestMethod]
        public void GivenASignalAndDatum_Decimal_ReturnOneElement()
        {
            var existingSignal = SignalWith(1, DataType.Decimal, Granularity.Month, Path.FromString("root/signal1"));
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
            SetupZeroQuality(existingSignal, existingDatum, new DateTime(2000, 6, 1), new DateTime(2000, 7, 1),
                new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyDecimal(), filledDatum);
        }

        [TestMethod]
        public void GivenASignalAndDatum_Integer_ReturnOneElement()
        {
            var existingSignal = SignalWith(1, DataType.Integer, Granularity.Month, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]

            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (int)1 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1),  Value = (int)2 },
                        new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1),  Value = (int)5 }
            };
            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 9, 1),  Value = (int)0 },
            };
            SetupZeroQuality(existingSignal, existingDatum, new DateTime(2000, 9, 1), new DateTime(2000, 10, 1),
                new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyInteger(), filledDatum);
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
        private void SetupZeroQuality(Signal existingSignal, Dto.Datum[] existingDatum,
                DateTime fromIncludedUtc, DateTime toExcludedUtc,
                Domain.MissingValuePolicy.MissingValuePolicyBase missingValuePolicyBase,
                Dto.Datum[] filledDatum)
        {
           
            signalsRepositoryMock = new Mock<ISignalsRepository>();
            GivenASignal(existingSignal);
            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();           
            choiseDataType(existingSignal, existingDatum, fromIncludedUtc, toExcludedUtc);
            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
            missingValuePolicyRepositoryMock
                .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(missingValuePolicyBase);
            var choiseSetupMock = new Dictionary<DataType, Action>()
            {
                {DataType.Decimal, ()=>DataTypeDecimal_Setup() },
                {DataType.Double, ()=>DataTypeDouble_Setup() },
                {DataType.Integer,()=>DataTypeInteger_Setup() }
            };
            choiseSetupMock[existingSignal.DataType].Invoke();
            
            var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetData(existingSignal.Id.Value, fromIncludedUtc, toExcludedUtc);
            AssertDatum(result, filledDatum);

        }

        private void DataTypeDouble_Setup()
        {
            List<Datum<double>> existingDatumFirst = new List<Datum<double>>();
            existingDatumFirst.Add(new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1 });
            List<Datum<double>> existingDatumSecond = new List<Datum<double>>();
            existingDatumSecond.Add(new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2 });
            List<Datum<double>> existingDatumThird = new List<Datum<double>>();
            existingDatumThird.Add(new Datum<double>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5 });

            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 2, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 3, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 4, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 5, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 6, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 7, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 8, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 9, 1), 1)).Returns(existingDatumThird);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 10, 1), 1)).Returns(existingDatumThird);

            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(1999, 11, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(1999, 12, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 1, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 2, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 3, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 4, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 5, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 6, 1), 1)).Returns(existingDatumThird);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 7, 1), 1)).Returns(existingDatumThird);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 8, 1), 1)).Returns(existingDatumThird);

        }

        private void DataTypeDecimal_Setup()
        {
            List<Datum<decimal>> existingDatumFirst = new List<Datum<decimal>>();
            existingDatumFirst.Add(new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1m });
            List<Datum<decimal>> existingDatumSecond = new List<Datum<decimal>>();
            existingDatumSecond.Add(new Datum<decimal>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2m });
            List<Datum<decimal>> existingDatumThird = new List<Datum<decimal>>();
            existingDatumThird.Add(new Datum<decimal>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5m });

            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 2, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 3, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 4, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 5, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 6, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 7, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 8, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 9, 1), 1)).Returns(existingDatumThird);
            signalsDataRepositoryMock.Setup(s => s.GetDataOlderThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 10, 1), 1)).Returns(existingDatumThird);

            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(1999, 11, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(1999, 12, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 1, 1), 1)).Returns(existingDatumFirst);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 2, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 3, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 4, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 5, 1), 1)).Returns(existingDatumSecond);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 6, 1), 1)).Returns(existingDatumThird);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 7, 1), 1)).Returns(existingDatumThird);
            signalsDataRepositoryMock.Setup(s => s.GetDataNewerThan<decimal>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2000, 8, 1), 1)).Returns(existingDatumThird);

        }

        private void DataTypeInteger_Setup()
        {

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
