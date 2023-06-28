using System;
using SceneContents;

namespace Loaders
{
    public interface IMaterialGetter
    {
        event EventHandler SoundLoadCompleted;

        ISound GetSound(string path);

        ISound GetSound(string path, ISound sound);

        SpriteWrapper LoadImage(string path);
    }
}