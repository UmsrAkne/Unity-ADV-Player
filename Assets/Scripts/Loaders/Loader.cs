using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using SceneContents;
using UnityEngine;

namespace Loaders
{
    public class Loader
    {
        public event EventHandler TextLoadCompleted;
        public event EventHandler MediaLoadCompleted;
        public event EventHandler LoadCompleted;

        private int loadCompleteCount;
        private readonly TextLoader textLoader = new TextLoader();
        private readonly ImageLoader imageLoader = new ImageLoader{TargetImageType = TargetImageType.EventCg};
        // private readonly ImageLoader maskLoader = new ImageLoader{TargetImageType = TargetImageType.mask};
        private readonly BGMLoader bgmLoader = new GameObject().AddComponent<BGMLoader>();
        private readonly VoiceLoader voiceLoader = new();
        private readonly VoiceLoader bgvLoader = new();
        private readonly VoiceLoader seLoader = new();
        private readonly SceneSettingLoader sceneSettingLoader = new SceneSettingLoader();
        private bool textLoadComplete;
        private bool mediaLoadComplete;

        public Resource Resource { get; private set; } = new ();

        public void LoadMedias(string path)
        {
            // ReSharper disable once StringLiteralTypo
            var settingXMLPath = $@"{path}\texts\setting.xml";

            if (File.Exists(settingXMLPath))
            {
                DebugTools.Logger.Add($"Loader : {settingXMLPath} の読み込みを開始します。");
                Resource.SceneSetting = sceneSettingLoader.LoadSetting(XDocument.Parse(File.ReadAllText(settingXMLPath)));
            }
            else
            {
                DebugTools.Logger.Add($"Loader : {settingXMLPath} が見つかりませんでした。");
                DebugTools.Logger.Add($"Loader : setting.xml を読み込めませんでした");
            }

            voiceLoader.TargetAudioType = TargetAudioType.Voice;
            bgvLoader.TargetAudioType = TargetAudioType.BgVoice;
            seLoader.TargetAudioType = TargetAudioType.Se;
            bgmLoader.BGMNumber = Resource.SceneSetting.BGMNumber;
            bgmLoader.BGMFileName = Resource.SceneSetting.BGMFileName;

            imageLoader.UsingFileNames = textLoader.UsingImageFileNames;

            voiceLoader.UsingVoiceFileNames = textLoader.UsingVoiceFileNames;
            voiceLoader.UsingVoiceNumbers = textLoader.UsingVoiceNumbers;

            bgvLoader.UsingBgvFileNames = textLoader.UsingBgvFileNames;

            var loaders = new List<IContentsLoader>()
            {
                // bgm のロードを最後にすると、読み込む前にシーンが始まる？
                bgmLoader,
                imageLoader,
                // maskLoader,
                voiceLoader,
                bgvLoader,
                seLoader,
            };

            loaders.ForEach(l =>
            {
                l.LoadCompleted += (_, e) =>
                {
                    loadCompleteCount++;

                    if (loadCompleteCount >= loaders.Count)
                    {
                        MediaLoadCompleted?.Invoke(this,e);
                    }
                };

                l.Resource = Resource;
                l.Load(path);
            });

            // Resource.MessageWindowImage = new ImageLoader().LoadImage($@"{ResourcePath.CommonUIDirectoryName}\msgWindowImage.png").Sprite;
            Resource.SceneDirectoryPath = path;
        }

        public void LoadTexts(string targetDirectoryPath)
        {
            textLoader.Resource = Resource;
            textLoader.Load(targetDirectoryPath);
            TextLoadCompleted?.Invoke(this, EventArgs.Empty);
        }

        public void Recycle(Resource res)
        {
            Resource = res;
        }

        /// <summary>
        /// テキスト、メディアを全てロードします。
        /// ロード完了後、 LoadCompleted が送出されます。
        /// </summary>
        /// <param name="targetDirectoryPath">対象のシナリオが格納されているディレクトリのパス</param>
        public void Load(string targetDirectoryPath)
        {
            DebugTools.Logger.Add($"Loader : {targetDirectoryPath} からリソースのロードを開始します");

            TextLoadCompleted += OnTextLoadCompleted;
            LoadTexts(targetDirectoryPath);

            MediaLoadCompleted += OnMediaLoadCompleted;
            LoadMedias(targetDirectoryPath);
        }

        private void OnTextLoadCompleted(object sender, EventArgs e)
        {
            textLoadComplete = true;
            if (textLoadComplete && mediaLoadComplete)
            {
                LoadCompleted?.Invoke(this, EventArgs.Empty);
                Resource.Used = true;
            }

            TextLoadCompleted -= OnTextLoadCompleted;
            DebugTools.Logger.Add($"Loader : テキストのロードが完了しました");
        }

        private void OnMediaLoadCompleted(object sender, EventArgs e)
        {
            mediaLoadComplete = true;
            if (textLoadComplete && mediaLoadComplete)
            {
                LoadCompleted?.Invoke(this, EventArgs.Empty);
                Resource.Used = true;
            }

            MediaLoadCompleted -= OnMediaLoadCompleted;
            DebugTools.Logger.Add($"Loader : メディアのロードが完了しました。");
        }
    }
}