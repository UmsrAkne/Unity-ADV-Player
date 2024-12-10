using UnityEngine;

namespace Utils
{
    public class AudioMixerVolumeConverter
    {
        public static readonly float MinVolumeDb = -80f; // ミュート時のdB
        public static readonly float MaxVolumeDb = 6f;  // 2.0倍（元音量の2倍）のdB

        /// <summary>
        /// 線形スケールをdBスケールに変換します。
        /// </summary>
        /// <param name="linear">線形スケールの値（0.0～2.0）を入力します。<br/>
        /// 値の意味はそれぞれ次の通りです。(0, 1.0, 2.0) - (無音, 本来の音量, 本来の二倍の音量)
        /// </param>
        /// <returns>dBスケールの値</returns>
        public static float ConvertLinearToDecibel(float linear)
        {
            if (linear <= 0)
            {
                return MinVolumeDb; // 0以下はミュート扱い
            }

            if (linear >= 2.0)
            {
                return MaxVolumeDb;
            }

            return Mathf.Log10(linear) * 20; // デシベル計算式
        }

        /// <summary>
        /// dBスケールを線形スケールに変換します。
        /// </summary>
        /// <param name="db">dBスケールの値</param>
        /// <returns>線形スケールの値</returns>
        public static float ConvertDecibelToLinear(float db)
        {
            if (db <= MinVolumeDb)
            {
                return 0f; // ミュート状態
            }

            return Mathf.Pow(10, db / 20f);
        }
    }
}