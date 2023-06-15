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

        void Dispose();

        void SetParent(Transform transform);

        void SetSortingOrder(int order);
    }
}