using System;
using System.IO;
using SceneContents;
using UnityEngine;

namespace Loaders
{
    public class MaterialLoader : IMaterialGetter
    {
        private int loadCounter;

        public event EventHandler SoundLoadCompleted;

        public ISound GetSound(string path)
        {
            loadCounter++;
            var soundLoader = new GameObject().AddComponent<SoundLoader>();
            soundLoader.Load(path);
            soundLoader.LoadCompleted += (_, _) =>
            {
                if (--loadCounter <= 0)
                {
                    SoundLoadCompleted?.Invoke(this, EventArgs.Empty);
                }
            };

            return soundLoader.Sound;
        }

        public SpriteWrapper LoadImage(string path)
        {
            var size = GetImageSize(path);
            var sp = Sprite.Create(ReadTexture(path, (int)size.x, (int)size.y),
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
}