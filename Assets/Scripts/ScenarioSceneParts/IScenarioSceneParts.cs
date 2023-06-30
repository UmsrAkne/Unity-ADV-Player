using SceneContents;

namespace ScenarioSceneParts
{
    public interface IScenarioSceneParts
    {
        bool NeedExecuteEveryFrame { get; }

        ExecutionPriority Priority { get; }

        void Execute();

        void ExecuteEveryFrame();

        void SetScenario(Scenario scenario);

        void SetResource(Resource resource);

        /// <summary>
        /// SceneParts にリソースを再度セットし、新しいリソースに応じて動作できるよう状態を整えます。
        /// </summary>
        /// <param name="resource">新しくセットするリソース。既にセットされているリソースと同じものも入力可。</param>
        void Reload(Resource resource);

        // void SetUI(UI ui);
    }
}