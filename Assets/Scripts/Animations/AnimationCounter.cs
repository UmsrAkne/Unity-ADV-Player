namespace Animations
{
    public class AnimationCounter
    {
        public int RepeatCount { get; set; }

        public int Delay { get; set; }

        public bool IsWorking { get; private set; } = true;

        public int Interval { get; set; }

        public int Duration { get; set; }

        public bool CanProcess { get; set; }

        /// <summary>
        /// CountUp() を実行した回数です。
        /// この値は動作中にリセットされる場合があるため、必ずしも実行した合計回数が取得できるわけではありません
        /// 合計回数が必要な場合は TotalExecutionCounter を使ってください。
        /// </summary>
        public int ExecutionCounter { get; private set; }

        /// <summary>
        /// CountUp() を実行した合計回数です。この値はいかなる場合もリセットされません。
        /// </summary>
        public int TotalExecutionCounter { get; private set; }

        public void CountUp()
        {
            TotalExecutionCounter++;

            if (Delay > 0)
            {
                Delay--;
                CanProcess = false;
                return;
            }

            ExecutionCounter++;
            CanProcess = true;

            if (Duration > 0 && ExecutionCounter >= Duration)
            {
                ExecutionCounter = 0;

                if (RepeatCount > 0)
                {
                    RepeatCount--;
                    Delay = Interval;
                    return;
                }

                IsWorking = false;
                CanProcess = false;
            }
        }
    }
}