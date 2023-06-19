using System.Xml.Linq;
using Loaders.XmlElementConverters;
using NUnit.Framework;
using SceneContents;

namespace Tests.Loaders.XmlElementConverters
{
    [TestFixture]
    public class VoiceElementConverterTest
    {
        [Test]
        public void Convertのテスト()
        {
            var converter = new VoiceElementConverter();
            var s = new Scenario();
            var xml = XElement.Parse(
                @"<scenario><voice fileName=""sampleVoice"" number=""3"" channel=""2""/></scenario>");

            converter.Convert(xml, s);

            Assert.AreEqual("sampleVoice", s.VoiceOrders[0].FileName);
            Assert.AreEqual(3, s.VoiceOrders[0].Index);
            Assert.AreEqual(2, s.VoiceOrders[0].Channel);
        }
    }
}