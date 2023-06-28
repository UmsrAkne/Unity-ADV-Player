using System;
using System.Collections.Generic;
using System.IO;
using SceneContents;

namespace Loaders
{
    public class VoiceLoader : IContentsLoader
    {
        private int loadCompleteCounter;

        public IPathListGen PathListGen { private get; set; } = new FilePathListGen();

        public IMaterialGetter MaterialLoader { private get; set; } = new MaterialLoader();

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
                    targetDirectoryPath += $@"\bgvs";
                    break;
                case TargetAudioType.Se:
                    targetDirectoryPath = @"commonResource\ses";
                    break;
            }

            if (!Directory.Exists(targetDirectoryPath))
            {
                DebugTools.Logger.Add($"VoiceLoader : {targetDirectoryPath} が見つかりませんでした。");
            }

            var audioPaths = PathListGen.GetSoundFilePaths(targetDirectoryPath);

            if (audioPaths.Count == 0 || !NeedLoad())
            {
                LoadCompleted?.Invoke(this, EventArgs.Empty);
            }

            MaterialLoader.SoundLoadCompleted += (_, _) =>
            {
                DebugTools.Logger.Add($"VoiceLoader : {TargetAudioType} のロードが完了しました。");
                LoadCompleted?.Invoke(this, EventArgs.Empty);
            };

            foreach (var s in audioPaths)
            {
                var soundFileName = Path.GetFileName(s);
                var soundFileNameWe = Path.GetFileNameWithoutExtension(s);

                var loaded = Resource.ContainsSoundKey(TargetAudioType, soundFileName);

                // loaded == true の場合は既に空のサウンドオブジェクトが入っているので、それを使用する。
                // そうでない場合は GetSound(string) で新しいサウンドオブジェクトを生成する。
                var sound = loaded
                    ? MaterialLoader.GetSound(s, Resource.GetSound(TargetAudioType, soundFileName))
                    : MaterialLoader.GetSound(s);

                Resource.AddSound(TargetAudioType, sound, soundFileName);
                Resource.AddSound(TargetAudioType, sound, soundFileNameWe);
                DebugTools.Logger.Add($"VoiceLoader : {s} のロードを開始しました。");
            }
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