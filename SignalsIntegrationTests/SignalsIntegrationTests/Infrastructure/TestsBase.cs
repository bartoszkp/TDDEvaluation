using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignalsIntegrationTests.Infrastructure
{
    [TestClass]
    public abstract class TestsBase
    {
        private static IDisposable serviceGuard;
        protected WS.SignalsWebServiceClient client;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            (new Bootstrapper.Bootstrapper()).Run(new Microsoft.Practices.Unity.UnityContainer());

            serviceGuard = ServiceManagerGuard.Attach();
        }

        [TestInitialize]
        public void InitializeClient()
        {
            client = new WS.SignalsWebServiceClient();
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            serviceGuard.Dispose();
        }

        protected void GivenASignal(Path path, DataType dataType = DataType.Integer, Granularity granularity = Granularity.Day)
        {
            signalId = AddNewSignal(dataType, granularity, path).Id.Value;
        }

        protected void GivenASignalWith(DataType dataType, Granularity granularity)
        {
            signalId = AddNewSignal(dataType, granularity).Id.Value;
        }

        protected void GivenASignalWith(Granularity granularity)
        {
            signalId = AddNewIntegerSignal(granularity).Id.Value;
        }

        protected void GivenNoSignals()
        {
            signalId = 0;
        }

        protected void GivenNoData()
        {
        }

        protected void GivenSingleDatum<T>(Datum<T> datum)
        {
            GivenData(datum);
        }

        protected void GivenSingleDatum(Dto.Datum datum)
        {
            client.SetData(signalId, new[] { datum });
        }

        protected void GivenData<T>(params Datum<T>[] datums)
        {
            GivenData(datums.ToDto<Dto.Datum[]>());
        }

        protected void GivenData(params Dto.Datum[] datums)
        {
            client.SetData(signalId, datums);
        }

        protected Dto.Signal AddNewIntegerSignal(Domain.Granularity granularity = Granularity.Second, Domain.Path path = null)
        {
            return AddNewSignal(DataType.Integer, granularity, path);
        }

        protected Dto.Signal AddNewStringSignal()
        {
            return AddNewSignal(DataType.String, Granularity.Day);
        }

        protected Dto.Signal AddNewSignal(Domain.DataType dataType, Domain.Granularity granularity, Domain.Path path = null)
        {
            if (path == null)
            {
                path = SignalPathGenerator.Generate();
            }

            var signal = new Signal()
            {
                Path = path,
                Granularity = granularity,
                DataType = dataType,
            };

            return client.Add(signal.ToDto<Dto.Signal>());
        }

        protected void ForAllSignalTypesAndQualites(Action<DataType, Granularity, Quality, DateTime, string> test)
        {
            foreach (var dataType in Enum.GetValues(typeof(DataType)).Cast<DataType>())
            {
                foreach (var granularity in Enum.GetValues(typeof(Granularity)).Cast<Granularity>())
                {
                    foreach (var quality in Enum.GetValues(typeof(Quality)).Cast<Quality>())
                    {
                        var message = dataType.ToString() + ", " + granularity.ToString() + ", " + quality.ToString();
                        test(dataType, granularity, quality, timestamps[granularity], message);
                    }
                }
            }
        }

        protected void ForAllSignalTypes(Action<DataType, Granularity, DateTime, string> test)
        {
            foreach (var dataType in Enum.GetValues(typeof(DataType)).Cast<DataType>())
            {
                foreach (var granularity in Enum.GetValues(typeof(Granularity)).Cast<Granularity>())
                {
                    var message = dataType.ToString() + ", " + granularity.ToString();
                    test(dataType, granularity, timestamps[granularity], message);
                }
            }
        }

        protected DateTime GetNextTimestamp(DateTime dateTime, Granularity granularity)
        {
            var te = new TimeEnumerator(dateTime, 1, granularity);
            return te.ToExcludedUtcUtc;
        }

        protected DateTime GetSecondNextTimestamp(DateTime dateTime, Granularity granularity)
        {
            var te = new TimeEnumerator(dateTime, 2, granularity);
            return te.ToExcludedUtcUtc;
        }

        protected int signalId;
        protected Dictionary<DataType, object> values = new Dictionary<DataType, object>()
            { { DataType.Boolean, true },
              { DataType.Decimal, 42.0m },
              { DataType.Double, 42.42 },
              { DataType.Integer, 42 },
              { DataType.String, "string" } };
        protected Dictionary<Granularity, DateTime> timestamps = new Dictionary<Granularity, DateTime>()
            { { Granularity.Day, new DateTime(2000, 2, 12) },
              { Granularity.Hour, new DateTime(2000, 2, 12, 13, 0, 0) },
              { Granularity.Minute, new DateTime(2000, 2, 12, 13, 14, 0) },
              { Granularity.Month, new DateTime(2000, 2, 1) },
              { Granularity.Second, new DateTime(2000, 2, 12, 13, 14, 15) },
              { Granularity.Week, new DateTime(2000, 2, 7) },
              { Granularity.Year, new DateTime(2000, 1, 1) } };
    }
}
