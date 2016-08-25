using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using DataAccess.GenericInstantiations;
using System.Collections.Generic;
using WebService.Tests.SignalsWebServiceTests.Infrastructure;

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
