using Loaders;

namespace SceneContents
{
    public interface IResource
    {
        ISound GetSound(TargetAudioType targetAudioType, string targetName);

        ISound GetSound(TargetAudioType targetAudioType, int index);

        SpriteWrapper GetImage(TargetImageType imageType, string targetName);

        public ImageLocation GetImageLocationFromName(string name);

        Scenario GetScenario(int index);

        bool Used { get; set; }
    }
}