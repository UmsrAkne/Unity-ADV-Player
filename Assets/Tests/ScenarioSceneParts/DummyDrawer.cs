using System;
using System.Collections.Generic;
using ScenarioSceneParts;
using SceneContents;

namespace Tests.ScenarioSceneParts
{
    public class DummyDrawer : IDrawer
    {
        public IDisplayObjectContainer ImageContainer { get; set; }

        public bool NeedExecuteEveryFrame => true;

        public ImageOrder LastOrder { get; set; }

        public List<ImageOrder> ImageOrderHistories { get; set; } = new ();

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void ExecuteEveryFrame()
        {
            throw new NotImplementedException();
        }

        public void SetResource(Resource res)
        {
            throw new NotImplementedException();
        }

        public void SetResource(IResource res)
        {
            throw new NotImplementedException();
        }

        public void Reload(Resource res)
        {
            throw new NotImplementedException();
        }

        public void SetScenario(Scenario scn)
        {
            throw new NotImplementedException();
        }

        public void DrawImage(ImageOrder order)
        {
            ImageOrderHistories.Add(order);
        }
    }
}