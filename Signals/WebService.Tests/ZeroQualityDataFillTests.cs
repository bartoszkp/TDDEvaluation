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
    public class ZeroQualityDataFillTests
    {
        SignalsWebService signalsWebService;


        [TestMethod]
        public void ZeroQuality_WhenGet_ReturnFillData_Double_Month()
        {
            var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0 }
            };
            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1),  Value = (double)2.0}
            };
            SetupZeroQuality(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1), 
                new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble(),filledDatum);

        }

        [TestMethod]
        public void ZeroQuality_WhenGet_ReturnFillData_Double_Day()
        {
            var existingSignal = SignalWith(1, DataType.Double, Granularity.Day, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2),  Value = (double)2.0 }
            };
            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3),  Value = (double)2.0}
            };
            SetupZeroQuality(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 1, 4),
                new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble(), filledDatum);


        }

        [TestMethod]
        public void ZeroQuality_WhenGet_ReturnFillData_Integer_Second()
        {
            var existingSignal = SignalWith(1, DataType.Integer, Granularity.Second, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,0,0,1),  Value = (int)1 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,0,0,2),  Value = (int)2 }
            };
            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,0,0,1),  Value = (int)1 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,0,0,2),  Value = (int)2},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,0,0,3),  Value = (int)2}
            };
            SetupZeroQuality(existingSignal, existingDatum, new DateTime(2000, 1, 1, 0, 0, 1), new DateTime(2000, 1, 1, 0, 0, 4),
                new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger(), filledDatum);

        }

        [TestMethod]
        public void ZeroQuality_WhenGet_ReturnFillData_Integer_Minute()
        {
            var existingSignal = SignalWith(1, DataType.Integer, Granularity.Minute, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,0,1,0),  Value = (int)1 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,0,2,0),  Value = (int)2 }
            };
            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,0,1,0),  Value = (int)1 },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,0,2,0),  Value = (int)2},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1,0,3,0),  Value = (int)2}
            };
            SetupZeroQuality(existingSignal, existingDatum, new DateTime(2000, 1, 1, 0, 1, 0), new DateTime(2000, 1, 1, 0, 4, 0),
                new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyInteger(), filledDatum);

        }

        [TestMethod]
        public void ZeroQuality_WhenGet_ReturnFillData_Decimal_Year()
        {
            var existingSignal = SignalWith(1, DataType.Decimal, Granularity.Year, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (decimal)1m },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (decimal)2m }
            };
            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (decimal)1m },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2001, 1, 1),  Value = (decimal)2m},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (decimal)2m}
            };
            SetupZeroQuality(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2003, 1, 1),
                new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal(), filledDatum);

        }

        [TestMethod]
        public void ZeroQuality_WhenGet_ReturnFillData_Decimal_Week()
        {
            var existingSignal = SignalWith(1, DataType.Decimal, Granularity.Week, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 9, 5),  Value = (decimal)1m },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 9, 12),  Value = (decimal)2m }
            };
            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 9, 5),  Value = (decimal)1m },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 9, 12),  Value = (decimal)2m},
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 9, 19),  Value = (decimal)2m}
            };
            SetupZeroQuality(existingSignal, existingDatum, new DateTime(2016, 9, 5), new DateTime(2016, 9, 26),
                new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDecimal(), filledDatum);
        }

        [TestMethod]
        public void TestFixedBug_TaskZeroOrderPolicyCrashes()
        {
            var existingSignal = SignalWith(1, DataType.Boolean, Granularity.Day, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1),  Value = (bool)false },
                        new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 5),  Value = (bool)true }
            };
            var filledDatum = new Dto.Datum[]
            {
                       new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1),  Value = (bool)false },
                       new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 2),  Value = (bool)false },
                       new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 3),  Value = (bool)false },
                       new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 4),  Value = (bool)false },
                       new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 5),  Value = (bool)true },
                       new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 6),  Value = (bool)true },
                       new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 7),  Value = (bool)true },
                       new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 8),  Value = (bool)true },
                       new Dto.Datum {Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 9),  Value = (bool)true }
            };
            SetupZeroQuality(existingSignal, existingDatum, new DateTime(2000, 1, 1), new DateTime(2000, 1, 10),
                new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyBoolean(), filledDatum);
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
            var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetData(existingSignal.Id.Value, fromIncludedUtc, toExcludedUtc);
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
