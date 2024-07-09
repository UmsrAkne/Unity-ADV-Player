using UnityEngine;

namespace SceneContents
{
    public class DisplayObject : IDisplayObject
    {
        private SpriteRenderer spriteRenderer;
        private float x;
        private float y;

        public DisplayObject(GameObject gameObject)
        {
            GameObject = gameObject;
        }

        public GameObject GameObject { get; set; }

        public float Alpha { get; set; }

        public double Scale { get; set; }

        public float X
        {
            get
            {
                if (spriteRenderer != null)
                {
                    return spriteRenderer.transform.position.x;
                }

                spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
                return spriteRenderer.transform.position.x;
            }
            set
            {
                Transform t;
                Vector3 p;

                if (spriteRenderer != null)
                {
                    t = spriteRenderer.transform;
                    p = t.position;
                    t.position = new Vector2(value, p.y);
                    return;
                }

                spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
                t = spriteRenderer.transform;
                p = t.position;
                t.position = new Vector2(value, p.y);
            }
        }

        public float Y
        {
            get
            {
                if (spriteRenderer != null)
                {
                    return spriteRenderer.transform.position.y;
                }

                spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
                return spriteRenderer.transform.position.y;
            }
            set
            {
                Transform t;
                Vector3 p;

                if (spriteRenderer != null)
                {
                    t = spriteRenderer.transform;
                    p = t.position;
                    t.position = new Vector2(p.x, value);
                    return;
                }

                spriteRenderer = GameObject.GetComponent<SpriteRenderer>();
                t = spriteRenderer.transform;
                p = t.position;
                t.position = new Vector2(p.x, value);
            }
        }

        public int Angle { get; set; }

        public float Wx { get; }

        public float Wy { get; }

        public bool Overwriting { get; set; }

        public void Dispose()
        {
            GameObject = null;
        }

        public void SetParent(Transform transform)
        {
        }

        public void SetSortingOrder(int order)
        {
        }

        public void Overwrite(float drawingDepth)
        {
        }

        public void SetSprite(SpriteWrapper spw, int index, Color color)
        {
        }
    }
}