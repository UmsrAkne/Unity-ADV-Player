namespace SceneContents
{
    using UnityEngine;

    public interface ISound
    {
        AudioSource AudioSource { get; set; }

        double Volume { get; set; }

        float PanStereo { get; set; }

        bool IsPlaying { get; }

        /// <summary>
        /// このオブジェクトが利用可能であるかを取得します。
        /// 主に、ロード完了時に true になります。
        /// </summary>
        bool Available { get; set; }

        void Play();

        void Stop();

        float Delay { get; set; }
    }
}