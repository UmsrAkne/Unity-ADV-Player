using System;
using System.Collections.Generic;
using System.Linq;
using Loaders;
using UnityEngine;

namespace SceneContents
{
    public class Resource : IResource
    {
        public SceneSetting SceneSetting { get; set; } = new SceneSetting();

        public List<string> Log { get; set; } = new List<string>();

        public List<Scenario> Scenarios { get; set; }

        public List<SpriteWrapper> Images { get; set; } = new();

        // public List<SpriteWrapper> MaskImages { get; set; }

        public Dictionary<string, SpriteWrapper> MaskImagesByName { get; set; }

        public AudioSource BGMAudioSource { get; set; }

        public Sprite MessageWindowImage { get; set; }

        public string SceneDirectoryPath { get; set; }

        public bool Used { get; set; }

        private Dictionary<string, SpriteWrapper> ImagesByName { get; set; } = new();

        private List<ISound> Voices { get; set; } = new();

        private Dictionary<string, ISound> VoicesByName { get; set; } = new();

        private List<ISound> BgVoices { get; set; } = new();

        private Dictionary<string, ISound> BgVoicesByName { get; set; } = new();

        private List<ISound> Ses { get; set; } = new();

        private Dictionary<string, ISound> SesByName { get; set; } = new();

        public ISound GetSound(TargetAudioType targetAudioType, string targetName)
        {
            return targetAudioType switch
            {
                TargetAudioType.Voice => VoicesByName[targetName],
                TargetAudioType.Se => SesByName[targetName],
                TargetAudioType.BgVoice => BgVoicesByName[targetName],
                _ => throw new NotImplementedException()
            };
        }

        public ISound GetSound(TargetAudioType targetAudioType, int index)
        {
            return targetAudioType switch
            {
                TargetAudioType.Voice => Voices[index],
                TargetAudioType.Se => Ses[index],
                TargetAudioType.BgVoice => BgVoices[index],
                _ => throw new NotImplementedException()
            };
        }

        public SpriteWrapper GetImage(TargetImageType imageType, string targetName)
        {
            return imageType switch
            {
                TargetImageType.EventCg => ImagesByName[targetName],
                TargetImageType.UiImage => throw new NotImplementedException(),
                _ => throw new NotImplementedException()
            };
        }

        public Scenario GetScenario(int index)
        {
            return Scenarios[index];
        }

        /// <summary>
        /// 指定した種類の画像リソースを、このリソースが既に持っているかを取得します。
        /// </summary>
        /// <param name="targetImageType">調査する画像リソースのタイプを入力します</param>
        /// <param name="name">画像ファイルの名前を入力します</param>
        /// <returns>指定のリソースを持っているか</returns>
        /// <exception cref="System.NotImplementedException">
        /// 調査対象として、TargetImageType.EventCg 以外の種類を指定した場合にスローされます
        /// </exception>
        public bool ContainsImage(TargetImageType targetImageType, string name)
        {
            return targetImageType switch
            {
                TargetImageType.EventCg => ImagesByName.ContainsKey(name),
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// 入力した画像リソースを、適切な場所に入力、保管します。
        /// </summary>
        /// <param name="targetImageType">入力する画像リソースのタイプです。これによって入力先を判定します。</param>
        /// <param name="spw">画像リソースです</param>
        /// <param name="fileName">画像リソースを保管する辞書のキーとなり文字列</param>
        /// <exception cref="System.NotImplementedException">
        /// リソースのタイプに TargetImageType.EventCg 以外の種類を指定した場合にスローされます
        /// </exception>
        public void AddImages(TargetImageType targetImageType, SpriteWrapper spw, string fileName)
        {
            switch (targetImageType)
            {
                case TargetImageType.EventCg:
                    if (!Images.Contains(spw))
                    {
                        Images.Add(spw);
                    }

                    ImagesByName.TryAdd(fileName, spw);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void AddSound(TargetAudioType audioType, ISound sound, string fileName)
        {
            switch (audioType)
            {
                case TargetAudioType.Voice:
                    if (Voices.Contains(sound))
                    {
                        Voices.Add(sound);
                    }

                    VoicesByName.TryAdd(fileName, sound);
                    break;

                case TargetAudioType.BgVoice:
                    if (BgVoices.Contains(sound))
                    {
                        BgVoices.Add(sound);
                    }

                    BgVoicesByName.TryAdd(fileName, sound);
                    break;

                case TargetAudioType.Se:
                    if (Ses.Contains(sound))
                    {
                        Ses.Add(sound);
                    }

                    SesByName.TryAdd(fileName, sound);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 指定したタイプのオーディオが、指定したキーで辞書に登録されているかを取得します。
        /// </summary>
        /// <param name="targetAudioType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool ContainsSoundKey(TargetAudioType targetAudioType, string key)
        {
            return targetAudioType switch
            {
                TargetAudioType.Voice => VoicesByName.ContainsKey(key),
                TargetAudioType.Se => SesByName.ContainsKey(key),
                TargetAudioType.BgVoice => BgVoicesByName.ContainsKey(key),
                _ => throw new NotImplementedException()
            };
        }

        public ImageLocation GetImageLocationFromName(string name)
        {
            return SceneSetting?.ImageLocations.FirstOrDefault(loc => loc.Name == name);
        }
    }
}