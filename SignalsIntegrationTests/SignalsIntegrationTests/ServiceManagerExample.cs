using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class ServiceManagerExample
    {
        private WS.SignalsClient signalsClient;
        private static ServiceManager serviceManager;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            serviceManager = new ServiceManager();
            serviceManager.StartService();
        }

        [TestInitialize]
        public void TestInitialize()
        { 
            signalsClient = new WS.SignalsClient();
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            serviceManager.StopService();
        }

        [TestMethod]
        public void DummyTest()
        {
            Assert.IsTrue(true);
        }
    }
}
