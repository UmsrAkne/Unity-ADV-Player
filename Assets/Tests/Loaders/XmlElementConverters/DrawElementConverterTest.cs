using System.Xml.Linq;
using Loaders.XmlElementConverters;
using NUnit.Framework;
using SceneContents;

namespace Tests.Loaders.XmlElementConverters
{
    public class DrawElementConverterTest
    {
        [Test]
        public void Convertのテスト()
        {
            var converter = new DrawElementConverter();
            var scenario = new Scenario();

            const string xmlText = @"<scenario>" +
                                   @"<draw a=""imgA"" b=""imgB"" c=""imgC"" d="""" depth=""0.5"" delay=""20"" targetLayerIndex=""2""/>" +
                                   @"</scenario>";

            var drawElement = XElement.Parse(xmlText);
            converter.Convert(drawElement, scenario);

            Assert.AreEqual(1, scenario.DrawOrders.Count);
            Assert.AreEqual("imgA", scenario.DrawOrders[0].Names[0]);
            Assert.AreEqual("imgB", scenario.DrawOrders[0].Names[1]);
            Assert.AreEqual("imgC", scenario.DrawOrders[0].Names[2]);
            Assert.IsTrue(string.IsNullOrEmpty(scenario.DrawOrders[0].Names[3]));
            Assert.AreEqual(0.5, scenario.DrawOrders[0].Depth);
            Assert.AreEqual(20, scenario.DrawOrders[0].Delay);
            Assert.AreEqual(2, scenario.DrawOrders[0].TargetLayerIndex);
            Assert.IsTrue(scenario.DrawOrders[0].IsDrawOrder);
        }

        [Test]
        public void 複数生成テスト()
        {
            var converter = new DrawElementConverter();
            var scenario = new Scenario();

            const string xmlText = @"<scenario>" +
                                   @"<draw a=""imgA"" b=""imgB"" c=""imgC"" d="""" depth=""0.5"" targetLayerIndex=""2""/>" +
                                   @"<draw a=""imgA"" b=""imgB"" c=""imgC"" d="""" depth=""0.5"" targetLayerIndex=""2""/>" +
                                   @"</scenario>";

            var drawElement = XElement.Parse(xmlText);
            converter.Convert(drawElement, scenario);
            Assert.AreEqual(2, scenario.DrawOrders.Count);
        }
    }
}