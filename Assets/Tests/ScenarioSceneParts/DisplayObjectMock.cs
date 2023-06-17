using SceneContents;
using UnityEngine;

namespace Tests.ScenarioSceneParts
{
    public class DisplayObjectMock : IDisplayObject
    {
        public float Alpha { get; set; }

        public double Scale { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public int Angle { get; set; }

        public float Wx { get; }

        public float Wy { get; }

        public bool Overwriting { get; set; }

        public void Dispose()
        {
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