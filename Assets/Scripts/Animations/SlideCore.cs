using System;
using SceneContents;

namespace Animations
{
    public class SlideCore : IAnimation
    {
        private double beforeDistance;
        private double endPos;
        private double radian;
        private double startPos;
        private double totalLength;
        private double x;
        private double y;

        public int Duration { get; set; }

        public int Distance { get; set; }

        public int Degree { get; set; }

        public int ExecuteCounter { get; private set; }

        /// <summary>
        /// 移動時に使うロジックの中で使用する Math.Sin グラフの左右部分をそれぞれ指定した範囲だけ切り取ります。
        /// この値を大きくすると、速度変化の勾配が急になります。
        /// 値は 0 - 10 の間での使用を想定。デフォルトは 5
        /// </summary>
        public int SideCutting { get; set; } = 5;

        public string AnimationName => "slideCoreB";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public int Interval { get; set; }

        public string GroupName { get; set; }

        public void Execute()
        {
            if (!IsWorking)
            {
                return;
            }

            if (ExecuteCounter == 0)
            {
                startPos = GetPositionRatio(SideCutting);
                endPos = GetPositionRatio(90.0 - SideCutting);
                totalLength = endPos - startPos;
                radian = Degree * (Math.PI / 180);
            }

            ExecuteCounter++;

            var d = Distance * Core(ExecuteCounter);

            var beforeX = x;
            var beforeY = y;

            x = Math.Cos(radian) * d;
            y = Math.Sin(radian) * d;

            Target.X += (float)(x - beforeX);
            Target.Y += (float)(y - beforeY);

            if (ExecuteCounter >= Duration)
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
            Duration = 0;
            Distance = 0;
        }

        public double GetPositionRatio(double counter)
        {
            var deg = (counter - 45.0) * 2.0;

            // Math.Sin((counter - 45.0) * 2.0) の計算で、 counter をインクリメントすると -1 --> 1 が返る
            var sin = Math.Sin(deg * (Math.PI / 180));

            // sin の値は -1...1 となる。これを 0...1 に変換する計算を行っている。
            return (sin + 1.0) / 2.0;
        }

        /// <summary>
        /// 0 - 1.0 の値を返却します。
        /// </summary>
        /// <param name="counter">0 から Duration までの値を入力します。</param>
        /// <returns>counter の値が 0　に近いほど 0 に近い値が出力され、Duration に近いほど 1.0 に近い値が出力されます</returns>
        private double Core(int counter)
        {
            var cnt = (90.0 - SideCutting * 2) / Duration;

            // 入力する値は sideCutting で下駄を履かせた counter に cnt をかける。
            // ここでの入力値は常に sideCutting から (90度 - sideCutting) の値の間の値となる
            var r = GetPositionRatio(SideCutting + counter * cnt);

            // 先程の値からスタート地点の値を引く。すると counter == 0 のとき、 r == 0
            // r の値は、常に (0 >= r <= totalLength)
            // totalLength に対する r の割合を返却する。
            return (r - startPos) / totalLength;
        }
    }
}