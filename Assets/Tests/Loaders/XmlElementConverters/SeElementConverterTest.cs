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
            var xml = XElement.Parse(@"<scenario> <se fileName=""sampleFile""/> </scenario>");
            var scenario = new Scenario();
            converter.Convert(xml, scenario);
        }
    }
}