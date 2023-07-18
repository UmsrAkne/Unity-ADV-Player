using NUnit.Framework;
using ScenarioSceneParts;

namespace Tests.ScenarioSceneParts
{
    [TestFixture]
    public class ScenePartsProviderTest
    {
        [Test]
        public void GetImageDrawerTest()
        {
            var dr2 = ScenePartsProvider.GetImageDrawer(2);
            var dr0 = ScenePartsProvider.GetImageDrawer(0);
            var dr1 = ScenePartsProvider.GetImageDrawer(1);
            
            Assert.NotNull(dr2);
            Assert.NotNull(dr0);
            Assert.NotNull(dr1);
            
            Assert.AreEqual(dr0 ,ScenePartsProvider.GetImageDrawer(0));
            Assert.AreEqual(dr1 ,ScenePartsProvider.GetImageDrawer(1));
            Assert.AreEqual(dr2 ,ScenePartsProvider.GetImageDrawer(2));
        }
    }
}