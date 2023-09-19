using System.Collections.Generic;
using Animations;
using NUnit.Framework;
using SceneContents;
using Tests.ScenarioSceneParts;
using Tests.SceneContents;

namespace Tests.Animations
{
    [TestFixture]
    public class BlinkTest
    {
        [Test]
        public void ExecuteTest()
        {
            var drawer = new DummyDrawer()
            {
                LastOrder = new ImageOrder() { Names = { string.Empty, "b", string.Empty, string.Empty, }, },
            };

            var resource = new DummyResource();
            resource.BlinkOrderDictionary = new Dictionary<string, BlinkOrder>()
            {
                { "b", new BlinkOrder() { Names = new List<string>() { "blink1", "blink2", "blink3", }, } },
            };

            var blink = new Blink { ImageDrawer = drawer, Resource = resource, };

            blink.Execute();
            blink.Execute();
            blink.Execute();

            Assert.That(drawer.ImageOrderHistories[0].Names[1], Is.EqualTo("blink1"));
            Assert.That(drawer.ImageOrderHistories[1].Names[1], Is.EqualTo("blink2"));
            Assert.That(drawer.ImageOrderHistories[2].Names[1], Is.EqualTo("blink3"));
        }
    }
}