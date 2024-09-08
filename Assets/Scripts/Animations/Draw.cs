using System.Collections.Generic;
using ScenarioSceneParts;
using SceneContents;

namespace Animations
{
    public class Draw : IAnimation
    {
        private bool drew;

        // ReSharper disable once MemberCanBePrivate.Global / リフレクションでアクセスを行うため
        public string A { get; set; } = string.Empty;

        // ReSharper disable once MemberCanBePrivate.Global / リフレクションでアクセスを行うため
        public string B { get; set; } = string.Empty;

        // ReSharper disable once MemberCanBePrivate.Global / リフレクションでアクセスを行うため
        public string C { get; set; } = string.Empty;

        // ReSharper disable once MemberCanBePrivate.Global / リフレクションでアクセスを行うため
        public string D { get; set; } = string.Empty;

        // ReSharper disable once MemberCanBePrivate.Global / リフレクションでアクセスを行うため
        public int Wait { get; set; }

        public string AnimationName { get; } = "draw";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public int Interval { get; set; }

        public double Depth { get; set; }

        public string GroupName { get; set; } = string.Empty;

        public bool PlayOnce { get; set; }

        private ImageDrawer ImageDrawer { get; set; }

        public void Execute()
        {
            if (!IsWorking)
            {
                return;
            }

            ImageDrawer ??= ScenePartsProvider.GetImageDrawer(TargetLayerIndex);

            if (Delay-- > 0)
            {
                return;
            }

            if (Wait > 0 && drew)
            {
                Wait--;
                return;
            }

            if (!drew)
            {
                drew = true;
                var drawOrder = new ImageOrder()
                {
                    Names = { A, B, C, D, },
                    IsDrawOrder = true,
                    Depth = Depth,
                };

                var scenario = new Scenario() { DrawOrders = new List<ImageOrder>() { drawOrder, }, };
                ImageDrawer.SetScenario(scenario);
                ImageDrawer.Execute();
            }
            else
            {
                Stop();
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
            IsWorking = false;
        }
    }
}