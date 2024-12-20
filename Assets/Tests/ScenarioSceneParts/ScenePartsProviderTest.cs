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

            Assert.AreEqual(dr0, ScenePartsProvider.GetImageDrawer(0));
            Assert.AreEqual(dr1, ScenePartsProvider.GetImageDrawer(1));
            Assert.AreEqual(dr2, ScenePartsProvider.GetImageDrawer(2));
        }

        [Test]
        public void GetVoicePlayerTest()
        {
            var v1 = ScenePartsProvider.GetVoicePlayer(1);
            var v0 = ScenePartsProvider.GetVoicePlayer(0);

            Assert.NotNull(v0);
            Assert.NotNull(v1);

            Assert.AreEqual(v0, ScenePartsProvider.GetVoicePlayer(0));
            Assert.AreEqual(v1, ScenePartsProvider.GetVoicePlayer(1));
            Assert.AreEqual(v0.Channel, 0);
            Assert.AreEqual(v1.Channel, 1);
        }

        [Test]
        public void GetBgvPlayerTest()
        {
            var v1 = ScenePartsProvider.GetBgvPlayer(1, null);
            var v0 = ScenePartsProvider.GetBgvPlayer(0, null);

            Assert.NotNull(v0);
            Assert.NotNull(v1);

            Assert.AreEqual(v0, ScenePartsProvider.GetBgvPlayer(0, null));
            Assert.AreEqual(v1, ScenePartsProvider.GetBgvPlayer(1, null));
            Assert.AreEqual(v0.VoicePlayer, ScenePartsProvider.GetVoicePlayer(0));
            Assert.AreEqual(v1.VoicePlayer, ScenePartsProvider.GetVoicePlayer(1));
        }
    }
}