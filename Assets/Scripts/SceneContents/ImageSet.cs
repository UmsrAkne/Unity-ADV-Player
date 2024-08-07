using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace SceneContents
{
    public class ImageSet : IDisplayObject
    {
        private float alpha = 1.0f;
        private int angle;
        private ImageUnit maskUnit;
        private int overwriteLayerIndex = 1;
        private float scale = 1.0f;

        /// <summary>
        ///     ImageSet に追加される ImageUnit のまとめるための親ユニット
        ///     この Unit 自体に画像の情報は入らないが、描画順やマスクなど、描画に関する内容を管理するのに必須。
        /// </summary>
        private readonly ImageUnit parentUnit = new ();

        /// <summary>
        ///     現在の X座標(ワールド座標) を取得します。
        ///     ローカル座標は setParent の時点で値がずれるため、現在の画像のポジションを外側で利用する場合は、このプロパティを利用します。
        /// </summary>
        public float Wx
            => GameObject.transform.parent.TransformPoint(GameObject.transform.localPosition).x;

        /// <summary>
        ///     現在の Y座標(ワールド座標) を取得します。
        ///     ローカル座標は setParent の時点で値がずれるため、現在の画像のポジションを外側で利用する場合は、このプロパティを利用します。
        /// </summary>
        public float Wy
            => GameObject.transform.parent.TransformPoint(GameObject.transform.localPosition).y;

        public int SortingLayerIndex { get; set; }

        public SortingGroup SortingGroup => parentUnit.SortingGroup;

        public GameObject GameObject => parentUnit.GameObject;

        private GameObject MaskObject => maskUnit.GameObject;

        public bool Overwriting { get; set; }

        private List<ImageUnit> ImageUnits { get; set; } = new (4) { null, null, null, null, };

        private List<ImageUnit> TemporaryImages { get; set; } = new (4) { null, null, null, null, };

        public float Alpha
        {
            get => alpha;
            set
            {
                ImageUnits.ForEach(u =>
                {
                    if (u != null)
                    {
                        u.SpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, value);
                    }
                });

                alpha = value;
            }
        }

        public double Scale
        {
            get => scale;
            set
            {
                GameObject.transform.localScale = new Vector3((float)value, (float)value, 0);
                scale = (float)value;
            }
        }

        public float X
        {
            get => GameObject.transform.localPosition.x;
            set => GameObject.transform.localPosition = new Vector3(value, GameObject.transform.localPosition.y);
        }

        public float Y
        {
            get => GameObject.transform.localPosition.y;
            set => GameObject.transform.localPosition = new Vector3(GameObject.transform.localPosition.x, value);
        }

        public int Angle
        {
            get => angle;
            set
            {
                GameObject.transform.localRotation = Quaternion.AngleAxis(value, Vector3.forward);
                angle = value;
            }
        }

        /// <summary>
        ///     この ImageSet が参照している GameObject を SetActive(false) に設定します。
        /// </summary>
        public void Dispose()
        {
            GameObject.SetActive(false);
            // MaskObject.SetActive(false);

            ImageUnits.ForEach(u =>
            {
                u?.GameObject.SetActive(false);
            });
        }

        public void SetParent(Transform transform)
        {
            GameObject.transform.SetParent(transform);
        }

        public void SetSortingOrder(int order)
        {
            SortingGroup.sortingOrder = order;
        }

        public void Draw(List<SpriteWrapper> spriteWrappers, string layerName = "ImageLayer")
        {
            parentUnit.SortingGroup.sortingLayerName = layerName;

            for (var i = 0; i < spriteWrappers.Count; i++)
            {
                var spw = spriteWrappers[i];

                if (spw == null)
                {
                    continue;
                }

                var imageUnit = new ImageUnit(spw);
                ImageUnits[i] = imageUnit;
                imageUnit.SetParent(GameObject);
                // imageUnit.SpriteRenderer.sprite = spw.Sprite;

                if (spw.ImageLocation != null)
                {
                    var transform = imageUnit.SpriteRenderer.transform;
                    var pos = transform.localPosition;
                    pos.x = spw.ImageLocation.X;
                    pos.y = spw.ImageLocation.Y;
                    transform.localPosition = pos;
                }

                if (i == 0)
                {
                    imageUnit.SpriteRenderer.sortingOrder = -1;
                    imageUnit.SpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                }
            }

            Alpha = alpha;
            Scale = scale;
        }

        public void SetSprite(SpriteWrapper spw, int index, Color color)
        {
            parentUnit.SortingGroup.sortingLayerName = "ImageLayer";

            Overwriting = true;

            if (TemporaryImages[index] != null)
            {
                ReplaceImage(TemporaryImages[index], index);
            }

            var imageUnit = new ImageUnit(spw);
            TemporaryImages[index] = imageUnit;
            imageUnit.SetParent(GameObject);

            if (spw.ImageLocation != null)
            {
                var transform = imageUnit.SpriteRenderer.transform;
                var pos = transform.localPosition;
                pos.x = spw.ImageLocation.X;
                pos.y = spw.ImageLocation.Y;
                transform.localPosition = pos;
            }

            // sprite の上書きを常に最前面に対して行う。
            imageUnit.SpriteRenderer.sortingOrder = ++overwriteLayerIndex;
            imageUnit.SpriteRenderer.color = color;
        }

        public void Overwrite(float depth)
        {
            if (!Overwriting)
            {
                return;
            }

            for (var i = 0; i < TemporaryImages.Count; i++)
            {
                var imageUnit = TemporaryImages[i];

                if (imageUnit == null)
                {
                    continue;
                }

                var a = imageUnit.SpriteRenderer.color.a + depth;
                imageUnit.SpriteRenderer.color = new Color(1, 1, 1, a);

                if (a >= 1)
                {
                    ReplaceImage(imageUnit, i);
                }
            }

            Overwriting = TemporaryImages.Any(i => i != null);
        }

        /// <summary>
        /// このオブジェクトに登録されている全てのイメージユニットに対して SpriteMaskInteraction を設定します。
        /// </summary>
        /// <param name="smi">設定する SpriteMaskInteraction 。</param>
        public void ChangeMaskInteractions(SpriteMaskInteraction smi)
        {
            foreach (var imageUnit in ImageUnits.Concat(TemporaryImages).Where(u => u != null))
            {
                imageUnit.SpriteRenderer.maskInteraction = smi;
            }
        }

        private void ReplaceImage(ImageUnit temporaryImageUnit, int index)
        {
            ImageUnits[index]?.GameObject.SetActive(false);
            ImageUnits[index] = temporaryImageUnit;
            temporaryImageUnit.SpriteRenderer.color = new Color(1, 1, 1, 1);
            TemporaryImages[index] = null;
        }
    }
}