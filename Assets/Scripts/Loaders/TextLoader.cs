using System;
using Loaders.XmlElementConverters;

namespace Loaders
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using SceneContents;

    public class TextLoader
    {
        private readonly string ignoreElement = "ignore";
        private readonly string strAttribute = "str";
        private readonly string stringAttribute = "string";
        private readonly string textAttribute = "text";

        public List<Scenario> Scenarios { get; set; }

        public List<string> Log { get; private set; } = new();

        public HashSet<string> UsingImageFileNames { get; private set; } = new();

        public HashSet<string> UsingVoiceFileNames { get; private set; } = new();

        public HashSet<int> UsingVoiceNumbers { get; private set; } = new();

        public HashSet<string> UsingBgvFileNames { get; private set; } = new();

        public Resource Resource { get; set; }

        public event EventHandler LoadCompleted;

        private List<IXMLElementConverter> Converters { get; set; } = new();

        public void Load(string targetPath)
        {
            // ReSharper disable once StringLiteralTypo
            targetPath += @"\texts\scenario.xml";

            if (!File.Exists(targetPath))
            {
                DebugTools.Logger.Add($"TextLoader : {targetPath} が見つかりませんでした");
                Scenarios = new List<Scenario>() { new() { Text = "シナリオの読み込みに失敗しました。" } };
                return;
            }

            XDocument xml;

            try
            {
                xml = XDocument.Parse(File.ReadAllText(targetPath));
            }
            catch (XmlException e)
            {
                Scenarios = new List<Scenario>() { new() { Text = "シナリオの読み込みに失敗しました。" } };
                DebugTools.Logger.Add($"TextLoader : Scenario.xmlのパースに失敗しました。");
                DebugTools.Logger.Add($"{Environment.NewLine}{e.Message}");
                return;
            }

            Converters.Add(new ChapterElementConverter());
            Converters.Add(new ImageElementConverter());
            Converters.Add(new DrawElementConverter());
            Converters.Add(new VoiceElementConverter());
            Converters.Add(new SeElementConverter());
            Converters.Add(new BackgroundVoiceElementConverter());
            Converters.Add(new StopElementConverter());
            Converters.Add(new AnimeElementConverter());

            var scenarioIndex = 0;
            var scenarioList =
                xml.Root?.Descendants()
                    .Where(x => x.Name.LocalName == "scn" || x.Name.LocalName == "scenario")
                    .Where(x => x.Element(ignoreElement) == null).ToList();

            if (scenarioList?.FirstOrDefault(x => x.Element("start") != null) != null)
            {
                scenarioList = scenarioList.SkipWhile(x => x.Element("start") == null).ToList();
            }

            if (scenarioList != null)
            {
                Scenarios = scenarioList.Select(x =>
                {
                    var scenario = new Scenario() { Index = ++scenarioIndex };

                    if (x.Element(textAttribute)?.Attribute(strAttribute) != null)
                    {
                        scenario.Text = GetChildAttributeValue(x, textAttribute, strAttribute);
                    }

                    if (x.Element(textAttribute)?.Attribute(stringAttribute) != null)
                    {
                        scenario.Text = GetChildAttributeValue(x, textAttribute, stringAttribute);
                    }

                    Converters.ForEach(c => c.Convert(x, scenario));
                    return scenario;
                }).ToList();

                // 使用している画像のファイル名を抽出する
                UsingImageFileNames = GetUsingImageFileNames(scenarioList);

                // 使用している音声ファイル名と番号を抽出する
                UsingVoiceFileNames = GetUsingVoiceFileNames(scenarioList);
                UsingVoiceNumbers = GetUsingVoiceNumbers(scenarioList);
                UsingBgvFileNames = GetUsingBgvFileNames(scenarioList);
            }

            // Converters.ForEach(c => Log.AddRange(c.Log));

            Resource.Scenarios = Scenarios;
            DebugTools.Logger.Add($"TextLoader : Scenarios.Count = {Scenarios.Count}");
            DebugTools.Logger.Add($"TextLoader : Scenario.xml のパースが完了しました。");
            LoadCompleted?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 入力された scenario 要素の中で使用されている全ての画像ファイル名のリストを取得します
        /// </summary>
        /// <param name="xElements">scenario をルート要素とする xElements の List</param>
        /// <returns>scenario 要素の中で指定されている重複のない画像ファイル名一覧</returns>
        private HashSet<string> GetUsingImageFileNames(IEnumerable<XElement> xElements)
        {
            var targetElements = xElements.Descendants()
                .Where(x => x.Name.LocalName == "image" || x.Name.LocalName == "draw" || x.Name.LocalName == "anime");

            var usingImgFileNames = new HashSet<string>();

            foreach (var x in targetElements)
            {
                var a = x.Attribute("a");
                var b = x.Attribute("b");
                var c = x.Attribute("c");
                var d = x.Attribute("d");

                if (a?.Value != null)
                {
                    usingImgFileNames.Add(a.Value);
                }

                if (b?.Value != null)
                {
                    usingImgFileNames.Add(b.Value);
                }

                if (c?.Value != null)
                {
                    usingImgFileNames.Add(c.Value);
                }

                if (d?.Value != null)
                {
                    usingImgFileNames.Add(d.Value);
                }
            }

            return usingImgFileNames;
        }

        /// <summary>
        /// 入力された scenario 要素の中の voice 要素で指定されているファイル名の一覧を取得します。
        /// </summary>
        /// <param name="xElements">scenario をルート要素とする xElements の List</param>
        /// <returns>voice 要素の中で指定されている重複のないファイル名一覧</returns>
        private HashSet<string> GetUsingVoiceFileNames(IEnumerable<XElement> xElements)
        {
            var targetElements = xElements.Descendants("voice");

            var usingVcFileNames = new HashSet<string>();
            foreach (var fileNameAtt in targetElements
                         .Select(v => v.Attribute("fileName"))
                         .Where(fileNameAtt => fileNameAtt != null && !string.IsNullOrWhiteSpace(fileNameAtt.Value)))
            {
                usingVcFileNames.Add(fileNameAtt.Value);
            }

            return usingVcFileNames;
        }

        /// <summary>
        /// 入力された scenario 要素の中の voice 要素で指定されているインデックスの一覧を取得します。
        /// </summary>
        /// <param name="xElements">scenario をルート要素とする xElements の List</param>
        /// <returns>voice 要素の中で指定されている重複のないインデックスのリスト</returns>
        private HashSet<int> GetUsingVoiceNumbers(IEnumerable<XElement> xElements)
        {
            var targetElements = xElements.Descendants("voice");

            var usingNumbers = new HashSet<int>();
            foreach (var numberAtt in targetElements
                         .Select(v => v.Attribute("number"))
                         .Where(n => n != null && int.Parse(n.Value) != 0))
            {
                usingNumbers.Add(int.Parse(numberAtt.Value));
            }

            return usingNumbers;
        }

        /// <summary>
        /// 入力された scenario 要素の中の bgv 要素で指定されているファイル名の一覧を取得します。
        /// </summary>
        /// <param name="xElements">scenario をルート要素とする xElements の List</param>
        /// <returns>bgv 要素の中で指定されている重複のないファイル名一覧</returns>
        private HashSet<string> GetUsingBgvFileNames(IEnumerable<XElement> xElements)
        {
            var targetElements = xElements
                .Elements()
                .Where(x => x.Name == "backgroundVoice" || x.Name == "bgv");

            var usingFileNames = new HashSet<string>();
            foreach (var element in targetElements)
            {
                var namesAtt = element.Attribute("names");
                if (namesAtt == null)
                {
                    continue;
                }

                foreach (var s in namesAtt.Value.Replace(" ", "").Split(','))
                {
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        usingFileNames.Add(s);
                    }
                }
            }

            return usingFileNames;
        }

        private string GetChildAttributeValue(XElement xElement, string childElementName, string attributeName)
        {
            var childElement = xElement.Element(childElementName);
            var att = childElement?.Attribute(attributeName);
            return att != null ? att.Value : string.Empty;
        }
    }
}