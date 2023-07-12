using SceneContents;

namespace Animations
{
    public class AlphaChanger : IAnimation
    {
        private readonly bool canUpdateTarget;
        private float amount = 0.1f;
        private IDisplayObject target;

        public AlphaChanger(bool canUpdateTarget = false)
        {
            this.canUpdateTarget = canUpdateTarget;
        }

        public double Amount
        {
            set => amount = (float)value;
        }

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target
        {
            get => target;
            set
            {
                if (canUpdateTarget || target == null)
                {
                    target = value;
                }
            }
        }

        public ImageContainer TargetContainer
        {
            set => _ = value;
        }

        public int TargetLayerIndex { get; set; }

        public string AnimationName => nameof(AlphaChanger);

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; } = string.Empty;

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

            Target.Alpha += amount;

            if (Target.Alpha > 1.0f)
            {
                IsWorking = false;
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
            IsWorking = false;
            Target.Alpha = 1.0f;
        }
    }
}