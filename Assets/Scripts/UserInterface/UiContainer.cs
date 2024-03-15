using ScenarioSceneParts;
using UnityEngine;

namespace UserInterface
{
    public class UiContainer
    {
        private bool found;

        public GameObject Canvas { get; } = GameObject.Find(nameof(Canvas));
        
        public GameObject OverBlack { get; } = GameObject.Find(nameof(OverBlack));

        public GameObject OverWhite { get; } = GameObject.Find(nameof(OverWhite));

        public GameObject LeftFrame { get; } = GameObject.Find(nameof(LeftFrame));

        public GameObject RightFrame { get; } = GameObject.Find(nameof(RightFrame));

        public GameObject MessageWindow { get; } = GameObject.Find(nameof(MessageWindow));

        public BGMPlayer BGMPlayer { get; set; }

        /// <summary>
        /// 入力された画面の幅に合わせて、フレームの位置を調節します。
        /// </summary>
        /// <param name="width">画面の幅を入力します。</param>
        public void SetScreenWidth(int width)
        {
            const int normalScreenWidth = 1280;
            var horizontalFramePos = (width - normalScreenWidth) / 2;
            var leftPos = LeftFrame.transform.position;
            leftPos = new Vector2(leftPos.x - horizontalFramePos, leftPos.y);
            LeftFrame.transform.position = leftPos;

            var rightPos = RightFrame.transform.position;
            rightPos = new Vector2(rightPos.x + horizontalFramePos, rightPos.y);
            RightFrame.transform.position = rightPos;
        }

        /// <summary>
        /// MessageWindow の透明度を設定します。
        /// </summary>
        /// <param name="value">入力値は 0 - 1.0 の範囲で入力します。</param>
        public void SetMsgWindowOpacity(float value)
        {
            var comp = MessageWindow.GetComponent<SpriteRenderer>();
            if (comp != null)
            {
                comp.color = new Color(1f, 1f, 1f, value);
            }
        }
    }
}