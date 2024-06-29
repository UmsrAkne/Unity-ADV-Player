using SceneContents;
using UserInterface;

namespace ScenarioSceneParts
{
    public class MsgWindowMover : IScenarioSceneParts
    {
        private readonly int duration = 50;
        private int direction = 1;
        private int executionCount;
        private bool isEnabled;

        public bool NeedExecuteEveryFrame => true;

        public ExecutionPriority Priority => ExecutionPriority.Low;

        public UiContainer UiContainer { get; set; }

        public void Execute()
        {
            isEnabled = true;
        }

        public void ExecuteEveryFrame()
        {
            if (!isEnabled)
            {
                return;
            }

            UiContainer.MoveMessageWindow(0,4f * direction);

            if (executionCount == duration)
            {
                executionCount = 0;
                isEnabled = false;
                direction *= -1;
            }

            executionCount++;
        }

        public void SetScenario(Scenario scenario)
        {
            if (scenario.MoveMessageWindow)
            {
                isEnabled = scenario.MoveMessageWindow;
            }
        }

        public void SetResource(Resource resource)
        {
        }

        public void Reload(Resource resource)
        {
        }
    }
}