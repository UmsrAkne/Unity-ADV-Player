using System.Collections.Generic;
using Loaders;
using NUnit.Framework;
using SceneContents;

namespace Tests.Loaders
{
    [TestFixture]
    public class ImageLoaderTest
    {
        [Test]
        public void 画像ロードのテスト()
        {
            var res = new Resource();
            var materialGetter = new DummyMaterialGetter();
            var pathListGen = new DummyPathListGen()
            {
                ImageFilePaths = new List<string>
                {
                    "a.png", "b.png", "c.png"
                }
            };

            var loader = new ImageLoader
            {
                MaterialLoader = materialGetter,
                PathListGen = pathListGen,
                Resource = res,
                TargetImageType = TargetImageType.EventCg,
                UsingFileNames = new HashSet<string>
                {
                    "a", "b", "c",
                }
            };

            loader.Load("testPath");

            Assert.NotNull(res.GetImage(TargetImageType.EventCg, "a.png"));
            Assert.NotNull(res.GetImage(TargetImageType.EventCg, "b.png"));
            Assert.NotNull(res.GetImage(TargetImageType.EventCg, "c.png"));
            Assert.AreEqual(res.Images.Count, 3);

            Assert.AreEqual(@"testPath\images\a.png", materialGetter.Paths[0]);
            Assert.AreEqual(@"testPath\images\b.png", materialGetter.Paths[1]);
            Assert.AreEqual(@"testPath\images\c.png", materialGetter.Paths[2]);
        }
    }
}