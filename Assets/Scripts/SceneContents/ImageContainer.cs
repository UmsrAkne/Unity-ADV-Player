using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace SceneContents
{
    public class ImageContainer : IDisplayObjectContainer
    {
        public delegate void ImageAddedEventHandler(object sender, ImageAddedEventArgs e);

        private GameObject gameObject;
        private GameObject maskGameObject = new GameObject();

        private readonly SortingGroup sortingGroup;
        private readonly SpriteMask spriteMask;
        private readonly SpriteRenderer maskFrameSpriteRenderer;

        public ImageContainer()
        {
        }

        /// <summary>
        /// このコンテナのマスクが有効かどうかを取得します。
        /// SpriteMask に画像が設定されている場合は true を返します。
        /// </summary>
        public bool IsMaskEnabled => spriteMask.sprite != null;

        /// <summary>
        /// 生成と同時に、このオブジェクトがもつゲームオブジェクトの親として、引数のゲームオブジェクトを設定します。
        /// </summary>
        /// <param name="parentObject">このオブジェクトの親に設定されるオブジェクト</param>
        public ImageContainer(GameObject parentObject)
        {
            GameObject = new GameObject("ImageContainerChild");
            sortingGroup = GameObject.AddComponent<SortingGroup>();
            sortingGroup.sortingLayerName = "ImageLayer";

            GameObject.transform.SetParent(parentObject.transform);

            maskGameObject.transform.SetParent(GameObject.transform);
            spriteMask = maskGameObject.AddComponent<SpriteMask>();
            maskFrameSpriteRenderer = maskGameObject.AddComponent<SpriteRenderer>();
        }

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

        public int Index
        {
            get => sortingGroup.sortingOrder;
            set
            {
                if (maskGameObject != null)
                {
                    maskGameObject.name = $"MaskObject-{value}";
                }

                sortingGroup.sortingOrder = value;
            }
        }

        public int Capacity { get; set; } = 4;

        public List<IDisplayObject> Children { get; } = new();

        public int AddedChildCount { get; set; }

        public event ImageAddedEventHandler Added;

        public void AddChild(IDisplayObject childObject, ImageOrder order)
        {
            // childObject.GameObject.name = "children";
            childObject.SetParent(GameObject.transform);
            Children.Insert(0, childObject);

            var e = new ImageAddedEventArgs
            {
                CurrentImageSet = childObject,
                CurrentOrder = order,
            };

            Added?.Invoke(this, e);

            while (Children.Count > Capacity)
            {
                Children.LastOrDefault()?.Dispose();
                Children.RemoveAt(Children.Count - 1);
            }

            childObject.SetSortingOrder(AddedChildCount++);

            if (IsMaskEnabled)
            {
                ((ImageSet)childObject).ChangeMaskInteractions(SpriteMaskInteraction.VisibleInsideMask);
            }
        }

        /// <summary>
        /// このコンテナ全体に対してマスクを適用します。
        /// </summary>
        /// <param name="maskImage">マスクの画像のスプライト</param>
        /// <param name="maskFrameImage">マスクした画像の上に表示される画像です。マスクの縁部分を隠すための線の画像をセットします。</param>
        public void SetMask(SpriteWrapper maskImage, SpriteWrapper maskFrameImage)
        {
            spriteMask.sprite = maskImage.Sprite;
            maskFrameSpriteRenderer.sprite = maskFrameImage.Sprite;

            if (spriteMask.sprite != null)
            {
                foreach (var displayObject in Children)
                {
                    ((ImageSet)displayObject).ChangeMaskInteractions(SpriteMaskInteraction.VisibleInsideMask);
                }
            }
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