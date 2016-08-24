using System;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace WebService.Tests
{
    [TestClass]
    public class SignalsWebServiceGetTests : SignalsWebServiceRepository
    {
        [TestMethod]
        public void GivenNoSignals_WhenGettingASignalByPath_ReturnsNull()
        {
            Domain.Signal sig = null;
            Setup(sig);

            var result = signalsWebService.Get(new Dto.Path() { Components = new[] { "a", "b" } });

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GivenASignal_WhenGettingASignalByPath_ReturnsIt()
        {
            int signalId = 1;
            var path = new Dto.Path() { Components = new[] { "a", "b" } };
            Setup(Utils.SignalWith(signalId, Domain.DataType.Decimal, Domain.Granularity.Year, Domain.Path.FromString("a/b")));

            var result = signalsWebService.Get(path);

            Assert.AreEqual(signalId, result.Id);
            Assert.AreEqual(Dto.DataType.Decimal, result.DataType);
            Assert.AreEqual(Dto.Granularity.Year, result.Granularity);
            CollectionAssert.AreEqual(path.Components.ToArray(), result.Path.Components.ToArray());
        }

        protected override void Setup(params object[] param)
        {
            signalsRepositoryMock
                .Setup(sr => sr.Get(It.IsAny<Domain.Path>()))
                .Returns(param[0] as Domain.Signal);
        }
    }
}
