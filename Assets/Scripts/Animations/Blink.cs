using System;
using ScenarioSceneParts;
using SceneContents;

namespace Animations
{
    public class Blink : IAnimation
    {
        private int drawCounter;
        private const int EyeImageIndex = 1;

        public string AnimationName => "blink";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; }

        public IResource Resource { get; set; }

        public IDrawer ImageDrawer { private get; set; }

        private ImageOrder LastImageOrder { get; set; }

        private BlinkOrder CurrentOrder { get; set; }

        public void Execute()
        {
            if (!IsWorking)
            {
                return;
            }

            if (--Interval >= 0)
            {
                return;
            }

            ImageDrawer ??= ScenePartsProvider.GetImageDrawer(TargetLayerIndex);

            if (ImageDrawer.LastOrder != null && LastImageOrder != ImageDrawer.LastOrder)
            {
                LastImageOrder = ImageDrawer.LastOrder;
                CurrentOrder = Resource.GetBlinkOrderFromName(LastImageOrder.Names[EyeImageIndex]);
            }

            if (drawCounter < CurrentOrder.Names.Count)
            {
                ImageDrawer.DrawImage(new ImageOrder(CurrentOrder, drawCounter));
                drawCounter++;
            }
            else
            {
                drawCounter = 0;
                Interval = 100;
            }
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}