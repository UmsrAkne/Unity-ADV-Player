using System.IO;
using System.Linq;
using Loaders.XmlElementConverters;
using SceneContents;
using UnityEngine;

namespace Loaders
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class SceneSettingLoader
    {
        private const string BgmElementName = "bgm";
        private const string SeElementName = "se";
        private const string VoiceElementName = "voice";
        private const string MessageWindowElementName = "messageWindow";
        private const string BgvElementName1 = "bgv";
        private const string BgvElementName2 = "backgroundVoice";
        private const string DefaultSizeElementName = "defaultSize";
        private const string FileNameAttribute = "fileName";
        private const string HeightAttribute = "height";
        private const string AlphaAttribute = "alpha";

        private const string NameAttribute = "name";
        private const string NumberAttribute = "number";
        private const string VolumeAttribute = "volume";

        private const string WidthAttribute = "width";
        private const string XAttribute = "x";
        private const string YAttribute = "y";
        private readonly XName blinkElementName = "blink";

        public List<string> Log { get; private set; } = new List<string>();

        public SceneSetting LoadSetting(XDocument xml)
        {
            var setting = new SceneSetting();
            var settingTag = xml.Root;

            if (settingTag == null)
            {
                return setting;
            }

            if (settingTag.Element(DefaultSizeElementName) != null)
            {
                var w = settingTag.Element(DefaultSizeElementName)?.Attribute(WidthAttribute)?.Value;
                setting.DefaultImageWidth = w != null ? int.Parse(w) : setting.DefaultImageWidth;

                var h = settingTag.Element(DefaultSizeElementName)?.Attribute(HeightAttribute)?.Value;
                setting.DefaultImageHeight = h != null ? int.Parse(h) : setting.DefaultImageHeight;
            }

            var bgmElement = settingTag.Element(BgmElementName);
            if (bgmElement != null)
            {
                var bgmNumber = bgmElement.Attribute(NumberAttribute)?.Value;
                setting.BGMNumber = bgmNumber != null ? int.Parse(bgmNumber) : setting.BGMNumber;

                var bgmName = bgmElement.Attribute(FileNameAttribute);
                setting.BGMFileName = bgmName != null ? bgmName.Value : string.Empty;

                var bgmVolume = bgmElement.Attribute(VolumeAttribute)?.Value;
                setting.BGMVolume = bgmVolume != null ? float.Parse(bgmVolume) : setting.BGMVolume;
            }

            // se 要素が記述されていれば、効果音のボリュームを設定する。
            var seElement = settingTag.Element(SeElementName);
            if (seElement != null)
            {
                var seVolume = seElement.Attribute(VolumeAttribute)?.Value;
                setting.SeVolume = seVolume != null ? float.Parse(seVolume) : setting.SeVolume;
            }

            // voice 要素が記述されていれば、ボイスのボリュームを設定する。
            var voiceElement = settingTag.Element(VoiceElementName);
            if (voiceElement != null)
            {
                var voiceVolume = voiceElement.Attribute(VolumeAttribute)?.Value;
                setting.VoiceVolume = voiceVolume != null ? float.Parse(voiceVolume) : setting.VoiceVolume;
            }

            var messageWindowElement = settingTag.Element(MessageWindowElementName);
            if (messageWindowElement != null)
            {
                var alpha = messageWindowElement.Attribute(AlphaAttribute)?.Value;
                setting.MessageWindowAlpha = alpha != null ? float.Parse(alpha) : setting.MessageWindowAlpha;

                var x = messageWindowElement.Attribute(XAttribute)?.Value;
                var y = messageWindowElement.Attribute(YAttribute)?.Value;

                setting.MessageWindowPos =
                    new Vector2(
                        x != null ? float.Parse(x) : setting.MessageWindowPos.x,
                        y != null ? float.Parse(y) : setting.MessageWindowPos.y
                    );
            }

            var bgvElement = settingTag.Element(BgvElementName1) ?? settingTag.Element(BgvElementName2);

            if (bgvElement != null)
            {
                var bgvVolume = bgvElement.Attribute(VolumeAttribute)?.Value;
                setting.BgvVolume = bgvVolume != null ? float.Parse(bgvVolume) : setting.BgvVolume;
            }

            var blinkElement = settingTag.Element(blinkElementName);
            if (blinkElement != null)
            {
                setting.BlinkOrders = new BlinkElementConverter().Convert(blinkElement);
                DebugTools.Logger.Add($"SceneSettingLoader : Blink order をロードしました。");
            }

            DebugTools.Logger.Add($"SceneSettingLoader : シーン設定ファイルを読み込みました。");
            DebugTools.Logger.Add($"{setting}");

            setting.ImageLocations = LoadImageLocations(settingTag);
            FixImageLocations(setting.ImageLocations, setting);

            return setting;
        }

        /// <summary>
        /// imageLocation 要素を子に持つ要素から ImageLocation オブジェクトのリストを生成します。
        /// imageLocation 要素の name 属性にはファイル名を入力します。
        /// このファイル名は拡張子を含むかどうかに関わらず、拡張子を除いたファイル名が採用されます。
        /// </summary>
        /// <param name="xml">imageLocation 要素を子にもつ XElement</param>
        /// <returns>XElement を読み込んで生成された ImageLocation オブジェクトのリスト</returns>
        public List<ImageLocation> LoadImageLocations(XElement xml)
        {
            var locations = new List<ImageLocation>();

            if (xml == null || !xml.Elements("imageLocation").Any())
            {
                return locations;
            }

            locations.AddRange(xml.Elements("imageLocation")
                .Where(locationTag => !string.IsNullOrWhiteSpace(XElementHelper.GetStringFromAttribute(locationTag, NameAttribute)))
                .Select(locationTag =>
                {
                    var rawName = XElementHelper.GetStringFromAttribute(locationTag, NameAttribute);
                    var fileNameWe = Path.GetFileNameWithoutExtension(new FileInfo(rawName).FullName);

                    return new ImageLocation
                    {
                        Name = fileNameWe,
                        X = XElementHelper.GetIntFromAttribute(locationTag, XAttribute),
                        Y = XElementHelper.GetIntFromAttribute(locationTag, YAttribute),
                        Width = XElementHelper.GetIntFromAttribute(locationTag, WidthAttribute),
                        Height = XElementHelper.GetIntFromAttribute(locationTag, HeightAttribute),
                    };
                }));

            return locations;
        }

        /// <summary>
        /// 画面左上を基準として記述された ImageLocation の座標を、画面中心を基準とした座標に修正します。
        /// </summary>
        /// <param name="locations"></param>
        /// <param name="setting"></param>
        private void FixImageLocations(List<ImageLocation> locations, SceneSetting setting)
        {
            foreach (var loc in locations.Where(l => l.Y != 0 || l.X != 0))
            {
                loc.X -= (setting.DefaultImageWidth - loc.Width) / 2;
                loc.Y -= (setting.DefaultImageHeight - loc.Height) / 2;
                loc.Y = -loc.Y;
            }
        }
    }
}