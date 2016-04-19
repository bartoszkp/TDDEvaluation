using System.Linq;
using Domain;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class PathStructureTests : TestsBase
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            TestsBase.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            TestsBase.ClassCleanup();
        }

        private PathEntry GetPathEntry(Path path)
        {
            return client
                .GetPathEntry(path.ToDto<Dto.Path>())
                .ToDomain<PathEntry>();
        }

        [TestMethod]
        public void GivenRootPath_WhenNoSignalsPresent_ReturnsEmptyPathEntry()
        {
            var result = GetPathEntry(Path.Root + "emptyPath");

            Assert.IsFalse(result.Signals.Any());
        }

        [TestMethod]
        public void GivenRootPath_WhenOneSignalPresentInRoot_ReturnsPathEntryWithThisSignal()
        {
            var directory = Path.Root + "oneSignal";
            var signalPath = directory + "signal";
            AddNewIntegerSignal(path: signalPath);

            var result = GetPathEntry(directory);

            CollectionAssert.AreEquivalent(new[] { signalPath }, result.Signals.Select(s => s.Path).ToArray());
        }

        [TestMethod]
        public void GivenRootPath_WhenTwoPathLevelsPresent_ReturnsOnlyDirectDescendants()
        {
            var directory = Path.Root + "twoLevels";
            var topLevelSignalPath = directory + "topLevelSignal";
            var nextLevelSignalPath = (directory + "subDirectory") + "nextLevelSignal";

            AddNewIntegerSignal(path: topLevelSignalPath);
            AddNewIntegerSignal(path: nextLevelSignalPath);

            var result = GetPathEntry(directory);

            CollectionAssert.AreEquivalent(new[] { topLevelSignalPath }, result.Signals.Select(s => s.Path).ToArray());
        }

        [TestMethod]
        public void GivenRootPath_WhenTwoPathLevelsPresent_ReturnsSubpaths()
        {
            var directory = Path.Root + "twoLevels2";
            var topLevelSignalPath = directory + "topLevelSignal";
            var subDirectory = directory + "subDirectory";
            var nextLevelSignalPath = subDirectory + "nextLevelSignal";

            AddNewIntegerSignal(path: topLevelSignalPath);
            AddNewIntegerSignal(path: nextLevelSignalPath);

            var result = GetPathEntry(directory);

            CollectionAssert.AreEquivalent(new[] { subDirectory }, result.SubPaths.ToArray());
        }

        [TestMethod]
        public void GivenRootPath_WhenOnePathLevelPresentAndTwoSignalsOnThatLevel_ReturnsOneCommonSubpath()
        {
            var topLevelDirectory = Path.Root + "topLevel";
            var directory = topLevelDirectory + "twoSignals";
            var firstTopLevelSignalPath = directory + "topLevelSignal1";
            var secondTopLevelSignalPath = directory + "topLevelSignal2";

            AddNewIntegerSignal(path: firstTopLevelSignalPath);
            AddNewIntegerSignal(path: secondTopLevelSignalPath);

            var result = GetPathEntry(topLevelDirectory);

            CollectionAssert.AreEquivalent(new[] { directory }, result.SubPaths.ToArray());
        }
    }
}
