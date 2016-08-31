using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain;
using System.Collections.Generic;
using Domain.Services.Implementation;
using Moq;
using Domain.Repositories;
using System.Linq;

namespace WebService.Tests
{
    [TestClass]
    public class FirstOrderFillTests
    {
        private Mock<ISignalsRepository> signalsRepoMock = new Mock<ISignalsRepository>();
        private Mock<ISignalsDataRepository> dataRepoMock = new Mock<ISignalsDataRepository>();
        private Mock<IMissingValuePolicyRepository> mvpRepoMock = new Mock<IMissingValuePolicyRepository>();

        private SignalsWebService signalsWebService;

        [TestMethod]
        public void GivenAnIntegerSecondSignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Second,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 0, 13), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 5), Value = (int)11 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 9), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 0, 12), Value = (int)20 }
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 0, 13));

            var expectedDatum = GetExpectedDatums(Granularity.Second);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerMinuteSignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Minute,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 13, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 5, 0), Value = (int)11 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 9, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 12, 0), Value = (int)20 }
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 0, 13, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Minute);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerHourSignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Hour,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 13, 0, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 1, 5, 0, 0), Value = (int)11 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 9, 0, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 12, 0, 0), Value = (int)20 }
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 1, 13, 0, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Hour);

            AssertEqual(expectedDatum, result);
        }

        [TestMethod]
        public void GivenAnIntegerDailySignal_WhenGettingDataWithCorrectRange_FirstOrderPolicy_CorrectlyFillsMissingData()
        {
            SetupFirstOrderPolicy(Granularity.Day,
                new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 14, 0, 0, 0), new List<Datum<int>>()
                {
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                    new Datum<int>() { Quality = Quality.Good, Timestamp = new DateTime(2000, 1, 6, 0, 0, 0), Value = (int)11 },
                    new Datum<int>() { Quality = Quality.Fair, Timestamp = new DateTime(2000, 1, 10, 0, 0, 0), Value = (int)5 },
                    new Datum<int>() { Quality = Quality.Poor, Timestamp = new DateTime(2000, 1, 13, 0, 0, 0), Value = (int)20 }
                });

            var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1, 0, 0, 0), new DateTime(2000, 1, 14, 0, 0, 0));

            var expectedDatum = GetExpectedDatums(Granularity.Day);

            AssertEqual(expectedDatum, result);
        }

        private void SetupFirstOrderPolicy(Granularity granularity,
            DateTime fromIncluded, DateTime toExluded, List<Datum<int>> actualToBeReturnedByMockDatums)
        {
            SignalsDomainService domainService = new SignalsDomainService(signalsRepoMock.Object, dataRepoMock.Object, mvpRepoMock.Object);
            signalsWebService = new SignalsWebService(domainService);

            var returnedSignal = new Signal() { Id = 1, Granularity = granularity, DataType = DataType.Integer };
            signalsRepoMock.Setup(sr => sr.Get(1)).Returns(returnedSignal);

            mvpRepoMock.Setup(m => m.Get(returnedSignal))
                .Returns(new DataAccess.GenericInstantiations.FirstOrderMissingValuePolicyInteger()
                { Id = 1, Signal = returnedSignal });


            dataRepoMock.Setup(d => d.GetData<int>(returnedSignal, fromIncluded, toExluded))
                .Returns(actualToBeReturnedByMockDatums);

            SetupGetDataOlderAndNewerThanForSignal(returnedSignal);
        }

        private void AssertEqual(List<Dto.Datum> expectedDatums, IEnumerable<Dto.Datum> actualDatums)
        {
            int i = 0;

            Assert.AreEqual(13, actualDatums.Count());
            foreach (var actualData in actualDatums)
            {
                Assert.AreEqual(expectedDatums[i].Quality, actualData.Quality);
                Assert.AreEqual(expectedDatums[i].Timestamp, actualData.Timestamp);
                Assert.AreEqual(expectedDatums[i].Value, actualData.Value);

                i++;
            }
        }

        private List<Dto.Datum> GetExpectedDatums(Granularity granularity)
        {
            switch (granularity)
            {
                case Granularity.Second:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 1), Value = (int)3 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 2), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 3), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 4), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 0, 5), Value = (int)11 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 6), Value = (int)10 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 7), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 8), Value = (int)8 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 9), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 0, 10), Value = (int)14 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 0, 11), Value = (int)21 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 0, 12), Value = (int)28 },
                    };

                case Granularity.Minute:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 1, 0), Value = (int)3 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 2, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 3, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 4, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 0, 5, 0), Value = (int)11 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 6, 0), Value = (int)10 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 7, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 8, 0), Value = (int)8 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 9, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 10,0), Value = (int)14 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 11,0), Value = (int)21 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 0, 12,0), Value = (int)28 },
                    };

                case Granularity.Hour:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 1, 0, 0), Value = (int)3 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 2, 0, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 3, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 4, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1, 5, 0, 0), Value = (int)11 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 6, 0, 0), Value = (int)10 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 7, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 8, 0, 0), Value = (int)8 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 9, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 10, 0, 0), Value = (int)14 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 11, 0, 0), Value = (int)21 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 1, 12, 0, 0), Value = (int)28 },
                    };

                case Granularity.Day:
                    return new List<Dto.Datum>()
                    {
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1, 0, 0, 0), Value = (int)1 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 2, 0, 0, 0), Value = (int)3 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 3, 0, 0, 0), Value = (int)5 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 4, 0, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 5, 0, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 6, 0, 0, 0), Value = (int)11 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 7, 0, 0, 0), Value = (int)10 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 8, 0, 0, 0), Value = (int)9 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 9, 0, 0, 0), Value = (int)8 },
                        new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 10, 0, 0, 0), Value = (int)7 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 11, 0, 0, 0), Value = (int)14 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 12, 0, 0, 0), Value = (int)21 },
                        new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 13, 0, 0, 0), Value = (int)28 },
                    };
            }
            return null;
        }

        private void SetupGetDataOlderAndNewerThanForSignal(Signal returnedSignal)
        {
            switch (returnedSignal.Granularity)
            {
                case Granularity.Second:
                    var firstTimestamp = new DateTime(2000, 1, 1, 0, 0, 1);
                    var secondTimestamp = new DateTime(2000, 1, 1, 0, 0, 5);
                    var thirdTimestamp = new DateTime(2000, 1, 1, 0, 0, 9);
                    var fourthTimestamp = new DateTime(2000, 1, 1, 0, 0, 12);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, firstTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = firstTimestamp.AddSeconds(-1), Value = (int)1 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, firstTimestamp, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestamp, Value = (int)11 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, secondTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestamp, Value = (int)11 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, secondTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestamp, Value = (int)7 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, thirdTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestamp, Value = (int)7 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, thirdTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestamp, Value = (int)28 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, fourthTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestamp, Value = (int)28 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, fourthTimestamp.AddSeconds(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.None, Signal = returnedSignal, Timestamp = fourthTimestamp.AddSeconds(1), Value = default(int) }
                        });
                    break;

                case Granularity.Minute:
                    var firstTimestampMinute = new DateTime(2000, 1, 1, 0, 1, 0);
                    var secondTimestampMinute = new DateTime(2000, 1, 1, 0, 5, 0);
                    var thirdTimestampMinute = new DateTime(2000, 1, 1, 0, 9, 0);
                    var fourthTimestampMinute = new DateTime(2000, 1, 1, 0, 12, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, firstTimestampMinute, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = firstTimestampMinute.AddMinutes(-1), Value = (int)1 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, firstTimestampMinute, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampMinute, Value = (int)11 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, secondTimestampMinute.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampMinute, Value = (int)11 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, secondTimestampMinute.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampMinute, Value = (int)7 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, thirdTimestampMinute.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampMinute, Value = (int)7 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, thirdTimestampMinute.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampMinute, Value = (int)28 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, fourthTimestampMinute.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampMinute, Value = (int)28 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, fourthTimestampMinute.AddMinutes(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.None, Signal = returnedSignal, Timestamp = fourthTimestampMinute.AddMinutes(1), Value = default(int) }
                        });
                    break;

                case Granularity.Hour:
                    var firstTimestampHour = new DateTime(2000, 1, 1, 1, 0, 0);
                    var secondTimestampHour = new DateTime(2000, 1, 1, 5, 0, 0);
                    var thirdTimestampHour = new DateTime(2000, 1, 1, 9, 0, 0);
                    var fourthTimestampHour = new DateTime(2000, 1, 1, 12, 0, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, firstTimestampHour, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = firstTimestampHour.AddHours(-1), Value = (int)1 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, firstTimestampHour, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampHour, Value = (int)11 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, secondTimestampHour.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampHour, Value = (int)11 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, secondTimestampHour.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampHour, Value = (int)7 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, thirdTimestampHour.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampHour, Value = (int)7 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, thirdTimestampHour.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampHour, Value = (int)28 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, fourthTimestampHour.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampHour, Value = (int)28 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, fourthTimestampHour.AddHours(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.None, Signal = returnedSignal, Timestamp = fourthTimestampHour.AddHours(1), Value = default(int) }
                        });
                    break;

                case Granularity.Day:
                    var firstTimestampDay = new DateTime(2000, 1, 1, 0, 0, 0);
                    var secondTimestampDay = new DateTime(2000, 1, 6, 0, 0, 0);
                    var thirdTimestampDay = new DateTime(2000, 1, 10, 0, 0, 0);
                    var fourthTimestampDay = new DateTime(2000, 1, 13, 0, 0, 0);

                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, firstTimestampDay, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = firstTimestampDay.AddDays(-1), Value = (int)1 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, firstTimestampDay, 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampDay, Value = (int)11 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, secondTimestampDay.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Good, Signal = returnedSignal, Timestamp = secondTimestampDay, Value = (int)11 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, secondTimestampDay.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampDay, Value = (int)7 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, thirdTimestampDay.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Fair, Signal = returnedSignal, Timestamp = thirdTimestampDay, Value = (int)7 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, thirdTimestampDay.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampDay, Value = (int)28 }
                        });


                    dataRepoMock
                        .Setup(d => d.GetDataOlderThan<int>(returnedSignal, fourthTimestampDay.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.Poor, Signal = returnedSignal, Timestamp = fourthTimestampDay, Value = (int)28 }
                        });

                    dataRepoMock
                        .Setup(d => d.GetDataNewerThan<int>(returnedSignal, fourthTimestampDay.AddDays(1), 1))
                        .Returns(new List<Datum<int>>()
                        {
                            new Datum<int>() { Quality = Quality.None, Signal = returnedSignal, Timestamp = fourthTimestampDay.AddDays(1), Value = default(int) }
                        });
                    break;
            }
        }
    }
}
