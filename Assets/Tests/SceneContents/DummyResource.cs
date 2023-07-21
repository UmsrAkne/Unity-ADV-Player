using System.Collections.Generic;
using Loaders;
using SceneContents;

namespace Tests.SceneContents
{
    public class DummyResource : IResource
    {
        private int imageGotCounter;
        
        public List<SpriteWrapper> SpriteWrappers { get; set; }

        public List<string> RequestedImageNames { get; set; } = new ();
        
        public ISound GetSound(TargetAudioType targetAudioType, string targetName)
        {
            throw new System.NotImplementedException();
        }

        public ISound GetSound(TargetAudioType targetAudioType, int index)
        {
            throw new System.NotImplementedException();
        }

        public SpriteWrapper GetImage(TargetImageType imageType, string targetName)
        {
            RequestedImageNames.Add(targetName);
            return SpriteWrappers[imageGotCounter++];
        }

        public ImageLocation GetImageLocationFromName(string name)
        {
            return new ImageLocation();
        }

        public BlinkOrder GetBlinkOrderFromName(string name)
        {
            throw new System.NotImplementedException();
        }

        public Scenario GetScenario(int index)
        {
            throw new System.NotImplementedException();
        }

        public bool Used { get; set; }
    }
}