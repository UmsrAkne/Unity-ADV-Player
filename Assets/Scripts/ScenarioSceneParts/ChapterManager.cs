using System.Collections.Generic;
using System.Linq;
using SceneContents;

namespace ScenarioSceneParts
{
    public class ChapterManager : IScenarioSceneParts
    {
        private int currentIndex;
        private List<Scenario> scenarios;

        public bool NeedExecuteEveryFrame => false;

        public ExecutionPriority Priority => ExecutionPriority.Low;

        public void Execute()
        {
        }

        public void ExecuteEveryFrame()
        {
        }

        public void SetResource(Resource resource)
        {
            scenarios = resource.Scenarios;
        }

        public void SetScenario(Scenario scenario)
        {
            currentIndex = scenario.Index;
        }

        public int GetNextChapterIndex()
        {
            if (scenarios == null || scenarios.Count() <= currentIndex)
            {
                return currentIndex;
            }

            var nextChapterScenario = scenarios.Skip(currentIndex + 1)
                .FirstOrDefault(scenario => scenario.ChapterName != string.Empty);

            if (nextChapterScenario != null)
            {
                currentIndex = scenarios.IndexOf(nextChapterScenario);
                return scenarios.IndexOf(nextChapterScenario);
            }
            else
            {
                return currentIndex;
            }
        }

        public int GetLastChapterIndex()
        {
            if (scenarios == null || scenarios.Count() <= currentIndex)
            {
                return currentIndex;
            }

            var lastChapterScenario = scenarios.Skip(currentIndex + 1)
                .LastOrDefault(scenario => scenario.ChapterName != string.Empty);

            if (lastChapterScenario != null)
            {
                currentIndex = scenarios.IndexOf(lastChapterScenario);
                return scenarios.IndexOf(lastChapterScenario);
            }
            else
            {
                return currentIndex;
            }
        }
    }
}