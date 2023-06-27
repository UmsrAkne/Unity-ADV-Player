using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Animations;
using SceneContents;

namespace Loaders.XmlElementConverters
{
    public class AnimeElementConverter : IXMLElementConverter
    {
        public string TargetElementName => "anime";

        public List<string> Log { get; } = new List<string>();

        public void Convert(XElement xmlElement, Scenario scenario)
        {
            var tags = xmlElement.Elements(TargetElementName);

            foreach (XElement animeTag in tags)
            {
                var anime = GenerateAnimation(animeTag);
                if (anime != null)
                {
                    if (anime is AnimationChain chain)
                    {
                        // 生成したアニメが AnimationChain だった場合は、内部のアニメタグを取り出して、オブジェクトに XElement の状態で記録する。
                        var nestedAnimationTags = animeTag.Elements(TargetElementName);
                        foreach (var na in nestedAnimationTags)
                        {
                            chain.AddAnimationTag(na);
                        }
                    }

                    scenario.Animations.Add(anime);
                }
            }
        }

        public IAnimation GenerateAnimation(XElement element)
        {
            var att = element.Attribute("name");
            if (att == null || string.IsNullOrWhiteSpace(att.Value))
            {
                return null;
            }

            IAnimation anime = GenerateAnimation(att.Value);

            if (anime == null)
            {
                return null;
            }

            Type type = anime.GetType();
            var attributes = element.Attributes().Where(a => a.Name != "name");

            foreach (var attribute in attributes)
            {
                // xml の属性名は先頭小文字。プロパティは先頭大文字となるため
                // プロパティの存在を確認する前に先頭を大文字に変換した文字列を準備する。
                var originalName = attribute.Name.ToString();
                var attributeNameUpperCamel = $"{Char.ToUpper(originalName[0])}{originalName.Substring(1)}";

                // リフレクションを使用して、セットしたいプロパティがクラスに存在するかを確認してからセットする。
                var propInfo = type.GetProperty(attributeNameUpperCamel);

                if (propInfo != null)
                {
                    if (propInfo.PropertyType == typeof(int))
                    {
                        propInfo.SetValue(anime, int.Parse(attribute.Value));
                    }

                    if (propInfo.PropertyType == typeof(double))
                    {
                        propInfo.SetValue(anime, double.Parse(attribute.Value));
                    }

                    if (propInfo.PropertyType == typeof(string))
                    {
                        propInfo.SetValue(anime, attribute.Value);
                    }
                }
            }

            return anime;
        }

        private IAnimation GenerateAnimation(string animationName)
        {
            switch (animationName)
            {
                case "alphaChanger": return new AlphaChanger(true);
                case "shake": return new Shake();
                case "slide": return new Slide();
                case "flash": return new Flash();
                case "bound": return new Bound();
                case "chain": return new AnimationChain();
                case "animationChain": return new AnimationChain();
                case "draw": return new Draw();
                case "scaleChange": return new ScaleChange();
            }

            DebugTools.Logger.Add($"AnimeElementConverter : アニメーションの生成に失敗。 name={animationName}");
            return null;
        }
    }
}