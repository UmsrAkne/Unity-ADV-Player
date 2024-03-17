using System;
using SceneContents;
using UnityEngine;

namespace Animations
{
    public class AlphaChanger : IAnimation
    {
        private readonly bool canUpdateTarget;
        private float amount = 0.1f;
        private IDisplayObject target;
        private float pp;
        private int frameCounter = 1;
        private int duration;

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

        public bool PlayOnce { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; } = string.Empty;

        public int Duration
        {
            get => duration;
            set
            {
                if (value != 0)
                {
                    // pp は、 Math.Cos に対して、 Duration の回数だけ入力すれば -1 >> +1 まで値が変化するような値
                    // Duration が入力された時点で算出可能なので、ここに記述する。
                    pp = (float)(Math.PI) / value;
                }

                duration = value;
            }
        }

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

            if (Duration <= 0)
            {
                Target.Alpha += amount;
            }
            else
            {
                Target.Alpha = (float)((Math.Cos(Math.PI + pp * frameCounter) + 1.0) / 2.0);
                frameCounter++;
            }

            if (Target.Alpha >= 0.999f)
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