using System.Collections.Generic;
using System.Linq;
using SceneContents;

namespace Tests.ScenarioSceneParts
{
    public class DisplayObjectContainerMock : IDisplayObjectContainer
    {
        public IDisplayObject FrontChild => Children.LastOrDefault();

        public int Capacity { get; set; }

        public List<IDisplayObject> Children { get; } = new ();

        public int AddedChildCount { get; set; }

        public event ImageContainer.ImageAddedEventHandler Added;

        public void AddChild(IDisplayObject childObject, ImageOrder order)
        {
            AddedChildCount++;
            Children.Add(childObject);
            
            Added?.Invoke(this, new ImageAddedEventArgs()
            {
                CurrentImageSet = childObject,
                CurrentOrder = order,
            });
        }
    }
}