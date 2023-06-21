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

        private int loadCompleteCount;
        private readonly TextLoader textLoader = new TextLoader();
        private readonly ImageLoader imageLoader = new ImageLoader{TargetImageType = TargetImageType.EventCg};
        // private readonly ImageLoader maskLoader = new ImageLoader{TargetImageType = TargetImageType.mask};
        private readonly BGMLoader bgmLoader = new GameObject().AddComponent<BGMLoader>();
        private readonly VoiceLoader voiceLoader = new GameObject().AddComponent<VoiceLoader>();
        private readonly VoiceLoader bgvLoader = new GameObject().AddComponent<VoiceLoader>();
        private readonly VoiceLoader seLoader = new GameObject().AddComponent<VoiceLoader>();
        private readonly SceneSettingLoader sceneSettingLoader = new SceneSettingLoader();

        public Resource Resource { get; } = new Resource();

        public void LoadMedias(string path)
        {
            // ReSharper disable once StringLiteralTypo
            var settingXMLPath = $@"{path}\tests\setting.xml";

            if (File.Exists(settingXMLPath))
            {
                Resource.SceneSetting = sceneSettingLoader.LoadSetting(XDocument.Parse(File.ReadAllText(settingXMLPath)));
            }
            else
            {
                Resource.Log.Add("setting.xml を読み込めませんでした");
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
                imageLoader,
                // maskLoader,
                voiceLoader,
                bgvLoader,
                seLoader,
                bgmLoader,
            };

            loaders.ForEach(l =>
            {
                l.LoadCompleted += (sender, e) =>
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
    }
}