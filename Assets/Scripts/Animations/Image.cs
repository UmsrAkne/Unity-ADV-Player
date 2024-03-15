using System.Collections.Generic;
using ScenarioSceneParts;
using SceneContents;

namespace Animations
{
    public class Image : IAnimation
    {
        public string AnimationName => "image";

        private bool drew;

        public int X { get; set; }

        public int Y { get; set; }

        // ReSharper disable once MemberCanBePrivate.Global / リフレクションでアクセスを行うため
        public double Scale { get; set; } = 1.0;

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

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public bool PlayOnce { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; } = string.Empty;
        
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