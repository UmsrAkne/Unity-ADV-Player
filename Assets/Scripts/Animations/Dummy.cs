using SceneContents;

namespace Animations
{
    /// <summary>
    ///     ダミーアニメーションです。
    ///     Delay, Duration プロパティのみ動作します。
    ///     このアニメーションは実行時にカウントダウンを行いますが、外部に影響を及ぼす動作は何も行いません。
    ///     アニメーション間のインターバルや、テストに使用します。
    /// </summary>
    public class Dummy : IAnimation
    {
        public string AnimationName => "dummy";

        public bool IsWorking { get; private set; } = true;

        public IDisplayObject Target { get; set; }

        public ImageContainer TargetContainer { get; set; }

        public int TargetLayerIndex { get; set; }

        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public bool PlayOnce { get; set; }

        public int Interval { get; set; }

        public int Duration { get; set; }

        public string GroupName { get; set; }

        public void Execute()
        {
            if (--Delay >= 0)
            {
                return;
            }

            if (--Duration <= 0)
            {
                Stop();
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
            Duration = 0;
            IsWorking = false;
        }
    }
}