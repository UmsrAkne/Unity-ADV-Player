using System;
using ScenarioSceneParts;
using SceneContents;

namespace Animations
{
    public class Blink : IAnimation
    {
        private int drawCounter;
        
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

        private ImageDrawer ImageDrawer { get; set; }

        private ImageOrder LastImageOrder { get; set; }

        private BlinkOrder CurrentOrder { get; set; }

        public void Execute()
        {
            if (!IsWorking)
            {
                return;
            }

            ImageDrawer ??= ScenePartsProvider.GetImageDrawer(TargetLayerIndex);

            if (--Interval >= 0)
            {
                return;
            }

            if (ImageDrawer.LastOrder != null && LastImageOrder != ImageDrawer.LastOrder)
            {
                LastImageOrder = ImageDrawer.LastOrder;
                const int eyeImageIndex = 1;
                CurrentOrder = Resource.GetBlinkOrderFromName(LastImageOrder.Names[eyeImageIndex]);
            }

            var imageOrder = new ImageOrder()
            {
                Names = { string.Empty, CurrentOrder.Names[0], string.Empty, string.Empty },
                IsExpressionOrder = true,
                IsDrawOrder = true,
            };

            ImageDrawer.DrawImage(imageOrder);
            drawCounter++;

            if (drawCounter++ > CurrentOrder.Names.Count * 2)
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