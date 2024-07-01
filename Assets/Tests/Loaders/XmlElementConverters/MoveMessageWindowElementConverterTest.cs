using System.Xml.Linq;
using Loaders.XmlElementConverters;
using NUnit.Framework;
using SceneContents;

namespace Tests.Loaders.XmlElementConverters
{
    [TestFixture]
    public class MoveMessageWindowElementConverterTest
    {
        [Test]
        public void Convertのテスト()
        {
            var converter = new MoveMessageWindowElementConverter();
            var xml = XElement.Parse(
                @"<scenario><moveMessageWindow /></scenario>");

            var s = new Scenario();
            Assert.IsFalse(s.MoveMessageWindow, "デフォルトは false");

            converter.Convert(xml, s);
            Assert.IsTrue(s.MoveMessageWindow, "要素を読み取って値が変化しているか");
        }

        [Test]
        public void Convertのテスト_要素なし()
        {
            var converter = new MoveMessageWindowElementConverter();
            var xml = XElement.Parse(
                @"<scenario></scenario>");

            var s = new Scenario();
            Assert.IsFalse(s.MoveMessageWindow, "デフォルトは false");

            converter.Convert(xml, s);
            Assert.IsFalse(s.MoveMessageWindow, "該当の要素を含まないので、 false のままのはず");
        }
    }
}