using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Animations;
using Loaders.XmlElementConverters;
using NUnit.Framework;
using SceneContents;

namespace Tests.Animations
{
    [TestFixture]
    public class AnimeElementConverterTest
    {
        [Test]
        public void Xmlからアニメーションを生成するテスト()
        {
            var aec = new AnimeElementConverter();

            var animationTypes = new Dictionary<string, Type>
            {
                { "alphaChanger", typeof(AlphaChanger) },
                { "shake", typeof(Shake) },
                { "slide", typeof(Slide) },
                { "flash", typeof(Flash) },
                { "bound", typeof(Bound) },
                { "chain", typeof(AnimationChain) },
                { "animationChain", typeof(AnimationChain) },
                { "draw", typeof(Draw) },
                { "image", typeof(Image) },
                { "scaleChange", typeof(ScaleChange) },
                { "dummy", typeof(Dummy) },
            };

            var s = new Scenario();

            foreach (var animationType in animationTypes)
            {
                var xml = $@"<scenario><anime name=""{animationType.Key}"" /></scenario>";
                var doc = XDocument.Parse(xml);
                aec.Convert(doc.Root, s);

                Assert.IsTrue(s.Animations.First().GetType() == animationType.Value,
                    $"Expected {animationType.Value}, but got {s.Animations[0].GetType()}");

                s = new Scenario();
            }
        }
    }
}