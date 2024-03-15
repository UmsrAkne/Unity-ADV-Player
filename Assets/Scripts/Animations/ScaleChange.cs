using System;
using SceneContents;

namespace Animations
{
    public class ScaleChange : IAnimation
    {
        private double changeAmount;
        private int frameCounter;
        private IDisplayObject target;
        private readonly double halfPi = Math.PI / 2;
        private double piPerDur;

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

        public bool PlayOnce { get; set; }

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
            Target.Scale = To;
            frameCounter = Duration + 1;
        }

        private void CoreProcess()
        {
            Target.Scale += (To - Target.Scale) * GetRatio(frameCounter);
            frameCounter++;

            if (frameCounter >= Duration)
            {
                Stop();
            }
        }

        private double GetRatio(int count)
        {
            if (piPerDur == 0 && Duration != 0)
            {
                piPerDur = Math.PI / Duration;
            }

            var v = Math.Sin(halfPi + piPerDur * count);
            return Math.Abs((v - 1) / 2);
        }
    }
}