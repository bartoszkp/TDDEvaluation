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

        protected void GivenSingleDatum(Datum<int> datum)
        {
            GivenData(datum);
        }

        protected void GivenSingleDatum(Dto.Datum datum)
        {
            client.SetData(signalId, new[] { datum });
        }

        protected void GivenData(params Datum<int>[] datums)
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

        protected int signalId;
    }
}
