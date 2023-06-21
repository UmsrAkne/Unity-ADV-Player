using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SceneContents;
using UnityEngine;
using UnityEngine.Networking;

namespace Loaders
{
    public class VoiceLoader : MonoBehaviour, IContentsLoader
    {
        private int loadCompleteCounter;

        public List<ISound> AudioSources { get; private set; } = new();

        public Dictionary<string, ISound> AudioSourcesByName { get; private set; } = new();

        private List<AudioClip> AudioClips { get; set; }

        public HashSet<string> UsingVoiceFileNames { get; set; } = new();

        public HashSet<int> UsingVoiceNumbers { get; set; } = new();

        public HashSet<string> UsingBgvFileNames { get; set; } = new();

        public TargetAudioType TargetAudioType { get; set; }

        public event EventHandler LoadCompleted;

        public List<string> Log { get; set; } = new List<string>();

        public Resource Resource { get; set; }

        public void Load(string targetDirectoryPath)
        {
            switch (TargetAudioType)
            {
                case TargetAudioType.Voice:
                    // ReSharper disable once StringLiteralTypo
                    targetDirectoryPath += $@"\voices";
                    break;
                case TargetAudioType.BgVoice:
                    targetDirectoryPath += $@"\bgVoices";
                    break;
                case TargetAudioType.Se:
                    targetDirectoryPath = @"\commonResource\ses";
                    break;
            }

            if (!Directory.Exists(targetDirectoryPath))
            {
                Log.Add($"{targetDirectoryPath} が見つかりませんでした");
                return;
            }

            var audioPaths = GetSoundFilePaths(targetDirectoryPath);

            if (audioPaths.Count == 0 || !NeedLoad())
            {
                LoadCompleted?.Invoke(this, EventArgs.Empty);
            }

            PartLoadCompleted += (_, _) =>
            {
                loadCompleteCounter++;
                if (loadCompleteCounter < AudioSources.Count)
                {
                    return;
                }

                AudioSources.Insert(0, null);

                switch (TargetAudioType)
                {
                    case TargetAudioType.Voice:
                        Resource.Voices = AudioSources;
                        Resource.VoicesByName = AudioSourcesByName;
                        break;
                    case TargetAudioType.BgVoice:
                        Resource.BGVoices = AudioSources;
                        Resource.BGVoicesByName = AudioSourcesByName;
                        break;
                    case TargetAudioType.Se:
                        Resource.Ses = AudioSources;
                        Resource.SesByName = AudioSourcesByName;
                        break;
                }

                LoadCompleted?.Invoke(this, EventArgs.Empty);
            };

            if (Resource is { Used: true })
            {
                switch (TargetAudioType)
                {
                    case TargetAudioType.Voice:
                        AudioSources = Resource.Voices;
                        AudioSourcesByName = Resource.VoicesByName;
                        break;
                    case TargetAudioType.BgVoice:
                        AudioSources = Resource.BGVoices;
                        AudioSourcesByName = Resource.BGVoicesByName;
                        break;
                    case TargetAudioType.Se:
                        AudioSources = Resource.Ses;
                        AudioSourcesByName = Resource.SesByName;
                        break;
                }
            }
            else
            {
                // 初回ロード時のみ、AudioSources に Sound を挿入する
                foreach (var s in audioPaths)
                {
                    var sound = new Sound() { AudioSource = new GameObject().AddComponent<AudioSource>() };
                    AudioSources.Add(sound);
                    AudioSourcesByName.Add(Path.GetFileName(s), sound);
                    AudioSourcesByName.Add(Path.GetFileNameWithoutExtension(s), sound);
                }
            }

            AudioClips = Enumerable.Repeat<AudioClip>(null, audioPaths.Count).ToList();

            for (var i = 0; i < audioPaths.Count; i++)
            {
                if (Resource is { Used: true } && AudioSources[i].Available)
                {
                    loadCompleteCounter++;
                    continue;
                }

                var path = audioPaths[i];

                if (TargetAudioType == TargetAudioType.Voice)
                {
                    var isUsingNumber = UsingVoiceNumbers.Contains(i);
                    var isUsingFileName = UsingVoiceFileNames.Contains(Path.GetFileName(path))
                                          || UsingVoiceFileNames.Contains(Path.GetFileNameWithoutExtension(path));

                    if (!isUsingNumber && !isUsingFileName)
                    {
                        loadCompleteCounter++;
                        continue;
                    }
                }

                if (TargetAudioType == TargetAudioType.BgVoice)
                {
                    var isUsingFileName = UsingBgvFileNames.Contains(Path.GetFileName(path))
                                          || UsingBgvFileNames.Contains(Path.GetFileNameWithoutExtension(path));

                    if (!isUsingFileName)
                    {
                        loadCompleteCounter++;
                        continue;
                    }
                }

                StartCoroutine(LoadAudio(path, i));
            }
        }

        private event EventHandler PartLoadCompleted;

        private List<string> GetSoundFilePaths(string targetDirectoryPath)
        {
            var allFilePaths = new List<string>(Directory.GetFiles(targetDirectoryPath));

            // まず ogg ファイルを where で抜き出し、Select でパスを絶対パスに変換する。
            return allFilePaths.Where(f => Path.GetExtension(f) == ".ogg")
                .Select(p => Path.GetFullPath(p)).ToList();
        }

        private IEnumerator LoadAudio(string path, int index)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                yield break;
            }

            using UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.OGGVORBIS);
            req.SendWebRequest();

            while (!req.isDone)
            {
                yield return null;
            }

            AudioClips[index] = DownloadHandlerAudioClip.GetContent(req);

            if (AudioClips[index].loadState != AudioDataLoadState.Loaded)
            {
                yield break;
            }

            AudioSources[index].AudioSource.clip = AudioClips[index];
            AudioSources[index].Available = true;
            PartLoadCompleted?.Invoke(this, EventArgs.Empty);
        }

        private bool NeedLoad()
        {
            switch (TargetAudioType)
            {
                case TargetAudioType.BgVoice:
                    return UsingBgvFileNames.Count != 0;
                case TargetAudioType.Voice:
                    return UsingVoiceFileNames.Count != 0 || UsingVoiceNumbers.Count != 0;
                default:
                    return true;
            }
        }
    }

    public enum TargetAudioType
    {
        Voice,
        BgVoice,
        Se,
    }
}