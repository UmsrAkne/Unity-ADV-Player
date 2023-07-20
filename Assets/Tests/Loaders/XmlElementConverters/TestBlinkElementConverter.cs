using System.Collections.Generic;
using System.Xml.Linq;
using Loaders.XmlElementConverters;
using NUnit.Framework;

namespace Tests.Loaders.XmlElementConverters
{
    public class TestBlinkElementConverter
    {
        [TestCase("<blink> <image baseImageName=\"A\" names=\"B, C,D\" /> </blink>")]
        [TestCase("<blink> <image baseImageName=\"A.png\" names=\"B.png, C.png,D.png\" /> </blink>")]
        public void Convertテスト(string text)
        {
            var doc = XElement.Parse(text);
            var converter = new BlinkElementConverter();
            var orders = converter.Convert(doc);

            Assert.AreEqual(orders[0].BaseImageName, "A");
            CollectionAssert.AreEqual(orders[0].Names, new List<string>() { "B", "C", "D" });
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