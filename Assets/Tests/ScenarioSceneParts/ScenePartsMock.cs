using ScenarioSceneParts;
using SceneContents;

namespace Tests.ScenarioSceneParts
{
    public class ScenePartsMock : IScenarioSceneParts
    {
        public int ExecutionCounter { get; private set; }

        public Scenario Scenario { get; private set; }

        public int ExecutionEveryFrameCounter { get; private set; }

        public bool NeedExecuteEveryFrame => false;

        public ExecutionPriority Priority { get; set; } = ExecutionPriority.Low;

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
            Scenario = scenario;
        }

        public void SetResource(Resource resource)
        {
        }

        public void Reload(Resource resource)
        {
        }
    }
}