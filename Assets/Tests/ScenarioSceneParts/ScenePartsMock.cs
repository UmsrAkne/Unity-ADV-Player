using ScenarioSceneParts;
using SceneContents;

namespace Tests.ScenarioSceneParts
{
    public class ScenePartsMock : IScenarioSceneParts
    {
        public bool NeedExecuteEveryFrame { get; } = false;

        public ExecutionPriority Priority { get; } = ExecutionPriority.Low;

        public int ExecutionCounter { get; set; }

        public int ExecutionEveryFrameCounter { get; set; }

        public void Execute()
        {
            ExecutionCounter++;
        }

        public void ExecuteEveryFrame()
        {
            ExecutionEveryFrameCounter++;
        }

        public void SetScenario(Scenario scenario)
        {
        }

        public void SetResource(Resource resource)
        {
        }
    }
}