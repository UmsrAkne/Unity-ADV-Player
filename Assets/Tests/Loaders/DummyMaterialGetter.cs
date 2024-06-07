using System;
using System.Collections.Generic;
using System.Drawing;
using Loaders;
using SceneContents;

namespace Tests.Loaders
{
    public class DummyMaterialGetter : IMaterialGetter
    {
        public List<string> Paths { get; } = new();

        public event EventHandler SoundLoadCompleted;

        public ISound GetSound(string path)
        {
            Paths.Add(path);
            return new DummySound();
        }

        public ISound GetSound(string path, ISound sound)
        {
            Paths.Add(path);
            sound.Available = true;
            return sound;
        }

        public SpriteWrapper LoadImage(string path)
        {
            Paths.Add(path);
            return new SpriteWrapper();
        }

        public SpriteWrapper LoadImage(string path, float scale)
        {
            throw new NotImplementedException();
        }

        public Size GetImageSizeFrom(string path)
        {
            throw new NotImplementedException();
        }
    }
}