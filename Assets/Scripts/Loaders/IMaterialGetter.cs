using System;
using System.Drawing;
using SceneContents;

namespace Loaders
{
    public interface IMaterialGetter
    {
        event EventHandler SoundLoadCompleted;

        ISound GetSound(string path);

        ISound GetSound(string path, ISound sound);

        SpriteWrapper LoadImage(string path);

        SpriteWrapper LoadImage(string path, float scale);

        public Size GetImageSizeFrom(string path);
    }
}