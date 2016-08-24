using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using System.Linq;
using WebService.Tests.SignalsWebServiceTests.Infrastructure;

namespace WebService.Tests.SignalsWebServiceTests
{
    [TestClass]
    public class SignalsWebServiceGetPathEntryTests : SignalsWebServiceRepository
    {
        [TestMethod]
        public void GivenASignals_WhenGettingFiveSignalsWithPrefix_ItReturnsOneSignal()
        {
            Setup(_premade5);

            var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root" } });

            Assert.AreEqual(1, result.Signals.Count());
            CollectionAssert.AreEquivalent(new[] { "root", "s1" }, result.Signals.First().Path.Components.ToArray());
        }
        [TestMethod]
        public void GivenASignals_WhenGettingFiveSignalsWithPrefix_ItReturnsTwoPrefixes()
        {
            Setup(_premade5);

            var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root" } });

            Assert.AreEqual(2, result.SubPaths.Count());
            CollectionAssert.AreEquivalent(new[] { "root", "podkatalog" }, result.SubPaths.First().Components.ToArray());
            CollectionAssert.AreEquivalent(new[] { "root", "podkatalog2" }, result.SubPaths.Last().Components.ToArray());

        }
        [TestMethod]
        public void GivenASignals_WhenGettingSignalsWithPrefixThatSignalIsAlsoCatalog_ItReturnsThatSignal()
        {
            Setup(_premade2);

            var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root" } });

            Assert.AreEqual(1, result.SubPaths.Count());
            Assert.AreEqual(1, result.Signals.Count());
            CollectionAssert.AreEquivalent(new[] { "root", "s5" }, result.SubPaths.First().Components.ToArray());
            CollectionAssert.AreEquivalent(new[] { "root", "s5" }, result.Signals.First().Path.Components.ToArray());
        }
        [TestMethod]
        public void GivenASignals_WhenGettingSignalsWithPrefixThatIsFullSignalPath_ItReturnsThatSignal()
        {
            Setup(_premade2);

            var result = signalsWebService.GetPathEntry(new Dto.Path() { Components = new[] { "root", "s5" } });

            Assert.AreEqual(0, result.SubPaths.Count());
            Assert.AreEqual(1, result.Signals.Count());
            CollectionAssert.AreEquivalent(new[] { "root", "s5", "s4" }, result.Signals.First().Path.Components.ToArray());
        }

        private void Setup(IEnumerable<Domain.Signal> signals)
        {
            signalsRepositoryMock.Setup(sr => sr
                .GetAllWithPathPrefix(It.IsAny<Domain.Path>()))
                .Returns(signals);
        }

        private List<Domain.Signal> _premade5 = new List<Domain.Signal>() {
            Utils.SignalWith(path: Domain.Path.FromString("root/s1")),
            Utils.SignalWith(path: Domain.Path.FromString("root/podkatalog/s2")),
            Utils.SignalWith(path: Domain.Path.FromString("root/podkatalog/s3")),
            Utils.SignalWith(path: Domain.Path.FromString("root/podkatalog/podpodkatalog/s4")),
            Utils.SignalWith(path: Domain.Path.FromString("root/podkatalog2/s5"))
        };

        private List<Domain.Signal> _premade2 = new List<Domain.Signal>() {
            Utils.SignalWith(path: Domain.Path.FromString("root/s5")),
            Utils.SignalWith(path: Domain.Path.FromString("root/s5/s4"))
        };
    }
}
