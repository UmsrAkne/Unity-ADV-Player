using System.Collections.Generic;
using System.Linq;
using Loaders;
using SceneContents;
using UnityEngine;

namespace ScenarioSceneParts
{
    public class ImageDrawer : IScenarioSceneParts
    {
        private double drawingDepth = 0.1;
        private IDisplayObject drawingImageSet;
        private IResource resource;
        private Scenario scenario;
        private int addedImageCounter;
        private int drawingDelayCounter;
        private IDisplayObjectContainer imageContainer;
        private int targetLayerIndex;

        public bool NeedExecuteEveryFrame => true;

        public ExecutionPriority Priority { get; } = ExecutionPriority.Middle;

        /// <summary>
        ///     最後の描画に使用した order を保持しています。
        ///     このプロパティは、 Execute() で order を読み込んで実行した場合だけ記録されます。
        /// </summary>
        public ImageOrder LastOrder { get; private set; }

        public IDisplayObjectContainer ImageContainer
        {
            private get => imageContainer;
            set
            {
                imageContainer = value;
                if (imageContainer is ImageContainer container)
                {
                    targetLayerIndex = container.Index;
                }
            }
        }

        public void Execute()
        {
            if (scenario.ImageOrders.Count == 0 && scenario.DrawOrders.Count == 0)
            {
                return;
            }

            if (ImageContainer == null)
            {
                return;
            }

            foreach (var order in scenario.ImageOrders.Where(o => o.TargetLayerIndex == targetLayerIndex))
            {
                AddBaseImage(order);
                if (!order.IsExpressionOrder)
                {
                    LastOrder = order;
                }
            }

            foreach (var order in scenario.DrawOrders.Where(o => o.TargetLayerIndex == targetLayerIndex))
            {
                DrawImage(order);
                if (!order.IsExpressionOrder)
                {
                    LastOrder = order;
                }
            }
        }

        public void ExecuteEveryFrame()
        {
            if (drawingImageSet == null)
            {
                return;
            }

            if (drawingDelayCounter > 0)
            {
                drawingDelayCounter--;
                return;
            }

            drawingImageSet.Overwrite((float)drawingDepth);

            if (!drawingImageSet.Overwriting)
            {
                drawingImageSet = null;
            }
        }

        public void SetResource(Resource res)
        {
            resource = res;
        }

        public void SetResource(IResource res)
        {
            resource = res;
        }

        public void Reload(Resource res)
        {
            drawingImageSet = null;
            scenario = null;
            resource = res;
        }

        public void SetScenario(Scenario scn)
        {
            scenario = scn;
        }

        public void DrawImage(ImageOrder order)
        {
            if (imageContainer.Children.Count == 0)
            {
                var imageSet = new ImageSet();
                imageContainer.AddChild(imageSet, new ImageOrder());
            }

            var frontImageSet = imageContainer.FrontChild;

            drawingImageSet = frontImageSet;
            drawingDepth = order.Depth;
            drawingDelayCounter = order.Delay;

            var spriteWrappers = new List<SpriteWrapper>();
            foreach (var name in order.Names)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    spriteWrappers.Add(null);
                }
                else
                {
                    var sp = resource.GetImage(TargetImageType.EventCg, name);
                    sp.ImageLocation = resource.GetImageLocationFromName(name);
                    spriteWrappers.Add(sp);
                }
            }

            for (var i = 0; i < spriteWrappers.Count; i++)
            {
                if (spriteWrappers[i] == null)
                {
                    continue;
                }

                frontImageSet.SetSprite(spriteWrappers[i], i, new Color(1.0f, 1.0f, 1.0f, 0));
            }
        }

        private void AddBaseImage(ImageOrder order)
        {
            // Canvas の子である ImageContainer に、空のゲームオブジェクトを乗せる。
            var imageSet = new ImageSet();

            var spriteWrappers = new List<SpriteWrapper>();
            order.Names.ForEach(name =>
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    spriteWrappers.Add(null);
                }
                else
                {
                    var sp = resource.GetImage(TargetImageType.EventCg, name);
                    sp.ImageLocation = resource.GetImageLocationFromName(name);
                    spriteWrappers.Add(sp);
                }
            });

            // InheritStatus が指定されている場合は、最前面の画像の状態をコピーする
            if (order.InheritStatus)
            {
                var f = imageContainer.FrontChild;
                if (f != null)
                {
                    order.Scale = f.Scale;
                    order.X = (int)f.Wx;
                    order.Y = (int)f.Wy;
                    order.Angle = order.Angle;
                }
            }

            imageSet.Alpha = 0;
            imageSet.Scale = order.Scale;
            imageSet.X = order.X;
            imageSet.Y = order.Y;
            imageSet.Angle = order.Angle;
            imageSet.SortingLayerIndex = order.TargetLayerIndex;
            imageSet.SetSortingOrder(addedImageCounter++);
            imageContainer.AddChild(imageSet, order);

            imageSet.Draw(spriteWrappers);

            if (!string.IsNullOrWhiteSpace(order.MaskImageName))
            {
                // imageSet.SetMask(resource.MaskImagesByName[order.MaskImageName].Sprite);
            }
        }
    }
}