using ScenarioSceneParts;
using UnityEngine;
using UnityEngine.UI;

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

        public Text TextField { get; set; } = GameObject.Find("TextField").GetComponent<Text>();

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

        /// <summary>
        /// メッセージウィンドウと、その上にあるテキストオブジェクトを移動させます。
        /// 移動は、引数に入力した値を現在のウィンドウ、テキストオブジェクトの座標に加算して実現します。
        /// </summary>
        /// <param name="x">x の移動量</param>
        /// <param name="y">y の移動量</param>
        public void MoveMessageWindow(float x, float y)
        {
            var r = TextField.rectTransform.anchoredPosition;
            TextField.rectTransform.anchoredPosition = new Vector2(r.x + x, r.y + y);

            var mPos = MessageWindow.transform.position;
            MessageWindow.transform.position = new Vector3(mPos.x + x, mPos.y + y);
        }
    }
}