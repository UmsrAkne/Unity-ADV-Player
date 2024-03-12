using System.Linq;
using Loaders;
using SceneContents;

namespace ScenarioSceneParts
{
    public class SePlayer : IScenarioSceneParts
    {
        private readonly TargetAudioType audioType = TargetAudioType.Se;
        private StopOrder stopOrder;

        private SoundOrder CurrentOrder { get; set; }

        private ISound PlayingSound { get; set; }

        public bool NeedExecuteEveryFrame => false;

        public ExecutionPriority Priority => ExecutionPriority.Low;

        private IResource Resource { get; set; }

        private float BaseVolume { get; set; }

        public void Execute()
        {
            if (stopOrder != null)
            {
                PlayingSound?.Stop();
                stopOrder = null;
            }

            if (CurrentOrder == null)
            {
                return;
            }

            PlayingSound?.Stop();

            if (CurrentOrder.Index != 0)
            {
                PlayingSound = Resource.GetSound(audioType, CurrentOrder.Index);
            }

            if (!string.IsNullOrWhiteSpace(CurrentOrder.FileName))
            {
                PlayingSound = Resource.GetSound(audioType, CurrentOrder.FileName);
            }

            if (PlayingSound == null)
            {
                return;
            }

            if (CurrentOrder.RepeatCount > 0)
            {
                PlayingSound.AudioSource.loop = true;
            }

            PlayingSound.Volume = BaseVolume;
            PlayingSound.Play();
            CurrentOrder = null;
        }

        public void ExecuteEveryFrame()
        {
        }

        public void SetResource(Resource resource)
        {
            Resource = resource;
            BaseVolume = resource.SceneSetting.SeVolume;
        }

        public void Reload(Resource resource)
        {
            PlayingSound?.Stop();
            PlayingSound = null;
            CurrentOrder = null;
            stopOrder = null;
            Resource = resource;
        }

        public void SetScenario(Scenario scenario)
        {
            scenario.StopOrders.ForEach(order =>
            {
                if (order.Target == StoppableSceneParts.Se)
                {
                    stopOrder = order;
                }
            });

            if (scenario.SeOrders.Count == 0)
            {
                return;
            }

            CurrentOrder = scenario.SeOrders.FirstOrDefault();
        }
    }
}