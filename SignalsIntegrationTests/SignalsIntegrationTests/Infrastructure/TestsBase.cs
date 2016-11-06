using System;
using System.Linq;
using System.Reflection;
using Domain;
using Dto.Conversions;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignalsIntegrationTests.Infrastructure
{
    [TestClass]
    public abstract class TestsBase
    {
        public TestContext TestContext { get; set; }

        private static IDisposable serviceGuard;
        protected static WebService.ISignalsWebService client;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var unityContainer = new UnityContainer();
            (new Bootstrapper.Bootstrapper()).Run(unityContainer, true);

            client = Intercept.ThroughProxy<WebService.ISignalsWebService>(
                unityContainer.Resolve<WebService.ISignalsWebService>(),
                new InterfaceInterceptor(),
                new[] { new WebServiceEmulationProxy(testContext) });
        }

        [TestInitialize]
        public void InitializeClient()
        {
            if (TimeoutRegistry.ContainsAll(GetCurrentCategories()))
            {
                Assert.Fail("One of previous tests from this category failed due to TimeoutException");
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            client = null;
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
            foreach (var data in datums.Batch(10000))
            {
                client.SetData(signalId, data.ToArray());
            }
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

        protected DateTime UniversalBeginTimestamp { get { return new DateTime(2018, 1, 1); } }

        protected DateTime UniversalEndTimestamp(Granularity granularity)
        {
            return UniversalBeginTimestamp.AddSteps(granularity, 5);
        }

        protected DateTime UniversalMiddleTimestamp(Granularity granularity)
        {
            return UniversalBeginTimestamp.AddSteps(granularity, 2);
        }

        protected Dto.Datum[] ClientGetData(int signalId, DateTime fromIncludedUtc, DateTime toExcludedUtc)
        {
            try
            {
                return client.GetData(signalId, fromIncludedUtc, toExcludedUtc).ToArray();
            }
            catch (TimeoutException)
            {
                Timeout();
                throw;
            }
        }

        protected void Timeout()
        {
            TimeoutRegistry.RegisterTimeout(GetCurrentCategories());
        }

        private string[] GetCurrentCategories()
        {
            Type currentTestType = null;

            foreach (var a in AppDomain
                .CurrentDomain
                .GetAssemblies())
            {
                currentTestType = a
                    .GetType(TestContext.FullyQualifiedTestClassName);

                if (currentTestType != null)
                {
                    break;
                }
            }

            var currentMethod = currentTestType
                .GetMethod(TestContext.TestName);

            return currentMethod
                .GetCustomAttributes(typeof(TestCategoryAttribute), false)
                .Cast<TestCategoryAttribute>()
                .SelectMany(tca => tca.TestCategories)
                .ToArray();
        }

        protected int signalId;
    }
}
