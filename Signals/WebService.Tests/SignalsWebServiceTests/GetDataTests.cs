﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DataAccess.GenericInstantiations;
using System.Collections.Generic;
using WebService.Tests.SignalsWebServiceTests.Infrastructure;
using Moq;
using Domain;
using System.Reflection;

namespace WebService.Tests.SignalsWebServiceTests
{
    [TestClass]
    public class SignalsWebServiceGetDataTests : SignalsWebServiceRepository
    {
        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.SignalNotExistException))]
        public void GivenNoSignals_GetDataByIdAndTime_ExpectedException()
        {
            SetupGet();

            signalsWebService.GetData(1, new DateTime(2000, 1, 1), new DateTime(2000, 3, 1));
        }
        
        [TestMethod]
        public void GivenASignalWithNoDataSource_WhenGettingData_ChecksLengthOfArray()
        {
            var signalId = 1;
            SetupGet(Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Day));
            SetupMVPGet(new NoneQualityMissingValuePolicyDouble());

            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 1, 21));
            
            Assert.AreEqual(20, result.Count());
        }

        [TestMethod]
        public void GivenASignalWithDataSource_WhenGettingData_ChecksContentAndLengthOfArray()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new NoneQualityMissingValuePolicyInteger());
            SetupGetData(new[]
            {
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2005, 1, 1), Value = 5 },
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Good,
                    Timestamp = new DateTime(2005, 3, 1), Value = 7 }
            });

            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 4, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = 5},
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2005, 2, 1), Value = default(int)},
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 3, 1), Value = 7 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignalWithCompleteDataSource_WhenGettingSortedgData_ChecksContentAndLengthOfArray()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new NoneQualityMissingValuePolicyInteger());
            SetupGetData(new[]
            {
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Good,
                    Timestamp = new DateTime(2005, 3, 1), Value = 7, },
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2005, 1, 1), Value = 11 },
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2005, 2, 1), Value = 2 }
            });

            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 4, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = 11 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 2, 1), Value = 2 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 3, 1), Value = 7 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignalWithIncompleteDataSource_WhenGettingSortedData_ChecksContentAndLengthOfArray()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new NoneQualityMissingValuePolicyInteger());
            SetupGetData(new[]
            {
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Good,
                    Timestamp = new DateTime(2005, 3, 1), Value = 7, },
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2005, 1, 1), Value = 11 }
            });

            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 4, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = 11 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2005, 2, 1), Value = default(int) },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 3, 1), Value = 7 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignalWithData_WhenGettingDataWithFromIncludedUtcEqualsToExcludedUtc_ReturnsExpectedValue()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new NoneQualityMissingValuePolicyInteger());
            SetupGetData(new[]
            {
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2005, 1, 1), Value = (int)11 }
            });

            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 1, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = 11 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignalWithSpecificMissingValuePolicy_WhenGettingData_ReturnsDataWithFilledWithSpecificValue()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new SpecificValueMissingValuePolicyInteger() { Quality = Domain.Quality.Poor, Value = 16 });
            SetupGetData(new[]
            {
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Good,
                    Timestamp = new DateTime(2005, 3, 1), Value = 7, },
                new Domain.Datum<Int32>() { Signal = signal, Quality = Domain.Quality.Fair,
                    Timestamp = new DateTime(2005, 1, 1), Value = 11 }
            });
            
            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 6, 1)).ToArray();
            var expected =new[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = 11 },
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2005, 2, 1), Value = 16 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2005, 3, 1), Value = 7 },
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2005, 4, 1), Value = 16 },
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2005, 5, 1), Value = 16 }
            };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignalWithOutData_WhenGettingDataFromSpecificPointInTime_ReturnsDataWithSpecificValue()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new SpecificValueMissingValuePolicyInteger() { Quality = Domain.Quality.Poor, Value = 16 });
            SetupGetData(new List<Domain.Datum<Int32>>());

            var result = signalsWebService.GetData(signalId, new DateTime(2005, 1, 1), new DateTime(2005, 1, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2005, 1, 1), Value = 16 }
            };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignal_WhenGettingDataWithZeroOrderMissingValuePolicy_ReturnsIt()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new ZeroOrderMissingValuePolicyDouble());
            SetupGetData(new[] {
                new Domain.Datum<double> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1.1 },
                new Domain.Datum<double> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 3.3 },
                new Domain.Datum<double> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 5.5 }
            });

            var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 6, 1)).ToArray();
            var expected = new[]{
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1.1 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = 1.1 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 3.3 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 3.3 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 5.5 }
            };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }
        
        [TestMethod]
        public void GivenASignal_WhenGettingDataWithZeroOrderMissingValuePolicy_ReturnsDefault()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new ZeroOrderMissingValuePolicyDouble());
            SetupGetData(new[] {
                new Domain.Datum<double> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = 2.0 },
                new Domain.Datum<double> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 4.0 }
            });

            var result = signalsWebService.GetData(signalId, new DateTime(2000, 1, 1), new DateTime(2000, 5, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 1, 1), Value = default(double) },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = 2.0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 2.0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 4.0 }
            };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }

        [TestMethod]
        public void GetData_ExistDataBeforeRequestedTime_ZeroOrderMissingValuePolicyShouldUseThatData()
        {

            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new ZeroOrderMissingValuePolicyDouble());
            SetupGetData(new[] {
                new Domain.Datum<double> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 6, 1), Value = 6.0 },
                new Domain.Datum<double> { Quality = Domain.Quality.Good, Timestamp = new DateTime(2000, 7, 1), Value = 7.0 }
            });

            signalsDataRepositoryMock.Setup(sdr => sdr.GetDataOlderThan<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(new[] {
                    new Domain.Datum<double> {Quality = Domain.Quality.Bad, Timestamp = new DateTime(2000,1,1), Value = 1.0 },
                    new Domain.Datum<double> {Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000,2,1), Value = 2.0 }
                });

            var result = signalsWebService.GetData(signalId, new DateTime(2000, 5, 1), new DateTime(2000, 8, 1)).ToArray();
            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 5, 1), Value = 2.0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 6, 1), Value = 6.0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 7, 1), Value = 7.0 }
            };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }

        [TestMethod]
        public void GetData_SignalWithDataTypeDouble_FirstOrderMissingValuePolicyShouldFillMissingData()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new FirstOrderMissingValuePolicyDouble());

            var datum = new[] {
               new Domain.Datum<double> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2000,1,1), Value = 1.0 },
               new Domain.Datum<double> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2000,5,1), Value = 2.0},
               new Domain.Datum<double> {Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000,8,1), Value = 5.0}
            };

            SetupGetData(datum);

            signalsDataRepositoryMock.Setup(sdr => sdr.GetDataOlderThan<double>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(new[] { (Domain.Datum<double>)null});


            signalsDataRepositoryMock
                    .Setup(q => q.GetDataNewerThan<double>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                    .Returns<Signal, DateTime, int>((sig, dateTime, count) => {
                        return new[] { datum.FirstOrDefault(x=>x.Timestamp >= dateTime) };
                    });
            
            var result = signalsWebService.GetData(signalId, new DateTime(1999, 11, 1), new DateTime(2000, 11, 1)).ToArray();

            var expected = new[] {
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(1999,11, 1), Value = 0.0},
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(1999, 12, 1), Value = 0.0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 1, 1), Value = 1.0 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 2, 1), Value = 1.25 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 3, 1), Value = 1.50},
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 4, 1), Value = 1.75 },
                new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2000, 5, 1), Value = 2.0 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 6, 1), Value = 3.0 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 7, 1), Value = 4.0 },
                new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000, 8, 1), Value = 5.0 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 9, 1), Value = 0.0 },
                new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2000, 10, 1), Value = 0.0 },
            };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }

        [TestMethod]
        public void GetData_NoDataInRequestedSectorDecimal_FirstOrderMissingValuePolicyShouldReturnOneElement()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Decimal, Domain.Granularity.Month);
            SetupGet(signal);
            SetupMVPGet(new FirstOrderMissingValuePolicyDecimal());

            var datum = new[] {
               new Domain.Datum<decimal> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2000,1,1), Value = 1m },
               new Domain.Datum<decimal> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2000,5,1), Value = 2m},
               new Domain.Datum<decimal> {Quality = Domain.Quality.Fair, Timestamp = new DateTime(2000,8,1), Value = 5m}
            };

            SetupGetData(datum);

            signalsDataRepositoryMock.Setup(sdr => sdr.GetDataOlderThan<decimal>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(new[] {datum[1]});


            signalsDataRepositoryMock
                    .Setup(q => q.GetDataNewerThan<decimal>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                    .Returns<Signal, DateTime, int>((sig, dateTime, count) => {
                        return new[] { datum.FirstOrDefault(x => x.Timestamp >= dateTime) };
                    });

            var result = signalsWebService.GetData(signalId, new DateTime(2000, 6, 1), new DateTime(2000, 7, 1)).ToArray();
            var expected = new[] { new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2000,6,1), Value = 3} };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }

        [TestMethod]
        public void GetData_NoDataInRequestedSectorInteger_FirstOrderMissingValuePolicyShouldFillMissingData()
        {

            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Year);
            SetupGet(signal);
            SetupMVPGet(new FirstOrderMissingValuePolicyInteger());

            var datum = new[] {
               new Domain.Datum<int> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2001,1,1), Value = (int)1 },
               new Domain.Datum<int> {Quality = Domain.Quality.Fair, Timestamp = new DateTime(2010,1,1), Value = (int)10}
            };

            SetupGetData(datum);

            signalsDataRepositoryMock.Setup(sdr => sdr.GetDataOlderThan<int>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(new[] { datum[0] });


            signalsDataRepositoryMock
                    .Setup(q => q.GetDataNewerThan<int>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                    .Returns<Signal, DateTime, int>((sig, dateTime, count) => {
                        return new[] { datum.FirstOrDefault(x => x.Timestamp >= dateTime) };
                    });

            var result = signalsWebService.GetData(signalId, new DateTime(2003, 1, 1), new DateTime(2006, 1, 1)).ToArray();

            var expected = new[] {
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2003, 1, 1), Value = 3 },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2004, 1, 1), Value = 4 },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2005, 1, 1), Value = 5 }
            };

            Assert.IsTrue(Utils.CompareDatum(expected, result));
        }

        [TestMethod]
        public void GetData_SignalGranualityWeek_FirstOrderMissingValuePolicyShouldFillMissingData()
        {
            var signalId = 1;
            var signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Week);
            SetupGet(signal);
            SetupMVPGet(new FirstOrderMissingValuePolicyInteger());

            var datum = new[] {
               new Domain.Datum<int> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2016,8,15), Value = (int)1 },
               new Domain.Datum<int> {Quality = Domain.Quality.Fair, Timestamp = new DateTime(2016,8,29), Value = (int)10}
            };

            SetupGetData(datum);

            signalsDataRepositoryMock.Setup(sdr => sdr.GetDataOlderThan<int>(It.IsAny<Domain.Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                .Returns(new[] { datum[0] });


            signalsDataRepositoryMock
                    .Setup(q => q.GetDataNewerThan<int>(It.IsAny<Signal>(), It.IsAny<DateTime>(), It.IsAny<int>()))
                    .Returns<Signal, DateTime, int>((sig, dateTime, count) =>
                    {
                        return new[] { datum.FirstOrDefault(x => x.Timestamp >= dateTime) };
                    });

            var result = signalsWebService.GetData(signalId, new DateTime(2016, 8, 15), new DateTime(2016,9,5)).ToArray();

            var expected = new[] {
                  new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2016, 8, 15), Value = 1 },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 8, 22), Value = 5 },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 8, 29), Value = 10 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));

        }

        [TestMethod]
        public void GivenASignal_GetDataWithNoneQualityMvp_ReturnsNoneData()
        {
            int signalId = 5;
            Signal signal = Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Year);
            SetupGet(signal);
            SetupMVPGet(new NoneQualityMissingValuePolicyInteger());

            var datum = new[] {
               new Domain.Datum<int> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2016,1,1), Value = (int)1 }
            };

            var result = signalsWebService.GetData(signalId, new DateTime(2017, 1, 1), new DateTime(2019, 1, 1)).ToArray();

            var expected = new[] {
                  new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2017, 1, 1), Value = 0 },
                  new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2018, 1, 1), Value = 0 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));

        }

        [TestMethod]
        public void GivenASignal_GetDataWithSpecificValueMvp_ReturnsIt()
        {
            int signalId = 5;
            Signal signal = Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Year);
            SetupGet(signal);
            SetupMVPGet(new SpecificValueMissingValuePolicyDouble() { Quality = Quality.Good, Value = 5.0 } );

            var datum = new[] {
               new Domain.Datum<double> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2016,1,1), Value = (double)1 }
            };

            var result = signalsWebService.GetData(signalId, new DateTime(2017, 1, 1), new DateTime(2019, 1, 1)).ToArray();

            var expected = new[] {
                  new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2017, 1, 1), Value = 5.0 },
                  new Dto.Datum() { Quality = Dto.Quality.Good, Timestamp = new DateTime(2018, 1, 1), Value = 5.0 } };

            Assert.IsTrue(Utils.CompareDatum(expected, result));

        }

        [TestMethod]
        [ExpectedException(typeof(TargetInvocationException))]
        public void GivenASignalDataTypeString_GetDataWithFirstOrderMVP_ExpectedException()
        {
            int signalId = 3;
            Signal signal = Utils.SignalWith(signalId, Domain.DataType.String, Domain.Granularity.Year);
            SetupGet(signal);
            SetupMVPGet(new FirstOrderMissingValuePolicyString() { });

            var datum = new[] {
               new Domain.Datum<string> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2016,1,1), Value = "a" }
            };

            signalsWebService.GetData(signalId, new DateTime(2014, 1, 1), new DateTime(2018, 1, 1));
        }

        [TestMethod]
        public void GivenTwoSignals_GetDataWihShadowMVP_ExpectedResult()
        {
            int signalId = 3;
            int shadowSignalId = 5;
            Signal signal = Utils.SignalWith(signalId, Domain.DataType.String, Domain.Granularity.Day);
            Signal shadowSignal = Utils.SignalWith(shadowSignalId, Domain.DataType.String, Domain.Granularity.Day);

            SetupGet(signal);
            SetupGet(shadowSignal);

            var signalDatums = new[] {
               new Domain.Datum<string> {Quality = Domain.Quality.Fair, Timestamp = new DateTime(2016,1,2), Value = "signal1" },
               new Domain.Datum<string> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2016,1,3), Value = "signal2"  }
            };

            var shadowSignalsDatums = new[]{
                new Domain.Datum<string> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2016,1,2), Value = "shadowSignal1" },
                new Domain.Datum<string> {Quality = Domain.Quality.Good, Timestamp = new DateTime(2016,1,4), Value = "shadowSignal2" }
            };

            var expectedResult = new[]
            {
                new Dto.Datum {Quality = Dto.Quality.None, Timestamp = new DateTime(2016,1,1), Value = null }, //default value for string
                new Dto.Datum {Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016,1,2), Value = "signal1" },
                new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016,1,3), Value = "signal2"  },
                new Dto.Datum {Quality = Dto.Quality.Good, Timestamp = new DateTime(2016,1,4), Value = "shadowSignal2" }
            };

            SetupGetData<string>(signalDatums, signal);
            SetupGetData<string>(shadowSignalsDatums, shadowSignal);
            
            SetupMVPGet(new ShadowMissingValuePolicyString() { ShadowSignal = shadowSignal });

            var result = signalsWebService.GetData(signalId, new DateTime(2016, 1, 1), new DateTime(2016, 1, 5));

            Assert.IsTrue(Utils.CompareDatum(result.ToArray(), expectedResult));
        }

        public void GivenASignal_WhenGettingSignalDataWithInvalidMilliseconds_ExpectHandledExceptions()
        {
            Assert.IsTrue(IsGetDataTimestampValid(Utils.validTimestamp, Utils.validTimestamp.AddMilliseconds(1)));
        }

        [TestMethod]
        public void GivenASignal_WhenGettingSignalDataWithInvalidSeconds_ExpectHandledExceptions()
        {
            Assert.IsTrue(IsGetDataTimestampValid(Utils.validTimestamp, Utils.validTimestamp.AddSeconds(1)));
        }

        [TestMethod]
        public void GivenASignal_WhenGettingSignalDataWithInvalidMinutes_ExpectHandledExceptions()
        {
            Assert.IsTrue(IsGetDataTimestampValid(Utils.validTimestamp, Utils.validTimestamp.AddMinutes(1)));
        }

        [TestMethod]
        public void GivenASignal_WhenGettingSignalDataWithInvalidHours_ExpectHandledExceptions()
        {
            Assert.IsTrue(IsGetDataTimestampValid(Utils.validTimestamp, Utils.validTimestamp.AddHours(1)));
        }

        [TestMethod]
        public void GivenASignal_WhenGettingSignalDataWithInvalidWeekDay_ExpectHandledExceptions()
        {
            var signalId = 1;
            var dtCorrect = new DateTime(2016, 08, 22);
            SetupGet(Utils.SignalWith(signalId, Domain.DataType.Double, Domain.Granularity.Week, Domain.Path.FromString("a/b")));

            Assert.IsTrue(IsGetDataTimestampThrowingException(signalId, dtCorrect.AddDays(-1), dtCorrect));
            Assert.IsTrue(IsGetDataTimestampThrowingException(signalId, dtCorrect, dtCorrect.AddDays(1)));
        }

        [TestMethod]
        public void GivenASignal_WhenGettingSignalDataWithInvalidFirstDayOfMonth_ExpectHandledExceptions()
        {
            Assert.IsTrue(IsGetDataTimestampValid(Utils.validTimestamp, Utils.validTimestamp.AddDays(1)));
        }

        [TestMethod]
        public void GivenASignal_WhenGettingSignalDataWithInvalidMonths_ExpectHandledExceptions()
        {
            Assert.IsTrue(IsGetDataTimestampValid(Utils.validTimestamp, Utils.validTimestamp.AddMonths(1)));
        }

        [TestMethod]
        public void GivenASignal_GetDataWithShadowMVPWithDatum_ReturnsDatum()
        {
            int shadowSignalId = 3;
            int signalId = 5;
            Signal signal = Utils.SignalWith(signalId, Domain.DataType.Boolean, Domain.Granularity.Month);
            Signal shadowSignal = Utils.SignalWith(shadowSignalId, Domain.DataType.Boolean, Domain.Granularity.Month);
            SetupGet(signal);
            SetupGet(shadowSignal);
            SetupMVPGet(new ShadowMissingValuePolicyBoolean() { ShadowSignal = shadowSignal });

            var shadowDatum = new[] {
               new Domain.Datum<bool> {Quality = Domain.Quality.None, Timestamp = new DateTime(2016,1,1), Value = false },
               new Domain.Datum<bool> {Quality = Domain.Quality.Fair, Timestamp = new DateTime(2016,2,1), Value = true },
               new Domain.Datum<bool> {Quality = Domain.Quality.None, Timestamp = new DateTime(2016,3,1), Value = false },
               new Domain.Datum<bool> {Quality = Domain.Quality.Poor, Timestamp = new DateTime(2016,4,1), Value = false },
               new Domain.Datum<bool> {Quality = Domain.Quality.None, Timestamp = new DateTime(2016,5,1), Value = false },
            };
            var datum = new Domain.Datum<bool>[] {
                    
            };

            SetupGetData<bool>(shadowDatum, shadowSignal);
            SetupGetData<bool>(datum, signal);
            var result = signalsWebService.GetData(signalId, new DateTime(2016, 1, 1), new DateTime(2016, 6, 1)).ToArray();

            var expected = new[] {
                  new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2016, 1, 1), Value = false },
                  new Dto.Datum() { Quality = Dto.Quality.Fair, Timestamp = new DateTime(2016, 2, 1), Value = true },
                  new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2016, 3, 1), Value = false},
                  new Dto.Datum() { Quality = Dto.Quality.Poor, Timestamp = new DateTime(2016, 4, 1), Value = false },
                  new Dto.Datum() { Quality = Dto.Quality.None, Timestamp = new DateTime(2016, 5, 1), Value = false },
                           };


            Assert.IsTrue(Utils.CompareDatum(expected, result));

        }
        private bool IsGetDataTimestampValid(DateTime dtCorrect, DateTime dtWrong)
        {
            var signalId = 1;
            SetupGet(Utils.SignalWith(signalId, Domain.DataType.Integer, Domain.Granularity.Year));

            return IsGetDataTimestampThrowingException(signalId, dtWrong, dtCorrect.AddYears(1)) &&
                    IsGetDataTimestampThrowingException(signalId, dtCorrect, dtWrong);
        }

        private bool IsGetDataTimestampThrowingException(int id, DateTime dtStart, DateTime dtEnd)
        {
            try
            {
                signalsWebService.GetData(id, dtStart, dtEnd);
                return false;
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                return (ex.InnerException.GetType() == typeof(Domain.Exceptions.DatetimeIsInvalidException));
            }
        } 
    }
}