using System.Collections.Generic;
using NUnit.Framework;
using ScenarioSceneParts;
using SceneContents;
using Tests.SceneContents;

namespace Tests.ScenarioSceneParts
{
    public class ImageDrawerTest
    {
        [Test]
        public void ExecuteAddImageTest()
        {
            var res = new DummyResource() { SpriteWrappers = new List<SpriteWrapper>() { new (), new (), new (), }, };

            var imageDrawer = new ImageDrawer() { ImageContainer = new DisplayObjectContainerMock(), };
            imageDrawer.SetResource(res);

            var scenario = new Scenario();
            scenario.ImageOrders.Add(new ImageOrder()
            {
                IsDrawOrder = false,
                Names = { "A0", "B0", "C0", string.Empty, },
                TargetLayerIndex = 0,
            });

            imageDrawer.SetScenario(scenario);
            imageDrawer.Execute();

            CollectionAssert.AreEqual(new List<string> { "A0", "B0", "C0", }, res.RequestedImageNames);
        }

        [Test]
        public void DrawTest_最初から呼び出し()
        {
            var res = new DummyResource() { SpriteWrappers = new List<SpriteWrapper>() { new (), new (), new (), }, };

            var imageDrawer = new ImageDrawer() { ImageContainer = new DisplayObjectContainerMock(), };
            imageDrawer.SetResource(res);

            var scenario = new Scenario();
            scenario.DrawOrders.Add(new ImageOrder()
            {
                IsDrawOrder = true,
                Names = { "A0", "B0", "C0", string.Empty, },
                TargetLayerIndex = 0,
            });

            imageDrawer.SetScenario(scenario);
            imageDrawer.Execute();

            CollectionAssert.AreEqual(new List<string> { "A0", "B0", "C0", }, res.RequestedImageNames);
        }
    }
}