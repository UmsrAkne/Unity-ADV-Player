using System;
using SceneContents;

namespace Loaders
{
    public interface IMaterialGetter
    {
        event EventHandler SoundLoadCompleted;

        ISound GetSounds(string path);

        SpriteWrapper LoadImage(string path);
    }
}