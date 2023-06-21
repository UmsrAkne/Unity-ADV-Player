namespace SceneContents
{
    using UnityEngine;

    public interface ISound
    {
        AudioSource AudioSource { get; set; }

        double Volume { get; set; }

        bool IsPlaying { get; }

        void Play();

        void Stop();
    }
}