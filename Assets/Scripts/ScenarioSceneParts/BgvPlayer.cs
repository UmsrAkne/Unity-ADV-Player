using System;
using System.Collections.Generic;
using System.Linq;
using Loaders;
using SceneContents;
using UnityEngine.Audio;
using Utils;

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

        public VoicePlayer VoicePlayer { get; }

        public double BaseVolume { get; set; } = 1.0f;

        private double Volume
        {
            get => volume;
            set
            {
                if (value >= 0 && value <= BaseVolume)
                {
                    var vol = AudioMixerVolumeConverter.ConvertLinearToDecibel((float)value);
                    AudioMixer.SetFloat("bgvMixVol", vol);
                    volume = value;
                }
            }
        }

        private bool Playing => voices != null && voices.Any();

        private IResource Resource { get; set; }

        public bool NeedExecuteEveryFrame => true;

        public AudioMixer AudioMixer { private get; set; }

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
                var v = currentIndex == voices.Count - 1
                    ? voices.OrderBy(_ => Guid.NewGuid()).ToList().First()
                    : voices[currentIndex + 1];

                v.AudioSource.outputAudioMixerGroup = AudioMixer.FindMatchingGroups("bgv").First();
                Play(v);
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

                foreach (var v in voices)
                {
                    v.PanStereo = bgvOrder.PanStereo;
                }

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
            BaseVolume = resource.SceneSetting.BgvVolume;
        }

        public void Reload(Resource resource)
        {
            mute = true;
            playingVoice = null;
            volume = 1.0;
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