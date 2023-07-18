using System;
using ScenarioSceneParts;
using SceneContents;

namespace Animations
{
    public class Blink : IAnimation
    {
        public string AnimationName => "blink";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; }

        private ImageDrawer ImageDrawer { get; set; }

        public void Execute()
        {
            if (!IsWorking)
            {
                return;
            }

            ImageDrawer ??= ScenePartsProvider.GetImageDrawer(TargetLayerIndex);

            if (--Interval <= 0)
            {
                Interval = 100;
                // ImageDrawer.DrawImage();
            }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}