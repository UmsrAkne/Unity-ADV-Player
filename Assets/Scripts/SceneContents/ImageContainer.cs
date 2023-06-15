using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SceneContents
{
    public class ImageContainer : IDisplayObjectContainer
    {
        public delegate void ImageAddedEventHandler(object sender, ImageAddedEventArgs e);

        private GameObject gameObject;

        public GameObject GameObject
        {
            get => gameObject;
            set
            {
                if (gameObject == null)
                {
                    gameObject = value;
                }
            }
        }

        public IDisplayObject FrontChild => Children.FirstOrDefault();

        // public GameObject EffectGameObject { get; }

        // public ImageSet EffectImageSet { get; private set; }

        public int Index { get; set; }

        public int Capacity { get; set; } = 4;

        public List<IDisplayObject> Children { get; } = new();

        public int AddedChildCount { get; set; }

        public event ImageAddedEventHandler Added;

        public void AddChild(IDisplayObject childObject)
        {
            // childObject.GameObject.name = "children";
            childObject.SetParent(GameObject.transform);
            Children.Insert(0, childObject);

            var e = new ImageAddedEventArgs
            {
                CurrentImageSet = childObject
            };

            Added?.Invoke(this, e);

            while (Children.Count > Capacity)
            {
                Children.LastOrDefault()?.Dispose();
                Children.RemoveAt(Children.Count - 1);
            }

            childObject.SetSortingOrder(AddedChildCount++);
        }

        // public void AddEffectLayer()
        // {
        //     if (EffectGameObject == null)
        //     {
        //         var imageSet = new ImageSet();
        //         effectGameObject = imageSet.GameObject;
        //         EffectGameObject.name = "EffectGameObject";

        //         var loader = new ImageLoader();
        //         var sp = loader.LoadImage(@"commonResource\uis\fillWhite.png");
        //         imageSet.Alpha = 0;

        //         EffectGameObject.transform.SetParent(GameObject.transform);

        //         imageSet.Draw(new List<SpriteWrapper>() { sp });
        //         var sortingGroup = EffectGameObject.GetComponent<SortingGroup>();
        //         sortingGroup.sortingOrder = 10000;

        //         EffectImageSet = imageSet;
        //     }
        // }
    }
}