using System;
using DebugTools;
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

        public RandomGen RandomGen { private get; set; } = new ();

        private ImageOrder LastImageOrder { get; set; }

        private BlinkOrder CurrentOrder { get; set; }

        public void Execute()
        {
            if (!IsWorking)
            {
                return;
            }

            if (--Interval > 0)
            {
                return;
            }

            ImageDrawer ??= ScenePartsProvider.GetImageDrawer(TargetLayerIndex);

            if (ImageDrawer.LastOrder != null && LastImageOrder != ImageDrawer.LastOrder)
            {
                LastImageOrder = ImageDrawer.LastOrder;
                CurrentOrder = Resource.GetBlinkOrderFromName(LastImageOrder.Names[EyeImageIndex]);
            }

            if (CurrentOrder == null)
            {
                return;
            }

            if (drawCounter < CurrentOrder.Names.Count)
            {
                ImageDrawer.DrawImage(new ImageOrder(CurrentOrder, drawCounter));
                drawCounter++;
            }
            else
            {
                drawCounter = 0;
                Interval = RandomGen.GetInt(20, 100);
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}