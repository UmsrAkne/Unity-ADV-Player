using UnityEngine;

namespace SceneContents
{
    public class DummySound : ISound
    {
        public AudioSource AudioSource { get; set; }

        public double Volume { get; set; } = 1.0;

        public float PanStereo { get; set; }

        public bool IsPlaying { get; set; }

        public bool Available { get; set; }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public float Delay { get; set; }
    }
}