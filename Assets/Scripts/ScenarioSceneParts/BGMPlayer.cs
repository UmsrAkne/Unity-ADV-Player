using Loaders;
using SceneContents;
using UnityEngine;

namespace ScenarioSceneParts
{
    public class BGMPlayer : IScenarioSceneParts
    {
        private AudioSource BGM { get; set; }

        private bool Playing { get; set; }

        private SceneSetting SceneSetting { get; set; }

        public bool NeedExecuteEveryFrame => false;

        public ExecutionPriority Priority => ExecutionPriority.Low;

        public void Execute()
        {
            if (!Playing)
            {
                BGM.loop = true;
                BGM.volume = SceneSetting.BGMVolume;
                BGM.Play();
                Playing = true;
            }
        }

        public void ExecuteEveryFrame()
        {
            // throw new System.NotImplementedException();
        }

        public void SetResource(Resource resource)
        {
            BGM = resource.BGMAudioSource;
            SceneSetting = resource.SceneSetting;
        }

        public void Reload(Resource resource)
        {
            // BGM の切り替えは行わないので、何の動作も行わない。
        }

        public void SetScenario(Scenario scenario)
        {
        }
    }
}