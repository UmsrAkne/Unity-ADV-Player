using SceneContents;

namespace ScenarioSceneParts
{
    public interface IDrawer
    {
        IDisplayObjectContainer ImageContainer { set; }

        bool NeedExecuteEveryFrame { get; }

        ImageOrder LastOrder { get; }

        void Execute();

        void ExecuteEveryFrame();

        void SetResource(Resource res);

        void SetResource(IResource res);

        void Reload(Resource res);

        void SetScenario(Scenario scn);

        void DrawImage(ImageOrder order);
    }
}