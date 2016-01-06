using Microsoft.VisualStudio.TestTools.UnitTesting;
using Signals.Domain;
using Signals.Dto.Conversions;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class UnitTest1
    {
        private WS.SignalsClient signalsClient;
        
        [TestInitialize]
        public void TestInitialize()
        {
            signalsClient = new WS.SignalsClient();
        }

        [TestMethod]
        public void CanAddAndGetSignal()
        {
            var path = Path.FromString(string.Empty);

            var signal = new Signal()
            {
                Path = path,
                Granularity = Granularity.Day,
                DataType = DataType.Integer
            };

            signalsClient.Add(signal.ToDto());

            var result = signalsClient.Get(path.ToDto()).ToDomain();

            Assert.AreEqual(0, result.Id);
            Assert.AreEqual(DataType.Integer, result.DataType);
            Assert.AreEqual(path.ToString(), result.Path.ToString());
            Assert.AreEqual(Granularity.Day, result.Granularity);
        }
    }
}
