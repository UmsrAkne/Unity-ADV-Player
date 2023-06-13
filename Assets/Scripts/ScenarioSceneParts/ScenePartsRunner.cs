using System.Collections.Generic;
using System.Linq;

namespace ScenarioSceneParts
{
    public class ScenePartsRunner
    {
        private List<IScenarioSceneParts> ScenePartsList { get; set; } = new List<IScenarioSceneParts>();

        public void Run()
        {
        }

        public void RunExecuteEveryFrame()
        {
        }

        public void Add(IScenarioSceneParts sceneParts)
        {
            ScenePartsList.Add(sceneParts);
            ScenePartsList = ScenePartsList.OrderByDescending(s => (int)s.Priority).ToList();
        }
    }
}