using UnityEngine;
using UnityEngine.Rendering;

namespace SceneContents
{
    public class ImageUnit
    {
        private GameObject gameObject;
        private SortingGroup sortingGroup;
        private SpriteRenderer spriteRenderer;

        public ImageUnit()
        {
        }

        public ImageUnit(SpriteWrapper spw)
        {
            Width = spw.Width;
            Height = spw.Height;
            SpriteRenderer.sprite = spw.Sprite;
        }

        public SpriteRenderer SpriteRenderer
        {
            get
            {
                spriteRenderer ??= GameObject.AddComponent<SpriteRenderer>();
                return spriteRenderer;
            }
        }

        public SortingGroup SortingGroup
        {
            get
            {
                sortingGroup ??= GameObject.AddComponent<SortingGroup>();
                return sortingGroup;
            }
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public GameObject GameObject
        {
            get
            {
                gameObject = gameObject ? gameObject : new GameObject("ImageUnit-Container");
                return gameObject;
            }
        }

        public void SetMaskSprite(Sprite sprite)
        {
            var spriteMask = GameObject.GetComponent<SpriteMask>();
            if (spriteMask == null)
            {
                spriteMask = GameObject.AddComponent<SpriteMask>();
            }

            spriteMask.sprite = sprite;
        }

        public void SetParent(GameObject parent)
        {
            GameObject.transform.SetParent(parent.transform, false);
        }
    }
}