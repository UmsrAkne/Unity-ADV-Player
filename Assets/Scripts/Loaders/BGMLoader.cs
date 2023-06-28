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
    public class BGMLoader : MonoBehaviour, IContentsLoader
    {
        // ReSharper disable once StringLiteralTypo
        private readonly string commonBgmDirectoryName = @"commonResource/bgms";
        private AudioClip ac;
        private bool isFirstLoad = true;

        public AudioSource AudioSource { get; private set; }

        public int BGMNumber { private get; set; }

        public string BGMFileName { get; set; } = string.Empty;

        public event EventHandler LoadCompleted;

        public List<string> Log { get; } = new();

        public Resource Resource { get; set; }

        /// <summary>
        /// BGM をロードします。インターフェースの都合でパスを引数に取りますが、入力値に関わらず　commonResource/bgm/ が参照されます。
        /// </summary>
        /// <param name="targetDirectoryPath"></param>
        public void Load(string targetDirectoryPath)
        {
            if (!isFirstLoad)
            {
                DebugTools.Logger.Add($"BGMLoader : 初回ロードではないため、 BGM のロードを中断しました。");
                LoadCompleted?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (!Directory.Exists(commonBgmDirectoryName))
            {
                Log.Add($"{commonBgmDirectoryName} が見つかりませんでした");
                AudioSource = new GameObject().AddComponent<AudioSource>();
                return;
            }

            AudioSource = new GameObject().AddComponent<AudioSource>();
            StartCoroutine(LoadAudio(GetSoundFilePath(commonBgmDirectoryName)));
        }

        private string GetSoundFilePath(string targetDirectoryPath)
        {
            var allFilePaths = new List<string>(Directory.GetFiles(targetDirectoryPath))
                .Where(p => Path.GetExtension(p) == ".ogg")
                .Select(Path.GetFullPath)
                .ToList();

            // ファイル名で指定が入っている場合は、番号による指定よりも優先する
            if (!string.IsNullOrWhiteSpace(BGMFileName))
            {
                var path = allFilePaths.FirstOrDefault(
                    f => Path.GetFileName(f) == BGMFileName || Path.GetFileNameWithoutExtension(f) == BGMFileName);

                return path ?? string.Empty;
            }

            if (allFilePaths.Count >= BGMNumber)
            {
                return allFilePaths[BGMNumber];
            }

            return allFilePaths.FirstOrDefault() ?? string.Empty;
        }

        private IEnumerator LoadAudio(string path)
        {
            if (AudioSource == null || string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                yield break;
            }

            using UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("file://" + path, AudioType.OGGVORBIS);
            req.SendWebRequest();

            while (!req.isDone)
            {
                yield return null;
            }

            ac = DownloadHandlerAudioClip.GetContent(req);

            if (ac.loadState != AudioDataLoadState.Loaded)
            {
                yield break;
            }

            AudioSource.clip = ac;
            Resource.BGMAudioSource = AudioSource;
            isFirstLoad = false;
            DebugTools.Logger.Add($"BGMLoader : {path} のロードが完了しました。");
            LoadCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}