using SceneContents;

namespace Animations
{
    public class ScaleChange : IAnimation
    {
        private double changeAmountPerFrame;
        private int frameCounter;
        private IDisplayObject target;

        public double To { get; set; }

        public int Duration { get; set; }

        public string AnimationName { get; } = "scaleChange";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target
        {
            get => target;
            set
            {
                if (frameCounter == 0)
                {
                    target = value;
                }
            }
        }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; } = string.Empty;

        public void Execute()
        {
            if (!IsWorking || Target == null)
            {
                return;
            }

            if (Delay-- > 0)
            {
                return;
            }

            CoreProcess();
        }

        public void Start()
        {
        }

        public void Stop()
        {
            IsWorking = false;
            changeAmountPerFrame = 0;
            Target.Scale = To;
            frameCounter = Duration + 1;
        }

        private void CoreProcess()
        {
            if (frameCounter == 0)
            {
                changeAmountPerFrame = (To - Target.Scale) / Duration;
            }

            Target.Scale += changeAmountPerFrame;
            frameCounter++;

            if (frameCounter >= Duration)
            {
                Stop();
            }
        }
    }
}