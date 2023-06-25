using SceneContents;

namespace Loaders
{
    public interface IMaterialGetter
    {
        SpriteWrapper LoadImage(string path);
    }
}