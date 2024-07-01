using SceneContents;
using UnityEngine;
using UserInterface;

namespace ScenarioSceneParts
{
    public class MsgWindowMover : IScenarioSceneParts
    {
        private readonly int duration = 50;
        private readonly int distance = 540;
        private readonly Vector2 defaultPos = new Vector2(0, -270);
        private int direction = 1;
        private int executionCount;
        private bool isEnabled;
        private float lastMoveDistance;

        public bool NeedExecuteEveryFrame => true;

        public ExecutionPriority Priority => ExecutionPriority.Low;

        public UiContainer UiContainer { get; set; }

        public void Execute()
        {
        }

        public void ExecuteEveryFrame()
        {
            if (!isEnabled)
            {
                return;
            }

            var t = EaseInOutQuad((float)executionCount / duration);
            var d = Vector2.Lerp(new Vector2(0,0), new Vector2(0,distance * direction), t);

            UiContainer.SetMessageWindowPos(0, d.y + (defaultPos.y * direction));

            if (executionCount >= duration)
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

        private float EaseInOutQuad(float t)
        {
            return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
        }
    }
}