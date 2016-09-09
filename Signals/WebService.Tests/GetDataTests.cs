using Domain.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Domain.Services.Implementation;
using Domain;
using Domain.Exceptions;
using Domain.MissingValuePolicy;
using Dto.Conversions;

namespace WebService.Tests
{
    [TestClass]
    public class GetDataTests
    {

        private SignalsWebService signalsWebService;


        [TestMethod]
        [ExpectedException(typeof(NoSuchSignalException))]
        public void SignalNotInDatabase_GetData_ThrowsException()
        {
            SetupWebService();
            signalsRepositoryMock.Setup(sr => sr.Get(1)).Returns((Signal)null);
            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 2, 1));
        }



        [TestMethod]
        public void SignalExists_GetData_WithSameTimestaps_SingleDatumReturned()
        {
            var existingSignal = SignalWith(1, DataType.Double, Granularity.Month, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1),  Value = default(double) },
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1),  Value = (double)2.0},
                        new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1),  Value = default(double)}
            };
            signalsRepositoryMock = new Mock<ISignalsRepository>();
            GivenASignal(existingSignal);
            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
            choiseDataType(existingSignal, existingDatum, new DateTime(2000, 2, 1), new DateTime(2000, 2, 1));
            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
            missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble());
            var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 2, 1), new DateTime(2000, 2, 1));
            Assert.AreEqual(result.ElementAt(0).Quality, Dto.Quality.Good);
            Assert.AreEqual(result.ElementAt(0).Timestamp, new DateTime(2000, 2, 1));
            Assert.AreEqual(result.ElementAt(0).Value, (double)2.0);
        }


        #region TimeStampVerifyTests
        [ExpectedException(typeof(TimestampHaveWrongFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Second_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Second
            };

            signalsRepositoryMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 1, 1, 1, 11, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [ExpectedException(typeof(TimestampHaveWrongFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Minute_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Minute
            };

            signalsRepositoryMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 1, 1, 0, 1, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [ExpectedException(typeof(TimestampHaveWrongFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Hour_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Hour
            };

            signalsRepositoryMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 1, 0, 0, 1, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [ExpectedException(typeof(TimestampHaveWrongFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Day_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Day
            };

            signalsRepositoryMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0, 1, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [ExpectedException(typeof(TimestampHaveWrongFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Month_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Month
            };

            signalsRepositoryMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [ExpectedException(typeof(TimestampHaveWrongFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Week_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Week
            };

            signalsRepositoryMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }

        [ExpectedException(typeof(TimestampHaveWrongFormatException))]
        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Year_Exception()
        {
            SetupWebService();

            var signal = new Signal()
            {
                Id = 1,
                DataType = Domain.DataType.Integer,
                Granularity = Domain.Granularity.Year
            };

            signalsRepositoryMock.Setup(x => x.Get(It.Is<int>(z => z == 1)))
                .Returns(signal);

            var data = signalsWebService.GetData(1, new DateTime(2000, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 2));
        }


        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Second()
        {
            var existingSignal = SignalWith(1, DataType.Double, Granularity.Second, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 3),  Value = (double)2.5 }
            };
            var filledDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 1, 1, 2),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 3),  Value = (double)2.5 }
            };
            SetupGetDataWithPolicy(existingSignal, existingDatum,
                new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 4),
                new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble(), filledDatum);

        }

        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Minute()
        {
            var existingSignal = SignalWith(1, DataType.Double, Granularity.Minute, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 0),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 3, 0),  Value = (double)2.5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 1, 0),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 1, 2, 0),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 3, 0),  Value = (double)2.5 }
            };
            SetupGetDataWithPolicy(existingSignal, existingDatum,
                new DateTime(2000, 1, 1, 1, 1, 0), new DateTime(2000, 1, 1, 1, 4, 0),
                new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble(), filledDatum);

        }

        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Hour()
        {
            var existingSignal = SignalWith(1, DataType.Double, Granularity.Hour, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0),  Value = (double)2.5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1, 2, 0, 0),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0),  Value = (double)2.5 }
            };
            SetupGetDataWithPolicy(existingSignal, existingDatum,
                new DateTime(2000, 1, 1, 1, 0, 0), new DateTime(2000, 1, 1, 4, 0, 0),
                new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble(), filledDatum);
        }

        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Day()
        {
            var existingSignal = SignalWith(1, DataType.Double, Granularity.Day, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3),  Value = (double)2.5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 2),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3),  Value = (double)2.5 }
            };
            SetupGetDataWithPolicy(existingSignal, existingDatum,
                new DateTime(2000, 1, 1), new DateTime(2000, 1, 4),
                new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble(), filledDatum);
        }


        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Month()
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
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 3, 1),  Value = default(double)}
            };
            SetupGetDataWithPolicy(existingSignal, existingDatum,
                new DateTime(2000, 1, 1), new DateTime(2000, 4, 1),
                new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble(), filledDatum);
            ;
        }

        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Week()
        {
            var existingSignal = SignalWith(1, DataType.Double, Granularity.Week, Path.FromString("root/signal1"));
            var existingDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 15),  Value = (double)2.5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2016, 8, 8),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 15),  Value = (double)2.5 }
            };
            SetupGetDataWithPolicy(existingSignal, existingDatum,
                new DateTime(2016, 8, 1), new DateTime(2016, 8, 22),
                new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble(), filledDatum);
        }


        [TestMethod]
        public void WhenGetData_VerifyTimeStamp_Year()
        {
            var existingSignal = SignalWith(1, DataType.Double, Granularity.Year, Path.FromString("root/signal1"));

            var existingDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (double)2.5 }
            };

            var filledDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (double)1.5 },
                    new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2001, 1, 1),  Value = default(double)},
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2002, 1, 1),  Value = (double)2.5 }
            };
            SetupGetDataWithPolicy(existingSignal, existingDatum,
                new DateTime(2000, 1, 1), new DateTime(2003, 1, 1),
                new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyDouble(), filledDatum);
        }
        [TestMethod]
        public void GivenASignal_WhenFormatTimeStampIsWrong_ReturnZeroElements()
        {
            var existingSignal = SignalWith(1, DataType.Boolean, Granularity.Day, Path.FromString("Bool"));
            var existingDatum = new Dto.Datum[]
            {
                    new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1),  Value = (bool)false }
            };


            signalsRepositoryMock = new Mock<ISignalsRepository>();
            GivenASignal(existingSignal);
            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
            choiseDataType(existingSignal, existingDatum, new DateTime(2000, 1, 3), new DateTime(2000, 1, 1));
            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
            missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyBoolean());
            var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetData(existingSignal.Id.Value, new DateTime(2000, 3, 1), new DateTime(2000, 1, 1));
            Assert.AreEqual(result.Count(), 0);
        }

        
        #endregion

        private void SetupWebService()
        {
            var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, signalsDataRepositoryMock.Object, missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
        }



        private void SetupGetDataWithPolicy(Signal existingSignal, Dto.Datum[] existingDatum,
                DateTime fromIncludedUtc, DateTime toExcludedUtc, Domain.MissingValuePolicy.MissingValuePolicyBase missingValuePolicyBase,
                Dto.Datum[] filledDatum)
        {
            signalsRepositoryMock = new Mock<ISignalsRepository>();
            GivenASignal(existingSignal);
            signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
            choiseDataType(existingSignal, existingDatum, fromIncludedUtc, toExcludedUtc);
            missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
            missingValuePolicyRepositoryMock.Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(missingValuePolicyBase);
            var signalsDomainService = new SignalsDomainService(
                    signalsRepositoryMock.Object,
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetData(existingSignal.Id.Value, fromIncludedUtc, toExcludedUtc);
            AssertDatum(result, filledDatum);
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

        private Mock<ISignalsDataRepository> signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
        private Mock<ISignalsRepository> signalsRepositoryMock = new Mock<ISignalsRepository>();
        private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();

    }
}
