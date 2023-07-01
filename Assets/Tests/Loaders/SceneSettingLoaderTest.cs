using NUnit.Framework;
using System.Xml.Linq;
using Loaders;

namespace Tests.Loaders
{
    [TestFixture]
    public class SceneSettingLoaderTest
    {

        [Test]
        public void 設定の読み込みのテスト()
        {
            var loader = new SceneSettingLoader();

            const string xmlText =
                @"<setting>
                    <defaultSize width=""1024"" height=""768"" />
                    <bgm number=""3"" />
                </setting>";

            var xDocument = XDocument.Parse(xmlText);
            var setting = loader.LoadSetting(xDocument);

            Assert.AreEqual(setting.DefaultImageWidth, 1024);
            Assert.AreEqual(setting.DefaultImageHeight, 768);
            Assert.AreEqual(setting.BGMNumber, 3);
        }

        [Test]
        public void Bgmがファイル名指定の設定の読み込みテスト()
        {
            var loader = new SceneSettingLoader();

            const string xmlText =
                @"<setting>
                    <defaultSize width=""1024"" height=""768"" />
                    <bgm number=""3"" fileName=""testSoundFile"" />
                </setting>";

            var xDocument = XDocument.Parse(xmlText);
            var setting = loader.LoadSetting(xDocument);

            Assert.AreEqual(setting.DefaultImageWidth, 1024);
            Assert.AreEqual(setting.DefaultImageHeight, 768);
            Assert.AreEqual(setting.BGMNumber, 3);
            Assert.AreEqual(setting.BGMFileName, "testSoundFile");
        }

        [Test]
        public void 設定に何も書いてない場合のテスト()
        {
            var loader = new SceneSettingLoader();

            const string xmlText =
                @"<setting>
                    <defaultSize />
                    <bgm />
                </setting>";

            var xDocument = XDocument.Parse(xmlText);
            var setting = loader.LoadSetting(xDocument);

            Assert.AreEqual(setting.DefaultImageWidth, 1280);
            Assert.AreEqual(setting.DefaultImageHeight, 720);
            Assert.AreEqual(setting.BGMNumber, 0);
        }
    }
}