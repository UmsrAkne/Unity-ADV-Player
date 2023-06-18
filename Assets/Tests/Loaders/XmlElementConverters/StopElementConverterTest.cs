using System.Linq;
using System.Xml.Linq;
using Loaders.XmlElementConverters;
using NUnit.Framework;
using ScenarioSceneParts;
using SceneContents;

namespace Tests.Loaders.XmlElementConverters
{
    public class StopElementConverterTest
    {
        [TestCase("<scenario> <stop target=\"se\" /> </scenario>", StoppableSceneParts.Se)]
        [TestCase("<scenario> <stop target=\"voice\" /> </scenario>", StoppableSceneParts.Voice)]
        [TestCase("<scenario> <stop target=\"anime\" /> </scenario>", StoppableSceneParts.Anime)]
        [TestCase("<scenario> <stop target=\"bgv\" /> </scenario>", StoppableSceneParts.Bgv)]
        [TestCase("<scenario> <stop target=\"backgroundVoice\" /> </scenario>", StoppableSceneParts.BackgroundVoice)]
        [TestCase("<scenario> <stop target=\"bgm\" /> </scenario>", StoppableSceneParts.Bgm)]
        public void Target属性の読み込みテスト(string xmlText, StoppableSceneParts result)
        {
            var converter = new StopElementConverter();
            var scenarioElement = XDocument.Parse(xmlText).Root;
            var scenario = new Scenario();
            converter.Convert(scenarioElement, scenario);

            Assert.AreEqual(scenario.StopOrders.Count, 1);
            Assert.AreEqual(scenario.StopOrders.First().Target, result);
        }

        [Test]
        public void Target属性がなかった場合のテスト()
        {
            var converter = new StopElementConverter();
            Assert.AreEqual(converter.Log.Count, 0);
            var scenarioElement = XDocument.Parse("<scenario> <stop /> </scenario>").Root;
            var scenario = new Scenario();
            converter.Convert(scenarioElement, scenario);

            Assert.AreEqual(scenario.StopOrders.Count, 1);
            Assert.AreEqual(converter.Log.Count, 1, "エラーログが追加されているか");
            Assert.AreEqual(scenario.StopOrders.First().Target, new StoppableSceneParts());
        }

        [Test]
        public void 属性読み込みテスト()
        {
            var converter = new StopElementConverter();
            var scenarioElement = XDocument.Parse("<scenario> <stop layerIndex=\"2\" channel=\"1\" name=\"shake\" /> </scenario>").Root;
            var scenario = new Scenario();
            converter.Convert(scenarioElement, scenario);

            Assert.AreEqual(scenario.StopOrders.Count, 1);
            Assert.AreEqual(scenario.StopOrders.First().Channel, 1);
            Assert.AreEqual(scenario.StopOrders.First().LayerIndex, 2);
            Assert.AreEqual(scenario.StopOrders.First().Name, "shake");
        }

        [Test]
        public void 複数要素の読み込みテスト()
        {
            var converter = new StopElementConverter();
            var scenarioElement = XDocument.Parse("<scenario> <stop layerIndex=\"2\"  /><stop name=\"shake\" /> </scenario>").Root;
            var scenario = new Scenario();
            converter.Convert(scenarioElement, scenario);

            Assert.AreEqual(scenario.StopOrders.Count, 2);
        }
    }
}