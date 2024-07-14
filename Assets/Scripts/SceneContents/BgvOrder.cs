using System.Collections.Generic;

namespace SceneContents
{
    public class BgvOrder
    {
        public List<string> FileNames { get; set; } = new List<string>();

        public int Channel { get; set; }

        public bool IsStopOrder { get; set; }

        public float PanStereo { get; set; }
    }
}