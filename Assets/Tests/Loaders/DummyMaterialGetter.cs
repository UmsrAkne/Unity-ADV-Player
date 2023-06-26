using System;
using System.Collections.Generic;
using Loaders;
using SceneContents;

namespace Tests.Loaders
{
    public class DummyMaterialGetter : IMaterialGetter
    {
        public List<string> Paths { get; } = new();

        public event EventHandler SoundLoadCompleted;

        public ISound GetSounds(string path)
        {
            throw new NotImplementedException();
        }

        public SpriteWrapper LoadImage(string path)
        {
            Paths.Add(path);
            return new SpriteWrapper();
        }
    }
}