using Loaders;
using SceneContents;

namespace Tests.Loaders
{
    public class DummyMaterialGetter : IMaterialGetter
    {
        public SpriteWrapper LoadImage(string path)
        {
            return new SpriteWrapper();
        }
    }
}