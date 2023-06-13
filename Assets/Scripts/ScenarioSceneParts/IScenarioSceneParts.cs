namespace ScenarioSceneParts
{
    public interface IScenarioSceneParts
    {
        bool NeedExecuteEveryFrame { get; }

        ExecutionPriority Priority { get; }

        void Execute();

        void ExecuteEveryFrame();

        // void SetScenario(Scenario scenario);
        //
        // void SetResource(Resource resource);
        //
        // void SetUI(UI ui);
    }
}