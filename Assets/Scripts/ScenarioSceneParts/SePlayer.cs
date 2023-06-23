using System.Collections.Generic;
using System.Linq;
using SceneContents;

namespace ScenarioSceneParts
{
    public class SePlayer : IScenarioSceneParts
    {
        private StopOrder stopOrder;

        private List<ISound> Ses { get; set; }

        private Dictionary<string, ISound> SeByName { get; set; }

        private SoundOrder CurrentOrder { get; set; }

        private ISound PlayingSound { get; set; }

        public bool NeedExecuteEveryFrame => false;

        public ExecutionPriority Priority => ExecutionPriority.Low;

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

            if (Ses.Count > CurrentOrder.Index && CurrentOrder.Index != 0)
            {
                PlayingSound = Ses[CurrentOrder.Index];
            }

            if (!string.IsNullOrWhiteSpace(CurrentOrder.FileName))
            {
                PlayingSound = SeByName[CurrentOrder.FileName];
            }

            if (PlayingSound == null)
            {
                return;
            }

            if (CurrentOrder.RepeatCount > 0)
            {
                PlayingSound.AudioSource.loop = true;
            }

            PlayingSound.Play();
            CurrentOrder = null;
        }

        public void ExecuteEveryFrame()
        {
        }

        public void SetResource(Resource resource)
        {
            Ses = resource.Ses;
            SeByName = resource.SesByName;
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