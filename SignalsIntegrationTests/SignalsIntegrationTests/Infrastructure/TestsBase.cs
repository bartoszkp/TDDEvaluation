using System;
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
            ServiceManagerGuard.EnsureRunning();
            client = new WS.SignalsWebServiceClient("NetTcpBinding_ISignalsWebService");
            client.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, 45);
            client.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 0, 45);
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

        protected void GivenASignal()
        {
            signalId = AddNewIntegerSignal().Id.Value;
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

        protected void GivenMissingValuePolicy(Dto.MissingValuePolicy.MissingValuePolicy missingValuePolicy)
        {
            client.SetMissingValuePolicy(signalId, missingValuePolicy);
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

        protected Quality OtherThan(Quality quality)
        {
            var values = Enum.GetValues(typeof(Quality));
            return (Quality)values.GetValue(((int)quality + 1) % values.Length);
        }

        protected int signalId;

        protected DateTime UniversalBeginTimestamp { get { return new DateTime(2018, 1, 1); } }

        protected DateTime UniversalEndTimestamp(Granularity granularity)
        {
            return UniversalBeginTimestamp.AddSteps(granularity, 5);
        }

        protected DateTime UniversalMiddleTimestamp(Granularity granularity)
        {
            return UniversalBeginTimestamp.AddSteps(granularity, 2);
        }
    }
}
