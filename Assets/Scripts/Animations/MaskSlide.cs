using SceneContents;

namespace Animations
{
    public class MaskSlide : IAnimation
    {
        private SlideCore core;
        private bool isInitialExecute = true;

        public string AnimationName => "maskSlide";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public bool PlayOnce { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; }

        // ReSharper disable once MemberCanBePrivate.Global
        // リフレクションでアクセスするために公開する。
        public int Duration { get; set; }

        // ReSharper disable once MemberCanBePrivate.Global
        // リフレクションでアクセスするために公開する。
        public int Degree { get; set; }

        // ReSharper disable once MemberCanBePrivate.Global
        // リフレクションでアクセスするために公開する。
        public int Distance { get; set; }

        public void Execute()
        {
            if (!IsWorking)
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
            var targetProvider = new EffectLayerProvider();
            core = new SlideCore()
            {
                Target = targetProvider.GetMask(TargetLayerIndex),
                Distance = Distance,
                Degree = Degree,
                Duration = Duration,
            };

            core.Start();
        }
    }
}