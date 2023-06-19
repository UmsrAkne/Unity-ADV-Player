using System.Xml.Linq;
using Loaders.XmlElementConverters;
using NUnit.Framework;
using SceneContents;

namespace Tests.Loaders.XmlElementConverters
{
    [TestFixture]
    public class ChapterElementConverterTest
    {
        [Test]
        public void Converterのテスト()
        {
            var converter = new ChapterElementConverter();
            var scenario = new Scenario();
            converter.Convert(XElement.Parse(@"<scenario><chapter name=""testChapter"" /></scenario>"), scenario);
            Assert.AreEqual("testChapter", scenario.ChapterName);
        }
    }
}