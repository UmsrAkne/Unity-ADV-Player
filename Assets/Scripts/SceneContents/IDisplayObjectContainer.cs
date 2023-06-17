using System.Collections.Generic;

namespace SceneContents
{
    public interface IDisplayObjectContainer
    {
        IDisplayObject FrontChild { get; }

        int Capacity { get; set; }

        List<IDisplayObject> Children { get; }

        int AddedChildCount { get; set; }

        event ImageContainer.ImageAddedEventHandler Added;

        void AddChild(IDisplayObject childObject);
    }
}