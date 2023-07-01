using System;
using System.Collections.Generic;
using System.IO;
using SceneContents;

namespace Loaders
{
    public class ImageLoader : IContentsLoader
    {
        public List<string> Log { get; set; } = new();

        public Resource Resource { get; set; }

        public TargetImageType TargetImageType { get; set; }

        public HashSet<string> UsingFileNames { get; set; } = new();

        public IPathListGen PathListGen { private get; set; } = new FilePathListGen();

        public IMaterialGetter MaterialLoader { get; set; } = new MaterialLoader();

        public event EventHandler LoadCompleted;

        public void Load(string targetDirectoryPath)
        {
            switch (TargetImageType)
            {
                case TargetImageType.EventCg:
                    targetDirectoryPath += $@"\images";
                    break;
                // case TargetImageType.Mask:
                //     targetDirectoryPath += $@"\{ResourcePath.SceneMaskImageDirectoryName}";
                //     break;
                // case TargetImageType.UiImage:
                //     targetDirectoryPath = ResourcePath.CommonUIDirectoryName;
                //     break;
            }

            if (!Directory.Exists(targetDirectoryPath))
            {
                Log.Add($"{targetDirectoryPath} が見つかりませんでした");
            }

            PathListGen.GetImageFilePaths(targetDirectoryPath).ForEach(path =>
            {
                var fileName = Path.GetFileName(path);
                var fileNameWe = Path.GetFileNameWithoutExtension(path);
                var pathIsUsingFile = UsingFileNames.Contains(fileName) || UsingFileNames.Contains(fileNameWe);

                if (pathIsUsingFile || TargetImageType != TargetImageType.EventCg)
                {
                    var containsName = Resource.ContainsImage(TargetImageType, fileName);
                    var containsNameWe = Resource.ContainsImage(TargetImageType, fileNameWe);

                    if (!containsName && !containsNameWe)
                    {
                        var spWrapper = MaterialLoader.LoadImage(path);
                        Resource.AddImages(TargetImageType, spWrapper, fileName);
                        Resource.AddImages(TargetImageType, spWrapper, fileNameWe);
                        DebugTools.Logger.Add($"ImageLoader : {path} をロードしました");
                    }
                }
            });

            DebugTools.Logger.Add($"ImageLoader : {targetDirectoryPath} から画像のロードを完了しました");
            
            // 上の LoadImage(path) が非同期的な処理だった場合、この時点ではロード完了していないかも
            LoadCompleted?.Invoke(this, EventArgs.Empty);
        }
    }

    public enum TargetImageType
    {
        EventCg,
        UiImage
    }
}