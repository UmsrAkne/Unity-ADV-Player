using System;
using SceneContents;

namespace Animations
{
    public class Flash : IAnimation
    {
        private IDisplayObject effectImageSet;
        private int frameCounter;
        private int intervalCounter;
        private ImageContainer targetContainer;

        public IDisplayObject EffectImageSet
        {
            get => effectImageSet;
            set => effectImageSet = value;
        }

        public int Duration { get; set; } = 40;

        public double Alpha { get; set; } = 1.0f;

        public string AnimationName => "flash";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { private get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; } = 1;

        public int Delay { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; } = string.Empty;

        public ImageContainer TargetContainer { get => targetContainer; set => targetContainer ??= value; }

        public void Execute()
        {
            if (!IsWorking)
            {
                return;
            }

            if (Delay > 0)
            {
                Delay--;
                return;
            }

            if (intervalCounter > 0)
            {
                intervalCounter--;
                return;
            }

            frameCounter++;
            effectImageSet.Alpha = GetAlpha();

            if (frameCounter >= Duration)
            {
                RepeatCount--;
                if (RepeatCount > 0)
                {
                    frameCounter = 0;
                    intervalCounter = Interval;
                }
                else
                {
                    Stop();
                }
            }
        }

        public void Start()
        {
            // effectImageSet = TargetContainer.EffectImageSet;
        }

        public void Stop()
        {
            effectImageSet.Alpha = 0;
            IsWorking = false;
        }

        private float GetAlpha()
        {
            var rad = frameCounter * (180.0 / Duration) * (Math.PI / 180);
            return (float)Math.Abs(Math.Sin(rad)) * (float)Alpha;
        }
    }
}