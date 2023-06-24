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
        }
    }
}