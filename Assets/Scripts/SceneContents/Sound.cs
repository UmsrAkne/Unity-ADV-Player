namespace SceneContents
{
    using System;
    using UnityEngine;

    public class Sound : ISound
    {
        private bool playing;
        private DateTime playStartedDateTime;

        public AudioSource AudioSource { get; set; }

        public double Volume
        {
            get => AudioSource.volume;
            set => AudioSource.volume = (float)value;
        }

        public bool IsPlaying
        {
            get
            {
                if (DateTime.Now - playStartedDateTime < TimeSpan.FromMilliseconds(150))
                {
                    return playing;
                }
                else
                {
                    return AudioSource.isPlaying;
                }
            }
        }

        public bool Available { get; set; }

        public void Play()
        {
            AudioSource.Play();
            playStartedDateTime = DateTime.Now;
            playing = true;
        }

        public void Stop()
        {
            AudioSource.Stop();
            playing = false;
        }
    }
}