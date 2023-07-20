using System.Collections.Generic;
using System.Xml.Linq;
using Loaders.XmlElementConverters;
using NUnit.Framework;

namespace Tests.Loaders.XmlElementConverters
{
    public class TestBlinkElementConverter
    {
        [Test]
        public void Convertテスト()
        {
            const string xml = "<blink>"
                               + "    <image baseImageName=\"A\" names=\"B, C,D\" />"
                               + "    <image baseImageName=\"E\" names=\"F,G\" />"
                               + "</blink>";

            var doc = XElement.Parse(xml);
            var converter = new BlinkElementConverter();
            var orders = converter.Convert(doc);

            Assert.AreEqual(orders[0].BaseImageName, "A");
            CollectionAssert.AreEqual(orders[0].Names, new List<string>() { "B", "C", "D" });

            Assert.AreEqual(orders[1].BaseImageName, "E");
            CollectionAssert.AreEqual(orders[1].Names, new List<string>() { "F", "G" });
        }

        [Test]
        public void Convertテスト_空要素の場合()
        {
            const string xml = "<blink>" + "</blink>";

            var doc = XElement.Parse(xml);
            var converter = new BlinkElementConverter();
            var orders = converter.Convert(doc);

            Assert.AreEqual(0, orders.Count);
        }
    }
}