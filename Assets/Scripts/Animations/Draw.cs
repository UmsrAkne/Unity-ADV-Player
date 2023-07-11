using System.Collections.Generic;
using ScenarioSceneParts;
using SceneContents;

namespace Animations
{
    public class Draw : IAnimation
    {
        private bool drawed;

        public string A { get; set; } = string.Empty;

        public string B { get; set; } = string.Empty;

        public string C { get; set; } = string.Empty;

        public string D { get; set; } = string.Empty;

        public int Wait { get; set; }

        public static ImageDrawer ImageDrawer { private get; set; }

        public string AnimationName { get; } = "draw";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; } = string.Empty;

        public void Execute()
        {
            if (ImageDrawer == null || !IsWorking)
            {
                return;
            }

            if (Delay-- > 0)
            {
                return;
            }

            if (Wait > 0 && drawed)
            {
                Wait--;
                return;
            }

            if (!drawed)
            {
                drawed = true;
                var drawOrder = new ImageOrder()
                {
                    Names = { A, B, C, D },
                    IsDrawOrder = true,
                };

                var scenario = new Scenario() { DrawOrders = new List<ImageOrder>() { drawOrder } };
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