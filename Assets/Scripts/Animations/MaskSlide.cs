using SceneContents;

namespace Animations
{
    public class MaskSlide : IAnimation
    {
        private SlideCore core;
        private int executeCounter;
        private bool isInitialExecute;

        public string AnimationName { get; }

        public bool IsWorking { get; private set; }

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public bool PlayOnce { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; }

        public int Duration { get; set; }

        public int Degree { get; set; }

        public int Distance { get; set; }

        public void Execute()
        {
            if (Target == null || !IsWorking)
            {
                return;
            }

            if (Delay-- > 0)
            {
                return;
            }

            if (isInitialExecute)
            {
                isInitialExecute = false;
                Initialize();
            }

            core.Execute();
            executeCounter++;

            if (!core.IsWorking)
            {
                if (RepeatCount > 0)
                {
                    Delay = Interval;
                    RepeatCount--;
                    Degree += 180;
                    Initialize();
                }
                else
                {
                    Stop();
                }
            }
        }

        public void Start()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            IsWorking = false;
            Duration = 0;
            Distance = 0;
            Target = null;
        }

        private void Initialize()
        {
            core = new SlideCore()
            {
                Target = Target,
                Distance = Distance,
                Degree = Degree,
                Duration = Duration,
            };

            core.Start();
        }
    }
}