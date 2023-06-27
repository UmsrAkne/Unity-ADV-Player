namespace Loaders
{
    using System.Collections.Generic;
    using System.Xml.Linq;

    public class SceneSettingLoader
    {
        private readonly string bgmElementName = "bgm";
        private readonly string defaultSizeElementName = "defaultSize";
        private readonly string fileNameAttribute = "fileName";
        private readonly string heightAttribute = "height";
        private readonly string numberAttribute = "number";
        private readonly string volumeAttribute = "volume";

        private readonly string widthAttribute = "width";

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

            DebugTools.Logger.Add($"SceneSettingLoader : シーン設定ファイルを読み込みました。");
            DebugTools.Logger.Add($"{setting}");

            return setting;
        }
    }
}