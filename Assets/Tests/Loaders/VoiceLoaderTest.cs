using System.Collections.Generic;
using Loaders;
using NUnit.Framework;
using SceneContents;

namespace Tests.Loaders
{
    public class VoiceLoaderTest
    {
        [Test]
        [TestCase(TargetAudioType.Voice, "voices")]
        [TestCase(TargetAudioType.BgVoice, "bgvs")]
        public void サウンドロードのテスト(TargetAudioType audioType, string directoryName)
        {
            var res = new Resource();
            var materialGetter = new DummyMaterialGetter();
            var pathListGen = new DummyPathListGen()
            {
                SoundFilePaths = new List<string> { "a.ogg", "b.ogg", "c.ogg" }
            };

            var loader = new VoiceLoader()
            {
                MaterialLoader = materialGetter,
                PathListGen = pathListGen,
                Resource = res,
                TargetAudioType = audioType
            };

            loader.Load("testPath");

            Assert.NotNull(res.GetSound(audioType, "a.ogg"));
            Assert.NotNull(res.GetSound(audioType, "b.ogg"));
            Assert.NotNull(res.GetSound(audioType, "c.ogg"));

            Assert.AreEqual(@$"testPath\{directoryName}\a.ogg", materialGetter.Paths[0]);
            Assert.AreEqual(@$"testPath\{directoryName}\b.ogg", materialGetter.Paths[1]);
            Assert.AreEqual(@$"testPath\{directoryName}\c.ogg", materialGetter.Paths[2]);
        }

        /// <summary>
        /// Se に関しては読み込み先のディレクトリが異なるため。テストを分離している。
        /// </summary>
        [TestCase(TargetAudioType.Se, @"commonResource\ses")]
        public void Seロードのテスト(TargetAudioType audioType, string directoryName)
        {
            var res = new Resource();
            var materialGetter = new DummyMaterialGetter();
            var pathListGen = new DummyPathListGen()
            {
                SoundFilePaths = new List<string> { "a.ogg", "b.ogg", "c.ogg" }
            };

            var loader = new VoiceLoader()
            {
                MaterialLoader = materialGetter,
                PathListGen = pathListGen,
                Resource = res,
                TargetAudioType = audioType
            };

            loader.Load("testPath");

            Assert.NotNull(res.GetSound(audioType, "a.ogg"));
            Assert.NotNull(res.GetSound(audioType, "b.ogg"));
            Assert.NotNull(res.GetSound(audioType, "c.ogg"));

            Assert.AreEqual(@$"{directoryName}\a.ogg", materialGetter.Paths[0]);
            Assert.AreEqual(@$"{directoryName}\b.ogg", materialGetter.Paths[1]);
            Assert.AreEqual(@$"{directoryName}\c.ogg", materialGetter.Paths[2]);
        }
    }
}