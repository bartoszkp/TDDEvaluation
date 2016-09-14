using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebService.Tests
{
    [TestClass]
    public class SignalDataTests
    {
        private SignalsWebService signalsWebService;


        [TestMethod]
        [ExpectedException(typeof(NoSuchSignalException))]
        public void SignalNotExists_GetData_ThrowsException()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns((Signal)null);

            var result = signalsWebService.GetData(1, new DateTime(), new DateTime());

        }

        [TestMethod]
        public void SignalHasNoData_GetData_NullIsReturned()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(new Signal() { Id = 1, DataType = DataType.Double });

            signalsDataRepoMock.Setup(sd => sd.GetData<double>(It.Is<Signal>(s => s.Id == 1),
                                                               It.IsAny<DateTime>(),
                                                               It.IsAny<DateTime>()))
                                                                .Returns((IEnumerable<Datum<double>>)null);


            var result = signalsWebService.GetData(1, new DateTime(2016, 1, 1), new DateTime(2016, 3, 1));

            Assert.IsNull(result);

        }

        [TestMethod]
        public void SignalHasData_GetData_DataIsReturned()
        {
            SetupWebService();
            IEnumerable<Datum<double>> resultData = new[] { new Domain.Datum<double>() { Quality = Quality.Fair,
                                                                 Timestamp = new DateTime(2000, 1, 1),
                                                                 Value = 1.2 },

                                                                 new Domain.Datum<double>() { Quality = Quality.Fair,
                                                                 Timestamp = new DateTime(2000, 2, 1),
                                                                 Value = 1.5 },

                                                                 new Domain.Datum<double>() { Quality = Quality.Fair,
                                                                 Timestamp = new DateTime(2000, 1, 1),
                                                                 Value = 2.4 }
                                                          };

            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(new Signal() { Id = 1, DataType = DataType.Double });

            signalsDataRepoMock.Setup(sd => sd.GetData<double>(It.Is<Signal>(s => s.Id == 1 && s.DataType == DataType.Double),
                                                               It.IsAny<DateTime>(),
                                                               It.IsAny<DateTime>()))
                                                                .Returns(resultData);

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Dto.Datum>));
        }


        [TestMethod]
        [ExpectedException(typeof(NoSuchSignalException))]
        public void SignalNotExists_SetSignalData_ThrowsException()
        {
            SetupWebService();

            Mock<IEnumerable<Dto.Datum>> signalDataMock = new Mock<IEnumerable<Dto.Datum>>();

            signalsWebService.SetData(1, signalDataMock.Object);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GivenNullData_SetSignalData_ThrowsException()
        {
            SetupWebService(new Signal() { Id = 1 });
            signalsWebService.SetData(1, null);
        }

        [TestMethod]
        public void WhenGettingSignalData_ReturnsItSortedByDate()
        {
            var datums = new[]
            {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new System.DateTime(2000, 2, 1), Value = 1.51 },
                    new Datum<double>() { Quality = Quality.Fair, Timestamp = new System.DateTime(2000, 1, 1), Value = 1.45 },
                    new Datum<double>() { Quality = Quality.Poor, Timestamp = new System.DateTime(2000, 3, 1), Value = 2.47 }
            };
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = Path.FromString("sfd/pk")
            };
            SetupMocks(datums, signal);
            
            var result = signalsWebService.GetData(1, System.DateTime.MinValue, System.DateTime.MaxValue);
            var datum = datums[1];
            datums[1] = datums[0];
            datums[0] = datum;

            int i = 0;
            foreach (var d in result)
            {
                Assert.AreEqual(datums[i++].Timestamp, d.Timestamp);
            }

        }

        [TestMethod]
        public void NoneQualityMissingValuePolicy_ShouldFillMissingData()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Month
            };
            var datums = new Datum<int>[]
            {
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 2 }
            };
            SetupMocks(datums, signal);

            var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
            foreach (var d in result)
                if (d.Timestamp == new DateTime(2000, 2, 1))
                    return;

            Assert.Fail();
        }

        [TestMethod]
        public void WhenGettingDataForFromUtcSameAsToUtc_ReturnsSingleDatum()
        {
            var datums = new Datum<int>[]
            {
                new Datum<int>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = 1 },
            };
            var signal = new Domain.Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Month,
                Path = Domain.Path.FromString("example/path"),
            };

            SetupMocks<int>(datums, signal);

            var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 1, 1));

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(Dto.Quality.Bad, result.ElementAt(0).Quality);
            Assert.AreEqual(new DateTime(2000, 1, 1), result.ElementAt(0).Timestamp);
            Assert.AreEqual(1, result.ElementAt(0).Value);
        }

        [TestMethod]
        public void WhenGettingWithGivenPathPrefix_CorrectlyReturnsSubPaths()
        {
            var domainSignalsToReturn = new Domain.Signal[]
            {
                new Signal() { Id = 1, DataType = DataType.Boolean, Granularity = Granularity.Year, Path = Domain.Path.FromString("root/signal1") },
                new Signal() { Id = 2, DataType = DataType.Boolean, Granularity = Granularity.Month, Path = Domain.Path.FromString("root/signal1/signal2") },
                new Signal() { Id = 3, DataType = DataType.Boolean, Granularity = Granularity.Month, Path = Domain.Path.FromString("root/signal1/signal2/signal3") },
                new Signal() { Id = 4, DataType = DataType.Decimal, Granularity = Granularity.Week, Path = Domain.Path.FromString("root/signal1/signal2/signal3/signal4") },
            };
            SetupMocksGetPath(domainSignalsToReturn);

            var pathDto = new Dto.Path() { Components = new[] { "root", "signal1" } };
            var result = signalsWebService.GetPathEntry(pathDto);

            var expectedSubPaths = new Dto.Path[]
            {
                new Dto.Path() {Components = new[] {"root", "signal1", "signal2" } },
            };

            var actualResultA = result.SubPaths.ToArray();

            int i = 0;
            foreach (var actualItem in actualResultA)
            {
                CollectionAssert.AreEqual(expectedSubPaths[i].Components.ToArray(), actualItem.Components.ToArray());

                i++;
            }
        }

        [TestMethod]
        public void WhenGettingWithGivenPathPrefix_ReturnsSignals_ContainedInSpecifiedPath()
        {
            var domainSignalsToReturn = new Domain.Signal[]
            {
                new Signal() { Id = 1, DataType = DataType.Boolean, Granularity = Granularity.Year, Path = Domain.Path.FromString("root/signal1") },
                new Signal() { Id = 2, DataType = DataType.Boolean, Granularity = Granularity.Month, Path = Domain.Path.FromString("root/signals1/signal2") },
                new Signal() { Id = 3, DataType = DataType.Boolean, Granularity = Granularity.Month, Path = Domain.Path.FromString("root/signals1/signal3") },
                new Signal() { Id = 4, DataType = DataType.Decimal, Granularity = Granularity.Week, Path = Domain.Path.FromString("root/signals2/signals1/") },
            };
            SetupMocksGetPath(domainSignalsToReturn);

            var pathDto = new Dto.Path() { Components = new[] { "root", "signals1" } };
            var result = signalsWebService.GetPathEntry(pathDto);

            var dtoSignals = new Dto.Signal[]
            {
                new Dto.Signal() { Id = 1, DataType = Dto.DataType.Boolean, Granularity = Dto.Granularity.Year, Path =  new Dto.Path() { Components = new[] { "root", "signal1" } } },
                new Dto.Signal() { Id = 2, DataType = Dto.DataType.Boolean, Granularity = Dto.Granularity.Month, Path = new Dto.Path() { Components = new[] { "root", "signals1", "signal2" } } },
                new Dto.Signal() { Id = 3, DataType = Dto.DataType.Boolean, Granularity = Dto.Granularity.Month, Path = new Dto.Path() { Components = new[] { "root", "signals1", "signal3" } } },
                new Dto.Signal() { Id = 4, DataType = Dto.DataType.Decimal, Granularity = Dto.Granularity.Week, Path = new Dto.Path() { Components = new[] { "root", "signals2", "signals1", "" } } },
            };
            var actualResultA = result.Signals.ToArray();
            Assert.AreEqual(2, actualResultA.Count());

            int id = 1;
            foreach (var signal in actualResultA)
            {
                Assert.AreEqual(dtoSignals[id].DataType, signal.DataType);
                Assert.AreEqual(dtoSignals[id].Granularity, signal.Granularity);
                Assert.AreEqual(dtoSignals[id].Id, signal.Id);
                CollectionAssert.AreEqual(dtoSignals[id].Path.Components.ToArray(), signal.Path.Components.ToArray());

                id++;
            }
        }

        [TestMethod]
        public void SpecificValueMissingValuePolicyIsSet_GettingData_FillsMissingData_WithSpecificPolicy()
        {
            SetupMocksSpecificPolicy();

            var expectedDatumsResult = new Dto.Datum[]
            {
                new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = 12.5 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = 13.5 },
                new Dto.Datum() { Quality = Dto.Quality.Bad, Timestamp = new DateTime(2000, 3, 1), Value = 14.5 },
            };

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
            Assert.AreEqual(expectedDatumsResult.Count(), result.Count());

            int i = 0;
            foreach (var datum in result)
            {
                Assert.AreEqual(expectedDatumsResult[i].Quality, datum.Quality);
                Assert.AreEqual(expectedDatumsResult[i].Timestamp, datum.Timestamp);
                Assert.AreEqual(expectedDatumsResult[i].Value, datum.Value);

                i++;
            }
        }

        [TestMethod]
        public void ZeroOrderValueMissingValuePolicy_ShouldFillMissingData()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Month
            };

            var datums = new Datum<double>[]
            {
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1.5 },
                    new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 2.5 }
            };

            var expectedDatums = new Dto.Datum[]
            {
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                   new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = (double)2.5 }
            };

            SetupWebService(signal);
            SetupMocksForZero<double>(datums, signal);

            var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2000, 1, 1), new DateTime(2000, 4, 1));
            Assert.AreEqual(expectedDatums.Count(), result.Count());

            Assert.AreEqual(result.ElementAt(1).Value, expectedDatums[1].Value);
        }

        [TestMethod]
        public void TestFixedBug_SpecificValuepolicyWork_WhenInTheSameTimeStamp_ReturmOneElement()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Boolean,
                Granularity = Granularity.Day,
                Path = Path.FromString("r/vbc")
            };

            var policy = new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyBoolean()
            {
                Value = true,
                Quality = Quality.Good
            };
            List<Datum<bool>> existingDatumFirst = new List<Datum<bool>>();
            existingDatumFirst.Add(new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2018, 12, 12), Value = true });


            var filledDatum = new Dto.Datum[]
            {
                        new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2018, 12, 12),  Value = (bool)true }
            };
            signalsRepoMock = new Mock<ISignalsRepository>();
            GivenASignal(signal);
            signalsDataRepoMock = new Mock<ISignalsDataRepository>();
            signalsDataRepoMock.Setup(sdrm => sdrm.GetData<bool>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2018, 12, 12), new DateTime(2018, 12, 12)))
                            .Returns(existingDatumFirst);
            missingValueRepoMock = new Mock<IMissingValuePolicyRepository>();
            missingValueRepoMock
                .Setup(mvprm => mvprm.Get(It.IsAny<Domain.Signal>()))
                .Returns(policy);
            var signalsDomainService = new SignalsDomainService(
                    signalsRepoMock.Object,
                    signalsDataRepoMock.Object,
                    missingValueRepoMock.Object);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetData(signal.Id.Value, new DateTime(2018, 12, 12), new DateTime(2018, 12, 12));
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
        [ExpectedException(typeof(NoSuchSignalException))]
        public void SignalNotExists_GetCoarseData_ThrowsException()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns((Signal)null);

            var result = signalsWebService.GetCoarseData<bool>(1,new Dto.Granularity(), new DateTime(), new DateTime());

        }

        [TestMethod]
        public void GivenASignal_WhenTimeStampIsEmpty_ReturmEmptyList()
        {
            SetupWebService();
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(new Signal() { Id = 1, DataType = DataType.Double });

            var result = signalsWebService.GetCoarseData<double>(1, Dto.Granularity.Day, new DateTime(2000, 2, 1), new DateTime(2000, 1, 1));

            Assert.AreEqual(result.Count(),0);
        }

        [TestMethod]
        [ExpectedException(typeof(NoSuchGranularityException))]
        public void GivenASignal_GetCoarseDataWhenGranularityGreaterThanSignalGranularity_ThrowsException()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Boolean,
                Granularity = Granularity.Day,
                Path = Path.FromString("r/vbc")
            };
            List<Datum<bool>> existingDatumFirst = new List<Datum<bool>>();
            existingDatumFirst.Add(new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2018, 12, 12), Value = true });
            existingDatumFirst.Add(new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2018, 12, 13), Value = true });
            existingDatumFirst.Add(new Datum<bool>() { Quality = Quality.Good, Timestamp = new DateTime(2018, 12, 14), Value = true });

            signalsRepoMock = new Mock<ISignalsRepository>();
            GivenASignal(signal);
            signalsDataRepoMock = new Mock<ISignalsDataRepository>();
            signalsDataRepoMock.Setup(sdrm => sdrm.GetData<bool>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2018, 12, 12), new DateTime(2018, 12, 15)))
                            .Returns(existingDatumFirst);
            var signalsDomainService = new SignalsDomainService(
                    signalsRepoMock.Object,
                    signalsDataRepoMock.Object,
                    null);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetCoarseData<bool>(1, Dto.Granularity.Second, new DateTime(2018, 12, 12), new DateTime(2018, 12, 15));
        }

        [TestMethod]
        public void GivenASignalAndDatum_WhenGetCoarseDataWithInTheSameGranularity_ReturnThisDatum()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Day,
                Path = Path.FromString("r/vbc")
            };

            List<Datum<double>> existingDatumFirst = new List<Datum<double>>();
            existingDatumFirst.Add(new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2018, 12, 12), Value = 1.0 });
            existingDatumFirst.Add(new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2018, 12, 13), Value = 2.0 });
            existingDatumFirst.Add(new Datum<double>() { Quality = Quality.Good, Timestamp = new DateTime(2018, 12, 14), Value = 3.0 });

            signalsRepoMock = new Mock<ISignalsRepository>();
            GivenASignal(signal);
            signalsDataRepoMock = new Mock<ISignalsDataRepository>();
            signalsDataRepoMock.Setup(sdrm => sdrm.GetData<double>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2018, 12, 12), new DateTime(2018, 12, 15)))
                            .Returns(existingDatumFirst);
            
            var signalsDomainService = new SignalsDomainService(
                    signalsRepoMock.Object,
                    signalsDataRepoMock.Object,
                    null);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetCoarseData<double>(signal.Id.Value,Dto.Granularity.Day, new DateTime(2018, 12, 12), new DateTime(2018, 12, 15));
            
            int index = 0;
            foreach (var fd in existingDatumFirst)
            {
                Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality.ToDomain<Domain.Quality>());
                Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp.ToDomain<DateTime>());
                Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                index++;
            }
        }

        [TestMethod]
        public void GivenASignalGranularityDayAndDatumWihoutPolicy_WhenGetCoarseDataWithNewGranuralityWeek_ReturnNewGranularity()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Day,
                Path = Path.FromString("r/vbc")
            };

            List<Datum<int>> existingDatum = new List<Datum<int>>();
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 4), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 5), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 6), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 7), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 8), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 9), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 10), Value = 1 });

            existingDatum.Add(new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 1, 11), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 12), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 13), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 14), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 15), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 16), Value = 2 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 17), Value = 1 });

            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 18), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 19), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 20), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Bad, Timestamp = new DateTime(2016, 1, 21), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 22), Value = 0 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 23), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 24), Value = 0 });

            List<Datum<int>> filledDatum = new List<Datum<int>>();
            filledDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 4), Value = 1 });
            filledDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 11), Value = 4 });
            filledDatum.Add(new Datum<int>() { Quality = Quality.Bad, Timestamp = new DateTime(2016, 1, 18), Value = 3 });
            signalsRepoMock = new Mock<ISignalsRepository>();
            GivenASignal(signal);
            signalsDataRepoMock = new Mock<ISignalsDataRepository>();
            signalsDataRepoMock.Setup(sdrm => sdrm.GetData<int>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2016, 1, 4), new DateTime(2016, 1, 24)))
                            .Returns(existingDatum);

            var signalsDomainService = new SignalsDomainService(
                    signalsRepoMock.Object,
                    signalsDataRepoMock.Object,
                    null);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetCoarseData<double>(signal.Id.Value, Dto.Granularity.Week, new DateTime(2016, 1, 4), new DateTime(2016, 1, 24));

            int index = 0;
            foreach (var fd in filledDatum)
            {
                Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality.ToDomain<Domain.Quality>());
                Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp.ToDomain<DateTime>());
                Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                index++;
            }
        }

        [TestMethod]
        public void GivenASignalGranuralityDayAndDatumWithoutPolicy_WhenGetCoarseDataWithNewGranuralityMonth_ReturnNewList()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Day,
                Path = Path.FromString("r/vbc")
            };
            List<Datum<int>> existingDatum = new List<Datum<int>>();
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 1), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 2), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 3), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 4), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 5), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 6), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 7), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 8), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 9), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 10), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2016, 1, 11), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 12), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 13), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 14), Value = 2 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 15), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 16), Value = 2 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 17), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 18), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 19), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 20), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 21), Value = 0 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 22), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 23), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 24), Value = 2 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 25), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 26), Value = 2 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 27), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 28), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 29), Value = 5 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 30), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 31), Value = 0 });

            List<Datum<int>> filledDatum = new List<Datum<int>>();
            filledDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 1), Value = 2 });
            signalsRepoMock = new Mock<ISignalsRepository>();
            GivenASignal(signal);
            signalsDataRepoMock = new Mock<ISignalsDataRepository>();
            signalsDataRepoMock.Setup(sdrm => sdrm.GetData<int>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2016, 1, 1), new DateTime(2016, 1, 31)))
                            .Returns(existingDatum);
            var signalsDomainService = new SignalsDomainService(
                    signalsRepoMock.Object,
                    signalsDataRepoMock.Object,
                    null);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetCoarseData<double>(signal.Id.Value, Dto.Granularity.Month, new DateTime(2016, 1, 1), new DateTime(2016, 1, 31));

            int index = 0;
            foreach (var fd in filledDatum)
            {
                Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality.ToDomain<Domain.Quality>());
                Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp.ToDomain<DateTime>());
                Assert.AreEqual(fd.Value, result.ElementAt(index).Value);
                index++;
            }
        }


        [TestMethod]
        public void GivenASignalGranularityDayAndDatumWihoutPolicy_WhenGetCoarseDataWithNewGranuralityWeek_AndTimeStampInTheSame_ReturnNewGranularity()
        {
            var signal = new Signal()
            {
                Id = 1,
                DataType = DataType.Integer,
                Granularity = Granularity.Day,
                Path = Path.FromString("r/vbc")
            };

            List<Datum<int>> existingDatum = new List<Datum<int>>();
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 4), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 5), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 6), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 7), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 8), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 9), Value = 1 });
            existingDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 10), Value = 1 });

            List<Datum<int>> filledDatum = new List<Datum<int>>();
            filledDatum.Add(new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2016, 1, 4), Value = 1 });
            signalsRepoMock = new Mock<ISignalsRepository>();
            GivenASignal(signal);
            signalsDataRepoMock = new Mock<ISignalsDataRepository>();
            signalsDataRepoMock.Setup(sdrm => sdrm.GetData<int>(It.Is<Domain.Signal>(x => x.Id == 1), new DateTime(2016, 1, 4), new DateTime(2016, 1, 10)))
                            .Returns(existingDatum);

            var signalsDomainService = new SignalsDomainService(
                    signalsRepoMock.Object,
                    signalsDataRepoMock.Object,
                    null);
            signalsWebService = new SignalsWebService(signalsDomainService);
            var result = signalsWebService.GetCoarseData<double>(signal.Id.Value, Dto.Granularity.Week, new DateTime(2016, 1, 4), new DateTime(2016, 1, 4));

            int index = 0;
            foreach (var fd in filledDatum)
            {
                Assert.AreEqual(fd.Quality, result.ElementAt(index).Quality.ToDomain<Domain.Quality>());
                Assert.AreEqual(fd.Timestamp, result.ElementAt(index).Timestamp.ToDomain<DateTime>());
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
        private void SetupWebService(Signal signal=null)
        {
            signalsDataRepoMock = new Mock<ISignalsDataRepository>();
            signalsRepoMock = new Mock<ISignalsRepository>();
            missingValueRepoMock = new Mock<IMissingValuePolicyRepository>();
            SignalsDomainService domainService = new SignalsDomainService(
                signalsRepoMock.Object, signalsDataRepoMock.Object, missingValueRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);
            
            signalsRepoMock
                .Setup(sr => sr.Get(It.IsAny<int>()))
                .Returns(signal);
        }

        private void SetupMocks<T>(Datum<T>[] datums, Signal signal)
        {
            SetupWebService(signal);
            missingValueRepoMock
                .Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.NoneQualityMissingValuePolicyInteger());
            signalsDataRepoMock
                .Setup(dr => dr.GetData<T>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(datums);
        }

        private void SetupMocksForZero<T>(Datum<T>[] datums, Signal signal)
        {
            SetupWebService(signal);
            missingValueRepoMock
                .Setup(mvpr => mvpr.Get(It.IsAny<Signal>()))
                .Returns(new DataAccess.GenericInstantiations.ZeroOrderMissingValuePolicyDouble());
            signalsDataRepoMock
                .Setup(dr => dr.GetData<T>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(datums);
        }


        private void SetupMocksGetPath(IEnumerable<Signal> domainSignalsToReturn)
        {
            SetupWebService();
            signalsRepoMock
                .Setup(sdrm => sdrm.GetAllWithPathPrefix(It.IsAny<Domain.Path>()))
                .Returns(domainSignalsToReturn);
        }

        private void SetupMocksSpecificPolicy()
        {
            var exampleSignal = new Domain.Signal()
            {
                Id = 1,
                DataType = DataType.Double,
                Granularity = Granularity.Month,
                Path = Domain.Path.FromString("example/path"),
            };

            SetupWebService(exampleSignal);

            missingValueRepoMock
                .Setup(mvrm => mvrm.Get(It.IsAny<Domain.Signal>()))
                .Returns(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyDouble()
                {
                    Id = 1,
                    Quality = Quality.Good,
                    Signal = exampleSignal,
                    Value = 13.5,
                });

            var datums = new Datum<double>[]
            {
                new Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 1, 1), Value = 12.5 },
                new Datum<double>() { Quality = Quality.Bad, Timestamp = new DateTime(2000, 3, 1), Value = 14.5 },
            };

            signalsDataRepoMock
                .Setup(srm => srm.GetData<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(datums);
        }

        private Mock<ISignalsDataRepository> signalsDataRepoMock;
        private Mock<ISignalsRepository> signalsRepoMock;
        private Mock<IMissingValuePolicyRepository> missingValueRepoMock;

    }
}
