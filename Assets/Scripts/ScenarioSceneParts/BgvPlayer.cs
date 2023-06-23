using System;
using System.Collections.Generic;
using System.Linq;
using Loaders;
using SceneContents;

namespace ScenarioSceneParts
{
    public class BgvPlayer : IScenarioSceneParts
    {
        private readonly TargetAudioType audioType = TargetAudioType.BgVoice;
        private bool mute = true;
        private ISound playingVoice;

        private List<ISound> voices;
        private double volume = 1.0;

        public BgvPlayer(VoicePlayer voicePlayer)
        {
            VoicePlayer = voicePlayer;
            VoicePlayer.SoundComplete += VoicePlayerCompleteEventHandler;
            VoicePlayer.SoundStart += VoicePlayerStartEventHandler;
        }

        private VoicePlayer VoicePlayer { get; }

        private double Volume
        {
            get => volume;
            set
            {
                if (value is >= 0 and <= 1.0)
                {
                    if (playingVoice != null)
                    {
                        playingVoice.Volume = value;
                    }

                    volume = value;
                }
            }
        }

        private bool Playing => voices != null && voices.Any();

        private IResource Resource { get; set; }

        public bool NeedExecuteEveryFrame => true;

        public ExecutionPriority Priority => ExecutionPriority.Low;

        public void Execute()
        {
        }

        public void ExecuteEveryFrame()
        {
            if (!Playing)
            {
                return;
            }

            if (!playingVoice.IsPlaying)
            {
                // 再生中の音声が終了したら、リスト上で次の音声があれば再生、なければシャッフルして先頭から再生する。
                var currentIndex = voices.IndexOf(playingVoice);
                if (currentIndex == voices.Count - 1)
                {
                    voices = voices.OrderBy(_ => Guid.NewGuid()).ToList();
                    Play(voices.First());
                }
                else
                {
                    Play(voices[currentIndex + 1]);
                }
            }

            if (mute)
            {
                return;
            }

            Volume += 0.03;
        }

        public void SetScenario(Scenario scenario)
        {
            var bgvOrder = scenario.BgvOrders.FirstOrDefault(vo => vo.Channel == VoicePlayer.Channel);
            if (bgvOrder != null)
            {
                playingVoice?.Stop();
                voices = bgvOrder.FileNames.Select(n => Resource.GetSound(audioType, n))
                    .OrderBy(_ => Guid.NewGuid())
                    .ToList();

                Volume = 0;
                Play(voices.FirstOrDefault());
            }

            scenario.StopOrders.ForEach(order =>
            {
                if (order.Target is StoppableSceneParts.BackgroundVoice or StoppableSceneParts.Bgv)
                {
                    if (order.Channel == VoicePlayer.Channel)
                    {
                        mute = true;
                        Volume = 0;
                        voices = new List<ISound>();
                        playingVoice?.Stop();
                    }
                }
            });
        }

        public void SetResource(Resource resource)
        {
            Resource = resource;
        }

        private void VoicePlayerStartEventHandler(object sender, EventArgs e)
        {
            mute = true;
            Volume = 0;
        }

        private void VoicePlayerCompleteEventHandler(object sender, EventArgs e)
        {
            mute = false;
        }

        private void Play(ISound v)
        {
            playingVoice = v;
            v.Play();
            v.Volume = Volume;
        }
    }
}