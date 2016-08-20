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
        [TestCategory("issue7")]
        public void GivenRootPath_WhenNoSignalsPresent_ReturnsEmptyPathEntry()
        {
            var result = GetPathEntry(Path.Root + "emptyPath");

            Assert.IsFalse(result.Signals.Any());
            Assert.IsFalse(result.SubPaths.Any());
        }

        [TestMethod]
        [TestCategory("issue7")]
        public void GivenOneSignal_WhenReadingItsParent_ReturnsPathEntryWithThisSignal()
        {
            var directory = Path.Root + "oneSignal";
            var signalPath = directory + "signal";
            AddNewIntegerSignal(path: signalPath);

            var result = GetPathEntry(directory);

            CollectionAssert.AreEquivalent(new[] { signalPath }, result.Signals.Select(s => s.Path).ToArray());
        }

        [TestMethod]
        [TestCategory("issue7")]
        public void GivenOneSignal_WhenReadingItsParent_ReturnsPathEntryWithNoSubPaths()
        {
            var directory = Path.Root + "oneSignal_nosubs";
            var signalPath = directory + "signal";
            AddNewIntegerSignal(path: signalPath);

            var result = GetPathEntry(directory);

            Assert.IsFalse(result.SubPaths.Any());
        }

        [TestMethod]
        [TestCategory("issue7")]
        public void GivenTwoPathLevels_WhenReadingTopLevel_ReturnsOnlyDirectDescendants()
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
        [TestCategory("issue7")]
        public void GivenTwoPathLevels_WhenReadingTopLevel_ReturnsSubpaths()
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
        [TestCategory("issue7")]
        public void GivenTwoSignalsInOnePath_WhenReadingItsParent_ReturnsOneCommonSubpath()
        {
            var topLevelDirectory = Path.Root + "topLevelWithTwoSignals";
            var directory = topLevelDirectory + "twoSignals";
            var firstTopLevelSignalPath = directory + "topLevelSignal1";
            var secondTopLevelSignalPath = directory + "topLevelSignal2";

            AddNewIntegerSignal(path: firstTopLevelSignalPath);
            AddNewIntegerSignal(path: secondTopLevelSignalPath);

            var result = GetPathEntry(topLevelDirectory);

            CollectionAssert.AreEquivalent(new[] { directory }, result.SubPaths.ToArray());
        }

        [TestMethod]
        [TestCategory("issue7")]
        public void GivenPathsWithCommonPrefixInName_WhenReadingTheFirstOne_DoesntReturnTheOther()
        {
            var topLevelDirectory = Path.Root + "topLevelWithCommonPrefix";
            var directory1 = topLevelDirectory + "commonPrefix";
            var directory2 = topLevelDirectory + "commonPrefixOther";

            var expectedSignalPath = directory1 + "signal1";
            AddNewIntegerSignal(path: expectedSignalPath);
            AddNewIntegerSignal(path: directory2 + "signal2");

            var result = GetPathEntry(directory1);

            CollectionAssert.AreEquivalent(new[] { expectedSignalPath }, result.Signals.Select(s => s.Path).ToArray());
        }

        [TestMethod]
        [TestCategory("issue7")]
        public void GivenThreePathsLevels_ReadingTopLevel_ReturnsOnlyDirectSubPaths()
        {
            var topLevelDirectory = Path.Root + "topLevelWithTwoSubLevels";
            var subDir1 = topLevelDirectory + "sub1";
            var subDir2 = topLevelDirectory + "sub2";
            var subsubDir = subDir1 + "subsub";

            AddNewIntegerSignal(path: subDir1 + "signal1");
            AddNewIntegerSignal(path: subDir2 + "signal2");
            AddNewIntegerSignal(path: subsubDir + "signal3");

            var result = GetPathEntry(topLevelDirectory);

            CollectionAssert.AreEquivalent(new[] { subDir1, subDir2 }, result.SubPaths.ToArray());
        }

        [TestMethod]
        [TestCategory("issue7")]
        public void GivenSignalPathWithSignalsInSubdirs_ReadingTopLevel_ReturnsNoSignalsAndTwoSubPaths()
        {
            var topLevelDirectory = Path.Root + "topLevelWithoutSignalsButWithSubpaths";
            var subDir1 = topLevelDirectory + "sub1";
            var subDir2 = topLevelDirectory + "sub2";

            AddNewIntegerSignal(path: subDir1 + "signal1");
            AddNewIntegerSignal(path: subDir2 + "signal2");

            var result = GetPathEntry(topLevelDirectory);

            CollectionAssert.AreEquivalent(new[] { subDir1, subDir2 }, result.SubPaths.ToArray());
            Assert.IsFalse(result.Signals.Any());
        }

        [TestMethod]
        [TestCategory("issue7")]
        public void GivenSingleSignalAtSecondSubLevel_ReadingTopLevel_ReturnsFirstLevelSubPath()
        {
            var topLevel = Path.Root + "topLevelWithSingleDeepChild";
            var level1 = topLevel + "level1";
            var level2 = level1 + "level2";

            AddNewIntegerSignal(path: level2 + "signal1");

            var result = GetPathEntry(topLevel);

            CollectionAssert.AreEquivalent(new[] { level1 }, result.SubPaths.ToArray());
        }
    }
}
