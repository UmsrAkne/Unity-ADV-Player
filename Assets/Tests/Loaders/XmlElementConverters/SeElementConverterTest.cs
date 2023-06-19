using System.Xml.Linq;
using Loaders.XmlElementConverters;
using NUnit.Framework;
using SceneContents;

namespace Tests.Loaders.XmlElementConverters
{
    [TestFixture]
    public class SeElementConverterTest
    {
        [Test]
        public void Converterのテスト()
        {
            var converter = new SeElementConverter();
            var xml = XElement.Parse(@"<scenario> <se fileName=""sampleFile"" repeatCount=""4""/> </scenario>");
            var scenario = new Scenario();
            converter.Convert(xml, scenario);

            Assert.AreEqual("sampleFile", scenario.SeOrders[0].FileName);
            Assert.AreEqual(4, scenario.SeOrders[0].RepeatCount);
        }
    }
}