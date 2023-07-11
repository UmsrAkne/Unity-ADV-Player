using System.Collections.Generic;
using ScenarioSceneParts;
using SceneContents;

namespace Animations
{
    public class Image : IAnimation
    {
        public string AnimationName => "image";
        
        private bool drawed;

        public int X { get; set; }

        public int Y { get; set; }

        public double Scale { get; set; } = 1.0;

        public string A { get; set; } = string.Empty;

        public string B { get; set; } = string.Empty;

        public string C { get; set; } = string.Empty;

        public string D { get; set; } = string.Empty;

        public int Wait { get; set; }

        public static ImageDrawer ImageDrawer { private get; set; }

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
                System.Diagnostics.Debug.WriteLine($"aaaaaaaaaaaaaaaaaaaa(Image : 67)");
                drawed = true;
                var imageOrder = new ImageOrder()
                {
                    X = X,
                    Y = Y,
                    Names = { A, B, C, D },
                    Scale = Scale,
                };

                var scenario = new Scenario() { ImageOrders = new List<ImageOrder>() { imageOrder } };
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