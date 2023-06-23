using System.Collections.Generic;
using SceneContents;
using UnityEngine;

namespace ScenarioSceneParts
{
    public class ImageDrawer : IScenarioSceneParts
    {
        private double drawingDepth = 0.1;
        private IDisplayObject drawingImageSet;
        private Resource resource;
        private Scenario scenario;

        public List<IDisplayObjectContainer> ImageContainers { get; set; }

        public bool NeedExecuteEveryFrame => true;

        public ExecutionPriority Priority { get; } = ExecutionPriority.Middle;

        public void Execute()
        {
            if (scenario.ImageOrders.Count == 0 && scenario.DrawOrders.Count == 0)
            {
                return;
            }

            foreach (var order in scenario.ImageOrders)
            {
                AddBaseImage(order);
            }

            foreach (var order in scenario.DrawOrders)
            {
                DrawImage(order);
            }
        }

        public void ExecuteEveryFrame()
        {
            if (drawingImageSet == null)
            {
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

        public void SetScenario(Scenario scn)
        {
            scenario = scn;
        }

        public void DrawImage(ImageOrder order)
        {
            var targetContainer = ImageContainers[order.TargetLayerIndex];
            var frontImageSet = targetContainer.FrontChild;
            drawingImageSet = frontImageSet;
            drawingDepth = order.Depth;

            for (var i = 0; i < order.Names.Count; i++)
            {
                var name = order.Names[i];
                if (string.IsNullOrEmpty(name))
                {
                    continue;
                }

                var sw = new SpriteWrapper { Sprite = resource.ImagesByName[name].Sprite };
                frontImageSet.SetSprite(sw, i, new Color(1.0f, 1.0f, 1.0f, 0));
            }
        }

        public void AddBaseImage(ImageOrder order)
        {
            // Canvas の子である ImageContainer に、空のゲームオブジェクトを乗せる。
            var targetContainer = ImageContainers[order.TargetLayerIndex];
            var imageSet = new ImageSet();

            var spriteWrappers = new List<SpriteWrapper>();
            order.Names.ForEach(name =>
            {
                spriteWrappers.Add(!string.IsNullOrEmpty(name) ? resource.ImagesByName[name] : null);
            });

            // InheritStatus が指定されている場合は、最前面の画像の状態をコピーする
            if (order.InheritStatus)
            {
                var f = targetContainer.FrontChild;
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
            targetContainer.AddChild(imageSet);

            imageSet.Draw(spriteWrappers);

            if (!string.IsNullOrWhiteSpace(order.MaskImageName))
            {
                // imageSet.SetMask(resource.MaskImagesByName[order.MaskImageName].Sprite);
            }
        }

        // public void SetUI(UI ui)
        // {
        //     ImageContainers = ui.ImageContainers;
        // }
    }
}