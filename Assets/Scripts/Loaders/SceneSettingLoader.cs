using System.IO;
using System.Linq;
using Loaders.XmlElementConverters;
using SceneContents;

namespace Loaders
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class SceneSettingLoader
    {
        private readonly string bgmElementName = "bgm";
        private readonly string seElementName = "se";
        private readonly string voiceElementName = "voice";
        private readonly string defaultSizeElementName = "defaultSize";
        private readonly string fileNameAttribute = "fileName";
        private readonly string heightAttribute = "height";

        private readonly string nameAttribute = "name";
        private readonly string numberAttribute = "number";
        private readonly string volumeAttribute = "volume";

        private readonly string widthAttribute = "width";
        private readonly string xAttribute = "x";
        private readonly string yAttribute = "y";
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

            if (settingTag.Element(defaultSizeElementName) != null)
            {
                var w = settingTag.Element(defaultSizeElementName)?.Attribute(widthAttribute)?.Value;
                setting.DefaultImageWidth = w != null ? int.Parse(w) : setting.DefaultImageWidth;

                var h = settingTag.Element(defaultSizeElementName)?.Attribute(heightAttribute)?.Value;
                setting.DefaultImageHeight = h != null ? int.Parse(h) : setting.DefaultImageHeight;
            }

            var bgmElement = settingTag.Element(bgmElementName);
            if (bgmElement != null)
            {
                var bgmNumber = bgmElement.Attribute(numberAttribute)?.Value;
                setting.BGMNumber = bgmNumber != null ? int.Parse(bgmNumber) : setting.BGMNumber;

                var bgmName = bgmElement.Attribute(fileNameAttribute);
                setting.BGMFileName = bgmName != null ? bgmName.Value : string.Empty;

                var bgmVolume = bgmElement.Attribute(volumeAttribute)?.Value;
                setting.BGMVolume = bgmVolume != null ? float.Parse(bgmVolume) : setting.BGMVolume;
            }

            // se 要素が記述されていれば、効果音のボリュームを設定する。
            var seElement = settingTag.Element(seElementName);
            if (seElement != null)
            {
                var seVolume = seElement.Attribute(volumeAttribute)?.Value;
                setting.SeVolume = seVolume != null ? float.Parse(seVolume) : setting.SeVolume;
            }

            // voice 要素が記述されていれば、ボイスのボリュームを設定する。
            var voiceElement = settingTag.Element(voiceElementName);
            if (voiceElement != null)
            {
                var voiceVolume = voiceElement.Attribute(volumeAttribute)?.Value;
                setting.VoiceVolume = voiceVolume != null ? float.Parse(voiceVolume) : setting.VoiceVolume;
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
                .Where(locationTag => !string.IsNullOrWhiteSpace(XElementHelper.GetStringFromAttribute(locationTag, nameAttribute)))
                .Select(locationTag =>
                {
                    var rawName = XElementHelper.GetStringFromAttribute(locationTag, nameAttribute);
                    var fileNameWe = Path.GetFileNameWithoutExtension(new FileInfo(rawName).FullName);

                    return new ImageLocation
                    {
                        Name = fileNameWe,
                        X = XElementHelper.GetIntFromAttribute(locationTag, xAttribute),
                        Y = XElementHelper.GetIntFromAttribute(locationTag, yAttribute),
                        Width = XElementHelper.GetIntFromAttribute(locationTag, widthAttribute),
                        Height = XElementHelper.GetIntFromAttribute(locationTag, heightAttribute),
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