using System;
using System.Collections.Generic;
using System.Linq;
using SceneContents;

namespace ScenarioSceneParts
{
    public class VoicePlayer : IScenarioSceneParts
    {
        private ISound currentVoice;
        private SoundOrder nextOrder;
        private bool playRequire;

        public List<ISound> Voices { get; set; }

        public Dictionary<string, ISound> VoicesByName { get; set; }

        public int Channel { get; set; }

        public bool NeedExecuteEveryFrame => false;

        public ExecutionPriority Priority => ExecutionPriority.Low;

        public void Execute()
        {
            if (!playRequire)
            {
                return;
            }

            if (currentVoice != null)
            {
                currentVoice.Stop();
            }

            currentVoice = nextOrder.Index > 0 ? Voices[nextOrder.Index] : VoicesByName[nextOrder.FileName];

            currentVoice.Play();
            SoundStart?.Invoke(this, EventArgs.Empty);
            nextOrder = null;
            playRequire = false;
        }

        public void ExecuteEveryFrame()
        {
            if (currentVoice is { IsPlaying: false })
            {
                SoundComplete?.Invoke(this, EventArgs.Empty);
                currentVoice = null;
            }
        }

        public void SetResource(Resource resource)
        {
            Voices = resource.Voices;
            VoicesByName = resource.VoicesByName;
        }

        public void SetScenario(Scenario scenario)
        {
            if (!scenario.VoiceOrders.Any())
            {
                return;
            }

            nextOrder = scenario.VoiceOrders.FirstOrDefault(order => order.Channel == Channel);

            // nextOrder.Index == 0 は無視する。
            // [0] は未使用番号。インデックス 0 はデフォルト値であり、未設定の状態を表す。
            if (nextOrder != null)
            {
                if (nextOrder.Index > 0 || !string.IsNullOrWhiteSpace(nextOrder.FileName))
                {
                    playRequire = true;
                }
            }
        }

        public event EventHandler SoundComplete;

        public event EventHandler SoundStart;
    }
}