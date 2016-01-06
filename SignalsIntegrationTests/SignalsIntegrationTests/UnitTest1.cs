using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class UnitTest1
    {
        private Signals.SignalsClient signalsClient;
        
        [TestInitialize]
        public void TestInitialize()
        {
            signalsClient = new Signals.SignalsClient();
        }

        [TestMethod]
        public void CanAddAndGetSignal()
        {
            var path = new Signals.Path();
            path.Components = new[] { string.Empty };
            signalsClient.Add(path, typeof(int).ToString(), Signals.Granularity.Hour);

            var result = signalsClient.Get(path);

            Assert.AreEqual(0, result.Id);
            Assert.AreEqual(typeof(int).ToString(), result.DataType);
            Assert.AreEqual(path.ToString(), result.Path.ToString());
            Assert.AreEqual(Signals.Granularity.Hour, result.Granularity);
        }
    }
}
