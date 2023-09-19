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
                LastOrder = new ImageOrder(),
            };

            var resource = new DummyResource();
            resource.BlinkOrderDictionary = new Dictionary<string, BlinkOrder>()
            {
                { "b", new BlinkOrder() { Names = new List<string>() { "blink1", "blink2", "blink3", }, } },
            };

            var blink = new Blink { ImageDrawer = drawer, Resource = resource, };

            blink.Execute();
        }
    }
}