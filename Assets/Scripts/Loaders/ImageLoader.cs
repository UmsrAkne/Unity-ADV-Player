using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SceneContents;
using UnityEngine;

namespace Loaders
{
    public class ImageLoader : IContentsLoader
    {
        public List<string> Log { get; set; } = new();

        public Resource Resource { get; set; }

        public TargetImageType TargetImageType { get; set; }

        public HashSet<string> UsingFileNames { get; set; } = new();

        private List<SpriteWrapper> Sprites { get; set; } = new();

        private Dictionary<string, SpriteWrapper> SpriteDictionary { get; set; } = new();

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
                return;
            }

            GetImageFileLPaths(targetDirectoryPath).ForEach(path =>
            {
                var fileName = Path.GetFileName(path);
                var fileNameWe = Path.GetFileNameWithoutExtension(path);
                var pathIsUsingFile = UsingFileNames.Contains(fileName) || UsingFileNames.Contains(fileNameWe);

                if (pathIsUsingFile || TargetImageType != TargetImageType.EventCg)
                {
                    var spWrapper = LoadImage(path);
                    Sprites.Add(spWrapper);
                    SpriteDictionary.Add(fileName, spWrapper);
                    SpriteDictionary.Add(fileNameWe, spWrapper);
                }
            });

            // 上の LoadImage(path) が非同期的な処理だった場合、この時点ではロード完了していないかも
            LoadCompleted?.Invoke(this, EventArgs.Empty);

            switch (TargetImageType)
            {
                case TargetImageType.EventCg:
                    Resource.Images = Sprites;
                    Resource.ImagesByName = SpriteDictionary;
                    break;
                case TargetImageType.UiImage:
                    break;
            }
        }

        public SpriteWrapper LoadImage(string targetFilePath)
        {
            var size = GetImageSize(targetFilePath);
            var sp = Sprite.Create(ReadTexture(targetFilePath, (int)size.x, (int)size.y),
                new Rect(0, 0, (int)size.x, (int)size.y), new Vector2(0.5f, 0.5f), 1);
            return new SpriteWrapper { Sprite = sp, Width = (int)size.x, Height = (int)size.y };
        }

        private Texture2D ReadTexture(string path, int width, int height)
        {
            var bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(width, height);
            texture.LoadImage(bytes);
            texture.filterMode = FilterMode.Point;
            return texture;
        }

        private List<string> GetImageFileLPaths(string targetDirectoryPath)
        {
            var allFilePaths = new List<string>(Directory.GetFiles(targetDirectoryPath));
            return allFilePaths.Where(f => Path.GetExtension(f) == ".png" || Path.GetExtension(f) == ".jpg").ToList();
        }

        private Vector2 GetImageSize(string path)
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            fs.Seek(16, SeekOrigin.Begin);
            var buf = new byte[8];
            var _ = fs.Read(buf, 0, 8);
            fs.Dispose();
            var width = ((uint)buf[0] << 24) | ((uint)buf[1] << 16) | ((uint)buf[2] << 8) | buf[3];
            var height = ((uint)buf[4] << 24) | ((uint)buf[5] << 16) | ((uint)buf[6] << 8) | buf[7];
            return new Vector2(width, height);
        }
    }

    public enum TargetImageType
    {
        EventCg,
        UiImage
    }
}