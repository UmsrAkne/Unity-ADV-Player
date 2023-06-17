using UnityEngine;

namespace SceneContents
{
    public interface IDisplayObject
    {
        float Alpha { get; set; }

        double Scale { get; set; }

        float X { get; set; }

        float Y { get; set; }

        int Angle { get; set; }

        float Wx { get; }

        float Wy { get; }

        bool Overwriting { get; set; }

        void Dispose();

        void SetParent(Transform transform);

        void SetSortingOrder(int order);

        void Overwrite(float drawingDepth);

        void SetSprite(SpriteWrapper spw, int index, Color color);
    }
}