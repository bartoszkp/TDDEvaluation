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
            GivenData();
        }

        protected void GivenSingleDatum(Datum<int> datum)
        {
            GivenData(datum);
        }

        protected void GivenData(params Datum<int>[] datums)
        {
            client.SetData(signalId, datums.ToDto<Dto.Datum[]>());
        }

        protected Dto.Signal AddNewIntegerSignal(Domain.Granularity granularity = Granularity.Second, Domain.Path path = null)
        {
            if (path == null)
            {
                path = SignalPathGenerator.Generate();
            }

            var signal = new Signal()
            {
                Path = path,
                Granularity = granularity,
                DataType = DataType.Integer,
            };

            return client.Add(signal.ToDto<Dto.Signal>());
        }

        protected Dto.Signal AddNewStringSignal()
        {
            var signal = new Signal()
            {
                Path = SignalPathGenerator.Generate(),
                Granularity = Granularity.Day,
                DataType = DataType.String
            };

            return client.Add(signal.ToDto<Dto.Signal>());
        }

        protected int signalId;
    }
}
