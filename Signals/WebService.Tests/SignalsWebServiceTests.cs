using System.Linq;
using Domain;
using Domain.Repositories;
using Domain.Services.Implementation;
using Dto.Conversions;
using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace WebService.Tests
{
    namespace WebService.Tests
    {
        [TestClass]
        public class SignalsWebServiceTests
        {
            private ISignalsWebService signalsWebService;

            [TestMethod]
            [ExpectedException(typeof(Domain.Exceptions.IdNotNullException))]
            public void GivenNoSignals_WhenAddingASignalWithId_ThrowIdNotNullException()
            {
                GivenNoSignals();

                var result = signalsWebService.Add(SignalWith(id: 1));
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_ReturnsIt()
            {
                GivenNoSignals();

                var result = signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Double,
                    granularity: Dto.Granularity.Month,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }
                    ));

                Assert.AreEqual(Dto.DataType.Double, result.DataType);
                Assert.AreEqual(Dto.Granularity.Month, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_CallsRepositoryAdd()
            {
                GivenNoSignals();

                signalsWebService.Add(SignalWith(
                    dataType: Dto.DataType.Double,
                    granularity: Dto.Granularity.Month,
                    path: new Dto.Path() { Components = new[] { "root", "signal" } }
                    ));

                signalsRepositoryMock.Verify(sr => sr.Add(It.Is<Domain.Signal>(passedSignal
                    => passedSignal.DataType == DataType.Double
                        && passedSignal.Granularity == Granularity.Month
                        && passedSignal.Path.ToString() == "root/signal")));
            }

            [TestMethod]
            public void GivenNoSignals_WhenGettingById_ReturnsNull()
            {
                GivenNoSignals();

                var result = signalsWebService.GetById(0);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByItsId_ReturnsIt()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.GetById(signalId);

                Assert.AreEqual(signalId, result.Id);
                Assert.AreEqual(Dto.DataType.Integer, result.DataType);
                Assert.AreEqual(Dto.Granularity.Second, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenASignal_WhenGettingByItsPath_ReturnsIt()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));
                var path = new Dto.Path() { Components = new string[] { "root", "signal" } };

                var result = signalsWebService.Get(path);

                Assert.AreEqual(signalId, result.Id);
                Assert.AreEqual(Dto.DataType.Integer, result.DataType);
                Assert.AreEqual(Dto.Granularity.Second, result.Granularity);
                CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
            }

            [TestMethod]
            public void GivenASignal_WhenSettingData_CallsRepositorySetData()
            {
                var signalId = 1;
                var data = new Dto.Datum[] {
                    new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } };
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.Double,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                signalsWebService.SetData(signalId, data);

                signalsDataRepositoryMock.Verify(sdr => sdr.SetData(It.Is<IEnumerable<Datum<double>>>(
                    d => d.Count() == 3
                    && DatumDomainEquals(d.First(), Domain.Quality.Fair, new DateTime(2000, 1, 1), 1.0, signalId)
                    && DatumDomainEquals(d.Skip(1).First(), Domain.Quality.Good, new DateTime(2000, 2, 1), 1.5, signalId)
                    && DatumDomainEquals(d.Skip(2).First(), Domain.Quality.Poor, new DateTime(2000, 3, 1), 2.0, signalId)
                    )));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignal_WhenSettingData_ThrowsArgumentException()
            {
                GivenNoSignals();

                var data = new Dto.Datum[] {
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } };

                signalsWebService.SetData(1, data);
            }


            [TestMethod]
            public void GivenData_WhenGettingData_ReturnsIt()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Day, Domain.Path.FromString("x/y")));
                GivenData(signalId,  new Domain.Datum<double>[] {
                    new Domain.Datum<double>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                Assert.AreEqual(2, result.Count());
                Assert.AreEqual(Dto.Quality.Fair, result.First().Quality);
                Assert.AreEqual(Dto.Quality.Good, result.Skip(1).First().Quality);
                Assert.AreEqual(new DateTime(2000, 1, 1), result.First().Timestamp);
                Assert.AreEqual(new DateTime(2000, 2, 1), result.Skip(1).First().Timestamp);
                Assert.AreEqual(1.0, result.First().Value);
                Assert.AreEqual(1.5, result.Skip(1).First().Value);
            }


            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignal_WhenGettingData_ThrowsArgumentException()
            {
                GivenNoSignals();
                GivenData(1, new Domain.Datum<double>[] {
                    new Domain.Datum<double>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 } });

                var result = signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
            }


            [TestMethod]
            public void GivenNoData_WhenGettingData_ReturnsEmptyCollection()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Day, Domain.Path.FromString("x/y")));

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                Assert.AreEqual(0, result.Count());
            }


            [TestMethod]
            public void GivenAMissingValuePolicy_WhenGettingMissingValuePolicy_ReturnsIt()
            {
                var signalId = 1;
                var signal = SignalWith(
                    id: signalId,
                    dataType: DataType.Integer,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal"));
                GivenASignal(signal);
                GivenMissingValuePolicy(new DataAccess.GenericInstantiations.SpecificValueMissingValuePolicyInteger()
                {
                    Signal = signal,
                    Quality = Domain.Quality.Poor,
                    Value = 4
                });

                var result = signalsWebService.GetMissingValuePolicy(signalId) as Dto.MissingValuePolicy.SpecificValueMissingValuePolicy;


                Assert.AreEqual(signalId, result.Signal.Id);
                Assert.AreEqual(Dto.DataType.Integer, result.DataType);
                Assert.AreEqual(4, result.Value);
                Assert.AreEqual(Dto.Quality.Poor, result.Quality);
            }


            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignal_WhenGettingMissingValuePolicy_ArgumentExceptionIsThrown()
            {
                GivenNoSignals();

                var result = signalsWebService.GetMissingValuePolicy(1);
            }


            [TestMethod]
            public void GivenNoMissingValuePolicy_WhenGettingMissingValuePolicy_ReturnsNull()
            {
                var signalId = 1;
                GivenASignal(SignalWith(
                    id: signalId,
                    dataType: DataType.String,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal")));

                var result = signalsWebService.GetMissingValuePolicy(signalId);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenASignal_WhenSettingMissingValuePolicy_CallsRepositorySet()
            {
                var signalId = 1;
                var signal = SignalWith(
                    id: signalId,
                    dataType: DataType.String,
                    granularity: Granularity.Second,
                    path: Domain.Path.FromString("root/signal"));
                var signalDto = new Dto.Signal()
                {
                    Id = signalId,
                    DataType = Dto.DataType.String,
                    Granularity = Dto.Granularity.Second,
                    Path = new Dto.Path { Components = new string[] { "root", "signal" } }
                };
                GivenASignal(signal);
                var missingValuePolicy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
                {
                    Quality = Dto.Quality.Fair,
                    Value = "Example",
                    DataType = Dto.DataType.String,
                    Signal = signalDto
                };

                signalsWebService.SetMissingValuePolicy(signalId, missingValuePolicy);

                missingValuePolicyRepositoryMock
                    .Verify(mvpr => mvpr.Set(It.Is<Domain.Signal>(s => s.Id == signalId),
                    It.Is<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<string>>(mvp =>
                    mvp.Value == "Example"
                    && mvp.Quality == Quality.Fair
                    && mvp.Signal.Id == signalId)));
            }

            [TestMethod]
            [ExpectedException(typeof(ArgumentException))]
            public void GivenNoSignal_WhenSettingMissingValuePolicy_ArgumentExceptionIsThrown()
            {
                GivenNoSignals();
                var missingValuePolicy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy()
                {
                    Quality = Dto.Quality.Fair,
                    Value = "Example",
                    DataType = Dto.DataType.String
                };

                signalsWebService.SetMissingValuePolicy(1, missingValuePolicy);
            }

            [TestMethod]
            public void GivenNoSignal_WhenGettingById_NullIsReturned()
            {
                GivenNoSignals();
                int nonExistingId = 5;

                var result = signalsWebService.GetById(nonExistingId);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GivenNoSignal_WhenGettingByPath_NullIsReturned()
            {
                GivenNoSignals();
                var nonExistingPathDto = new Dto.Path()
                {
                    Components = new[] { "non", "existing", "path" }
                };

                var result = signalsWebService.Get(nonExistingPathDto);

                Assert.IsNull(result);
            }

            [TestMethod]
            public void GetData_EqualTimestamps_ReturnSingleDatum()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Month, Domain.Path.FromString("x/y")));
                signalsDataRepositoryMock.Setup(x => x.GetData<double>(It.IsAny<Domain.Signal>(), new DateTime(2000, 2, 1), new DateTime(2000, 2, 1))).Returns(Enumerable.Repeat(new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },1));

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 2, 1), new DateTime(2000, 2, 1));

                var expectedDataDto = Enumerable.Repeat(new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 }, 1);
                AssertDataDtoEquals(result,expectedDataDto);
            }

            [TestMethod]
            public void GivenData_WhenGettingData_DataIsReturnedOrderedByTimestampAscending()
            {
                int signalId = 1;
                GivenASignal(SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Day, Domain.Path.FromString("x/y")));
                GivenData(signalId, new Domain.Datum<double>[] {
                    new Domain.Datum<double>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 }
                     });

                var expectedDataDto = new Dto.Datum[]
                {
                    new Dto.Datum() {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 },
                    new Dto.Datum() {Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                };

                var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));

                AssertDataDtoEquals(expectedDataDto, result);
            }

            [TestMethod]
            public void GivenData_WhenGettingData_GetMissingValuePolicyIsCalled()
            {
                int signalId = 1;
                var signal = SignalWith(signalId, DataType.Double, Granularity.Month, Path.FromString("x/y"));

                GivenASignal(SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Day, Domain.Path.FromString("x/y")));
                GivenData(signalId, new Domain.Datum<double>[] {
                    new Domain.Datum<double>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = (double)1.5 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000, 1, 1), Value = (double)1 }
                });

                signalsWebService.GetData(signalId, DateTime.MinValue, DateTime.MaxValue);
                missingValuePolicyRepositoryMock.Verify(mvp => mvp.Get(It.Is<Domain.Signal>(s => s.Id == signalId)), Times.Once);
            }

            [TestMethod]
            public void GivenDataMissingSingleElement_WhenGettingData_SetDataIsCalledWithDataFromNoneQualityMVP()
            {
                int signalId = 1;
                var signal = SignalWith(signalId, DataType.Double, Granularity.Month, Path.FromString("x/y"));
                DateTime dateFrom = new DateTime(2000, 1, 1);
                DateTime dateTo = new DateTime(2000, 4, 1);

                var dataToReturnInFirstIter = new Domain.Datum<double>[] {
                    new Domain.Datum<double>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                };
                var dataToReturnInSecondIter = new Domain.Datum<double>[] {
                    new Domain.Datum<double>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 3, 1), Value = (double)2 },
                    new Domain.Datum<double>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = (double)1.5 },
                    new Domain.Datum<double>() { Quality = Quality.None, Timestamp = new DateTime(2000, 2, 1), Value = default(double) }
                };

                GivenASignal(signal);
                signalsDataRepositoryMock.SetupSequence(sdr => sdr.GetData<double>(It.Is<Domain.Signal>(s => s.Id == signalId), dateFrom, dateTo)).
                    Returns(dataToReturnInFirstIter).
                    Returns(dataToReturnInSecondIter);
                GivenMissingValuePolicy(new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<double>()
                {
                    Signal = signal,
                    Id = signalId
                });

                signalsWebService.GetData(signalId, dateFrom, dateTo);

                VerifySetDataCallToFillSingleMissingData<double>(signalId, new DateTime(2000, 2, 1));
            }

            [TestMethod]
            public void GivenDataMissingMultipleElements_WhenGettingData_SetDataIsCalledWithDataFromNoneQualityMVP()
            {
                int signalId = 7;
                var signal = SignalWith(signalId, DataType.Decimal, Granularity.Year, Path.FromString("x/y"));
                DateTime dateFrom = new DateTime(2000, 1, 1);
                DateTime dateTo = new DateTime(2005, 1, 1);

                var dataToReturnInFirstIter = new Domain.Datum<decimal>[] {
                    new Domain.Datum<decimal>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2001, 1, 1), Value = (decimal)2 },
                    new Domain.Datum<decimal>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2003, 1, 1), Value = (decimal)1 },
                };
                var dataToReturnInSecondIter = new Domain.Datum<decimal>[] {
                    new Domain.Datum<decimal>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2001, 1, 1), Value = (decimal)2 },
                    new Domain.Datum<decimal>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2003, 1, 1), Value = (decimal)1 },
                    new Domain.Datum<decimal>() { Quality = Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = default(decimal) },
                    new Domain.Datum<decimal>() { Quality = Quality.None, Timestamp = new DateTime(2004, 1, 1), Value = default(decimal) },
                    new Domain.Datum<decimal>() { Quality = Quality.None, Timestamp = new DateTime(2002, 1, 1), Value = default(decimal) },
                };

                GivenASignal(signal);
                signalsDataRepositoryMock.SetupSequence(sdr => sdr.GetData<decimal>(It.Is<Domain.Signal>(s => s.Id == signalId), dateFrom, dateTo)).
                    Returns(dataToReturnInFirstIter).
                    Returns(dataToReturnInSecondIter);
                GivenMissingValuePolicy(new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<decimal>()
                {
                    Signal = signal,
                    Id = signalId
                });

                signalsWebService.GetData(signalId, dateFrom, dateTo);

                VerifySetDataCallToFillSingleMissingData<decimal>(signalId, new DateTime(2000, 1, 1));
                VerifySetDataCallToFillSingleMissingData<decimal>(signalId, new DateTime(2004, 1, 1));
                VerifySetDataCallToFillSingleMissingData<decimal>(signalId, new DateTime(2002, 1, 1));
            }

            [TestMethod]
            public void GivenDataMissingMultipleElements_WhenGettingData_DataWithFilledMissingValuesIsReturned()
            {
                int signalId = 5;
                var signal = SignalWith(signalId, DataType.Integer, Granularity.Day, Path.FromString("x/y"));
                DateTime dateFrom = new DateTime(2000, 1, 1);
                DateTime dateTo = new DateTime(2000, 1, 6);

                var dataToReturnInFirstIter = new Domain.Datum<int>[] {
                    new Domain.Datum<int>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 1, 2), Value = (int)2 },
                    new Domain.Datum<int>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 4), Value = (int)1 },
                };
                var dataToReturnInSecondIter = new Domain.Datum<int>[] {
                    new Domain.Datum<int>() { Quality = Domain.Quality.Poor, Timestamp = new DateTime(2000, 1, 2), Value = (int)2 },
                    new Domain.Datum<int>() { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 4), Value = (int)1 },
                    new Domain.Datum<int>() { Quality = Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = default(int) },
                    new Domain.Datum<int>() { Quality = Quality.None, Timestamp = new DateTime(2000, 1, 5), Value = default(int) },
                    new Domain.Datum<int>() { Quality = Quality.None, Timestamp = new DateTime(2000, 1, 3), Value = default(int) },
                };

                GivenASignal(signal);
                signalsDataRepositoryMock.SetupSequence(sdr => sdr.GetData<int>(It.Is<Domain.Signal>(s => s.Id == signalId), dateFrom, dateTo)).
                    Returns(dataToReturnInFirstIter).
                    Returns(dataToReturnInSecondIter);
                GivenMissingValuePolicy(new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>()
                {
                    Signal = signal,
                    Id = signalId
                });

                var expectedResult = new Dto.Datum[]
                {
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = default(int) },
                    new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2000, 1, 2), Value = (int)2 },
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 3), Value = default(int) },
                    new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 4), Value = (int)1 },
                    new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 5), Value = default(int) }
                };

                var result = signalsWebService.GetData(signalId, dateFrom, dateTo);

                AssertDataDtoEquals(expectedResult, result);
            }

            [TestMethod]
            public void GivenNoSignals_WhenAddingASignal_SetMissingValuePolicyIsCalled()
            {
                GivenNoSignals();

                signalsWebService.Add(SignalWith(null, Dto.DataType.Integer, Dto.Granularity.Minute, new Dto.Path()
                {
                    Components = new[] { "root", "signal1" }
                }));

                missingValuePolicyRepositoryMock.Verify(mvp => mvp.Set(It.Is<Domain.Signal>(s => s.DataType == DataType.Integer &&
                    s.Granularity == Granularity.Minute &&
                    s.Path.ToString().Equals("root/signal1")), 
                    It.IsAny<Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>>()), 
                    Times.Once);
            }

            private void VerifySetDataCallToFillSingleMissingData<T>(int signalId, DateTime timeStamp, Quality quality = Quality.None, T value = default(T))
            {
                signalsDataRepositoryMock.Verify(sdrm => sdrm.SetData<T>(It.Is<IEnumerable<Datum<T>>>(data => data.Count().Equals(1) &&
                    data.ElementAt(0).Signal.Id == signalId &&
                    data.ElementAt(0).Quality == quality &&
                    data.ElementAt(0).Timestamp == timeStamp &&
                    data.ElementAt(0).Value.Equals(value))), Times.Once);
            }

            private void GivenMissingValuePolicy(Domain.MissingValuePolicy.MissingValuePolicyBase missingValuePolicy)
            {
                missingValuePolicyRepositoryMock
                    .Setup(mvpr => mvpr.Get(It.Is<Domain.Signal>(s => s.Id == missingValuePolicy.Signal.Id)))
                    .Returns<Domain.Signal>(s => missingValuePolicy);
            }

            private void AssertDataDtoEquals(IEnumerable<Dto.Datum> data1, IEnumerable<Dto.Datum> data2)
            {
                bool countsAreEqual = data1.Count() == data2.Count();

                Assert.IsTrue(countsAreEqual);

                if (countsAreEqual)
                {
                    for (int i = 0; i < data1.Count(); i++)
                    {
                        Assert.AreEqual(data1.ElementAt(i).Quality, data2.ElementAt(i).Quality);
                        Assert.AreEqual(data1.ElementAt(i).Timestamp, data2.ElementAt(i).Timestamp);
                        Assert.AreEqual(data1.ElementAt(i).Value, data2.ElementAt(i).Value);
                    }
                }
            }

            private bool DatumDomainEquals<T>(Domain.Datum<T> datum, Domain.Quality quality, DateTime timeStamp, T value, int signalId)
            {
                return datum.Quality == quality && datum.Timestamp == timeStamp 
                    && datum.Value.Equals(value) && datum.Signal.Id == signalId;
            }

            private Dto.Signal SignalWith(
                int? id = null,
                Dto.DataType dataType = Dto.DataType.Boolean,
                Dto.Granularity granularity = Dto.Granularity.Day,
                Dto.Path path = null)
            {
                return new Dto.Signal()
                {
                    Id = id,
                    DataType = dataType,
                    Granularity = granularity,
                    Path = path
                };
            }

            private void GivenData<T>(int signalId, IEnumerable<Domain.Datum<T>> data)
            {
                signalsDataRepositoryMock
                    .Setup(sdr => sdr.GetData<T>(It.Is<Domain.Signal>(s => s.Id == signalId), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                    .Returns<Domain.Signal, DateTime, DateTime>((s, from, to) => data.Where(d => d.Timestamp >= from && d.Timestamp < to));


            }

            private Domain.Signal SignalWith(
                int id,
                Domain.DataType dataType,
                Domain.Granularity granularity,
                Domain.Path path)
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
                signalsDataRepositoryMock = new Mock<ISignalsDataRepository>();
                missingValuePolicyRepositoryMock = new Mock<IMissingValuePolicyRepository>();
                signalsRepositoryMock
                    .Setup(sr => sr.Add(It.IsAny<Domain.Signal>()))
                    .Returns<Domain.Signal>(s => s);
                var signalsDomainService = new SignalsDomainService(signalsRepositoryMock.Object, 
                    signalsDataRepositoryMock.Object,
                    missingValuePolicyRepositoryMock.Object);
                signalsWebService = new SignalsWebService(signalsDomainService);
            }

            private void GivenASignal(Signal signal)
            {
                GivenNoSignals();

                signalsRepositoryMock
                    .Setup(sr => sr.Get(signal.Id.Value))
                    .Returns(signal);

                signalsRepositoryMock
                .Setup(sr => sr.Get(It.Is<Domain.Path>(p => p.ToString() == signal.Path.ToString())))
                .Returns(signal);
            }

            private Mock<ISignalsRepository> signalsRepositoryMock;
            private Mock<ISignalsDataRepository> signalsDataRepositoryMock;
            private Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock;
        }
    }
}