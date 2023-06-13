using System.Collections.Generic;
using System.Linq;
using SceneContents;

namespace ScenarioSceneParts
{
    public class ScenePartsRunner
    {
        public List<IScenarioSceneParts> ScenePartsList { get; private set; } = new();

        public void Run(Scenario scenario)
        {
            ScenePartsList.ForEach(s => s.SetScenario(scenario));
            ScenePartsList.ForEach(s => s.Execute());
        }

        public void RunEveryFrame()
        {
            ScenePartsList.ForEach(s => s.ExecuteEveryFrame());
        }

        /// <summary>
        /// ScenarioSceneParts を内部のリストに加えます。
        /// このメソッドの実行毎に内部のリストは IScenarioSceneParts.Priority の順番に沿ってソートされます。
        /// 並び替えは優先順位が高いほどインデックスが小さくなります。
        /// </summary>
        /// <param name="sceneParts"></param>
        public void Add(IScenarioSceneParts sceneParts)
        {
            ScenePartsList.Add(sceneParts);
            ScenePartsList = ScenePartsList.OrderByDescending(s => (int)s.Priority).ToList();
        }
    }
}