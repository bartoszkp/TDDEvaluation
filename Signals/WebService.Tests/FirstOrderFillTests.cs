﻿using System;
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
            }
        }
    }
}
